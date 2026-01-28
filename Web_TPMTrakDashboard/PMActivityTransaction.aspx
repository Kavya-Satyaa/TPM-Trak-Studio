<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMActivityTransaction.aspx.cs" Inherits="Web_TPMTrakDashboard.PMActivityTransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
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
            background-color:#202648;
        }
        .headerFixerhere tr:first-child th:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 6;
            background-color: #5391CA;
        }
        #lblPMStatus{
            font-size:16px;
        }
        .table-style tr td:nth-of-type(n+5) {
            text-align: center;
        }
    </style>
    <div>
        <asp:HiddenField runat="server" ID="hdnLoginDisplayStatus" ClientIDMode="Static" />
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
                        <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Width="140px" ClientIDMode="Static" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="btn btn-info" Text="View" Width="80" />
                    </td>
                </tr>
            </table>
            <div style="display: inline-block;float: right; margin-right: 7px;margin-top: 10px;">
                    <label style="background-color: red" class="lbls">Activity Pending</label>
                    <label style="background-color: #30ce30;" class="lbls">Completed</label>
                    <label style="background-color: #ffa302;" class="lbls">Under Plan</label>
            </div>
        </div>
        <div style="height: 80vh; width: 100%; overflow: auto">
            <asp:GridView ID="gvPMTransactionDetails" runat="server" AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found." ShowHeaderWhenEmpty="true" ShowHeader="true" ShowFooter="false" ClientIDMode="Static" CssClass="table-style headerFixerhere" OnRowDataBound="gvPMTransactionDetails_RowDataBound">
            </asp:GridView>
        </div>
        <div style="margin-top: 5px;">
            <div style="width: 80%; display: inline-block; text-align: center">
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
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="    float: right;"></i>
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
                    <asp:Button runat="server" ID="btnLoginConfirmation"  ClientIDMode="Static" Text="OK" CssClass="btn btn-info" OnClientClick="return btnLoginConfirmationClick();" />
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
                     <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="   float: right;" ></i>
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
                     <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style=" float: right" ></i>
                    <%--position: relative; top: -22px;--%>
                </div>
                <div class="modal-body">
                    <div style="height: 70vh">
                        <iframe id="iframeDocument" style="width: 100%; height: 100%" ></iframe>
                    </div>

                </div>
                <div class="modal-footer">

                    <input type="button" value="Close" class="btn btn-info" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $("#gvPMTransactionDetails tr td").click(function () {
            let tdValue = $(this).find('#lblPMStatus').text();
            selectedColIndex = $(this).index();
            selectedRowIndex = $(this).closest('tr').index();
            if (selectedColIndex == 1) {
                let hasFile = $(this).find("#hfHasFile").val();
                if (hasFile == "True") {
                    let activityid = $(this).find('#hdnActivityID').val();
                    let machineid= $("#ddlMachineId").val();
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: '<%= ResolveUrl("PMActivityTransaction.aspx/getFile") %>',
                        contentType: "application/json; charset=utf-8",
                        crossDomain: true,
                        data: '{activityid: "' + activityid + '",machineid:"' + machineid+'"}',
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

            if (tdValue.trim() == "P") {
                let tdindex = $("#gvPMTransactionDetails tr td").index($(this));
                $(".mandatory-message").text("");
                //$("#txtUserid").val("");
                //$("#txtPassword").val("");
                let td = $($("#gvPMTransactionDetails tr")[selectedRowIndex]).find('td')[selectedColIndex];
                if ($('#hdnLoginDisplayStatus').val() == "show") {
                    // $('#tblUserLoginInfo').css("display", "table");
                    // $("#loginConfirmationModal").modal('show');
                    $("#txtUserid").val("");
                    $("#txtPassword").val("");
                    $("#loginConfirmationModal").modal('show');
                    $("#txtUserid").focus();
                }
                else {
                    //$('#tblUserLoginInfo').css("display", "none");
                     btnLoginConfirmationClick();
                }
                //$("#loginConfirmationModal").modal('show');
                //$("#txtUserid").focus();
            }
            
        });
        function btnLoginConfirmationClick() {
            var isAuthorized = false;
            let userid = "";
            let password = "";
            if ($('#hdnLoginDisplayStatus').val() == "show") {
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
                    url: '<%= ResolveUrl("PMActivityTransaction.aspx/getActvityTransactionAuthorization") %>',
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
            }
            else {
                isAuthorized = true;
            }
            //if ($('#txtRemarks').val() == "") {
            //    $(".mandatory-message").text("Remarks required.");
            //    return false;
            //}
            //if ($('#txtTargetTime').val() == "") {
            //    $(".mandatory-message").text("Cycle Time required.");
            //    return false;
            //}
            if (isAuthorized) {
                $('#hdnLoginDisplayStatus').val("hide");
                let td = $($("#gvPMTransactionDetails tr")[selectedRowIndex]).find('td')[selectedColIndex];
                $(td).find('#lblPMStatus')[0].textContent = "C";
                $(td).find('#lblPMStatus').css("color", "#05f5dd");
                //$(td).find('.lblPMRemarks')[0].textContent = $('#txtRemarks').val();
                //$(td).find('.lblPMTargetTime')[0].textContent = $('#txtTargetTime').val();
                $("#loginConfirmationModal").modal('hide');
            }
            else {
                //$(".mandatory-message").text("Authorization Failed.");
                //$(".mandatory-message").css("color", "red");
                alert("Authorization Failed.");
            }
            return false;
        }
        function btnActivityTransactionSaveClick() {
            debugger;
            var tblRows = $("#gvPMTransactionDetails tr");
            var isFalied = false;
            let machineid = $("#ddlMachineId").val();
            for (var rowCount = 1; rowCount < tblRows.length; rowCount++) {
                var tblColumns = $(tblRows[rowCount]).find("td");
                let activity = $(tblRows[rowCount]).find("#hdnActivityID")[0].value;
                let frequency = $(tblRows[rowCount]).find("#lblFrequency")[0].textContent;

                for (var colCount = 4; colCount < tblColumns.length; colCount++) {
                    let status = $(tblColumns[colCount]).find('#lblPMStatus')[0].textContent;
                    //let PMRemarks = $(tblColumns[colCount]).find('.lblPMRemarks')[0].textContent;
                    //let PMTargetTime = $(tblColumns[colCount]).find('.lblPMTargetTime')[0].textContent;
                    if (status == "C") {
                        let header = $("#gvPMTransactionDetails tr th")[colCount].textContent;
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: '<%= ResolveUrl("PMActivityTransaction.aspx/saveActvityTransactionData") %>',
                            contentType: "application/json; charset=utf-8",
                            crossDomain: true,
                            data: '{activity: "' + activity + '",frequency: "' + frequency + '",header: "' + header + '",machineid: "' + machineid + '"}',
                            dataType: "json",
                            success: function (response) {
                                debugger;
                                var isUpdated = response.d;
                                if (isUpdated) {
                                    // showSuccessMsg('Record updated Successfully.', '');
                                    //-- $("#btnActivityTransactionSave").css("display", "none");
                                }
                                //else {
                                //isFalied = true;
                                //openErrorModal('Failed to update record.')
                                // $("#btnActivityTransactionSave").css("display", "inline-block");
                                //}
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
            return true;
        }
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
