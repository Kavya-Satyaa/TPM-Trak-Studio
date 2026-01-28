<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rejection.aspx.cs" Inherits="Web_TPMTrakDashboard.Nippon.Rejection" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/drilldown.js"></script>
    <script src="https://code.highcharts.com/modules/pareto.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>

    <link href="../css/elegant-icons-style.css" rel="stylesheet" />
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
        <div>
            <div class="row clearfix" >
                <div class="card-head" >


                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-2 col-4" style="min-width:50px;min-height:150px">
                        <select id="AssetType" class="form-control">
                            <option value="Plant" selected="selected">Plant</option>
                            <option value="Shop">Shop</option>
                            <option value="Cell">Cell</option>
                            <option value="Machine">Machine</option>
                        </select>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-2 col-4">
                        <select id="CatType" class="form-control">
                            <option value="Category" selected="selected">Category</option>
                            <option value="SubCategory">Sub-Category</option>
                            <option value="Reason">Reason</option>
                        </select>
                    </div>

                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-2 col-4">
                        <select id="TimePeriod" class="form-control">
                            <option value="Shift" selected="selected">Shift</option>
                            <option value="Day">Day</option>
                            <option value="Week">Week</option>
                            <option value="Month">Month</option>
                            <option value="Year">Year</option>
                        </select>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-2 col-4">
                        <select id="Displaytype" class="form-control">
                            <option value="HH:mm" selected="selected">HH:mm</option>
                            <option value="HH">Hours</option>
                            <option value="mm">Minutes</option>
                        </select>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-2 col-4">
                        <select id="Charttype" class="col-lg-2 col-md-2 col-sm-4 col-xs-2 col-2 form-control">
                            <option value="Pareto" selected="selected">Pareto</option>
                            <option value="PIE">PIE</option>
                        </select>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-2 col-4">
                        <input type="button" value="View" id="View" class="col-lg-2 col-md-2 col-sm-4 col-xs-2 col-2 form-control" />
                    </div>
                </div>
            </div>

            <div class="row clearfix">
                <div class="card-head" style="padding: 20px;">
                    <ul class="nav nav-pills" style="text-align: center">
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu1">Time Pareto</a></li>
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu3">Frequency Pareto</a></li>
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu2">Time PIE</a></li>
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu4">Frequency PIE</a></li>
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu5">Time Matrix</a></li>
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu6">Frequency Matrix</a></li>
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu7">MTBF Matrix</a></li>
                        <li class="active"><a class="menuData" style="min-width: 175px; max-width: 200px" data-toggle="tab" href="#menu8">MTTF Matrix</a></li>
                    </ul>
                </div>
            </div>
            <div class="row clearfix" id="DayBarChart">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12" id="firstdiv">
                    <div class="card">
                        <div>
                            <div class="card-head">
                                <header><b>Down-Reason Wise Chart</b></header>
                                <div class="tools">
                                    <a><span class="icon_refresh"></span></a>
                                </div>
                            </div>
                            <div id="ReasonParetoPieChart" style="margin: 10px">
                            </div>
                        </div>
                    </div>
                </div>
                <%-- </div>
            <div id="DivYearMonth">
                <div class="row clearfix" id="YearMonthBarChart">--%>
                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                    <div class=" card">
                        <div class="card-head">
                            <header><b>Down-Category Wise Chart</b></header>
                            <div class="tools Year">
                                <a><span class="icon_refresh"></span></a>
                                <a><span class="arrow_carrot-down Year" style="font-size: 20px;"></span></a>
                            </div>
                        </div>
                        <div id="CategoryParetoPieChart" style="margin: 10px">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6" style="display: none" id="divSubCategoryByCategory">
                    <div class=" card">
                        <div class="card-head">
                            <header id="SubCatbyCategory"><b>SubCategory Wise DownTime Chart</b></header>
                            <div class="tools Year">
                                <a><span class="icon_refresh"></span></a>
                                <a><span class="arrow_carrot-down Year" style="font-size: 20px;"></span></a>
                                <a><span class=" icon_close" onclick="CloseTheDiv('SubCategory')" style="font-size: 20px;"></span></a>
                            </div>
                        </div>
                        <div id="CategoryWiseSubCategoryParetoPieChart" style="margin: 10px">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                    <div class=" card">
                        <div class="card-head">
                            <header><b>Down-SubCategory Wise Chart</b></header>
                            <div class="tools">
                                <a><span class="icon_refresh"></span></a>
                                <a><span class="arrow_carrot-down Month" style="font-size: 20px;"></span></a>
                            </div>
                        </div>
                        <div id="SubCategoryParetoPieChart" style="margin: 10px">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6" style="display: none">
                    <div class=" card">
                        <div class="card-head">
                            <header><b>Down-Category Wise Chart</b></header>
                            <div class="tools Year">
                                <a><span class="icon_refresh"></span></a>
                                <a><span class="arrow_carrot-down Year" style="font-size: 20px;"></span></a>
                            </div>
                        </div>
                        <div id="SubCategoryWiseReasonParetoPieChart" style="margin: 10px">
                        </div>
                    </div>
                </div>
                <%--</div>--%>
            </div>
        </div>

    </form>
    <script>
        var Category = "";
        var SubCategory = "";
        $(document).ready(function () {
            BindChart();
        });
        $("#View").click(function () {
            BindChart();
        });
        function BindChart() {
            GetReasonChartData();
            GetCategoryChartData();
            GetSubCategoryChartData();
        }
        function CloseTheDiv(param) {
            switch (param) {
                case "SubCategory":
                    $("#divSubCategoryByCategory").css("display", "none");
                    break;
            }
        }
        function GetReasonChartData() {
            debugger;
            if ($("#Charttype").val() == "Pareto") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetReasonData",
                    data: '{}',
                    dataType: "json",
                    success: function (result) {

                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindReasonChart(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }
            else {

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetReasonPieData",
                    data: '{}',
                    dataType: "json",
                    success: function (result) {

                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindReasonChart(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }

        }
        function BindReasonChart(Data) {
            if ($("#Charttype").val() == "Pareto") {
                Highcharts.chart('ReasonParetoPieChart', {
                    colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                        '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                        '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                    credits: { enabled: false },
                    tooltip: { enabled: false },
                    exporting: { enabled: false },
                    chart: {
                        type: 'column',
                        drilled: false,
                    },
                    title: {
                        text: ''
                    }, xAxis: {
                        categories: Data.Catergory,
                        labels: {
                            style: {
                                fontSize: '12px',
                            }
                        },
                        //type: 'category',
                    }, tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";
                            if (this.series.name == 'Pareto') {
                                value = (this.y).toFixed(2);
                                type = "Cumulative Value (%)";
                            } else {

                                var time = this.y;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                type = "DownTime (HH:mm)";
                                //var hours1 = parseInt(time / (60*60));
                                //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                                //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            }
                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    yAxis: [{
                        title: {
                            text: 'Time',
                            style: {
                                color: '#525151',
                                fontSize: '12px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        },
                        labels: {
                            style: {
                                fontSize: '12px'
                            },
                            formatter: function () {
                                var value = '';
                                //if (this.chart.series[0].name == 'Pareto') {
                                //value = this.y;
                                //} else {
                                var time = this.value;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                // }
                                return value;
                            }
                        }
                    }, {
                        title: {
                            text: ''
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        tickInterval: 25,
                        labels: {
                            format: "{value}%"
                        }
                    }],
                    plotOptions: {
                        series: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    var value = '';
                                    if (this.series.name != 'Pareto') {
                                        var time = this.y;
                                        var hours1 = parseInt(time / 60);
                                        var mins1 = parseInt(time % 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                    }
                                    return value;
                                }
                            }, style: {
                                fontSize: '12px'
                            }
                        }
                    },
                    series: [{
                        type: 'pareto',//pareto
                        name: 'Pareto',
                        yAxis: 1,
                        zIndex: 10,
                        baseSeries: 1

                    }, {
                        name: 'DownID',
                        type: 'column',
                        colorByPoint: true,
                        zIndex: 2,
                        data: Data.Data,
                    }],
                });
            }
            else {
                Highcharts.chart('ReasonParetoPieChart', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie',
                        title: null,
                    },
                    credits: { enabled: false },
                    title: {
                        text: ''
                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";



                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            type = "DownTime (HH:mm)";
                            //var hours1 = parseInt(time / (60*60));
                            //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                            //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);

                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    xAxis: {
                        categories: Data.Catergory,
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                            }
                        }
                    },
                    series: [{
                        name: 'DownTime',
                        colorByPoint: true,
                        data: Data.Data
                    }]
                });
            }

        }
        function GetCategoryChartData() {
            if ($("#Charttype").val() == "Pareto") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetCategoryData",
                    data: '{}',
                    dataType: "json",
                    success: function (result) {
                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindCategoryChart(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetCategoryPieData",
                    data: '{}',
                    dataType: "json",
                    success: function (result) {
                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindCategoryChart(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }
        }
        function BindCategoryChart(Data) {
            if ($("#Charttype").val() == "Pareto") {
                Highcharts.chart('CategoryParetoPieChart', {
                    colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                        '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                        '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                    credits: { enabled: false },
                    tooltip: { enabled: false },
                    exporting: { enabled: false },
                    chart: {
                        type: 'column',
                        drilled: false,
                    },
                    title: {
                        text: ''
                    }, xAxis: {
                        categories: Data.Catergory,
                        labels: {
                            style: {
                                fontSize: '12px',
                            }
                        },
                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";
                            if (this.series.name == 'Pareto') {
                                value = (this.y).toFixed(2);
                                type = "Cumulative Value (%)";
                            } else {

                                var time = this.y;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                type = "DownTime (HH:mm)";
                                //var hours1 = parseInt(time / (60*60));
                                //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                                //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            }
                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    yAxis: [{
                        title: {
                            text: 'Time',
                            style: {
                                color: '#525151',
                                fontSize: '12px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        },
                        labels: {
                            style: {
                                fontSize: '12px'
                            },
                            formatter: function () {
                                var value = '';
                                //if (this.chart.series[0].name == 'Pareto') {
                                //value = this.y;
                                //} else {
                                var time = this.value;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                // }
                                return value;
                            }
                        }
                    }, {
                        title: {
                            text: ''
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        tickInterval: 25,
                        labels: {
                            format: "{value}%"
                        }
                    }],
                    plotOptions: {
                        series: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    var value = '';
                                    if (this.series.name != 'Pareto') {
                                        var time = this.y;
                                        var hours1 = parseInt(time / 60);
                                        var mins1 = parseInt(time % 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                    }
                                    return value;
                                }
                            }, style: {
                                fontSize: '12px'
                            },
                            point: {
                                events: {
                                    click: function () {

                                        GetSubCategoryChartDataByCategory(this.category);
                                    }
                                }
                            }
                        }
                    },
                    series: [{
                        type: 'pareto',//pareto
                        name: 'Pareto',
                        yAxis: 1,
                        zIndex: 10,
                        baseSeries: 1

                    }, {
                        name: 'DownID',
                        type: 'column',
                        colorByPoint: true,
                        zIndex: 2,
                        data: Data.Data,
                    }],
                });
            }
            else {
                Highcharts.chart('CategoryParetoPieChart', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: ''
                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";



                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            type = "DownTime (HH:mm)";
                            //var hours1 = parseInt(time / (60*60));
                            //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                            //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);

                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    xAxis: {
                        categories: Data.Catergory,
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                            },
                            point: {
                                events: {
                                    click: function () {
                                        debugger;
                                        GetSubCategoryChartDataByCategory(this.category);
                                    }
                                }
                            }
                        },

                    },
                    series: [{
                        name: 'DownTime',
                        colorByPoint: true,
                        data: Data.Data
                    }]
                });
            }
        }
        function GetSubCategoryChartDataByCategory(category) {
            Category = category;
            if ($("#Charttype").val() == "Pareto") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetSubCategoryDataByCategory",
                    data: '{Category:"' + category + '"}',
                    dataType: "json",
                    success: function (result) {

                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindSubCategoryChabrtByCategory(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetSubCategoryPieDataByCategory",
                    data: '{Category:"' + category + '"}',
                    dataType: "json",
                    success: function (result) {

                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindSubCategoryChabrtByCategory(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }
        }

        function BindSubCategoryChabrtByCategory(Data) {
            $("#divSubCategoryByCategory").css("display", "");
            $("#SubCatbyCategory").text("Sub Category Chart For Categorty :" + Category);
            if ($("#Charttype").val() == "Pareto") {
                var subcat = Highcharts.chart('CategoryWiseSubCategoryParetoPieChart', {
                    colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                        '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                        '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                    credits: { enabled: false },
                    tooltip: { enabled: false },
                    exporting: { enabled: false },
                    chart: {
                        type: 'column',
                        events: {
                            drillup: function (e) {
                                debugger;
                                if (e.seriesOptions.id == undefined) {

                                    catValue = '';
                                    contValue = 0;
                                    subcat.xAxis[0].setCategories(Data.Catergory);
                                    subcat.series.data = Data.Series;
                                    subcat.series[0].setName("DownID");
                                }
                            },
                        }
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: Data.Catergory,
                        labels: {
                            style: {
                                fontSize: '12px',
                            }
                        },
                        //type: 'category',
                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";
                            if (this.series.name == 'Pareto') {
                                value = (this.y).toFixed(2);
                                type = "Cumulative Value (%)";
                            } else {

                                var time = this.y;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                type = "DownTime (HH:mm)";
                            }
                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    yAxis: [{
                        title: {
                            text: 'Time',
                            style: {
                                color: '#525151',
                                fontSize: '12px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        },
                        labels: {
                            style: {
                                fontSize: '12px'
                            },
                            formatter: function () {
                                var value = '';
                                //if (this.chart.series[0].name == 'Pareto') {
                                //value = this.y;
                                //} else {
                                var time = this.value;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                // }
                                return value;
                            }
                        }
                    }, {
                        title: {
                            text: ''
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        tickInterval: 25,
                        labels: {
                            format: "{value}%"
                        }
                    }],
                    plotOptions: {
                        series: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    var value = '';
                                    if (this.series.name != 'Pareto') {
                                        var time = this.y;
                                        var hours1 = parseInt(time / 60);
                                        var mins1 = parseInt(time % 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                    }
                                    return value;
                                }
                            }, style: {
                                fontSize: '12px'
                            }
                        },

                    },
                    series: [{
                        type: 'pareto',
                        name: 'Pareto',
                        yAxis: 1,
                        zIndex: 10,
                        baseSeries: 1,
                    },
                    {
                        name: 'DownID',
                        colorByPoint: true,
                        data: Data.Series
                    }],
                    drilldown: {
                        series: Data.DrillDownData
                    }

                });
            }
            else {
                Highcharts.chart('CategoryWiseSubCategoryParetoPieChart', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: ''
                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";



                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            type = "DownTime (HH:mm)";
                            //var hours1 = parseInt(time / (60*60));
                            //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                            //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);

                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    xAxis: {
                        categories: Data.Catergory,
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                            },
                            point: {
                                events: {
                                    click: function () {
                                        debugger;
                                        GetSubCategoryChartDataByCategory(this.category);
                                    }
                                }
                            }
                        },

                    },
                    series: [{
                        name: 'DownTime',
                        colorByPoint: true,
                        data: Data.Data
                    }]
                });
            }
        }

        function GetSubCategoryChartData() {
            debugger;
            if ($("#Charttype").val() == "Pareto") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetSubCategoryData",
                    data: '{}',
                    dataType: "json",
                    success: function (result) {

                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindSubCategoryChart(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Rejection.aspx/GetSubCategoryPieData",
                    data: '{}',
                    dataType: "json",
                    success: function (result) {

                        var ReasonData = result.d;
                        if (ReasonData.Data.length > 0) {
                            BindSubCategoryChart(ReasonData);
                        }
                    },
                    error: function (Result) {
                        alert("Error Retreving Data");
                    }
                });
            }
        }
        function BindSubCategoryChart(Data) {
            if ($("#Charttype").val() == "Pareto") {
                Highcharts.chart('SubCategoryParetoPieChart', {
                    colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                        '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                        '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                    credits: { enabled: false },
                    tooltip: { enabled: false },
                    exporting: { enabled: false },
                    chart: {
                        type: 'column',
                        drilled: false,
                    },
                    title: {
                        text: ''
                    }, xAxis: {
                        categories: Data.Catergory,
                        labels: {
                            style: {
                                fontSize: '12px',
                            }
                        },
                        //type: 'category',
                    }, tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";
                            if (this.series.name == 'Pareto') {
                                value = (this.y).toFixed(2);
                                type = "Cumulative Value (%)";
                            } else {

                                var time = this.y;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                type = "DownTime (HH:mm)";
                                //var hours1 = parseInt(time / (60*60));
                                //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                                //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            }
                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    yAxis: [{
                        title: {
                            text: 'Time',
                            style: {
                                color: '#525151',
                                fontSize: '12px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        },
                        labels: {
                            style: {
                                fontSize: '12px'
                            },
                            formatter: function () {
                                var value = '';
                                //if (this.chart.series[0].name == 'Pareto') {
                                //value = this.y;
                                //} else {
                                var time = this.value;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                // }
                                return value;
                            }
                        }
                    }, {
                        title: {
                            text: ''
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        tickInterval: 25,
                        labels: {
                            format: "{value}%"
                        }
                    }],
                    plotOptions: {
                        series: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    var value = '';
                                    if (this.series.name != 'Pareto') {
                                        var time = this.y;
                                        var hours1 = parseInt(time / 60);
                                        var mins1 = parseInt(time % 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                    }
                                    return value;
                                }
                            }, style: {
                                fontSize: '12px'
                            }
                        }
                    },
                    series: [{
                        type: 'pareto',//pareto
                        name: 'Pareto',
                        yAxis: 1,
                        zIndex: 10,
                        baseSeries: 1

                    }, {
                        name: 'DownID',
                        type: 'column',
                        colorByPoint: true,
                        zIndex: 2,
                        data: Data.Data,
                    }],
                });
            }
            else {
                Highcharts.chart('SubCategoryParetoPieChart', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: ''
                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            var value = 0; var type = "";



                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            type = "DownTime (HH:mm)";
                            //var hours1 = parseInt(time / (60*60));
                            //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                            //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);

                            return '<b>' + this.series.name + '</b><br/>' +
                                type + ': ' + value;
                        }
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    xAxis: {
                        categories: Data.Catergory,
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                            }
                        }
                    },
                    series: [{
                        name: 'DownTime',
                        colorByPoint: true,
                        data: Data.Data
                    }]
                });
            }
        }

    </script>
</body>
</html>
