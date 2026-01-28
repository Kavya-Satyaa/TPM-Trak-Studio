<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPIGate.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.MPIGate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        #tblSlnoData, #tblFileGeneratedSlno {
            background-color: white;
            color: black;
        }
        
        fieldset {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }


        legend {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
        }
    </style>
    <div style="font-size: 50px; color: white; text-align: center">
        Scan
                  <input type="text" autocomplete="off" id="txtSlnoSearch" data-toggle="tooltip" title="search !" placeholder="Serial Number..." class="form-control" style="width: 450px; display: inline; font-size: 48px; height: 60px; padding-bottom: 12px" onkeypress="scanSlNo(event)" onkeydown="manuallyEnterSlNo()">
        <i class="glyphicon glyphicon-refresh" style="margin-right: 3%; font-size: 35px;" onclick="refreshClick();"></i>
    </div>
    <div id="slnoDataDiv" style="margin-top: 20px">
        <table>
            <tr>
                <td>
                    <label style="color: white">Serial Number:</label></td>
                <td>
                    <asp:Label runat="server" ID="lblSlno" ClientIDMode="Static" Style="color: white"></asp:Label>
                </td>
                <td>&nbsp;
                    <label style="color: white">Component:</label></td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlComponent" class="form-control" ClientIDMode="Static"></asp:DropDownList>
                </td>
                <td>&nbsp; 
                    <label style="color: white">Heat Code:</label></td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlHeatCode" class="form-control" ClientIDMode="Static"></asp:DropDownList>
                </td>
                <td>&nbsp; 
                      <input value="Generate" type="button" class="btn btn-info" id="btnGenerate" style="display: inline; margin-left: 6px" onclick="generateClick();" />
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 20px;">
        <table id="tblSlnoData" class="table table-bordered  headerFixer">
            <tr>
                <th>Part Number</th>
                <th>Revision Number</th>
                <th>Part Name</th>
                <th>Supplier Code</th>
                <th>Serial Number</th>
                <th>Heat Code</th>
            </tr>
            <tr class="trSlnoData">
            </tr>
        </table>
    </div>
    <div style="margin-top: 50px;">
        <%-- <label style="color: white">Generated File Serial Number Details:</label>--%>
        <fieldset>
            <legend class="commontd" style="margin-bottom:5px;"> Today's Generated Serial Number Details:</legend>
            <div style="overflow: auto; height: 60vh;">
                <table id="tblFileGeneratedSlno" class="table table-bordered  headerFixer">
                    <tr>
                        <th>Part Number</th>
                        <th>Revision Number</th>
                        <th>Part Name</th>
                        <th>Supplier Code</th>
                        <th>Serial Number</th>
                        <th>Heat Code</th>
                        <th>Updated TS</th>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
    <div class="modal fade" id="slnoGenerateConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">This Serial Number already generate. Do you want to generate again?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" style="width: 80px;" onclick="clickGeneration();">Yes</button>
                    <button type="button" style="width: 80px;" data-dismiss="modal" onclick="cancelGeneration();">No</button>
                </div>
            </div>
        </div>
    </div>
      <div class="modal fade" id="partSlnoCSVConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black" id="confirmMsg">This Serial Number already generate. Do you want to generate again?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" style="width: 80px;" data-dismiss="modal" onclick="continueGeneration();">Yes</button>
                    <button type="button" style="width: 80px;" data-dismiss="modal" onclick="cancelGeneration();">No</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Warning!</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="lblWarningMsg" style="color: black"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        var PartNumber = "", RevisonNumber = "", PartName = "", SupplierCode = "", SerialNumber = "", HeatCode = "";
        $(document).ready(function () {
            ClearData();
            $('#txtSlnoSearch').focus();
            BindGeneratedSlnoData();
            $.unblockUI({});
        });
        function refreshClick() {
            ClearData();
            $('#txtSlnoSearch').val("");
        }
        function ClearData() {
            PartNumber = ""; RevisonNumber = ""; PartName = ""; SupplierCode = ""; SerialNumber = ""; HeatCode = "";
            $('#tblSlnoData tr.trSlnoData').empty();
            $('#slnoDataDiv').css("display", "none");
        }
        function cancelGeneration() {
            ClearData();
            $('#txtSlnoSearch').val("");
            $.unblockUI({});
        }
        function clickGeneration() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            generateCSVFile();
        }
        function continueGeneration() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            sendCSVFile();
            $.unblockUI({});
        }
        function BindGeneratedSlnoData() {
            $('#tblFileGeneratedSlno tr:not(:first-child)').empty();
            $.ajax({
                async: false,
                type: "POST",
                url: "MPIGate.aspx/getAllGeneratedSlnoDetails",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var list = response.d;
                    var appendStr = "";
                    for (var i = 0; i < list.length; i++) {
                        appendStr += '<tr><td>' + list[i].Component + '</td><td>' + list[i].RevisionNumber + '</td><td>' + list[i].CompDesc + '</td><td>' + list[i].SupplierCode + '</td><td>' + list[i].SerialNumber + '</td><td>' + list[i].HeatCode + '</td><td>' + list[i].UpdatedTS + '</td></tr>';
                    }
                    $('#tblFileGeneratedSlno').append(appendStr);
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
        }
        function generateCSVFile() {
            $.ajax({
                async: false,
                type: "POST",
                url: "MPIGate.aspx/serialNumberValidation",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{PartNumber:"' + PartNumber + '",RevNumber:"' + RevisonNumber + '",PartName:"' + PartName + '",SupplierCode:"' + SupplierCode + '",SerialNumber:"' + SerialNumber + '",HeatCode:"' + HeatCode + '"}',
                success: function (response) {
                    var validateResult = response.d;
                    if (validateResult == "1") {
                        var result = sendCSVFile();
                        if (result != "") {
                            openWarningModal(result);
                            return;
                        }
                    }
                    else if (validateResult == "insertionError") {
                        openWarningModal("Insertion failed.");
                    } else {
                        var confirmMsg = "";
                        if (validateResult == "2") {
                            confirmMsg = "This Operation is not allowed on this Machine."
                        }
                        else if (validateResult == "3") {
                            confirmMsg = "Slno already there."
                        }
                        else if (validateResult == "4") {
                            confirmMsg = "This Slno is Rejected."
                        }
                        else if (validateResult == "5") {
                            confirmMsg = "This Slno marked for Rework."
                        }
                        else if (validateResult == "6") {
                            confirmMsg = "Slno not exists in autodata."
                        }
                        else if (validateResult == "7") {
                            confirmMsg = "Not allowed on this machine."
                        }
                        else if (validateResult == "8") {
                            confirmMsg = "Out of sequence."
                        }
                        else if (validateResult == "9") {
                            confirmMsg = "Next operation not exist for that component."
                        }
                        else if (validateResult == "10") {
                            confirmMsg = "Component master not exist."
                        }
                        else if (validateResult == "11") {
                            confirmMsg = "SPC master not exists."
                        }
                        else if (validateResult == "12") {
                            confirmMsg = "SPC trancsaction not exist."
                        }
                        else if (validateResult == "13") {
                            confirmMsg = "Operation is in Bypass mode."
                        }
                        // openWarningModal("This Serial Number not allowed on this Machine.");
                        $('#confirmMsg').text(confirmMsg + " Do you want to generate?");
                        $('#partSlnoCSVConfirmModal').modal('show');
                        // $('#txtSlnoSearch').val("");
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
            $('#slnoGenerateConfirmModal').modal('hide');
            $.unblockUI({});
        }
        function sendCSVFile() {
            var result = "";
            $.ajax({
                async: false,
                type: "POST",
                url: "MPIGate.aspx/sendCSVFile",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{PartNumber:"' + PartNumber + '",RevNumber:"' + RevisonNumber + '",PartName:"' + PartName + '",SupplierCode:"' + SupplierCode + '",SerialNumber:"' + SerialNumber + '",HeatCode:"' + HeatCode + '"}',
                success: function (response) {
                    var generatedResult = response.d;
                    if (generatedResult == "fail") {
                        // $('#txtSlnoSearch').val("");
                        result = "Failed to generate CSV file.";
                        return result;
                    }
                    else if (generatedResult == "enterusername") {
                        // $('#txtSlnoSearch').val("");
                        result = "Please enter Username or Password.";
                        return result;
                    }
                    else {
                        openSuccessModal("File generated Successfuly.");
                        // $('#txtSlnoSearch').val("");
                        if ($('#slnoDataDiv').css("display") != "none") {
                            ClearData();
                        }
                        BindGeneratedSlnoData();
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
            return result;
        }
        function SlnoExistenceValidation() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            $('#txtSlnoSearch').val("");
            $('#tblSlnoData tr.trSlnoData').empty();
            var str = '<td>' + PartNumber + '</td><td>' + RevisonNumber + '</td><td>' + PartName + '</td><td>' + SupplierCode + '</td><td>' + SerialNumber + '</td><td>' + HeatCode + '</td>';
            $('#tblSlnoData tr.trSlnoData').append(str);
            $.ajax({
                async: false,
                type: "POST",
                url: "MPIGate.aspx/PartSlnoExistenceValidation",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{PartNumber:"' + PartNumber + '",RevNumber:"' + RevisonNumber + '",PartName:"' + PartName + '",SupplierCode:"' + SupplierCode + '",SerialNumber:"' + SerialNumber + '",HeatCode:"' + HeatCode + '"}',
                success: function (response) {
                    var isExist = response.d;
                    if (isExist) {
                        $('#slnoGenerateConfirmModal').modal('show');
                        $.unblockUI({});
                    }
                    else {
                        generateCSVFile();
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
        }
        function setSlnoDetails() {
            PartNumber = "";
            RevisonNumber = "";
            PartName = "";
            SupplierCode = "";
            HeatCode = "";
            $('#lblSlno').text(SerialNumber);
            BindComponent();
        }
        function BindComponent() {
            $.ajax({
                async: false,
                type: "POST",
                url: "MPIGate.aspx/getComponent",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{SerialNumber:"' + SerialNumber + '"}',
                success: function (response) {
                    var list = response.d;
                    $('#ddlComponent').empty();
                    if (list != null) {
                        var appendStr = "";
                        for (var i = 0; i < list.length; i++) {
                            appendStr += '<option>' + list[i] + '</option>';
                        }
                        $('#ddlComponent').append(appendStr);
                    }
                    BindHeatCode();
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
        }
        function BindHeatCode() {

            $.ajax({
                async: false,
                type: "POST",
                url: "MPIGate.aspx/getHeatCode",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{SerialNumber:"' + SerialNumber + '",Component:"' + $('#ddlComponent').val() + '"}',
                success: function (response) {
                    var list = response.d;
                    $('#ddlHeatCode').empty();
                    if (list != null) {
                        var appendStr = "";
                        for (var i = 0; i < list.length; i++) {
                            appendStr += '<option>' + list[i] + '</option>';
                        }
                        $('#ddlHeatCode').append(appendStr);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
            $.unblockUI({});
        }
        $('#ddlComponent').change(function () {
            BindHeatCode();
        });
        function generateClick() {
            $.ajax({
                async: false,
                type: "POST",
                url: "MPIGate.aspx/getSlnoDetails",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{SerialNumber:"' + SerialNumber + '",Component:"' + $('#ddlComponent').val() + '",HeatCode:"' + $('#ddlHeatCode').val() + '"}',
                success: function (response) {
                    var slnoData = response.d;
                    debugger;
                    if (slnoData != null) {
                        if (slnoData.Component != null) {
                            var splitData = slnoData.Component.split("-");
                            PartNumber = splitData[0];
                            RevisonNumber = splitData[1];
                        }
                        PartName = slnoData.CompDesc;
                        SupplierCode = slnoData.SupplierCode;
                        HeatCode = slnoData.HeatCode;
                    }
                    if (SerialNumber.trim() == "") {
                        openWarningModal("Please enter Serial Number.");
                        return;
                    }
                    if (PartNumber.trim() == "") {
                        openWarningModal("Please enter Part Number.");
                        return;
                    }
                    if (RevisonNumber.trim() == "") {
                        openWarningModal("Please enter Revison Number.");
                        return;
                    }
                    if (PartName.trim() == "") {
                        openWarningModal("Please enter Part Name.");
                        return;
                    }
                    if (SupplierCode.trim() == "") {
                        openWarningModal("Please enter Supplier Code.");
                        return;
                    }
                    if (HeatCode.trim() == "") {
                        openWarningModal("Please enter Heat Code.");
                        return;
                    }
                    SlnoExistenceValidation();
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
        }
        var inputval;
        var temptime;
        var enterCondition = 0;
        var completePNScan = true;
        function scanSlNo(e) {
            if (e.keyCode == 13) {
                debugger;
                completePNScan = true;
                if (enterCondition == 1) {
                    enterCondition = 0;
                    temptime = "";
                    $('#txtSlnoSearch').blur();
                    clearTimeout(inputval);
                    let scannedText = $('#txtSlnoSearch').val();
                    if (scannedText.trim() == "") {
                        openWarningModal("Please enter Serial Number");
                        e.preventDefault();
                        return false;
                    }
                    if (scannedText != "") {
                        if (scannedText.includes("-")) {
                            let splittedScannedText = [];
                            splittedScannedText = scannedText.split("-");
                            PartNumber = splittedScannedText[0];
                            RevisonNumber = splittedScannedText[1];
                            PartName = splittedScannedText[2];
                            SupplierCode = splittedScannedText[3];
                            SerialNumber = splittedScannedText[4];
                            HeatCode = splittedScannedText[5];
                            $('#slnoDataDiv').css("display", "none");
                        } else {
                            SerialNumber = scannedText;
                            $('#slnoDataDiv').css("display", "block");
                            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                            setSlnoDetails();
                            e.preventDefault();
                            $('#txtSlnoSearch').val("");

                            return false;
                        }
                    }
                    $('#txtSlnoSearch').val("");
                    if (SerialNumber.trim() == "") {
                        openWarningModal("Please enter Serial Number.");
                        e.preventDefault();
                        return false;
                    }
                    if (PartNumber.trim() == "") {
                        openWarningModal("Please enter Part Number.");
                        e.preventDefault();
                        return false;
                    }
                    if (RevisonNumber.trim() == "") {
                        openWarningModal("Please enter Revison Number.");
                        e.preventDefault();
                        return false;
                    }
                    if (PartName.trim() == "") {
                        openWarningModal("Please enter Part Name.");
                        e.preventDefault();
                        return false;
                    }
                    if (SupplierCode.trim() == "") {
                        openWarningModal("Please enter Supplier Code.");
                        e.preventDefault();
                        return false;
                    }
                    if (HeatCode.trim() == "") {
                        openWarningModal("Please enter Heat Code.");
                        e.preventDefault();
                        return false;
                    }
                    SlnoExistenceValidation();
                    e.preventDefault();
                }
                else {
                    $('#txtSlnoSearch').val("");
                    $('#txtSlnoSearch').blur();
                    clearTimeout(inputval);
                    e.preventDefault();
                    console.log("Enter 13");
                    return false;
                }
            }
            return true;
        }

        function manuallyEnterSlNo() {
            var timeout = 3000;
            var timeDiff;
            clearTimeout(inputval);
            var today = new Date();
            var time = today.getMilliseconds();

            if (temptime != undefined && temptime != "") {
                timeDiff = parseInt(temptime) - time;
                if (timeDiff < 0) {
                    timeDiff = timeDiff * -1;
                }
                if (timeDiff <= 10) {
                    timeout = 1000;
                }
                else {
                    timeout = 3000;
                    enterCondition = 1;
                }
            }
            temptime = time;
            ClearData();
            inputval = setTimeout(function () {
                let scannedText = $('#txtSlnoSearch').val();
                if (scannedText.trim() == "") {
                    openWarningModal("Please enter Serial Number.");
                    return;
                }
                $('#txtSlnoSearch').blur();
                if (scannedText != "") {
                    if (scannedText.includes("-")) {
                        //let splittedScannedText = [];
                        //splittedScannedText = scannedText.split("-");
                        //slno = splittedScannedText[splittedScannedText.length - 2];
                        //$('#txtSlnoSearch').val(slno);
                        let splittedScannedText = [];
                        splittedScannedText = scannedText.split("-");
                        PartNumber = splittedScannedText[0];
                        RevisonNumber = splittedScannedText[1];
                        PartName = splittedScannedText[2];
                        SupplierCode = splittedScannedText[3];
                        SerialNumber = splittedScannedText[4];
                        HeatCode = splittedScannedText[5];
                        //slno = splittedScannedText[splittedScannedText.length - 2];
                        // $('#txtSlnoSearch').val(slno);
                        $('#slnoDataDiv').css("display", "none");
                    } else {
                        SerialNumber = scannedText;
                        $('#slnoDataDiv').css("display", "block");
                        setSlnoDetails();
                        $('#txtSlnoSearch').val("");
                        $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                        return;
                    }
                }
                $('#txtSlnoSearch').val("");
                if (SerialNumber.trim() == "") {
                    openWarningModal("Please enter Serial Number");
                    return;
                }
                if (PartNumber.trim() == "") {
                    openWarningModal("Please enter Part Number.");
                    return;
                }
                if (RevisonNumber.trim() == "") {
                    openWarningModal("Please enter Revison Number.");
                    return;
                }
                if (PartName.trim() == "") {
                    openWarningModal("Please enter Part Name.");
                    return;
                }
                if (SupplierCode.trim() == "") {
                    openWarningModal("Please enter Supplier Code.");
                    return;
                }
                if (HeatCode.trim() == "") {
                    openWarningModal("Please enter Heat Code.");
                    return;
                }
                SlnoExistenceValidation();
                enterCondition = 0;
                temptime = "";
            }, timeout);
        }
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
            $.unblockUI({});
        }
        function openSuccessModal(msg) {
            $.unblockUI({});
            $('#toast-container').empty();
            Command: toastr["success"](msg)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
