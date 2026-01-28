<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuntimeChart.aspx.cs" Inherits="Web_TPMTrakDashboard.Nippon.RuntimeChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="../css/elegant-icons-style.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/highstock.js"></script>
    <script src="../js/moment.js"></script>
    <script src="../js/offline-exporting.js"></script>
    <script src="../js/xrange.js"></script>
    <style>
        .card {
            background-color: #fff;
            border-radius: 10px;
            position: relative;
            margin-bottom: 30px;
            border: 1px solid #deebfd;
            box-shadow: -8px 12px 18px 0 #dadee8;
            position: relative;
            display: flex;
            flex-direction: column;
            min-width: 0;
            word-wrap: break-word;
            background-color: #fff;
            background-clip: border-box;
            border: 1px solid rgba(0,0,0,.125);
            border-radius: .25rem;
            border-radius: 24px;
            margin: 10px;
        }

        .card-head {
            border-radius: 2px 2px 0 0;
            border-bottom: 1px dotted rgba(0, 0, 0, 0.2);
            padding: 2px;
            text-transform: uppercase;
            color: #3a405b;
            font-size: 14px;
            font-weight: 600;
            line-height: 40px;
            min-height: 40px;
        }

            .card-head .tools {
                padding-right: 16px;
                float: right;
                margin-top: 7px;
                margin-bottom: 7px;
                margin-left: 24px;
                line-height: normal;
                vertical-align: middle;
            }

                .card-head .tools .btn-color {
                    color: #97a0b3;
                    margin-right: 3px;
                    font-size: 12px;
                }

        .row {
            --bs-gutter-x: 1.5rem;
            --bs-gutter-y: 0;
            display: flex;
            flex-wrap: wrap;
            margin-top: calc(var(--bs-gutter-y) * -1);
            margin-right: calc(var(--bs-gutter-x)/ -2);
            margin-left: calc(var(--bs-gutter-x)/ -2);
        }

        .card-head header {
            display: inline-block;
            padding: 11px 20px;
            vertical-align: middle;
            line-height: 17px;
            font-size: 20px;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row clearfix" style="margin-top: 5px;">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <div class=" card">
                    <div class="card-head">
                        <header><b>Run Time Chart</b></header>
                        <div class="tools Year">
                            <a><span class="icon_refresh" onclick="RefreshData('Year')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('Year')" style="font-size: 20px;"></span></a>
                        </div>
                    </div>
                    <div id="container-col">
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        $(document).ready(function () {
            debugger;
            BuildChart()
        })
        function BuildChart(data) {
            console.log();
            //if (data != null) {

            Highcharts.chart('container-col', {
                chart: {
                    type: 'xrange',
                    height: 600
                },
                title: {
                    text: ''
                },
                xAxis: {
                    type: 'datetime'
                },
                yAxis: {
                    title: {
                        text: ''
                    },

                    categories: ["M1", "M2", "M3"],
                },

                tooltip: {
                    formatter: function () {
                        let tmpTooltip = '<span style="color:' + this.point.color + '">\u25CF</span> ' + ': <b>' + this.series.chart.yAxis[0].categories[this.y] + '</b><br/> Start Time :  <b>' +
                            Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '</b><br/> End Time :  <b>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x2) + '</b><br/> Status :  <b>' +
                            this.point.status + '</b>';

                        return tmpTooltip;

                    }
                },

                navigator: {
                    enabled: true,
                    xAxis: {

                        type: 'datetime'
                    }
                },

                exporting: {
                    enabled: false
                },
                rangeSelector: {
                    verticalAlign: 'top',
                    x: 0,
                    y: 0,
                    buttons: [{
                        type: 'minute',
                        count: 5,
                        text: '5M'
                    }, {
                        type: 'minute',
                        count: 10,
                        text: '10M'
                    }, {
                        type: 'minute',
                        count: 20,
                        text: '20M'
                    }, {
                        type: 'minute',
                        count: 30,
                        text: '30M'
                    }, {
                        type: 'minute',
                        count: 45,
                        text: '45M'
                    }, {
                        type: 'hour',
                        count: 5,
                        text: '5h'
                    }, {
                        type: 'hour',
                        count: 8,
                        text: '8h'
                    }, {
                        type: 'hour',
                        count: 10,
                        text: '10h'
                    }, {
                        type: 'hour',
                        count: 20,
                        text: '20h'
                    }, {
                        type: 'all',
                        text: 'All'
                    }],
                    selected: 6,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                //series: data.series,
                series: [{
                    name: '',
                    // pointPadding: 0,
                    // groupPadding: 0,
                    borderColor: 'gray',
                    pointWidth: 50,
                    //data: data.series.data,
                    //turboThreshold: data.series.data.length,
                    data: [{ "y": 1, "x": 1516349457000, "x2": 1516357084000 }, { "y": 2, "x": 1516347595000, "x2": 1516350061000 }, { "y": 2, "x": 1516367206000, "x2": 1516372249000 }, { "y": 4, "x": 1516357424000, "x2": 1516362888000 }, { "y": 2, "x": 1516349405000, "x2": 1516353312000 }, { "y": 4, "x": 1516352416000, "x2": 1516356200000 }, { "y": 4, "x": 1516349449000, "x2": 1516351853000 }, { "y": 5, "x": 1516349235000, "x2": 1516352661000 }, { "y": 3, "x": 1516350622000, "x2": 1516354105000 }],
                    //// dataLabels: {
                    ////    enabled: true
                    ////} 
                }]

            });
            //}
        }
    </script>
</body>

</html>
