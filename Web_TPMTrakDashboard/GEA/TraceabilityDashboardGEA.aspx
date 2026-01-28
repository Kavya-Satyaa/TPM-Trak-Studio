<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TraceabilityDashboardGEA.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.TraceabilityDashboardGEA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/HighCharts10.3.2/highcharts.js"></script>
    <script src="../Scripts/HighCharts10.3.2/highcharts-more.js"></script>
    <script src="../Scripts/HighCharts10.3.2/solid-gauge.js"></script>
    <script src="../Scripts/HighCharts10.3.2/exporting.js"></script>
    <script src="../Scripts/HighCharts10.3.2/export-data.js"></script>
    <script src="../Scripts/HighCharts10.3.2/accessibility.js"></script>
    <style>
        .lbl-header {
            text-align: center;
            font-size: 20px;
            color: white;
            background-color: darkblue;
            margin: auto;
            width: fit-content;
            padding: 3px 10px;
        }

        .dark-backcolor {
            background-color: white;
            box-shadow: 2px 3px 5px 2px silver;
            vertical-align: top;
            border: 2px solid silver;
        }

        .lbl-style2 {
            font-size: 30px;
            font-weight: 600;
            width: fit-content;
            margin: auto;
            background-color: #e4e4e4;
            background-color: #fff328;
            padding: 20px;
            margin-top: 40px;
            min-width: 200px;
            text-align: center;
            border: 1px solid silver;
        }

        .headerFixer tr td {
            background-color: white;
        }
    </style>
    <div id="tblFilter" style="background-color: #21217d; height: 40px; margin-bottom: 7px; padding-top: 3px;">
        <asp:DropDownList runat="server" ID="ddlViewType" ClientIDMode="Static" CssClass="form-control" onchange="BindData();" Style="width: auto; margin-left: auto; margin-right: 30px;">
            <asp:ListItem Text="Week" Value="Week"></asp:ListItem>
            <asp:ListItem Text="Month" Value="Month"></asp:ListItem>
            <%--  <asp:ListItem Text="Day" Value="Day"></asp:ListItem>--%>
        </asp:DropDownList>
    </div>
    <div>
        <table style="width: 100%; table-layout: fixed">
            <tr>
                <td class="dark-backcolor">
                    <div class="lbl-header">
                        Receipt Completion
                    </div>
                    <div id="receiptCompletionContainer"></div>
                </td>
                <td style="width: 10px;"></td>
                <td class="dark-backcolor">
                    <div class="lbl-header" id="lblTimeWaitingHeader">
                        Time waited at stores in a day / week        
                    </div>
                    <div class="lbl-style2" id="lblTotalDownTime">
                        HH:mm:ss
                    </div>
                </td>
                <td style="width: 10px;"></td>
                <td class="dark-backcolor">
                    <div class="lbl-header">Stock Receipt</div>
                    <div id="lblStockAvailability" class="lbl-style2">16</div>
                </td>
            </tr>
        </table>
        <table style="width: 100%; table-layout: fixed; margin-top: 10px">
            <tr>
                <td class="dark-backcolor">
                    <div id="weeklyBarChart" class="columnChartDiv"></div>
                </td>
                <td style="width: 10px"></td>
                <td class="dark-backcolor">
                    <div id="weeklyBarChart2" class="columnChartDiv"></div>
                </td>
            </tr>
        </table>
        <div id="storeDataContainer" style="margin-top: 10px; overflow: auto">
            <table id="tblStoresData" class="table table-bordered table-hover headerFixer">
                <tr>
                    <th>Sl. No.</th>
                    <th>Machine ID</th>
                    <th>Date of Schedule</th>
                    <th>PO No.</th>
                    <th>Component ID</th>
                    <th>Component Desc</th>
                    <th>Series No.</th>
                    <th>Cycle End Time</th>
                    <th>Time waited at Stores</th>
                    <th>Receiver Name</th>
                    <th>Completion DateTime</th>
                </tr>
            </table>
        </div>
    </div>

    <script>
        var RefreshIntervalCounter;
        var RefreshIntervalTime = 15000;
        $(document).ready(function () {
            $('#sidebar').hide();
            $('#main-content').css('margin-left', '0px');
            $('.wrapper').css("padding-top", "0px");
            document.body.style.backgroundColor = "#dfdfdf";
            setHeight();
            BindData();
        });
        function setHeight() {
            var height = $(window).height() - $('.header ').height() - $('#tblFilter').val() - 220 - 100;
            height = height / 2;
            $('.columnChartDiv').css('height', height);
            $('#storeDataContainer').css('height', height);
        }
        function BindData() {
            clearInterval(RefreshIntervalCounter);
            $('#lblTotalDownTime').text("00:00:00");
            $('#lblStockAvailability').text("0");
            $('#lblTimeWaitingHeader').text("Time waited at stores in a " + $('#ddlViewType').val());
            $.ajax({
                async: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "TraceabilityDashboardGEA.aspx/getTraceabilityData",
                data: '{viewType:"' + $('#ddlViewType').val() + '"}',
                dataType: "json",
                success: function (result) {
                    var chartData = result.d;
                    $('#lblTotalDownTime').text(chartData.TotalDownTime);
                    $('#lblStockAvailability').text(chartData.StockAvailability);
                    BindReceiptCompletionChart(chartData.ReceiptCompletion);
                    BindWeeklyBarChart(chartData.WeeklyChartData);
                    BindStackedBarChart(chartData.MaterialStatusChartData);
                    BindStoresData(chartData.StoresList);
                    RefreshIntervalCounter = setInterval(function () {
                        BindData();
                    }, RefreshIntervalTime);
                },
                error: function (Result) {
                    //$('#loadingModal').modal('hide');
                    $.unblockUI({});
                    alert("Error");
                }
            });
        }
        function BindReceiptCompletionChart(value) {
            var color = "green";
            Highcharts.chart('receiptCompletionContainer', {
                chart: {
                    type: 'solidgauge',
                    height: 180,
                    backgroundColor: null,
                    margin: 3,
                },

                title: null,

                pane: {
                    center: ['50%', '85%'],
                    size: '150%',
                    startAngle: -90,
                    endAngle: 90,
                    background: {
                        innerRadius: '70%',
                        outerRadius: '100%',
                        shape: 'arc'
                    }
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },

                tooltip: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                // the value axis
                yAxis: {
                    min: 0,
                    max: 100,
                    lineWidth: 0,
                    tickWidth: 0,
                    minorTickInterval: null,
                    stops: [
                        [0.9, color] // green
                    ],
                    labels: {
                        distance: 10,
                        enabled: true,
                        style: {
                            fontSize: '10px',
                        }

                    }
                },


                plotOptions: {
                    solidgauge: {
                        innerRadius: '70%',
                        dataLabels: {
                            y: 0,
                            borderWidth: 0,
                            useHTML: true,
                            enabled: true,
                        },

                    },
                    series: {
                        animation: false
                    },
                    marker: {
                        enabled: true,
                        symbol: 'triangle',
                    }
                },
                series: [{
                    name: 'Speed',
                    data: [value],
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px;color:' + color + '">{y}%</span><br/>' +
                            //'<span style="font-size:16px;opacity:1">' + effy + '</span>' +
                            '</div>'
                    },
                },
                    // dial series (gauge)
                    //{
                    //    name: '',
                    //    data: [value],
                    //    type: 'gauge',
                    //    dial: {
                    //        baseLength: "100%",
                    //        radius: "105%",
                    //        rearLength: "-70%"
                    //    },
                    //    dataLabels: {
                    //        enabled: false
                    //    }
                    //}
                ],

            });
        }
        function BindWeeklyBarChart(chartData) {
            Highcharts.chart('weeklyBarChart', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: $('#ddlViewType').val() + 'ly Data',
                    style: {
                        fontSize: 18,
                        fontWeight: 'bold'
                    }
                },
                legend: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                xAxis: {
                    categories: chartData.Category,
                    crosshair: true,
                    labels: {
                        style: {
                            fontSize: 13,
                            color: 'black',
                            fontWeight: 'bold'
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Qty',
                        style: {
                            fontSize: 15,
                            fontWeight: 'bold',
                            color: 'black',

                        }
                    },
                    labels: {
                        style: {
                            fontSize: 13,
                            color: 'black',
                            fontWeight: 'bold'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                color: 'black',
                                fontSize: 15,
                                textOutline: 'transparent'
                            }
                        },
                        animation: false
                    }
                },
                series: [{
                    name: '',
                    data: chartData.Data,
                    color: '#FAA43A'

                }]
            });
        }
        function BindStackedBarChart(chartData) {
            Highcharts.chart('weeklyBarChart2', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: $('#ddlViewType').val() + 'ly : Material and Status',
                    style: {
                        fontSize: 18,
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false,
                },
                exporting: {
                    enabled: false
                },
                xAxis: {
                    categories: chartData.Category,
                    labels: {
                        style: {
                            fontSize: 13,
                            color: 'black',
                            fontWeight: 'bold'
                        }
                    }
                },
                yAxis: {
                    //min: 0,
                    //max: 100,
                    title: {
                        text: ''
                    },
                    labels: {
                        style: {
                            fontSize: 13,
                            color: 'black',
                            fontWeight: 'bold'
                        }
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        dataLabels: {
                            enabled: true
                        },
                        animation: false
                    },

                },
                series: [{
                    name: 'Total Qty. waiting at Stores',
                    data: chartData.Data,
                    color: '#5DA5DA'
                }, {
                    /*name: 'Completed',*/
                    name: 'Stock Receipt Completed',
                    data: chartData.Data2,
                    color: '#FAA43A'
                }]
            });
        }
        function BindStoresData(stoesDataList) {
            $('#tblStoresData tr:gt(0)').empty();
            var appendStr = "";
            for (var i = 0; i < stoesDataList.length; i++) {
                appendStr += '<tr>';
                appendStr += '<td>' + stoesDataList[i].SNo + '</td>';
                appendStr += '<td>' + stoesDataList[i].MachineID + '</td>';
                appendStr += '<td>' + stoesDataList[i].DateOfSchedule + '</td>';
                appendStr += '<td>' + stoesDataList[i].ProductionOrderNo + '</td>';
                appendStr += '<td>' + stoesDataList[i].CompID + '</td>';
                appendStr += '<td>' + stoesDataList[i].CompDesc + '</td>';
                appendStr += '<td>' + stoesDataList[i].seriesNo + '</td>';
                appendStr += '<td>' + stoesDataList[i].CycleTime + '</td>';
                appendStr += '<td>' + stoesDataList[i].TimeWaitingAtStores + '</td>';
                appendStr += '<td>' + stoesDataList[i].ReceiverName + '</td>';
                appendStr += '<td>' + stoesDataList[i].DateTimeCompletion + '</td>';
                appendStr += '</tr>';
            }
            $('#tblStoresData').append(appendStr);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
