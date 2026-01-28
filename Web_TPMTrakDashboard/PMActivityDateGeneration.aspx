<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMActivityDateGeneration.aspx.cs" Inherits="Web_TPMTrakDashboard.PMActivityDateGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
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

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }

        .headerFixerhere tr:nth-child(2n+1) td:nth-child(3), .headerFixerhere tr:nth-child(2n) td:nth-child(3) {
            position: sticky;
            left: 0px;
            z-index: 1;
            background-color: #202648;
        }

        .headerFixerhere tr:first-child th:nth-child(3) {
            position: sticky;
            left: 0px;
            z-index: 6;
            background-color: #5391CA;
        }

        .bootstrap-datetimepicker-widget table td {
            /*background-color: #5391CA !important;*/
            color: black !important;
        }

        .table-condensed > thead > tr > th:hover, .table-condensed > tbody > tr > th:hover, .table-condensed > tfoot > tr > th:hover, .table-condensed > thead > tr > td:hover, .table-condensed > tbody > tr > td:hover, .table-condensed > tfoot > tr > td:hover {
            background-color: #5391CA !important;
        }

        fieldset {
            border: 1px solid white !important;
            padding: 0px 5px 5px 5px !important;
            margin: 0 0 8px 0 !important;
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
    </style>
    <div>
        <div>
            <table style="display: inline-block;" class="table-style">
                <tr>


                    <td style="text-align: center; vertical-align: central; align-content: center;">Machine ID:
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" Width="140px" ClientIDMode="Static" />
                    </td>
                    <td style="text-align: center; vertical-align: central; align-content: center;">Frequency:
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Width="140px" ClientIDMode="Static" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <fieldset class="masterFS" style="margin-left: 5px !important; margin-right: 4px !important; display: inline-block">
                            <legend>View</legend>
                            <table class="table-style">
                                <tr>
                                    <td style="text-align: center; vertical-align: central; align-content: center;">From Date:
                                    </td>

                                    <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar" style="color: black"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date restrictDateControl" placeholder="Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td style="text-align: center; vertical-align: central; align-content: center;">ToDate:
                                    </td>

                                    <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar" style="color: black;"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date restrictDateControl" placeholder="Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="btn btn-info" Text="View" Width="80" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="masterFS" style="margin-left: 5px !important; margin-right: 4px !important; display: inline-block">
                            <legend>Generate</legend>
                            <table class="table-style">
                                <tr>
                                    <td style="vertical-align: central; align-content: center;">Year:
                                    </td>
                                    <td style="width: 120px">
                                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" Style="width: 100%; display: inline;" ClientIDMode="Static" onblur="txtYearBlur();" onfocus="txtYearFocus();"></asp:TextBox>
                                    </td>
                                    <td style="text-align: center; vertical-align: central; align-content: center;">Start Date: 
                                    </td>
                                    <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar" style="color: black;"></i>
                                        </div>
                                        <asp:TextBox ID="txtStartDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date restrictDateControl" placeholder="Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>

                                        <asp:Button runat="server" ID="btnGenerate" OnClick="btnGenerate_Click" OnClientClick="if(!generateConfirmation()){return false};" CssClass="btn btn-info" Text="Generate" />

                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>

        </div>
        <div style="width: 100%; height: 75vh; overflow: auto">
            <asp:GridView ID="gvActvityTiming" runat="server" Width="100%" EmptyDataText="No Data Found." ShowHeaderWhenEmpty="true" ShowHeader="true" ClientIDMode="Static" CssClass="table table-bordered table-hover headerFixer bajaj-table-style">
            </asp:GridView>
        </div>


        <div class="modal infoModal" id="activityTimingUpdateModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered " style="width: 25%">
                <div class="modal-content modalThemeCss">
                    <div class="modal-header">
                        <h4 class="modal-title" runat="server">Enter New Date</h4>
                        <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right;"></i>
                        <%--position: relative; top: -21px;--%>
                    </div>
                    <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                        <span style="color: red; margin-left: 20px;" class="mandatory-message"></span>
                        <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;" class="div-border-style">
                            <table style="width: 100%; margin: auto" class="modal-tbl">
                                <tr>
                                    <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtNewDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdnActivityTimingUniqueCode" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hdnActivityTimingMachine" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hdnActivityTimingActive" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hdnActivityTimingOldDate" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hdnActivityTimingShiftID" ClientIDMode="Static" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnActivityTimingDateUpdate" ClientIDMode="Static" Text="Save" CssClass="btn btn-info" OnClientClick="return activityTimingNewDateUpdateValidation();" OnClick="btnActivityTimingDateUpdate_Click" />
                        <button type="button" data-dismiss="modal" class="btn btn-info">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal infoModal" id="activityHeaderTimingUpdateModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered " style="width: 25%">
                <div class="modal-content modalThemeCss">
                    <div class="modal-header">
                        <h4 class="modal-title" runat="server">Enter New Date</h4>
                        <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right;"></i>
                        <%--position: relative; top: -21px;--%>
                    </div>
                    <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                        <span style="color: red; margin-left: 20px;" class="mandatory-message"></span>
                        <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;" class="div-border-style">
                            <table style="width: 100%; margin: auto" class="modal-tbl">
                                <tr>
                                    <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtNewHeaderDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdnDateVal" ClientIDMode="Static" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnHeaderActivityTimingDateUpdate" ClientIDMode="Static" Text="Save" CssClass="btn btn-info" OnClientClick="return activityHeaderTimingNewDateUpdateValidation();" OnClick="btnActivityTimingDateUpdate_Click" />
                        <button type="button" data-dismiss="modal" class="btn btn-info">Close</button>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <script>
        $('[id$=txtYear]').datepicker({
            minViewMode: 2,
            format: 'yyyy',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });
        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtToDate]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtStartDate]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtNewDate]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtNewHeaderDate]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        var YearWhenFocus = "";
        var fromDateFocus = "";
        var toDateFocus = "";
        addUpdateIcon();
        showAndHideColumns();
        txtYearBlur();
        function addUpdateIcon() {
            var ths = $('#gvActvityTiming tr th');
            var appendStr = '&nbsp;&nbsp;<i class="glyphicon glyphicon-edit update"></i>';
            if ($('#ddlFrequency option:selected').text() != "Daily" && $('#ddlFrequency option:selected').text() != "Shift") {
                for (var i = 9; i < ths.length; i++) {
                    $(ths[i]).append(appendStr);
                }
            }
        }

        function txtYearFocus() {
            YearWhenFocus = $('#txtYear').val();
            toDateFocus = $("[id$=txtFromDate]").val();
        }
        function txtYearBlur() {
            debugger;
            //$("[id$=txtFromDate]").data("DateTimePicker").maxDate("01-01-9999");
            //$("[id$=txtFromDate]").data("DateTimePicker").minDate("01-01-1000");
            //$("[id$=txtToDate]").data("DateTimePicker").maxDate("01-01-9999");
            //$("[id$=txtToDate]").data("DateTimePicker").minDate("01-01-1000");
            $("[id$=txtStartDate]").data("DateTimePicker").maxDate("01-01-9999");
            $("[id$=txtStartDate]").data("DateTimePicker").minDate("01-01-1000");
            //$("[id$=txtNewHeaderDate]").data("DateTimePicker").maxDate("01-01-9999");
            //$("[id$=txtNewHeaderDate]").data("DateTimePicker").minDate("01-01-1000");
            if (YearWhenFocus != "") {
                //$("[id$=txtFromDate]").attr("value", "");
                //$("[id$=txtToDate]").attr("value", "");
                $("[id$=txtStartDate]").attr("value", "");
                //$("[id$=txtNewHeaderDate]").attr("value", "");
            }
            setTimeout(function () {
                var year = $('#txtYear').val();
                var minDate = "01-01-" + year;
                var maxDate = "31-12-" + year;

                if (YearWhenFocus != "") {
                    //$("[id$=txtFromDate]").attr("value", minDate + " 00:00");
                    //$("[id$=txtToDate]").attr("value", minDate + " 00:00");
                    $("[id$=txtStartDate]").attr("value", minDate + " 00:00");
                    //$("[id$=txtNewHeaderDate]").attr("value", minDate + " 00:00");
                }

                //$("[id$=txtFromDate]").data("DateTimePicker").maxDate(maxDate);
                //$("[id$=txtFromDate]").data("DateTimePicker").minDate(minDate);
                //$("[id$=txtToDate]").data("DateTimePicker").maxDate(maxDate);
                // $("[id$=txtToDate]").data("DateTimePicker").minDate(minDate);
                $("[id$=txtStartDate]").data("DateTimePicker").maxDate(maxDate);
                $("[id$=txtStartDate]").data("DateTimePicker").minDate(minDate);
                //$("[id$=txtNewHeaderDate]").data("DateTimePicker").maxDate(maxDate);
                //$("[id$=txtNewHeaderDate]").data("DateTimePicker").minDate(minDate);

                if (YearWhenFocus != "") {
                    //$("[id$=txtFromDate]").attr("value", minDate + " 00:00");
                    //$("[id$=txtToDate]").attr("value", minDate + " 00:00");
                    //$("[id$=txtStartDate]").attr("value", minDate + " 00:00");

                }

                debugger;
                //if (YearWhenFocus != "") {
                //    console.log($("[id$=txtToDate]").val());
                //    let fromDate = $("[id$=txtFromDate]").val();
                //    let toDate = $("[id$=txtToDate]").val();
                //    let startDate = $("[id$=txtStartDate]").val();
                //    //fromDate = fromDate.replace(YearWhenFocus, year);
                //    //toDate = toDateFocus.replace(YearWhenFocus, year);
                //    // startDate = startDate.replace(YearWhenFocus, year);


                //    //$("[id$=txtFromDate]").val(fromDate);
                //    //$("[id$=txtToDate]").attr("value", toDate);
                //    //$("[id$=txtToDate]").val(toDate);
                //    //$("[id$=txtStartDate]").val(startDate);
                //}



            }, 100);
        }
        function showAndHideColumns() {
            $('#gvActvityTiming tr th:nth-child(5)').hide();
            $('#gvActvityTiming tr td:nth-child(5)').hide();
            if ($('#ddlFrequency option:selected').text() == "Shift") {
                $('#gvActvityTiming tr td:nth-child(8)').show();
                $('#gvActvityTiming tr th:nth-child(8)').show();
                $('#gvActvityTiming tr td:nth-child(9)').show();
                $('#gvActvityTiming tr th:nth-child(9)').show();
            }
            else {
                $('#gvActvityTiming tr td:nth-child(8)').hide();
                $('#gvActvityTiming tr th:nth-child(8)').hide();
                $('#gvActvityTiming tr td:nth-child(9)').hide();
                $('#gvActvityTiming tr th:nth-child(9)').hide();
            }
        }

        $('#gvActvityTiming tr th .update').click(function () {
            $("#activityHeaderTimingUpdateModal").modal('show');
            $("#hdnDateVal").val($(this).parent().index());
        });

        function activityHeaderTimingNewDateUpdateValidation() {
            //if ($('#txtYear').val() == "") {
            //    alert("Please select Year.");
            //    return false;
            //}
            if ($('#ddlFrequency').val() == null || $('#ddlFrequency').val() == "") {
                alert("Please select Frequency.");
                return false;
            }
            if ($('#txtNewHeaderDate').val() == "") {
                alert("Please select New Date.");
                return false;
            }
            if ($('#ddlMachineID').val() == null || $('#ddlMachineID').val() == "") {
                alert("Please select Machine ID.");
                return false;
            }
            var count = 0;
            var ActNotUpdated = "";
            var rows = $("#gvActvityTiming tr");
            for (var i = 1; i < rows.length; i++) {
                var row = $(rows)[i];
                var selectedMachineid = $(row).find('td').eq(1).text();
                var selectedActivity = $(row).find('td').eq(2).text();
                var selectedUniqueCode = $(row).find('td').eq(3).text();
                var shiftID = $(row).find('td').eq(7).text().trim();
                var selectedOlDate = $(row).find('td').eq($("#hdnDateVal").val()).text();

                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("PMActivityDateGeneration.aspx/activityTimingsUpdationValidation") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    data: '{year: "' + $('#txtYear').val() + '",activity: "' + selectedActivity + '",frequency:"' + $('#ddlFrequency').val() + '",frequencyName:"' + $('#ddlFrequency option:selected').text() + '", oldDateTime:"' + selectedOlDate + '",newDateTime:"' + $('#txtNewHeaderDate').val() + '", machineid:"' + selectedMachineid + '", uniqueCode:"' + selectedUniqueCode + '", main_MachineID:"' + $('#ddlMachineID').val() + '", shiftID:"' + shiftID + '"}',
                    dataType: "json",
                    success: function (response) {
                        var message = response.d;
                        if (message == "Updated") {
                            count = count + 1;
                        }
                        else if (message == "Failed") {
                            count = count + 0;
                        }
                        else {
                            //ActNotUpdated = ActNotUpdated + selectedActivity + "- " + message + "\n";
                            ActNotUpdated = message;
                        }
                    },
                    error: function (Result) {
                        alert("Error" + Result);
                    }
                });
            }
            //ActNotUpdated = ActNotUpdated.slice(0, -1);
            if (rows.length - 1 == count) {
                alert('Record updated Successfully.', '');
            }
            else if (rows.length - 1 != count) {
                alert(ActNotUpdated + " \nDate not updated");
            }
            else {
                alert('Failed to update data.');
            }
            $(".modal-backdrop").removeClass("modal-backdrop in");
            return true;
        }

        function generateConfirmation() {
            let isConfirmed = confirm("Delete existing data and generate information?");
            return isConfirmed;
        }
        $('#gvActvityTiming tr td').click(function () {
            //if ($(this).index() >= 9) {
            //    var selectedUniqueCode = $(this).closest('tr').find('td')[1].textContent;
            //    // var selectedMachineID = $(this).closest('tr').find('td')[4].textContent;
            //    var selctedActivity = $(this).closest('tr').find('td')[2].textContent;
            //    var shiftID = $(this).closest('tr').find('td')[7].textContent.trim();
            //    var selctedOldDate = $(this).text();
            //    $('#hdnActivityTimingUniqueCode').val(selectedUniqueCode);
            //    // $('#hdnActivityTimingMachine').val(selectedMachineID);
            //    $('#hdnActivityTimingActive').val(selctedActivity);
            //    $('#hdnActivityTimingOldDate').val(selctedOldDate);
            //    $('#hdnActivityTimingShiftID').val(shiftID);
            //    $('#txtNewDate').val(selctedOldDate);
            //    openActivityTimingUpdateModal();
            //}
        });
        function openActivityTimingUpdateModal() {
            $("#activityTimingUpdateModal").modal('show');
        }
        function activityTimingNewDateUpdateValidation() {
            if ($('#txtYear').val() == "") {
                alert("Please select Year.");
                return false;
            }
            if ($('#ddlFrequency').val() == null || $('#ddlFrequency').val() == "") {
                alert("Please select Frequency.");
                return false;
            }
            if ($('#txtNewDate').val() == "") {
                alert("Please select New Date.");
                return false;
            }
            if ($('#ddlMachineID').val() == null || $('#ddlMachineID').val() == "") {
                alert("Please select Machine ID.");
                return false;
            }
            var selectedMachineid = $('#hdnActivityTimingMachine').val();
            var selectedActivity = $('#hdnActivityTimingActive').val();
            var selectedUniqueCode = $('#hdnActivityTimingUniqueCode').val();
            var selectedOlDate = $('#hdnActivityTimingOldDate').val();
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: '<%= ResolveUrl("PMActivityDateGeneration.aspx/activityTimingsUpdationValidation") %>',
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                data: '{year: "' + $('#txtYear').val() + '",activity: "' + selectedActivity + '",frequency:"' + $('#ddlFrequency').val() + '",frequencyName:"' + $('#ddlFrequency option:selected').text() + '", oldDateTime:"' + selectedOlDate + '",newDateTime:"' + $('#txtNewDate').val() + '", machineid:"' + selectedMachineid + '", uniqueCode:"' + selectedUniqueCode + '", main_MachineID:"' + $('#ddlMachineID').val() + '", shiftID:"' + $('#hdnActivityTimingShiftID').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var message = response.d;
                    if (message == "Updated") {
                        alert('Record updated Successfully.', '');
                    }
                    else if (message == "Failed") {
                        alert('Failed to update data.');
                    }
                    else {
                        alert(message);
                    }
                },
                error: function (Result) {
                    alert("Error" + Result);
                }
            });
            $(".modal-backdrop").removeClass("modal-backdrop in");
            return true;
        }
        $(document).keypress(function (e) {
            if ($("#activityTimingUpdateModal").hasClass('in') && (e.keycode == 13 || e.which == 13)) {
                $("#btnActivityTimingDateUpdate").trigger("click");
                return false;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
