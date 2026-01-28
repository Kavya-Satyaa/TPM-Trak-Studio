<%@ Page Title="Quality Gate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QualityGate.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.QualityGate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .submenus-style a {
            background-color: #393e46;
            color: white;
            padding: 20px 30px !important;
            font-size: 20px;
        }

        .submenus-style > li:hover {
            background-color: #393e46;
        }

            .submenus-style > li:hover a {
                background-color: #393e46;
                color: white;
            }

        .submenus-style li.active a {
            background-color: #158eb3;
            color: white;
        }

        .gridContainer {
            height: 60vh;
            overflow: auto;
        }

            .gridContainer table {
                width: 100%;
            }

                .gridContainer table th {
                    color: white;
                    background-color: #2E6886 !important;
                    padding: 8px 10px;
                }

                .gridContainer table tr td {
                    padding: 8px 10px;
                }

                .gridContainer table tbody tr:nth-child(odd) {
                    background-color: #DCDCDC;
                    color: black;
                }

                .gridContainer table tbody tr:nth-child(even) {
                    background-color: #FFFFFF;
                    color: black;
                }

        .btnStartEnd {
            width: auto;
            border: 2px solid #179890;
            color: white;
            background-color: #397d73;
            border-radius: 25px;
            padding: 10px 15px;
            font-size: 25px;
            font-weight: 600;
            box-shadow: 0px 5px 4px #3c454a;
            display:none;
        }

        .redBackColor {
            background-color: #e65959 !important;
        }

            .redBackColor td {
                color: white !important;
            }

        #lblProcessCount {
            border: 3px solid white;
            padding: 7px 20px;
            border-radius: 10px;
            color: white;
            font-size: 30px;
            display:none;
        }

        .btn-info {
            padding: 7px 15px;
        }

        .slnoInfo tr td {
            color: white;
            font-size: 18px;
        }

        .slnoInfoLbl {
            font-weight: bold;
        }
        .slnoInfo tr td label{
            margin-bottom:0px;
        }
        #manualInpectionOperationContainer input[type="checkbox"] {
            width: 18px;
            height: 18px;
        }

        #lblProcessStatus {
            font-size: 25px;
            color: white;
            font-weight: 600;
            margin-top: 5px;
        }
    </style>
    <div class="">
        <div style="text-align: center">
            <asp:DropDownList runat="server"  ID="ddlCompId" CssClass="form-control" Style="width: 400px; display: inline; font-size: 35px; height: 45px; padding-top: 0px" ClientIDMode="Static"></asp:DropDownList>
        <%--    <asp:TextBox runat="server" ID="txtCompID" ClientIDMode="Static" Style="width: 400px; display: inline; font-size: 35px; height: 45px; padding-bottom: 9px" placeholder="Component ID..." CssClass="form-control" ></asp:TextBox>--%>
            <asp:TextBox runat="server" ID="txtSlnoSearch" ClientIDMode="Static" Style="width: 400px; display: inline; font-size: 35px; height: 45px; padding-bottom: 9px" placeholder="Serial Number..." CssClass="form-control" ></asp:TextBox>
             <input value="View" type="button" class="btn btn-info" id="btnView" style="display: inline; margin-left: 6px"  onclick="BindQAGate();"/>
            <%--onkeypress="scanSlNo(event)" onkeydown="manuallyEnterSlNo()"--%>
        </div>
        <div style="height:77vh">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <div style="display: inline-block; margin-top: 7px; width: 100%">
                        <div id="subMenuContainer" class="navbar-collapse collapse" style="height: 42px !important; display: inline-block !important; width: 40%; padding-left: 0px">
                            <ul class="nav navbar-nav submenus-style" id="subMenuUl">
                                <li id="liEquatorSPCMenu"><a runat="server" class="submenuData" id="A15" clientidmode="static" data-toggle="tab" href="#equatorSPCMenu">Equator SPC</a>
                                    <i></i>
                                </li>
                                <li><a runat="server" class="submenuData" id="A14" clientidmode="static" data-toggle="tab" href="#manualInpectionMenu">Manual Inspection</a>
                                    <i></i>
                                </li>
                                <li><a runat="server" class="submenuData" id="A1" clientidmode="static" data-toggle="tab" href="#CMMDataFile">CMM Data</a>
                                    <i></i>
                                </li>
                            </ul>
                        </div>
                        <div style="width: 58%; display: inline-block; vertical-align: top; margin-top: 4px; text-align: center">
                            <input type="button" id="btnProcessStart" value="Process Start" class="btnStartEnd"  />&nbsp;&nbsp;
                             <input type="button" id="btnProcessEnd" value="Process End" class="btnStartEnd" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label runat="server" ID="lblProcessCount" ClientIDMode="Static">00:00</asp:Label>
                              <asp:Label runat="server" ID="lblProcessStatus" ClientIDMode="Static"></asp:Label>
                        </div>
                    </div>
                    <div class="tab-content themetoggle" id="equatorContainer" style="overflow: auto; width: 100%; margin: 20px auto;">
                        <div id="equatorSPCMenu" class="tab-pane fade">
                        <table class="slnoInfo">
                                <tr>
                                    <td class="slnoInfoLbl">Plant ID: </td>
                                    <td><label id="lblSPCPlantID" ></label></td>
                                    <td class="slnoInfoLbl">&nbsp;&nbsp;Component ID:</td>
                                    <td><label id="lblSPCCompID" ></td>
                                    <td class="slnoInfoLbl">&nbsp;&nbsp;Serial Number:</td>
                                    <td><label id="lblSPCSerialNo"></td>
                                </tr>
                            </table>
                            <div class="gridContainer" style="margin-top: 10px">
                                <div id="equatorSPCOperationContainer">

                                </div>
                            </div>
                        </div>
                        <div id="manualInpectionMenu" class="tab-pane fade">
                              <table class="slnoInfo">
                                <tr>
                                    <td class="slnoInfoLbl">Plant ID: </td>
                                    <td><label id="lblMIPlantID" ></label></td>
                                    <td class="slnoInfoLbl">&nbsp;&nbsp;Component ID:</td>
                                    <td><label id="lblMICompID" ></td>
                                    <td class="slnoInfoLbl">&nbsp;&nbsp;Serial Number:</td>
                                    <td><label id="lblMISerialNo"></td>
                                </tr>
                            </table>
                            <div class="gridContainer" style="margin-top: 10px">
                                <div id="manualInpectionOperationContainer">

                                </div>
                            </div>
                            <div style="margin-top: 10px;">
                                <input type="button" id="btnSaveManualInspection" class="btn btn-info"  value="Save"/>
                            </div>
                        </div>
                          <div id="CMMDataFile" class="tab-pane fade" style="margin-top:30px;">
                              <div id="CMMFileContainer">
                                    <a id="aCMMFile" href="" style="color:white;font-size:30px;text-decoration-line: underline;"> <i class="glyphicon glyphicon-download-alt"></i> <span id="lblCMMFileName"></span></a>
                              </div>
                          
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <br />
        <div>
            <input type="button" id="btnSendToCMM"  class="btn btn-info" value="Send To CMM" />
             <input type="button" id="btnApprove"  class="btn btn-info" value="Approve" />
             <input type="button" id="btnReject"  class="btn btn-info"  value="Reject"/>
         <%--   <asp:Button runat="server" ID="btnSendToCMM" Text="Send To CMM" CssClass="btn btn-info" OnClientClick="return sendToCMMClick('snxbs');" />
            <asp:Button runat="server" ID="btnApprove" Text="Approve" CssClass="btn btn-info" />
            <asp:Button runat="server" ID="btnReject" Text="Reject" CssClass="btn btn-info" />--%>
        </div>

    </div>
     <div class="modal fade" id="approvedConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation?</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to Approve this Serial Number?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" style="width: 80px;" class="modalBtns" onclick="return approveConfirm();">Yes</button>
                     <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">No</button>
                </div>
            </div>
        </div>
    </div>
     <div class="modal fade" id="rejectedConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation?</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to Reject this Serial Number?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" style="width: 80px;" class="modalBtns" onclick="return rejectConfirm();">Yes</button>
                     <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">No</button>
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
     <div class="modal fade" id="errorModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Error!</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="lblErrorMsg" style="color: black"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>
   
    <script>
        var processTimeCounter;
        var processCount = 0;
        var selectedSubMenu, SerialNumber, PartNumber, ProcessStartTime = "", QualityGateStatus = "", ProcessStatus = "", SendToCMMStatus = "", CMMFileName = "";
        var slnoRefreshInterval,sendToCMMInterval;
        $(document).ready(function () {
            $('#txtSlnoSearch').focus();
            hideControls();
            getScannedSlno();
           
        });
        function BindQAGate() {
            if ($('#txtSlnoSearch').val().trim() == "") {
                openWarningModal("Please enter Serial Number.");
                return;
            }
            if ($('#ddlCompId').val() == "" || $('#ddlCompId').val() == null) {
                openWarningModal("Please enter Component ID.");
                return;
            }
            clearProcessVariable();
            PartNumber = $('#ddlCompId').val();
            slnoScanned();
        }
        function isPartNumberAndSlnoCorrect() {
            var isExists = false;
            $.ajax({
                async: false,
                type: "POST",
                url: "QualityGate.aspx/isPartNumberAndSlnoExistsInSPCAutoData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + SerialNumber + '",partnum:"' + PartNumber + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    isExists = itemData;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            return isExists;
        }
        $('#ddlCompId').change(function () {
            $('#txtSlnoSearch').focus();
        });
        function getScannedSlno() {
            slnoRefreshInterval = setInterval(function () {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "QualityGate.aspx/getScannedSlnoFromTbl",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var itemData = response.d;
                        if (itemData.length > 0) {
                         
                            let slno = itemData[0];
                            let partno = itemData[1];
                            if (slno != "" && slno != null && partno != "" && partno != null) {
                                clearProcessVariable();
                                $('#txtSlnoSearch').val(slno);
                                $('#txtCompID').val(partno);
                                PartNumber = partno;
                                slnoScanned();
                                updateSlnoStatus(slno);
                            }
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                    }
                });
            }, 5000);
        }
        function clearProcessVariable() {
            SerialNumber = "";
            PartNumber = "";
            ProcessStartTime = "";
            QualityGateStatus = "";
            ProcessStatus = "";
            SendToCMMStatus = "";
            CMMFileName = "";
            $('#lblProcessStatus').text("");
        }
        function updateSlnoStatus(slnum) {
            $.ajax({
                async: false,
                type: "POST",
                url: "QualityGate.aspx/setSlnoStatus",
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
        function hideControls() {
            $('#btnSendToCMM').css("display", "none");
            $('#btnApprove').css("display", "none");
            $('#btnReject').css("display", "none");
            $('#equatorContainer').css("visibility", "hidden");
           // $('#btnProcessStart').css("display", "none"); //--- nosn
           // $('#btnProcessEnd').css("display", "none"); //--- nosn
        /*  $('#lblProcessCount').css("visibility", "hidden");*/ //--- nosn
            $('#subMenuContainer').css("pointer-events", "none");
          //  clearInterval(processTimeCounter); //--- nosn
            $('#subMenuUl  li').removeClass("active");
            $('.tab-content .tab-pane').removeClass("active in");
            clearInterval(sendToCMMInterval);
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
                    let slno = "";
                    let scannedText = $('#txtSlnoSearch').val();

                    if (scannedText != "") {
                        if (scannedText.includes("-")) {
                            let splittedScannedText = [];
                            splittedScannedText = scannedText.split("-");
                            slno = splittedScannedText[splittedScannedText.length - 2];
                            $('#txtSlnoSearch').val(slno);
                        } else {
                            slno = scannedText;
                        }
                    }
                    if (slno.trim() == "") {
                        return;
                    }
                    clearProcessVariable();
                    slnoScanned();
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
                let slno = "";
                let scannedText = $('#txtSlnoSearch').val();

                if (scannedText != "") {
                    if (scannedText.includes("-")) {
                        let splittedScannedText = [];
                        splittedScannedText = scannedText.split("-");
                        slno = splittedScannedText[splittedScannedText.length - 2];
                        $('#txtSlnoSearch').val(slno);
                    } else {
                        slno = scannedText;
                    }
                }
                if (slno.trim() == "") {

                    return;
                }
                clearProcessVariable();
                slnoScanned();
                enterCondition = 0;
                temptime = "";
            }, timeout);
        }
        function slnoScanned() {
            hideControls();
          
            $('#txtSlnoSearch').blur();
            SerialNumber = $('#txtSlnoSearch').val();
            if (PartNumber == "" || PartNumber == undefined || PartNumber == null) {
                openWarningModal("Part Number is required.");
                return;
            }
            //if (isPartNumberAndSlnoCorrect() == false) {
            //    openWarningModal("This Part Number and Serial Number not exists.");
            //    return;
            //}
            showControls();
            $.ajax({
                timeout: 5000,
                async: false,
                type: "POST",
                url: "QualityGate.aspx/getSlnoStatus",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + $('#txtSlnoSearch').val() + '",partNum:"' + PartNumber + '"}',
                datatype: "json",
                success: function (response) {
                    var itemdata = response.d;
                    debugger;
                    if (itemdata != null && itemdata != undefined) {
                        if (itemdata.ProcessStatus == "Error") {
                            openWarningModal("Error.");
                            return;
                        }
                        else {
                          //  processCount = itemdata.ProcessCount; //--- nosn
                           // $('#lblProcessCount').text(convertSecToMinSec(itemdata.ProcessCount)); //--- nosn
                            ProcessStartTime = itemdata.ProcessStartTime;
                            $('#subMenuUl  li').removeClass("active");
                            $('.tab-content .tab-pane').removeClass("active in");
                            $('#liEquatorSPCMenu').addClass("active");
                            $('#equatorSPCMenu').addClass("active in");
                            QualityGateStatus = itemdata.QualityGateStatus;
                            ProcessStatus = itemdata.ProcessStatus;
                            SendToCMMStatus = itemdata.SendToCMMStatus;
                            CMMFileName = itemdata.CMMFileName;
                            if (itemdata.ProcessStatus == "Running") {
                                $('#subMenuContainer').css("pointer-events", "unset");
                               // $('#btnProcessStart').css("display", "none"); //--- nosn
                               // $('#btnProcessEnd').css("display", "inline-block"); //--- nosn
                                $('#btnProcessStart').click();
                            }
                            else if (itemdata.ProcessStatus == "Pending") {
                               // $('#btnProcessStart').css("display", "inline-block"); //--- nosn
                               // $('#btnProcessEnd').css("display", "none"); //--- nosn
                                $('#subMenuContainer').css("pointer-events", "unset");
                                //  $('#liEquatorSPCMenu').addClass("active");
                                // $('#equatorSPCMenu').addClass("active in");
                                $('#btnProcessStart').click();
                            }
                            else if (itemdata.ProcessStatus == "Completed") {
                              // $('#btnProcessStart').css("display", "none");  //--- nosn
                              //  $('#btnProcessEnd').css("display", "none"); //--- nosn
                                $('#subMenuContainer').css("pointer-events", "unset");
                                //  $('#liEquatorSPCMenu').addClass("active");
                                //  $('#equatorSPCMenu').addClass("active in");
                                openWarningModal("Serial Number is Completed.");
                                $('#lblProcessStatus').text("Status: " + QualityGateStatus);
                            }
                          
                            getSerialNumberOperationDetails();
                            getSerialNumberManualInpsectionDetails();
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });
           
        }
        function showControls() {
            $('#equatorContainer').css("visibility", "visible");
           // $('#lblProcessCount').css("visibility", "visible"); //--- nosn
          
        }
        function convertSecToMinSec(seconds) {
            var min = Math.floor(parseInt(seconds) / 60);
            var sec = parseInt(seconds) - min * 60;
            var tempMin = "", tempSec = "";
            if (min.toString().length == 1) {
                tempMin = "0" + min;
            }
            else {
                tempMin = min.toString();
            }
            if (sec.toString().length == 1) {
                tempSec = "0" + sec;
            }
            else {
                tempSec = sec.toString();
            }
            return tempMin + ":" + tempSec;
        }
        $('#btnProcessStart').click(function () {
            $.ajax({
                async: false,
                type: "POST",
                url: "QualityGate.aspx/saveProcessStartEndData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + SerialNumber + '",partno:"' + PartNumber + '",param:"insert",processStartTime:""}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    if (itemData == "error") {
                        openWarningModal("Insertion falied.");
                        return;
                    }
                    else {
                        if (itemData != "exists") {
                            ProcessStartTime = itemData;
                            $.ajax({
                                timeout: 5000,
                                async: false,
                                type: "POST",
                                url: "QualityGate.aspx/getSlnoStatus",
                                contentType: "application/json; charset=utf-8",
                                data: '{slno:"' + SerialNumber + '",partNum:"' + PartNumber + '"}',
                                datatype: "json",
                                success: function (response) {
                                    var itemdata = response.d;
                                    debugger;
                                    if (itemdata != null && itemdata != undefined) {
                                        if (itemdata.ProcessStatus == "Error") {
                                            openWarningModal("Error.");
                                            return;
                                        }
                                        else {
                                            ProcessStartTime = itemdata.ProcessStartTime;
                                            QualityGateStatus = itemdata.QualityGateStatus;
                                            ProcessStatus = itemdata.ProcessStatus;
                                            SendToCMMStatus = itemdata.SendToCMMStatus;
                                            CMMFileName = itemdata.CMMFileName;
                                            $('#subMenuUl  li').removeClass("active");
                                            $('.tab-content .tab-pane').removeClass("active in");
                                            $('#liEquatorSPCMenu').addClass("active");
                                            $('#equatorSPCMenu').addClass("active in");
                                        }
                                    }
                                },
                                error: function (jqXHR, textStatus, err) {
                                    console.log(err);
                                }
                            });
                        }
                        if (SendToCMMStatus != "") {
                            $('#btnSendToCMM').css("display", "none");
                        }
                        else {
                            $('#btnSendToCMM').css("display", "inline-block");
                        }
                        $('#btnApprove').css("display", "inline-block");
                        $('#btnReject').css("display", "inline-block");
                       // $('#btnProcessStart').css("display", "none"); //--- nosn 
                      //  $('#btnProcessEnd').css("display", "inline-block"); //--- nosn
                        $('#subMenuContainer').css("pointer-events", "unset");
                        //$('#liEquatorSPCMenu').addClass("active");
                        //$('#equatorSPCMenu').addClass("active in");
                        //processTimeCounter = setInterval(function () {
                        //    processCount++;
                        //    $('#lblProcessCount').text(convertSecToMinSec(processCount));
                        //}, 1000);   //--- nosn
                        //getSerialNumberOperationDetails();
                        //getSerialNumberManualInpsectionDetails();
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        });
        $('#btnProcessEnd').click(function () {
            $.ajax({
                async: false,
                type: "POST",
                url: "QualityGate.aspx/saveProcessStartEndData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + SerialNumber + '",partno:"' + PartNumber + '",param:"update",processStartTime:"' + ProcessStartTime + '"}',
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    if (itemData == "error") {
                        openWarningModal("Insertion falied.");
                        return;
                    }
                    else {
                        hideControls();
                        clearProcessVariable();
                        $('#txtSlnoSearch').val(SerialNumber);
                        $('#txtSlnoSearch').focus();
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
         
        });
        function getSerialNumberOperationDetails() {
            if (SerialNumber == "" || SerialNumber == undefined || SerialNumber == null) {
                openWarningModal("Serial Number is required.");
                return false;
            }
            if (PartNumber == "" || PartNumber == undefined || PartNumber == null) {
                openWarningModal("Part Number is required.");
                return false;
            }
            $.ajax({
                timeout: 5000,
                async: false,
                type: "POST",
                url: "QualityGate.aspx/getSerialNumberOperationData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + SerialNumber + '",partnum:"' + PartNumber + '"}',
                datatype: "json",
                success: function (response) {
                    var itemdata = response.d;
                    $('#equatorSPCOperationContainer').empty();
                    var appendString = '<table class="table table-bordered  headerFixer"><tr><th>Characteristic ID</th> <th>Characteristic Code</th><th>LSL</th><th>USL</th><th>Value</th><th>Unit</th> <th>Status</th></tr>';
                    if (itemdata != null && itemdata != undefined) {
                        if (itemdata.length > 0) {
                            for (var i = 0; i < itemdata.length; i++) {
                                if (i == 0) {
                                    $('#lblSPCPlantID').text(itemdata[i].PlantID);
                                    $('#lblSPCCompID').text(itemdata[i].ComponentID);
                                    $('#lblSPCSerialNo').text(itemdata[i].SerialNumber);
                                }
                                appendString += '<tr class="' + itemdata[i].BackColor + '"><td>' + itemdata[i].CharacteristicID + '</td><td>' + itemdata[i].CharecteristicCode + '</td><td>' + itemdata[i].LSL + '</td><td>' + itemdata[i].USL + '</td><td>' + itemdata[i].Value + '</td><td>' + itemdata[i].Unit + '</td><td>' + itemdata[i].Status + '</td></tr>';
                            }
                        }
                        else {
                            appendString += '<tr><td colspan="7">No Data Found.</td></tr>';
                        }
                    }
                    else {
                        appendString += '<tr><td colspan="7">No Data Found.</td></tr>';
                    }
                    appendString += "</table>";
                    $('#equatorSPCOperationContainer').append(appendString);

                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });

        }
        function getSerialNumberManualInpsectionDetails() {
            if (SerialNumber == "" || SerialNumber == undefined || SerialNumber == null) {
                openWarningModal("Serial Number is required.");
                return false;
            }
            if (PartNumber == "" || PartNumber == undefined || PartNumber == null) {
                openWarningModal("Part Number is required.");
                return false;
            }
            $.ajax({
                timeout: 5000,
                async: false,
                type: "POST",
                url: "QualityGate.aspx/getSlnoManualInpsectionDetails",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + SerialNumber + '",partnum:"' + PartNumber + '"}',
                datatype: "json",
                success: function (response) {
                    var itemdata = response.d;
                    $('#manualInpectionOperationContainer').empty();
                    var appendString = '<table class="table table-bordered  headerFixer"><tr><th>Operation</th><th>Component ID</th><th>Characteristic ID</th> <th>Characteristic Code</th><th>LSL</th> <th>USL</th><th>Unit</th><th>Measure Value</th><th>Remarks</th><th>Timestamp</th><th>User</th></tr>';
                    if (itemdata != null && itemdata != undefined) {
                        if (itemdata.length > 0) {
                            for (var i = 0; i < itemdata.length; i++) {
                                if (i == 0) {
                                    $('#lblMIPlantID').text(itemdata[i].PlantID);
                                    $('#lblMICompID').text(itemdata[i].ComponentID);
                                    $('#lblMISerialNo').text(itemdata[i].SerialNumber);
                                }
                                if (itemdata[i].DataType == "VisualInspection") {
                                    itemdata[i].BackColor = "";
                                }
                                appendString += '<tr class="' + itemdata[i].BackColor + '"><td><label id="lblOperationName">' + itemdata[i].OperationName + '</label></td><td><input type="hidden" id="hdnMachineID" value="' + itemdata[i].MachineID + '" /><input type="hidden" id="hdnSpecification" value="' + itemdata[i].SpecificationMean + '" /><label id="lblCompID">' + itemdata[i].ComponentID + '</label></td><td><label id="lblCharacteristicID">' + itemdata[i].CharacteristicID + '</label></td><td><label id="lblCharCode">' + itemdata[i].CharecteristicCode + '</label></td><td><label id="lblLSL">' + itemdata[i].LSL + '</label></td><td><label id="lblUSL">' + itemdata[i].USL + '</label></td><td><label id="lblUnit">' + itemdata[i].Unit + '</label></td><td><input type="hidden" id="hdnMeasureValue" value="' + itemdata[i].Value + '" /><input type="hidden" id="hdnDataType" value="' + itemdata[i].DataType + '" />';
                                let txtAttr = "";
                                let chkAttr = "";
                                if (QualityGateStatus == "Approved" || QualityGateStatus == "Rejected" || ProcessStatus == "Completed") {
                                    txtAttr = 'disabled="disabled"';
                                    chkAttr = 'onclick="return false"';
                                }
                                if (itemdata[i].DataType == "Numeric") {
                                    appendString += '<input type="text" class="form-control" onkeypress="return allowNumber(event);" style="" id="txtMeasureValue" value="' + itemdata[i].Value + '" ' + txtAttr + '  onblur="manualInpectionValueValidation(this);"/>';
                                }
                                else if (itemdata[i].DataType == "Ok" || itemdata[i].DataType == "NotOk") {
                                    if (itemdata[i].Value == "1") {
                                        appendString += '<input type="checkbox" checked="checked"  id="chkMeasureValue" ' + chkAttr + '/>';
                                    }
                                    else {
                                        appendString += '<input type="checkbox"  id="chkMeasureValue" ' + chkAttr + '/>';
                                    }
                                }
                                else if (itemdata[i].DataType == "VisualInspection") {
                                    let chkokValue = "", chknotokValue = "";
                                    if (itemdata[i].Value == "1") {
                                        chkokValue = 'checked="checked" ';
                                    }
                                    else if (itemdata[i].Value == "0") {
                                        chknotokValue = 'checked="checked" ';
                                    }
                                    appendString += '<input type="checkbox" id="chkVIOk"  name="viok' + i + '"  ' + chkAttr + ' ' + chkokValue + ' onclick="visualChkClick(this);"/><label for="viok' + i + '">OK</label>&nbsp;&nbsp;<input type="checkbox"  name="vinotok' + i + '"  id="chkVINotOk" ' + chkAttr + ' ' + chknotokValue +' onclick="visualChkClick(this);"/><label for="vinotok' + i + '">NOT OK</label></td>';
                                }
                                else {
                                    appendString += '<input type="text" id="txtMeasureValue" value="' + itemdata[i].Value + '" class="form-control" ' + txtAttr + ' onblur="manualInpectionValueValidation(this);"/>';
                                }
                                appendString += '</td >';
                                appendString += '<td><input type="text" id="txtRemarks" value="' + itemdata[i].Result + '" class="form-control" ' + txtAttr + '/></td>';
                                appendString += '</td ><td>' + itemdata[i].TimeStamp + '</td><td>' + itemdata[i].UpdatedBy + '</td></tr > ';
                            }

                            ////visual inspection row
                            //appendString += '<tr><td><label>65</label></td><td><input type="hidden" id="hdnMachineID" value="' + itemdata[0].MachineID + '" /><label id="lblCompID">' + itemdata[0].ComponentID + '</label></td><td><label id="lblCharacteristicID">Visual Inspection</label></td>';
                            //appendString += '<td ><input type="checkbox"  name="viok' + i + '" /><label for="viok' + i + '">OK</label>&nbsp;&nbsp;<input type="checkbox"  name="vinotok' + i + '" /><label for="vinotok' + i + '">NOT OK</label></td>';
                            //appendString += '<td colspan="4"><input type="text" id="txtRemarks" class="form-control"  /></td>';
                            //appendString += '</td ><td>' + itemdata[0].TimeStamp + '</td><td>' + itemdata[0].UpdatedBy + '</td></tr > ';

                            if (QualityGateStatus == "Approved" || QualityGateStatus == "Rejected" || ProcessStatus=="Completed") {
                                $('#btnSaveManualInspection').css("visibility", "hidden");
                            }
                            else {
                                $('#btnSaveManualInspection').css("visibility", "visible");
                            }
                        }
                        else {
                            appendString += '<tr><td colspan="11">No Data Found.</td></tr>';
                            $('#btnSaveManualInspection').css("visibility", "hidden");
                        }
                    }
                    else {
                        appendString += '<tr><td colspan="11">No Data Found.</td></tr>';
                        $('#btnSaveManualInspection').css("visibility", "hidden");
                    }
                    appendString += "</table>";
                    $('#manualInpectionOperationContainer').append(appendString);

                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });
        }
        function visualChkClick(chk) {
            debugger;
            var idforunclick = "chkVIOk";
            if ($(chk).attr('id') == "chkVIOk") {
                idforunclick = "chkVINotOk";
            }
            var chkUncheck = $(chk).closest('tr').find("#" + idforunclick)[0];
            $(chkUncheck).prop("checked", false);
        }
        function manualInpectionValueValidation(txt) {
            let lsl = $(txt).closest('tr').find("#lblLSL")[0].textContent;
            let usl = $(txt).closest('tr').find("#lblUSL")[0].textContent;
            var enteredValue = $(txt).closest('tr').find("#txtMeasureValue")[0].value;
            if (enteredValue == "") {
                $(txt).closest('tr').removeAttr("class");
                return;
            }
            if (lsl == undefined || lsl == null || lsl == "") {
                lsl = 0;
            }
            if (usl == undefined || usl == null || usl == "") {
                usl = 0;
            }
            if (parseFloat(enteredValue) > parseFloat(usl) || parseFloat(enteredValue) < parseFloat(lsl)) {
                openWarningModal("The value you have entered is out of range.");
                $(txt).closest('tr').attr("class", "redBackColor");
                return;
            }
            else {
                $(txt).closest('tr').removeAttr("class");
            }
        }
        $('#btnSaveManualInspection').click(function () {
            debugger;
            var tblRows = $('#manualInpectionOperationContainer table tr');
            for (var i = 1; i < tblRows.length; i++) {
                let operationname = $(tblRows[i]).find("#lblOperationName")[0].textContent;
                let compid = $(tblRows[i]).find("#lblCompID")[0].textContent;
                let charid = $(tblRows[i]).find("#lblCharacteristicID")[0].textContent;
                let hdnDataTpe = $(tblRows[i]).find("#hdnDataType")[0].value;
                let hdnValue = $(tblRows[i]).find("#hdnMeasureValue")[0].value;
                let hdnMachine = $(tblRows[i]).find("#hdnMachineID")[0].value;
                let lsl = $(tblRows[i]).find("#lblLSL")[0].textContent;
                let usl = $(tblRows[i]).find("#lblUSL")[0].textContent;
                let charcode = $(tblRows[i]).find("#lblCharCode")[0].textContent;
                let hdnmean = $(tblRows[i]).find("#hdnSpecification")[0].value;
                let unit = $(tblRows[i]).find("#lblUnit")[0].textContent;
                let remark = $(tblRows[i]).find("#txtRemarks")[0].value;
                let enteredValue = "";
                if (hdnDataTpe == "Ok" || hdnDataTpe == "NotOk") {
                    if ($(tblRows[i]).find("#chkMeasureValue")[0].checked) {
                        enteredValue = "1";
                    }
                    else {
                        enteredValue = "0";
                    }
                }
                else if (hdnDataTpe == "VisualInspection") {
                    if ($($(tblRows[i]).find("#chkVIOk")[0]).prop("checked")) {
                        enteredValue = "1";
                    }
                    else if ($($(tblRows[i]).find("#chkVINotOk")[0]).prop("checked")) {
                        enteredValue = "0";
                        if (remark == "") {
                            openWarningModal("Please enter Remarks for Characteristic ID " + charid + ".");
                            return;
                        }
                    }
                    else {
                        openWarningModal("Please select Measure Value  for Characteristic ID " + charid + ".");
                        return;
                    }
                } else {
                    enteredValue = $(tblRows[i]).find("#txtMeasureValue")[0].value;
                }
                //if (hdnValue != enteredValue) {
                if (hdnDataTpe == "Numeric") {
                    if (lsl == undefined || lsl == null || lsl == "") {
                        lsl = 0;
                    }
                    if (usl == undefined || usl == null || usl == "") {
                        usl = 0;
                    }
                    if (enteredValue == "") {
                       // openWarningModal("Please enter value for Characteristic ID " + charid + ".");
                       // return;
                        enteredValue = "";
                    }
                    //if (parseFloat(enteredValue) > parseFloat(usl) || parseFloat(enteredValue) < parseFloat(lsl)) {
                    //    openWarningModal("Measure Value should be greater than or equal to LSL and less than or equal to USL.");
                    //    return;
                    //}
                }
                var resultFlag = false;
                $.ajax({
                    timeout: 5000,
                    async: false,
                    type: "POST",
                    url: "QualityGate.aspx/saveManualInpectionData",
                    contentType: "application/json; charset=utf-8",
                    data: '{slno:"' + SerialNumber + '",componentid:"' + compid + '",characteristicID:"' + charid + '",value:"' + enteredValue + '",machineid:"QA Gate",operation:"65",characteristiccode:"' + charcode + '",selectedlsl:"' + lsl + '",selectedusl:"' + usl + '",selectedunit:"' + unit + '",mean:"' + hdnmean + '",remarks:"' + remark + '",dataType:"' + hdnDataTpe + '"}',
                    datatype: "json",
                    success: function (response) {
                        var itemdata = response.d;
                        if (itemdata != "success") {
                            openErrorModal("Insertion failed.");
                            resultFlag = true;
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                        console.log(err);
                    }
                });
                if (resultFlag) {
                    break;
                }
                //}
            }
            getSerialNumberManualInpsectionDetails();
        });
        $('#btnSendToCMM').click(function () {
            var isUpdated = saveProcessStatusData("SendToCMM");
            if(isUpdated)
            {
                $('#btnSendToCMM').css("display", "none");
            }
        });
        $('#btnReject').click(function () {
            //var tblRows = $('#manualInpectionOperationContainer table tr');
            //for (var i = 0; i < tblRows.length; i++) {
            //    let hdnValue = $(tblRows[i]).find("#hdnMeasureValue").val();
            //    if (hdnValue == "") {
            //        openWarningModal("Please enter Manual Inspection Data.")
            //        break;
            //    }
            //}
            $('#rejectedConfirmModal').modal('show');
        });
        function rejectConfirm() {
            var isUpdated = saveProcessStatusData("Rejected");
            if (isUpdated) {
                $('#rejectedConfirmModal').modal('hide');
            }
            else {
                openWarningModal("Failed to update record.");
            }
            return false;
        }
        $('#btnApprove').click(function () {
            debugger;
            var tblRows = $('#manualInpectionOperationContainer table tr');
            var count = 0;
            for (var i = 1; i < tblRows.length; i++) {
                let hdnDataTpe = $(tblRows[i]).find("#hdnDataType")[0].value;
                let hdnValue = $(tblRows[i]).find("#hdnMeasureValue")[0].value;
                let hdnMachine = $(tblRows[i]).find("#hdnMachineID")[0].value;
                let enteredValue = "";
                let remark = $(tblRows[i]).find("#txtRemarks")[0].value;
                if (hdnDataTpe == "Ok" || hdnDataTpe == "NotOk") {
                    if ($(tblRows[i]).find("#chkMeasureValue")[0].checked) {
                        enteredValue = "1";
                    }
                    else {
                        enteredValue = "0";
                    }
                }
                else if (hdnDataTpe == "VisualInspection") {
                    if ($($(tblRows[i]).find("#chkVIOk")[0]).prop("checked")) {
                        enteredValue = "1";
                    }
                    else if ($($(tblRows[i]).find("#chkVINotOk")[0]).prop("checked")) {
                        enteredValue = "0";
                        if (remark == "") {
                            openWarningModal("Please enter Remarks for Characteristic ID " + charid + ".");
                            return;
                        }
                    }
                    else {
                        openWarningModal("Please select Measure Value  for Characteristic ID " + charid + ".");
                        return;
                    }
                }
                else
                {
                    enteredValue = $(tblRows[i]).find("#txtMeasureValue")[0].value;
                }
                if (hdnValue == "") {
                    //count++;
                    //break;
                    openWarningModal("Please enter value.");
                    return;
                }
            }
            //if (count == 0) {
            //    openWarningModal("Please enter value.");
            //    return;
            //}
            $('#approvedConfirmModal').modal('show');
        });
        function approveConfirm() {
            var isUpdated = saveProcessStatusData("Approved");
            if (isUpdated) {
                $('#approvedConfirmModal').modal('hide');
            }
            else {
                openWarningModal("Failed to update record.");
            }
            return false;
        }
        function saveProcessStatusData(status) {
            var isUpdated = false;
            if (ProcessStartTime == "") {
                openWarningModal("Something went wrong");
                return false;
            }
            $.ajax({
                timeout: 5000,
                async: false,
                type: "POST",
                url: "QualityGate.aspx/saveProcessStatusDetails",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + SerialNumber + '",partnum:"' + PartNumber + '",starttime:"' + ProcessStartTime + '",processStatus:"' + status + '"}',
                datatype: "json",
                success: function (response) {
                    var itemdata = response.d;
                    if (itemdata != "") {
                        isUpdated = true;
                        if (status == "Approved" || status == "Rejected") {
                            $('#btnProcessEnd').click();
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });
            return isUpdated;
        }
        function bindCMMFileData() {
            $('#CMMFileContainer').css("display", "none");
           // if (ProcessStatus == "Running" || ProcessStatus == "Completed") {
            if (!showCMMFile()) {
                sendToCMMInterval = setInterval(showCMMFile, 3000);
            }
           // }
        }
        function showCMMFile() {
            var isFileExists = false;
            $.ajax({
                timeout: 5000,
                async: false,
                type: "POST",
                url: "QualityGate.aspx/getSlnoCMMFileData",
                contentType: "application/json; charset=utf-8",
                data: '{slno:"' + SerialNumber + '",partnum:"' + PartNumber + '",fileName:"' + CMMFileName + '"}',
                datatype: "json",
                success: function (response) {
                    var itemdata = response.d;
                    $("#aCMMFile").attr("href", "");
                    $('#lblCMMFileName').text("");
                    if (itemdata != "") {
                        isFileExists = true;
                        $("#aCMMFile").attr("href", itemdata);
                        if (itemdata.split("/").length > 1) {
                            $('#lblCMMFileName').text(itemdata.split("/")[1]);
                        }
                       
                        $('#CMMFileContainer').css("display", "block");
                        clearInterval(sendToCMMInterval);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });
            return isFileExists;
        }
        $(".submenuData").click(function () {
            debugger;
            clearInterval(sendToCMMInterval);
            $.blockUI({ message: '<img src="../img/loadIcon/ajax-loader.gif" />' });
            selectedSubMenu = $(this).attr('href');
            if (selectedSubMenu == "#equatorSPCMenu") {
                getSerialNumberOperationDetails();
                getSerialNumberManualInpsectionDetails();
            }
            else if (selectedSubMenu == "#CMMDataFile") {
                bindCMMFileData();
            }
            //else if (selectedSubMenu == "#manualInpectionMenu") {
            //    getSerialNumberManualInpsectionDetails();
            //}
            //else {

            //}
            $.unblockUI({}); $('.ajax-loader').hide();
        });
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        function openErrorModal(msg) {
            $('#lblErrorMsg').text(msg);
            $('[id*=errorModal]').modal('show');
        }
        function allowNumber(evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode == 45 && pos != 0) {
                return false;
            } else if (charCode == 43 && pos != 0) {
                return false;
            } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                return false;
            } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                return false;
            }
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                $.unblockUI({}); $('.ajax-loader').hide();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
