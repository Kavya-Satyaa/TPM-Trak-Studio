<%@ Page Title="Packing Station" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PackingStation.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.PackingStation" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #PSDataContainer table tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #PSDataContainer table tr:nth-child(even) {
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
    </style>
    <div class="row">
        <div class="col-lg-12" style="text-align: left; display: block; margin-bottom: 10px;">
            <asp:HiddenField runat="server" ID="hdnFilenName" ClientIDMode="Static" />
            <div style="font-size: 50px; color: white; text-align: center">Scan
                <input type="text" autocomplete="off" id="txtSlnoSearch" onkeypress="scanSlNo(event)" onkeydown="manuallyEnterSlNo()" data-toggle="tooltip" title="search !" placeholder="Serial Number..." class="form-control" style="width: 400px; display: inline; font-size: 48px; height: 60px; margin-right: 15%; padding-bottom: 12px"></div>
            <table style="margin-top: 5px">
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

                    <td class="input-group" style="min-width: 150px; border: 0">
                        <label style="color: white">Date&nbsp;&nbsp;</label>
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtDateSearch" runat="server" ClientIDMode="Static" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>

                    </td>
                    <td>
                        <%--  <input type="date" autocomplete="off" id="txtDateSearch" data-toggle="tooltip" class="form-control" style="width: 250px; display: inline;">--%>
                        <input value="View" type="button" class="btn btn-info" id="btnDateView" style="display: inline; margin-left: 6px" />
                    </td>
                </tr>
            </table>

        </div>
    </div>
    <div id="PSDataContainer" style="overflow: auto; width: 100%; height: 75vh">
    </div>
    <div id="PSSlnoDataContainer" style="overflow: auto; width: 100%; margin-top: 10px">
        <%--height:40vh--%>
    </div>


    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 600px">
            <div class="modal-content" style="border: 2px solid #5D7B9D; background-color: white">
                <div class="modal-header" style="background-color: #5D7B9D; padding: 8px">

                    <h4 class="modal-title" style="color: white; font-size: 25px">Warning!</h4>
                </div>
                <div class="modal-body" style="padding: 20px 15px">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="lblWarningMsg" style="font-size: 23px;"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #5D7B9D; text-align: right">
                    <button type="button" data-dismiss="modal" style="width: 80px; font-size: 18px" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>
    <asp:Button runat="server" ID="btnExport" Visible="false" ClientIDMode="Static" OnClick="btnExport_Click" UseSubmitBehavior="false" />
    <script>
        var rejectapprovebtn;
        var slnoRefreshInterval;
        $(document).ready(function () {
            $('[id$=txtDateSearch]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            BindPlantData();
            // setCurrentDate();
            $('#btnDateView').click();
            $("#txtSlnoSearch").focus();
            getScannedSlno();
        });
        function getScannedSlno() {
            slnoRefreshInterval = setInterval(function () {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "PackingStation.aspx/getScannedSlnoFromTbl",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var itemData = response.d;
                        if (itemData != "") {
                            $('#txtSlnoSearch').val(itemData);
                            BindSlnoPSData(itemData);
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
                url: "PackingStation.aspx/setSlnoStatus",
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
                    BindSlnoPSData(SerialNumber);
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
                BindSlnoPSData(SerialNumber);

                enterCondition = 0;
                temptime = "";
            }, timeout);
        }


        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }

        $('#btnSlnoView').click(function () {
            if ($('#ddlCellID').val() == "" || $('#ddlCellID').val() == null) {
                openWarningModal("Please select Cell ID.");
                return;
            }
            if ($('#txtSlnoSearch').val().trim() == "") {
                openWarningModal("Please enter Serial Number."); //kkkkkk
                return;
            }
            BindSlnoPSData();
        });
        $('#btnDateView').click(function () {
            if ($('#ddlCellID').val() == "" || $('#ddlCellID').val() == null) {
                openWarningModal("Please select Cell ID.");
                return;
            }
            BindDatePSData();
        });
        function BindSlnoPSData(SerialNumber) {
            // setCurrentDate();
            let currentDate = new Date();
            $("#txtDateSearch").val(currentDate.format("dd-MM-yyyy"));
            //var searchslno = $('#txtSlnoSearch').val();
            var searchslno = SerialNumber;
            $.ajax({
                async: false,
                type: "POST",
                url: "PackingStation.aspx/getSlnoPSData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + searchslno + '",plant:"' + $('#ddlPlantID').val() + '",cell:"' + $('#ddlCellID').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;

                    debugger;
                    BindPSData(itemData.FICheckList);
                    // BindPSSlnoDetails(itemData.FISlnoDetails);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function BindDatePSData() {
            $('#txtSlnoSearch').val("");
            var searchdate = $('#txtDateSearch').val();
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: "PackingStation.aspx/getDatePSData",
                contentType: "application/json; charset=utf-8",
                data: '{date:"' + searchdate + '",plant:"' + $('#ddlPlantID').val() + '",cell:"' + $('#ddlCellID').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    debugger;
                    BindPSData(itemData.FICheckList);
                    //  BindPSSlnoDetails(itemData.FISlnoDetails);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function BindPSData(itemData) {
            debugger;
            $('#PSDataContainer').empty();
            var fistr = '<table id="tblFiInfo" class="table table-bordered table-hover headerFixer" style="width: 100%">';
            if (itemData.length > 0) {

                for (var i = 0; i < itemData.length; i++) {
                    if (i == 0) {
                        fistr += '<thead class="blue"><tr>';
                        for (var j = 0; j < itemData[i].FIData.length; j++) {
                            fistr += '<th>' + itemData[i].FIData[j].Parameter + '</th>';
                        }
                        //   fistr += '<th>Action</th>';
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
                                // if (itemData[i].FIData[j].Approval == "true") {
                                //fistr += '<td><input  type="text" class="form-control" readonly="readonly" id="' + itemData[i].FIData[j].ControlIDOrClass + '"  value="' + itemData[i].FIData[j].Value + '"/></td>';
                                //}
                                //else {
                                fistr += '<td><input  type="text" class="form-control" id="' + itemData[i].FIData[j].ControlIDOrClass + '"  value="' + itemData[i].FIData[j].Value + '"/></td>';
                                //}
                            }
                            else if (itemData[i].FIData[j].Type == "link") {
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
                    //  fistr += '<td><input type="button" value="Save" class="btn btn-info" style="display: inline; margin-left: 6px;visibility:' + itemData[i].SaveVisibility + '" onclick="savePackingStationData(this)"/></td>'
                    fistr += '</tr>';
                }


            }
            else {
                fistr += '<tr style="background-color:white"><td>No Data Found</td></tr>';
            }
            fistr += '</table>';
            $('#PSDataContainer').append(fistr);
        }
        function downloadFileandSetValueToHdnVar(lnk) {
            debugger;
            $('#hdnFilenName').val(lnk.text);
            __doPostBack('<%= btnExport.UniqueID%>', '');
        }
        function BindPSSlnoDetails(itemData) {
            debugger;
            console.log(itemData);
            $('#PSSlnoDataContainer').empty();
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
            $('#PSSlnoDataContainer').append(appendstr);
        }
        function setCurrentDate() {
            $.ajax({
                async: false,
                type: "POST",
                url: "PackingStation.aspx/getCuurentDate",
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
                url: "PackingStation.aspx/getPlantData",
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
                url: "PackingStation.aspx/getCellID",
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
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function savePackingStationData(btn) {

            saveOrApproveFIData(btn, "save");
        }
        function saveOrApproveFIData(btn, status) {
            debugger;
            var serialNumber = $(btn).closest('tr').find('#lblSlno')[0].textContent;
            var compID = $(btn).closest('tr').find('#lblCompID')[0].value;

            var remarks = $(btn).closest('tr').find('#txtRemarks')[0].value;

            $.ajax({
                async: false,
                type: "POST",
                url: "PackingStation.aspx/savePackingStationData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + serialNumber + '",comp:"' + compID + '",remarkvalue:"' + remarks + '",statusvalue:"' + status + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    if (itemData == 0) {
                        alert("Insertion failed.");
                    }
                    else {
                        if ($('#txtSlnoSearch').val() == "") {
                            BindDatePSData();
                        }
                        else {
                            BindSlnoPSData(serialNumber);
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
