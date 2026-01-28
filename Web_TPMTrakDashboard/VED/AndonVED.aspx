<%@ Page Title="VED Industries(Andon)" Language="C#" AutoEventWireup="true" CodeBehind="AndonVED.aspx.cs" Inherits="Web_TPMTrakDashboard.Andon.AndonVED" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VED Industries(Andon)</title>


    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.min.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/Site.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/TextAnimation.css" rel="stylesheet" />
    <script src="../GEA/Andon_GEA/Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>

    <script src="../Scripts/HighCharts10.3.2/highcharts.js"></script>
    <script src="../Scripts/HighCharts10.3.2/highcharts-more.js"></script>
    <script src="../Scripts/HighCharts10.3.2/solid-gauge.js"></script>
    <script src="../Scripts/HighCharts10.3.2/exporting.js"></script>
    <script src="../Scripts/HighCharts10.3.2/export-data.js"></script>
    <script src="../Scripts/HighCharts10.3.2/accessibility.js"></script>
    <script src="../Scripts/HighCharts10.3.2/pareto.js"></script>

    <style>
        * {
            font-family: <%= uiSettings.FontFamily %>;
            font-style: <%= uiSettings.FontStyle %>
        }

        .HeaderImage {
            flex: 1;
            float: left;
        }

        .headerRight {
            color: white;
            font-weight: 600;
            font-size: 20px;
            margin: 0px;
        }

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

        .ScreenDiv {
            display: flex;
            align-content: baseline;
            justify-content: center;
            flex-wrap: wrap;
            flex-direction: row;
            overflow: auto;
        }

        .innerDiv {
            display: flex;
            align-content: baseline;
            justify-content: center;
            flex-wrap: wrap;
            flex-direction: row;
            overflow: auto;
            margin: 10px 15px 10px 15px;
            border-radius: 20px;
            background: #F8F8F8;
            box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25);
        }

        .child-container-downtime {
            margin: 0px !important;
        }

        .innerDiv-Group {
            display: flex;
            align-content: baseline;
            justify-content: center;
            flex-wrap: wrap;
            flex-direction: row;
            overflow: auto;
        }

        .row {
            margin: 0px !important;
        }

        .previous {
            background-color: #f1f1f1;
            color: black;
        }

        .next {
            background-color: #04AA6D;
            color: white;
        }

        .round {
            border-radius: 50%;
        }

        .groupKPI {
        }


        .progress {
            background: #e2e2e0;
        }

        .KPILabel {
            display: inline-block;
            font-weight: bold;
            font-size: 25px;
        }

        .otherKPIPlantInnerDiv {
            margin-top: 60px;
        }

        .KPILabel-Parent-Div {
            margin-bottom: 3px;
        }

        .highcharts-background {
            fill: #F8F8F8 !important;
        }

        .KPIContainers {
            padding: 0px !important;
        }

        #container-machine-KPI .highcharts-title {
            font-weight: bold !important;
            font-size: 21px !important;
        }

        .OuterDivContainerStyle {
            display: flex;
            align-content: baseline;
            justify-content: center;
            flex-wrap: wrap;
        }

        .cellLabelDiv {
            background-color: #0f4987;
            padding: 5px 2px 0px 2px;
        }

            .cellLabelDiv span {
                color: white;
                font-weight: bold;
                font-size: 25px;
                font-family: sans-serif;
            }



        .colspan-1-class {
            min-width: 160px;
        }

        .colspan-2-class {
            min-width: 320px;
        }

        .HourlyTargetCount-table tbody tr:first-child td {
            background: #d9dfff;
            font-weight: bold;
            font-size: 25px;
        }

        .HourlyTargetCount-table tbody tr:last-child td {
            font-weight: bold;
            font-size: 25px;
        }

        .HourlyTargetCount-table tbody tr td {
            border: 1px solid silver;
            border-collapse: collapse;
        }

            .HourlyTargetCount-table tbody tr td:first-child {
                border: 2px solid silver;
                font-size: 25px;
            }

        .IsCurrentHour {
            background-color: #6FFACC !important;
        }

        .TotalRow {
            background-color: #F9F871 !important;
        }


        .carousel-indicators {
            visibility: hidden;
        }

        #myCarousel {
            height: 100%;
        }

            #myCarousel .item {
                height: 100%;
            }

            #myCarousel .carousel-inner {
                height: 100%;
            }

            #myCarousel img, #myCarousel video {
                height: 100%;
            }
    </style>
</head>
<body style="overflow: hidden; background: white;">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdnWidth" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnHeight" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdntotalWidth" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnUseCustomWidthFlag" ClientIDMode="Static" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Timer runat="server" ID="refreshTimer" OnTick="refreshTimer_Tick" ClientIDMode="Static"></asp:Timer>

                <div class="row text-center" style="overflow: hidden; margin: 0px !important;">
                    <asp:Label runat="server" ID="lbltestMsg" ClientIDMode="Static"></asp:Label>
                    <div class="navbar navbar-default navbar-fixed-top text-center headerDiv" style="padding: 0px 5px; background-color: #0F69EF; border: none; height: 60px;">
                        <asp:Label ID="headerName" runat="server" ClientIDMode="static" Style="color: white; font-weight: bold; font-size: 42px; text-align: right; vertical-align: middle">Andon</asp:Label>
                    </div>
                    <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 0px; background-color: transparent; border: none;" id="headerDiv">
                        <div class="HeaderImage" style="height: 50px; margin-left: 5px;padding: 5px;">
                            <asp:Image ID="ComanyLogo" runat="server" class="img-responsive" ImageUrl="~/VED/Images/VEDGroupLogo.jpg" Style="height: 50px;border-radius: 20px;" />
                        </div>
                        <div style="float: right; position: relative;" runat="server" id="divHeader">
                            <table id="tableSettings" style="">
                                <tr>
                                    <td colspan="2">
                                        <div class="datetimeDiv" style="margin-right: 5px;">
                                            <asp:Label runat="server" ID="lblDateTime" ForeColor="White" Font-Size="Large"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblShift" ForeColor="White" Font-Size="Large"></asp:Label>
                                        </div>
                                    </td>
                                    <td runat="server" id="tdAndonSettings" onclick="location.href='AndonSettingsVED.aspx';" style="cursor: pointer">
                                        <%--<asp:ImageButton runat="server" ID="btnSettings" CssClass="companyicon"  Style="width: 70px; height: 50px; margin-bottom: 7px; margin-right: 20px; cursor: pointer;"  ImageUrl="~/Image/Settings3.png" OnClick="btnSettings_Click" />--%>
                                        <asp:Image ID="Image1" runat="server" CssClass="companyicon" ImageUrl="~/Image/Settings3.png" Style="width: 70px; height: 50px; margin-bottom: 7px; margin-right: 20px; cursor: pointer;" />
                                    </td>

                                    <td style="padding: 0px !important;">
                                        <div class="HeaderImage" style="height: 56px">
                                            <asp:Image ID="customerLogo" runat="server" class="img-responsive" ImageUrl="~/VED/Images/VEDLogo.png" Style="height: 57px;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>


                    <div id="MainContainer" runat="server" clientidmode="static">

                        <div class="row" id="AlertDiv" runat="server">
                            <div class="AlertDiv" style="margin-top: 45px;">
                                <asp:Label runat="server" ID="lblAlertMessage" ClientIDMode="Static" Style="font-weight: bold; font-size: xx-large; color: crimson;"></asp:Label>
                            </div>
                        </div>

                        <div id="ProductionAndonContainer" runat="server" clientidmode="static" class="ProductionAndonContainer" style="padding-top: 10px;">
                            <div runat="server" id="MonthlyPlantEfficiency" clientidmode="static" class="ScreenDiv">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 mainDiv" style="width: inherit; padding: 0px !important;">
                                        <div class="col-lg-5 col-sm-5 col-md-5 innerDiv KPIContainers" style="margin-left: 25px;">

                                            <div id="container-OEE-Plant" class="KPIPlant"></div>

                                            <div class="KPIPlant" style="background: #F8F8F8;">

                                                <div class="otherKPIPlantInnerDiv">
                                                    <div id="container-AE-Plant"></div>
                                                    <div id="container-PE-Plant"></div>
                                                    <div id="container-QE-Plant"></div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="col-lg-5 col-sm-5 col-md-5 innerDiv KPIContainers" style="margin-left: 25px;">
                                            <div class="row group-container-header-div" style="margin: 8px !important;">
                                                <div style="float: left; font-weight: bold; font-size: 20px;">Cell Wise OEE</div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 innerDiv-Group  KPIContainers-inner-div">
                                                    <div id="container-group1-KPI" class="groupKPI"></div>
                                                    <div id="container-group2-KPI" class="groupKPI"></div>
                                                    <div id="container-group3-KPI" class="groupKPI"></div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 DowntimeChartDiv innerDiv" style="width: inherit; padding: 0px !important;">
                                        <div id="container-machine-KPI"></div>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="CurrentMonthDowntime" clientidmode="static" class="ScreenDiv">
                                <div id="container-pie" class="child-container-downtime"></div>
                                <div id="container-pareto" class="child-container-downtime innerDiv"></div>
                                <br />
                                <div id="container-col" class="child-container-downtime innerDiv" style="margin-top: 10px !important;"></div>
                            </div>
                            <div runat="server" id="HourlyPartCountMachineLevel" clientidmode="static" class="ScreenDiv">
                                <div class="OuterDivContainerStyle CurrentMonthDowntime" style="margin-top: 40px;">
                                    <asp:ListView runat="server" ID="lvhourlyTargetCount">
                                        <LayoutTemplate>
                                            <table class="HourlyTargetCount-table" style="box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25);">
                                                <tr runat="server" id="itemplaceholder"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Eval("IsCurrentHour") %>'>
                                                <td >
                                                    <asp:Label runat="server" ID="lblTime" Text='<%# Eval("HourTiminigs") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:ListView runat="server" ID="lvInnerListView" ItemPlaceholderID="itemplaceholderinnerlv" DataSource='<%# Eval("HourDataByMachine") %>'>
                                                        <LayoutTemplate>
                                                            <table class="lvInnerListView">
                                                                <tr>
                                                                    <td runat="server" id="itemplaceholderinnerlv"></td>
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <td class='<%# Eval("cssClass") %> <%# Eval("IsCurrentHour") %>' style="font-size: <%# Eval("fontSize")%>px; display: <%# Eval("td1visible")%>">
                                                                <asp:Label runat="server" ID="lblTarget" Text='<%# Eval("Target") %>'></asp:Label>
                                                            </td>
                                                            <td class='<%# !Eval("tdVisibility").ToString().Equals("none", StringComparison.OrdinalIgnoreCase) ? "colspan-1-class" : "" %> <%# Eval("IsCurrentHour") %>' style='display: <%# Eval("tdVisibility") %>; color: <%# Eval("ActualColorCode") %>; font-size: <%# Eval("fontSize")%>px;'>
                                                                <asp:Label runat="server" ID="lblActual" Text='<%# Eval("Actual") %>'></asp:Label>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>

                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                            <div runat="server" id="Andon" clientidmode="static" class="ScreenDiv" style="overflow: hidden;">
                                <div class="">
                                    <div style="text-align: center" class="cellLabelDiv" runat="server" id="cellLabelDiv">
                                        <asp:Label runat="server" ID="lblCellName" CssClass="cellLabel"></asp:Label>
                                    </div>
                                    <div id="cockpitContainer" class="OuterDivContainerStyle" style="overflow: hidden;">
                                        <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="lvCockpit">
                                            <LayoutTemplate>
                                                <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <div class="myItem" style="margin: <%# Eval("TopMargin")%>px <%# Eval("LeftMargin")%>px 0px <%# Eval("LeftMargin")%>px; min-width: 100px; display: inline-block; vertical-align: top; width: <%# Eval("CockpitBoxWidth") %>px; overflow-x: hidden">
                                                    <div class="<%# Eval("BorderClass") %>">
                                                        <div class="<%# Eval("MachineOEE") %> <%# Eval("BorderClass") %>" style="padding: 5px; background-color: <%# Eval("BackColor") %>;">
                                                            <table style="width: 100%;" class="outercockpit">
                                                                <tr>
                                                                    <td style="text-align: center; color: black; font-weight: bold; padding-left: 20px; padding-bottom: 0px;" id="tdMachineID">
                                                                        <label id="lblMachineID" style="font-size: <%# Eval("MachineFontSize")%>px;" clientidmode="static"><%# Eval("MachineId") %></label>
                                                                    </td>
                                                                    <td style="background-color: transparent; width: 35px;">
                                                                        <asp:Image ImageUrl='<%# Eval("StatusImage") %>' runat="server" meta:resourcekey="ImageResource1" />
                                                                        <div class="loaders-container" runat="server" visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?false:true %>'>
                                                                            <div class="la-cog la-2x" style="float: right;">
                                                                                <div class="<%# Eval("MachineStatus") %>"></div>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white; margin-bottom: 0px; width: 100%; table-layout: <%# Eval("TableLayout") %>'>
                                                                <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                                    <LayoutTemplate>
                                                                        <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td style="text-align: <%# Eval("TextAlign") %>; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>; font-size: <%# Eval("FontSizeInnerData") %>px; width: 45%;">
                                                                                <%# Eval("LabelText")%>                                           
                                                                            </td>
                                                                            <td style='text-align: <%# Eval("TextAlign") %>; white-space: nowrap; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>; font-size: <%# Eval("DataFontSize") %>px; width: 45%;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                                                <div class="ellipsistooltip">
                                                                                    <asp:Label runat="server" Text='<%# Eval("LabelValue1")%>'></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label runat="server" Text='<%# Eval("LabelValue2")%>'></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </table>
                                                        </div>
                                                        <div style='text-align: center; padding: 1px; display: <%# Eval("ShowSmileyBlock") %>;'>
                                                            <img src="<%# Eval("SmileyImagePath") %>" style='height: <%# Eval("SmileySize") %>px; width: auto;' />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </div>
                            </div>

                            <div id="slideShowContainer" style="" runat="server" clientidmode="static">
                                <div id="myCarousel" class="carousel slide" data-bs-ride="carousel">
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
                            </div>
                            <asp:Button runat="server" ID="btnPost" ClientIDMode="Static" OnClick="btnPost_Click" Style="visibility: hidden" />

                        </div>

                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="refreshTimer" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="" id="ComputerDiv" runat="server">
            <div class="modal-dialog">
                <fieldset style="border-color: black" id="lblfieldset">
                    <legend class="legendStyleSetting">Computer Name</legend>
                    <table id="tblComputer" style="width: 95%; margin-top: 10px;">

                        <tr>
                            <td><span style="font-size: medium; font-weight: 400;">Computer Name</span></td>
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


        <div class="modal infoModal bajaj-info-modal" id="loginModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 30%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Login</h4>
                        <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                    </div>
                    <div class="modal-body">
                        <table>
                            <tr>
                                <td>User Name</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="rfvUserName" ControlToValidate="txtUserName" runat="server" ErrorMessage="Username is Mandatory"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>Password</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-primary" Text="Login" OnClientClick="return validateLogin();" />
                    </div>
                </div>
            </div>
        </div>

        <script>
            var dataRefreshTimer;
            $(document).ready(function () {
                $(".HourlyTargetCount-table tbody tr").each(function () {
                    debugger;
                    $(this).find(".lvInnerListView tbody tr td").css("border", "0px");
                });
                $(".HourlyTargetCount-table tbody tr").each(function () {
                    debugger;
                    $(this).find(".lvInnerListView tbody tr td").css("border-right", "1px solid silver");
                });
                $(".HourlyTargetCount-table tbody tr:gt(3)").each(function () {
                    debugger;
                    $(this).find(".lvInnerListView tbody tr td").css("background-color", "white");
                });

                if (!($('#ComputerDiv').is(':visible'))) {
                    setDataRefreshTimer();
                }
                else {
                    $('#MainContainer').css("display", "none");
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
            });

            var scalefactor = 1.0;
            if (window.devicePixelRatio >= 1.25) {
                scalefactor = 1.0 / window.devicePixelRatio;
            }

            function SetContainerWidth() {
                debugger;
                $(".ScreenDiv").width($(window).width());
                $(".cellLabelDiv").width($(window).width());
                var footerDivHeight = 0;
                if ($("#FooterDiv").height() != undefined)
                    footerDivHeight = $("#FooterDiv").height()
                $(".ScreenDiv").height($(window).height() - footerDivHeight - 60);

                $(".mainDiv").width($(".ScreenDiv").width());
                $(".KPIContainers").width(($(".ScreenDiv").width() / 2) - 40);
                $(".KPIContainers").height(($(".ScreenDiv").height() / 2) - 70);

                $(".KPIContainers-inner-div").width($(".KPIContainers").width() - 60);
                $(".group-container-header-div").width($(".KPIContainers").width() - 60);
                $(".KPIContainers-inner-div").height($(".KPIContainers").height() - 60);

                $(".DowntimeChartDiv").width($(".ScreenDiv").width() - 30);
                $(".DowntimeChartDiv").height(($(".ScreenDiv").height() / 2) + 30);

                $(".child-container-downtime").width(($(".ScreenDiv").width()) - 10);
                $(".child-container-downtime").height(($(".ScreenDiv").height() / 3) - 10);


                let cellLabelDivHeight = $(".cellLabelDiv").height();
                if (cellLabelDivHeight == undefined) {
                    cellLabelDivHeight = 0;
                }

            }

            function setDataRefreshTimer() {
                clearTimeout(dataRefreshTimer);
                dataRefreshTimer = setTimeout(insertLatestDataToMainCache,<%= uiSettings.DataRefreshInterval %>);
                $("#hdnFilterShowHide").val("Hide");
                //ShowPanelFilter();
            }

            function insertLatestDataToMainCache() {
                //
                $.ajax({
                    async: true,
                    type: "POST",
                    url: "AndonVED.aspx/insertLatestDataToMainCacheMemory",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    success: function (response) {
                        setDataRefreshTimer();
                    },
                    error: function (Result) {
                        setDataRefreshTimer();
                    }
                });
            }

            function setFlipInterval() {
                SetContainerWidth();
                $("#hdnWidth").val("");
                $("#hdntotalWidth").val("");

                let cellLabelDivHeight = $(".cellLabelDiv").height();
                if (cellLabelDivHeight == undefined) {
                    cellLabelDivHeight = 0;
                }

                var footerDivHeight = 0;
                if ($("#FooterDiv").height() != undefined)
                    footerDivHeight = $("#FooterDiv").height()

                $('#cockpitContainer').css("height", $(window).height() - footerDivHeight - cellLabelDivHeight - 70);
                $('#cockpitContainer').css("width", $(window).width());

                setMachineFontSize();
                SetIconicBoxWidth();

                var divH = Math.max.apply(null, $('.myItem').map(function () {
                    return $(this).outerHeight(true);
                }).get());

                var divW = Math.max.apply(null, $('.myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                let screenH = $('#cockpitContainer').height();
                let screenW = $('#cockpitContainer').width();
                let totalH = Math.floor(screenH / (divH));
                let totalW = Math.floor(screenW / (divW));
                let totalBox = Math.floor(totalH * totalW);
                $("#cockpitContainer .myItem").hide();
                $("#cockpitContainer .myItem").slice(0, totalBox).show();
                $("#hdnWidth").val(divW);
                $("#hdntotalWidth").val(totalW);

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "AndonVED.aspx/setFlipIntervalToSession",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: '{displayedItemCount:' + totalBox + '}',
                    success: function (response) {
                    },
                    error: function (Result) {
                    }
                });
            }

            function setMachineFontSize() {
                $(".cellLabelDiv").width($(window).width());

                var list = $('#cockpitContainer .myItem');
                var maxLength = 14;
                for (var i = 0; i < list.length; i++) {
                    if (list[i].querySelector('#lblMachineID').innerHTML.length >= maxLength) {
                        var machineID = list[i].querySelector('#lblMachineID').innerHTML;
                        list[i].querySelector('#lblMachineID').innerHTML = machineID.slice(0, maxLength) + '..';
                    }
                }
            }

            function SetIconicBoxWidth() {

                var divW;
                if ($("#hdnUseCustomWidthFlag").val() != "1") {
                    if ($("#hdnWidth").val() == "") {
                        divW = Math.max.apply(null, $('.myItem').map(function () {
                            return $(this).outerWidth(true);
                        }).get());
                        $("#cockpitContainer .myItem").width(divW);
                    }
                    else {
                        $("#cockpitContainer .myItem").width($("#hdnWidth").val());
                        divW = $("#hdnWidth").val();
                    }
                }
                else {
                    divW = Math.max.apply(null, $('.myItem').map(function () {
                        return $(this).outerWidth(true);
                    }).get());
                    $("#cockpitContainer .myItem").width(divW);
                }

                //$("#txtBoxWidth").val("Box width= " + Math.round(Number(divW), 3).toString() + "px / " + (Number($('#cockpitContainer').width()) - 7).toString() + "px");
                //$("#txtBoxWidth").attr("title", "Box width= Width of each Box / Total Available Width \n Box width=" + Math.round(Number(divW), 3).toString() + "px / " + (Number($('#cockpitContainer').width()) - 7).toString() + "px");


                if ($("#hdnHeight").val() == "" || $("#hdnHeight").val() == undefined) {
                    var divH = Math.max.apply(null, $('.myItem .cockpit').map(function () {
                        return $(this).outerHeight(true);
                    }).get());
                    $("#hdnHeight").val(divH);
                }
                $("#cockpitContainer .myItem .cockpit").height($("#hdnHeight").val());
                if ($("#hdntotalWidth").val() != "") {
                    $("#cockpitContainer").addClass("OuterDivContainerStyle");
                }


            }


            function BindEfficiencyCharts() {
                SetContainerWidth();
                $.ajax({
                    async: true,
                    type: "POST",
                    url: "AndonVED.aspx/GetKPIChartData",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    success: function (response) {
                        if (response.d != undefined) {
                            var chartData = response.d;
                            var CellData = chartData.CellWiseKPIsMonthly;
                            var MachineData = chartData.MachineWiseKPIsMonthly;

                            //Set Containers dimensions

                            //Cell KPI
                            if (CellData.length > 0)
                                $(".groupKPI").width(($(".KPIContainers-inner-div").width() / CellData.length) - 10);
                            else {
                                $(".groupKPI").width($(".KPIContainers-inner-div").width() - 10);
                            }

                            //Plant KPI
                            $(".KPIPlant").width(($(".KPIContainers").width() / 2) - 10);
                            debugger;

                            BindDonutChart(chartData, 'container-OEE-Plant', $(".KPIContainers").height() , 'Plant', chartData.OEE, chartData.DonutChartFontSize, chartData.OEEColor);

                            $("#container-AE-Plant").empty();
                            var strAppendAE = '';
                            strAppendAE += '<div class="KPILabel-Parent-Div"><div class="KPILabel" style="float: left;">Availability</div> <div class="KPILabel" style="float: right;">' + chartData.AE + '%</div></div><div class="progress"><div class="progress-bar" id="progress-bar-AE" role="progressbar" aria-valuenow="' + chartData.AE + '" aria-valuemin="0" aria-valuemax="100" style="background-color:' + chartData.AEColor + '"></div></div>';
                            $("#container-AE-Plant").append(strAppendAE);


                            $("#container-PE-Plant").empty();
                            var strAppendPE = '';
                            strAppendPE += '<div class="KPILabel-Parent-Div"><div class="KPILabel" style="float: left;">Performance</div> <div class="KPILabel" style="float: right;">' + chartData.PE + '%</div></div><div class="progress"><div class="progress-bar" id="progress-bar-PE" role="progressbar" aria-valuenow="' + chartData.PE + '" aria-valuemin="0" aria-valuemax="100" style="background-color:' + chartData.PEColor + '"></div></div>';
                            $("#container-PE-Plant").append(strAppendPE);


                            $("#container-QE-Plant").empty();
                            var strAppendQE = '';
                            strAppendQE += '<div class="KPILabel-Parent-Div"><div class="KPILabel" style="float: left;">Quality</div> <div class="KPILabel" style="float: right;">' + chartData.QE + '%</div></div><div class="progress"><div class="progress-bar" id="progress-bar-QE" role="progressbar" aria-valuenow="' + chartData.QE + '" aria-valuemin="0" aria-valuemax="100" style="background-color:' + chartData.QEColor + '"></div></div>';
                            $("#container-QE-Plant").append(strAppendQE);

                            $(".progress").width($(".KPIPlant").width() - 50);
                            $(".KPILabel-Parent-Div").width($(".KPIPlant").width() - 50);

                            $("#progress-bar-AE").width((chartData.AE / 100) * $(".KPILabel-Parent-Div").width());
                            $("#progress-bar-PE").width((chartData.PE / 100) * $(".KPILabel-Parent-Div").width());
                            $("#progress-bar-QE").width((chartData.QE / 100) * $(".KPILabel-Parent-Div").width());

                            for (var i = 0; i < CellData.length; i++) {
                                BindDonutChart(CellData[i], 'container-group' + (i + 1) + '-KPI', ($(".KPIContainers").height() - 60), 'Cell', CellData[i].OEE + '%', chartData.DonutChartFontSize, CellData[i].OEEColor); //
                            }

                            $("#container-machine-KPI").width($(".DowntimeChartDiv").width() - 10);
                            $("#container-machine-KPI").height($(".DowntimeChartDiv").height());
                            var Categories = [], values = [];
                            for (var i = 0; i < MachineData.length; i++) {
                                Categories.push(MachineData[i].EntityID);
                                values.push(MachineData[i].OEE);
                            }

                            BindColumnChart(Categories, 'container-machine-KPI', values, 'OEE in %', 'Machine Wise OEE', chartData.ColumnChartxAxisFontSize, chartData.ColumnChartyAxisFontSize, chartData.ColumnChartDataLabelFontSize);
                        }
                    },
                    error: function (Result) {

                    }
                });
            }

            function BindDowntimeCharts() {
                SetContainerWidth();
                $.ajax({
                    async: true,
                    type: "POST",
                    url: "AndonVED.aspx/GetDowntimeScreenData",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    success: function (response) {
                        if (response.d != undefined) {
                            var itmdata = response.d;

                            var downtimeData = itmdata.downtimeData;

                            var pieAndcolData = [];
                            var Categories = [];
                            var dtimeInSec = [];
                            for (var i = 0; i < downtimeData.length; i++) {
                                var temp = {
                                    name: downtimeData[i].dCode,
                                    y: downtimeData[i].downTimeinMin
                                }
                                Categories.push(downtimeData[i].dCode);
                                dtimeInSec.push(downtimeData[i].downTimeinMin);
                                pieAndcolData.push(temp);
                            }

                            Bind3DPieChart('container-pie', pieAndcolData, itmdata.PieChartFontSize);
                            BindParetoChart('container-pareto', dtimeInSec, '25px', Categories, "Down code", itmdata.ParetoChartxAxisFontSize, itmdata.ParetoChartyAxisFontSize, itmdata.ParetoChartColumnDatalabelsFontSize, itmdata.ParetoChartParetoDatalabelsFontSize);
                            Categories = [];
                            var values = [];

                            for (var j = 0; j < itmdata.CategoriesData.length; j++) {
                                Categories.push(itmdata.CategoriesData[j].dCategory);
                                values.push(itmdata.CategoriesData[j].downTimeinMin);
                            }
                            BindDowntimeChart(Categories, 'container-col', values, 'Downtime in HH:mm', '', 'Down category', itmdata.ColumnChartxAxisFontSize, itmdata.ColumnChartyAxisFontSize, itmdata.ColumnChartDataLabelFontSize);
                        }
                    },
                    error: function (Result) {

                    }
                });


            }

            function BindDonutChart(gdata, ContainerID, heightValue, KPILevel, chartValue, fontSize, backgroundColor) {

                var labelXValue = 35;
                var formattedText = (ContainerID == 'container-OEE-Plant' ? '<div style="text-align:center; color: black"><label style="font-size:' + fontSize + ';color:black">' + chartValue + '%</label></div>' : '<label style="font-size:' + fontSize + ';color:black">' + chartValue + '</label>');
                var textAlign = 'center';

                if (KPILevel == "Plant") {
                    gdata.EntityID = 'Plant OEE';
                    textAlign = 'left';
                }

                if (KPILevel != "Plant") {
                    fontSize = Number(fontSize) - 4;
                }
                if (gdata.OEE == 0) {
                    labelXValue = 25;
                }
                Highcharts.chart(ContainerID, {
                    chart: {
                        type: 'solidgauge',
                        height: heightValue + 'px',
                        displayName: chartValue,
                    },

                    title: {
                        useHTML: true,
                        text: gdata.EntityID,
                        align: textAlign,
                        style: {
                            fontSize: '21px',
                            fontWeight: 'bold'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    tooltip: {
                        enabled: false
                    },
                    exporting: {
                        enabled: false
                    },
                    pane: {
                        startAngle: 0,
                        endAngle: 360,
                        background: [{ // Track for Move
                            outerRadius: '112%',
                            innerRadius: '95%',
                            //borderWidth: 0,
                            // borderColor: '#5c5a5a',
                            backgroundColor: '#e2e2e0'

                        }],

                    },

                    yAxis: {
                        min: 0,
                        max: 100,
                        lineWidth: 0,
                        tickPositions: []
                    },
                    exporting: {
                        enabled: false
                    },
                    credits: {
                        enabled: false
                    },
                    plotOptions: {
                        solidgauge: {
                            dataLabels: {
                                borderColor: 'white',
                                borderWidth: 0,
                                verticalAlign: 'middle',
                                align: 'center',
                                enabled: true,
                                //style: {
                                //    fontSize: '30px'
                                //},
                                padding: 0,
                                x: labelXValue,
                                style: {
                                    textShadow: false,
                                    textOutline: false,
                                    width: "95px",
                                    textAnchor: 'middle',
                                },
                                format:
                                    formattedText
                            },
                            stickyTracking: true,
                            rounded: true,
                        },
                        series: {
                            cursor: 'pointer',
                        }
                    },

                    series: [{
                        name: '',
                        data: [{
                            color: backgroundColor,
                            radius: '112%',
                            innerRadius: '95%',
                            y: gdata.OEE
                        }],
                    }]
                });

                if (ContainerID == "container-OEE-Plant") {
                    var x = ($("#container-OEE-Plant").width() / 2) - 10;
                    var y = (heightValue / 2) - 30;
                    $("#container-OEE-Plant").find(".highcharts-data-label").attr('transform', 'translate(' + x + ', ' + y + ')');
                }
                else {
                    var x = ($('#' + ContainerID).width() / 2) - 10;
                    var y = (heightValue / 3) + 10;
                    $('#' + ContainerID).find(".highcharts-data-label").attr('transform', 'translate(' + x + ',' + y + ')');
                }
            }

            var grossLabel;

            function alignLabel() {
                debugger;
                var chart = this;

                if (grossLabel) {
                    grossLabel.destroy();
                }

                var newX = chart.plotWidth / 2 + chart.plotLeft,
                    newY = chart.plotHeight / 2 + chart.plotTop;

                grossLabel = chart.renderer.text('', newX, newY)
                    .attr({
                        align: 'center'
                    })
                    .css({
                        color: '#4572A7',
                        fontSize: '12pt'
                    }).add();
            }

            function BindColumnChart(Categories, ContainerID, values, yAxisTitle, gTitle, xAxisFontSize, yAxisFontSize, DatalabelsFontSize) {
                Highcharts.chart(ContainerID, {
                    chart: {
                        type: 'column'
                    },
                    legend: { enabled: false },
                    credits: { enabled: false },
                    exporting: { enabled: false },
                    title: {
                        text: gTitle,
                        align: 'left'
                    },
                    xAxis: {
                        categories: Categories,
                        crosshair: true,
                        labels: {
                            enabled: true,
                            style: {
                                fontSize: xAxisFontSize,
                                fontWeight: 'bold',
                                color: 'black'
                            }
                        }
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: yAxisTitle,
                            style: {
                                fontWeight: 'bold',
                                color: 'black'
                            }
                        },
                        max: 100,
                        labels: {
                            enabled: true,
                            style: {
                                fontSize: yAxisFontSize,
                                fontWeight: 'bold',
                                color: 'black'
                            }
                        }
                    },
                    tooltip: {
                        enabled: false,
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                style: {
                                    fontSize: DatalabelsFontSize,
                                    fontWeight: 'bold',
                                }
                            }
                        },
                        series: {
                            colorByPoint: true
                        }
                    },
                    series: [{
                        data: values
                    }]
                });

            }


            function BindDowntimeChart(Categories, ContainerID, values, yAxisTitle, gTitle, xAxisTitle, xAxisFontSize, yAxisFontSize, DatalabelsFontSize) {
                Highcharts.chart(ContainerID, {
                    chart: {
                        type: 'column'
                    },
                    legend: { enabled: false },
                    credits: { enabled: false },
                    exporting: { enabled: false },
                    title: {
                        text: gTitle,
                        align: 'left'
                    },
                    xAxis: {
                        categories: Categories,
                        crosshair: true,
                        title: {
                            text: xAxisTitle,
                            style: {
                                color: 'black',
                                fontWeight: 'bold',
                            }
                        },
                        labels: {
                            style: {
                                color: 'black',
                                fontWeight: 'bold',
                                fontSize: xAxisFontSize,
                            },
                        }
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: yAxisTitle,
                            style: {
                                color: 'black',
                                fontWeight: 'bold',
                            },
                        },
                        labels: {
                            style: {
                                color: 'black',
                                fontWeight: 'bold',
                                fontSize: yAxisFontSize,
                            },
                            formatter: function () {
                                var value = '';
                                var time = this.value;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                return value;
                            }
                        }
                    },

                    tooltip: {
                        enabled: false
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                style: {
                                    color: 'black',
                                    fontWeight: 'bold',
                                    fontSize: DatalabelsFontSize,
                                },
                                formatter: function () {
                                    var value = '';
                                    if (this.series.name != 'Pareto') {
                                        var time = this.y;
                                        var hours1 = parseInt(time / 60);
                                        var mins1 = parseInt(time % 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                    }
                                    return value;
                                },
                            }
                        },
                        series: {
                            colorByPoint: true
                        }
                    },
                    series: [{
                        data: values
                    }]
                });

            }

            function Bind3DPieChart(ContainerID, dData, datalabelsFontSize) {
                Highcharts.chart(ContainerID, {
                    colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                        '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                        '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                    credits: { enabled: false },
                    tooltip: { enabled: false },
                    exporting: { enabled: false },
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie',
                    },
                    title: {
                        text: ''
                    },

                    tooltip: {
                        enabled: false
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: false,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                style: {
                                    fontSize: datalabelsFontSize,
                                    fontWeight: 'bold',
                                    color: 'black'
                                },
                                connectorColor: 'rgba(128,128,128,0.5)',
                                formatter: function () {
                                    var value = '';
                                    if (this.series.name != 'Pareto') {
                                        var time = this.y;
                                        var hours1 = parseInt(time / 60);
                                        var mins1 = parseInt(time % 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                        value = this.point.name + " : " + value + "  (" + this.percentage.toFixed(1) + "%)";
                                    }
                                    return value;
                                }
                            },
                            showInLegend: false
                        }
                    },
                    series: [{
                        data: dData,
                    }]
                });
            }

            function BindParetoChart(ContainerID, dData, labelFontSize, Categories, xAxisTitle, xAxisFontSize, yAxisFontSize, ColumnDatalabelsfontSize, ParetoLabelsfontsize) {

                Highcharts.chart(ContainerID, {
                    colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                        '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                        '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                    credits: { enabled: false },
                    tooltip: { enabled: false },
                    exporting: { enabled: false },
                    chart: {
                        type: 'column',
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: Categories,
                        title: {
                            text: xAxisTitle,
                            style: {
                                color: 'black',
                                fontWeight: 'bold',
                            },
                        },
                        labels: {
                            style: {
                                color: 'black',
                                fontWeight: 'bold',
                                fontSize: xAxisFontSize
                            },
                        },
                    },
                    yAxis: [{
                        title: {
                            text: 'Downtime in HH:mm',
                            style: {
                                color: 'black',
                                fontWeight: 'bold'
                            }
                        },
                        labels: {
                            style: {
                                fontSize: yAxisFontSize,
                                color: 'black',
                                fontWeight: 'bold',
                            },
                            formatter: function () {
                                var value = '';
                                var time = this.value;
                                var hours1 = parseInt(time / 60);
                                var mins1 = parseInt(time % 60);
                                value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                return value;
                            }
                        }
                    }, {
                        title: {
                            text: ''
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        tickInterval: 20,
                        labels: {
                            format: "{value}%",
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: yAxisFontSize,
                            }
                        }
                    }],

                    tooltip: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                style: {
                                    fontSize: ColumnDatalabelsfontSize,
                                    color: 'black',
                                    fontWeight: 'bold',
                                },
                                formatter: function () {

                                    var value = '';
                                    if (this.series.name != 'pareto') {
                                        var time = this.y;
                                        var hours1 = parseInt(time / 60);
                                        var mins1 = parseInt(time % 60);
                                        value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                    }
                                    else {
                                        value = this.y == undefined ? '' : (Math.round((this.y + Number.EPSILON) * 100) / 100) + '%';
                                    }
                                    return value;
                                }
                            },
                            showInLegend: false,
                        },
                        pareto: {
                            dataLabels: {
                                enabled: true,
                                style: {
                                    fontSize: ParetoLabelsfontsize,
                                    color: 'black',
                                    fontWeight: 'bold',
                                }
                            }
                        }
                    },
                    series: [{
                        name: 'pareto',
                        type: 'pareto',
                        yAxis: 1,
                        zIndex: 10,
                        baseSeries: 1

                    }, {
                        name: 'Downtime',
                        type: 'column',
                        colorByPoint: true,
                        zIndex: 2,
                        data: dData,
                    }]
                });
            }


            var countItem = 0;

            function showImageVideo() {


                let footerHeight = $('#footerDiv').height();
                if (footerHeight == undefined) {
                    footerHeight = 0;
                }

                $('#slideShowContainer').css("height", $(window).height() - footerHeight - 70);
                $('#slideShowContainer').css("width", $(window).width() - 10);

                var flipInterval = getImageVideoFlipInterval();

                $('.carousel').carousel({
                    pause: "false",
                    interval: flipInterval
                });
                //
                if (document.getElementById('slideShowContainer') != null) {
                    countItem = 0;
                    countItem++;
                    $("#myCarousel").on('slid.bs.carousel', function () {
                        countItem++;
                        $('#slideShowContainer').css("height", $(window).height() - footerHeight - 70);
                        $('#slideShowContainer').css("width", $(window).width() - 10);

                        var video = $("#myCarousel .item.active").children("video");
                        var isVideo = (video.length > 0);
                        if (isVideo) {

                            $("#myCarousel").carousel("pause");

                            video.get(0).play();

                            var c = $('.carousel');
                            opt = c.data()['bs.carousel'].options;
                            opt.interval = 3000;
                            c.data({ options: opt });
                        }
                        //
                        if (countItem == $("#myCarousel .item").length) {
                            if (!isVideo) {
                                setTimeout(function () {

                                    $("#myCarousel").carousel("pause");
                                    __doPostBack('<%= btnPost.UniqueID%>', '');
                                }, flipInterval - 1);

                            }
                        }
                    });
                    //
                    if ($("#myCarousel .item").length == 1) {
                        var image = $("#myCarousel .item.active").children("img");
                        var isImage = (image.length > 0);
                        if (isImage) {
                            setTimeout(function () {

                                __doPostBack('<%= btnPost.UniqueID%>', '');
                            }, flipInterval);
                        }
                    }
                    if (countItem == 1) {
                        var video = $("#myCarousel .item.active").children("video");
                        var isVideo = (video.length > 0);
                        if (isVideo) {
                            $("#myCarousel").carousel("pause");
                            //video.get(0).muted = false;
                            video.get(0).play();

                            var c = $('.carousel');
                            opt = c.data()['bs.carousel'].options;
                            opt.interval = 3000;
                            c.data({ options: opt });
                        }
                        else {
                            pauseTheVideo(); //for the first image, video's audio play automatically
                        }
                    }
                }
            }

            function pauseTheVideo() {
                var videos = $("#myCarousel video");
                for (var i = 0; i < videos.length; i++) {
                    let videoCtrl = videos[i];
                    videoCtrl.pause();
                }
            }
            $("video").each(function () {
                this.addEventListener('ended', myHandler, false);
                this.addEventListener('error', myHandler, false);
            });
            function myHandler(e) {

                if ($("#myCarousel .item").length == 1) {

                    $("#myCarousel").carousel("pause");
                    __doPostBack('<%= btnPost.UniqueID%>', '');
                    return;

                }
                if ($("#myCarousel .item").length > 0) {
                    if (countItem == $("#myCarousel .item").length) {
                        $("#myCarousel").carousel("pause");
                        __doPostBack('<%= btnPost.UniqueID%>', '');
                        return;
                    }
                }
                $("#myCarousel").carousel("cycle");
            }
            function getImageVideoFlipInterval() {

                var ivFilpInterval = "";
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("AndonVED.aspx/getImageVideoFlipIntervalValue") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    success: function (response) {
                        var dataitem = response.d;
                        ivFilpInterval = dataitem * 1000;
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
                return ivFilpInterval;
            }

            function validateLogin() {
                debugger;
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("AndonVED.aspx/LoginValidation") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: "{userName: '" + $("#").val() + "', password: '" + $("#").val() + "'}",
                    success: function (response) {
                        var dataitem = response.d;
                        if (dataitem != undefined) {
                            if (dataitem.toLowerCase() == "valid") {
                                window.location.href = "AndonSettingsVED.aspx";
                            }
                            else {

                            }
                        }
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
            }

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

                $(".HourlyTargetCount-table tbody tr").each(function () {
                    debugger;
                    $(this).find(".lvInnerListView tbody tr td").css("border", "0px");
                });
                $(".HourlyTargetCount-table tbody tr").each(function () {
                    debugger;
                    $(this).find(".lvInnerListView tbody tr td").css("border-right", "1px solid silver");
                });
                $(".HourlyTargetCount-table tbody tr:gt(3)").each(function () {
                    debugger;
                    $(this).find(".lvInnerListView tbody tr td").css("background-color", "white");
                });
                $("video").each(function () {
                    this.addEventListener('ended', myHandler, false);
                    this.addEventListener('error', myHandler, false);
                });
            });
        </script>
    </form>
</body>
</html>

<%--<asp:ListView runat="server" ID="lvGroupKPIContainer" OnPagePropertiesChanging="lvGroupKPIContainer_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <table>
                                                            <thead>
                                                                <div class="container-group-KPI"></div>
                                                            </thead>
                                                            <tbody>
                                                                <tr runat="server" id="itemplaceholder"></tr>
                                                                <tr runat="server" style="position: sticky; bottom: -1px;">
                                                                    <td runat="server" colspan="6" style="border: 0px; text-align: center">
                                                                        <asp:DataPager runat="server" PageSize="500" ID="DataPager1">
                                                                            <Fields>
                                                                                <asp:NextPreviousPagerField ButtonCssClass="previous round" ShowFirstPageButton="false" ShowNextPageButton="false" ShowPreviousPageButton="true" PreviousPageText="&laquo;" />
                                                                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="next round" ShowFirstPageButton="false" ShowNextPageButton="true" ShowPreviousPageButton="false" ShowLastPageButton="false" NextPageText="&raquo;" />
                                                                            </Fields>
                                                                        </asp:DataPager>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                </asp:ListView>--%>