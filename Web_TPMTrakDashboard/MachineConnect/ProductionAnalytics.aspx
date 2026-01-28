<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductionAnalytics.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineConnect.ProductionAnalytics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/xrange.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/exporting.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/accessibility.js"></script>
    <style>
        .P1Table tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        .P1Table tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        #rblLeftSelction tr td, #rblRightSelction tr td {
            padding-right: 10px;
        }

        .checkbox-list label {
            font-weight: bold;
            color: white;
        }

        .headerFixer tr:nth-child(2) th {
            top: 34px;
        }


        .highchart-xyaxis-label text {
            fill: black !important;
            font-weight: bold;
            font-size: 14px !important;
            /* stroke: white;*/
            /*font-family: 'Glyphicons Halflings';*/
            /*font-family: emoji;*/
            /*   font-family: initial;*/
            /* font-family: 'Lato' !important;
            font-weight: unset !important;*/
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfNoOfMachines" ClientIDMode="Static" Value="10" />
            <asp:HiddenField runat="server" ID="hfMachine" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto;">
                <tr>
                    <td class="commanTd" style="vertical-align: middle;">From Date</td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Shift</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Plant</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Machine</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cbAutoRefresh" ClientIDMode="Static" CssClass=" checkbox-list" Text="Auto Refresh" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" OnClientClick="return callLoader();" />
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="grid-container" style="height: 38vh; overflow: auto; margin-top: 10px;">

                <asp:ListView runat="server" ID="lvProductionDetails" ClientIDMode="Static">
                    <%--OnPagePropertiesChanging="lvProductionDetails_PagePropertiesChanging"--%>
                    <LayoutTemplate>
                        <table class="P1Table pa-outer-tbl table table-bordered table-hover headerFixer">
                            <tr>
                                <th rowspan="2" style="vertical-align: middle">Machine ID</th>
                                <th rowspan="2" style="vertical-align: middle">Status</th>
                                <th rowspan="2" style="vertical-align: middle">Running
                                    <br />
                                    Program</th>
                                <th colspan="4" style="text-align: center">Time - Minute (%)</th>
                                <th colspan="5" style="text-align: center">Part Count By Program</th>
                            </tr>
                            <tr>
                                <th>Total Time</th>
                                <th>Power On Time</th>
                                <th>Operating Time</th>
                                <th>Cutting Time</th>
                                <th>Prog 1(#)</th>
                                <th>Prog 2(#)</th>
                                <th>Prog 3(#)</th>
                                <th>Prog 4(#)</th>
                                <th>Total</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>

                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>

                            <td onclick="BindChartForSelectedMachine(this);">
                                <asp:LinkButton runat="server" ID="lbMachineID" ForeColor="Blue" ClientIDMode="Static" Text=' <%# Eval("MachineID") %>'></asp:LinkButton>
                            </td>
                            <td style='background-color: <%# Eval("StatusBackColor") %>'>
                                <%# Eval("Status") %>
                            </td>
                            <td>
                                <%# Eval("RunningProgram") %>
                            </td>
                            <td>
                                <%# Eval("TotalTime") %>
                            </td>
                            <td>
                                <%# Eval("PowerOnTime") %>
                            </td>
                            <td>
                                <%# Eval("OperatingTime") %>
                            </td>
                            <td>
                                <%# Eval("CuttingTime") %>
                            </td>

                            <td>
                                <%# Eval("Program1") %>
                            </td>
                            <td>
                                <%# Eval("Program2") %>
                            </td>
                            <td>
                                <%# Eval("Program3") %>
                            </td>
                            <td>
                                <%# Eval("Program4") %>
                            </td>
                            <td>
                                <%# Eval("TotalPartCount") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>

            </div>
            <div style="width: 100%; text-align: center; padding: 5px; display: none;">
                <asp:LinkButton runat="server" ID="lbPrevious" OnClick="lbPrevious_Click" CssClass="Btns" Text="Previous"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="lbNext" OnClick="lbNext_Click" CssClass="Btns" Text="Next"></asp:LinkButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="timerToAutoRefresh" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="timerToAutoRefresh" runat="server" Enabled="False" OnTick="timerToAutoRefresh_Tick"></asp:Timer>

    <div class="row" style="margin-top: 15px;">
        <div class=" col-lg-8 col-sm-8 col-md-8 " style="padding: 0px 5px;">
            <div>
                <asp:RadioButtonList runat="server" ID="rblLeftSelction" ClientIDMode="Static" CssClass=" checkbox-list" onchange="return BindLeftDataClick()" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Run Chart" Value="RunChart"></asp:ListItem>
                    <asp:ListItem Text="Hourly Run Time" Value="HourlyRunTime"></asp:ListItem>
                    <asp:ListItem Text="Hourly Part Count" Value="HourlyPartCount"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div id="leftchartcontainer">
            </div>
        </div>
        <div class=" col-lg-4 col-sm-4 col-md-4 " style="padding: 0px">
            <div>
                <asp:RadioButtonList runat="server" ID="rblRightSelction" ClientIDMode="Static" CssClass=" checkbox-list" onchange="return BindRightDataClick()" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Time Analysis" Value="TimeAnalysis"></asp:ListItem>
                    <asp:ListItem Text="Stoppages" Value="Stoppages"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div id="rightchartcontainer">
            </div>
        </div>

        <div class="modal infoModal" id="exportModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered " style="width: 950px">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="newEditModalTitle" runat="server">Reports</h4>
                    </div>
                    <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                        <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                            <table style="width: 100%; margin: auto" class="modal-tbl addcomponent" id="addcomponent" clientidmode="static">
                                <tr>
                                    <td style="padding-left: 20px">From Date</td>
                                    <td>
                                        <div class="input-group">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtExportFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td style="padding-left: 20px">To Date</td>
                                    <td>
                                        <div class="input-group">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtExportToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px">Plant</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlExportPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExportPlant_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td style="padding-left: 20px">Machine</td>
                                    <td>
                                        <asp:ListBox ID="lbExporMachine" runat="server" SelectionMode="Multiple" CssClass="textboxcss select listbox-control"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px">Shift</td>
                                    <td>
                                        <asp:ListBox ID="lbExportShift" runat="server" SelectionMode="Multiple" CssClass="textboxcss select listbox-control"></asp:ListBox>
                                    </td>
                                    <td style="padding-left: 20px">Report By</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlExportReportBy" ClientIDMode="Static" CssClass="form-control">
                                            <asp:ListItem Value="Day" Text="Day"></asp:ListItem>
                                            <asp:ListItem Value="Hour" Text="Hour"></asp:ListItem>
                                            <asp:ListItem Value="Shift" Text="Shift" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px">Report</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlReportType" ClientIDMode="Static" CssClass="form-control">
                                            <asp:ListItem Value="ProducionAnalysis" Text="Producion Analysis"></asp:ListItem>
                                            <asp:ListItem Value="Stoppages" Text="Stoppages"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary btn-style cancel-btn" data-dismiss="modal">CLOSE</button>
                        <asp:Button runat="server" ID="btnExportConfirm" ClientIDMode="Static" Text="EXPORT" CssClass="btn btn-primary btn-style " OnClick="btnExportConfirm_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <script>
        var Plant = "", Machine = "", FromDate = "", Shift = "", IsAutoRereshChecked = false;
        $(document).ready(function () {
            // alert("Document");
            setDateTimePicker();
        });
        function BindChartAfterPostBack() {
            BindLeftDataClick();
            BindRightDataClick();
        }
        function setDateTimePicker() {
            $('[id$=txtDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('.listbox-control').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=txtExportFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtExportToDate]').datetimepicker({
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
        function setFilterData(param) {
            Plant = $("#ddlPlant").val();
            if (param == "AutoRefresh") {
                if (Machine == "" || Machine == null) {
                    Machine = $(".pa-outer-tbl > tbody > tr:nth-child(3) > td:first-child > a").text().trim();
                }
            } else {
                Machine = $(".pa-outer-tbl > tbody > tr:nth-child(3) > td:first-child > a").text().trim();
            }
            Shift = $("#ddlShift").val();
            $("#hfMachine").val(Machine);
            FromDate = $("#txtDate").val();
            IsAutoRereshChecked = $("#cbAutoRefresh").prop('checked');
        }
        function BindLeftDataClick() {
            callLoader();
            setTimeout(function () {
                BindLeftData();
                callUnLoader();
            }, 100);
        }
        function BindLeftData() {
            let selectedleftoption = $('#rblLeftSelction input:checked').val();
            if (selectedleftoption == "RunChart") {
                //  BindRunTimeChart();
                startRunTimeChartBind();
            } else if (selectedleftoption == "HourlyRunTime") {
                startHourlyRunTimeChartBind();
            }
            else if (selectedleftoption == "HourlyPartCount") {
                startHourlyPartCountChartBind();
            }
        }
        function BindRightDataClick() {
            callLoader();
            setTimeout(function () {
                BindRightData();
                callUnLoader();
            }, 100);
        }
        function BindRightData() {
            let selectedrightoption = $('#rblRightSelction input:checked').val();
            if (selectedrightoption == "TimeAnalysis") {
                startTimeAnalysisChartBind();
            } else if (selectedrightoption == "Stoppages") {
                startStoppageDataBind();
            }
        }
        function BindChart(param) {
            setFilterData(param);
            BindChartDetails();
        }
        function BindChartDetails() {
            $.when(BindLeftData(), BindRightData());
            //BindLeftData();
            //BindRightData();
        }
        function BindHourlyRunTimeChart(dataitem) {

            $("#leftchartcontainer").empty();
            Highcharts.chart('leftchartcontainer', {
                chart: {
                    height: '310px'
                },
                title: {
                    text: Machine + ': Time'
                },

                yAxis: {
                    title: {
                        text: 'Time (min)'
                    }
                    ,
                    className: 'highchart-xyaxis-label'
                },

                xAxis: {
                    categories: dataitem.Date,
                    title: {
                        text: 'Time (hour)'
                    },
                    className: 'highchart-xyaxis-label'
                },

                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom'
                },

                series: [{
                    name: 'Power On Time',
                    data: dataitem.PowerOntTime
                }, {
                    name: 'Cutting Time',
                    data: dataitem.CuttingTime
                }, {
                    name: 'Operating Time',
                    data: dataitem.OperatingTime
                }],

                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                },
                exporting: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },


            });
            //    },
            //    error: function (Result) {
            //        alert("Error" + Result);
            //    }
            //});

        }

        function startHourlyRunTimeChartBind() {
            //if (chartWorker != null && chartWorker != undefined) {

            //} else {
            chartWorker = new Worker('Worker_ProductionAnalytics.js');
            // }

            const messageReceiver = (Message) => {
                BindHourlyRunTimeChart(Message.data.d);
            }
            chartWorker.onmessage = messageReceiver;
            postHourlyRTDataToWorker(chartWorker);
        }
        const postHourlyRTDataToWorker = (worker) => {
            worker.postMessage({ plant: Plant, machine: Machine, shift: Shift, date: FromDate, isautorefresh: IsAutoRereshChecked, param: "HourlyRunTime" });
        }


        function BindHourlyPartCountChart(dataitem) {

            //let shiftforhour = "";
            //for (let i = 0; i < $("#ddlShift option").length; i++) {
            //    let option = $("#ddlShift option")[i];
            //    let optionval = $(option).val();
            //    if (shiftforhour == "") {
            //        shiftforhour = optionval;
            //    }
            //    else {
            //        if (optionval != "Day")
            //            shiftforhour = shiftforhour + ":" + optionval;
            //    }
            //}
            <%--$.ajax({
                //async: false,
                type: "POST",
                url: '<%= ResolveUrl("ProductionAnalytics.aspx/getHourlyPartCountChartData") %>',
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                data: "{company:'" + Company + "',plant:'" + Plant + "',machine:'" + Machine + "',shift:'" + Shift + "',date:'" + FromDate + "',shiftforhour:'" + shiftforhour + "',isautorefresh:'" + IsAutoRereshChecked + "'}",
                dataType: "json",
                success: function (response) {
                    dataitem = response.d;--%>
            $("#leftchartcontainer").empty();
            Highcharts.chart('leftchartcontainer', {
                chart: {
                    type: 'column',
                    height: '310px'
                },
                title: {
                    text: Machine + ': Hourly Part Count'
                },
                xAxis: {
                    categories: dataitem.XAxisData,
                    title: {
                        text: 'Time (hour)'
                    },
                    className: 'highchart-xyaxis-label',
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Part Count'
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            fontWeight: 'bold',
                            //color: ( // theme
                            //    Highcharts.defaultOptions.title.style &&
                            //    Highcharts.defaultOptions.title.style.color
                            //) || 'gray',
                            textShadow: false,
                            textOutline: false,
                        }
                    },
                    className: 'highchart-xyaxis-label',
                },
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom',
                    //backgroundColor:
                    //    Highcharts.defaultOptions.legend.backgroundColor || 'white',
                    //borderColor: '#CCC',
                    //borderWidth: 1,
                    //shadow: false
                },
                tooltip: {
                    headerFormat: '<b>{point.x}</b><br/>',
                    pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                },
                plotOptions: {
                    column: {
                        stacking: 'normal',
                        dataLabels: {
                            enabled: true,
                            style: {
                                textShadow: false,
                                textOutline: false,
                            },
                            className: 'highchart-text-on-charts',
                            //to hide the 0 value label
                            formatter: function () {
                                var val = this.y;
                                if (val == 0) {
                                    return '';
                                }
                                return val;
                            },
                        }
                    }
                },
                exporting: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                series: dataitem.partCountSeriesDatas
            });
            //    },
            //    error: function (Result) {
            //        alert("Error" + Result);
            //    }
            //});
        }
        function startHourlyPartCountChartBind() {
            //if (chartWorker != null && chartWorker != undefined) {

            //} else {
            chartWorker = new Worker('Worker_ProductionAnalytics.js');
            // }

            const messageReceiver = (Message) => {
                BindHourlyPartCountChart(Message.data.d);
            }
            chartWorker.onmessage = messageReceiver;
            postHourlyPCDataToWorker(chartWorker);
        }
        const postHourlyPCDataToWorker = (worker) => {
            let shiftforhour = "";
            for (let i = 0; i < $("#ddlShift option").length; i++) {
                let option = $("#ddlShift option")[i];
                let optionval = $(option).val();
                if (shiftforhour == "") {
                    shiftforhour = optionval;
                }
                else {
                    if (optionval != "Day")
                        shiftforhour = shiftforhour + ":" + optionval;
                }
            }
            worker.postMessage({ plant: Plant, machine: Machine, shift: Shift, date: FromDate, isautorefresh: IsAutoRereshChecked, shiftforhour: shiftforhour, param: "HourlyPartCount" });
        }

        function BindRunTimeChart(dataitem) {
          <%--  $.ajax({
                // async: false,
                type: "POST",
                url: '<%= ResolveUrl("ProductionAnalytics.aspx/getRunTimeChartData") %>',
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                data: "{company:'" + Company + "',plant:'" + Plant + "',machine:'" + Machine + "',shift:'" + Shift + "',date:'" + FromDate + "',isautorefresh:'" + IsAutoRereshChecked + "'}",
                dataType: "json",
                success: function (response) {
                    dataitem = response.d;--%>

            $("#leftchartcontainer").empty();
            var firstPoint = null;
            var lastPoint = null;
            var tickInterval = 30 * 60 * 1000;
            if (dataitem.length > 0) {
                firstPoint = dataitem[0].x;
                lastPoint = dataitem[dataitem.length - 1].x2;
                if (dataitem[0].EnabledTickIneterval == false) {
                    tickInterval = null;
                }
            }
            Highcharts.chart("leftchartcontainer", {
                chart: {
                    type: 'xrange',
                    height: '310px',
                },
                title: {
                    text: Machine + ': Machine Status'
                },
                xAxis: {
                    min: firstPoint,
                    max: lastPoint,
                    tickInterval: tickInterval,
                    type: 'datetime',
                    //className: 'highchart-xyaxis-label',
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: 11,
                            fontWeight: 'bold',
                            color: 'black'
                        },
                        step: 2
                    },
                    //endOnTick: true,
                    //showFirstLabel: false,
                    //startOnTick: true
                    showLastLabel: true,
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    labels: {
                        enabled: false
                    },
                    // categories: chartdata.Category,
                    // className: 'highchart-xyaxis-label',
                    //  plotLines: chartdata.plotLines,
                    gridLineColor: 'transparent',
                    className: 'highchart-xyaxis-label'
                },

                tooltip: {
                    useHTML: true,
                    //  percentageDecimals: 2,
                    backgroundColor: "rgba(255,255,255,1)",
                    formatter: function () {
                        //let tmpTooltip = '<span style="color:' + this.point.color + '">\u25CF</span> ' + ': <b>' + this.point.name + '</b><br/>Alarm No.: <b>' + this.point.alarmno + '</b><br/> Start Time :  <b>' +
                        //    Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '</b><br/> End Time :  <b>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x2);
                        let tmpTooltip = '<span style="color:' + this.point.color + '">\u25CF</span> ' + ': <b>' + this.point.name + '</b><br/> Start Time :  <b>' +
                            Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '</b><br/> End Time :  <b>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x2);

                        return tmpTooltip;

                    },
                    // backgroundColor: '#FCFFC5',
                    style: {
                        zIndex: 99998
                    }
                },

                //navigator: {
                //    enabled: true,
                //    xAxis: {
                //        type: 'datetime',
                //        width: $('#leftchartcontainer').width() - 130
                //    }
                //},

                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        turboThreshold: 1000000,
                    }
                },
                //plotOptions: {
                //    series: {
                //        turboThreshold: 1000000,
                //        // pointInterval: 24 * 3600 * 1000 // one day
                //        pointInterval: 1000, // one day
                //        //groupPadding: 0,
                //        //pointPadding: 0,
                //        borderWidth: 0,
                //        borderRadius: 0,
                //        gapSize: 4
                //    }
                //},
                series: [{
                    //pointStart: firstPoint,
                    // pointPadding: 0,
                    // groupPadding: 0,
                    //  borderColor: 'gray',
                    //  borderWidth: 0,
                    // borderRadius: 0,
                    pointWidth: 50,
                    //data: data.series.data,
                    //turboThreshold: data.series.data.length,
                    data: dataitem,
                    //// dataLabels: {
                    ////    enabled: true
                    ////} 
                }]

            });

            //    },
            //    error: function (Result) {
            //        alert("Error" + Result);
            //    }
            //});
        }


        var chartWorker;
        function startRunTimeChartBind() {
            //if (chartWorker1 != null && chartWorker != undefined) {

            //} else {
            chartWorker = new Worker('Worker_ProductionAnalytics.js');
            // }

            const messageReceiver = (Message) => {
                BindRunTimeChart(Message.data.d);
            }
            chartWorker.onmessage = messageReceiver;
            postDatahWorker(chartWorker);
        }
        const postDatahWorker = (worker) => {
            worker.postMessage({ plant: Plant, machine: Machine, shift: Shift, date: FromDate, isautorefresh: IsAutoRereshChecked, param: "RunTimeChart" });
        }


        function BindStoppageData(dataitem) {
         <%--   $.ajax({
                // async: false,
                type: "POST",
                url: '<%= ResolveUrl("ProductionAnalytics.aspx/getStoppageReasonData") %>',
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                data: "{company:'" + Company + "',plant:'" + Plant + "',machine:'" + Machine + "',shift:'" + Shift + "',date:'" + FromDate + "',isautorefresh:'" + IsAutoRereshChecked + "'}",
                dataType: "json",
                success: function (response) {--%>
            //dataitem = response.d;
            $("#rightchartcontainer").empty();
            let str = "<div><div style='text-align: center;background-color:white;'><span style='font-weight:bold;color:black;font-size:15px;'>";
            str += dataitem.Title + " </span>";

            str += "<div style='overflow: auto; height: 290px'><table class='P1Table table table-bordered table-hover headerFixer'> <tr> <th>Start Time</th> <th>End Time</th> <th> Duration (hh:mm:ss)</th></tr>";
            for (let i = 0; i < dataitem.stoppageReasonDatas.length; i++) {
                str += "<tr><td>" + dataitem.stoppageReasonDatas[i].StartDate + "</td><td>" + dataitem.stoppageReasonDatas[i].EndDate + "</td><td>" + dataitem.stoppageReasonDatas[i].Value + "</td></tr>";
            }
            str + "</table></div></div></div>";
            $("#rightchartcontainer").append(str);
            //    },
            //    error: function (Result) {
            //        alert("Error" + Result);
            //    }
            //});
        }
        function startStoppageDataBind() {
            //if (chartWorker != null && chartWorker != undefined) {

            //} else {
            chartWorker = new Worker('Worker_ProductionAnalytics.js');
            // }

            const messageReceiver = (Message) => {
                BindStoppageData(Message.data.d);
            }
            chartWorker.onmessage = messageReceiver;
            postStoppageDataToWorker(chartWorker);
        }
        const postStoppageDataToWorker = (worker) => {
            worker.postMessage({ plant: Plant, machine: Machine, shift: Shift, date: FromDate, isautorefresh: IsAutoRereshChecked, param: "StoppageData" });
        }

        function BindTimeAnalysisChartData(dataitem) {
            if (dataitem != undefined) {


          <%--  $.ajax({
                //  async: false,
                type: "POST",
                url: '<%= ResolveUrl("ProductionAnalytics.aspx/getTimeAnalysisChartData") %>',
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                data: "{company:'" + Company + "',plant:'" + Plant + "',machine:'" + Machine + "',shift:'" + Shift + "',date:'" + FromDate + "',isautorefresh:'" + IsAutoRereshChecked + "'}",
                dataType: "json",
                success: function (response) {--%>
                // dataitem = response.d;
                $("#rightchartcontainer").empty();
                //let PFirstColor = "", PSecondColor = "", OFirstColor = "", OSecondColor = "", CFirstColor = "", CSecondColor = "";
                //if (dataitem.SummaryPowerOntTime >= dataitem.SummaryPowerOffTime) {
                //    PFirstColor = "#4e9cf5";
                //} else {

                //}
                var yaxisMax = null, tickAmount = null;
                if ($('#ddlShift').val() == "Day") {
                    yaxisMax = 24;
                    tickAmount = 5;
                }
                var MainTitlevalue = '';
                var time = dataitem.SummaryTotalTime;
                var hours1 = parseInt(time / 3600);
                var mins1 = parseInt((time % 3600) / 60);
                MainTitlevalue = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);

                Highcharts.chart('rightchartcontainer', {
                    chart: {
                        type: 'bar',
                        height: '310px'
                    },
                    title: {
                        text: Machine + ": Time Analysis (" + MainTitlevalue + " hh : mm)",
                    },
                    xAxis: {
                        categories: ['PowerOn/Off', 'OperatingTime/</br>Non-OperatingTime', 'CuttingTime/</br>Non-CuttingTime'],
                        className: 'highchart-xyaxis-label',
                    },
                    yAxis: {
                        min: 0,
                        tickAmount: tickAmount,
                        title: {
                            text: ''
                        },
                        className: 'highchart-xyaxis-label',
                        labels: {
                            formatter: function () {
                                var value = '';
                                var time = this.value;
                                var hours1 = parseInt(time / 3600);
                                var mins1 = parseInt((time % 3600) / 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                return value;
                            }
                        },
                    },
                    legend: {
                        enabled: false
                    }
                    ,
                    exporting: {
                        enabled: false
                    },
                    credits: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            stacking: 'normal',
                            dataLabels: {
                                enabled: true,
                                style: {
                                    textShadow: false,
                                    textOutline: false,
                                    //  color: 'black'
                                },
                                className: 'highchart-text-on-charts',
                                formatter: function () {
                                    if (this.y != 0) {
                                        var value = '';
                                        var time = this.y;
                                        var hours1 = parseInt(time / 3600);
                                        var mins1 = parseInt((time % 3600) / 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                        return value;
                                    }
                                },
                            },
                          
                        }
                    },
                    series: [{
                        name: '',
                        data: [
                            { y: dataitem.SummaryPowerOffTime, color: "#4e9cf5", name: 'Power Off' },
                            { y: dataitem.SummaryNonOperatingTime, color: "#f3f702", name: 'Non-Operating Time' },
                            { y: dataitem.SummaryOperatingWithoutCutting, color: "#0ff56b", name: 'Non-Cutting Time' }]
                    },
                    {
                        name: '',
                        data: [
                            { y: dataitem.SummaryPowerOntTime, color: "#87bfff", name: 'Power On' },
                            { y: dataitem.SummaryOperatingTime, color: "#feffb0", name: 'Operating Time' },
                            { y: dataitem.SummaryCuttingTime, color: "#bdffd7", name: 'Cutting Time' }]
                    },]
                });
                //    },
                //    error: function (Result) {
                //        alert("Error" + Result);
                //    }
                //});
            }
        }

        function startTimeAnalysisChartBind() {
            //if (chartWorker != null && chartWorker != undefined) {

            //} else {
            chartWorker = new Worker('Worker_ProductionAnalytics.js');
            // }

            const messageReceiver = (Message) => {
                BindTimeAnalysisChartData(Message.data.d);
            }
            chartWorker.onmessage = messageReceiver;
            postTimeAnalysisDataToWorker(chartWorker);
        }
        const postTimeAnalysisDataToWorker = (worker) => {
            worker.postMessage({ plant: Plant, machine: Machine, shift: Shift, date: FromDate, isautorefresh: IsAutoRereshChecked, param: "TimeAnalysis" });
        }

        function BindChartForSelectedMachine(element) {

            $.ajax({
                async: false,
                type: "POST",
                url: '<%= ResolveUrl("ProductionAnalytics.aspx/makeSessionNull") %>',
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                //data: "",
                dataType: "json",
                success: function (response) {
                },
                error: function (Result) {
                    alert("Error" + Result);
                }
            });

            let machineid = $(element).find("#lbMachineID").text().trim();

            if (machineid == "Machine ID") {
                return;
            }
            callLoader();
            setTimeout(function () {
                Machine = machineid;
                $("#hfMachine").val(Machine);
                BindChartDetails();
                callUnLoader();
            }, 100);
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
