<%@ Page Title="Flow Meter Dashboard" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="FlowMeterDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.FlowMeterDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/series-label.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>

    <style type="text/css">
        th {
            cursor: pointer;
        }

            th.headerSortUp {
                background-image: url(Image/asc.gif);
                background-position: right center;
                background-repeat: no-repeat;
            }

            th.headerSortDown {
                background-image: url(Image/desc.gif);
                background-position: right center;
                background-repeat: no-repeat;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .table tr th a {
            color: white;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            z-index: 10;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        .table tbody > tr > td {
            vertical-align: middle !important;
        }

        #MainContent_gridviewTableData tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        #MainContent_gridviewTableData tbody tr:nth-child(even) {
            background-color: white;
        }

        a {
            color: black;
        }

        .table .lbl {
            padding-top: 15px;
        }

        #MainContent_updateTableViewData {
            margin-top: -15px;
        }

        .GridHeader {
            text-align: center !important;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .dropdown {
            -webkit-appearance: none;
        }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }

        .multiselect {
            min-width: 160px;
        }

        .highcharts-container {
            margin: 0px;
        }

        .multiselect-container
        {
             height: 50vh;
             overflow: auto;     
        }
    </style>

    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdateFlowMeterDashboard" runat="server">
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="min-width: 80px;"><%=GetGlobalResourceObject("CommanResource","FromDate") %></td>
                            <td class="input-group" style="min-width: 160px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Style="min-width: 160px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="width: 80px;"><%=GetGlobalResourceObject("CommanResource","ToDate") %></td>
                            <td class="input-group" style="min-width: 160px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" Style="min-width: 160px;" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="width: 60px;"><%=GetGlobalResourceObject("CommanResource","Plant") %></td>
                            <td style="min-width: 180px;">
                                <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                            <td class="commontd" style="min-width: 80px;"><b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b></td>
                            <td style="min-width: 160px;">
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1" />
                            </td>
                            <td>
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnGetComponents" Text="Get Components" Style="min-width: 120px;" OnClientClick="ShowLoader()" OnClick="btnGetComponents_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="commanTd" style="min-width: 80px;">Component ID</td>
                            <td style="min-width: 160px;">
                                <asp:ListBox ID="ddlMultiComponentId" CssClass="textboxcommon dropdown multiDropdown" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ddlMultiComponentId_SelectedIndexChanged" Style="min-width: 160px;"></asp:ListBox>
                            </td>
                            <td>
                                <input type="button" id="btnSearch" class="btn btn-info" value="View Charts" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="container-fluid">
                    <div id="GraphFirstRow" class="row" style="height: 50%">
                        <div class="col-lg-6 col-md-6" id="FirstFlowMeterGraph"></div>
                        <div class="col-lg-6 col-md-6" id="SecondFlowMeterGraph"></div>
                    </div>
                    <div id="GraphSecondRow" class="row" style="height: 50%; margin-top: 6px;">
                        <div class="col-lg-6 col-md-6" id="ThirdFlowMeterGraph"></div>
                        <div class="col-lg-6 col-md-6" id="FourthFlowMeterGraph"></div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var count = 0;
        var LSLY = 2, MeanY = 2, USLY = 5, LSL20 = 3, USL20 = 2;
        var LSLX = 60, MeanX = 115, USLX = 60, LSLX20 = 60, USLX20 = 60;
        var MachineDesc = "";
        $(document).ready(function () {
            dateTimePicker();

            $('.multiDropdown').multiselect({
                includeSelectAllOption: false
            });
        });

        function dateTimePicker() {
            $('.date1').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('.date2').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $(".date1").on("dp.change", function (e) {
                $('.date2').data("DateTimePicker").minDate(e.date);
            });
            $(".date2").on("dp.change", function (e) {
                $('.date1').data("DateTimePicker").maxDate(e.date);
            });
        }

        function ShowLoader() {
            $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
        }

        function HideLoader() {
            $.unblockUI({});
        }

        $("[id$=btnGetComponents]").click(function () {
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
        });

        $("[id$=btnSearch]").click(function () {
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            if ($("[id$=ddlMachineId]").val() == "" || $("[id$=ddlMachineId]").val() == undefined || $("[id$=ddlMachineId]").val() == null) {
                alert("Select a machine first.")
                return false;
            }
            else {
                var machineID = $("[id$=ddlMachineId]").val();
                $.ajax({
                    type: "POST",
                    url: "FlowMeterDashboard.aspx/GetMachineDescription",
                    contentType: "application/json; charset=utf-8",
                    data: '{machineId:"' + machineID + '"}',
                    dataType: "json",
                    success: function (response) {
                        MachineDesc = response.d;
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                    }
                });
            }

            ShowLoader();
            count = $("[id$=ddlMultiComponentId]").find('option:selected').length;
            if (count != undefined && count > 0) {
                $("[id$=ddlMultiComponentId]").find('option:selected').each(function (index, selectedItem) {
                    var component = $(selectedItem).val();
                    if (component != undefined && component != null && component != "") {
                        GetFlowMeterData(component, index);
                    }
                });
                HideLoader();
            }
            else {
                alert("Please select a component.");
                HideLoader();
            }
        });

        function GetFlowMeterData(comp, index) {

            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();
            var machineID = $("[id$=ddlMachineId]").val();
            //var machineID = "3703044310";
            $.ajax({
                type: "POST",
                url: "FlowMeterDashboard.aspx/GetFlowMeterData",
                contentType: "application/json; charset=utf-8",
                data: '{fromDate:"' + from + '",toDate:"' + to + '",machineId:"' + machineID + '", componentId:"' + comp + '"}',
                dataType: "json",
                success: function (response) {

                    var FlowMeterData = response.d;
                    if (FlowMeterData.flowMeterChartDatas != null && FlowMeterData.flowMeterChartDatas != undefined) {
                        if (count == 1) {
                            $("#GraphSecondRow").css("height", "0px");
                            $("#GraphFirstRow").css("height", "100%");
                            $("#SecondFlowMeterGraph").attr("class", "");
                            $("#SecondFlowMeterGraph").css("width", "0px");
                            $("#FirstFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                            PlotFirstGraph(FlowMeterData, comp);
                        }
                        if (count == 2) {
                            $("#SecondFlowMeterGraph").attr("class", "");
                            $("#SecondFlowMeterGraph").css("width", "0px");
                            $("#FirstFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                            $("#FourthFlowMeterGraph").attr("class", "");
                            $("#FourthFlowMeterGraph").css("width", "0px");
                            $("#ThirdFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                            if (index == 0) {
                                PlotFirstGraph(FlowMeterData, comp);
                            }
                            if (index == 1) {
                                PlotThirdGraph(FlowMeterData, comp);
                            }
                        }
                        if (count == 3) {
                            $("#FourthFlowMeterGraph").attr("class", "");
                            $("#FourthFlowMeterGraph").css("width", "0px");
                            $("#ThirdFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                            if (index == 0) {
                                PlotFirstGraph(FlowMeterData, comp);
                            }
                            if (index == 1) {
                                PlotSecondGraph(FlowMeterData, comp);
                            }
                            if (index == 2) {
                                PlotThirdGraph(FlowMeterData, comp);
                            }
                        }
                        if (count == 4) {
                            if (index == 0) {
                                PlotFirstGraph(FlowMeterData, comp);
                            }
                            if (index == 1) {
                                PlotSecondGraph(FlowMeterData, comp);
                            }
                            if (index == 2) {
                                PlotThirdGraph(FlowMeterData, comp);
                            }
                            if (index == 3) {
                                PlotFourthGraph(FlowMeterData, comp);
                            }
                        }
                    }
                    else {
                        alert("No data available for component : " + comp);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }

        function PlotFirstGraph(GraphSeries, component) {
            let plotLineList = [];
            if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
            }
            else {
                plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
            }
            Highcharts.chart('FirstFlowMeterGraph', {
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: 'Flow Meter Graph : ' + component,
                    style: {
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                subtitle: {
                    text: 'Plot of individual flow values for component',
                    style: {
                        fontWeight: 'bold'
                    }
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
                xAxis: {
                    title: {
                        text: 'Part Count',
                        style: {
                            fontWeight: 'bold'
                        }
                    }
                },
                rangeSelector: {
                    buttons: [{
                        count: 15,
                        type: 'minute',
                        text: '15M'
                    }, {
                        count: 30,
                        type: 'minute',
                        text: '30M'
                    }, {
                        count: 1,
                        type: 'hour',
                        text: '1H'
                    },
                    {
                        count: 2,
                        type: 'hour',
                        text: '2H'
                    }, {
                        count: 5,
                        type: 'hour',
                        text: '5H'
                    }, {
                        count: 8,
                        type: 'hour',
                        text: '8H'
                    }],
                    inputEnabled: false,
                    selected: 3
                },

                yAxis: {
                    //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                    max: 500,
                    title: {
                        text: 'Clearance ( cc/min)',
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    plotLines: plotLineList,
                    //plotLines: [{
                    //    value: GraphSeries.MinValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '',
                    //        x: LSLX,
                    //        y: LSLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MedianValue,
                    //    color: 'green',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '',
                    //        x: MeanX,
                    //        y: MeanY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '',
                    //        x: USLX,
                    //        y: USLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MinValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        /*text: GraphSeries.MaxValue != 0 ? 'USL - Upper Limit : ' + GraphSeries.MaxValue : '',*/
                    //        text: GraphSeries.MinValue1 != 0 ? '+20: ' + GraphSeries.MinValue1 : '',
                    //        x: LSLX20,
                    //        y: LSL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        /*text: GraphSeries.MaxValue != 0 ? 'USL - Upper Limit : ' + GraphSeries.MaxValue : '',*/
                    //        text: GraphSeries.MaxValue1 != 0 ? '-20: ' + GraphSeries.MaxValue1 : '',
                    //        x: USLX20,
                    //        y: USL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}]
                },
                rangeSelector: {
                    buttons: [{
                        count: 15,
                        type: 'minute',
                        text: '15M'
                    }, {
                        count: 30,
                        type: 'minute',
                        text: '30M'
                    }, {
                        count: 1,
                        type: 'hour',
                        text: '1H'
                    },
                    {
                        count: 2,
                        type: 'hour',
                        text: '2H'
                    }, {
                        count: 5,
                        type: 'hour',
                        text: '5H'
                    }, {
                        count: 8,
                        type: 'hour',
                        text: '8H'
                    }],
                    inputEnabled: false,
                    selected: 3
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    y: 30
                },

                plotOptions: {
                    series: {
                        color: '#000060',
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: 1
                    }
                },
                tooltip: {
                    formatter: function () {
                        return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                    }
                },
                series: GraphSeries.flowMeterChartDatas,

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
                    buttons: {
                        contextButton: {
                            menuItems: ["viewFullscreen",
                                "printChart",
                                "separator",
                                "downloadPNG",
                                "downloadJPEG",
                                "downloadPDF",
                                "downloadSVG",
                                "separator",
                                "downloadCSV",
                                "downloadXLS"
                            ]
                        }
                    }
                }
            });
        }

        function PlotSecondGraph(GraphSeries, component) {
               let plotLineList = [];
                if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }
                else {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }
            Highcharts.chart('SecondFlowMeterGraph', {
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: 'Flow Meter Graph : ' + component,
                    style: {
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                subtitle: {
                    text: 'Plot of individual flow values for component',
                    style: {
                        fontWeight: 'bold'
                    }
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
                rangeSelector: {
                    buttons: [{
                        count: 15,
                        type: 'minute',
                        text: '15M'
                    }, {
                        count: 30,
                        type: 'minute',
                        text: '30M'
                    }, {
                        count: 1,
                        type: 'hour',
                        text: '1H'
                    },
                    {
                        count: 2,
                        type: 'hour',
                        text: '2H'
                    }, {
                        count: 5,
                        type: 'hour',
                        text: '5H'
                    }, {
                        count: 8,
                        type: 'hour',
                        text: '8H'
                    }],
                    inputEnabled: false,
                    selected: 3
                },
                xAxis: {
                    title: {
                        text: 'Part Count',
                        style: {
                            fontWeight: 'bold'
                        }
                    }
                },
                yAxis: {
                    //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                    max: 500,
                    title: {
                        text: 'Clearance ( cc/min)',
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    plotLines: plotLineList,
                    //plotLines: [{
                    //    value: GraphSeries.MinValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '',
                    //        x: LSLX,
                    //        y: LSLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MedianValue,
                    //    color: 'green',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '',
                    //        x: MeanX,
                    //        y: MeanY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '',
                    //        x: USLX,
                    //        y: USLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MinValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        /*text: GraphSeries.MaxValue != 0 ? 'USL - Upper Limit : ' + GraphSeries.MaxValue : '',*/
                    //        text: GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '',
                    //        x: LSLX20,
                    //        y: LSL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        /*text: GraphSeries.MaxValue != 0 ? 'USL - Upper Limit : ' + GraphSeries.MaxValue : '',*/
                    //        text: GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '',
                    //        x: USLX20,
                    //        y: USL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}]
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    y: 30
                },

                plotOptions: {
                    series: {
                        color: '#000060',
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: 1
                    }
                },
                tooltip: {
                    formatter: function () {
                        return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                    }
                },
                series: GraphSeries.flowMeterChartDatas,

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
                    buttons: {
                        contextButton: {
                            menuItems: ["viewFullscreen",
                                "printChart",
                                "separator",
                                "downloadPNG",
                                "downloadJPEG",
                                "downloadPDF",
                                "downloadSVG",
                                "separator",
                                "downloadCSV",
                                "downloadXLS"
                            ]
                        }
                    }
                }
            });
        }

        function PlotThirdGraph(GraphSeries, component) {
            let plotLineList = [];
          
            if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
            }
            else {
                plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));

            }
            Highcharts.chart('ThirdFlowMeterGraph', {
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: 'Flow Meter Graph : ' + component,
                    style: {
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                subtitle: {
                    text: 'Plot of individual flow values for component',
                    style: {
                        fontWeight: 'bold'
                    }
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
                xAxis: {
                    title: {
                        text: 'Part Count',
                        style: {
                            fontWeight: 'bold'
                        }
                    }
                },
                rangeSelector: {
                    buttons: [{
                        count: 15,
                        type: 'minute',
                        text: '15M'
                    }, {
                        count: 30,
                        type: 'minute',
                        text: '30M'
                    }, {
                        count: 1,
                        type: 'hour',
                        text: '1H'
                    },
                    {
                        count: 2,
                        type: 'hour',
                        text: '2H'
                    }, {
                        count: 5,
                        type: 'hour',
                        text: '5H'
                    }, {
                        count: 8,
                        type: 'hour',
                        text: '8H'
                    }],
                    inputEnabled: false,
                    selected: 3
                },
                yAxis: {
                    //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                    max: 500,
                    title: {
                        text: 'Clearance ( cc/min)',
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    plotLines: plotLineList,
                    //plotLines: [{
                    //    value: GraphSeries.MinValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '',
                    //        x: LSLX,
                    //        y: LSLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MedianValue,
                    //    color: 'green',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '',
                    //        x: MeanX,
                    //        y: MeanY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '',
                    //        x: USLX,
                    //        y: USLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MinValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '',
                    //        x: LSLX20,
                    //        y: LSL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '',
                    //        x: USLX20,
                    //        y: USL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}]
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    y: 30
                },

                plotOptions: {
                    series: {
                        color: '#000060',
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: 1
                    }
                },
                tooltip: {
                    formatter: function () {
                        return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                    }
                },
                series: GraphSeries.flowMeterChartDatas,

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
                    buttons: {
                        contextButton: {
                            menuItems: ["viewFullscreen",
                                "printChart",
                                "separator",
                                "downloadPNG",
                                "downloadJPEG",
                                "downloadPDF",
                                "downloadSVG",
                                "separator",
                                "downloadCSV",
                                "downloadXLS"
                            ]
                        }
                    }
                }
            });
        }

        function PlotFourthGraph(GraphSeries, component) {
            let plotLineList = [];
            if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
            }
            else {
                plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
            }

            Highcharts.chart('FourthFlowMeterGraph', {
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: 'Flow Meter Graph : ' + component,
                    style: {
                        fontWeight: 'bold'
                    }
                },
                credits: {
                    enabled: false
                },
                subtitle: {
                    text: 'Plot of individual flow values for component',
                    style: {
                        fontWeight: 'bold'
                    }
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
                xAxis: {
                    title: {
                        text: 'Part Count',
                        style: {
                            fontWeight: 'bold'
                        }
                    }
                },
                rangeSelector: {
                    buttons: [{
                        count: 15,
                        type: 'minute',
                        text: '15M'
                    }, {
                        count: 30,
                        type: 'minute',
                        text: '30M'
                    }, {
                        count: 1,
                        type: 'hour',
                        text: '1H'
                    },
                    {
                        count: 2,
                        type: 'hour',
                        text: '2H'
                    }, {
                        count: 5,
                        type: 'hour',
                        text: '5H'
                    }, {
                        count: 8,
                        type: 'hour',
                        text: '8H'
                    }],
                    inputEnabled: false,
                    selected: 3
                },
                yAxis: {
                    //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                    max: 500,
                    title: {
                        text: 'Clearance ( cc/min)',
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    plotLines: plotLineList,
                    //plotLines: [{
                    //    value: GraphSeries.MinValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '',
                    //        x: LSLX,
                    //        y: LSLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MedianValue,
                    //    color: 'green',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '',
                    //        x: MeanX,
                    //        y: MeanY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '',
                    //        x: USLX,
                    //        y: USLY,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MinValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        text: GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '',
                    //        x: LSLX20,
                    //        y: LSL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}, {
                    //    value: GraphSeries.MaxValue1,
                    //    color: 'red',
                    //    dashStyle: 'solid',
                    //    width: 2,
                    //    label: {
                    //        /*text: GraphSeries.MaxValue != 0 ? 'USL - Upper Limit : ' + GraphSeries.MaxValue : '',*/
                    //        text: GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '',
                    //        x: USLX20,
                    //        y: USL20,
                    //        align: 'right',
                    //        style: {
                    //            fontWeight: 'bold'
                    //        }
                    //    }
                    //}]
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    y: 30
                },

                plotOptions: {
                    series: {
                        color: '#000060',
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: 1
                    }
                },
                tooltip: {
                    formatter: function () {
                        return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                    }
                },
                series: GraphSeries.flowMeterChartDatas,

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
                    buttons: {
                        contextButton: {
                            menuItems: ["viewFullscreen",
                                "printChart",
                                "separator",
                                "downloadPNG",
                                "downloadJPEG",
                                "downloadPDF",
                                "downloadSVG",
                                "separator",
                                "downloadCSV",
                                "downloadXLS"
                            ]
                        }
                    }
                }
            });
        }

        function GetMaxLimit(arr) {
            var max = GetArrayMax(arr);
            return Math.ceil(max / 100) * 100;
        }

        function GetArrayMax(arr) {
            var len = arr.length;
            var max = -Infinity;
            while (len--) {
                if (arr[len] > max) {
                    max = arr[len];
                }
            }
            return max;
        }

        function GetArrayMin(arr) {
            var len = arr.length;
            var min = Infinity;
            while (len--) {
                if (arr[len] < min) {
                    min = arr[len];
                }
            }
            return min;
        }
        var multiselctListExpand = false;
        function openComponentIdModal() {
            multiselctListExpand = true;
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            dateTimePicker();
            var count = 0;
            var LSLY = 2, MeanY = 2, USLY = 5, LSL20 = 3, USL20 = 2;
            var LSLX = 60, MeanX = 115, USLX = 60, LSLX20 = 60, USLX20 = 60;
            $('.multiDropdown').multiselect({
                includeSelectAllOption: false
            });
            $(document).ready(function () {
                if (multiselctListExpand) {
                    $(".btn-group").addClass('open');
                } else {
                    $(".btn-group").removeClass('open');
                }
            });

            function dateTimePicker() {
                $('.date1').datetimepicker({
                    format: 'YYYY-MM-DD HH:mm:ss',
                    locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
                });
                $('.date2').datetimepicker({
                    format: 'YYYY-MM-DD HH:mm:ss',
                    useCurrent: false,
                    locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
                });
                $(".date1").on("dp.change", function (e) {
                    $('.date2').data("DateTimePicker").minDate(e.date);
                });
                $(".date2").on("dp.change", function (e) {
                    $('.date1').data("DateTimePicker").maxDate(e.date);
                });
            }

            function ShowLoader() {
                $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
            }

            function HideLoader() {
                $.unblockUI({});
            }

            $("[id$=btnGetComponents]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
            });

            $("[id$=btnSearch]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
                if ($("[id$=ddlMachineId]").text() == "" || $("[id$=ddlMachineId]").val() == undefined || $("[id$=ddlMachineId]").val() == null) {
                    alert("Select a machine first.")
                    return false;
                }
                else {

                    var machineID = $("[id$=ddlMachineId]").val();
                    $.ajax({
                        type: "POST",
                        url: "FlowMeterDashboard.aspx/GetMachineDescription",
                        contentType: "application/json; charset=utf-8",
                        data: '{machineId:"' + machineID + '"}',
                        dataType: "json",
                        success: function (response) {
                            MachineDesc = response.d;
                        },
                        error: function (jqXHR, textStatus, err) {
                            alert('Error: ' + err);
                        }
                    });
                }

                ShowLoader();
                count = $("[id$=ddlMultiComponentId]").find('option:selected').length;
                if (count != undefined && count > 0) {
                    $("[id$=ddlMultiComponentId]").find('option:selected').each(function (index, selectedItem) {
                        var component = $(selectedItem).val();
                        if (component != undefined && component != null && component != "") {
                            GetFlowMeterData(component, index);
                        }
                    });
                    HideLoader();
                }
                else {
                    alert("Please select a component.");
                    HideLoader();
                }
            });

            function GetFlowMeterData(comp, index) {

                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();
                var machineID = $("[id$=ddlMachineId]").val();
                
                //var machineID = "3703044310";
                $.ajax({
                    type: "POST",
                    url: "FlowMeterDashboard.aspx/GetFlowMeterData",
                    contentType: "application/json; charset=utf-8",
                    data: '{fromDate:"' + from + '",toDate:"' + to + '",machineId:"' + machineID + '", componentId:"' + comp + '"}',
                    dataType: "json",
                    success: function (response) {

                        var FlowMeterData = response.d;
                        if (FlowMeterData.flowMeterChartDatas != null && FlowMeterData.flowMeterChartDatas != undefined) {
                            if (count == 1) {
                                $("#GraphSecondRow").css("height", "0px");
                                $("#GraphFirstRow").css("height", "100%");
                                $("#SecondFlowMeterGraph").attr("class", "");
                                $("#SecondFlowMeterGraph").css("width", "0px");
                                $("#FirstFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                                PlotFirstGraph(FlowMeterData, comp);
                            }
                            if (count == 2) {
                                $("#SecondFlowMeterGraph").attr("class", "");
                                $("#SecondFlowMeterGraph").css("width", "0px");
                                $("#FirstFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                                $("#FourthFlowMeterGraph").attr("class", "");
                                $("#FourthFlowMeterGraph").css("width", "0px");
                                $("#ThirdFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                                if (index == 0) {
                                    PlotFirstGraph(FlowMeterData, comp);
                                }
                                if (index == 1) {
                                    PlotThirdGraph(FlowMeterData, comp);
                                }
                            }
                            if (count == 3) {
                                $("#FourthFlowMeterGraph").attr("class", "");
                                $("#FourthFlowMeterGraph").css("width", "0px");
                                $("#ThirdFlowMeterGraph").attr("class", "col-lg-12 col-md-12");
                                if (index == 0) {
                                    PlotFirstGraph(FlowMeterData, comp);
                                }
                                if (index == 1) {
                                    PlotSecondGraph(FlowMeterData, comp);
                                }
                                if (index == 2) {
                                    PlotThirdGraph(FlowMeterData, comp);
                                }
                            }
                            if (count == 4) {
                                if (index == 0) {
                                    PlotFirstGraph(FlowMeterData, comp);
                                }
                                if (index == 1) {
                                    PlotSecondGraph(FlowMeterData, comp);
                                }
                                if (index == 2) {
                                    PlotThirdGraph(FlowMeterData, comp);
                                }
                                if (index == 3) {
                                    PlotFourthGraph(FlowMeterData, comp);
                                }
                            }
                        }
                        else {
                            alert("No data available for component : " + comp);
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                    }
                });
            }

            function GetPlotLineDetails(GraphSeriesVal,color,textval,x,y) {
                var data = {};
                let labelValues = {};
                let labelStyle = {};
                data.value = GraphSeriesVal;
                data.color = color;
                data.dashStyle = 'solid';
                data.width = 2;
                labelValues.text = textval;
                labelValues.x = x;
                labelValues.y = y;
                labelValues.align = 'right';
                labelStyle.fontWeight = 'bold';
                labelValues.style = labelStyle;
                data.label = labelValues;
                return data;
            }

            function PlotFirstGraph(GraphSeries, component) {
                let plotLineList = [];
                if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }
                else {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }
                Highcharts.chart('FirstFlowMeterGraph', {
                    chart: {
                        zoomType: 'x'
                    },
                    title: {
                        text: 'Flow Meter Graph : ' + component,
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    subtitle: {
                        text: 'Plot of individual flow values for component',
                        style: {
                            fontWeight: 'bold'
                        }
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
                    xAxis: {
                        title: {
                            text: 'Part Count',
                            style: {
                                fontWeight: 'bold'
                            }
                        }
                    },
                    rangeSelector: {
                        buttons: [{
                            count: 15,
                            type: 'minute',
                            text: '15M'
                        }, {
                            count: 30,
                            type: 'minute',
                            text: '30M'
                        }, {
                            count: 1,
                            type: 'hour',
                            text: '1H'
                        },
                        {
                            count: 2,
                            type: 'hour',
                            text: '2H'
                        }, {
                            count: 5,
                            type: 'hour',
                            text: '5H'
                        }, {
                            count: 8,
                            type: 'hour',
                            text: '8H'
                        }],
                        inputEnabled: false,
                        selected: 3
                    },

                    yAxis: {
                        //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                        max: 500,
                        title: {
                            text: 'Clearance ( cc/min)',
                            style: {
                                fontWeight: 'bold'
                            }
                        },
                        plotLines: plotLineList
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'top',
                        y: 30
                    },

                    plotOptions: {
                        series: {
                            color: '#000060',
                            label: {
                                connectorAllowed: false
                            },
                            pointStart: 1
                        }
                    },
                    tooltip: {
                        formatter: function () {
                            return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                        }
                    },
                    series: GraphSeries.flowMeterChartDatas,

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
                        buttons: {
                            contextButton: {
                                menuItems: ["viewFullscreen",
                                    "printChart",
                                    "separator",
                                    "downloadPNG",
                                    "downloadJPEG",
                                    "downloadPDF",
                                    "downloadSVG",
                                    "separator",
                                    "downloadCSV",
                                    "downloadXLS"
                                ]
                            }
                        }
                    }
                });
            }

            function PlotSecondGraph(GraphSeries, component) {
                let plotLineList = [];
                if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }
                else {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }

                Highcharts.chart('SecondFlowMeterGraph', {
                    chart: {
                        zoomType: 'x'
                    },
                    title: {
                        text: 'Flow Meter Graph : ' + component,
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    subtitle: {
                        text: 'Plot of individual flow values for component',
                        style: {
                            fontWeight: 'bold'
                        }
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
                    xAxis: {
                        title: {
                            text: 'Part Count',
                            style: {
                                fontWeight: 'bold'
                            }
                        }
                    },
                    rangeSelector: {
                        buttons: [{
                            count: 15,
                            type: 'minute',
                            text: '15M'
                        }, {
                            count: 30,
                            type: 'minute',
                            text: '30M'
                        }, {
                            count: 1,
                            type: 'hour',
                            text: '1H'
                        },
                        {
                            count: 2,
                            type: 'hour',
                            text: '2H'
                        }, {
                            count: 5,
                            type: 'hour',
                            text: '5H'
                        }, {
                            count: 8,
                            type: 'hour',
                            text: '8H'
                        }],
                        inputEnabled: false,
                        selected: 3
                    },
                    yAxis: {
                        //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                        max: 500,
                        title: {
                            text: 'Clearance ( cc/min)',
                            style: {
                                fontWeight: 'bold'
                            }
                        },
                        plotLines: plotLineList
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'top',
                        y: 30
                    },

                    plotOptions: {
                        series: {
                            color: '#000060',
                            label: {
                                connectorAllowed: false
                            },
                            pointStart: 1
                        }
                    },
                    tooltip: {
                        formatter: function () {
                            return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                        }
                    },
                    series: GraphSeries.flowMeterChartDatas,

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
                        buttons: {
                            contextButton: {
                                menuItems: ["viewFullscreen",
                                    "printChart",
                                    "separator",
                                    "downloadPNG",
                                    "downloadJPEG",
                                    "downloadPDF",
                                    "downloadSVG",
                                    "separator",
                                    "downloadCSV",
                                    "downloadXLS"
                                ]
                            }
                        }
                    }
                });
            }

            function PlotThirdGraph(GraphSeries, component) {
                let plotLineList = [];
                if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }
                else {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));

                }

                Highcharts.chart('ThirdFlowMeterGraph', {
                    chart: {
                        zoomType: 'x'
                    },
                    title: {
                        text: 'Flow Meter Graph : ' + component,
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    subtitle: {
                        text: 'Plot of individual flow values for component',
                        style: {
                            fontWeight: 'bold'
                        }
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
                    xAxis: {
                        title: {
                            text: 'Part Count',
                            style: {
                                fontWeight: 'bold'
                            }
                        }
                    },
                    rangeSelector: {
                        buttons: [{
                            count: 15,
                            type: 'minute',
                            text: '15M'
                        }, {
                            count: 30,
                            type: 'minute',
                            text: '30M'
                        }, {
                            count: 1,
                            type: 'hour',
                            text: '1H'
                        },
                        {
                            count: 2,
                            type: 'hour',
                            text: '2H'
                        }, {
                            count: 5,
                            type: 'hour',
                            text: '5H'
                        }, {
                            count: 8,
                            type: 'hour',
                            text: '8H'
                        }],
                        inputEnabled: false,
                        selected: 3
                    },
                    yAxis: {
                        //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                        max: 500,
                        title: {
                            text: 'Clearance ( cc/min)',
                            style: {
                                fontWeight: 'bold'
                            }
                        },
                        plotLines: plotLineList
                        
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'top',
                        y: 30
                    },

                    plotOptions: {
                        series: {
                            color: '#000060',
                            label: {
                                connectorAllowed: false
                            },
                            pointStart: 1
                        }
                    },
                    tooltip: {
                        formatter: function () {
                            return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                        }
                    },
                    series: GraphSeries.flowMeterChartDatas,

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
                        buttons: {
                            contextButton: {
                                menuItems: ["viewFullscreen",
                                    "printChart",
                                    "separator",
                                    "downloadPNG",
                                    "downloadJPEG",
                                    "downloadPDF",
                                    "downloadSVG",
                                    "separator",
                                    "downloadCSV",
                                    "downloadXLS"
                                ]
                            }
                        }
                    }
                });
            }

            function PlotFourthGraph(GraphSeries, component) {
                let plotLineList = [];
                if (MachineDesc.toLowerCase().indexOf("shaft") != -1) {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }
                else {
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue, "red", GraphSeries.MinValue != 0 ? 'LSL: ' + GraphSeries.MinValue : '', LSLX, LSLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MedianValue, "green", GraphSeries.MedianValue != 0 ? 'Mean(Xbar): ' + GraphSeries.MedianValue : '', MeanX, MeanY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue, "red", GraphSeries.MaxValue != 0 ? 'USL: ' + GraphSeries.MaxValue : '', USLX, USLY));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MinValue1, "red", GraphSeries.MinValue1 != 0 ? '-20: ' + GraphSeries.MinValue1 : '', LSLX20, LSL20));
                    plotLineList.push(GetPlotLineDetails(GraphSeries.MaxValue1, "red", GraphSeries.MaxValue1 != 0 ? '+20: ' + GraphSeries.MaxValue1 : '', USLX20, USL20));
                }

                Highcharts.chart('FourthFlowMeterGraph', {
                    chart: {
                        zoomType: 'x'
                    },
                    title: {
                        text: 'Flow Meter Graph : ' + component,
                        style: {
                            fontWeight: 'bold'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    subtitle: {
                        text: 'Plot of individual flow values for component',
                        style: {
                            fontWeight: 'bold'
                        }
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
                    xAxis: {
                        title: {
                            text: 'Part Count',
                            style: {
                                fontWeight: 'bold'
                            }
                        }
                    },
                    rangeSelector: {
                        buttons: [{
                            count: 15,
                            type: 'minute',
                            text: '15M'
                        }, {
                            count: 30,
                            type: 'minute',
                            text: '30M'
                        }, {
                            count: 1,
                            type: 'hour',
                            text: '1H'
                        },
                        {
                            count: 2,
                            type: 'hour',
                            text: '2H'
                        }, {
                            count: 5,
                            type: 'hour',
                            text: '5H'
                        }, {
                            count: 8,
                            type: 'hour',
                            text: '8H'
                        }],
                        inputEnabled: false,
                        selected: 3
                    },
                    yAxis: {
                        //max: GetMaxLimit(GraphSeries.flowMeterChartDatas[0].data),
                        max: 500,
                        title: {
                            text: 'Clearance ( cc/min)',
                            style: {
                                fontWeight: 'bold'
                            }
                        },
                        plotLines: plotLineList
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'top',
                        y: 30
                    },

                    plotOptions: {
                        series: {
                            color: '#000060',
                            label: {
                                connectorAllowed: false
                            },
                            pointStart: 1
                        }
                    },
                    tooltip: {
                        formatter: function () {
                            return 'Flow Value<b>: ' + this.y + '</b><br/>Start Time<b>: ' + this.point.StartDateTime + '</b><br/>End Time<b>: ' + this.point.EndDateTime + '</b>';
                        }
                    },
                    series: GraphSeries.flowMeterChartDatas,

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
                        buttons: {
                            contextButton: {
                                menuItems: ["viewFullscreen",
                                    "printChart",
                                    "separator",
                                    "downloadPNG",
                                    "downloadJPEG",
                                    "downloadPDF",
                                    "downloadSVG",
                                    "separator",
                                    "downloadCSV",
                                    "downloadXLS"
                                ]
                            }
                        }
                    }
                });
            }

            function GetMaxLimit(arr) {
                var max = GetArrayMax(arr);
                return Math.ceil(max / 100) * 100;
            }

            function GetArrayMax(arr) {
                var len = arr.length;
                var max = -Infinity;
                while (len--) {
                    if (arr[len] > max) {
                        max = arr[len];
                    }
                }
                return max;
            }

            function GetArrayMin(arr) {
                var len = arr.length;
                var min = Infinity;
                while (len--) {
                    if (arr[len] < min) {
                        min = arr[len];
                    }
                }
                return min;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
