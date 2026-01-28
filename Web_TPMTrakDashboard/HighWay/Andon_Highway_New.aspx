<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Andon_Highway_New.aspx.cs" Inherits="Web_TPMTrakDashboard.HighWay.Andon_Highway_New" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HIGHWAY PLANT ANDON SCREEN</title>
    <meta name="description" content="TPM Trak Analytics web application">
    <meta name="author" content="GeeksLabs">
    <meta name="keyword" content="Creative, Dashboard, Admin, Template, Theme, Bootstrap, Responsive, Retina, Minimal">
    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.min.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/Site.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/TextAnimation.css" rel="stylesheet" />
    <script src="../GEA/Andon_GEA/Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <style>
        .HeaderImage {
            flex: 1;
            float: left;
        }

        /*   .headerRight {
            color: black;
            font-weight: 600;
            font-size: 20px;
            margin: 0px;
            margin-right: 20px;
        }*/

        .addBorder {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }

        .removeBorder {
            border-radius: 0px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }

        #tableSettings > tbody > tr > td {
            padding: 2px;
            width: 20px;
        }

        #tblHeader {
            width: 99.5%;
            margin-top: 1%;
            margin-left: 0.5%;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
        }

            #tblHeader tr:not(:first-child) td {
                min-width: 23%;
                width: 23%;
            }

        .iconcss {
            border-radius: 50%;
            /*background-color: white;*/
            text-align: center !important;
            color: green;
            font-size: 20px;
        }

        .iconcsstd {
            border-radius: 50%;
            /*background-color: white;*/
            text-align: center !important;
            color: green;
            font-size: 20px;
            min-width: 35px;
            padding-right: 10px;
        }

        .innertbl {
            /*border: 3px solid gray;*/
            /*width: 90%;*/
            text-align: center;
            height: 100%;
        }

            .innertbl tr:not(:first-child) td:first-child {
                text-align: center;
                width: 70% !important;
            }

            .innertbl tr:not(:first-child) td:not(:first-child) {
                text-align: center;
                width: 10% !important;
            }

        .btnSave {
            font-weight: 600;
            background-color: #3777bc;
            color: white;
            width: 60px;
            height: 35px;
            border: 0px;
        }

        .legendStyleSetting {
            display: block;
            padding: 4px 10px;
            width: 30%;
            font-size: 18px;
            margin-left: 25px;
            color: #333;
            border: 0px !important;
            font-weight: bold;
        }


        #lblfieldset {
            border: 1px solid black;
            height: 125px;
            align-content: center;
        }

        .OuterDivContainerStyle {
            display: flex;
            align-content: baseline;
            justify-content: center;
            flex-wrap: wrap;
        }
    </style>
</head>
<body style="background-color: #e9f8ee;">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdnHeight" ClientIDMode="Static" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="" id="ComputerDiv" runat="server">
                    <div class="modal-dialog">
                        <fieldset style="border-color: black" id="lblfieldset">
                            <legend class="legendStyleSetting">Device Name</legend>
                            <table id="tblComputer" style="width: 95%; margin-top: 10px;">

                                <tr>
                                    <td><span style="font-size: medium; font-weight: 600;">Device Name</span></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtComputerName" CssClass="form-control focus"></asp:TextBox>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Button runat="server" ID="BtnSave" Text="Save" CssClass="btnSave" OnClick="BtnSave_Click" />
                                    </td>
                                </tr>
                            </table>

                        </fieldset>
                    </div>
                </div>
                <div id="content" runat="server">
                    <div class="row">
                        <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 5px; background-color: #106884" id="headerDiv">
                            <div class="HeaderImage" style="height: 60px">
                                <img runat="server" src="~/Images/logo/AMITLogo.png" id="toggle" class="img-responsive img-rounded" alt="Logo" style="cursor: pointer; padding-right: 1px; margin-top: 4px; height: 52px;" />
                                <%--<asp:Image ID="customerLogo" runat="server" class="img-responsive img-rounded" Style="width: 200px; height: 56px; margin-top: 2px" />--%>
                            </div>
                            <%--  <label id="headerName" runat="server" clientidmode="static" style="color: white; font-weight: bold; font-size: 35px; text-align: right; margin-top: 5px; vertical-align: middle">HIGHWAY PLANT ANDON SCREEN</label>--%>
                            <label id="headerName" runat="server" clientidmode="static" style="color: white; font-weight: bold; font-size: 35px; text-align: right; margin-top: 5px; vertical-align: middle">JL-SHAFT LIVE ANDON SCREEN</label>
                            <div style="float: right; position: relative; display: inline-flex">
                                <div style="padding-right: 15px;">
                                    <table>
                                        <tr>
                                            <td runat="server" id="tdAndonSettings" onclick="location.href='AndonSettings_Highway.aspx';">
                                                <i class="glyphicon glyphicon-cog" style="font-size: 20px; color: white"></i>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="font-size: 16px; cursor: pointer; color: white; vertical-align: text-top" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="height: 60px; float: right;">
                                    <asp:Image ID="Image1" runat="server" class="img-responsive img-rounded" ImageUrl="Logo/HighwayLogo.jpg" Style="width: 200px; height: 56px; margin-top: 2px" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <div id="userControlContainer" runat="server">
                        <div id="textdiv">
                            <div class="contaner-fluid" style="border-bottom: 3px solid gray;">
                                <div class="row" id="tblHeader">
                                    <div class="col-lg-4" style="font-weight: bold; text-align: left; font-size: <%= settings.MainHeaderFontSize%>;">
                                        <asp:Label runat="server" ClientIDMode="Static" ID="headerText" Text="Overall Statistics"></asp:Label>
                                    </div>
                                    <div class="col-lg-8" style="font-weight: bold; text-align: right; font-size: <%= settings.MainHeaderFontSize%>;">
                                        <label class="headerRight" runat="server" id="lblDateTime" clientidmode="static" style="display: inline-block"></label>
                                        &nbsp;&nbsp;&nbsp;
                                     <asp:Label runat="server" ClientIDMode="Static" ID="lblShift" CssClass="headerRight"></asp:Label>
                                    </div>
                                </div>
                                <div class="row" style="margin: 0px; font-weight: bold;">
                                    <div class="col-lg-3 contentDiv" style="padding: 10px">
                                        <div style="border: 3px solid gray; border-radius: 20px; height: 100%;">
                                            <table class="innertbl" style="width: 100%;">
                                                <tr class="trheight">
                                                    <td style="font-size: <%=settings.EfficiencyFontSize%>;" colspan="2">
                                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblPlantUtilization"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: <%=settings.EfficiencyHeaderFontSize%>;">Line OEE</td>
                                                    <td class="iconcsstd"><span class="iconcss"><i class="glyphicon glyphicon-signal"></i></span></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 contentDiv" style="padding: 10px">
                                        <div style="border: 3px solid gray; border-radius: 20px; height: 100%;">
                                            <table class="innertbl" style="width: 100%;">
                                                <tr class="trheight">
                                                    <td style="font-size: <%=settings.EfficiencyFontSize%>;" colspan="2">
                                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblMachineUtilization"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: <%=settings.EfficiencyHeaderFontSize%>;">Machine<br />
                                                        Availability</td>
                                                    <td class="iconcsstd"><span class="iconcss"><i class="glyphicon glyphicon-th-list"></i></span></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 contentDiv" style="padding: 10px">
                                        <div style="border: 3px solid gray; border-radius: 20px; height: 100%;">
                                            <table class="innertbl" style="width: 100%;">
                                                <tr class="trheight">
                                                    <td style="font-size: <%=settings.EfficiencyFontSize%>;" colspan="2">
                                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblOperatorEfficiency"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: <%=settings.EfficiencyHeaderFontSize%>;">Performance<br />
                                                        Efficiency</td>
                                                    <td class="iconcsstd"><span class="iconcss"><i class="glyphicon glyphicon-user"></i></span></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 contentDiv" style="padding: 10px">
                                        <div style="border: 3px solid gray; border-radius: 20px; height: 100%;">
                                            <table class="innertbl" style="width: 100%;">
                                                <tr class="trheight">
                                                    <td style="font-size: <%=settings.EfficiencyFontSize%>;" colspan="2">
                                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblIdleTime"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: <%=settings.EfficiencyHeaderFontSize%>;">Total
                                                <br />
                                                        Down Time</td>
                                                    <td class="iconcsstd"><span class="iconcss"><i class="glyphicon glyphicon-time"></i></span></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="graphdiv" style="margin-top: 1%; background-color: white; text-align: center; vertical-align: middle;">
                            <div class="container-fluid" style="height: 97%;">
                                <div class="row" style="height: 100%; margin: 0px;">
                                    <div class="col-lg-8" style="height: 100%; padding: 10px;">
                                        <div id="areachartdiv" style="height: 100%; margin-top: 0%; border: 3px solid gray; border-radius: 25px;"></div>
                                    </div>
                                    <div class="col-lg-4" style="height: 100%; padding: 10px;">
                                        <div id="pieChartdiv" style="height: 100%; border: 3px solid gray; border-radius: 25px;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <script>
            var dataRefreshTimer;
            $(document).ready(function () {
                if (!($('#ComputerDiv').is(':visible'))) {
                    setDataRefreshTimer();
                }
                else {
                    $('#userControlContainer').css("display", "none");
                }
                function updateDateTime() {
                    var currentDate = new Date();
                    var formattedDateTime ="Date : "+ currentDate.getDate().toString().padStart(2, '0') + '-' +
                        (currentDate.getMonth() + 1).toString().padStart(2, '0') + '-' +
                        currentDate.getFullYear() + ' ' +"Time : "+
                        currentDate.getHours().toString().padStart(2, '0') + ':' +
                        currentDate.getMinutes().toString().padStart(2, '0') + ':' +
                        currentDate.getSeconds().toString().padStart(2, '0');
                    $('#lblDateTime').text(formattedDateTime);
                }
                // Call updateDateTime every second using setInterval
                setInterval(updateDateTime, 1000);
                GetChartData();
                //BindCharts();
            });
            function GetSettings() {

            }
            function setGraphHeight() {
                debugger;
                var divH = Math.max.apply(null, $('.contentDiv').map(function () {
                    return $(this).outerHeight(true);
                }).get());
                var heigh = Math.max.apply(null, $('.trheight').map(function () {
                    return $(this).outerHeight(true);
                }).get());
                if ($("#hdnHeight").val() == "" || $("#hdnHeight").val() == undefined) {
                    var divH = Math.max.apply(null, $('.contentDiv').map(function () {
                        return $(this).outerHeight(true);
                    }).get());
                    $("#hdnHeight").val(divH);
                }
                $(".trheight").height(heigh);
                $(".contentDiv").height($("#hdnHeight").val());
                var height = $(window).height() - $('#textdiv').height() - 130;
                $('#graphdiv').css("height", height);
            }
            function setDataRefreshTimer() {
                clearTimeout(dataRefreshTimer);
                dataRefreshTimer = setTimeout(GetChartData,<%= settings.DatarefreshInterval %>);
            }

            $("[id$=btnFullScreen]").click(function () {
                debugger;
                if ((document.fullScreenElement && document.fullScreenElement !== null) ||
                    (!document.mozFullScreen && !document.webkitIsFullScreen)) {
                    if (document.documentElement.requestFullScreen) {
                        document.documentElement.requestFullScreen();
                    } else if (document.documentElement.msRequestFullscreen) {
                        document.documentElement.msRequestFullscreen();
                    } else if (document.documentElement.mozRequestFullScreen) {
                        document.documentElement.mozRequestFullScreen();
                    } else if (document.documentElement.webkitRequestFullScreen) {
                        document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
                    }
                } else {
                    if (document.cancelFullScreen) {
                        document.cancelFullScreen();
                    } else if (document.msRequestFullscreen) {
                        document.msRequestFullscreen();
                    } else if (document.mozCancelFullScreen) {
                        document.mozCancelFullScreen();
                    } else if (document.webkitCancelFullScreen) {
                        document.webkitCancelFullScreen();
                    }
                }
                setGraphHeight();
            });

            function GetChartData() {
                $.ajax({
                    async: false,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Andon_Highway_New.aspx/GetSummeryPieData",
                    dataType: "json",
                    success: BindPieChart,
                    error: function (Result) {
                        setDataRefreshTimer();
                        alert("Error");
                    }
                });
                $.ajax({
                    async: false,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Andon_Highway_New.aspx/GetHourwiseEnergy",
                    dataType: "json",
                    success: BindAreaChart,
                    error: function (Result) {
                        setDataRefreshTimer();
                        alert("Error");
                    }
                });
                setDataRefreshTimer();
            }
            function BindAreaChart(result) {
                debugger;
                var Data = result.d
                Highcharts.chart('areachartdiv', {
                    title: {
                        //text: 'JL Shaft Hourly Statistics',
                        text: 'JL Shaft Daily Statistics',
                        align: 'left',
                        style: {
                            fontSize: '20px',
                            fontWeight: 'bold'
                        },
                        x: 30
                    },
                    credits: {
                        enabled: false
                    },
                    yAxis: {
                        title: {
                            text: 'Percentage (%)',
                            style: {
                                fontSize: '20px',
                                fontWeight: 'bold',
                                color: 'black'
                            }
                        },
                        labels: {
                            style: {
                                fontSize: '25px',
                                fontWeight: 'bold',
                                color: 'black'
                            }
                        }
                    },
                    xAxis: {
                        title: {
                            //text: 'Time (hh:mm)',
                            text: '',
                            style: {
                                fontSize: '20px',
                                fontWeight: 'bold',
                                color: 'black'
                            }
                        },
                        categories: Data[0].OperatorData.Time,
                        labels: {
                            rotation: 295,// Rotate the labels by 90 degrees
                            style: {
                                fontSize: '25px',
                                fontWeight: 'bold',
                                color: 'black'
                            }
                            //formatter: function () {
                            //    // Only display labels at even indices
                            //    if (this.pos % 2 === 0) {
                            //        return this.value;
                            //    } else {
                            //        return '';
                            //    }
                            //}
                        }
                    },

                    //legend: {
                    //    layout: 'vertical',
                    //    align: 'right',
                    //    verticalAlign: 'top'
                    //},
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    },
                    plotOptions: {
                        line: {
                            dataLabels: {
                                enabled: false,
                                useHTML: true,
                                format: '<span class="hc-label" style="z-index:10">{y}</span>'
                            },
                            enableMouseTracking: false,
                            animation: false // Disable animation
                        },
                        series: {
                            label: {
                                connectorAllowed: false
                            },
                            marker: {
                                enabled: true, // Enable markers
                                radius: 3 // Adjust the size of the markers
                            },
                            style: {
                                fontSize: '13px'
                            }
                        }
                    },
                    series: [{
                        name: 'Machine Availability',
                        data: Data[0].OperatorData.Value,
                        color: 'orange'
                    }, {
                        name: 'Performance Efficiency',
                        data: Data[0].PlantData.Value,
                        color: 'blue'
                    }, {
                        name: 'Line OEE',
                        data: Data[0].MachineData.Value,
                        color: 'green'
                    }],

                    //responsive: {
                    //    rules: [{
                    //        condition: {
                    //            maxWidth: 500
                    //        },
                    //        chartOptions: {
                    //            legend: {
                    //                layout: 'horizontal',
                    //                align: 'center',
                    //                verticalAlign: 'bottom'
                    //            }
                    //        }
                    //    }]
                    //}
                });
            }
            function BindPieChart(data) {
                $("#lblPlantUtilization").text(data.d.headerdata.PlantUtilization);
                $("#lblMachineUtilization").text(data.d.headerdata.MachineUtilization);
                $("#lblOperatorEfficiency").text(data.d.headerdata.OperatorEfficiency);
                $("#lblIdleTime").text(data.d.headerdata.IdleTime);
                $("#lblDateTime").text(data.d.headerdata.Date);
                $("#lblShift").text(data.d.headerdata.Shift);
                setGraphHeight();
                Highcharts.chart('pieChartdiv', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    credits: {
                        enabled: false
                    },
                    title: {
                        text: 'Top Down Time',
                        align: 'left',
                        style: {
                            fontSize: '20px',
                            fontWeight: 'bold'
                        },
                        x:30
                    },
                    tooltip: {
                        formatter: function () {
                            return "<b>" + this.point.name + "</b><br><b>" + this.series.name + ': </b>' + this.y + '%';
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            borderWidth: 0,
                            colorByPoint: true,
                            type: 'pie',
                            size: '100%',
                            innerSize: '60%',
                            dataLabels: {
                                enabled: true,
                                crop: false,
                                /* distance: '20%',*/
                                distance: '-10%',
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '16px'
                                },
                                connectorWidth: 0,
                                formatter: function () {
                                    return this.y + "%";
                                }
                            },
                            showInLegend: true,
                            animation: false
                        }
                    },
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom', // Set the legend to the bottom
                        layout: 'horizontal', // Display the legend items in a horizontal layout
                        itemStyle: {
                            fontWeight: 'bold',
                            fontSize: '14px'
                        }
                    },
                    series: [{
                        name: data.d.name,
                        colorByPoint: true,
                        data: data.d.data
                    }]
                });
                /*setGraphHeight();*/
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                $(document).ready(function () {
                    setGraphHeight();
                });
                $("[id$=btnFullScreen]").click(function () {
                    debugger;
                    if ((document.fullScreenElement && document.fullScreenElement !== null) ||
                        (!document.mozFullScreen && !document.webkitIsFullScreen)) {
                        if (document.documentElement.requestFullScreen) {
                            document.documentElement.requestFullScreen();
                        } else if (document.documentElement.msRequestFullscreen) {
                            document.documentElement.msRequestFullscreen();
                        } else if (document.documentElement.mozRequestFullScreen) {
                            document.documentElement.mozRequestFullScreen();
                        } else if (document.documentElement.webkitRequestFullScreen) {
                            document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
                        }
                    } else {
                        if (document.cancelFullScreen) {
                            document.cancelFullScreen();
                        } else if (document.msRequestFullscreen) {
                            document.msRequestFullscreen();
                        } else if (document.mozCancelFullScreen) {
                            document.mozCancelFullScreen();
                        } else if (document.webkitCancelFullScreen) {
                            document.webkitCancelFullScreen();
                        }
                    }
                });
            });
        </script>
    </form>
</body>
</html>
