<%@ Page Language="C#" Title="Process Parameter Dashboard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.ProcessParameterDashboard" meta:resourcekey="PageResource1" Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%--<script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>--%>
    <%--<script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>--%>
    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/stock/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/no-data-to-display.js"></script>
    <%--    <script src="https://code.jquery.com/jquery-3.1.1.min.js"></script>--%>
    <script src="https://code.highcharts.com/modules/data.js"></script>


    <style>
        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        .chartHeader {
            background-color: #2E6886 !important;
        }

        .tbl th {
            width: 10%;
        }

        .tbl td {
            width: 10%;
        }

        .displayCss {
            display: inline;
        }

        .footerCss {
            background-color: #2E6886 !important;
        }

        .ajax-loader {
            display: none;
            background-color: rgba(0, 0, 0, 0.6);
            position: absolute;
            z-index: +100 !important;
            width: 100%;
            height: 100%;
            margin-left: -15px;
            margin-top: -16px;
        }

        #load-div {
            position: fixed;
            padding-right: 100px;
            width: 30%;
            top: 40%;
            left: 35%;
            text-align: center;
            border: 3px solid rgb(170, 170, 170);
            background-color: rgb(255, 255, 255);
            cursor: wait;
        }

        .ajax-loader img {
            position: relative;
            left: 50%;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        text {
            text-decoration: none !important;
        }

        .ui-datepicker .ui-datepicker-prev,
        .ui-datepicker .ui-datepicker-next {
            display: none;
        }

        #tblDashboardInfo tbody tr:nth-child(odd) {
            background-color: white;
        }

        #tblDashboardInfo tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .chartColor {
            margin-top: 3px;
            font-size: 22px;
            cursor: pointer;
            color: white;
        }

        .highcharts-title {
            width: 600px;
            text-align: center;
            top: 1px;
        }

        .multiselect-container {
            overflow-y: auto;
        }

        #tblHeader {
            margin-bottom: 0px;
        }
    </style>

    <div class="row">
        <asp:Label ID="lblMessages" runat="server" EnableViewState="False" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>

    <div class="row" style="margin-bottom: 4px; margin-top: -11px;">
        <table id="tblHeader" class="table table-bordered" style="background-color: #394A59">
            <tr>
                <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                <td class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="From DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                </td>

                <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                <td class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="To DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                </td>
                <td class="commontd"><b>
                    <input type="checkbox" id="chkAutoRefresh" />
                    Auto Refresh </b></td>
                <td class="commontd"><b style="visibility: hidden">
                    <input type="checkbox" id="chkGoLive" />
                    Go Live </b></td>
            </tr>
            <tr>
                <td class="commontd"><b>Machine ID </b></td>
                <td>
                    <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" data-toggle="tooltip" ToolTip="Select Machine ID"></asp:DropDownList>
                </td>
                <td class="commontd"><b>Cycle Start Time </b></td>
                <td>
                    <select id="ddlCycleStartTime" data-toggle="tooltip" class="form-control"></select>
                </td>
                <td>
                    <button type="button" class="btn btn-info btn-sm displayCss" style="width: 100px;" id="btnView" aria-disabled="false">View </button>
                </td>
                <td>
                    <asp:Button ID="btnExport" runat="server" Text="Export" class="btn btn-info btn-sm displayCss" Width="100" OnClick="btnExportParameter_Click" Enabled="true" />
                </td>
            </tr>
        </table>
    </div>

    <div id="mainChartwindow" style="height: 600px; width: 100%;">
        <div id="tempratureChart" style="width: 100%; height: 560px; margin-left: -14px;" />

    </div>
    <div id="pressureChart" style="width: 100%; height: 560px; margin-left: -14px; margin-top: 5px" />

    <script type="text/javascript">
        var parameterTimer = null;
        var chart = null;
        var chart2 = null;
        $(document).ready(function () {
            $.unblockUI({});
            $('[data-toggle="tooltip"]').tooltip();
            $('[id$=txtFromDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: true,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txtToDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            BindCycleStartTime("Bytime");
            GetTempratureChartData();
        });

        var TempratureData = {};
        var operatingLimitsTemp = {};
        var operatingLimitsPres = {};
        var PressureData = {};

        function BindCycleStartTime(Param) {
            var machineId = $("[id$=ddlMachineId]").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProcessParameterDashboard.aspx/BindCycleStartTime",
                data: '{startDT:"' + $("[id$=txtFromDate]").val() + '", endDT:"' + $("[id$=txtToDate]").val() + '",MachineID:"' + machineId + '", param:"' + Param + '"}',
                dataType: "json",
                success: function (result) {
                    var cycleSatartTimeList = result.d;
                    if (cycleSatartTimeList.length > 0) {
                        $("#ddlCycleStartTime").empty();
                        for (var i = 0; i < cycleSatartTimeList.length; i++) {
                            $("#ddlCycleStartTime").append('<option value="' + cycleSatartTimeList[i] + '">' + cycleSatartTimeList[i] + '</option>');
                        }
                    }
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function GetOperatingLimitsTemprature(MachineId, TempData) {
            var machineId = MachineId;
            TempratureData = TempData;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProcessParameterDashboard.aspx/GetOperatingLimitsTemprature",
                data: '{MachineID:"' + machineId + '"}',
                dataType: "json",
                success: function (result) {
                    operatingLimitsTemp = result.d;
                    LoadTempratureChart(operatingLimitsTemp, TempratureData);
                    GetPressureChartData();
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function GetOperatingLimitsPressure(MachineId, PresData) {
            var machineId = MachineId;
            PressureData = PresData;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProcessParameterDashboard.aspx/GetOperatingLimitsPressure",
                data: '{MachineID:"' + machineId + '"}',
                dataType: "json",
                success: function (result) {
                    operatingLimitsPres = result.d;
                    LoadPressureChart(operatingLimitsPres, PressureData);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function GetTempratureChartData() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            var Header = {};
            Header.FromDate = $("[id$=ddlCycleStartTime]").val() == "All" || $("[id$=ddlCycleStartTime]").val() == null ? $("[id$=txtFromDate]").val() : $("[id$=ddlCycleStartTime]").val();
            Header.ToDate = $("[id$=txtToDate]").val();
            Header.MachineId = $("[id$=ddlMachineId]").val() == null ? "" : $("[id$=ddlMachineId]").val();
            Header.CycleStartTime = $("[id$=ddlCycleStartTime]").val();
            Header.Param = Header.CycleStartTime == "All" ? "Bytime" : "ByCycle";

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProcessParameterDashboard.aspx/GetTempratureChartData",
                data: JSON.stringify({
                    header: Header
                }),
                dataType: "json",
                success: function (result) {
                    TempratureData = result.d;
                    GetOperatingLimitsTemprature($("[id$=ddlMachineId]").val(), TempratureData);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function GetPressureChartData() {
            var Header = {};
            Header.FromDate = $("[id$=ddlCycleStartTime]").val() == "All" || $("[id$=ddlCycleStartTime]").val() == null ? $("[id$=txtFromDate]").val() : $("[id$=ddlCycleStartTime]").val();
            Header.ToDate = $("[id$=txtToDate]").val();
            Header.MachineId = $("[id$=ddlMachineId]").val() == null ? "" : $("[id$=ddlMachineId]").val();
            Header.CycleStartTime = $("[id$=ddlCycleStartTime]").val();
            Header.Param = Header.CycleStartTime == "All" ? "Bytime" : "ByCycle";

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProcessParameterDashboard.aspx/GetPressureChartData",
                data: JSON.stringify({
                    header: Header
                }),
                dataType: "json",
                success: function (result) {
                    PressureData = result.d;
                    GetOperatingLimitsPressure($("[id$=ddlMachineId]").val(), PressureData);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function LoadTempratureChart(opLimits, tempratureData) {
            chart = Highcharts.stockChart('tempratureChart', {
                legend: {
                    enabled: true,
                    align: 'top',
                    backgroundColor: '#FCFFC5',
                    borderColor: 'black',
                    borderWidth: 2,
                    layout: 'horizontal',
                    verticalAlign: 'top',
                    y: 30,
                    x: 440,
                    shadow: true
                },

                time: {
                    useUTC: false
                },
                lang: {
                    noData: "No Temprature data available for selected datetime and machine Id."
                },
                noData: {
                    style: {
                        fontWeight: 'bold',
                        fontSize: '17px',
                        color: '#303030'
                    }
                },
                rangeSelector: {
                    buttons: [{
                        count: 1,
                        type: 'minute',
                        text: '1M'
                    }, {
                        count: 5,
                        type: 'minute',
                        text: '5M'
                    }, {
                        count: 10,
                        type: 'minute',
                        text: '10M'
                    }, {
                        count: 20,
                        type: 'minute',
                        text: '20M'
                    }, {
                        count: 30,
                        type: 'minute',
                        text: '30M'
                    },
                    {
                        count: 1,
                        type: 'hour',
                        text: '1H'
                    }],
                    inputEnabled: false,
                    selected: 3
                },

                title: {
                    text: 'Live Temperature Data',
                    style: {
                        fontSize: '20px',
                        fontWeight: 'bold'
                    }
                },

                exporting: {
                    enabled: true
                },
                yAxis: {
                    opposite: false,
                    title: {
                        text: 'Temperature',
                        style: {
                            fontSize: '18px',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: '14px',
                            fontWeight: 'bold'
                        }
                    },
                    minorGridLineWidth: 0,
                    gridLineWidth: 0,
                    alternateGridColor: null,
                    plotBands: opLimits
                },
                credits: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        marker: {
                            enabled: true,
                            radius: 5
                        }
                    }
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
                    categories: tempratureData.updatedTimeList,
                    alignTicks: true,
                    gridLineWidth: 1,
                    gridLineColor: '#604800',
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontWeight: 'bold'
                        }
                    },
                    plotLines: tempratureData.plotLinesList
                },
                series: tempratureData.ChartDataList,
                navigation: {
                    menuItemStyle: {
                        fontSize: '13px',
                        fontWeight: 'bold'
                    }
                }
            });
        }

        function LoadPressureChart(opLimits, pressureData) {
            var startDate = $("[id$=txtToDate]").val();
            chart2 = Highcharts.stockChart('pressureChart', {

                legend: {
                    enabled: true,
                    align: 'top',
                    backgroundColor: '#FCFFC5',
                    borderColor: 'black',
                    borderWidth: 2,
                    layout: 'horizontal',
                    verticalAlign: 'top',
                    y: 30,
                    x: 550,
                    shadow: true
                },

                time: {
                    useUTC: false
                },
                lang: {
                    noData: "No Pressure data available for selected datetime and machine Id."
                },
                noData: {
                    style: {
                        fontWeight: 'bold',
                        fontSize: '17px',
                        color: '#303030'
                    }
                },
                rangeSelector: {
                    buttons: [{
                        count: 1,
                        type: 'minute',
                        text: '1M'
                    }, {
                        count: 5,
                        type: 'minute',
                        text: '5M'
                    }, {
                        count: 10,
                        type: 'minute',
                        text: '10M'
                    }, {
                        count: 20,
                        type: 'minute',
                        text: '20M'
                    }, {
                        count: 30,
                        type: 'minute',
                        text: '30M'
                    },
                    {
                        count: 1,
                        type: 'hour',
                        text: '1H'
                    }],
                    inputEnabled: false,
                    selected: 3
                },

                title: {
                    text: 'Live Pressure Data',
                    style: {
                        fontSize: '20px',
                        fontWeight: 'bold'
                    }
                },

                exporting: {
                    enabled: true
                },
                yAxis: {
                    opposite: false,
                    title: {
                        text: 'Pressure',
                        style: {
                            fontSize: '18px',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: '14px',
                            fontWeight: 'bold'
                        }
                    },
                    minorGridLineWidth: 0,
                    gridLineWidth: 0,
                    alternateGridColor: null,
                    plotBands: opLimits
                },
                credits: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        marker: {
                            enabled: true,
                            radius: 5
                        }
                    }
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
                    categories: pressureData.updatedTimeList,
                    alignTicks: true,
                    gridLineWidth: 1,
                    gridLineColor: '#604800',
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontWeight: 'bold'
                        }
                    },
                    plotLines: pressureData.plotLinesList
                },
                series: pressureData.ChartDataList,
                navigation: {
                    menuItemStyle: {
                        fontSize: '13px',
                        fontWeight: 'bold'
                    }
                }
            });
            $.unblockUI({});
            $('.ajax-loader').hide();
        }

        $("#btnView").click(function () {
            var mId = $("[id$=ddlMachineId]").val();
            GetTempratureChartData();
            BindCycleStartTime($("[id$=ddlCycleStartTime]").val() == "All" ? "Bytime" : "ByCycle");
        });

        $("#chkAutoRefresh").click(function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            $("[id$=btnExport], [id$=btnView]").attr('disabled', 'disabled');
            $("[id$=ddlCycleStartTime]").attr('disabled', 'disabled');
            var startDate = $("[id$=txtToDate]").val();
            if ($(this).is(":checked")) {
                parameterTimer = setInterval(function () {
                    var tempSerData = {};
                    var Header = {};
                    Header.FromDate = startDate;
                    var tdate = new Date(Header.FromDate);
                    tdate.setSeconds(tdate.getSeconds() + 10);
                    var todate = tdate.getFullYear() + "-" + (tdate.getMonth() + 1) + "-" + tdate.getDate() + " " + tdate.getHours() + ":" + tdate.getMinutes() + ":" + tdate.getSeconds();
                    Header.ToDate = todate;
                    Header.MachineId = $("[id$=ddlMachineId]").val() == null ? "" : $("[id$=ddlMachineId]").val();
                    Header.CycleStartTime = $("[id$=ddlCycleStartTime]").val();
                    if (Header.CycleStartTime == null || Header.CycleStartTime == '' || Header.CycleStartTime == "All") {
                        Header.CycleStartTime = "ALL";
                    }
                    Header.Param = Header.CycleStartTime == "All" ? "Bytime" : "ByCycle";
                    startDate = todate;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ProcessParameterDashboard.aspx/GetTempratureChartDataRefresh",
                        data: JSON.stringify({
                            header: Header
                        }),
                        dataType: "json",
                        success: function (result) {
                            tempSerData = result.d;
                            if (tempSerData.ChartDataList != null) {
                                for (var i = 0; i < tempSerData.ChartDataList.length; i++) {
                                    var naame = tempSerData.ChartDataList[i].name;
                                    if (naame == "T1") {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart.series[0].addPoint([x, y], true, false);
                                        }
                                    }
                                    else if (naame == "T2") {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart.series[1].addPoint([x, y], true, false);
                                        }
                                    }
                                    else if (naame == "T3") {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart.series[2].addPoint([x, y], true, false);
                                        }
                                    }
                                    else if (naame == "T4") {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart.series[3].addPoint([x, y], true, false);
                                        }
                                    }
                                    else if (naame == "T5") {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart.series[4].addPoint([x, y], true, false);
                                        }
                                    }
                                    else {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart.series[5].addPoint([x, y], true, false);
                                        }
                                    }
                                }

                                for (var j = 0; j < tempSerData.plotLinesList.length; j++) {
                                    if (tempSerData.plotLinesList[j].value > 10000) {
                                        chart.xAxis[0].addPlotLine(tempSerData.plotLinesList[j]);
                                    }
                                }
                            }
                            else {
                                chart.showNoData("Data not available for this refresh cycle");
                            }
                        },
                        error: function (Result) {
                            alert("Error");
                        }
                    });

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ProcessParameterDashboard.aspx/GetPressureChartDataRefresh",
                        data: JSON.stringify({
                            header: Header
                        }),
                        dataType: "json",
                        success: function (result) {
                            tempSerData = result.d;
                            if (tempSerData.ChartDataList != null) {
                                for (var i = 0; i < tempSerData.ChartDataList.length; i++) {
                                    var naame = tempSerData.ChartDataList[i].name;
                                    if (naame == "P1") {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart2.series[0].addPoint([x, y], true, false);
                                        }
                                    }
                                    else {
                                        for (var j = 0; j < tempSerData.ChartDataList[i].data.length; j++) {
                                            var x = tempSerData.ChartDataList[i].data[j][0];
                                            var y = tempSerData.ChartDataList[i].data[j][1];
                                            chart2.series[1].addPoint([x, y], true, false);
                                        }
                                    }
                                }

                                for (var j = 0; j < tempSerData.plotLinesList.length; j++) {
                                    if (tempSerData.plotLinesList[j].value > 10000) {
                                        chart2.xAxis[0].addPlotLine(tempSerData.plotLinesList[j]);
                                    }
                                }
                            }
                            else {
                                chart2.showNoData("Data not available for this refresh cycle");
                            }
                        },
                        error: function (Result) {
                            alert("Error");
                        }
                    });
                }, 20000);
            }
            if ($(this).is(":unchecked")) {
                clearInterval(parameterTimer);
                $("[id$=btnExport], [id$=btnView]").removeAttr('disabled');
                $("[id$=ddlCycleStartTime]").removeAttr('disabled');
            }
            $.unblockUI({});
            $('.ajax-loader').hide();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

