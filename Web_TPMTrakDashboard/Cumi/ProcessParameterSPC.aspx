<%@ Page Title="Energy SPC Trend" Trace="false" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterSPC.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.ProcessParameterSPC" meta:resourcekey="PageResource1" AsyncTimeout="6000" %>

<%--<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%--    <%: Scripts.Render("~/bundles/VDGjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>--%>


    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>

    <style>
        /*   .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }*/
        .inner-chart-div {
            /*margin-bottom: 10px;*/
            min-width: 380px;
            max-width: 380px;
            /*height: 210px;*/
            display: inline-block;
            /*border-radius: 8px;*/
            background-color: white;
        }

        .inner-parameter-div {
            /*margin: 10px;*/
            min-width: 380px;
            max-width: 380px;
            /*height: 210px;*/
            display: inline-block;
            border-radius: 8px;
            background-color: #29b7a9;
            /*background-color:#2775ea;*/
        }

        .inner-div {
            margin: 10px;
            min-width: 380px;
            max-width: 380px;
            /*height: 210px;*/
            /*height: 323px;*/
            display: inline-block;
            /*border-radius: 8px;*/
            /*background-color: #29b7a9;*/
            /*background-color:#2775ea;*/
        }

        .inner-table {
            width: 100%;
            border: none;
            border-collapse: collapse;
            height: 100%;
        }

            .inner-table tr td {
                border: none;
                /*font-weight: bold;*/
                font-size: 20px;
                text-align: center;
            }

        .td-header {
            vertical-align: middle;
            height: 50px;
            border-radius: 5px 5px 0px 0px;
        }

        .lbl-header {
            font-size: 18px;
            display: inline-block;
            overflow: hidden;
            width: 370px;
            min-width: 370px;
            text-overflow: ellipsis;
            white-space: nowrap;
            /*font-weight: bold;*/
        }


        .lbl-parameter-value {
            background-color: white;
            width: 95px;
            max-width: 95px;
            padding: 5px 2px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .table-min-max {
            width: 100%;
        }

            .table-min-max tr td {
                padding: 5px;
            }

        .lbl-min-max {
            vertical-align: super;
        }

        .lbl-min-max-value {
            background-color: white;
            width: 95px;
            max-width: 95px;
            padding: 5px 2px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            vertical-align: sub;
        }

        .lbl-values {
            min-height: 38px;
        }

        fieldset {
            border: 1px groove #d5d5d5 !important;
            /*     padding: 0.1em 0.5em 1em !important;*/
            margin: 0 0 1.5em 1em !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 2px 2px 6px 3px #060606;
            /*font-weight: bold;*/
            height: 93px;
            padding: 0px 8px;
            width: fit-content;
            background-color: #222e70;
            /*border-radius: 2px;*/
        }

        legend {
            font-size: 1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
            margin-top: -4px;
            color: white;
            margin-bottom: 0px;
        }

        .cumi-tbl-details tr td {
            background-color: white;
            padding: 8px;
            border: 1px solid #dfdede;
        }

        .cumi-filter-tbl tr td {
            color: white;
            border: unset;
            padding: 0px 10px;
        }

        .div-other-details {
            height: 78vh;
            overflow: auto;
            border: 1px solid #192d9b;
            padding: 20px 10px;
            border-radius: 7px;
            /*background-color: #1d2862;*/
            /*background-color:#2775ea;*/
            background-color: #6d9de7;
            box-shadow: 2px 2px 6px 3px #060606;
        }

        .div-parameter-details {
            height: 78vh;
            overflow: auto;
        }

        .switch {
            position: relative;
            display: inline-block;
            vertical-align: middle;
            width: 50px;
            height: 30px;
            /*float: right;*/
            margin: 5px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 22px;
                width: 22px;
                left: 3px;
                bottom: 3px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(23px);
            -ms-transform: translateX(23px);
            transform: translateX(23px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 30px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        /*td span{
                font-size: x-large;
            }*/

        /*.checkbox {
            font-size: large;
            font-weight: bold;
            color: black;
        }

        .multiselect-selected-text {
            font-size: small;
        }

        .multiselect {
            background-color: white;
        }

        input {
            font-size: initial;
        }

        .datepicker-days {
            display: block;
            font-size: small;
        }
        select{
            font-size: initial;
        }*/
        .card {
            width: fit-content;
            display: inline-block;
            margin-left: 10px;
        }

        #DivChart.fullscreen {
            z-index: 9999;
            width: 100vw;
            height: 100vh;
            position: absolute;
            top: 0;
            left: 0;
        }
    </style>

    <div class="content-div">
        <div class="row">
            <div class="col-lg-3">
                <asp:UpdatePanel ID="updatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Selection Criteria</legend>
                            <table class="cumi-filter-tbl">
                                <tr>
                                    <%--<td>
                                    <label class="filter-lbl-name">Plant</label>
                                    <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" onchange="BindMachineID();" Style="font-size: initial;"></asp:DropDownList>
                                </td>--%>
                                    <td>
                                        <label class="filter-lbl-name">Machine</label><br />
                                        <%--OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"    onchange="return BindMachineParameters();"     --%>
                                        <asp:DropDownList ClientIDMode="Static" ID="ddlMachine" runat="server" CssClass="form-control specInputField" Style="margin: 0 10px 0 0; min-width: 100px; width: auto"></asp:DropDownList>
                                    </td>
                                    <td class="" runat="server">
                                        <label class="filter-lbl-name">Year</label><br />
                                        <asp:TextBox ID="txtFromDateMonth" runat="server" CssClass="form-control" placeholder="Year" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" meta:resourcekey="txtDateMonthResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td class="" runat="server">
                                        <label class="filter-lbl-name">Month</label><br />
                                        <asp:TextBox ID="txtToDateMonth" runat="server" CssClass="form-control" placeholder="Month" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" meta:resourcekey="txtDateMonthResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnProcess" Text="Process" OnClientClick="btnProcessClicked()" CssClass="btn btn-info btn-sm" Style="margin-left: 10px; margin-left: 10px; margin-top: 24px; width: 100px; height: 35px;" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>

                        <%-- <div id="containerbottom">
                            <div class="row" id="DivChart">
                                <div class="col-lg-12" style="padding: 0 1px 0 1px" id="footerchart">
                                    <div id="productionDataChart" style="display: none; width: 100%; min-height: 250px;">
                                    </div>
                                </div>
                            </div>
                        </div>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div id="divSPCMasterChart" class="col-lg-12 col-md-12 col-sm-12">
                <%--<div id="divSPCDaily" style="display: none; width: 100%; background-color: white; min-height: 250px;">
                </div>--%>
                <div id="divSPCDaily" style="width: 100%; height: 500px; margin: 0 auto"></div>
            </div>
        </div>
        <%--<br />--%>
        <hr />
        <div class="row">
            <div id="divSPCMasterMonthlyChart" class="col-lg-12 col-md-12 col-sm-12">
                <%--<div id="divSPCDaily" style="display: none; width: 100%; background-color: white; min-height: 250px;">
                </div>--%>
                <div id="divSPCMonthly" style="width: 100%; height: 500px; margin: 0 auto; background-color: "></div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            SetControl();
            var fromDate = $("[id$=txtFromDateMonth]").val();
            var toDate = $("[id$=txtToDateMonth]").val();
            var selectedMachineId = $("[id$=ddlMachine]").val();
            switch (toDate) {
                case 'Jan':
                    toDate = 1;
                    break;
                case 'Feb':
                    toDate = 2;
                    break;
                case 'Mar':
                    toDate = 3;
                    break;
                case 'Apr':
                    toDate = 4;
                    break;
                case 'May':
                    toDate = 5;
                    break;
                case 'Jun':
                    toDate = 6;
                    break;
                case 'Jul':
                    toDate = 7;
                    break;
                case 'Aug':
                    toDate = 8;
                    break;
                case 'Sep':
                    toDate = 9;
                    break;
                case 'Oct':
                    toDate = 10;
                    break;
                case 'Nov':
                    toDate = 11;
                    break;
                case 'Dec':
                    toDate = 12;
                    break;
            }
            PlotProcessParameterChart(selectedMachineId, fromDate, toDate,);
        });
        function SPCMonthlyTrendChart(data) {
            var chart = {
                type: 'column',
                zoomType: 'x'
            };
            var zoomType = {
                zoomType: 'x'
            };
            var title = {
                text: 'ENERGY MACHINEWISE MONTHLY TREND'
            };
            var subtitle = {
                text: 'Monthly Trend'
            };
            var xAxis = {
                type: 'datetime',
                zoomEnabled: !0,
                //categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul',
                //    'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                crosshair: true
            };
            //var yAxis = {
            //    min: 0,
            //    title: {
            //        text: 'Rainfall (mm)'
            //    }
            //};
            var tooltip = {
                headerFormat: '<span style = "font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style = "color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style = "padding:0"><b>{point.y:.1f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            };
            var plotOptions = {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            };
            var credits = {
                enabled: false
            };
            //var series = [
            //    {
            //        name: 'Average Kwh',
            //        data: [49.9]
            //    },
            //    {
            //        name: 'Average Daily Tonnage',
            //        data: [83.6]
            //    },
            //    {
            //        name: 'Average SPC',
            //        data: [48.9]
            //    }
            //];

            var series = data;

            var json = {};
            json.chart = chart;
            json.zoomType = zoomType;
            json.title = title;
            json.subtitle = subtitle;
            json.tooltip = tooltip;
            json.xAxis = xAxis;
            //json.yAxis = yAxis;
            json.series = series;
            json.plotOptions = plotOptions;
            json.credits = credits;
            $('#divSPCMonthly').highcharts(json);

        }
        function OnSuccessGetItems(data) {
            $.unblockUI({});
            var chart = {
                type: 'column',
                zoomType: 'x'
            };
            var title = {
                text: 'ENERGY MACHINEWISE DAILY TREND'
            };
            var subtitle = {
                text: 'Daily Trend'
            };
            var xAxis = {
                type: 'datetime',
                //categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul',
                //    'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                //endOnTick: true,
                //showLastLabel: true,
                //startOnTick: true,
                crosshair: true
            };
            //var yAxis = {
            //    min: 0,
            //    title: {
            //        text: 'Rainfall (mm)'
            //    }
            //};
            var yAxis = [{ // Primary yAxis
                labels: {
                    //format: '£{value}',
                    style: {
                        //color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Values',
                    style: {
                        //color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                title: {
                    text: 'Values',
                    style: {
                        //color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    style: {
                        // color: Highcharts.getOptions().colors[0]
                    }
                },
                opposite: true
            }];
            var tooltip = {
                headerFormat: '<span style = "font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style = "color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style = "padding:0"><b>{point.y:.1f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            };
            var plotOptions = {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            };
            var credits = {
                enabled: false
            };
            var series = data.d.series;

            var json = {};
            json.chart = chart;
            json.title = title;
            json.subtitle = subtitle;
            json.tooltip = tooltip;
            json.xAxis = xAxis;
            json.yAxis = yAxis;
            json.series = series;
            json.plotOptions = plotOptions;
            json.credits = credits;
            $('#divSPCDaily').highcharts(json);
            SPCMonthlyTrendChart(data.d.seriesMonthly);
        }
        function btnProcessClicked() {
            debugger;

            var fromDate = $("[id$=txtFromDateMonth]").val();
            var toDate = $("[id$=txtToDateMonth]").val();
            var selectedMachineId = $("[id$=ddlMachine]").val();
            //var jsLang = currMonth + 1;
            switch (toDate) {
                case 'Jan':
                    toDate = 1;
                    break;
                case 'Feb':
                    toDate = 2;
                    break;
                case 'Mar':
                    toDate = 3;
                    break;
                case 'Apr':
                    toDate = 4;
                    break;
                case 'May':
                    toDate = 5;
                    break;
                case 'Jun':
                    toDate = 6;
                    break;
                case 'Jul':
                    toDate = 7;
                    break;
                case 'Aug':
                    toDate = 8;
                    break;
                case 'Sep':
                    toDate = 9;
                    break;
                case 'Oct':
                    toDate = 10;
                    break;
                case 'Nov':
                    toDate = 11;
                    break;
                case 'Dec':
                    toDate = 12;
                    break;
            }
            PlotProcessParameterChart(selectedMachineId, fromDate, toDate,);

        }
        function PlotProcessParameterChart(selectedMachineId, fromDate, toDate) {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProcessParameterSPC.aspx/GetDailySPCDataGraph",
                data: '{machineid:"' + selectedMachineId + '", fromDate:"' + fromDate + '", toDate:"' + toDate + '"}',
                dataType: "json",
                success: OnSuccessGetItems,
                error: function (Result) {
                    $.unblockUI({});
                    alert("Error");

                }
            });

        }

        function SetControl() {
            //$('[id$=txtFromDateMonth]').datepicker({
            //    viewMode: "date",
            //    minViewMode: "date",
            //    format: 'dd-mm-yyyy',
            //    todayHighlight: true,
            //    autoclose: true,
            //    language: 'en-US'
            //});
            //$('[id$=txtToDateMonth]').datepicker({
            //    viewMode: "date",
            //    minViewMode: "date",
            //    format: 'dd-mm-yyyy',
            //    todayHighlight: true,
            //    autoclose: true,
            //    language: 'en-US'
            //});
            debugger;

            $('[id$=txtFromDateMonth]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: 'en-US',
            });
            $('[id$=txtToDateMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: 'en-US',
            }).on('change', function (selected) {
                //alert("startDate..." + selected.timeStamp);
                ShowMonth();
            });
            //$('[id$=txtToDateMonth]').datepicker({
            //    format: "mm-yyyy",
            //    viewMode: "months",
            //    minViewMode: "months"
            //});
            if ($('[id$=txtToDateMonth]').val() == "" || $('[id$=txtToDateMonth]').val() == null || $('[id$=txtToDateMonth]').val() == "MMM") {
                var d = new Date();
                var currMonth = d.getMonth();
                $('[id$=txtToDateMonth]').val(currMonth + 1);
                ShowMonth();
            }
        }
        function ShowMonth() {

            debugger;

            var jsLang = $('[id$=txtToDateMonth]').val();
            switch (jsLang) {
                case '01':
                    $('[id$=txtToDateMonth]').val('Jan');
                    break;
                case '1':
                    $('[id$=txtToDateMonth]').val('Jan');
                    break;
                case '02':
                    $('[id$=txtToDateMonth]').val('Feb');
                    break;
                case '03':
                    $('[id$=txtToDateMonth]').val('Mar');
                    break;
                case '04':
                    $('[id$=txtToDateMonth]').val('Apr');
                    break;
                case '05':
                    $('[id$=txtToDateMonth]').val('May');
                    break;
                case '06':
                    $('[id$=txtToDateMonth]').val('Jun');
                    break;
                case '07':
                    $('[id$=txtToDateMonth]').val('Jul');
                    break;
                case '08':
                    $('[id$=txtToDateMonth]').val('Aug');
                    break;
                case '09':
                    $('[id$=txtToDateMonth]').val('Sep');
                    break;
                case '10':
                    $('[id$=txtToDateMonth]').val('Oct');
                    break;
                case '11':
                    $('[id$=txtToDateMonth]').val('Nov');
                    break;
                case '12':
                    $('[id$=txtToDateMonth]').val('Dec');
                    break;
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            //SetControl();
            var fromDate = $("[id$=txtFromDateMonth]").val();
            var toDate = $("[id$=txtToDateMonth]").val();
            var selectedMachineId = $("[id$=ddlMachine]").val();
            SetControl();
            $.unblockUI({});
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
