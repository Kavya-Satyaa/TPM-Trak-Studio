<%@ Page Title="DayAndShiftView" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DayAndShiftViewPTA.aspx.cs" Inherits="Web_TPMTrakDashboard.PTA.DayAndShiftViewPTA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <script src="../Scripts/HighCharts10.3.2/highcharts.js"></script>
    <script src="../Scripts/HighCharts10.3.2/highcharts-3d.js"></script>
    <script src="../Scripts/HighCharts10.3.2/exporting.js"></script>
    <script src="../Scripts/HighCharts10.3.2/export-data.js"></script>
    <script src="../Scripts/HighCharts10.3.2/accessibility.js"></script>
    <style>
        fieldset {
            border: 2px solid white;
            padding-left: 5px;
            padding-bottom: 5px;
            border-radius: 4px;
            padding-right: 5px;
            width: auto;
        }

        legend {
            color: white;
            width: auto;
            border-bottom: 0px;
            margin: 0px;
        }

        .plantGridTbl tr td {
            font-size: 22px;
            font-weight: bold;
            padding: 8px;
            border: 1px solid silver;
        }

        #ShiftViewContainer .plantGridTbl tr td {
            font-size: 17px;
            font-weight: bold;
            padding: 7px;
            border: 1px solid silver;
        }

        .redcolor-style td {
            color: red;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnPlantID" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto;">
                <tr>

                    <td class="commanTd" style="vertical-align: middle;">View Type</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlViewType" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlViewType_SelectedIndexChanged">
                            <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                            <asp:ListItem Text="Shift" Value="Shift"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Date</td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date date-control" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Machine</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td id="tdShiftHeader" runat="server" class="commanTd" style="vertical-align: middle;">Shift</td>
                    <td id="tdShiftContent" runat="server">
                        <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                    </td>
                </tr>
            </table>

            <div id="DayViewContainer" runat="server" clientidmode="static">
                <table style="width: 100%;">
                    <tr>
                        <td style="vertical-align: top; padding-right: 5px; width: 55%;">
                            <fieldset>
                                <legend>Run-time Analysis</legend>
                                <table style="width: 100%; table-layout: fixed;">
                                    <tr>
                                        <td style="width: 60%;">
                                            <div id="totalTimeChartDiv"></div>
                                        </td>
                                        <%-- <td>
                                            <div id="otherChartDiv"></div>
                                        </td>--%>
                                    </tr>
                                </table>

                                <table class="plantGridTbl alternate-table-style" style="width: 100%; table-layout: fixed; margin-top: 10px;">
                                    <tr>
                                        <td>Production Time
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblProductionTime" ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="redcolor-style">
                                        <td>Non-Production Time
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblNonProductionTime" ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="redcolor-style">
                                        <td>Load-unload
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblLoadUnload" ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="redcolor-style">
                                        <td>Machine Stoppages
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblMachineStoppage" ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Num. of Cycles (Parts)
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblNoOfParts" ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Target Revenue (MHR)
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblTargetRevenue" ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td style="vertical-align: top">

                            <fieldset>
                                <legend>Machine Stoppage</legend>
                                <div style="height: 77vh; overflow: auto">
                                    <asp:ListView runat="server" ID="lvMachineStoppage" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                        <LayoutTemplate>
                                            <table class="table table-bordered headerFixer alternate-table-style">
                                                <tr>
                                                    <th>From (HH:mm:ss)</th>
                                                    <th>To (HH:mm:ss)</th>
                                                    <th>Duration</th>
                                                    <th>Reason</th>
                                                </tr>
                                                <tr runat="server" id="itemplaceholder"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%# Eval("FromTime") %>
                                                </td>
                                                <td>
                                                    <%# Eval("ToTime") %>
                                                </td>
                                                <td>
                                                    <%# Eval("Duration") %>
                                                </td>
                                                <td>
                                                    <%# Eval("Reason") %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </table>

            </div>

            <div id="ShiftViewContainer" runat="server" clientidmode="static">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setDateTimePicker();
        });
        function BindDayChart() {
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: '<%= ResolveUrl("DayAndShiftViewPTA.aspx/getDayChartData") %>',
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (response) {
                    var chartData = response.d;
                    //BindDayPieChart(chartData.ProductionEffy, chartData.NonProductionEffy);
                    //BindDayBarChart(chartData.LoadUnloadEffy, chartData.MachineStoppageEffy);
                    BindDayPieChart(chartData);
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });

        }
        function BindDayPieChart(chartData) {
            if ($('#totalTimeChartDiv').length > 0) {
                Highcharts.chart('totalTimeChartDiv', {
                    chart: {
                        type: 'pie',
                        options3d: {
                            enabled: true,
                            alpha: 45,
                            beta: 0
                        }
                    },
                    title: {
                        text: '',
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            return '<b>' + this.point.name + '</b><br/>' + this.point.tooltipValue;
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            depth: 35,
                            dataLabels: {
                                enabled: true,
                                format: '{point.name} ({point.percentage:.2f}%)',
                                style: {
                                    fontSize: 13
                                }
                            }
                        },
                        series: {
                            animation: false
                        }
                    },
                    navigation: {
                        buttonOptions: {
                            enabled: false
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: 'Share',
                        data: [
                            {
                                name: 'Production',
                                y: chartData.ProductionEffy,
                                color: '#66FF00',
                                tooltipValue: chartData.ProductionTime
                            },
                            {
                                name: 'Load Unload',
                                y: chartData.LoadUnloadEffy,
                                color: '#13d8f2',
                                tooltipValue: chartData.LoadUnLoad
                            },
                            {
                                name: 'Machine Stoppage',
                                y: chartData.MachineStoppageEffy,
                                color: '#FF0000',
                                tooltipValue: chartData.MachineStoppage
                            },
                        ]
                    }]
                });
            }
        }
        function BindDayBarChart(loadUnloadEffy, stoppageEffy) {
            if ($('#otherChartDiv').length > 0) {
                Highcharts.chart('otherChartDiv', {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Other',
                    },
                    xAxis: {
                        categories: ['']
                    },
                    credits: {
                        enabled: false
                    },
                    yAxis: {
                        min: 0,
                        gridLineWidth: 0,
                        title: {
                            text: '',
                        },
                        labels: {
                            enabled: false
                        },
                        stackLabels: {
                            enabled: false
                        }
                    },
                    legend: {
                        verticalAlign: 'top',
                    },
                    tooltip: {
                        headerFormat: '<b>{point.x}</b><br/>',
                        // pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                        pointFormat: '{series.name}: {point.y}%'
                    },
                    plotOptions: {
                        column: {
                            stacking: 'normal',
                            dataLabels: {
                                enabled: true,
                                style: {
                                    fontSize: 14
                                }
                            }
                        },
                        series: {
                            animation: false,
                            borderColor: '#303030'
                        }
                    },
                    navigation: {
                        buttonOptions: {
                            enabled: false
                        }
                    },
                    series: [{
                        name: 'Machine Stoppage',
                        data: [stoppageEffy],
                        color: '#CCCCFF'
                    }, {
                        name: 'Load Unload',
                        data: [loadUnloadEffy],
                        color: '#6666FF'
                    }]
                });
            }
        }
        function BindShiftData() {
            $('#ShiftViewContainer').empty();
            $.ajax({
                async: false,
                type: "POST",
                url: '<%= ResolveUrl("DayAndShiftViewPTA.aspx/getShiftProductionData") %>',
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (response) {
                    var shiftPOList = response.d;
                    debugger;
                    var appendStr = '<table style="width: 100%">';
                    for (var i = 0; i < shiftPOList.length; i++) {
                        appendStr += '<tr><td style="width:50%;vertical-align:top;padding-right:10px;">';
                        appendStr += '<fieldset><legend>Shift - ' + shiftPOList[i].Shift + '</legend>';
                        appendStr += '<div id="shiftChartContainer' + i + '"></div>';
                        appendStr += '</fieldset></td>';

                        appendStr += '<td style="vertical-align:top">';
                        appendStr += '<fieldset><legend>Run-time Statistics</legend>';
                        appendStr += '<table class="plantGridTbl alternate-table-style" style="width: 100%; table-layout: fixed; margin-top: 10px;">';
                        appendStr += '<tr>';
                        appendStr += ' <td>Production Time</td><td> <span>' + shiftPOList[i].ProductionTime + '</span></td>';
                        appendStr += '</tr>';
                        appendStr += '<tr class="redcolor-style">';
                        appendStr += ' <td>Non-Production Time</td><td> <span>' + shiftPOList[i].NonProductionTime + '</span></td>';
                        appendStr += '</tr>';
                        appendStr += '<tr class="redcolor-style">';
                        appendStr += ' <td>Load-unload</td><td> <span>' + shiftPOList[i].LoadUnLoad + '</span></td>';
                        appendStr += '</tr>';
                        appendStr += '<tr class="redcolor-style">';
                        appendStr += ' <td>Machine Stoppages</td><td> <span>' + shiftPOList[i].MachineStoppage + '</span></td>';
                        appendStr += '</tr>';
                        appendStr += '<tr>';
                        appendStr += ' <td>Num. of Cycles</td><td> <span>' + shiftPOList[i].ActualParts + '</span></td>';
                        appendStr += '</tr>';
                        appendStr += '</table>';
                        appendStr += '</fieldset>';
                        appendStr += '</td> </tr>';
                    }
                    appendStr += '</table>';
                    $('#ShiftViewContainer').append(appendStr);
                    for (var i = 0; i < shiftPOList.length; i++) {
                        BindShiftChart("shiftChartContainer" + i, shiftPOList[i].ProductionEffy, shiftPOList[i].NonProductionEffy);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });

        }
        function BindShiftChart(conainerID, productionValue, nonProductionValue) {
            Highcharts.chart(conainerID, {
                chart: {
                    type: 'bar',
                    height: 210,
                },
                title: {
                    text: 'Run-time Analysis',
                },
                xAxis: {
                    labels: {
                        enabled: false
                    },
                    title: {
                        text: null
                    },
                    gridLineWidth: 0,
                    //lineWidth: 0
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    },
                    gridLineWidth: 1
                },
                plotOptions: {
                    bar: {
                        borderRadius: '0%',
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: 14
                            }
                        },
                        groupPadding: 0.1
                    },
                    series: {
                        animation: false,
                        borderColor: '#303030'
                    }
                },
                legend: {
                    align: 'left',
                    verticalAlign: 'top',
                },
                credits: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                series: [{
                    name: 'Production',
                    data: [productionValue],
                    color: '#66FF00'
                },
                {
                    name: 'Non-Production',
                    data: [nonProductionValue],
                    color: '#FF0000'
                }
                ]
            });

        }
        function setDateTimePicker() {
            $('[id$=txtDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();
                BindDayChart();
                BindShiftData();

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
