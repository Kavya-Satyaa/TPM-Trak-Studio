<%@ Page Title="Process Parameter Trend" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PPMachineAnalytics.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.PPMachineAnalytics" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<script src="Scripts/jquery-3.3.1.js"></script>--%>
    <%--<link href="Content/Ionic.css" rel="stylesheet" />--%>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <!-- Font Awesome -->
    <%--<link
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css"
        rel="stylesheet" />--%>
    <!-- Google Fonts -->
    <%--  <link
        href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
        rel="stylesheet" />--%>
    <!-- MDB -->
    <%-- <link
        href="https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/6.0.1/mdb.min.css"
        rel="stylesheet" />

    <!-- MDB -->
    <script
        type="text/javascript"
        src="https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/6.0.1/mdb.min.js"></script>--%>

    <%--<script src="https://code.highcharts.com/highcharts.js"></script>--%>
    <script src="https://code.highcharts.com/highcharts-more.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>


    <%--    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.1.7/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.1.7/css/dx.light.css" />
    <script src="https://cdn3.devexpress.com/jslib/20.1.7/js/dx.all.js"></script>

    <link href="http://cdn.syncfusion.com/20.3.0.47/js/web/flat-azure/ej.web.all.min.css" rel="stylesheet" />
    <script src="http://cdn.syncfusion.com/20.3.0.47/js/web/ej.web.all.min.js"></script>--%>




    <style>
        legend {
            display: block;
            width: 100%;
            padding: 0;
            margin-bottom: 20px;
            font-size: 21px;
            line-height: inherit;
            color: #ebe5e8;
            border: 0;
            border-bottom: 1px solid #e5e5e5;
            font-weight: bold;
        }

        label {
            font-weight: bold;
            color: white;
        }

        button, html input[type="button"], input[type="reset"], input[type="submit"] {
            cursor: pointer;
            -webkit-appearance: button;
            width: 125px;
            height: 45px;
            font-weight: bold;
        }

        .highcharts-title {
            font-weight: bold;
        }

        .multiselect-container > li > a > label.radio, .multiselect-container > li > a > label.checkbox {
            margin: 0;
            color: black;
        }

        .multiselect-container {
            width: fit-content;
        }

        .btn-group > .btn:first-child {
            margin-left: 0;
            width: 215px;
            /*min-width: 130px;*/
        }

        .form-control {
            height: 40px;
        }
    </style>



    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>Selection Criteria</legend>
                    <table class="cumi-filter-tbl">
                        <tr>
                            <td>
                                <label class="filter-lbl-name">Plant</label>
                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" onchange="BindMachineID();" Style="font-size: initial;"></asp:DropDownList>
                            </td>
                            <td>
                                <label class="filter-lbl-name">Machine</label><br />
                                <%--OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"    onchange="return BindMachineParameters();"     --%>
                                <asp:DropDownList ClientIDMode="Static" data-mdb-clear-button="true" multiple="multiple" ID="ddlMachine" runat="server" CssClass="form-control multiselect-ui-machine" Style="font-size: initial;"></asp:DropDownList>
                            </td>
                            <td>
                                <label class="filter-lbl-name">Parameter</label><br />
                                <asp:DropDownList multiple="multiple" data-mdb-clear-button="true" ID="ddlParameters" runat="server" CssClass="form-control multiselect-ui" ClientIDMode="Static" Style="font-size: initial;">
                                </asp:DropDownList>
                            </td>
                            <td class="" runat="server" style="position: relative">
                                <label class="filter-lbl-name">From Date</label><br />
                                <asp:TextBox ID="txtFromDateMonth" runat="server" CssClass="form-control" placeholder="From Date" meta:resourcekey="txtDateMonthResource1" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="" runat="server" style="position: relative">
                                <label class="filter-lbl-name">To Date</label><br />
                                <asp:TextBox ID="txtToDateMonth" runat="server" CssClass="form-control" placeholder="To Date" meta:resourcekey="txtDateMonthResource1" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="hidden">
                                <label class="filter-lbl-name">Shift</label><br />
                                <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control" Style="font-size: initial;"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnProcess" Text="Process" OnClientClick="return btnProcessClicked();" CssClass="btn btn-info btn-sm" Style="margin-left: 10px; margin-top: 25px;" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <%--<asp:Timer ID="timerToAutoRefreshPP" runat="server" Enabled="False" OnTick="timerToAutoRefreshPP_Tick"></asp:Timer>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <div class="row">

            <div id="divPPMasterChart" class="col-lg-12 col-md-12 col-sm-12" style="">
                <%--<div id="divHyd" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px"></div>
                <div id="divRam" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px">
                </div>
                <div id="divOil" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px">
                </div>--%>
                <%--                <div id="areachart" style="margin: 3px auto; height: 400px; width: 100%; background-color: aliceblue; border-bottom: 2px solid black; min-height: 400px">
                </div>--%>
                <%--                <div id = "areachart" style = "width: 550px; height: 400px; margin: 0 auto"></div>--%>
            </div>
        </div>
        <div class="modal fade" id="loadingModal" role="dialog" style="min-width: 300px; margin-top: 15%" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog  modal-dialog-centered" style="width: 500px">
                <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                    <%--<div class="modal-header" style="background-color: #0c0922; padding: 8px">

                        <h4 class="modal-title" style="color: white;">Warning!</h4>
                    </div>--%>
                    <div class="modal-body" style="text-align: center; padding: 20px">
                        <span style="color: black; font-size: 25px;">Please wait.....</span>
                    </div>
                    <%--<div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                        <button type="button" data-dismiss="modal" Style="width: 80px;" class="modalBtns">OK</button>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            resize();
            window.onresize = function () {
                resize();
            };

            BindMachineID();
            BindMachineParameters();


            //$('.date').datetimepicker({
            //    format: 'DD-MM-YYYY HH:mm:ss',
            //    useCurrent: false,
            //    locale: 'en-US'
            //});
            //$('.date').each(function () {
            //    $(this).on('dp.change', function (ev) {

            //        $(this).data('DateTimePicker').hide();

            //    });
            //});
            SetControl();
            var fromDate = $("[id$=txtFromDateMonth]").val();
            var toDate = $("[id$=txtToDateMonth]").val();
            var selectedMachineId = $("[id$=ddlMachine]").val();
            var selectedPlantId = $("[id$=ddlPlant]").val();
            var ddlShift = $("[id$=ddlShift]").val();
            var ddlParameters = $("[id$=ddlParameters]").val();

            PlotProcessParameterChart(selectedPlantId, selectedMachineId, fromDate, toDate, ddlShift, ddlParameters);
            //AreaChart();
        });
        function MultiSelectSet() {
            $('[id$=ddlMachine]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px',
                onChange: function (option, checked) {
                    debugger;
                    var selectedOptions = $('.multiselect-ui option:selected');
                    var selectedOptionsMachine = $('.multiselect-ui-machine option:selected');

                    if (selectedOptions.length >= 1 && selectedOptionsMachine.length >= 2) {
                        var nonSelectedOptions = $('.multiselect-ui option').filter(function () {
                            return !$(this).is(':selected');
                        });

                        nonSelectedOptions.each(function () {
                            var input = $('input[value="' + $(this).val() + '"]');
                            input.prop('disabled', true);
                            input.parent('li').addClass('disabled');
                        });
                    }
                    else {
                        $('.multiselect-ui option').each(function () {
                            var input = $('input[value="' + $(this).val() + '"]');
                            input.prop('disabled', false);
                            input.parent('li').addClass('disabled');
                        });
                    }
                }
            });

        }
        function BindMachineParameters() {
            debugger;
            $("[id$=ddlParameters]").empty();
            $('[id$=ddlParameters]').multiselect("destroy");
            let apprStr = "";
            apprStr += "<option value='1'>Hydraulic Pressure</option>";
            apprStr += "<option value='2'>Ram Stroke</option>";
            apprStr += "<option value='3'>Hydraulic Oil Temperature</option>";
            apprStr += "<option value='4'>Cooling Tower Water Temperature</option>";
            $("[id$=ddlParameters]").append(apprStr);
            $('[id$=ddlParameters]').multiselect({
                //includeSelectAllOption: true,
                buttonWidth: '250px',
                onChange: function (option, checked) {
                    debugger;
                    var selectedOptions = $('.multiselect-ui option:selected');
                    var selectedOptionsMachine = $('.multiselect-ui-machine option:selected');

                    if (selectedOptions.length >= 1 && selectedOptionsMachine.length >= 2) {
                        var nonSelectedOptions = $('.multiselect-ui option').filter(function () {
                            return !$(this).is(':selected');
                        });

                        nonSelectedOptions.each(function () {
                            var input = $('input[value="' + $(this).val() + '"]');
                            input.prop('disabled', true);
                            input.parent('li').addClass('disabled');
                        });
                    }
                    else {
                        $('.multiselect-ui option').each(function () {
                            var input = $('input[value="' + $(this).val() + '"]');
                            input.prop('disabled', false);
                            input.parent('li').addClass('disabled');
                        });
                    }
                }
            });
        }
        function HydAreaChart(data) {
            debugger;
            //$('#loadingModal').modal('hide');
            $.unblockUI({});
            //console.log(data);
            for (var i = 0; i < data.d.lineChartSeries.length; i++) {
                var machineid = data.d.lineChartSeries[i].MachineID.split(' ').join('-');
                appendString = '<div id="divHyd' + machineid + '" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px; display: flex; flex-wrap: wrap;"></div>';
                $('#divPPMasterChart').append(appendString);
                var chart = {
                    type: 'area',
                    resetZoomButton: {
                        position: {
                            align: 'right', // by default
                            verticalAlign: 'top', // by default
                            //x: 0,
                            //y: -3
                        },
                        relativeTo: 'chart'
                    },
                    zoomType: 'x'
                    //zoomType: 'xy',

                };
                var title = {
                    text: '<span style="color: blue">' + data.d.lineChartSeries[i].MachineID + '</span> - Hydraulic Pressure'
                };
                //var subtitle = {
                //    text: 'Source: Wikipedia.org'
                //};
                var xAxis = {
                    type: 'datetime',
                    //tickInterval: 60000,
                    //minRange: 1000,
                    //ordinal: false,
                    //dateTimeLabelFormats: {
                    //    month: '%e. %b',
                    //    year: '%b'
                    //},
                    //categories: data.d.Category

                    plotLines: [{
                        color: '#FF0000',
                        width: 1,
                        value: 5.5
                    }]
                };
                var rangeSelector = {
                    verticalAlign: 'top',
                    x: 0,
                    y: 0,
                    //buttonTheme: { // styles for the buttons
                    //    fill: 'none',
                    //    stroke: 'none',
                    //    'stroke-width': 0,
                    //    r: 1,
                    //    style: {
                    //        color: '#039',
                    //        width: '50px',
                    //        //fontWeight: 'bold'
                    //        labelWidth: '25px'
                    //    },
                    //    states: {
                    //        hover: {
                    //        },
                    //        select: {
                    //            fill: '#039',
                    //            style: {
                    //                color: 'white',
                    //                width: '25px'
                    //            }
                    //        }
                    //        // disabled: { ... }
                    //    }
                    //},
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
                            count: 8,
                            text: '8h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'Reset'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                };
                var tooltip = {
                    //shared: true,
                    valueSuffix: ''
                };
                var plotOptions = {
                    area: {
                        // stacking: 'normal',
                        lineColor: '#6D76CA',
                        lineWidth: 1,

                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                };
                var credits = {
                    enabled: false
                };
                var series = data.d.lineChartSeries[i].series;

                var json = {};
                json.chart = chart;
                json.title = title;
                //json.subtitle = subtitle;
                json.xAxis = xAxis;
                json.tooltip = tooltip;
                json.plotOptions = plotOptions;
                json.rangeSelector = rangeSelector;
                json.credits = credits;
                json.series = series;
                $('#divHyd' + machineid).highcharts(json);
            }


        }
        function RamAreaChart(data) {
            debugger;
            //$('#loadingModal').modal('hide');
            $.unblockUI({});
            //console.log(data);
            for (var i = 0; i < data.d.lineChartSeries.length; i++) {
                var machineid = data.d.lineChartSeries[i].MachineID.split(' ').join('-');
                appendString = '<div id="divRam' + machineid + '" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px; display: flex; flex-wrap: wrap;"></div>';
                $('#divPPMasterChart').append(appendString);
                var chart = {
                    type: 'area',
                    zoomType: 'x',
                    //zoomType: 'xy',
                    resetZoomButton: {
                        position: {
                            align: 'right', // by default
                            verticalAlign: 'top', // by default
                            //x: 0,
                            //y: -3
                        },
                        relativeTo: 'plot'
                    }
                };
                var title = {
                    text: '<span style="color: blue">' + data.d.lineChartSeries[i].MachineID + '</span> - Ram Stroke'
                };
                //var subtitle = {
                //    text: 'Source: Wikipedia.org'
                //};
                var xAxis = {
                    type: 'datetime',
                    //tickInterval: 60000,
                    //minRange: 1000,
                    //ordinal: false,
                    //dateTimeLabelFormats: {
                    //    month: '%e. %b',
                    //    year: '%b'
                    //},
                    //categories: data.d.Category

                    plotLines: [{
                        color: '#FF0000',
                        width: 1,
                        value: 5.5
                    }]
                };
                var rangeSelector = {
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
                            count: 8,
                            text: '8h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'Reset'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                };
                var tooltip = {
                    shared: true,
                    valueSuffix: ''
                };
                var plotOptions = {
                    area: {
                        //stacking: 'normal',
                        lineColor: '#6D76CA',
                        lineWidth: 1,

                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                };
                var credits = {
                    enabled: false
                };
                var series = data.d.lineChartSeries[i].series;
                var json = {};
                json.chart = chart;
                json.title = title;
                //json.subtitle = subtitle;
                json.xAxis = xAxis;
                json.tooltip = tooltip;
                json.plotOptions = plotOptions;
                json.rangeSelector = rangeSelector;
                json.credits = credits;
                json.series = series;

                $('#divRam' + machineid).highcharts(json);
            }

        }
        function SetControl() {
            $('[id$=txtFromDateMonth]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: 'en-US'
            });
            $('[id$=txtFromDateMonth]').on('dp.change', function (ev) {

                $(this).data('DateTimePicker').hide();

            });
            //$('[id$=txtFromDateMonth]').datepicker("setDate", "-1");
            $('[id$=txtToDateMonth]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: 'en-US'
            });
            $('[id$=txtToDateMonth]').on('dp.change', function (ev) {

                $(this).data('DateTimePicker').hide();

            });
            //$('[id$=txtToDateMonth]').datepicker("setDate", "-1d");
        }
        function resize() {

            var heights = window.innerHeight;
            var widths = window.innerWidth;
            document.getElementById("divPPMasterChart").style.height = heights + "px";
        }
        function BindMachineID() {
            debugger;
            $("[id$=ddlMachine]").empty();
            $("[id$=ddlMachine]").closest('div').find('.dropdown-menu').empty();
            //$("[id$=ddlMachine]").closest('div').find('button').closest('span').find('.multiselect-selected-text').empty();
            $("[id$=ddlMachine]").multiselect("destroy");
            $("[id$=ddlParameters]").multiselect("destroy");
            $.ajax({
                async: false,
                type: "POST",
                url: "PPMachineAnalytics.aspx/BindMachine",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: "{plant:'" + $("[id$=ddlPlant]").val() + "'}",
                success: function (response) {
                    let dataitem = response.d;
                    let apprStr = "";

                    //$("[id$=ddlParameters]").empty();
                    //for (let i = 0; i < dataitem.length; i++) {
                    //    apprStr += "<option>" + dataitem[i] + "</option>";
                    //}
                    //$("[id$=ddlParameters]").append(apprStr);

                    //$("[id$=ddlMachine]").empty();
                    for (let i = 0; i < dataitem.length; i++) {
                        //if (i == 0) {
                        //    apprStr += "<option selected>" + dataitem[i] + "</option>";
                        //} else {
                        //    apprStr += "<option>" + dataitem[i] + "</option>";
                        //}
                        apprStr += "<option>" + dataitem[i] + "</option>";
                    }
                    $("[id$=ddlMachine]").append(apprStr);

                },
                error: function (Result) {
                }
            });
            if ($("[id$=ddlMachine]").val() == "" || $("[id$=ddlMachine]").val() == null) {
                var selectTags = $("[id$=ddlMachine]");
                //console.log(selectTags);
                selectTags[0].selectedIndex = 0;
            }
            BindMachineParameters();
            MultiSelectSet();
        }
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        function btnProcessClicked() {
            debugger;


            var fromDate = $("[id$=txtFromDateMonth]").val();
            var toDate = $("[id$=txtToDateMonth]").val();
            var selectedMachineId = $("[id$=ddlMachine]").val();
            var selectedPlantId = $("[id$=ddlPlant]").val();
            var ddlShift = $("[id$=ddlShift]").val();
            var ddlParameters = $("[id$=ddlParameters]").val();
            //$('#loadingModal').modal('show');
            if (selectedMachineId.length > 1 && ddlParameters == null) {
                alert('Please select anyone parameter value');
                return false;
            }

            var fromDate1 = fromDate.substring(0, 10);
            var toDate1 = toDate.substring(0, 10);
            var diffe = dateDiffInDays(new Date(fromDate1.split("-").reverse().join("-")), new Date(toDate1.split("-").reverse().join("-")));
            var dateCom = compareDates(fromDate1, toDate1);
            if (dateCom == 1) {
                alert("From-Date Cannot Be greater than To-Date.");
                $("[id$=txtToDateTime]").focus();
                return false;
            }
            if (diffe > 2) {
                alert("Sorry, you can view Data for only 48 Hours.");
                return false;
            }

            PlotProcessParameterChart(selectedPlantId, selectedMachineId, fromDate, toDate, ddlShift, ddlParameters);
            //$('#loadingModal').modal('hide');
            return false;
        }
        function PlotProcessParameterChart(selectedPlantId, selectedMachineId, fromDate, toDate, ddlShift, ddlParameters) {
            debugger;
            $('#divPPMasterChart').empty();
            //$.blockUI({ message: '<img src="Images/ajax-loader.gif" />' });
            if (jQuery.inArray("1", ddlParameters) !== -1 || ddlParameters == "" || ddlParameters == null) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //$('#loadingModal').modal('show');
                setTimeout(function () {
                    $.ajax({
                        async: false,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "PPMachineAnalytics.aspx/GetPressureData",
                        data: '{plantid:"' + selectedPlantId + '", machineid:"' + selectedMachineId + '", fromDate:"' + fromDate + '", toDate:"' + toDate + '", ddlShift:"' + ddlShift + '"}',
                        dataType: "json",
                        success: HydAreaChart,
                        error: function (Result) {
                            //$('#loadingModal').modal('hide');
                            $.unblockUI({});
                            alert("Error");
                        }
                    });
                }, 500);
                //$.unblockUI({});
            }
            if (jQuery.inArray("2", ddlParameters) !== -1 || ddlParameters == "" || ddlParameters == null) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //$('#loadingModal').modal('show');
                setTimeout(function () {
                    $.ajax({
                        async: false,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "PPMachineAnalytics.aspx/GetRamData",
                        data: '{plantid:"' + selectedPlantId + '", machineid:"' + selectedMachineId + '", fromDate:"' + fromDate + '", toDate:"' + toDate + '", ddlShift:"' + ddlShift + '"}',
                        dataType: "json",
                        success: RamAreaChart,
                        error: function (Result) {
                            //$('#loadingModal').modal('hide');
                            $.unblockUI({});
                            alert("Error");
                        }
                    });
                }, 500);
                //$.unblockUI({});
            }
            if (jQuery.inArray("3", ddlParameters) !== -1 || ddlParameters == "" || ddlParameters == null) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //$('#loadingModal').modal('show');
                setTimeout(function () {
                    $.ajax({
                        async: false,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "PPMachineAnalytics.aspx/GetOilData",
                        data: '{plantid:"' + selectedPlantId + '", machineid:"' + selectedMachineId + '", fromDate:"' + fromDate + '", toDate:"' + toDate + '", ddlShift:"' + ddlShift + '"}',
                        dataType: "json",
                        success: OilLineChart,
                        error: function (Result) {
                            //$('#loadingModal').modal('hide');
                            $.unblockUI({});
                            alert("Error");
                        }
                    });
                }, 500);
                /*$.unblockUI({});           */
            }
            if (jQuery.inArray("4", ddlParameters) !== -1 || ddlParameters == "" || ddlParameters == null) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //$('#loadingModal').modal('show');
                setTimeout(function () {
                    $.ajax({
                        async: false,
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "PPMachineAnalytics.aspx/GetCoolantData",
                        data: '{plantid:"' + selectedPlantId + '", machineid:"' + selectedMachineId + '", fromDate:"' + fromDate + '", toDate:"' + toDate + '", ddlShift:"' + ddlShift + '"}',
                        dataType: "json",
                        success: CoolantAreaChart,
                        error: function (Result) {
                            //$('#loadingModal').modal('hide');
                            $.unblockUI({});
                            alert("Error");
                        }
                    });
                }, 500);
                /*$.unblockUI({});           */
            }
            $.ajax({
                async: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PPMachineAnalytics.aspx/ResetSession",
                data: '{}',
                dataType: "json",
                success: function (result) {

                },
                error: function (Result) {
                    $.unblockUI({});
                    alert("Error");
                }
            });
        }
        function HydLineChart(data) {
            $.unblockUI({});
            Highcharts.stockChart('divHyd', {
                credits: {
                    enabled: false
                },
                title: {
                    text: "Hydraulic Pressure",
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
                            count: 8,
                            text: '8h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'Reset'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 60000,
                    minRange: 1,
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
        }
        function RamLineChart(data) {
            $.unblockUI({});
            Highcharts.stockChart('divRam', {
                credits: {
                    enabled: false
                },
                title: {
                    text: "Ram Stroke",
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
                            count: 8,
                            text: '8h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'Reset'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 60000,
                    minRange: 1,
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
        }
        function OilLineChart(data) {
            debugger;
            //$('#loadingModal').modal('hide');
            $.unblockUI({});
            //console.log(data);
            for (var i = 0; i < data.d.lineChartSeries.length; i++) {
                var machineid = data.d.lineChartSeries[i].MachineID.split(' ').join('-');
                appendString = '<div id="divOil' + machineid + '" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px"></div>';
                $('#divPPMasterChart').append(appendString);
                var chart = {
                    type: 'area',
                    //zoomType: 'x'
                    zoomType: 'x',
                    resetZoomButton: {
                        position: {
                            align: 'right', // by default
                            verticalAlign: 'top', // by default
                            //x: 0,
                            //y: -3
                        },
                        relativeTo: 'plot'
                    }
                };
                var title = {
                    text: '<span style="color: blue">' + data.d.lineChartSeries[i].MachineID + '</span> - Oil Temperature'
                };
                //var subtitle = {
                //    text: 'Source: Wikipedia.org'
                //};
                var xAxis = {
                    type: 'datetime',
                    //tickInterval: 60000,
                    //minRange: 1000,
                    //ordinal: false,
                    //dateTimeLabelFormats: {
                    //    month: '%e. %b',
                    //    year: '%b'
                    //},
                    //categories: data.d.Category

                    plotLines: [{
                        color: '#FF0000',
                        width: 1,
                        value: 5.5
                    }]
                };
                var rangeSelector = {
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
                            count: 8,
                            text: '8h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'Reset'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                };
                var tooltip = {
                    shared: true,
                    valueSuffix: ''
                };
                var plotOptions = {
                    area: {
                        //stacking: 'normal',
                        lineColor: '#6D76CA',
                        lineWidth: 1,

                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                };
                var credits = {
                    enabled: false
                };
                var series = data.d.lineChartSeries[i].series;
                var json = {};
                json.chart = chart;
                json.title = title;
                //json.subtitle = subtitle;
                json.xAxis = xAxis;
                json.tooltip = tooltip;
                json.plotOptions = plotOptions;
                json.rangeSelector = rangeSelector;
                json.credits = credits;
                json.series = series;

                $('#divOil' + machineid).highcharts(json);
            }

        }
        function CoolantAreaChart(data) {
            debugger;
            //$('#loadingModal').modal('hide');
            $.unblockUI({});
            //console.log(data);
            for (var i = 0; i < data.d.lineChartSeries.length; i++) {
                var machineid = data.d.lineChartSeries[i].MachineID.split(' ').join('-');
                appendString = '<div id="divCoolant' + machineid + '" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px"></div>';
                $('#divPPMasterChart').append(appendString);
                var chart = {
                    type: 'area',
                    //zoomType: 'x'
                    zoomType: 'x',
                    resetZoomButton: {
                        position: {
                            align: 'right', // by default
                            verticalAlign: 'top', // by default
                            //x: 0,
                            //y: -3
                        },
                        relativeTo: 'plot'
                    }
                };
                var title = {
                    text: 'Cooling Tower Water Temperature'
                };
                //var subtitle = {
                //    text: 'Source: Wikipedia.org'
                //};
                var xAxis = {
                    type: 'datetime',
                    //tickInterval: 60000,
                    //minRange: 1000,
                    //ordinal: false,
                    //dateTimeLabelFormats: {
                    //    month: '%e. %b',
                    //    year: '%b'
                    //},
                    //categories: data.d.Category

                    plotLines: [{
                        color: '#FF0000',
                        width: 1,
                        value: 5.5
                    }]
                };
                var rangeSelector = {
                    verticalAlign: 'top',
                    align: 'right',
                    //x: 0,
                    //y: 0,
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
                            count: 8,
                            text: '8h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'day',
                            count: 24,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'Reset'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                };
                var tooltip = {
                    shared: true,
                    valueSuffix: ''
                };
                var plotOptions = {
                    area: {
                        // stacking: 'normal',
                        lineColor: '#6D76CA',
                        lineWidth: 1,

                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                };
                var credits = {
                    enabled: false
                };
                var series = data.d.lineChartSeries[i].series;
                var json = {};
                json.chart = chart;
                json.title = title;
                //json.subtitle = subtitle;
                json.xAxis = xAxis;
                json.tooltip = tooltip;
                json.plotOptions = plotOptions;
                json.rangeSelector = rangeSelector;
                json.credits = credits;
                json.series = series;

                $('#divCoolant' + machineid).highcharts(json);
            }

        }
        //function OilLineChart(data) {
        //    debugger;
        //    $.unblockUI({});
        //    //console.log(data);
        //    for (var i = 0; i < data.d.lineChartSeries.length; i++) {
        //        var machineid = data.d.lineChartSeries[i].MachineID.split(' ').join('-');
        //        appendString = '<div id="divOil' + machineid + '" style="margin: 3px auto; height: 400px; width: 100%; border-bottom: 2px solid black; min-height: 400px"></div>';
        //        $('#divPPMasterChart').append(appendString);
        //        Highcharts.stockChart('divOil' + machineid, {
        //            credits: {
        //                enabled: false
        //            },
        //            title: {
        //                text: '<span style="color: blue">' + data.d.lineChartSeries[i].MachineID + '</span> - Oil Temperature',
        //            },
        //            rangeSelector: {
        //                verticalAlign: 'top',
        //                x: 0,
        //                y: 0,
        //                buttons: [
        //                    {
        //                        type: 'hour',
        //                        count: 2,
        //                        text: '2h'
        //                    },
        //                    {
        //                        type: 'hour',
        //                        count: 5,
        //                        text: '5h'
        //                    },
        //                    {
        //                        type: 'hour',
        //                        count: 8,
        //                        text: '8h'
        //                    },
        //                    {
        //                        type: 'hour',
        //                        count: 10,
        //                        text: '10h'
        //                    },
        //                    {
        //                        type: 'day',
        //                        count: 1,
        //                        text: '1d'
        //                    },
        //                    {
        //                        type: 'all',
        //                        text: 'Reset'
        //                    }],
        //                selected: 10,
        //                enabled: true,
        //                allButtonsEnabled: true,

        //                inputEnabled: false
        //            },
        //            xAxis: {
        //                type: 'datetime',
        //                tickInterval: 60000,
        //                minRange: 1,
        //                ordinal: false,
        //                dateTimeLabelFormats: {
        //                    month: '%e. %b',
        //                    year: '%b'
        //                },
        //                categories: data.d.lineChartSeries[i].Category,

        //                plotLines: [{
        //                    color: '#FF0000',
        //                    width: 2,
        //                    value: 5.5
        //                }]
        //            },
        //            series: data.d.lineChartSeries[i].series,
        //        });
        //    }

        //}
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            resize();
            window.onresize = function () {
                resize();
            };

            //BindMachineID();
            //BindMachineParameters();

            //$('.date').datetimepicker({
            //    format: 'DD-MM-YYYY HH:mm:ss',
            //    useCurrent: false,
            //    locale: 'en-US'
            //});
            //$('.date').each(function () {
            //    $(this).on('dp.change', function (ev) {

            //        $(this).data('DateTimePicker').hide();

            //    });
            //});
            SetControl();
            //$('#loadingModal').modal('hide');
            //$.unblockUI({});
            //var fromDate = $("[id$=txtFromDateMonth]").val();
            //var toDate = $("[id$=txtToDateMonth]").val();
            //var selectedMachineId = $("[id$=ddlMachine]").val();
            //var selectedPlantId = $("[id$=ddlPlant]").val();
            //var ddlShift = $("[id$=ddlShift]").val();
            //var ddlParameters = $("[id$=ddlParameters]").val();
            //PlotProcessParameterChart(selectedPlantId, selectedMachineId, fromDate, toDate, ddlShift);
        });
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
