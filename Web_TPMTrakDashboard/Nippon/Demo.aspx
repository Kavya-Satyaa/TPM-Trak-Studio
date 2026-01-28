    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="Web_TPMTrakDashboard.Nippon.Demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <link href="../css/elegant-icons-style.css" rel="stylesheet" />
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>
    <style>
        .card {
            background-color: #fff;
            border-radius: 10px;
            position: relative;
            margin-bottom: 30px;
            border: 1px solid #deebfd;
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
            <div class="row clearfix" style="margin-top: 5px;">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                    <div class="tools Year">
                        <div class="col-lg-2 col-md-2 col-sm-4 col-xs-4 col-2">
                            <select id="AssetType" class="form-control">
                                <option value="Plant">Plant</option>
                                <option value="Cell">Cell</option>
                                <option value="Machine">machine</option>
                                <option value="Plant">Plant</option>
                            </select>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-4 col-xs-4 col-2">
                            <div id="checkboxes">
                                <label for="OEE">
                                    <input type="checkbox" id="OEE" />OEE</label>
                                <label for="AE">
                                    <input type="checkbox" id="AE" />AE
                                </label>
                                <label for="PE">
                                    <input type="checkbox" id="PE" />PE
                                </label>
                                <label for="QE">
                                    <input type="checkbox" id="QE" />QE
                                </label>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-4 col-xs-4 col-2">
                            <select id="Type" class="form-control">
                                <option value="Year">Year</option>
                                <option value="Month">Month</option>
                                <option value="Day">Day</option>
                                <option value="Week">Week</option>
                                <option selected="selected" value="YearMonthDay">Year-Month-Day</option>
                                <option value="YearMonthWeek">Year-Month-Week</option>
                            </select>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-4 col-xs-4 col-2">
                            <input type="checkbox" id="RowWise" value="Row Wise View" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-4 col-xs-4 col-2">
                            <input type="button" value="View" id="btnView" class=" btn btn-success" />
                        </div>

                    </div>
                </div>
            </div>
            <div id="DivYearMonth">
                <div class="row clearfix" id="YearMonthBarChart">
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                        <div class=" card">
                            <div class="card-head">
                                <header><b>Year Wise OEE Chart</b></header>
                                <div class="tools Year">
                                    <a><span class="icon_refresh" onclick="RefreshData('Year')"></span></a>
                                    <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('Year')" style="font-size: 20px;"></span></a>
                                </div>
                            </div>
                            <div id="YearlyChart" style="margin:10px">
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                        <div class=" card">
                            <div class="card-head">
                                <header id="MonthHeader"><b></b></header>
                                <div class="tools">
                                    <a><span class="icon_refresh" onclick="RefreshData('Month')"></span></a>
                                    <a><span class="arrow_carrot-down Month" onclick="HideShowMenu('Month')" style="font-size: 20px;"></span></a>
                                </div>
                            </div>
                            <div id="MonthlyChart" style="margin:10px">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row clearfix" id="DayBarChart">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                        <div class="card">
                            <div>
                                <div class="card-head">
                                    <header id="DayHeader"><b></b></header>
                                    <div class="tools">
                                        <a><span class="icon_refresh" onclick="RefreshData('Day')"></span></a>
                                    </div>
                                </div>
                                <div id="DayChart" style="margin:10px">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="DivIndividual">
                <div class="row clearfix" id="OEEAETrend">
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6" id="OEETrend">
                        <div class="card">
                            <div>
                                <div class="card-head">
                                    <header><b>OEE Trend</b></header>
                                    <div class="tools">
                                        <a><span class="icon_refresh" onclick="RefreshData('OEE')"></span></a>
                                        <a><span class="arrow_carrot-down OEE" onclick="HideShowMenu('OEE')" style="font-size: 20px;"></span></a>
                                    </div>
                                </div>
                                <div id="OEEChart" style="margin:10px">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6" id="AETrend">
                        <div class="card">
                            <div>
                                <div class="card-head">
                                    <header><b>AE Trend</b></header>
                                    <div class="tools">
                                        <a><span class="icon_refresh" onclick="RefreshData('AE')"></span></a>
                                        <a><span class="arrow_carrot-down AE" onclick="HideShowMenu('AE')" style="font-size: 20px;"></span></a>
                                    </div>
                                </div>
                                <div id="AEChart" style="margin:10px">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row clearfix" id="PEQETrend">
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6" id="PETrend">
                        <div class="card">
                            <div>
                                <div class="card-head">
                                    <header><b>PE Trend</b></header>
                                    <div class="tools">
                                        <a><span class="icon_refresh" onclick="RefreshData('PE')"></span></a>
                                        <a><span class="arrow_carrot-down PE" onclick="HideShowMenu('PE')" style="font-size: 20px;"></span></a>
                                    </div>
                                </div>
                                <div id="PEChart" style="margin:10px">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6" id="QETrend">
                        <div class="card">
                            <div>
                                <div class="card-head">
                                    <header><b>QE Trend</b></header>
                                    <div class="tools">
                                        <a><span class="icon_refresh" onclick="RefreshData('QE')"></span></a>
                                        <a><span class="arrow_carrot-down QE" onclick="HideShowMenu('QE')" style="font-size: 20px;"></span></a>
                                    </div>
                                </div>
                                <div id="QEChart" style="margin:10px">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>

        var SelectedYear = "";
        var SelectedMonth = "";

        $('#btnView').click(function () {
            HideAllDivision();
            if ($("#Type").val() == "YearMonthDay" || $("#Type").val() == "YearMonthWeek") {
                $("#DivIndividual").css("display", "none");
                $("#DivYearMonth").css("display", "");
                BindChart();
            }
            if ($("#Type").val() == "Year" || $("#Type").val() == "Month" || $("#Type").val() == "Week" || $("#Type").val() == "Day") {
                $("#DivYearMonth").css("display", "none");
                $("#DivIndividual").css("display", "");
                if ($('#OEE')[0].checked) {
                    if ($('#RowWise')[0].checked) {
                        $("#OEETrend").removeClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#OEETrend").addClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    else {
                        $("#OEETrend").addClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#OEETrend").removeClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    $("#OEETrend").css("display", "");
                    GetOEEChart($("#Type").val());
                }
                if ($('#AE')[0].checked) {
                    if ($('#RowWise')[0].checked) {
                        $("#AETrend").removeClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#AETrend").addClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    else {
                        $("#AETrend").addClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#AETrend").removeClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    $("#AETrend").css("display", "");
                    GetAEChart($("#Type").val());
                }
                if ($('#PE')[0].checked) {
                    if ($('#RowWise')[0].checked) {
                        $("#PETrend").removeClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#PETrend").addClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    else {
                        $("#PETrend").addClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#PETrend").removeClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    $("#PETrend").css("display", "");
                    GetPEChart($("#Type").val());
                }
                if ($('#QE')[0].checked) {
                    if ($('#RowWise')[0].checked) {
                        $("#QETrend").removeClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#QETrend").addClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    else {
                        $("#QETrend").addClass("col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6");
                        $("#QETrend").removeClass("col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12");
                    }
                    $("#QETrend").css("display", "");
                    GetQEChart($("#Type").val());
                }

            }
        });

        $(document).ready(function () {
            //BindYearChart();
            //BindMonthChart();
            //BindDayChart();
            BindChart();
        });

        var SameData = {

            title: null,

            tooltip: { enabled: true },
            yAxis: {
                min: 0,
                max: 100

            },
            credits: {
                enabled: false
            },

        };

        function RefreshData(Type) {
            switch (Type) {
                case "OEE":
                    GetOEEChart($("#Type").val());
                    break;
                case "AE":
                    GetAEChart($("#Type").val());
                    break;
                case "QE":
                    GetQEChart($("#Type").val());
                    break;
                case "PE":
                    GetPEChart($("#Type").val());
                    break;
                case "Day":
                    GetDayChartData(SelectedMonth, SelectedYear);
                    break;
                case "Month":
                    GetMonthChartData(SelectedYear);
                    break;
                case "Year":
                    BindChart();
                    break;
            }
        }

        function HideShowMenu(Type) {
            debugger;
            switch (Type) {
                case "OEE":
                    if ($('#OEEChart:visible').length > 0) {
                        $("#OEEChart").css("display", "none");
                    }
                    else {
                        $("#OEEChart").css("display", "");
                    }
                    break;
                case "AE":
                    if ($('#AEChart:visible').length > 0) {
                        $("#AEChart").css("display", "none");
                    }
                    else {
                        $("#AEChart").css("display", "");
                    }
                    break;
                case "QE":
                    if ($('#QEChart:visible').length > 0) {
                        $("#QEChart").css("display", "none");
                    }
                    else {
                        $("#QEChart").css("display", "");
                    }
                    break;
                case "PE":
                    if ($('#PEChart:visible').length > 0) {
                        $("#PEChart").css("display", "none");
                    }
                    else {
                        $("#PEChart").css("display", "");
                    }
                    break;
                case "Month":
                    if ($('#MonthlyChart:visible').length > 0) {
                        $("#MonthlyChart").css("display", "none");
                    }
                    else {
                        $("#MonthlyChart").css("display", "");
                    }
                    break;
                case "Year":
                    if ($('#YearlyChart:visible').length >0) {
                        $("#YearlyChart").css("display", "none");
                    }
                    else{
                        $("#YearlyChart").css("display", "");
                    }
                    break;
            }
        }

        function HideAllDivision() {
            $("#DivYearMonth").css("display", "none");
            $("#DivIndividual").css("display", "none");
            $("#OEETrend").css("display", "none");
            $("#AETrend").css("display", "none");
            $("#PETrend").css("display", "none");
            $("#QETrend").css("display", "none");
        }

        function BindChart() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Demo.aspx/GetYearDATA",
                data: '{}',
                dataType: "json",
                success: function (result) {
                    var YearData = result.d;
                    if (YearData.Data.length > 0) {
                        BindYearChart((YearData));
                        GetMonthChartData(YearData.Catergory[YearData.Catergory.length - 1]);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        };

        function BindYearChart(Data) {
            debugger;
            Highcharts.chart('YearlyChart', Highcharts.merge(SameData, {
                chart: { type: 'column' },
                xAxis: {
                    categories: Data.Catergory,
                    labels: {
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },

                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    GetMonthChartData(this.category);
                                }
                            }
                        }
                    },

                },
                tooltip: {
                    pointFormat: Data.PointerLabel
                },
                yAxis: {
                    title: {
                        text: Data.XaxisLabel
                    }
                },
                series: [{
                    name: Data.XaxisLabel,
                    data: Data.Data,
                    dataLabels: {
                        enabled: true,
                        rotation: -90,
                        color: '#FFFFFF',
                        align: 'right',
                        format: '{point.y:.1f}', // one decimal
                        y: 10, // 10 pixels down from the top
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                }]
            }));
        }

        function GetMonthChartData(Year) {
            SelectedYear = Year
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Demo.aspx/GetMonthDATA",
                data: '{Year:"' + Year + '"}',
                dataType: "json",
                success: function (result) {
                    var MonthData = result.d;
                    if (MonthData.Data.length > 0) {
                        BindMonthChart(MonthData);
                        debugger;
                        $("#MonthHeader").val(MonthData.Title);
                        $("#MonthHeader").text(MonthData.Title);
                        GetDayChartData(MonthData.Catergory[MonthData.Catergory.length - 1], MonthData.Year);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        function BindMonthChart(Data) {
            Highcharts.chart('MonthlyChart', Highcharts.merge(SameData, {
                chart: { type: 'column' },
                xAxis: {
                    categories: Data.Catergory,
                    labels: {
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },
                },
                yAxis: {
                    title: {
                        text: Data.XaxisLabel
                    }
                },
                tooltip: {
                    pointFormat: Data.PointerLabel
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {

                                    GetDayChartData(this.category, SelectedYear);
                                }
                            }
                        }
                    }
                },
                series: [{
                    name: Data.XaxisLabel,
                    data: Data.Data,
                    dataLabels: {
                        enabled: true,
                        rotation: -90,
                        color: '#FFFFFF',
                        align: 'right',
                        format: '{point.y:.1f}', // one decimal
                        y: 10, // 10 pixels down from the top
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                }]
            }));
        }

        function GetDayChartData(Month, Year) {
            SelectedMonth = Month;
            SelectedYear = Year;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Demo.aspx/GetDayDATA",
                data: '{Month:"' + Month + '",Year:"' + Year + '"}',
                dataType: "json",
                success: function (result) {
                    var DayData = result.d;
                    if (DayData.Data.length > 0) {
                        BindDayChart(DayData);
                        debugger;
                        $("#DayHeader").val(DayData.Title);
                        $("#DayHeader").text(DayData.Title);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        function BindDayChart(Data) {
            Highcharts.chart('DayChart', Highcharts.merge(SameData, {
                chart: { type: 'column' },
                xAxis: {
                    categories: Data.Catergory,
                    labels: {
                        //rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },
                    //opposite: true,
                },
                yAxis: {
                    title: {
                        text: Data.XaxisLabel
                    }
                },
                tooltip: {
                    pointFormat: Data.PointerLabel
                },
                series: [{
                    name: Data.XaxisLabel,
                    data: Data.Data,
                    dataLabels: {
                        enabled: true,
                        rotation: -90,
                        color: '#FFFFFF',
                        align: 'right',
                        format: '{point.y:.1f}', // one decimal
                        y: 10, // 10 pixels down from the top
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                }]
            }));
        }

        function GetOEEChart(Type) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Demo.aspx/GetOEEDATA",
                data: '{DateType:"' + Type + '",Asset:"' + $("#AssetType").val() + '"}',
                dataType: "json",
                success: function (result) {
                    debugger;
                    var OEEData = result.d;
                    if (OEEData.Data.length > 0) {
                        BindOEEChart(OEEData);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        function BindOEEChart(Data) {
            Highcharts.chart('OEEChart', Highcharts.merge(SameData, {
                chart: { type: 'line' },
                xAxis: {
                    categories: Data.Catergory,
                    labels: {
                        //rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },
                    //opposite: true,
                },
                yAxis: {
                    title: {
                        text: Data.XaxisLabel
                    }
                },
                tooltip: {
                    pointFormat: Data.PointerLabel
                },
                series: [{
                    name: Data.XaxisLabel,
                    data: Data.Data,

                }]
            }));
        }

        function GetAEChart(Type) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Demo.aspx/GetAEDATA",
                data: '{DateType:"' + Type + '",Asset:"' + $("#AssetType").val() + '"}',
                dataType: "json",
                success: function (result) {
                    var AEData = result.d;
                    if (AEData.Data.length > 0) {
                        BindAEChart(AEData);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        function BindAEChart(Data) {
            Highcharts.chart('AEChart', Highcharts.merge(SameData, {
                chart: { type: 'line' },
                xAxis: {
                    categories: Data.Catergory,
                    labels: {
                        //rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },
                    //opposite: true,
                },
                yAxis: {
                    title: {
                        text: Data.XaxisLabel
                    }
                },
                tooltip: {
                    pointFormat: Data.PointerLabel
                },
                series: [{
                    name: Data.XaxisLabel,
                    data: Data.Data,

                }]
            }));
        }

        function GetPEChart(Type) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Demo.aspx/GetAEDATA",
                data: '{DateType:"' + Type + '",Asset:"' + $("#AssetType").val() + '"}',
                dataType: "json",
                success: function (result) {
                    var PEData = result.d;
                    if (PEData.Data.length > 0) {
                        BindPEChart(PEData);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        function BindPEChart(Data) {
            Highcharts.chart('PEChart', Highcharts.merge(SameData, {
                chart: { type: 'line' },
                xAxis: {
                    categories: Data.Catergory,
                    labels: {
                        //rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },
                    //opposite: true,
                },
                yAxis: {
                    title: {
                        text: Data.XaxisLabel
                    }
                },
                tooltip: {
                    pointFormat: Data.PointerLabel
                },
                series: [{
                    name: Data.XaxisLabel,
                    data: Data.Data,

                }]
            }));
        }

        function GetQEChart(Type) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Demo.aspx/GetAEDATA",
                data: '{DateType:"' + Type + '",Asset:"' + $("#AssetType").val() + '"}',
                dataType: "json",
                success: function (result) {
                    var QEData = result.d;
                    if (QEData.Data.length > 0) {
                        BindQEChart(QEData);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        function BindQEChart(Data) {
            Highcharts.chart('QEChart', Highcharts.merge(SameData, {
                chart: { type: 'line' },
                xAxis: {
                    categories: Data.Catergory,
                    labels: {
                        //rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },
                    //opposite: true,
                },
                yAxis: {
                    title: {
                        text: Data.XaxisLabel
                    }
                },
                tooltip: {
                    pointFormat: Data.PointerLabel
                },
                series: [{
                    name: Data.XaxisLabel,
                    data: Data.Data,

                }]
            }));
        }
    </script>
</body>
</html>
