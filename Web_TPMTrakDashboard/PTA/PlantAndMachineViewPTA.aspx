<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlantAndMachineViewPTA.aspx.cs" Inherits="Web_TPMTrakDashboard.PTA.PlantAndMachineViewPTA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <script src="../Scripts/HighCharts10.3.2/highcharts.js"></script>
    <script src="../Scripts/HighCharts10.3.2/highcharts-3d.js"></script>
    <style>
        fieldset {
            border: 1px solid white;
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
            text-align: center;
            font-size: 16px;
        }

        .plantGridTbl tr td {
            font-size: 20px;
            font-weight: bold;
            padding: 8px;
            border: 1px solid silver;
        }

        .redcolor-style td {
            color: red;
        }

        .whitecolor-style td {
            color: white !important;
        }

        #MachineViewContainer a {
            color: #0000b7;
            text-decoration: underline;
        }

        .chartDiv {
            height: 40vh;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-bottom: 0px;">
                <tr>

                    <td class="commanTd" style="vertical-align: middle;">Plant</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
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
                    <td class="commanTd" style="vertical-align: middle;">View Type</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlViewType" ClientIDMode="Static" CssClass="form-control">
                            <asp:ListItem Text="Plant" Value="Plant"></asp:ListItem>
                            <asp:ListItem Text="Machine" Value="Machine"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                    </td>
                </tr>
            </table>
            <div id="PlantViewContainer" runat="server" clientidmode="static">

                <fieldset>
                    <legend>
                        <asp:Label runat="server" Text="Data aggregated from " ClientIDMode="Static"></asp:Label>
                        <asp:Label runat="server" ID="lblMachineCount" ClientIDMode="Static"></asp:Label>
                        <asp:Label runat="server" Text="machines" ClientIDMode="Static"></asp:Label>
                    </legend>
                    <table style="width: 100%; table-layout: fixed;">
                        <tr>
                            <td>
                                <div id="totalTimeChartDiv" class="chartDiv"></div>
                            </td>
                            <%-- <td>
                                <div id="otherChartDiv" class="chartDiv"></div>
                            </td>--%>
                        </tr>
                    </table>
                </fieldset>
                <table class="plantGridTbl alternate-table-style" style="width: 100%; table-layout: fixed; margin-top: 10px;">
                    <tr>
                        <td>Production Time (Nominal OEE %)
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
            </div>
            <div id="MachineViewContainer" runat="server" clientidmode="static" style="height: 80vh; overflow: auto;">
                <asp:ListView runat="server" ID="lvMachineData" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="table table-bordered headerFixer alternate-table-style">
                            <tr>
                                <th>Machine</th>
                                <th>Production Time (OEE %)</th>
                                <th>Load-unload</th>
                                <th>Down Time</th>
                                <th>No. Planned Parts (cycles)</th>
                                <th>No. Actual Parts (cycles)</th>
                                <th>Delivery (%)</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr style='background-color: <%# Eval("RowBackColor") %>; color: <%# Eval("RowForeColor") %>' class='<%# Eval("RowForeColor").ToString()=="white"?"whitecolor-style":"" %>'>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkMachine" ClientIDMode="Static" Text='<%# Eval("Machine") %>' OnClientClick="return goToDayShiftPage(this);"></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("ProductionTime") %>
                            </td>
                            <td>
                                <%# Eval("LoadUnLoad") %>
                            </td>
                            <td>
                                <%# Eval("DownTime") %>
                            </td>
                            <td>
                                <%# Eval("PlannedParts") %>
                            </td>
                            <td>
                                <%# Eval("ActualParts") %>
                            </td>
                            <td>
                                <%# Eval("Delivery") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setDateTimePicker();
        });
        function goToDayShiftPage(ctrl) {
            window.open("DayAndShiftViewPTA.aspx?date=" + $('#txtDate').val() + "&plantId=" + $('#ddlPlant').val() + "&machineID=" + $(ctrl).text(), "DayAndShiftView");
            return false;
        }
        function BindPlantChart() {
            $.ajax({
                async: false,
                type: "POST",
                url: '<%= ResolveUrl("PlantAndMachineViewPTA.aspx/getPlantChartData") %>',
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (response) {
                    var chartData = response.d;
                    BindPlantPieChart(chartData);
                    //BindPlantBarChart(chartData.LoadUnloadEffy, chartData.MachineStoppageEffy);
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });

        }
        function BindPlantPieChart(chartData) {
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
                //tooltip: {
                //    pointFormat: '<b>{point.percentage:.2f}%</b>'
                //},
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
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
                        events: {
                            click: function (event) {
                                window.open("../RunTimeChart.aspx?date=" + $('#txtDate').val() + "&plantId=" + $('#ddlPlant').val() + "&source=PTA", "RunTimeChart");
                            }
                        }
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
        function BindPlantBarChart(loadUnloadEffy, stoppageEffy) {
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
                        text: ''
                    },
                    labels: {
                        enabled: false
                    },
                    stackLabels: {
                        enabled: false
                    }
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
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
                },
                {
                    name: 'Load Unload',
                    data: [loadUnloadEffy],
                    color: '#6666FF'
                }]
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
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
