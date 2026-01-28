<%@ Page Title="PowerProfile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PowerProfile.aspx.cs" Inherits="Web_TPMTrakDashboard.EnergyModule.PowerProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%--<script src="https://code.highcharts.com/stock/highstock.js"></script>--%>
    <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <%--<script src="Scripts/jquery-3.4.1.js"></script>--%>
    <%--  <script src="Scripts/JavaScriptUIBlocker.js"></script>--%>
    <%-- <script src="Scripts/DateTimePicker/moment.js"></script>
    <script src="Scripts/DateTimePicker/bootstrap-datetimepicker.js"></script>
    <link href="Scripts/DateTimePicker/bootstrap-datetimepicker.css" rel="stylesheet" />--%>
    <div class="container-fluid">
        <div class="row">
            <%-- <asp:Label runat="server" ID="lblForProfile" Text="POWER PROFILE - FANUC" CssClass="col-lg-12 col-md-12 col-sm-12" Style="color: white; font-size: 24px; margin: 10px; font-weight: bold; width: 100%; text-align: center;" />--%>
            <div class="col-lg-12 col-md-12 col-sm-12">

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <fieldset class="masterFS">
                            <legend>Selection Criteria</legend>
                            <table class="filterTable">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="From" ID="lblFromDateTime" CssClass=" selectlbl" Style="margin-right: 10px;" />
                                    </td>
                                    <td class="input-group" runat="server" style="margin-top:6px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDateTime" runat="server" CssClass="form-control date" placeholder="From Date Time" MaxLength="15" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td style="padding: 10px;">
                                        <asp:DropDownList runat="server" ID="ddlMachineType" CssClass="form-control specInputField" Style="margin: 0 10px 0 0;" OnSelectedIndexChanged="ddlMachineType_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="Machine EM" Text="Machining" />
                                            <asp:ListItem Value="Non-Machine EM" Text="Non-Machining" />
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdPlant" runat="server" style="padding: 10px;" class="selectlbl">Plant</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td id="tdCell" runat="server" style="padding: 10px;" class="selectlbl">Cell</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="Machine" ID="lblMachine" CssClass=" selectlbl" Style="padding: 10px;" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMachine" AutoPostBack="false" CssClass="form-control specInputField" Style="margin: 0 10px 0 0; min-width: 100px;">
                                        </asp:DropDownList>
                                    </td>

                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rbl" RepeatLayout="Flow" RepeatDirection="Horizontal" Style="vertical-align: middle;" OnSelectedIndexChanged="rbl_SelectedIndexChanged" AutoPostBack="true" ForeColor="White">
                                            <asp:ListItem Value="History" Text="History" Selected="True" />
                                            <asp:ListItem Value="LIve" Text="LIve" />
                                        </asp:RadioButtonList>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="To" ID="lblToDateTime" CssClass=" selectlbl" Style="margin-right: 10px;" />
                                    </td>
                                    <td class="input-group" runat="server">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDateTime" runat="server" CssClass="form-control date" placeholder="To Date Time" MaxLength="15" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td style="display: none">
                                        <asp:Label runat="server" Text="Duration" ID="Label1" CssClass=" selectlbl" Style="margin: 0 10px 0 0;" />
                                    </td>
                                    <td style="display: none">
                                        <asp:DropDownList runat="server" ID="ddlDurationFormat" AutoPostBack="true" OnSelectedIndexChanged="ddlDurationFormat_SelectedIndexChanged" CssClass="form-control specInputField" Style="margin: 0 10px 0 0;">
                                            <asp:ListItem Value="Hour" Text="Hour" />
                                            <asp:ListItem Value="Minute" Text="Minute" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="display: none">
                                        <asp:DropDownList runat="server" ID="ddlDurationValue" AutoPostBack="true" CssClass="form-control specInputField" Style="margin: 0 10px 0 0;" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnProcess" Text="Process" OnClientClick="btnProcessClicked()" CssClass="btn btn-info btn-sm" Style="margin-left: 10px;" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <asp:Timer ID="timerToAutoRefresh" runat="server" Enabled="False" OnTick="timerToAutoRefresh_Tick"></asp:Timer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divPowerProfileCharts" class="col-lg-12 col-md-12 col-sm-12" style="">
                <div id="divWatt" style="margin: 3px auto; height: 400px; width: 100%; background-color: aliceblue; border-bottom: 2px solid black; min-height: 400px">
                </div>
                <div id="divAmpere" style="margin: 3px auto; height: 400px; width: 100%; background-color: aliceblue; border-bottom: 2px solid black; min-height: 400px">
                </div>
                <div id="divVolt" style="margin: 3px auto; height: 400px; width: 100%; background-color: aliceblue; border-bottom: 2px solid black; min-height: 400px">
                </div>
                <div id="divPF" style="margin: 3px auto; height: 400px; width: 100%; background-color: aliceblue; border-bottom: 2px solid black; min-height: 400px">
                </div>
            </div>
        </div>
    </div>
    <style>
        fieldset {
            /*border: 1px solid #2B7B78;*/
            padding: 0px;
            border-radius: 4px;
            width: auto;
            border: 1px solid #ece3e3;
            /*box-shadow: 2px 2px 8px 2px #efe7e7;*/
        }

        .masterFS {
            padding: 0 5px 10px 5px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            border-radius: 7px;
        }

        .selectlbl {
            color: white;
            font-weight: bold;
        }

        .filterTable {
            width: auto;
            border-collapse: separate;
            border-spacing: 0 3px;
        }

        label {
            display: inline-block;
            max-width: 100%;
            margin-bottom: 5px;
            font-weight: bold;
            padding: 5px;
        }

        legend {
            text-align: left;
            color: white;
            display: block;
            width: auto;
            padding: 0;
            margin-bottom: 5px;
            line-height: inherit;
            border-bottom: transparent;
            font-size: 15px;
            font-weight: bold;
        }

        .radioBox {
            display: inline-table;
            position: relative;
            padding-left: 25px;
            margin-bottom: 12px;
            cursor: pointer;
            font-size: 12px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            /* Hide the browser's default radio button */
            .radioBox input {
                position: absolute;
                opacity: 0;
                cursor: pointer;
            }

        /* Create a custom radio button */
        .checkmark {
            position: absolute;
            top: 0;
            left: 0;
            height: 15px;
            width: 15px;
            background-color: #eee;
            border-radius: 50%;
        }

        /* On mouse-over, add a grey background color */
        .radioBox:hover input ~ .checkmark {
            background-color: #ccc;
        }

        /* When the radio button is checked, add a blue background */
        .radioBox input:checked ~ .checkmark {
            background-color: #2196F3;
        }

        /* Create the indicator (the dot/circle - hidden when not checked) */
        .checkmark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the indicator (dot/circle) when checked */
        .radioBox input:checked ~ .checkmark:after {
            display: block;
        }

        /* Style the indicator (dot/circle) */
        .radioBox .checkmark:after {
            top: 4px;
            left: 4px;
            width: 6px;
            height: 6px;
            border-radius: 50%;
            background: white;
        }
    </style>
    <script>
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        function compareDates(date1, date2) {
            return invert(date1).localeCompare(invert(date2));
        }

        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }
        function resize() {

            var heights = window.innerHeight;
            var widths = window.innerWidth;
            document.getElementById("divPowerProfileCharts").style.height = heights + "px";
        }
        function btnProcessClicked() {
            debugger;
            var fromDate = $("[id$=txtFromDateTime]").val();
            var toDate = $("[id$=txtToDateTime]").val();
            var fromDate1 = fromDate.substring(0, 10);
            var toDate1 = toDate.substring(0, 10);
            var diffe = dateDiffInDays(new Date(fromDate1.split("-").reverse().join("-")), new Date(toDate1.split("-").reverse().join("-")));
            var dateCom = compareDates(fromDate1, toDate1);
            if (dateCom == 1) {
                alert("From-Date Cannot Be greater than To-Date.");
                $("[id$=txtToDateTime]").focus();
                return false;
            }
            if (diffe > 1) {
                alert("Sorry, you can view Data for only 24 Hours.");
                return false;
            }
            var selectedMachineId = $("[id$=ddlMachine]").val();
            var durationType = $("[id$=ddlDurationFormat]").val();
            var durationVal = $("[id$=ddlDurationValue]").val();
            //$('[id$=lblForProfile]').html("POWER PROFILE - " + selectedMachineId);
            PloatAllCharts(fromDate, toDate, selectedMachineId, durationType, durationVal);
        }

        $(document).ready(function () {
            resize();
            window.onresize = function () {
                resize();
            };
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: 'en-US'
            });
            $('.date').each(function () {
                $(this).on('dp.change', function (ev) {

                    $(this).data('DateTimePicker').hide();

                });
            });

            var fromDate = $("[id$=txtFromDateTime]").val();
            var toDate = $("[id$=txtToDateTime]").val();
            var selectedMachineId = $("[id$=ddlMachine]").val();
            var durationType = $("[id$=ddlDurationFormat]").val();
            var durationVal = $("[id$=ddlDurationValue]").val();
            PloatAllCharts(fromDate, toDate, selectedMachineId, durationType, durationVal);

        });

        function PloatAllCharts(fromDate, toDate, selectedMachineId, durationType, durationVal) {
            debugger;
            $.blockUI({ message: '<img src="Images/ajax-loader.gif" />' });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PowerProfile.aspx/GetWattData",
                data: '{fromDate:"' + fromDate + '", toDate:"' + toDate + '", selectedMachineId:"' + selectedMachineId + '", durationType:"' + durationType + '", durationVal:"' + durationVal + '"}',
                dataType: "json",
                success: WattLineChart,
                error: function (Result) {
                    alert("Error");
                }
            });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PowerProfile.aspx/GetAmpereData",
                data: '{fromDate:"' + fromDate + '", toDate:"' + toDate + '", selectedMachineId:"' + selectedMachineId + '", durationType:"' + durationType + '", durationVal:"' + durationVal + '"}',
                dataType: "json",
                success: AmpereLineChart,
                error: function (Result) {
                    alert("Error");
                }
            });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PowerProfile.aspx/GetVoltData",
                data: '{fromDate:"' + fromDate + '", toDate:"' + toDate + '", selectedMachineId:"' + selectedMachineId + '", durationType:"' + durationType + '", durationVal:"' + durationVal + '"}',
                dataType: "json",
                success: VoltLineChart,
                error: function (Result) {
                    alert("Error");
                }
            });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PowerProfile.aspx/GetPFData",
                data: '{fromDate:"' + fromDate + '", toDate:"' + toDate + '", selectedMachineId:"' + selectedMachineId + '", durationType:"' + durationType + '", durationVal:"' + durationVal + '"}',
                dataType: "json",
                success: PFLineChart,
                error: function (Result) {
                    alert("Error");
                }
            });
            $.unblockUI({});
        }

        function WattLineChart(data) {
            Highcharts.stockChart('divWatt', {
                credits: {
                    enabled: false
                },
                rangeSelector: {
                    verticalAlign: 'top',
                    x: 0,
                    y: 0,
                    buttons: [
                        {
                            type: 'hour',
                            count: 2,
                            text: '2h'
                        },
                        {
                            type: 'hour',
                            count: 5,
                            text: '5h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'hour',
                            count: 20,
                            text: '20h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'All'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                title: {
                    text: "Watt",
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 60000,
                    minRange: 5,
                    ordinal: false,
                    dateTimeLabelFormats: {
                        month: '%e. %b',
                        year: '%b'
                    },
                    categories: data.d.Category,

                    plotLines: [{
                        color: '#FF0000',
                        width: 2,
                        value: 5.5
                    }]
                },
                series: data.d.series,
            });
        };

        function AmpereLineChart(data) {
            Highcharts.stockChart('divAmpere', {
                credits: {
                    enabled: false
                },
                title: {
                    text: "Ampere",
                },
                rangeSelector: {
                    verticalAlign: 'top',
                    x: 0,
                    y: 0,
                    buttons: [
                        {
                            type: 'hour',
                            count: 2,
                            text: '2h'
                        },
                        {
                            type: 'hour',
                            count: 5,
                            text: '5h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'hour',
                            count: 20,
                            text: '20h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'All'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 60000,
                    minRange: 5,
                    ordinal: false,
                    dateTimeLabelFormats: {
                        month: '%e. %b',
                        year: '%b'
                    },
                    categories: data.d.Category,

                    plotLines: [{
                        color: '#FF0000',
                        width: 2,
                        value: 5.5
                    }]
                },
                series: data.d.series,
            });
        };

        function VoltLineChart(data) {
            Highcharts.stockChart('divVolt', {
                credits: {
                    enabled: false
                },
                title: {
                    text: "Volt",
                },
                rangeSelector: {
                    verticalAlign: 'top',
                    x: 0,
                    y: 0,
                    buttons: [
                        {
                            type: 'hour',
                            count: 2,
                            text: '2h'
                        },
                        {
                            type: 'hour',
                            count: 5,
                            text: '5h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'hour',
                            count: 20,
                            text: '20h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'All'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 60000,
                    minRange: 5,
                    ordinal: false,
                    dateTimeLabelFormats: {
                        month: '%e. %b',
                        year: '%b'
                    },
                    categories: data.d.Category,

                    plotLines: [{
                        color: '#FF0000',
                        width: 2,
                        value: 5.5
                    }]
                },
                series: data.d.series,
            });
        };

        function PFLineChart(data) {
            Highcharts.stockChart('divPF', {
                credits: {
                    enabled: false
                },
                rangeSelector: {
                    verticalAlign: 'top',
                    x: 0,
                    y: 0,
                    buttons: [
                        {
                            type: 'hour',
                            count: 2,
                            text: '2h'
                        },
                        {
                            type: 'hour',
                            count: 5,
                            text: '5h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'hour',
                            count: 20,
                            text: '20h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'All'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                title: {
                    text: "PF",
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 60000,
                    minRange: 5,
                    ordinal: false,
                    dateTimeLabelFormats: {
                        month: '%e. %b',
                        year: '%b'
                    },
                    categories: data.d.Category,

                    plotLines: [{
                        color: '#FF0000',
                        width: 2,
                        value: 5.5
                    }]
                },
                series: data.d.series,
            });
        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            resize();
            window.onresize = function () {
                resize();
            };
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: 'en-US'
            });
            $('.date').each(function () {
                $(this).on('dp.change', function (ev) {

                    $(this).data('DateTimePicker').hide();

                });
            });
        });
    </script>
</asp:Content>
