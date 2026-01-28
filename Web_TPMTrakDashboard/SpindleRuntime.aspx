<%@ Page Language="C#" Title="Spindle Runtime" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SpindleRuntime.aspx.cs" Inherits="Web_TPMTrakDashboard.SpindleRuntime" meta:resourcekey="PageResource1" Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%--<%: Scripts.Render("~/bundles/HighChartsJs") %>--%>

    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/stock/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/no-data-to-display.js"></script>

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
                <td class="commontd" style="visibility: hidden"><b>
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
                <%--<td>
                    <asp:Button ID="btnExport" runat="server" Text="Export" class="btn btn-info btn-sm displayCss" Width="100" OnClick="btnExportParameter_Click" Enabled="true" />
                </td>--%>
            </tr>
        </table>
    </div>

    <div id="mainChartwindow" style="height: 600px; width: 100%;">
        <div id="machineStatusStepLineChart" style="width: 100%; height: 560px; margin-left: -14px;" />
    </div>

    <script type="text/javascript">
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
            LoadMachineStatusData();
        });

        function BindCycleStartTime(Param) {
            
            var machineId = $("[id$=ddlMachineId]").val();
            var sdt = $("[id$=txtFromDate]").val();
            var edt = $("[id$=txtToDate]").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SpindleRuntime.aspx/BindCycleStartTime",
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

        var machineStatusData = {};
        function LoadMachineStatusData() {
            var machineId = $("[id$=ddlMachineId]").val();
            var startDT = $("[id$=ddlCycleStartTime]").val() == "All" || $("[id$=ddlCycleStartTime]").val() == null || $("[id$=ddlCycleStartTime]").val() == "" ? $("[id$=txtFromDate]").val() : $("[id$=ddlCycleStartTime]").val().split("(")[0];

            var endDT = $("[id$=txtToDate]").val();
            var param = $("[id$=ddlCycleStartTime]").val() == "All" ? "Bytime" : "Bycycle";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SpindleRuntime.aspx/GetSpindleRuntimeData",
                data: '{startDT:"' + startDT + '", endDT:"' + endDT + '", MachineID:"' + machineId + '", param:"' + param + '"}',
                dataType: "json",
                success: function (result) {
                    machineStatusData = result.d;
                    LoadMachineStatusChart(machineStatusData);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function LoadMachineStatusChart(MachineStatusData) {
            var spindleChart = Highcharts.stockChart('machineStatusStepLineChart', {
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
                    selected: 4
                },
                time: {
                    useUTC: false
                },
                lang: {
                    noData: "No data available for selected datetime and machine Id."
                },
                noData: {
                    style: {
                        fontWeight: 'bold',
                        fontSize: '17px',
                        color: '#303030'
                    }
                },
                labels: {
                    items: [{
                        html: MachineStatusData.totalSpindleRuntime,
                        style: {
                            left: $("#machineStatusStepLineChart").width() - 320,
                            top: '-70px',
                            fontSize: '14px',
                            fontWeight: 'bold'
                        }
                    }],
                },
                title: {
                    text: 'Spindle Run Chart',
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
                    labels: {
                        enabled: false,
                        style: {
                            fontSize: '14px',
                            fontWeight: 'bold'
                        }
                    },
                    minorGridLineWidth: 0,
                    gridLineWidth: 0,
                    alternateGridColor: null,
                },
                credits: {
                    enabled: false
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
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontWeight: 'bold'
                        }
                    },
                    plotLines: MachineStatusData.plotLinesList
                },
                series: [{
                    name: 'Spindle Runtime',
                    data: MachineStatusData.spRuntimeDataList,
                    step: true,
                    tooltip: {
                        valueDecimals: 2
                    }
                }]
            });
        }

        $("#btnView").click(function () {
            BindCycleStartTime($("[id$=ddlCycleStartTime]").val() == "All" ? "Bytime" : "ByCycle");
            LoadMachineStatusData();
            spindleChart.reflow();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
