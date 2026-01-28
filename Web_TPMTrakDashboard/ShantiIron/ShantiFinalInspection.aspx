<%@ Page Title="Final Inspection" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShantiFinalInspection.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.ShantiFinalInspection" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #FIDataContainer table tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #FIDataContainer table tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        input[type="checkbox"] {
            width: 20px;
            height: 20px;
        }
        .wrapper{
            margin-top:40px;
        }
        #tblFiInfo tr td{
            vertical-align:middle;
        }
    </style>
    <div class="row">
        <asp:HiddenField runat="server" ID="hdnFilenName" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnSlno" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCompID" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnPlantID" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCellID" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnSearchSlno" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnSearchDate" ClientIDMode="Static" />
        <div class="col-lg-12" style="text-align: left; display: block; margin-bottom: 10px;">
             <div style="font-size: 50px; color: white; text-align: center">Scan
                  <input type="text" autocomplete="off" id="txtSlnoSearch" data-toggle="tooltip" title="search !" placeholder="Serial Number..." class="form-control" style="width: 400px; display: inline;font-size: 48px; height: 60px; margin-right: 15%; padding-bottom: 12px" onkeypress="scanSlNo(event)" onkeydown="manuallyEnterSlNo()" >
             </div>
            <table style="margin-top:5px">
                <tr>
                    <td>
                        <label style="color: white">Plant ID</label>
                        <select id="ddlPlantID" class="form-control" style="display: inline-block; width: auto">
                        </select>&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <label style="color: white">Cell ID</label>
                        <select id="ddlCellID" class="form-control" style="display: inline-block; width: auto">
                        </select>&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="display:none">
                        <%--<input type="text" autocomplete="off" id="txtSlnoSearch" data-toggle="tooltip" title="search !" placeholder="Serial Number..." class="form-control" style="width: 250px; display: inline;" onkeypress="scanSlNo(event)" onkeydown="manuallyEnterSlNo()" >--%>
                        <input value="View" type="button" class="btn btn-info" id="btnSlnoView" style="display: none; margin-left: 6px" />&nbsp;&nbsp;&nbsp;
                    </td>
                    <%-- <td>
                        <input type="date" autocomplete="off" id="txtDateSearch" data-toggle="tooltip" class="form-control" style="width: 250px; display: inline;">
            <input value="View" type="button" class="btn btn-info" id="btnDateView" style="display: inline; margin-left: 6px" />
                    </td>--%>
                    <td class="input-group" style="min-width: 150px; border: 0">
                         <label style="color: white">Date&nbsp;&nbsp;</label>
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtDateSearch" runat="server" ClientIDMode="Static" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>

                    </td>
                    <td>
                        <input value="View" type="button" class="btn btn-info" id="btnDateView" style="display: inline; margin-left: 6px" />
                    </td>
                </tr>
            </table>

        </div>
    </div>
    <div id="FIDataContainer" style="overflow: auto; width: 100%; height: 40vh">
    </div>
    <div id="FISlnoDataContainer" style="overflow: auto; width: 100%; height: 37vh; margin-top: 10px">
    </div>
    <asp:Button runat="server" ID="btnExport" Visible="false" ClientIDMode="Static" OnClick="btnExport_Click" UseSubmitBehavior="false" />

    <div class="modal fade" id="ConfirmRejectModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to Reject this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" id="rejectYes" style="width: 80px;">Yes</button>
                    <button type="button" id="rejectNo" style="width: 80px;" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="ConfirmApproveModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to Approve this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" id="approveYes" style="width: 80px;">Yes</button>
                    <button type="button" id="approveNo" style="width: 80px;" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
     <div class="modal fade" id="ConfirmReworkModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to Reowrk this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" id="reworkYes" style="width: 80px;">Yes</button>
                    <button type="button" id="reworkNo" style="width: 80px;" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="loadingModal" role="dialog" style="min-width: 300px; margin-top: 15%" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog  modal-dialog-centered" style="width: 500px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <%--<div class="modal-header" style="background-color: #0c0922; padding: 8px">

                        <h4 class="modal-title" style="color: white;">Warning!</h4>
                    </div>--%>
                <div class="modal-body" style="text-align: center; padding: 20px">
                    <span style="color: black; font-size: 25px;">Please wait.....</span>
                </div>
                <%--<div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                        <button type="button" data-dismiss="modal" Style="width: 80px;" class="modalBtns">OK</button>
                    </div>--%>
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
    <asp:Button runat="server" ID="btnExportFile" Visible="false" ClientIDMode="Static" OnClick="btnExportFile_Click" UseSubmitBehavior="false" />
    <script>
        var rejectapprovebtn;
        var ComponentID, CompSerialNumber;
        var csvFileGenerationIntervalCount = 0;
        var bigDiv = document.getElementById('FIDataContainer');
        var slnoRefreshInterval;
        $(document).ready(function () {
            debugger;
            $('[id$=txtDateSearch]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            BindPlantData();
            //setCurrentDate();
            let currentDate = new Date();
            $("#txtDateSearch").val(currentDate.format("dd-MM-yyyy"));
            //$('#btnDateView').click();

            if ($('#hdnSearchSlno').val() != "" && $('#hdnSearchSlno').val() != undefined) {
                $('#txtSlnoSearch').val($('#hdnSearchSlno').val());
                $('#btnSlnoView').click();
            }
            else {
                if ($('#hdnSearchDate').val() != "" && $('#hdnSearchDate').val() != undefined) {
                    $('#txtDateSearch').val($('#hdnSearchDate').val());
                }
                $('#btnDateView').click();
            }
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            getScannedSlno();
        });
        function getScannedSlno() {
            slnoRefreshInterval = setInterval(function () {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "ShantiFinalInspection.aspx/getScannedSlnoFromTbl",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var itemData = response.d;
                        if (itemData != "") {
                            $('#txtSlnoSearch').val(itemData);
                            $('#btnSlnoView').click();
                            updateSlnoStatus(itemData);
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                    }
                });
            }, 3000);
        }
        function updateSlnoStatus(slnum) {
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/setSlnoStatus",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + slnum + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        var inputval;
        var temptime;
        var enterCondition = 0;
        var completePNScan = true;
        function scanSlNo(e) {
            if (e.keyCode == 13) {
                completePNScan = true;
                if (enterCondition == 1) {
                    enterCondition = 0;
                    temptime = "";

                    clearTimeout(inputval);
                    let SerialNumber = "";
                    let scannedText = $('#txtSlnoSearch').val();

                    if (scannedText != "") {
                        if (scannedText.includes("-")) {
                            let splittedScannedText = [];
                            splittedScannedText = scannedText.split("-");
                            SerialNumber = splittedScannedText[splittedScannedText.length - 2];
                            $('#txtSlnoSearch').val(SerialNumber);
                        } else {
                            SerialNumber = scannedText;
                        }
                    }
                    if (SerialNumber.trim() == "") {
                        return;
                    }
                    $('#btnSlnoView').click();
                    e.preventDefault();
                }
                else {
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

            inputval = setTimeout(function () {
                let SerialNumber = "";
                let scannedText = $('#txtSlnoSearch').val();

                if (scannedText != "") {
                    if (scannedText.includes("-")) {
                        let splittedScannedText = [];
                        splittedScannedText = scannedText.split("-");
                        SerialNumber = splittedScannedText[splittedScannedText.length - 2];
                        $('#txtSlnoSearch').val(SerialNumber);
                    } else {
                        SerialNumber = scannedText;
                    }
                }
                if (SerialNumber.trim() == "") {

                    return;
                }
                $('#btnSlnoView').click();
                enterCondition = 0;
                temptime = "";
            }, timeout);
        }

        function downloadFileandSetValueToHdnVar(lnk) {
            debugger;
            $('#hdnFilenName').val(lnk.text);
            __doPostBack('<%= btnExportFile.UniqueID%>', '');
        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
        }
        window.onload = function () {
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        function openConfirmRejectModal() {
            $('[id*=ConfirmRejectModal]').modal('show');
        }
        function openConfirmApproveModal() {
            $('[id*=ConfirmApproveModal]').modal('show');
        }
        function openConfirmReworkModal() {
            $('[id*=ConfirmReworkModal]').modal('show');
        }
        $('#rejectYes').click(function () {
            debugger;
            saveOrApproveFIData(rejectapprovebtn, "rejected");
            $('[id*=ConfirmRejectModal]').modal('hide');
        });
        $('#approveYes').click(function () {
            debugger;
            $('[id*=ConfirmApproveModal]').modal('hide');
            //  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            saveOrApproveFIData(rejectapprovebtn, "approve");

            // setTimeout($.unblockUI({}), 5000);

            //__doPostBack('<%= btnExport.UniqueID%>', '');
        });
        $('#reworkYes').click(function () {
            debugger;
            $('[id*=ConfirmReworkModal]').modal('hide');
            saveOrApproveFIData(rejectapprovebtn, "rework");
         });
        $('#btnSlnoView').click(function () {
            if ($('#ddlCellID').val() == "" || $('#ddlCellID').val() == null) {
                openWarningModal("Please select Cell ID.");
                return;
            }
            if ($('#txtSlnoSearch').val().trim() == "") {
                openWarningModal("Please enter Serial Number.");
                return;
            }
            $('#hdnPlantID').val($('#ddlPlantID').val());
            $('#hdnCellID').val($('#ddlCellID').val());
            $('#hdnSearchSlno').val($('#txtSlnoSearch').val().trim());
            $('#hdnSearchDate').val("");
            BindSlnoFIData();
        });
        $('#btnDateView').click(function () {
            if ($('#ddlCellID').val() == "" || $('#ddlCellID').val() == null) {
                openWarningModal("Please select Cell ID.");
                return;
            }
            $('#hdnPlantID').val($('#ddlPlantID').val());
            $('#hdnCellID').val($('#ddlCellID').val());
            $('#hdnSearchSlno').val("");
            $('#hdnSearchDate').val($('#txtDateSearch').val());
            BindDateFIData();
        });
        function BindSlnoFIData() {
            // setCurrentDate();
            clearInterval(csvFileGenerationInterval);
            let currentDate = new Date();
            $("#txtDateSearch").val(currentDate.format("dd-MM-yyyy"));
            var searchslno = $('#txtSlnoSearch').val();
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/getSlnoFIData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + searchslno + '",plant:"' + $('#ddlPlantID').val() + '",cell:"' + $('#ddlCellID').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;

                    debugger;
                    BindFIData(itemData.FICheckList);
                    BindFISlnoDetails(itemData.FISlnoDetails);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function BindDateFIData() {
            clearInterval(csvFileGenerationInterval);
            $('#txtSlnoSearch').val("");
            var searchdate = $('#txtDateSearch').val();
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/getDateFIData",
                contentType: "application/json; charset=utf-8",
                data: '{date:"' + searchdate + '",plant:"' + $('#ddlPlantID').val() + '",cell:"' + $('#ddlCellID').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    debugger;
                    BindFIData(itemData.FICheckList);
                    BindFISlnoDetails(itemData.FISlnoDetails);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function BindFIData(itemData) {
            debugger;
            $('#FIDataContainer').empty();
            var fistr = '<table id="tblFiInfo" class="table table-bordered table-hover headerFixer" style="width: 100%">';
            if (itemData.length > 0) {

                for (var i = 0; i < itemData.length; i++) {
                    if (i == 0) {
                        fistr += '<thead class="blue"><tr>';
                        for (var j = 0; j < itemData[i].FIData.length; j++) {
                            fistr += '<th>' + itemData[i].FIData[j].Parameter + '</th>';
                        }
                        fistr += '<th>Action</th>';
                        fistr += '</tr></thead>';
                    }
                    fistr += '<tr>';
                    for (var j = 0; j < itemData[i].FIData.length; j++) {
                        if (itemData[i].FIData[j].Type == "lbl" && itemData[i].FIData[j].ControlIDOrClass == "lblSlno") {
                            fistr += '<td><label id="' + itemData[i].FIData[j].ControlIDOrClass + '">' + itemData[i].FIData[j].Value + '</label><input type="hidden" id="lblCompID" value="' + itemData[i].FIData[j].ComponentID + '" /></td>';
                        } else if (itemData[i].FIData[j].Type == "lbl" && itemData[i].FIData[j].ControlIDOrClass == "lblComponentID") {
                            fistr += '<td><label>' + itemData[i].FIData[j].Value + '</label></td>';
                        } else
                            if (itemData[i].FIData[j].Type == "lbl") {
                                fistr += '<td><label id="' + itemData[i].FIData[j].ControlIDOrClass + '">' + itemData[i].FIData[j].Value + '</label></td>';
                            }
                            else if (itemData[i].FIData[j].Type == "txt") {
                                if (itemData[i].FIData[j].Approval == "true") {
                                    fistr += '<td><input  type="text" class="form-control" readonly="readonly" id="' + itemData[i].FIData[j].ControlIDOrClass + '"  value="' + itemData[i].FIData[j].Value + '" style="width: 150px;"/></td>';
                                }
                                else {
                                    fistr += '<td><input  type="text" class="form-control" id="' + itemData[i].FIData[j].ControlIDOrClass + '"  value="' + itemData[i].FIData[j].Value + '" style="width: 150px;"/></td>';
                                }
                            } else if (itemData[i].FIData[j].Type == "link") {
                                fistr += '<td><a id="' + itemData[i].FIData[j].ControlIDOrClass + '" onclick="downloadFileandSetValueToHdnVar(this);" >' + itemData[i].FIData[j].Value + '</a></td>';
                            }
                            else if (itemData[i].FIData[j].Type == "chkReadOnly") {
                                //if (itemData[i].FIData[j].Value == "Green") {
                                //    fistr += '<td><input type="checkbox" checked="checked" onclick="return false;" class="' + itemData[i].FIData[j].ControlIDOrClass + '" /></td>'
                                //}
                                //else {
                                //    fistr += '<td> <input type="checkbox"  onclick="return false;" class="' + itemData[i].FIData[j].ControlIDOrClass + '"/></td>'
                                //}
                                if (itemData[i].FIData[j].Value == "Green") {
                                    fistr += '<td style="text-align:center"><i class="glyphicon glyphicon-ok-circle ' + itemData[i].FIData[j].ControlIDOrClass + '" style="color:green;font-size:25px"></i></td>';
                                }
                                else if (itemData[i].FIData[j].Value == "Red") {
                                    fistr += '<td style="text-align:center">  <i class="glyphicon glyphicon-remove-circle ' + itemData[i].FIData[j].ControlIDOrClass + '" style="color:red;font-size:25px"></i></td>';
                                }
                                else if (itemData[i].FIData[j].Value == "Nan") {
                                    fistr += '<td style="text-align:center"> <i class="glyphicon glyphicon-remove-circle ' + itemData[i].FIData[j].ControlIDOrClass + '" style="color:red;font-size:25px"></i></br><span style="font-size:12px;color:red">Authorized Skip</span></td>';
                                }
                                else {
                                    fistr += '<td></td>';
                                }
                            }
                            else {
                                if (itemData[i].FIData[j].Value == "Green") {
                                    if (itemData[i].FIData[j].Approval == "true") {
                                        fistr += '<td style="text-align:center"> <input type="checkbox" checked="checked" onclick="return false;" id="' + itemData[i].FIData[j].ControlIDOrClass + '"/></td>';
                                    }
                                    else {
                                        fistr += '<td style="text-align:center"> <input type="checkbox" checked="checked" id="' + itemData[i].FIData[j].ControlIDOrClass + '"/></td>';
                                    }

                                }
                                else {
                                    if (itemData[i].FIData[j].Approval == "true") {
                                        fistr += '<td style="text-align:center"> <input type="checkbox" onclick="return false;" id="' + itemData[i].FIData[j].ControlIDOrClass + '"/></td>';
                                    }
                                    else {
                                        fistr += '<td style="text-align:center"> <input type="checkbox" id="' + itemData[i].FIData[j].ControlIDOrClass + '"/></td>';
                                    }
                                }
                            }
                    }
                /* fistr += '<td style="width:250px"><input type="button" value="Save" class="btn btn-info" style="display: inline; margin-left: 6px;visibility:' + itemData[i].SaveVisibility + '" onclick="saveFIData(this)"/><input type="button" value="Approve" class="btn btn-info" style="display: inline; margin-left: 6px;visibility:' + itemData[i].ApproveVisibility + '" onclick="approveFIData(this)"/><input type="button" value="Reject" class="btn btn-info" style="display: inline; margin-left: 6px;visibility:' + itemData[i].RejectVisibility + '" onclick="rejectFIData(this)"/><input type="button" value="Rework" class="btn btn-info" style="display: inline; margin-left: 6px;visibility:' + itemData[i].ReworkVisibility + '" onclick="reworkFIData(this)"/></td>'*/
                    if (itemData[i].Status != "") {
                        fistr += '<td style="white-space:nowrap"><label style="font-size:17px;">' + itemData[i].Status + '</label></td>'
                    }
                    else {
                        fistr += '<td style="white-space:nowrap"><input type="button" value="Save" class="btn btn-info" style="margin-left: 6px;display:' + getDisplayValue(itemData[i].SaveVisibility) + '" onclick="saveFIData(this)"/><input type="button" value="Approve" class="btn btn-info" style=" margin-left: 6px;display:' + getDisplayValue(itemData[i].ApproveVisibility) + '" onclick="approveFIData(this)"/><input type="button" value="Reject" class="btn btn-info" style="margin-left: 6px;display:' + getDisplayValue(itemData[i].RejectVisibility) + '" onclick="rejectFIData(this)"/><input type="button" value="Rework" class="btn btn-info" style="margin-left: 6px;display:' + getDisplayValue(itemData[i].ReworkVisibility) + '" onclick="reworkFIData(this)"/></td>'
                    }
                    fistr += '</tr>';
                }


            }
            else {
                fistr += '<tr style="background-color:white"><td>No Data Found</td></tr>';
            }
            fistr += '</table>';
            $('#FIDataContainer').append(fistr);
        }
        function getDisplayValue(value) {
            if (value == "visible") {
                value = "inline-block";
            }
            else {
                value = "none";
            }
            return value;

        }
        function BindFISlnoDetails(itemData) {
            debugger;
            console.log(itemData);
            $('#FISlnoDataContainer').empty();
            var appendstr = '<table class="table table-bordered table-hover headerFixer" style="width: 100%">';
            if (itemData.length > 0) {
                appendstr = appendstr + '<tr><th>Serial Number</th><th>Component ID</th><th>Line</th><th>Equipment</th><th>OPN</th><th>Issues Found</th><th>Refer</th></tr>';
                for (var i = 0; i < itemData.length; i++) {
                    appendstr += '<tr><td style="background-color:white;color:black;">' + itemData[i].SerialNumber + '</td><td style="background-color:white;color:black;">' + itemData[i].ComponentID + '</td><td style="background-color:white;color:black;">' + itemData[i].PlantID + '</td><td style="background-color:white;color:black;">' + itemData[i].MachineID + '</td><td style="background-color:white;color:black;">' + itemData[i].OperationNumber + '</td><td style="background-color:white;color:black;">' + itemData[i].IssueFound + '</td><td style="background-color:white;color:black;">' + itemData[i].Refer + '</td></tr>';
                }

            }
            else {
                appendstr += '<tr style="background-color:white"><td>No Data Found</td></tr>';
            }
            appendstr += '</table>';
            $('#FISlnoDataContainer').append(appendstr);
        }
        function setCurrentDate() {
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/getCuurentDate",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    $('#txtDateSearch').val(itemData);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function BindPlantData() {
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/getPlantData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    $('#ddlPlantID').empty();
                    if (itemData.length > 0) {
                        var strappend = "";
                        for (var i = 0; i < itemData.length; i++) {
                            strappend += '<option>' + itemData[i] + '</option>';
                        }
                        $('#ddlPlantID').append(strappend);
                        if ($('#hdnPlantID').val() != "" && $('#hdnPlantID').val() != undefined) {
                            $('#ddlPlantID').val($('#hdnPlantID').val());
                        }
                    }

                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            BindCellData();
        }
        $('#ddlPlantID').change(function () {
            BindCellData();
        });
        function BindCellData() {
            if ($('#ddlPlantID').val() == "") {
                openWarningModal("Please select Plant ID.");
                return;
            }
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/getCellID",
                contentType: "application/json; charset=utf-8",
                data: '{plant:"' + $('#ddlPlantID').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    $('#ddlCellID').empty();
                    if (itemData.length > 0) {
                        var strappend = "";
                        for (var i = 0; i < itemData.length; i++) {
                            strappend += '<option>' + itemData[i] + '</option>';
                        }
                        $('#ddlCellID').append(strappend);
                        if ($('#hdnCellID').val() != "" && $('#hdnCellID').val() != undefined) {
                            $('#ddlCellID').val($('#hdnCellID').val());
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        var csvFileGenerationInterval;
        var csvFileGenerationTimeout;
        function saveFIData(btn) {
            if (!$(btn).closest('tr').find('#chkApprove').prop("checked")) {
                openWarningModal("Please check Op90 Checkbox.");
                return;
            }
            $('#loadingModal').modal('show');
            // $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            saveOrApproveFIData(btn, "save");
            var serialNumber = $(btn).closest('tr').find('#lblSlno')[0].textContent;
            var compID = $(btn).closest('tr').find('#lblCompID')[0].value;
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/sendCSVFile",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{slno:"' + serialNumber + '",componentid:"' + compID + '",cell:"' + $('#ddlCellID').val() + '",plant:"' + $('#ddlPlantID').val() + '"}',
                success: function (response) {
                    var itemData = response.d;
                    CompSerialNumber = serialNumber;
                    ComponentID = compID;
                    // csvFileGenerationInterval = setInterval(checkFileGeneratedOrNot, 2000);
                    csvFileGenerationIntervalCount = 0;

                    //csvFileGenerationTimeout = setTimeout(checkFileGeneratedOrNot, 10000);
                    clearInterval(csvFileGenerationInterval);
                    csvFileGenerationInterval = setInterval(checkFileGeneratedOrNot, 5000);
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
            $('#loadingModal').modal('hide');
        }
        function checkFileGeneratedOrNot() {
            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/getFileGeneratedStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{slno:"' + CompSerialNumber + '",componentid:"' + ComponentID + '"}',
                success: function (response) {
                    var isFileGenerated = response.d;
                    debugger;
                    console.log("getFileGeneratedStatus: " + csvFileGenerationIntervalCount + "=" + isFileGenerated);
                    // csvFileGenerationIntervalCount++;
                    if (isFileGenerated == true) {
                        csvFileGenerationIntervalCount = 0;
                        clearInterval(csvFileGenerationInterval);
                        //$.unblockUI({});
                        // $('#loadingModal').modal('hide');
                        if ($('#txtSlnoSearch').val() == "") {
                            BindDateFIData();
                        }
                        else {
                            BindSlnoFIData();
                        }
                    }
                    //else {
                    //    if (csvFileGenerationIntervalCount >= 5) {
                    //        csvFileGenerationIntervalCount = 0;
                    //        clearInterval(csvFileGenerationInterval);
                    //        //$.unblockUI({});
                    //        $('#loadingModal').modal('hide');
                    //        openWarningModal("Not able to generate the File.");
                    //    }
                    //}
                },
                error: function (jqXHR, textStatus, err) {
                    //alert('Error: ' + err);
                }
            });
        }
        function approveFIData(btn) {
            debugger;
            //if (!$(btn).closest('tr').find('#chkApprove').prop("checked")) {
            //    openWarningModal("Please check Op100 Checkbox.");
            //    return;
            //}
            rejectapprovebtn = btn;
            openConfirmApproveModal();
        }
        function rejectFIData(btn) {
            //if (!$(btn).closest('tr').find('#chkApprove').prop("checked")) {
            //    openWarningModal("Please check Op100 Checkbox.");
            //    return;
            //}
            rejectapprovebtn = btn;
            openConfirmRejectModal();

        }
        function reworkFIData(btn) {
            debugger;
            rejectapprovebtn = btn;
            openConfirmReworkModal();
        }
        function saveOrApproveFIData(btn, status) {
            debugger;
            var serialNumber = $(btn).closest('tr').find('#lblSlno')[0].textContent;
            var compID = $(btn).closest('tr').find('#lblCompID')[0].value;
            $('#hdnSlno').val(serialNumber);
            $('#hdnCompID').val(compID);
            var chkApproveStatus = "nochk";
            if ($(btn).closest('tr').find('#chkApprove')[0] != undefined) {

                if ($(btn).closest('tr').find('#chkApprove')[0].checked) {
                    chkApproveStatus = "Green";
                }
                else {
                    chkApproveStatus = "Red";
                }
            }
            var remarks = $(btn).closest('tr').find('#txtRemarks')[0].value;

            $.ajax({
                async: false,
                type: "POST",
                url: "ShantiFinalInspection.aspx/saveFIData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + serialNumber + '",comp:"' + compID + '",remarkvalue:"' + remarks + '",statusvalue:"' + status + '",chk90:"' + chkApproveStatus + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    if (itemData == 0) {
                        alert("Insertion failed.");
                    }
                    else {
                        if ($('#txtSlnoSearch').val() == "") {
                            BindDateFIData();
                        }
                        else {
                            BindSlnoFIData();
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
