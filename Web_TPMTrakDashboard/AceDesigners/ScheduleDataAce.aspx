<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleDataAce.aspx.cs" Inherits="Web_TPMTrakDashboard.AceDesigners.ScheduleDataAce" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>--%>
    <script src="../MyCssAndJS/DatePicker2/moment.js"></script>
    <script src="../MyCssAndJS/DatePicker2/bootstrap-datetimepicker.min.js"></script>
    <link href="../MyCssAndJS/DatePicker2/bootstrap-datetimepicker.min.css" rel="stylesheet" />


    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        fieldset {
            border: 1px solid white !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            margin: 0px;
            vertical-align: top;
        }

        legend {
            font-size: 1.1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            color: white;
            border-bottom: none;
            margin-top: -4px;
            margin: 0px;
        }

        #tblScheduleData tr td {
            background-color: #FFFFFF;
            color: black;
            vertical-align: middle;
        }
        .modal
        {
            overflow-y: unset !important;
        }
        .table thead>tr>th, .table tbody>tr>th, .table tfoot>tr>th, .table thead>tr>td, .table tbody>tr>td, .table tfoot>tr>td {
            padding: 8px;
            line-height: 1.428571429;
            vertical-align: top;
            border-top: unset;
        }
    </style>
    <div class="container-fluid" style="">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <asp:HiddenField runat="server" ID="hdnViewType" ClientIDMode="Static" />
                <table id="tblFilter" class="" style="width: auto; margin-left: 10px;">
                    <tr>
                        <td class="commanTd" style="vertical-align: middle;">Schedule Status&nbsp;</td>
                        <td>
                            <asp:ListBox ID="lbStatus" CssClass="form-control" runat="server" SelectionMode="Multiple" ToolTip=" Status" ClientIDMode="Static">
                                <asp:ListItem Text="New" Value="New" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Running" Value="Running" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Hold" Value="Hold" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                            </asp:ListBox>
                        </td>
                        <td class="commanTd" style="vertical-align: middle;">&nbsp;&nbsp;SAP Status&nbsp;</td>
                        <td>
                            <asp:ListBox ID="lbSAPStatus" CssClass="form-control" runat="server" SelectionMode="Multiple" ToolTip=" Status" ClientIDMode="Static">
                                <asp:ListItem Text="REL" Value="REL" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="DLT" Value="DLT"></asp:ListItem>
                                <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                                <asp:ListItem Text="LKD" Value="LKD"></asp:ListItem>
                                <asp:ListItem Text="PCNF" Value="PCNF" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="CNF" Value="CNF"></asp:ListItem>
                                <asp:ListItem Text="DLV" Value="DLV"></asp:ListItem>
                            </asp:ListBox>
                        </td>
                        <td class="commanTd" style="vertical-align: middle;">&nbsp;&nbsp;Machine&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlMachine" runat="server" CssClass="form-control" />
                        </td>
                        <td class="commanTd" style="vertical-align: middle;">&nbsp;&nbsp;MRP Controller&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlMRPController" runat="server" CssClass="form-control" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Search by Scheduled Date</legend>
                                <table>
                                    <tr>
                                        <td class="commanTd" style="vertical-align: middle;">From</td>
                                        <td class="input-group" style="min-width: 130px; border: 0">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                        <td class="commanTd" style="vertical-align: middle;">To</td>
                                        <td class="input-group" style="min-width: 130px; border: 0">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnDateView" Text="View" Style="" OnClick="btnDateView_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <legend>Search by PO+Material ID</legend>
                                <table>
                                    <tr>
                                        <td class="commanTd" style="vertical-align: middle;">PO&nbsp;</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPOSearch" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td class="commanTd" style="vertical-align: middle;">&nbsp;&nbsp;Material ID&nbsp;</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCompSearch" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnCompPOView" Text="View" OnClick="btnCompPOView_Click" />
                                        </td>
                                    </tr>
                                </table>
                        </td>
                        <td>&nbsp;
                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnRefresh" Text="Refresh" OnClick="btnRefresh_Click" OnClientClick="return refreshClick();" />
                        </td>
                        <td>&nbsp;
                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSave" Text="Save" OnClick="btnSave_Click" OnClientClick="return saveValidation();" />&nbsp;
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSendToHMI" Text="Send To HMI" OnClick="btnSendToHMI_Click" OnClientClick="return sendToHMIValidation();" />
                        </td>
                    </tr>
                </table>


                <div id="gridContainer" style="margin-top: 10px; height: 73vh; overflow: auto">
                    <asp:HiddenField runat="server" ID="hdnSendToHMISelctedIDD" ClientIDMode="Static" />
                    <asp:ListView runat="server" ID="lvScheduleData" ItemPlaceholderID="itemplaceholder" OnItemDataBound="lvScheduleData_ItemDataBound">
                        <LayoutTemplate>
                            <table id="tblScheduleData" class="table table-bordered  headerFixer" style="width: 100%; background: white">
                                <tr>
                                    <th>Select</th>
                                    <th>Production Order</th>
                                    <th>Material ID</th>
                                    <th>Operation No.</th>
                                    <th>Std. Cycle Time</th>
                                    <th>Planned Qty.</th>
                                    <th>Delivered Qty.</th>
                                    <th>Scheduled Date</th>
                                    <th style="width: 100px">Scheduled Priority</th>
                                    <th style="width: 100px">Sequence</th>
                                    <th>Scheduled Status</th>
                                    <th>SAP Status</th>
                                    <th style="width: 100px">Send To HMI</th>
                                    <th>HMI Timestamp</th>
                                    <th>Development</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                    <asp:HiddenField runat="server" ID="hdnIDD" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                                    <asp:CheckBox runat="server" ID="chkSelect" ClientIDMode="Static" Enabled='<%# Eval("chkEnabled") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPO" ClientIDMode="Static" Text='<%# Eval("ProductionOrder") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("CompID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOpnNo" ClientIDMode="Static" Text='<%# Eval("OpnNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStdCycleTime" ClientIDMode="Static" Text='<%# Eval("StdCycleTime") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPlannedQty" ClientIDMode="Static" Text='<%# Eval("PlannedQty") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblCompletedQty" ClientIDMode="Static" Text='<%# Eval("CompletedQty") %>'></asp:Label>
                                </td>

                                <td class="input-group" style="min-width: 150px; border: 0">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtScheduleDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass='form-control updateControl scheduleDate ' placeholder="Date" AutoCompleteType="Disabled" Text='<%# Eval("ScheduleDate") %>' Enabled='<%# Eval("ControlEnabled") %>' onfocus="dateRestriction(this);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSchedulePriority" CssClass="form-control updateControl allowNumber" Text='<%# Eval("SchedulePriority") %>' Enabled='<%# Eval("ControlEnabled") %>' ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnOldSequence" ClientIDMode="Static" Value='<%# Eval("Sequence") %>' />
                                    <asp:TextBox runat="server" ID="txtSequence" CssClass="form-control updateControl allowNumber" Text='<%# Eval("Sequence") %>' Enabled='<%# Eval("ControlEnabled") %>' ClientIDMode="Static" onblur="sequenceValidation(this);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnStatus" ClientIDMode="Static" Value='<%# Eval("Status") %>' />
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control updateControl" style="width:120px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true" >
                                        <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                        <asp:ListItem Text="Running" Value="Running"></asp:ListItem>
                                        <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                                    </asp:DropDownList>
<%--                                <asp:Label runat="server" ID="lblStatus" ClientIDMode="Static" Text='<%# Eval("Status") %>'></asp:Label>--%>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label2" ClientIDMode="Static" Text='<%# Eval("SAPStatus") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text='<%# Eval("SendToHMI") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblHMIUpdatedTS" ClientIDMode="Static" Text='<%# Eval("UpdatedTsHMI") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="ChkJobType" CssClass="updateControl" ClientIDMode="Static" Checked='<%# Eval("ChkJobType") %>' Enabled='<%# Eval("chkEnabled") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

                 <div class="modal infoModal" id="newLoginModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                 <div class="modal-dialog modal-dialog-centered " style="width: 27vw;padding-top:193px;">
                  <div class="modal-content">
                    <div class="model-header" style="min-height:30px;background:#428bca;">
                        <h4 class="modal-title" id="modalTitle" runat="server" style="display:inline-block;"></h4>
                        <asp:HiddenField runat="server" ID="hdnStatusIDD" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnStatus" ClientIDMode="Static" />
<%--                    <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('newLoginModal');" style="margin-top:0px;float:right"></i>--%>
                    </div>
                    <div class="modal-body" style="padding-left: 0px; padding-right: 0px;text-align:center;">
                        <div style="overflow: auto;">
                            <table class="table">
                                <tr>
                                    <td style="min-width: 130px;">User Name</td>
                                    <td>
                                         <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" ClientIDMode="Static" Style="width: 200px"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>Password</td>
                                    <td>
                                         <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Style="width: 200px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <table class="table" style="margin-bottom:unset !important">
                            <tr>
                                <td colspan="2">
                                     <asp:Button runat="server" ID="btnLogin" class="btn btn-primary" Text="LOGIN" OnClick="btnLogin_Click" ClientIDMode="Static"/>
                                     <asp:Button runat="server" ID="btnclose" class="btn btn-primary"  Text="CANCEL" OnClick="btnclose_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                     <asp:Label ID="lblmsg" ClientIDMode="Static" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                      
                    </div>
                 </div>
                </div>
               </div>
            </ContentTemplate>
        </asp:UpdatePanel>
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
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            setControls();
        });
        function clearAllModalScreen() {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            $(".modal").modal("hide");
            $('body').removeClass('modal-open'); 
            return true;
        }

        function showWarningMsg(msg, title) {
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

            toastr['warning'](msg, title);
            return false;
        }

        function refreshClick() {
            $('[id*=hdnScrollPos]').val("");
            return true;
        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        function openModals(modalid) {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            storeModalDataBeforeChange(modalid);
            $('#' + modalid).modal('show');
        }
        function storeModalDataBeforeChange(modalid) {
            $('#' + modalid).modal('hide');
        }

        function sequenceValidation(txt) {
            debugger;
            var val = $(txt).val();
            if (val != "") {
                let sequence = parseInt(val);
                if (sequence < 0 || sequence > 10) {
                    $(txt).val("");
                    openWarningModal("Sequence should between 1-10.");
                }
            }
        }

        function sendToHMIValidation() {
            debugger;
            $('#hdnSendToHMISelctedIDD').val("");
            var rows = $('#tblScheduleData tr');
            if ($('#tblScheduleData input[type="checkbox"]:checked').length == 0) {
                openWarningModal("Please select record.");
                return false;
            }
            var sequenceList = [];
            var iddList = "";
            for (var i = 1; i < rows.length; i++) {
                var row = rows[i];
                if ($(row).find("#chkSelect").is(":checked") == true) {
                    /* var schedule = $(row).find("#txtSequence").val();*/
                    var schedule = $(row).find("#hdnOldSequence").val();
                    if (schedule != "") {
                        sequenceList.push(schedule);
                    }
                    if (iddList == "")
                        iddList += $(row).find("#hdnIDD").val();
                    else
                        iddList += "," + $(row).find("#hdnIDD").val();
                    //}
                }
            }
            var duplicates = sequenceList.filter((item, index) => index !== sequenceList.indexOf(item));
            if (duplicates.length > 0) {
                openWarningModal("Sequence " + duplicates[0] + " already exists.");
                return false;
            }
            for (var i = 0; i < sequenceList.length; i++) {
                var existenceData = priorityValidation("", sequenceList[i], "", "");
                if (existenceData.SequenceExists) {
                    openWarningModal("Sequence " + sequenceList[i] + " is Running.");
                    return false;
                }
            }
            $('#hdnSendToHMISelctedIDD').val(iddList);
            return true;
        }
        $('#tblScheduleData tr td .updateControl').focus(function () {
            $(this).closest("tr").find("#hdnUpdate").val("update");
        });

        $('#tblScheduleData tr td .updateControl #ChkJobType').focus(function () {
            $(this).closest("tr").find("#hdnUpdate").val("update");
        });

        function saveValidation() {
            var rows = $('#tblScheduleData tr');
            debugger;
            var list = [];
            //clientSideValidation();
            for (var i = 1; i < rows.length; i++) {
                var row = rows[i];
                if ($(row).find("#hdnUpdate").val() == "update") {
                    var productionOrder = $(row).find("#lblPO").text();
                    var priority = $(row).find("#txtSchedulePriority").val() == undefined ? "" : $(row).find("#txtSchedulePriority").val();
                    var scheduleDate = $(row).find("#txtScheduleDate").val() == undefined ? "" : $(row).find("#txtScheduleDate").val();
                    var sequence = $(row).find("#txtSequence").val() == undefined ? "" : $(row).find("#txtSequence").val();
                    var idd = $(row).find("#hdnIDD").val();

                    if (scheduleDate == "" && priority == "") {
                        $(row).find("#hdnUpdate").val("");
                        continue;
                    }
                    if (scheduleDate == "" || priority == "") {
                        if (scheduleDate == "") {
                            openWarningModal("Enter Scheduled Date for Production Order '" + productionOrder + "'");
                            return false;
                        }
                        if (priority == "") {
                            openWarningModal("Enter Priority for Production Order '" + productionOrder + "'");
                            return false;
                        }
                        //if (sequence == "") {
                        //    openWarningModal("Enter Sequence for Production Order '" + productionOrder + "'");
                        //    return false;
                        //}
                    }

                    if (idd != "" && priority != "" && scheduleDate != "" && idd != undefined && priority != undefined && scheduleDate != undefined) {
                        let data = { scheduleDate: scheduleDate, priority: priority, idd: idd };
                        list.push(data);
                    }
                }
            }

            var distScheduleDate = [...new Set(list.map(item => item.scheduleDate))];
            for (var i = 0; i < distScheduleDate.length; i++) {
                /*list.map(function (k) { return if (k.scheduleDate == 0) k.priority });*/
                var scheduleList = list.filter(k => k.scheduleDate == distScheduleDate[i]);
                var priorityList = [];
                //var sequenceList = [];
                for (var j = 0; j < scheduleList.length; j++) {
                    priorityList.push(scheduleList[j].priority);
                    //sequenceList.push(scheduleList[j].sequence);
                }
                var duplicates = priorityList.filter((item, index) => index !== priorityList.indexOf(item));
                if (duplicates.length > 0) {
                    openWarningModal("Priority " + duplicates[0] + " already exists.");
                    return false;
                }
                //duplicates = sequenceList.filter((item, index) => index !== sequenceList.indexOf(item));
                //if (duplicates.length > 0) {
                //    openWarningModal("Sequence " + duplicates[0] + " already exists.");
                //    return false;
                //}
            }

            for (var i = 0; i < list.length; i++) {
                var data = list[i];
                var existenceData = priorityValidation(data.priority, "", data.idd, data.scheduleDate);
                if (existenceData.PriorityExists) {
                    openWarningModal("Priority " + data.priority + " already exists.");
                    return false;
                }
                //if (existenceData.SequenceExists) {
                //    openWarningModal("Sequence " + data.sequence + " already exists.");
                //    return false;
                //}
            }
            return true;
        }
        function clientSideValidation() {
            var list = [];
            debugger;
            var rows = $('#tblScheduleData tr');
            for (var i = 1; i < rows.length; i++) {
                var row = rows[i];
                if ($(row).find("#hdnUpdate").val() == "update") {
                    let data = { scheduleDate: $(row).find("#txtScheduleDate").val(), priority: $(row).find("#txtSchedulePriority").val() };
                    list.push(data);
                }
            }

        }
        function priorityValidation(priority, sequence, idd, date) {
            var existenceData;
            $.ajax({
                async: false,
                type: "POST",
                url: "ScheduleDataAce.aspx/isPriorityExists",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{priority:"' + priority + '",sequence:"' + sequence + '",idd:"' + idd + '",scheduleDate:"' + date + '"}',
                success: function (response) {
                    existenceData = response.d;

                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            return existenceData;
        }
        function setControls() {
            $('[id$=lbStatus]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbSAPStatus]').multiselect({
                includeSelectAllOption: true
            });
            $('.date1').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US',
            });
            $('.scheduleDate').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US',
                //useCurrent: false,
            });
            $(document).keypress(function (e) {
                if ($("#newLoginModal").hasClass('in') && (e.keycode == 13 || e.which == 13)) {
                    $('#btnLogin').click();
                    return false;
                }
            });
        }
        function dateRestriction(txt) {
            $(txt).data("DateTimePicker").minDate(new Date());
        }
        function openSuccessModal(msg) {
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
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        $('.allowNumber').keypress(function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
            $('#tblScheduleData tr td .updateControl').focus(function () {
                $(this).closest("tr").find("#hdnUpdate").val("update");
            });
            $('#tblScheduleData tr td .updateControl #ChkJobType').focus(function () {
                $(this).closest("tr").find("#hdnUpdate").val("update");
            });

            $('.allowNumber').keypress(function (evt) {
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode < 48 || charCode > 57) {
                    return false;
                }
                return true;
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
