<%@ Page Language="C#" Title="Assembly Line Andon" AutoEventWireup="true" CodeBehind="AssemblyLineAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.AssemblyLineAndon" EnableEventValidation="false" Async="true" AsyncTimeout="120000" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Assembly Line Andon</title>
    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/Highchart8/highcharts.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/Highchart8/pareto.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/Highchart8/exporting.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/Highchart8/export-data.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/Highchart8/accessibility.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/Styles.css" rel="stylesheet" />
</head>

<body>
    <form id="andonForm" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdfChartsCount" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdfCachInterval" ClientIDMode="Static" />
        <div class="container-fluid">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row text-center">
                        <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 5px; background-color: #3777bc">
                            <div class="HeaderImage">
                                <asp:Image ID="ImgLogo" runat="server" class="img-responsive img-rounded" Style="width: 165px; height: 60px;" ImageUrl="~/Advik/Images/Advik_Logo.jpg" />
                            </div>
                            <label id="headerName" runat="server" clientidmode="static" style="color: white; font-weight: bold; font-size: 33px; text-align: right; margin-top: 5px">Assembly Line Andon</label>
                            <div style="float: right; position: relative; display: inline-flex">
                                <div style="text-align: left">
                                    <p class="headerRight"><%: DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss tt")%>&nbsp;&nbsp;</p>
                                    <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" Style="margin-top: 0px" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" />
                                    <asp:DropDownList runat="server" ID="ddlGroupID" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" Style="margin-top: 0px" OnSelectedIndexChanged="ddlGroupID_SelectedIndexChanged" />
                                </div>
                                &nbsp;&nbsp;
                               
                                <div>
                                    <p style="margin: 0px" onclick="location.href='#';">
                                        <img src="Images/List-Icon.jpg" height="29" />
                                    </p>
                                    <p class="headerRight"><span style="font-size: 18px; cursor: pointer; color: white; vertical-align: text-top" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span></p>
                                </div>
                            </div>
                            &nbsp;&nbsp;                       
                        </div>
                    </div>

                    <asp:Timer runat="server" ID="timerAndonRefresh" OnTick="timerAndonRefresh_Tick"></asp:Timer>
                    <div class="row">
                        <asp:Label runat="server" ID="lblMessages" EnableViewState="False" Style="font-weight: bold; font-family: Calibri;" />
                    </div>
                    <div class="row" style="margin-top: 15px">
                        <div id="poContainer" runat="server" clientidmode="static" style="height: 85vh; overflow-x: auto; overflow-y: auto; width: 100%;">
                            <asp:ListView runat="server" ItemPlaceholderID="placeHolderMachinewiseAndonData" ID="lstMachinewiseAndonData">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="placeHolderMachinewiseAndonData" />
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <div class="andonDataItem" style="margin: 15px; min-width: 250px; min-height: 300px; display: inline-block;">
                                        <div class="border">
                                            <div style="padding: 10px; background-color: #8BAF3F; border-radius: 25px;" class="dataItemContainer">
                                                <table style="width: 100%;" id="tblMachineID">
                                                    <tbody>
                                                        <tr>
                                                            <td style="text-align: center; color: black; font-weight: bold; padding-bottom: 5px;">
                                                                <asp:Label ID="lblMachine" runat="server"><%# Eval("MachineID")%></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <div class="itemContainer" style="background-color:white;">
                                                    <table class="table table-bordered" id="tblMachinewiseAndonData" style='background-color: white;'>
                                                        <thead>
                                                            <tr>
                                                                <th>Parameter</th>
                                                                <th style="text-align: center;"><span class="glyphicon glyphicon-ok" style="color: darkgreen;"></span></th>
                                                                <th style="text-align: center;"><span class="glyphicon glyphicon-remove" style="color: darkred;"></span></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:ListView ID="lstParameterData" runat="server" DataSource='<%# Eval("ParameterDataList") %>' ItemPlaceholderID="paramDataPlaceHolder">
                                                                <LayoutTemplate>
                                                                    <asp:PlaceHolder runat="server" ID="paramDataPlaceHolder" />
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td style="text-align: left; min-width: 100px;"><%# Eval("Parameter")%></td>
                                                                        <td style="text-align: center; min-width: 40px;"><%# Eval("SuccessValue")%></td>
                                                                        <td style="text-align: center; min-width: 40px;"><%# Eval("FailValue")%></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>

                    <div class="row" style="height: 85vh;">
                        <div class="col-sm-12 col-md-6 col-lg-6">
                            <div class="partsCountItem" style="margin: 15px; width: 95%; display: inline-block;">
                                <div class="border" style="padding: 10px;">
                                    <table style="width: 100%;" id="tblDateShift">
                                        <tbody>
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-bottom: 5px;">
                                                    <asp:Label ID="lblDateShift" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                    <asp:ListView runat="server" ID="lstAndonPartsCount" ItemPlaceholderID="andonPartsCountPlaceHolder">
                                        <LayoutTemplate>
                                            <table class="table table-bordered" id="tblAndonPartsCountData" style='background-color: white;'>
                                                <thead>
                                                    <tr>
                                                        <th style="font-weight: bold;">Machine ID</th>
                                                        <th style="font-weight: bold; text-align: center;">Parts</th>
                                                        <th style="font-weight: bold; text-align: center;">OK</th>
                                                        <th style="font-weight: bold; text-align: center;">Not OK</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder runat="server" ID="andonPartsCountPlaceHolder" />
                                                </tbody>
                                                <tfoot>
                                                    <tr style="text-align: center">
                                                        <td colspan="4">
                                                            <div>
                                                                <asp:Label CssClass="inlineLabel" runat="server" Text="TGT = " Style="font-weight: bold;" />
                                                                <asp:Label CssClass="inlineLabel" runat="server" ID="lblTGT" />
                                                                <asp:Label CssClass="inlineLabel" runat="server" Text="ACT = " Style="font-weight: bold; margin-left: 10px;" />
                                                                <asp:Label CssClass="inlineLabel" runat="server" ID="lblACT" />
                                                                <asp:Label CssClass="inlineLabel" runat="server" Text="Throughput = " Style="font-weight: bold; margin-left: 10px;" />
                                                                <asp:Label CssClass="inlineLabel" runat="server" ID="lblThroughput" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tfoot>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: left; min-width: 100px;"><%# Eval("MachineID")%></td>
                                                <td style="text-align: center; min-width: 40px;"><%# Eval("TotalPartsCount")%></td>
                                                <td style="text-align: center; min-width: 40px;"><%# Eval("OkPartsCount")%></td>
                                                <td style="text-align: center; min-width: 40px;"><%# Eval("NotOkPartsCount")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-12 col-md-6 col-lg-6" style="height: 90%;">
                            <div class="row" style="text-align: center; background-color: beige; height: 30px; width: 95%; margin-left: 5px; margin-right: 5px;">
                                <asp:Label runat="server" ID="lblMonthMTD" Font-Bold="true" Font-Size="Large" Style="vertical-align: -webkit-baseline-middle; }" />
                            </div>
                            <div class="efficiencyCharts" style="margin: 15px; width: 100%; display: inline-block;">
                                <div id="downTimeChart" style="width: 100%; height: 40%"></div>
                                <div id="efficiencyChart" style="width: 100%; height: 40%"></div>
                                <div style="text-align: center;">
                                    <label style="font-weight: bold;">MONTH AVERAGE : </label>
                                    <label style="font-weight: bold;">TGT = </label>
                                    <label id="lblMonthlyTGT"></label>
                                    <label style="font-weight: bold;">, ACT = </label>
                                    <label id="lblMonthlyACT"></label>
                                    <label style="font-weight: bold;">, Throughput = </label>
                                    <label id="lblMonthlyThroughput"></label>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <footer>
                <div class="navbar navbar-default navbar-fixed-bottom footerBottom" style="padding: 0px 5px; background-color: #3777bc; min-height: 40px; text-align: center">
                    <div style="float: left;">
                        <p style="color: #fcefef; font-style: italic; margin-top: 10px; font-size: 16px; display: inline-block">Powered by TPM-Trak®</p>
                        <asp:UpdatePanel runat="server" style="display: inline-block">
                            <ContentTemplate>
                                &nbsp;&nbsp;&nbsp;                              
                                <asp:ImageButton runat="server" ID="imgBtnSwitch" ImageUrl="Images/andon.jpg" ToolTip="Switch to ANDON Mode" Style="width: 32px; height: 27px; position: relative; top: 7px" OnClientClick="return blockUI();" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgBtnSwitch" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div style="display: inline-block; margin-top: 4px; width: 84%">
                    </div>
                    <div style="float: right;">
                        <img src="Images/AMIT.jpg" height="43" width="70" />
                    </div>
                </div>
            </footer>
        </div>

        <script type="text/javascript">
            var showItem = NaN, divH, divW, screenH, screenW, totalH, totalW, totalBox;
            var currentTab = 0;

            $(document).ready(function () {
                var winHeight = $(window).height();
                if (winHeight < 650) winHeight = 640;
                $("#poContainer").height(winHeight - 300);
                CountNumberOfBoxes();
                GetAllDowntimeAndEfficiencyDetails();
            });

            function CountNumberOfBoxes() {
                screenH = $(window).height() - 170;
                screenW = $(window).width() - 180;
                totalH = Math.floor(screenH / (divH));
                totalW = Math.floor(screenW / (divW));
                totalBox = Math.floor(totalH * totalW);
                showItem = totalBox;
            }

            function ApplyAndonItemDimensions() {
                divH = Math.max.apply(null, $('.andonDataItem').map(function () {
                    return $(this).outerHeight(true);
                }).get());
                $(".dataItemContainer").height(divH - 30);
                $(".itemContainer").height(divH - 58);
                divW = Math.max.apply(null, $('.andonDataItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                $(".andonDataItem").width(divW);
            }

            $("#ddlPlantID").change(function () {
                ApplyAndonItemDimensions();
                GetAllDowntimeAndEfficiencyDetails();
            });

            function GetAllDowntimeAndEfficiencyDetails() {
                let plantID = $("#ddlPlantID").val();
                let cellID = $("#ddlGroupID").val();
                if (plantID && plantID !== "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AssemblyLineAndon.aspx/GetDowntimeAndEfficiencyDetails",
                        //data: '{plantID:"' + plantID + '"}',
                        data: '{plantID:"' + plantID + '",CellID:"' + cellID + '"}',
                        dataType: "json",
                        success: function (result) {
                            var AllDowntimeAndEfficiencyDetails = result.d;
                            if (AllDowntimeAndEfficiencyDetails) {
                                PlotDowntimeGraph(AllDowntimeAndEfficiencyDetails.DownTimeDetails);
                                PlotEfficiencyGraph(AllDowntimeAndEfficiencyDetails.EfficiencyDetails);
                                SetMonthwiseAvgEffData(AllDowntimeAndEfficiencyDetails.monthwiseAvgEfficiency);
                            }
                        },
                        error: function (Result) {
                            alert("Error Code : 501 - Internal Server Error");
                        }
                    });
                }
            }

            function PlotDowntimeGraph(DownTimeDetails) {
                var seriesData = [];
                var categoryList = [];
                if (DownTimeDetails) {
                    for (i = 0; i < DownTimeDetails.length; i++) {
                        categoryList.push(DownTimeDetails[i].DownID);
                        seriesData.push([DownTimeDetails[i].DownID, DownTimeDetails[i].Downtime]);
                    }
                }
                Highcharts.chart('downTimeChart', {
                    chart: {
                        renderTo: 'downTimeChart',
                        type: 'column'
                    },
                    credits: {
                        enabled: false
                    },
                    exporting: {
                        enabled: false
                    },
                    title: {
                        text: 'Top 10 Line Downtimes'
                    },
                    xAxis: {
                        type: 'category',
                        categories: categoryList,
                        crosshair: true,
                        labels: {
                            rotation: -45,
                            style: {
                                fontSize: '13px',
                                fontFamily: 'Verdana, sans-serif'
                            }
                        }
                    },
                    yAxis: [{
                        min: 0,
                        title: {
                            text: 'Time (In Minutes)'
                        }
                    }, {
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        labels: {
                            format: "{value}%"
                        }
                    }],
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        shared: true
                    },
                    series: [{
                        type: 'pareto',
                        name: 'Pareto',
                        yAxis: 1,
                        zIndex: 10,
                        baseSeries: 1,
                        tooltip: {
                            valueDecimals: 2,
                            valueSuffix: '%'
                        }
                    },
                    {
                        name: 'DownTime',
                        type: 'column',
                        zIndex: 2,
                        data: seriesData
                    }]
                });
            }

            function PlotEfficiencyGraph(EfficiencyDetails) {
                var seriesData = [];
                var categoryList = [];
                if (EfficiencyDetails) {
                    for (i = 0; i < EfficiencyDetails.length; i++) {
                        categoryList.push(EfficiencyDetails[i].EfficiencyID);
                        seriesData.push([EfficiencyDetails[i].EfficiencyID, EfficiencyDetails[i].Efficiency]);
                    }
                }
                Highcharts.chart('efficiencyChart', {
                    chart: {
                        renderTo: 'efficiencyChart',
                        type: 'bar'
                    },
                    credits: {
                        enabled: false
                    },
                    exporting: {
                        enabled: false
                    },
                    title: {
                        text: 'Efficiency Chart'
                    },
                    subtitle: {
                        text: ''
                    },
                    xAxis: {
                        type: 'category',
                        categories: categoryList,
                        crosshair: true,
                        labels: {
                            style: {
                                fontSize: '13px',
                                fontFamily: 'Verdana, sans-serif'
                            }
                        }
                    },
                    yAxis: {
                        min: 0,
                        max: 100,
                        title: {
                            text: 'Efficiency (In Percentage)',
                            align: 'high',
                            fontWeight: 'bold'
                        },
                        labels: {
                            overflow: 'justify'
                        }
                    },
                    tooltip: {
                        valueSuffix: ' %'
                    },
                    plotOptions: {
                        bar: {
                            colorByPoint: true,
                            dataLabels: {
                                enabled: true
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    series: [{
                        name: 'Efficiency',
                        data: seriesData,
                    }]
                });
            }

            function SetMonthwiseAvgEffData(MonthwiseAvgEfficiency) {
                if (MonthwiseAvgEfficiency) {
                    document.querySelector("#lblMonthlyTGT").textContent = MonthwiseAvgEfficiency.Target;
                    document.querySelector("#lblMonthlyACT").textContent = MonthwiseAvgEfficiency.Actual;
                    document.querySelector("#lblMonthlyThroughput").textContent = MonthwiseAvgEfficiency.Throughput;
                }
            }

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $("#ddlPlantID").change(function () {
                    ApplyAndonItemDimensions();
                    GetAllDowntimeAndEfficiencyDetails();
                });

                function GetAllDowntimeAndEfficiencyDetails() {
                    let plantID = $("#ddlPlantID").val();
                    let cellID = $("#ddlGroupID").val();
                    if (plantID && plantID !== "") {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AssemblyLineAndon.aspx/GetDowntimeAndEfficiencyDetails",
                            data: '{plantID:"' + plantID + '",CellID:"' + cellID + '"}',
                            dataType: "json",
                            success: function (result) {
                                var AllDowntimeAndEfficiencyDetails = result.d;
                                if (AllDowntimeAndEfficiencyDetails) {
                                    PlotDowntimeGraph(AllDowntimeAndEfficiencyDetails.DownTimeDetails);
                                    PlotEfficiencyGraph(AllDowntimeAndEfficiencyDetails.EfficiencyDetails);
                                    SetMonthwiseAvgEffData(AllDowntimeAndEfficiencyDetails.monthwiseAvgEfficiency);
                                }
                            },
                            error: function (Result) {
                                alert("Error Code : 501 - Internal Server Error");
                            }
                        });
                    }
                }

                function PlotDowntimeGraph(DownTimeDetails) {
                    var seriesData = [];
                    var categoryList = [];
                    if (DownTimeDetails) {
                        for (i = 0; i < DownTimeDetails.length; i++) {
                            categoryList.push(DownTimeDetails[i].DownID);
                            seriesData.push([DownTimeDetails[i].DownID, DownTimeDetails[i].Downtime]);
                        }
                    }
                    Highcharts.chart('downTimeChart', {
                        chart: {
                            renderTo: 'downTimeChart',
                            type: 'column'
                        },
                        credits: {
                            enabled: false
                        },
                        exporting: {
                            enabled: false
                        },
                        title: {
                            text: 'Top 10 Line Downtimes'
                        },
                        xAxis: {
                            type: 'category',
                            categories: categoryList,
                            crosshair: true,
                            labels: {
                                rotation: -45,
                                style: {
                                    fontSize: '13px',
                                    fontFamily: 'Verdana, sans-serif'
                                }
                            }
                        },
                        yAxis: [{
                            min: 0,
                            title: {
                                text: 'Time (In Minutes)'
                            }
                        }, {
                            minPadding: 0,
                            maxPadding: 0,
                            max: 100,
                            min: 0,
                            opposite: true,
                            labels: {
                                format: "{value}%"
                            }
                        }],
                        legend: {
                            enabled: false
                        },
                        tooltip: {
                            shared: true
                        },
                        series: [{
                            type: 'pareto',
                            name: 'Pareto',
                            yAxis: 1,
                            zIndex: 10,
                            baseSeries: 1,
                            tooltip: {
                                valueDecimals: 2,
                                valueSuffix: '%'
                            }
                        },
                        {
                            name: 'DownTime',
                            type: 'column',
                            zIndex: 2,
                            data: seriesData
                        }]
                    });
                }

                function PlotEfficiencyGraph(EfficiencyDetails) {
                    var seriesData = [];
                    var categoryList = [];
                    if (EfficiencyDetails) {
                        for (i = 0; i < EfficiencyDetails.length; i++) {
                            categoryList.push(EfficiencyDetails[i].EfficiencyID);
                            seriesData.push([EfficiencyDetails[i].EfficiencyID, EfficiencyDetails[i].Efficiency]);
                        }
                    }
                    Highcharts.chart('efficiencyChart', {
                        chart: {
                            renderTo: 'efficiencyChart',
                            type: 'bar'
                        },
                        credits: {
                            enabled: false
                        },
                        exporting: {
                            enabled: false
                        },
                        title: {
                            text: 'Efficiency Chart'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            type: 'category',
                            categories: categoryList,
                            crosshair: true,
                            labels: {
                                style: {
                                    fontSize: '13px',
                                    fontFamily: 'Verdana, sans-serif'
                                }
                            }
                        },
                        yAxis: {
                            min: 0,
                            max: 100,
                            title: {
                                text: 'Efficiency (In Percentage)',
                                align: 'high',
                                fontWeight: 'bold'
                            },
                            labels: {
                                overflow: 'justify'
                            }
                        },
                        tooltip: {
                            valueSuffix: ' %'
                        },
                        plotOptions: {
                            bar: {
                                colorByPoint: true,
                                dataLabels: {
                                    enabled: true
                                }
                            }
                        },
                        legend: {
                            enabled: false
                        },
                        series: [{
                            name: 'Efficiency',
                            data: seriesData,
                        }]
                    });
                }

                function SetMonthwiseAvgEffData(MonthwiseAvgEfficiency) {
                    if (MonthwiseAvgEfficiency) {
                        document.querySelector("#lblMonthlyTGT").textContent = MonthwiseAvgEfficiency.Target;
                        document.querySelector("#lblMonthlyACT").textContent = MonthwiseAvgEfficiency.Actual;
                        document.querySelector("#lblMonthlyThroughput").textContent = MonthwiseAvgEfficiency.Throughput;
                    }
                }

                function ApplyAndonItemDimensions() {
                    divH = Math.max.apply(null, $('.andonDataItem').map(function () {
                        return $(this).outerHeight(true);
                    }).get());
                    $(".dataItemContainer").height(divH - 30);
                    $(".itemContainer").height(divH - 58);
                    divW = Math.max.apply(null, $('.andonDataItem').map(function () {
                        return $(this).outerWidth(true);
                    }).get());
                    $(".andonDataItem").width(divW);
                }
            });
        </script>
    </form>
</body>
</html>
