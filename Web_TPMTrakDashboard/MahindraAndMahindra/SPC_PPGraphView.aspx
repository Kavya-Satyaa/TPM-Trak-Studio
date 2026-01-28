<%@ Page Title="PP Graph View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SPC_PPGraphView.aspx.cs" Inherits="Web_TPMTrakDashboard.MahindraAndMahindra.SPC_PPGraphView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%--    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/stock/modules/data.js"></script>
    <script src="https://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/stock/modules/export-data.js"></script>--%>
    <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>

    <link href="../css/elegant-icons-style.css" rel="stylesheet" />

    <style>
        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

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

        #tdLstParameters .multiselect.btn {
            overflow: hidden;
            width: 200px;
        }
    </style>
    <div>
        <%--<asp:ScriptManager runat="server" />--%>
        <div class="container-fluid row">
            <div style="display: flex">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered" style="display: inline-block; text-align: center; vertical-align: initial; display: inline-grid;">
                            <tr>
                                <td style="vertical-align: inherit; color: white;">
                                    <b><span style="width: 100px; margin: 10px; vertical-align: central">Machine-ID</span></b>
                                </td>
                                <td style="vertical-align: inherit; min-width: 100px;">
                                    <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" AutoPostBack="true" />
                                </td>
                                <td id="tdFromDateLbl" runat="server" style="vertical-align: middle" class="commontd GraphView"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                                <td id="tdFromDate" runat="server" class=" GraphView" style="width: 240px; vertical-align: middle;">
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDate" Height="40" runat="server" CssClass="form-control date" data-toggle="tooltip" title="From DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </td>
                                <td id="tdToDateLbl" runat="server" class="commontd GraphView" style="vertical-align: middle"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                                <td id="tdToDate" runat="server" class="GraphView" style="width: 240px; vertical-align: middle;">
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDate" Height="40" runat="server" CssClass="form-control date" data-toggle="tooltip" title="To DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </td>
                                <td id="tdParameterLbl" runat="server" style="vertical-align: inherit; color: white;" class="GraphView">
                                    <b><span style="width: 100px; margin: 10px;">Parameters</span></b>
                                </td>
                                <td id="tdLstParameters" runat="server" clientidmode="static" style="vertical-align: inherit" class="GraphView">
                                    <asp:ListBox ID="lstParameters" Width="200" Style="margin: 10px; max-width: 300px" runat="server" SelectionMode="Multiple" ClientIDMode="Static" OnSelectedIndexChanged="lstParameters_SelectedIndexChanged"></asp:ListBox>
                                    <%--change="Checklistboxchanges"--%>
                                </td>
                                <td id="tdMarker" runat="server" class="GraphView">
                                    <asp:CheckBox runat="server" ID="chkShowMarker" ClientIDMode="Static" Text="Show Marker" Style="color: white" />
                                </td>
                                <td id="tdGraphViewBtn" runat="server" style="vertical-align: inherit; font: bold 20px" class="GraphView">

                                    <%--<asp:Button runat="server" ID="btnGraphView" CssClass="btn btn-success" Width="150" Font-Bold="true" Font-Size="14" OnClick="btnGraphView_Click" Height="45" Text="Graph View" ClientIDMode="Static" Style="margin: 10px;" />--%>

                                    <input type="button" id="btnGraphView" class="btn btn-success" style="width: 110px; font-size: 14px; font-weight: bold; margin: 10px" value="Graph View" />
                                </td>
                                <%--<td style="vertical-align: inherit; color: white;">
                            <asp:CheckBox runat="server" ID="GraphView" OnCheckedChanged="GraphView_CheckedChanged" Text="Graph View" />
                        </td>--%>
                            </tr>


                        </table>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row clearfix" id="DiVChart" runat="server">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <div class=" card">
                    <div class="card-head">
                        <header id="FirstChartHeader"><b></b></header>
                        <div class="tools Year">
                            <%--  <a><span class="icon_refresh" onclick="RefreshData('FirstChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('FirstChart')" style="font-size: 20px;"></span></a>--%>
                        </div>
                    </div>
                    <div id="divFirstChart" style="margin: 10px">
                    </div>
                </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <div class=" card">
                    <div class="card-head">
                        <header id="SecondChartHeader"><b></b></header>
                        <div class="tools Year">
                            <a><span class="icon_refresh" onclick="RefreshData('SecondChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('SecondChart')" style="font-size: 20px;"></span></a>
                        </div>
                    </div>
                    <div id="divSecondChart" style="margin: 10px">
                    </div>
                </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <div class=" card">
                    <div class="card-head">
                        <header id="ThirdChartHeader"><b></b></header>
                        <div class="tools Year">
                            <%-- <a><span class="icon_refresh" onclick="RefreshData('ThirdChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('ThirdChart')" style="font-size: 20px;"></span></a>--%>
                        </div>
                    </div>
                    <div id="divThirdChart" style="margin: 10px">
                    </div>
                </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <div class=" card">
                    <div class="card-head">
                        <header id="FourthChartHeader"><b></b></header>
                        <div class="tools Year">
                            <%-- <a><span class="icon_refresh" onclick="RefreshData('FourthChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('FourthChart')" style="font-size: 20px;"></span></a>--%>
                        </div>
                    </div>
                    <div id="divFourthChart" style="margin: 10px">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function clearChartContainer() {
            $('#FirstChartHeader').text("");
            $("#divFirstChart").css("display", "none");
            $('#SecondChartHeader').text("");
            $("#divSecondChart").css("display", "none");
            $('#ThirdChartHeader').text("");
            $("#divThirdChart").css("display", "none");
            $('#FourthChartHeader').text("");
            $("#divFourthChart").css("display", "none");
        }
        function setChartContainer(param) {
            if (param == "first") {
                $("#divFirstChart").css("display", "");
            }
            else if (param == "second") {
                $("#divSecondChart").css("display", "");
            }
            else if (param == "third") {
                $("#divThirdChart").css("display", "");
            }
            else if (param == "fourth") {
                $("#divFourthChart").css("display", "");
            }
        }
        function RefreshData(param) {
            switch (param) {
                case "FirstChart":
                    BindFirstChart(("#FirstChartHeader").val());
                    break;
                case "SecondChart":
                    BindSecondChart(("#SecondChartHeader").val());
                    break;
                case "ThirdChart":
                    BindThirdChart(("#ThirdChartHeader").val());
                    break;
                case "FourthChart":
                    BindFourthChart(("#FourthChartHeader").val());
                    break;
            }
        }
        function HideShowMenu(param) {
            debugger;
            switch (param) {
                case "FirstChart":
                    if ($('#divFirstChart:visible').length > 0) {
                        $("#divFirstChart").css("display", "none");
                    }
                    else {
                        $("#divFirstChart").css("display", "");
                    }
                    break;
                case "SecondChart":
                    if ($('#divSecondChart:visible').length > 0) {
                        $("#divSecondChart").css("display", "none");
                    }
                    else {
                        $("#divSecondChart").css("display", "");
                    }
                    break;
                case "ThirdChart":
                    if ($('#divThirdChart:visible').length > 0) {
                        $("#divThirdChart").css("display", "none");
                    }
                    else {
                        $("#divThirdChart").css("display", "");
                    }
                    break;
                case "FourthChart":
                    if ($('#divFourthChart:visible').length > 0) {
                        $("#divFourthChart").css("display", "none");
                    }
                    else {
                        $("#divFourthChart").css("display", "");
                    }
                    break;
            }
        }
        $(document).ready(function () {
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
            //$("#cmbtype").change(function () {

            //    if ($("#cmbtype").val() == "Dashboard") {
            //        $(".DashboardView").css("display", "");
            //        $(".GraphView").css("display", "none");
            //    }
            //    else if ($("#cmbtype").val() == "Graph") {
            //        $(".DashboardView").css("display", "none");
            //        $(".GraphView").css("display", "");
            //    }
            //});
            $("#btnGraphView").click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                clearChartContainer();
                var count = 0;
                $("#lstParameters option").each(function (i) {
                    if (this.selected) {
                        count++;
                        switch (count) {
                            case 1:
                                BindFirstChart(this.value);
                                break;
                            case 2:
                                BindSecondChart(this.value);
                                break;
                            case 3:
                                BindThirdChart(this.value);
                                break;
                            case 4:
                                BindFourthChart(this.value);
                                break;
                        }
                    }
                });
                $.unblockUI({});
            });
            $('#tdLstParameters ul.dropdown-menu').on('mousedown', 'li', function (event) {
                debugger;
                if ($('#tdLstParameters ul.dropdown-menu li.active').length >= 4 && (!$(this).hasClass("active"))) {
                    alert("Parameter Cannot be greater than 4");
                    event.preventDefault();
                }
            }).on('change', 'li', function () {
                setListBoxValue();
            });
            setListBoxValue();
            $("#btnGraphView").click();
            //$("#lstParameters option").change(function () {
            //    var count = 0;

            //    $("#lstParameters option").each(function (i) {
            //        if (this.selected) {
            //            count++;
            //            if (count > 4) {
            //                alert("Parameter Cannot be greater than 4")
            //                this.selected = false;
            //            }
            //        }
            //    });
            //});
        });
        function setListBoxValue() {
            if ($('#tdLstParameters').find('span.multiselect-selected-text').text() == "All") {
                $('#tdLstParameters').find('span.multiselect-selected-text').text("");
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
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
            //$("#cmbtype").change(function () {


            //    if ($("#cmbtype").val() == "Dashboard") {
            //        $(".DashboardView").css("display", "");
            //        $(".GraphView").css("display", "none");
            //    }
            //    else if ($("#cmbtype").val() == "Graph") {
            //        $(".DashboardView").css("display", "none");
            //        $(".GraphView").css("display", "");
            //    }
            //});
            $("#btnGraphView").click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                clearChartContainer();
                var count = 0;
                $("#lstParameters option").each(function (i) {
                    if (this.selected) {
                        count++;
                        switch (count) {
                            case 1:
                                BindFirstChart(this.value);
                                break;
                            case 2:
                                BindSecondChart(this.value);
                                break;
                            case 3:
                                BindThirdChart(this.value);
                                break;
                            case 4:
                                BindFourthChart(this.value);
                                break;
                        }
                    }
                });
                $.unblockUI({});
            });
            $('#tdLstParameters ul.dropdown-menu').on('mousedown', 'li', function (event) {
                debugger;
                if ($('#tdLstParameters').find('span.multiselect-selected-text').text() == "All") {
                    $('#tdLstParameters').find('span.multiselect-selected-text').text("");
                }
                if ($('#tdLstParameters ul.dropdown-menu li.active').length >= 4 && (!$(this).hasClass("active"))) {
                    alert("Parameter Cannot be greater than 4");
                    event.preventDefault();
                }
                if ($('#tdLstParameters').find('span.multiselect-selected-text').text() == "All") {
                    $('#tdLstParameters').find('span.multiselect-selected-text').text("");
                }
            }).on('change', 'li', function () {
                if ($('#tdLstParameters').find('span.multiselect-selected-text').text() == "All") {
                    $('#tdLstParameters').find('span.multiselect-selected-text').text("");
                }
            });
            setListBoxValue();
            //$("#lstParameters").change(function () {
            //    var count = 0;
            //    $("#lstParameters option").each(function (i) {
            //        $("#lstParameters option").each(function (i) {
            //            if (this.selected) {
            //                count++;
            //                if (count > 4) {
            //                    alert("Parameter Cannot be greater than 4")
            //                    this.selected = false;
            //                }
            //            }
            //        });
            //    });
            //});

        });
        function getMarkerValue() {
            var IsMasrkerEnabled = true;
            IsMasrkerEnabled = $('#chkShowMarker').prop("checked");
            return IsMasrkerEnabled;
        }
        function BindFirstChart(parameter) {
            setChartContainer("first");
            var MachineID = $("[id$=ddlMachineID]").val();
            var startdate = $("[id$=txtFromDate]").val();
            var enddate = $("[id$=txtToDate]").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SPC_PPGraphView.aspx/GetFirstParameter",
                data: '{Parameter: "' + parameter + '",MachineID:"' + MachineID + '",StartDate:"' + startdate + '",EndDate:"' + enddate + '",IsMarkerEnabled:"' + getMarkerValue() + '"}',
                dataType: "json",
                success: function (result) {
                    var Data = result.d;

                    FirstChartView((Data));

                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }
        function BindSecondChart(parameter) {
            setChartContainer("second");
            var MachineID = $("[id$=ddlMachineID]").val();
            var startdate = $("[id$=txtFromDate]").val();
            var enddate = $("[id$=txtToDate]").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SPC_PPGraphView.aspx/GetSecondParameter",
                data: '{Parameter: "' + parameter + '",MachineID:"' + MachineID + '",StartDate:"' + startdate + '",EndDate:"' + enddate + '",IsMarkerEnabled:"' + getMarkerValue() + '"}',
                dataType: "json",
                success: function (result) {
                    var Data = result.d;

                    SecondChartView((Data));

                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        function BindThirdChart(parameter) {
            setChartContainer("third");
            var MachineID = $("[id$=ddlMachineID]").val();
            var startdate = $("[id$=txtFromDate]").val();
            var enddate = $("[id$=txtToDate]").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SPC_PPGraphView.aspx/GetThirdParameter",
                data: '{Parameter: "' + parameter + '",MachineID:"' + MachineID + '",StartDate:"' + startdate + '",EndDate:"' + enddate + '",IsMarkerEnabled:"' + getMarkerValue() + '"}',
                dataType: "json",
                success: function (result) {
                    var Data = result.d;

                    ThirdChartView((Data));

                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }
        function BindFourthChart(parameter) {
            setChartContainer("fourth");
            var MachineID = $("[id$=ddlMachineID]").val();
            var startdate = $("[id$=txtFromDate]").val();
            var enddate = $("[id$=txtToDate]").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SPC_PPGraphView.aspx/GetFourthParameter",
                data: '{Parameter: "' + parameter + '",MachineID:"' + MachineID + '",StartDate:"' + startdate + '",EndDate:"' + enddate + '",IsMarkerEnabled:"' + getMarkerValue() + '"}',
                dataType: "json",
                success: function (result) {
                    var Data = result.d;

                    FourthChartView((Data));

                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        var SameData = {
            title: null,
            tooltip: { enabled: true },
            yAxis: {
                min: 0,
            },
            credits: {
                enabled: false
            },
        };
        function FirstChartView(Data) {
            debugger;
            $("#FirstChartHeader").text(Data.Title);
            chart = Highcharts.stockChart('divFirstChart', {
                title: {
                    text: '',
                    style: {
                        fontSize: '20px',
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                lang: {
                    noData: "No Data available for selected datetime and machine Id."
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
                    // selected: 3
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
                    categories: Data.Category,
                    plotLines: Data.PlotLines,
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
                            debugger;
                            if (point.series.name == "Vibration Data") {
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
                series: Data.series,
            });
        }

        function SecondChartView(Data) {
            $("#SecondChartHeader").text(Data.Title);
            chart = Highcharts.stockChart('divSecondChart', {
                title: {
                    text: '',
                    style: {
                        fontSize: '20px',
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                lang: {
                    noData: "No Data available for selected datetime and machine Id."
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
                    //selected: 3
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
                    categories: Data.Category,
                    plotLines: Data.PlotLines,
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
                            debugger;
                            if (point.series.name == "Vibration Data") {
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
                series: Data.series,
            });
        }

        function ThirdChartView(Data) {
            $("#ThirdChartHeader").text(Data.Title);
            chart = Highcharts.stockChart('divThirdChart', {
                title: {
                    text: '',
                    style: {
                        fontSize: '20px',
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                lang: {
                    noData: "No Data available for selected datetime and machine Id."
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
                    //selected: 3
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
                    categories: Data.Category,
                    plotLines: Data.PlotLines,
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
                            debugger;
                            if (point.series.name == "Vibration Data") {
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
                series: Data.series,
            });
        }

        function FourthChartView(Data) {
            $("#FourthChartHeader").text(Data.Title);
            chart = Highcharts.stockChart('divFourthChart', {
                title: {
                    text: '',
                    style: {
                        fontSize: '20px',
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                lang: {
                    noData: "No Data available for selected datetime and machine Id."
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
                    // selected: 3
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
                    categories: Data.Category,
                    plotLines: Data.PlotLines,
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
                            debugger;
                            if (point.series.name == "Vibration Data") {
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
                series: Data.series,
            });
        }


        function RefreshData(Param) {

        }
        function HideShowMenu(Param) {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
