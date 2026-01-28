<%@ Page Title="Vibration Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VibrationDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.VibrationDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%--<script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>--%>
    <%--<script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>--%>


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

        .chartDivStyle {
            width: 100%;
            height: 560px;
            margin-left: -14px;
            margin-top: 15px;
            height:400px;
        }
    </style>
    <div>
        <div class="row">
            <asp:Label ID="lblMessages" runat="server" EnableViewState="False" meta:resourcekey="lblMessagesResource1"></asp:Label>
        </div>

        <div class="row" style="margin-bottom: 4px; margin-top: -11px;">
            <table id="tblHeader" class="table table-bordered" style="background-color: #394A59; display: inline-block; vertical-align: sub;">
                <tr>
                    <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                    <td class="input-group" style="width: 280px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="From DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td class="commontd"><b>Machine ID </b></td>
                    <td>
                        <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" data-toggle="tooltip" ToolTip="Select Machine ID"></asp:DropDownList>
                    </td>
                    <td class="commontd"><b>Cycle Start Time </b></td>
                    <td>
                        <select id="ddlCycleStartTime" data-toggle="tooltip" class="form-control"></select>
                    </td>
                    <td class="commontd paramVisible"><b>Parameter </b></td>
                    <td class="paramVisible" id="tdLstParameters">
                        <asp:ListBox ID="lstParameters" Width="200" Style="margin: 10px; max-width: 300px" runat="server" SelectionMode="Multiple" ClientIDMode="Static" >
                            <asp:ListItem Text="Signal Energy" Value="SignalEnergy" order="1"></asp:ListItem>
                            <asp:ListItem Text="Temperature" Value="Temperature" order="2"></asp:ListItem>
                            <asp:ListItem Text="Noise" Value="Noise" order="3"></asp:ListItem>
                            <asp:ListItem Text="Velocity-X" Value="Velocity-X" order="4"></asp:ListItem>
                            <asp:ListItem Text="Velocity-Y" Value="Velocity-Y" order="5"></asp:ListItem>
                            <asp:ListItem Text="Velocity-Z" Value="Velocity-Z" order="6"></asp:ListItem>
                            <asp:ListItem Text="Acceleration-X" Value="Acceleration-X"></asp:ListItem>
                            <asp:ListItem Text="Acceleration-Y" Value="Acceleration-Y"></asp:ListItem>
                            <asp:ListItem Text="Acceleration-Z" Value="Acceleration-Z"></asp:ListItem>
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                    <td class="input-group" style="width: 280px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="To DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td class="commontd" style="width: 200px"><b>
                        <input type="checkbox" id="chkAutoRefresh" />
                        Auto Refresh </b></td>
                    <td style="width: 200px; text-align: center">
                        <button type="button" class="btn btn-info btn-sm displayCss" style="width: 100px;" id="btnView" aria-disabled="false">View </button>
                    </td>
                    <td style="text-align: center">
                        <asp:Button runat="server" ID="btnExport" Text="Expot" CssClass="btn btn-info btn-sm displayCss" Style="width: 100px;" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
        </div>

        <div id="mainChartwindow" style="height: 600px; width: 100%; margin: 10px; padding: 10px; text-align: center; vertical-align: central;">
            <div id="chartContainer">
            </div>

        </div>
    </div>
    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/stock/modules/data.js"></script>
    <script src="https://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/stock/modules/export-data.js"></script>
    <script type="text/javascript">
        var parameterTimer = null;
        var chart = null;
        var chartList = [], chartParameterList = [];
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
            $('[id$=lstParameters]').multiselect({
                includeSelectAllOption: false
            });
            GetChartData();
            ParameterVisibility();
            $('#tdLstParameters ul.dropdown-menu').on('mousedown', 'li', function (event) {
                debugger;
                if ($('#tdLstParameters ul.dropdown-menu li.active').length >= 3 && (!$(this).hasClass("active"))) {
                    alert("Parameter Cannot be greater than 3");
                    event.preventDefault();
                }
            });
        });
        function ParameterVisibility() {
            debugger;
            if (isParameterIdEnabled() == false) {
                $('.paramVisible').css('display', 'none');
            }
        }
        function BindCycleStartTime(data) {
            if (data != null && data != undefined) {
                if (data.length > 0) {
                    cycleSatartTimeList = data[0].CycleStartEnd;
                    if (cycleSatartTimeList.length > 0) {

                        for (var i = 0; i < cycleSatartTimeList.length; i++) {

                            $("#ddlCycleStartTime").append('<option value="' + cycleSatartTimeList[i].End + '">' + cycleSatartTimeList[i].Start + '</option>');
                        }
                    }
                }
                else {
                    if ($("#ddlCycleStartTime option").length == 0) {
                        $("#ddlCycleStartTime").append('<option value="All">All</option>');
                    }
                }
            }
        }
        function isParameterIdEnabled() {
            var isParamEnabled = false;
            $.ajax({
                async: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VibrationDashboard.aspx/isParameterIdRequired",
                dataType: "json",
                success: function (result) {
                    isParamEnabled = result.d;
                },
                error: function (Result) {
                    alert("Error");
                }
            });
            return isParamEnabled;
        }
        function GetChartData() {

            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            var Header = {};
            Header.FromDate = $("[id$=ddlCycleStartTime]").val() == "All" || $("[id$=ddlCycleStartTime]").val() == null ? $("[id$=txtFromDate]").val() : $("[id$=ddlCycleStartTime] option:selected").text();
            Header.ToDate = $("[id$=ddlCycleStartTime]").val() == "All" || $("[id$=ddlCycleStartTime]").val() == null ? $("[id$=txtToDate]").val() : $("[id$=ddlCycleStartTime] option:selected").val();
            Header.MachineId = $("[id$=ddlMachineId]").val() == null ? "" : $("[id$=ddlMachineId]").val();
            Header.CycleStartTime = $("[id$=ddlCycleStartTime]").val();
            if ($("[id$=ddlCycleStartTime]").text().startsWith("All")) {
                Header.Param = "NotCycleView";
            }
            else {
                Header.Param = "View"
            }
            Header.Parameter = "";
            if ($("[id$=lstParameters]")[0] != undefined) {
                var parametrs = $("[id$=lstParameters]").val();

                for (var i = 0; i < parametrs.length; i++) {
                    if (Header.Parameter == "") {
                        Header.Parameter += parametrs[i];
                    }
                    else {
                        Header.Parameter += "," + parametrs[i];
                    }
                }
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VibrationDashboard.aspx/GetChartData",
                data: JSON.stringify({
                    header: Header
                }),
                dataType: "json",
                success: function (result) {
                    Data = result.d;
                    BindCycleStartTime(result.d, Header.Param);
                    LoadCharts(Data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function LoadCharts(Data) {
            // Create the chart
            console.log(Data.series);
            $('#chartContainer').empty();
            for (var i = 0; i < Data.length; i++) {
                var parameter = Data[i].Parameter;
                $('#chartContainer').append('<div id="chartDiv' + i + '" class="chartDivStyle" parameter="' + parameter + '" />');
                chartParameterList[i] = parameter;
                chartList[i] = Highcharts.stockChart("chartDiv" + i, {
                    title: {
                        text: parameter + ' Data',
                        style: {
                            fontSize: '20px',
                            fontWeight: 'bold'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    lang: {
                        noData: "No " + parameter + " data available for selected datetime and machine Id."
                    },
                    exporting: {
                        enabled: true
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
                            type: 'second',
                            text: '1S'
                        }, {
                            count: 5,
                            type: 'second',
                            text: '5S'
                        }, {
                            count: 10,
                            type: 'second',
                            text: '10S'
                        }, {
                            count: 20,
                            type: 'second',
                            text: '20S'
                        }, {
                            count: 30,
                            type: 'second',
                            text: '30S'
                        }, {
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

                    navigation: {
                        buttonOptions: {
                            enabled: false
                        }
                    },
                    xAxis: {
                        type: 'datetime',
                        tickInterval: 200,
                        minRange: 5,
                        ordinal: false,
                        dateTimeLabelFormats: {
                            millisecond: '%H:%M:%S.%L',
                            second: '%H:%M:%S',
                            minute: '%H:%M',
                            hour: '%H:%M',
                            day: '%e. %b',
                            week: '%e. %b',
                            month: '%b \'%y',
                            year: '%Y'
                        },
                        categories: Data[i].Category,
                        plotLines: Data[i].PlotLines,
                    },
                    yAxis: {
                        opposite: false
                    },
                    //plotOptions: {
                    //    line: {
                    //        dataLabels: {
                    //            enabled: true,
                    //            useHTML: true,
                    //            format: '<span class="hc-label" style="z-index:10">{y}</span>'

                    //        },
                    //        enableMouseTracking: false
                    //    }
                    //},
                    tooltip: {
                        formatter: function () {
                            var tooltip = "";
                            this.points.reduce(function (s, point) {
                                if (point.series.name == parameter + " Data") {
                                    var i = point.point.index;
                                    tooltip += '<br/>' + point.series.name + ': ' + point.y;
                                }
                                else {
                                    if (s == undefined) {
                                        tooltip += '<br/>' + point.series.name + ': ' + point.y;
                                    }
                                    else {
                                        tooltip += '<b>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S.%L', point.x) + '</b><br/>' + point.series.name + ': ' + point.y;
                                    }
                                }
                            }, '<b>' + this.x + '</b>');
                            return tooltip;
                        },
                        shared: true
                    },
                    series: Data[i].series,
                });
            }
            $.unblockUI({});
            $('.ajax-loader').hide();
        }

        $("#btnView").click(function () {
            var mId = $("[id$=ddlMachineId]").val();
            if ($("#ddlCycleStartTime").val() == "All") {
                $("#ddlCycleStartTime").empty();
            }

            GetChartData();
        });
       
        $("#chkAutoRefresh").click(function () {
            $("[id$=btnView]").attr('disabled', 'disabled');
            $('[id$=tdLstParameters] button.multiselect').attr('disabled', 'disabled');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
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
                    startDate = todate;
                    Header.Parameter = "";
                    if ($("[id$=lstParameters]")[0] != undefined) {
                        var parametrs = $("[id$=lstParameters]").val();

                        for (var i = 0; i < parametrs.length; i++) {
                            if (Header.Parameter == "") {
                                Header.Parameter += parametrs[i];
                            }
                            else {
                                Header.Parameter += "," + parametrs[i];
                            }
                        }
                    }
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "VibrationDashboard.aspx/GetChartDataRefresh",
                        data: JSON.stringify({
                            header: Header
                        }),
                        dataType: "json",
                        success: function (result) {
                            var RefreshDataList = result.d;
                            //BindCycleStartTime(RefreshData);
                            if (RefreshDataList != null) {
                                for (var paramCount = 0; paramCount < RefreshDataList.length; paramCount++) {
                                    RefreshData = RefreshDataList[paramCount];
                                    chart = null;
                                    for (var i = 0; i < chartParameterList.length; i++) {
                                        if (RefreshData.ParameterID == chartParameterList[i]) {
                                            chart = chartList[i];
                                            break;
                                        }
                                    }
                                    if (RefreshData.Refreshdata != null) {
                                        //for (var i = 0; i < RefreshData.series.length; i++) {
                                        //    var naame = RefreshData.series[i].name;
                                        //    if (naame == "Error") {
                                        //        //for (var j = 0; j < RefreshData.series[i].data.length; j++) {
                                        //        //    var x = RefreshData.series[i].data[j][0];
                                        //        //    var y = RefreshData.series[i].data[j][1];
                                        //        chart.series[i].addPoint(RefreshData.series[i].data, false, false);
                                        //        //}
                                        //    }
                                        //    if (naame == "Warning") {
                                        //        //for (var j = 0; j < RefreshData.series[i].data.length; j++) {
                                        //        //    var x = RefreshData.series[i].data[j][0];
                                        //        //    var y = RefreshData.series[i].data[j][1];
                                        //        chart.series[i].addPoint(RefreshData.series[i].data, false, false);
                                        //        //}
                                        //    }
                                        //    if (naame == "Vibration Data") {
                                        //        //for (var j = 0; j < RefreshData.series[i].data.length; j++) {
                                        //        //    var x = RefreshData.series[i].data[j][0];
                                        //        //    var y = RefreshData.series[i].data[j][1];
                                        //        //    var ApplyRuleFor_N_Observation = RefreshData.series[j].ApplyRuleFor_N_Observation
                                        //        //    var Total_M_Observation = RefreshData.series[j].Total_M_Observation
                                        //        //    var Machineid = RefreshData.series[j].Machineid
                                        //        //    var ComponentID = RefreshData.series[j].ComponentID
                                        //        //    var Operationno = RefreshData.series[j].Operationno
                                        //        chart.series[i].addPoint(RefreshData.series[i].data, false, false);
                                        //            //chart.series[i].ApplyRuleFor_N_Observation.addPoint(ApplyRuleFor_N_Observation, true, false);
                                        //            //chart.series[i].Total_M_Observation.addPoint(Total_M_Observation, true, false);
                                        //            //chart.series[i].ComponentID.addPoint(ComponentID, true, false);
                                        //            //chart.series[i].Machineid.addPoint(Machineid, true, false);
                                        //            //chart.series[i].Operationno.addPoint(Operationno, true, false);
                                        //        }
                                        //    }
                                        //}
                                        if (RefreshData.Refreshdata.vibrationdata != null && RefreshData.Refreshdata.vibrationdata != undefined) {
                                            for (var i = 0; i < RefreshData.Refreshdata.vibrationdata.length; i++) {

                                                var x = RefreshData.Refreshdata.time[i];
                                                var y = RefreshData.Refreshdata.ErrorData[i];
                                                chart.series[0].addPoint([x, y], false, false);
                                                y = RefreshData.Refreshdata.WarningData[i];
                                                chart.series[1].addPoint([x, y], false, false);
                                                y = RefreshData.Refreshdata.vibrationdata[i];
                                                chart.series[2].addPoint([x, y], false, false);
                                                //var ApplyRuleFor_N_Observation = RefreshData.Refreshdata.ApplyRuleFor_N_Observation[i]
                                                //var Total_M_Observation = RefreshData.Refreshdata.Total_M_Observation[i]
                                                //var Machineid = RefreshData.Refreshdata.Machineid[i]
                                                //var ComponentID = RefreshData.Refreshdata.ComponentID[i]
                                                //var Operationno = RefreshData.Refreshdata.Operationno[i]
                                                //chart.series[2].ApplyRuleFor_N_Observation.addPoint(ApplyRuleFor_N_Observation, true, false);
                                                //chart.series[2].Total_M_Observation.addPoint(Total_M_Observation, true, false);
                                                //chart.series[2].ComponentID.addPoint(ComponentID, true, false);
                                                //chart.series[2].Machineid.addPoint(Machineid, true, false);
                                                //chart.series[2].Operationno.addPoint(Operationno, true, false);
                                            }
                                        }
                                        if (RefreshData.Refreshdata.PlotLines != null && RefreshData.Refreshdata.PlotLines != undefined) {
                                            for (var j = 0; j < RefreshData.Refreshdata.PlotLines.length; j++) {

                                                chart2.xAxis[0].addPlotLine(RefreshData.Refreshdata.PlotLines[j]);

                                            }
                                        }

                                        chart.redraw();
                                        //}
                                        //else {
                                        //    //chart.showNoData("Data not available for this refresh cycle");
                                        //}
                                    }
                                }
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
                $('[id$=tdLstParameters] button.multiselect').removeAttr('disabled');
            }
            $.unblockUI({});
            $('.ajax-loader').hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>


