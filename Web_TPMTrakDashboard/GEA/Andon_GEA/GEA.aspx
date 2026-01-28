<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GEA.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.Andon_GEA.GEA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Production Andon</title>
    <script src="/AndonScripts/jquery-3.1.0.min.js"></script>
    <script src="/AndonScripts/bootstrap.min.js"></script>
    <link href="/AndonContent/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/Site.css" rel="stylesheet" />
    <link href="Content/TextAnimation.css" rel="stylesheet" />
    <script src="Scripts/Highchart8/highcharts.js"></script>
    <script src="Scripts/Highchart8/exporting.js"></script>
    <script src="Scripts/Highchart8/export-data.js"></script>
    <script src="Scripts/Highchart8/accessibility.js"></script>
    <script src="Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <style>
        .HeaderImage {
            flex: 1;
            float: left;
        }

        .headerRight {
            color: white;
            font-weight: 600;
            font-size: 16px;
            margin: 1px;
        }

        #poContainer {
            height: 85vh;
            display: grid;
            grid-template-columns: 2fr 1fr;
            grid-gap: 5px;
        }

        #div1 {
            grid-column: 1 /2;
            grid-row: 1/ 5;
            overflow-y: hidden;
            overflow-x: auto;
        }

        #div2 {
            grid-column: 2/ 4;
            grid-row: 1/3;
            overflow: auto;
            background-color: white;
        }

        #div3 {
            grid-column: 2/ 4;
            grid-row: 3/5;
            overflow: auto;
            background-color: white;
        }

        #bottomDiv {
            background-color: #3777bc;
            text-align: center;
            padding: 3px;
        }

        #lvPODataTbl {
            width: 100%;
            background-color: white;
        }

            #lvPODataTbl tr th {
                color: darkblue;
                text-align: center;
                font-weight: 600;
                font-size: 20px;
                border-bottom: 1px solid silver;
                padding: 5px;
            }

            #lvPODataTbl tr td {
                padding: 5px;
                color: black;
                text-align: center;
                font-weight: 600;
                /*font-size: 16px;*/
                border-bottom: 1px solid silver;
            }

        .machineList {
            width: 100%;
            margin-top: 4px;
        }

            .machineList tr th, .machineList tr td:first-child {
                color: darkblue;
                font-size: 17px;
                font-weight: 600;
                padding: 8px;
                border: 1px solid #a4aab0;
            }

            .machineList tr:first-child td {
                color: darkblue;
                height: 37px;
                border: 1px solid #a4aab0;
            }

                .machineList tr:first-child td:first-child {
                    border: none;
                }

        .tdGreenColor {
            color: #004000;
            font-size: 17px;
            font-weight: 600;
            padding: 8px;
            border: 2px solid #a4aab0;
        }

        .tdRedColor {
            color: #ff0000;
            font-size: 17px;
            font-weight: 600;
            padding: 8px;
            border: 2px solid #a4aab0;
        }

        .imgInHeader {
            width: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdnChartsCount" ClientIDMode="Static" />
        <div class=" container-fluid">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row text-center">
                        <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 5px; background-color: #3777bc">
                            <div class="HeaderImage">
                                <img src="Images/SPFLogo.PNG" height="60" style="padding: 3px;" />
                            </div>
                            <%--      <asp:UpdatePanel runat="server" style="display:inline-block">
                        <ContentTemplate>--%>
                            <label id="headerName" runat="server" clientidmode="static" style="color: white; font-weight: bold; font-size: 33px; text-align: right; margin-top: 5px">Production Status</label>
                            <%--                        </ContentTemplate>
                        <Triggers>
                               <asp:AsyncPostBackTrigger ControlID="poInterval" EventName="Tick" />
                              <asp:AsyncPostBackTrigger ControlID="ddlPlantID" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnSwitch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>--%>


                            <div style="float: right; position: relative; display: inline-flex">
                               <%-- <div style="margin-right: 10px">
                                    <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" Style="margin-top: 2px">
                                    </asp:DropDownList>

                                </div>--%>
                                <div style="text-align: left">
                                    <p class="headerRight"><%: DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")%>&nbsp;&nbsp;</p>
                                    <%--   <asp:Button runat="server" ID="btnSwitch" ClientIDMode="Static"  Text="Switch to ANDON Mode" OnClick="btnSwitch_Click" style="margin:3px;border-radius:6px;outline:none;color:black" OnClientClick="return blockUI();"/>--%>
                                    <%--  <p class="headerRight"><span style="font-size: 20px; margin-top: 25px; cursor: pointer; color: white;" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span>&nbsp;&nbsp;&nbsp;</p>--%>
                                    <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" Style="margin-top: 0px">
                                    </asp:DropDownList>
                                </div>&nbsp;&nbsp;
                                <div>
                                    <p style="margin: 0px" onclick="location.href='AndonSetting.aspx';">
                                        <img src="Images/List-Icon.jpg" height="29" />
                                    </p>
                                    <p class="headerRight"><span style="font-size: 18px; cursor: pointer; color: white;vertical-align:text-top" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span></p>
                                    <%--  <p style="margin: 0px">
                                <img src="Images/Power1.jpg" height="29" />
                            </p>--%>
                                </div>
                            </div>
                            &nbsp;&nbsp;
                        </div>
                    </div>

                    <asp:Timer runat="server" ID="poInterval" OnTick="poInterval_Tick"></asp:Timer>

                    <div class="row" style="margin-top: 110px">
                        <div class="col-lg-12 col-sm-12 col-md-12" style="padding: 0px;">
                            <div id="poContainer" runat="server" clientidmode="static" style="height: 85vh">
                                <div id="div1">
                                    <asp:ListView runat="server" ID="lvPOData">
                                        <LayoutTemplate>
                                            <table id="lvPODataTbl" runat="server" clientidmode="static">
                                                <tr>
                                                    <th id="thMachine" runat="server">Machine</th>
                                                    <th id="thStatus" runat="server">Satus</th>
                                                    <th id="thComponent" runat="server">
                                                        <img src="Images/Printer1.png" class="imgInHeader" runat="server" /></th>
                                                    <th id="thSetting" runat="server">
                                                        <img src="Images/Settings-icon.png" class="imgInHeader" runat="server" /></th>
                                                    <th id="thTimer" runat="server">
                                                        <img src="Images/Alarm-clock.png" class="imgInHeader" runat="server" /></th>
                                                    <th id="thUser" runat="server">
                                                        <img src="Images/UserL.png" class="imgInHeader" runat="server" /></th>
                                                    <th id="thAvailabilityEfficiency" runat="server">AE%</th>
                                                    <th id="thProductionEfficiency" runat="server">PE%</th>
                                                    <th id="thOverAllEfficiency" runat="server">OOE%</th>
                                                    <th id="thPlan" runat="server">Plan</th>
                                                    <th id="thAct" runat="server">Act.</th>
                                                    <th id="thEmoji" runat="server">Emoji</th>
                                                </tr>
                                                <tr id="itemplaceholder" runat="server"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td id="tdMachine" runat="server"><%# Eval("Machine") %></td>
                                                <td id="tdStatus" runat="server">
                                                    <img src='<%# Eval("Status") %>' width="35" /></td>
                                                <td id="tdComponent" runat="server">
                                                    <img src='<%# Eval("Component") %>' width="35" /></td>
                                                <td id="tdSetting" runat="server">
                                                    <img src='<%# Eval("Setting") %>' width="35" /></td>
                                                <td id="tdTimer" runat="server">
                                                    <img src='<%# Eval("Alaram") %>' width="35" /></td>
                                                <td id="tdUser" runat="server">
                                                    <img src='<%# Eval("User") %>' width="35" /></td>
                                                <td id="tdAvailabilityEfficiency" runat="server" style='background-color: <%# Eval("AEBackColor") %>'><%# Eval("AE") %></td>
                                                <td id="tdProductionEfficiency" runat="server" style='background-color: <%# Eval("PEBackColor") %>'><%# Eval("PE") %></td>
                                                <td id="tdOverAllEfficiency" runat="server" style='background-color: <%# Eval("OEEBackColor") %>'><%# Eval("OEE") %></td>
                                                <td id="tdPlan" runat="server"><%# Eval("Plan") %></td>
                                                <td id="tdAct" runat="server"><%# Eval("Act") %></td>
                                                <td id="tdEmoji" runat="server">
                                                    <img src='<%# Eval("Emoji") %>' width="35" /></td>

                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                                <div id="div2">
                                    <div id="leadingMachineContainer" runat="server">
                                        <div style="background-color: #90ee90; padding: 10px; border-radius: 6px; text-align: center"><span style="font-size: 20px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Leading Machines</span></div>
                                        <div>
                                            <asp:ListView runat="server" ID="lvleadingMachine">
                                                <LayoutTemplate>
                                                    <table id="lvleadingMachineTbl" class="machineList">
                                                        <tr runat="server" id="itemplaceholder"></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="width: 170px"><%# Eval("Col1") %></td>
                                                        <td class="tdGreenColor"><%# Eval("Col2") %></td>
                                                        <td class="tdGreenColor"><%# Eval("Col3") %></td>
                                                        <td class="tdGreenColor"><%# Eval("Col4") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                    <div id="downTimeContainer" runat="server">
                                        <div style="background-color: #90ee90; padding: 5px; border-radius: 6px; text-align: center"><span style="font-size: 20px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Down Time Chart</span></div>
                                        <div id="DownTimeChartContainer">
                                        </div>
                                    </div>
                                </div>
                                <div id="div3">
                                    <div id="laggingMachineContainer" runat="server">
                                        <div style="background-color: #ffc0c0; padding: 10px; border-radius: 6px; text-align: center"><span style="font-size: 20px; font-weight: 800; color: #ff0000">Lagging Machines</span></div>
                                        <div>
                                            <asp:ListView runat="server" ID="lvLaggingMachine">
                                                <LayoutTemplate>
                                                    <table id="lvLaggingMachineTbl" class="machineList">
                                                        <tr id="itemplaceholder" runat="server"></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="width: 170px"><%# Eval("Col1") %></td>
                                                        <td class="tdRedColor"><%# Eval("Col2") %></td>
                                                        <td class="tdRedColor"><%# Eval("Col3") %></td>
                                                        <td class="tdRedColor"><%# Eval("Col4") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                    <div id="oeeConatiner" runat="server">
                                        <div style="background-color: #ffc0c0; padding: 5px; border-radius: 6px; text-align: center"><span style="font-size: 20px; font-weight: 800; color: #ff0000">This Month OEE</span></div>
                                        <div id="OEEChartContainer">
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-top: 5px" id="imageVideoConatiner" runat="server" clientidmode="static">

                                <div id="myCarousel" class="carousel slide" data-ride="carousel" data-interval="5000">
                                    <asp:Literal ID="ltlCarouselIndicators" runat="server" />
                                    <!-- Images-->
                                    <div class="carousel-inner" role="listbox">
                                        <asp:Literal ID="ltlCarouselImages" runat="server" />
                                    </div>
                                    <!-- Left Right Arrows -->
                                    <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
                                        <span class="glyphicon glyphicon-chevron-left" aria-hidden="true" style="visibility: hidden"></span>
                                        <span class="sr-only">Previous</span>
                                    </a>
                                    <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
                                        <span class="glyphicon glyphicon-chevron-right" aria-hidden="true" style="visibility: hidden"></span>
                                        <span class="sr-only">Next</span>
                                    </a>
                                </div>
                                <div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <asp:Button runat="server" ID="btnPost" ClientIDMode="Static" OnClick="btnPost_Click" Style="visibility: hidden" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="poInterval" EventName="Tick" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPlantID" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="imgBtnSwitch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <footer>
                <div class="navbar navbar-default navbar-fixed-bottom footerBottom" style="padding: 0px 5px; background-color: #3777bc; min-height: 40px; text-align: center">
                    <div style="float: left;">
                         <p style="color: #fcefef; font-style: italic; margin-top: 10px; font-size: 16px;display:inline-block">Powered by TPM-Trak®</p>
                        <asp:UpdatePanel runat="server" style="display:inline-block">
                            <ContentTemplate>
                               &nbsp;&nbsp;&nbsp;
                               <asp:ImageButton runat="server" ID="imgBtnSwitch" ImageUrl="Images/andon.jpg" OnClick="imgBtnSwitch_Click"  ToolTip="Switch to ANDON Mode" Style="width:32px;height:27px;position:relative;top:7px" OnClientClick="return blockUI();" />
                             <%--   <asp:Button runat="server" ID="btnSwitch" ClientIDMode="Static" Text="Switch to ANDON Mode" OnClick="btnSwitch_Click" Style="margin: 3px; border-radius: 6px; outline: none; color: black" OnClientClick="return blockUI();" />--%>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="poInterval" EventName="Tick" />
                                <asp:AsyncPostBackTrigger ControlID="ddlPlantID" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="imgBtnSwitch" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <%--<span style="color: white; font-weight: 600; font-size: 20px; text-align: center;" runat="server" id="spanWelcome"></span>--%>
                    <div style="display: inline-block; margin-top: 4px; width: 82%">
                        <marquee style="font-family: Book Antiqua; color: #FFFFFF; font-size: 25px; background-color: #0f4987" scrollamount="10" loop="infinite" runat="server" id="spanWelcome"></marquee>
                    </div>
                    <div style="float: right;">
                        <img src="Images/AMiT.jpg" height="40" width="70" />
                    </div>
                </div>
            </footer>
        </div>

        <script>

            var countItem = 0;
            $('.carousel').carousel({
                pause: "false"
            });
            $(document).ready(function () {

                var h = $(window).height() - 110;
                // $('#poContainer').css('height', h);
                setHeaderContentSize();
                // setPORowsBasedOnHeight();  // called after setHeaderContentSize()
                $('#hdnChartsCount').val("firstentry");
                var screenH = $(window).height() - 130;//screen.availHeight - 100;
                var screenW = $(window).width() - 30;//screen.availWidth - 5;
                $(".makeStyle").css("height", screenH);//.height = screenH;
                $(".makeStyle").css("width", "auto");//.width = screenW;
                var itemsCount = 0;
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "GEA.aspx/getNumberOfImagesVideo",
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (response) {
                        var itmdata = response.d;
                        itemsCount = itmdata;
                    },
                    error: function (jqXHR, textStatus, err) {
                    }
                });
                if (document.getElementById('imageVideoConatiner') != null) {                   
                    console.log("outside" + countItem + " , " + itemsCount);
                    $("#myCarousel").on('slid.bs.carousel', function () {
                        countItem++;
                        console.log("Inside" + countItem + " , " + itemsCount);
                        if (countItem == itemsCount + 1) {
                            console.log("eual" + countItem + " , " + itemsCount);
                            $('.carousel').carousel({
                                pause: "false"
                            });
                            window.location.href = "GEA.aspx";
                            return;
                        }
                      
                        var video = $("#myCarousel .item.active").children("video");
                        var isVideo = (video.length > 0);
                        if (isVideo) {

                            $("#myCarousel").carousel("pause");

                            video.get(0).play();
                        }
                        if (countItem == $("#myCarousel .item").length) {
                            window.location.href = "GEA.aspx";
                        }

                    });
                    //if ($("#myCarousel .item").length == 1) {
                    //    setTimeout(function () {
                    //        window.location.href = "GEA.aspx";
                    //    }, 5000);
                    //}
                }
            });
            $("video").each(function () {
                this.addEventListener('ended', myHandler, false);
                this.addEventListener('error', myHandler, false);
            });

            function myHandler(e) {
                // What you want to do after the event
                $("#myCarousel").carousel("cycle");
            }
            function blockUI() {
                $.blockUI({ message: '<img src="Images/LoadIcon/ajax-loader.gif" />' });
                return true;
            }
            $('#ddlPlantID').change(function () {
                $.blockUI({ message: '<img src="Images/LoadIcon/ajax-loader.gif" />' });
                return true;
            });
            function setscreenName() {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "GEA.aspx/setScreenName",
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (response) {
                        var itmdata = response.d;
                    },
                    error: function (jqXHR, textStatus, err) {
                    }
                });
            }

            $("[id$=btnFullScreen]").click(function () {
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
            $(window).resize(function () {

                var h = $(window).height() - 110;
                //$('#poContainer').css('height', h);
                var screenH = $(window).height() - 130;//screen.availHeight - 100;
                var screenW = $(window).width() - 30;//screen.availWidth - 5;
                $(".makeStyle").css("height", screenH);//.height = screenH;
                $(".makeStyle").css("width", "auto");//.width = screenW;
                console.log("Reisze h " + screenH);
            });
            
            function setHeaderContentSize() {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "GEA.aspx/getFontSize",
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (response) {
                        var itmdata = response.d;
                        $('#lvPODataTbl tr th').css('font-size', parseInt(itmdata[0]));
                        $('#lvPODataTbl tr td').css('font-size', parseInt(itmdata[1]));

                    },
                    error: function (jqXHR, textStatus, err) {
                    }
                });
            }
            function setPORowsBasedOnHeight() {
                debugger;
                setHeaderContentSize();
                var POContainerHeight = $('#poContainer').height();
                var headerHeight = $('#lvPODataTbl tr:first-child').height();
                POContainerHeight = POContainerHeight - headerHeight;
                var contentHeight = $('#lvPODataTbl tr:nth-child(2)').height();
                var fittedRows = Math.floor(POContainerHeight / contentHeight);
                var rows = $('#lvPODataTbl tr:not(:first-child)');
                var rowCount = rows.length;
                for (var i = (rows.length - 1); i >= fittedRows; i--) {
                    rows[i].remove();
                }//'<%= ResolveUrl("GEA.aspx/getDownTimeChartData") %>',
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%=ResolveUrl("GEA.aspx/setNoOfRowsToSession")%>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: '{rows:' + fittedRows + ',rowsLength:' + rowCount + '}',
                    success: function (response) {
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
            }
            function bindCharts() {
                setHeaderContentSize();
                BindOEEChart();
                BindDownTimeChart();
            }
            function BindDownTimeChart() {

                var downTimeData;
                //var plantId = $('#ddlPlantID').val() == "All" ? "" : $('#ddlPlantID').val();
                 var plantId =$('#ddlPlantID').val();
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("GEA.aspx/getDownTimeChartData") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: '{plant:"' + plantId + '"}',
                    success: function (response) {

                        var dataitem = response.d;
                        downTimeData = dataitem;
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
                console.log("DownTime Series =" + downTimeData.series);
                Highcharts.chart('DownTimeChartContainer', {
                    chart: {
                        height: '55%'
                    },
                    title: {
                        text: 'Down Time Report',
                        style: {
                            color: 'black',
                            fontSize: '20px',
                            fontWeight: '600'
                        }
                    },
                    exporting: { enabled: false },
                    xAxis: {
                        categories: downTimeData.Category,
                        title: {
                            text: "Down Time ID",
                            style: {
                                color: 'black',
                                fontSize: '17px',
                                fontWeight: '600'
                            }
                        },
                        labels: {
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: '600'
                            }
                        },
                        gridLineWidth: 1,

                    },
                    yAxis: [{ //--- Primary yAxis
                        gridLineWidth: 0,
                        title: {
                            text: 'Down Time',
                            style: {
                                color: '#1ba1e2',
                                fontSize: '17px',
                                fontWeight: '600'
                            }
                        },
                        labels: {
                            overflow: 'justify',
                            style: {
                                color: 'black',
                                fontSize: '15px',
                                fontWeight: '600'
                            }
                        }
                    }, { //--- Secondary yAxis
                        gridLineWidth: 0,
                        title: {
                            text: 'Percentage',
                            style: {
                                color: '#86ba35',
                                fontSize: '17px',
                                fontWeight: '600'
                            }
                        },
                        labels: {
                            overflow: 'justify',
                            style: {
                                color: 'black',
                                fontSize: '15px',
                                fontWeight: '600'
                            }
                        },
                        opposite: true,
                    },
                    ],
                    legend: {
                        enabled: false
                    },
                    credits: {
                        enabled: false
                    },
                    series: downTimeData.series,
                });
            }
            function BindOEEChart() {
                var XCategoty = [];
                var Value = [];
                var OEEData = [];
                //var plantId = $('#ddlPlantID').val() == "All" ? "" : $('#ddlPlantID').val();
                var plantId = $('#ddlPlantID').val();
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("GEA.aspx/getOEEChartData") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: '{plant:"' + plantId + '"}',
                    success: function (response) {
                        var dataitem = response.d;
                        OEEData = dataitem;
                        for (var i = 0; i < OEEData.length; i++) {
                            XCategoty[i] = OEEData[i].Category;
                            Value[i] = OEEData[i].Value;
                        }
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });

                console.log("Cat =" + OEEData.Category);
                Highcharts.chart('OEEChartContainer', {
                    chart: {
                        type: 'bar',
                        height: '55%'
                    },
                    title: {
                        text: 'This Month Plant OEE',
                        style: {
                            color: 'black',
                            fontSize: '20px',
                            fontWeight: '600'
                        }
                    },
                    exporting: { enabled: false },
                    xAxis: {
                        categories: XCategoty,
                        title: {
                            text: "Plant ID",
                            style: {
                                color: 'black',
                                fontSize: '17px',
                                fontWeight: '600'
                            }
                        },
                        labels: {
                            style: {
                                color: 'black',
                                fontSize: '15px',
                                fontWeight: '600'
                            }
                        },
                        gridLineWidth: 1,

                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Overall Efficiency %',
                            style: {
                                color: 'black',
                                fontSize: '17px',
                                fontWeight: '600'
                            }
                        },
                        labels: {
                            overflow: 'justify',
                            style: {
                                color: 'black',
                                fontSize: '15px',
                                fontWeight: '600'
                            }
                        }
                    },
                    tooltip: {
                        //valueSuffix: ' millions'
                    },
                    plotOptions: {
                        bar: {
                            dataLabels: {
                                enabled: true,
                                inside: true,
                                style: {
                                    color: 'White',
                                    fontSize: '14px'
                                }
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        //name: 'Year 1800',
                        data: Value,
                    }]
                });
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                var countItem = 0;
                $('.carousel').carousel({
                    pause: "false"
                });
                var screenH = $(window).height() - 130;//screen.availHeight - 100;
                var screenW = $(window).width() - 30;//screen.availWidth - 5;
                $(".makeStyle").css("height", screenH);//.height = screenH;
                $(".makeStyle").css("width", "auto");//.width = screenW;
                var chartsCount = 0;

                $(document).ready(function () {

                    var h = $(window).height() - 110;
                    // $('#poContainer').css('height', h);
                    setHeaderContentSize();
                    $.unblockUI({});

                    if (document.getElementById('imageVideoConatiner') != null) {
                        countItem = 0;
                        countItem++;
                        $("#myCarousel").on('slid.bs.carousel', function () {
                            countItem++;
                            //$(".makeStyle").css("height", screenH);//.height = screenH;
                            //$(".makeStyle").css("width", "auto");//.width = screenW;
                            var video = $("#myCarousel .item.active").children("video");
                            var isVideo = (video.length > 0);
                            if (isVideo) {

                                $("#myCarousel").carousel("pause");

                                video.get(0).play();
                            }
                            if (countItem == $("#myCarousel .item").length) {
                                if (!isVideo) {
                                    // $( "#btnSave" ).trigger( "click" );
                                    //  window.location.href = "GEA.aspx";
                                    // _doPostBack('btnSave');
                                    setscreenName();
                                    $("#myCarousel").carousel("pause");
                                    __doPostBack('<%= btnPost.UniqueID%>', '');

                                }
                            }
                        });
                        if ($("#myCarousel .item").length == 1) {
                            var image = $("#myCarousel .item.active").children("img");
                            var isImage = (image.length > 0);
                            if (isImage) {
                                setTimeout(function () {
                                    // window.location.href = "GEA.aspx";
                                    // $( "#btnSave" ).trigger( "click" );
                                    //_doPostBack('btnSave');
                                    setscreenName();
                                    __doPostBack('<%= btnPost.UniqueID%>', '');
                                }, 5000);
                            }
                        }
                        if (countItem == 1) {
                            var video = $("#myCarousel .item.active").children("video");
                            var isVideo = (video.length > 0);
                            if (isVideo) {

                                $("#myCarousel").carousel("pause");

                                video.get(0).play();
                            }
                        }
                    }

                    //if (document.getElementById('downTimeContainer') != null) {
                    //    if ($('#hdnChartsCount').val() == "firstentry") {
                    //        $('#hdnChartsCount').val("completeFirstEntry");
                    //        setPORowsBasedOnHeight();  // called after setHeaderContentSize()
                    //    }
                    //}

                });
                $("video").each(function () {
                    this.addEventListener('ended', myHandler, false);
                    this.addEventListener('error', myHandler, false);
                });
                function myHandler(e) {
                    // What you want to do after the event
                    //if (countItem == 0 || countItem == $("#myCarousel .item").length) {
                    if ($("#myCarousel .item").length == 1) {
                        // window.location.href = "GEA.aspx";
                        setscreenName();
                        $("#myCarousel").carousel("pause");
                        __doPostBack('<%= btnPost.UniqueID%>', '');
                        return;

                    }
                    if ($("#myCarousel .item").length > 0) {
                        if (countItem == $("#myCarousel .item").length) {
                            setscreenName();
                            $("#myCarousel").carousel("pause");
                            __doPostBack('<%= btnPost.UniqueID%>', '');
                            return;
                        }
                    }
                    $("#myCarousel").carousel("cycle");
                }
                $('#ddlPlantID').change(function () {
                    $.blockUI({ message: '<img src="Images/LoadIcon/ajax-loader.gif" />' });
                    return true;
                });
                //$(window).resize(function () {
                //    var h = $(window).height() - 110;
                //    // $('#poContainer').css('height', h);
                //    var screenH = $(window).height() - 120;//screen.availHeight - 100;
                //    var screenW = $(window).width() - 30;//screen.availWidth - 5;
                //    $(".makeStyle").css("height", screenH);//.height = screenH;
                //    $(".makeStyle").css("width", "auto");//.width = screenW;
                //});
                $("[id$=btnFullScreen]").click(function () {
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
