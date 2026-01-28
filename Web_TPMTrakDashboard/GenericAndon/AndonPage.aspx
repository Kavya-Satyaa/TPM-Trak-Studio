<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonPage.aspx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.AndonPage" %>

<!DOCTYPE html>
<%@ Register Src="~/GenericAndon/CockpitControl.ascx" TagPrefix="uc" TagName="Cockpit" %>
<%@ Register Src="~/GenericAndon/SlideShowControl.ascx" TagPrefix="uc" TagName="SlideShow" %>
<%@ Register Src="~/GenericAndon/ScheduleKTAControl.ascx" TagPrefix="uc" TagName="ScheduleKTA" %>
<%@ Register Src="~/GenericAndon/PoojaCastingMeltingControl.ascx" TagPrefix="uc" TagName="PoojaCastingMelting" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Production Andon</title>
    <meta name="description" content="TPM Trak Analytics web application">
    <meta name="author" content="GeeksLabs">
    <meta name="keyword" content="Creative, Dashboard, Admin, Template, Theme, Bootstrap, Responsive, Retina, Minimal">
    <%-- <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/TextAnimation.css" rel="stylesheet" />

    <script src="Scripts/Highchart8/highcharts.js"></script>
    <script src="Scripts/Highchart8/pareto.js"></script>
    <script src="Scripts/Highchart8/exporting.js"></script>
    <script src="Scripts/Highchart8/export-data.js"></script>
    <script src="Scripts/Highchart8/accessibility.js"></script>

    <script src="Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>--%>
    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.min.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/Site.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/TextAnimation.css" rel="stylesheet" />
    <script src="../GEA/Andon_GEA/Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <style>
        * {
            font-family: <%= settings.FontFamily %>;
            font-style: <%= settings.FontStyle %>;
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

        #tblComputer > tbody > tr:nth-child(1) {
            text-align: center;
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

        .Scrolltd {
            text-align: left !important;
        }

        #tblComputer > tbody > tr > td {
            padding: 2px;
        }


        .ScrollText {
            -moz-transform: translateX(100%);
            -webkit-transform: translateX(100%);
            transform: translateX(100%);
            -moz-animation: my-animation 2s linear infinite;
            -webkit-animation: my-animation 2s linear infinite;
            animation: my-animation 2s linear infinite;
        }

        .ScrollTextSchedule {
            -moz-transform: translateX(100%);
            -webkit-transform: translateX(100%);
            transform: translateX(100%);
            -moz-animation: my-animation 10s linear infinite;
            -webkit-animation: my-animation 10s linear infinite;
            animation: my-animation 10s linear infinite;
        }

        #tableSettings > tbody > tr > td {
            padding: 2px;
            width: 20px;
        }

        #tdchkDesktop input {
            width: 20px;
            height: 20px;
        }

        .panel-filter {
            position: absolute;
            top: 65px;
            right: 25px;
            width: 200px;
            padding: 10px;
            background-color: #fff;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.3);
            transition: opacity 0.2s ease-in-out, visibility 0.2s ease-in-out;
        }

        .td-center-align {
            vertical-align: middle !important;
            text-align: left !important;
        }

        .panel-filter:before {
            content: "";
            position: absolute;
            top: -10px;
            right: 10px;
            width: 0;
            height: 0;
            border-left: 10px solid transparent;
            border-right: 10px solid transparent;
            border-bottom: 10px solid #ccc;
        }

        @-moz-keyframes my-animation {
            from {
                -moz-transform: translateX(100%);
            }

            to {
                -moz-transform: translateX(-100%);
            }
        }

        @-webkit-keyframes my-animation {
            from {
                -moz-transform: translateX(100%);
                -webkit-transform: translateX(100%);
                transform: translateX(100%);
            }

            to {
                -moz-transform: translateX(-100%);
                -webkit-transform: translateX(-100%);
                transform: translateX(-100%);
            }
        }
    </style>

    <script>
        var dataRefreshTimer;
        $(document).ready(function () {
            setDivWidthHeight();
            //debugger;
            if (!($('#ComputerDiv').is(':visible'))) {
                setDataRefreshTimer();
            }
            else {
                $('#userControlContainer').css("display", "none");
            }
            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();
            //$("#panelFilter").show();
        });
        function setDivWidthHeight() {
            var width = $(window).width();
            //$('#scrollTextDiv').css("width", width - 70);
            var h = $('#footerDiv').height();
            if (h == undefined) {
                h = 0;
            }
            $('#userControlContainer').css("height", $(window).height() - $('#headerDiv').height() - h - 30);
        }
        function setDataRefreshTimer() {
            clearTimeout(dataRefreshTimer);
            dataRefreshTimer = setTimeout(insertLatestDataToMainCache,<%= settings.DataRefreshInterval %>);
            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();
        }
        function insertLatestDataToMainCache() {
            $.ajax({
                async: true,
                type: "POST",
                url: "AndonPage.aspx/insertLatestDataToMainCacheMemory",
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

        var flipInterval = 5000;
        var countItem = 0;

        function showImageVideo() {

            $('.carousel').carousel({
                pause: "false",
                interval: flipInterval
            });
            //debugger;
            if (document.getElementById('slideShowContainer') != null) {
                countItem = 0;
                countItem++;
                $("#myCarousel").on('slid.bs.carousel', function () {
                    countItem++;
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
                    //debugger;
                    if (countItem == $("#myCarousel .item").length) {
                        if (!isVideo) {
                            setTimeout(function () {

                                $("#myCarousel").carousel("pause");
                                __doPostBack('<%= btnPost.UniqueID%>', '');
                            }, 2000);

                        }
                    }
                });
                //debugger;
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
            // What you want to do after the event
            //if (countItem == 0 || countItem == $("#myCarousel .item").length) {
            //debugger;
            if ($("#myCarousel .item").length == 1) {

                $("#myCarousel").carousel("pause");
                __doPostBack('<%= btnPost.UniqueID%>', '');
                return;

            }
            if ($("#myCarousel .item").length > 0) {
                if (countItem == $("#myCarousel .item").length) {
                    //debugger;
                    $("#myCarousel").carousel("pause");
                    __doPostBack('<%= btnPost.UniqueID%>', '');
                    return;
                }
            }
            $("#myCarousel").carousel("cycle");
        }

        function setFlipIntervalKTA() {
            $("#hdnWidth").val("");
            $("#hdntdHeight").val("");
            $("#hdntotalWidth").val("");
            $("#hdnScreenHeight").val("");
            var h = $('#footerDiv').height();
            if (h == undefined) {
                h = 0;
            }
            let cellLabelDivHeight = $(".cellLabelDiv").height();
            if (cellLabelDivHeight == undefined) {
                cellLabelDivHeight = 0;
            }
            $('#scheduleKTAContainer').css("height", $(window).height() - $('#headerDiv').height() - cellLabelDivHeight - h) - 15;
            $("#hdnScreenHeight").val($("#scheduleKTAContainer").height());
            setDivsizeKTA();
            SetIconicBoxWidthKTA();

            var divH = Math.max.apply(null, $('#scheduleKTAContainer .myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());
            var divW = Math.max.apply(null, $('#scheduleKTAContainer .myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            $("#hdnWidth").val(divW);
            var tdH = Math.max.apply(null, $('#scheduleKTAContainer .myItem #tdComponentStatus').map(function () {
                return $(this).outerHeight(true);
            }).get());
            $("#hdntdHeight").val(tdH);

            let screenH = $('#scheduleKTAContainer').height();
            let screenW = $('#scheduleKTAContainer').width();
            let totalH = Math.floor(screenH / (divH));
            let totalW = Math.floor(screenW / (divW));
            let totalBox = Math.floor(totalH * totalW);
            $("#scheduleKTAContainer .myItem").hide();
            $("#scheduleKTAContainer .myItem").slice(0, totalBox).show();
            $("#hdntotalWidth").val(totalW);
            setCenterDivKTA();

            $.ajax({
                async: false,
                type: "POST",
                url: "AndonPage.aspx/setFlipIntervalToSession",
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

        //set schedulecontaiener height
        function setDivHeightKTA() {
            $('#scheduleKTAContainer').css("height", $(window).height() - $('#headerDiv').height() - $('#footerDiv').height()) - 15;
            $("#OuterDivContainer").css("height", $("#scheduleKTAContainer") + 20);
        }

        //Reduce Div size using Scrolling Text
        function setDivsizeKTA() {
            if ($("#hdnWrapContent").val() == "1") {
                var list = $('#scheduleKTAContainer .myItem');
                var maxLength = 40;
                for (var i = 0; i < list.length; i++) {
                    var table = list[i].querySelector("#tblComponentList");
                    var listtd = table.querySelectorAll("#tdComponentStatus");
                    if (listtd.length > 0) {
                        for (var j = 0; j < listtd.length; j++) {
                            var Componenttd = listtd[j];
                            Componenttd.classList.add("td-center-align");
                            var componentStatus = listtd[j].querySelector("#lblComponentStatus").innerHTML.trim();;
                            if (componentStatus != "") {
                                if (componentStatus.length <= maxLength) {
                                    listtd[j].querySelector("#lblComponentStatus").innerHTML = componentStatus;
                                    continue;
                                }
                                listtd[j].querySelector("#lblComponentStatus").innerHTML = componentStatus.slice(0, maxLength) + '</br>' + componentStatus.slice(maxLength);
                                //Add Scrolling Element
                                //var DivElement = document.createElement("div");
                                //DivElement.setAttribute("id", "divScrolling");
                                //DivElement.setAttribute("class", "ScrollContainer");
                                //var LabelElement = document.createElement("label");
                                //LabelElement.setAttribute("id", "lblComponentStatusScrolling");
                                //LabelElement.setAttribute("class", "ScrollTextSchedule");
                                //DivElement.appendChild(LabelElement);
                                //listtd[j].insertBefore(DivElement, listtd[j].children[1]);
                                //listtd[j].querySelector("#lblComponentStatus").innerHTML = componentStatus.slice(0, fixedLabelLength);
                                //listtd[j].querySelector("#lblComponentStatusScrolling").innerHTML = componentStatus.slice(fixedLabelLength);
                            }
                            //if (listtd[j].querySelector("#lblScrolling").innerHTML != "") {
                            //    listtd[j].classList.add("Scrolltd");
                            //    listtd[j].querySelector("#divScrolling").classList.add("ScrollContainer");
                            //    listtd[j].querySelector("#lblScrolling").classList.add("ScrollTextSchedule");
                            //}
                        }
                    }
                }
            }
            setIconicBoxtdHeightKTA();
        }

        function SetIconicBoxWidthKTA() {
            if ($("#hdnWidth").val() == "") { //first flip
                var divW = Math.max.apply(null, $('#scheduleKTAContainer .myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                $("#scheduleKTAContainer .myItem").width(divW - 10);
                $("#hdnWidth").val(divW - 10);
            }
            else {
                $("#scheduleKTAContainer .myItem").width($("#hdnWidth").val());
                $("#scheduleKTAContainer").height($("#hdnScreenHeight").val());
            }
            if ($("#hdntotalWidth").val() != "") {
                setCenterDivKTA(); //To center Schedule Container
            }
        }
        //Set uniform td height
        function setIconicBoxtdHeightKTA() {
            if ($("#hdnWrapContent").val() == "1") {
                if ($("#hdntdHeight").val() == "") {
                    var tdH = Math.max.apply(null, $('#scheduleKTAContainer .myItem #tdComponentStatus').map(function () {
                        return $(this).outerHeight(true);
                    }).get());
                    $('#scheduleKTAContainer .myItem #tdComponentStatus').height(tdH);
                    $("#hdntdHeight").val(tdH);

                }
                else {
                    $('#scheduleKTAContainer .myItem #tdComponentStatus').height($("#hdntdHeight").val());
                }
            }
        }

        function setCenterDivKTA() {
            debugger;
            var divW = Math.max.apply(null, $('#scheduleKTAContainer .myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            var totalW = $("#hdntotalWidth").val();
            $("#scheduleKTAContainer").width((Math.ceil(divW) + 7) * totalW);

            $("#OuterDivContainer").addClass("OuterDivContainerStyle");
        }

        function setFlipIntervalPooja() {
            let totalBox;
            if ($("[id$=hdnviewType]").val() == "Cockpit") {

                //debugger;
                $("[id$=PoojaCastingMeltingCockpit] ").css("height", $(window).height() - $('#headerDiv').height() - $('#footerDiv').height()) - 15;
                SetIconicBoxWidthPooja();
                //debugger;
                var divH = Math.max.apply(null, $('[id$=PoojaCastingMeltingCockpit] .myItem').map(function () {
                    return $(this).outerHeight(true);
                }).get());
                var divW = Math.max.apply(null, $('[id$=PoojaCastingMeltingCockpit] .myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                //let screenH = divH;
                let screenH = $("[id$=PoojaCastingMeltingCockpit]").height();
                let screenW = $("[id$=PoojaCastingMeltingCockpit]").width();
                let totalH = Math.floor(screenH / (divH));
                let totalW = Math.floor(screenW / (divW));
                totalBox = Math.floor(totalH * totalW);
                $("[id$=PoojaCastingMeltingCockpit] .myItem").hide();
                $("[id$=PoojaCastingMeltingCockpit] .myItem").slice(0, totalBox).show();
            }
            else if ($("[id$=hdnviewType]").val() == "Table") {
                var footerHeight = $('#footerDiv').height();
                if (footerHeight == undefined) {
                    footerHeight = 0;
                }
                $('[id$=PoojaCastingMeltingTable]').css("height", $(window).height() - $('#headerDiv').height() - footerHeight) - 15;
                SetIconicBoxWidthKTA();
                var divH = Math.max.apply(null, $('[id$=PoojaCastingMeltingTable] .myItem').map(function () {
                    return $(this).outerHeight(true);
                }).get());
                var divW = Math.max.apply(null, $('[id$=PoojaCastingMeltingTable] .myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                let screenH = $("[id$=PoojaCastingMeltingTable]").height();
                let screenW = $("[id$=PoojaCastingMeltingTable]").width();
                let totalH = Math.floor(screenH / (divH));
                let totalW = Math.floor(screenW / (divW));
                totalBox = Math.floor(totalH * totalW);
                $("[id$=PoojaCastingMeltingTable] .myItem").hide();
                $("[id$=PoojaCastingMeltingTable] .myItem").slice(0, totalBox).show();
            }
            //debugger;
            //$.ajax({
            //    async: false,
            //    type: "POST",
            //    url: "AndonPage.aspx/setFlipIntervalToSession",
            //    contentType: "application/json; charset=utf-8",
            //    crossDomain: true,
            //    dataType: "json",
            //    data: '{displayedItemCount:"' + totalBox + '"}',
            //    success: function (response) {
            //    },
            //    error: function (Result) {
            //    }
            //});
        }
        function SetIconicBoxWidthPooja() {
            //debugger;
            if ($("[id$=hdnviewType]").val() == "Cockpit") {
                var divW = Math.max.apply(null, $('[id$=PoojaCastingMeltingCockpit] .myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                $("[id$=PoojaCastingMeltingCockpit] .myItem").width(divW);
            }
            else if ($("[id$=hdnviewType]").val() == "Cockpit") {
                var divW = Math.max.apply(null, $('[id$=PoojaCastingMeltingTable] .myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                $("[id$=PoojaCastingMeltingTable] .myItem").width(divW);
            }
        }


        function ShowPanelFilter() {
            debugger;
            if ($("#hdnFilterShowHide").val() == "Show") {
                $("#panelFilter").show();
                $("#hdnFilterShowHide").val("Hide")
            }
            else {
                $("#panelFilter").hide();
                $("#hdnFilterShowHide").val("Show")
            }
            return false;
        }

        function hidePanelFilter() {
            $("#panelFilter").hide();
            $("#hdnFilterShowHide").val("Show")
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:Timer ID="imageVideoInterval" runat="server" OnTick="imageVideoInterval_Tick"></asp:Timer>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <%--  <asp:Timer ID="dataRefreshInterval" runat="server" OnTick="dataRefreshInterval_Tick"></asp:Timer>--%>

                <div class="row">
                    <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 5px; background-color: #3777bc" id="headerDiv">
                        <div class="HeaderImage" style="height: 60px">
                            <asp:Image ID="customerLogo" runat="server" class="img-responsive img-rounded" Style="width: 200px; height: 56px; margin-top: 2px" />
                        </div>
                        <label id="headerName" runat="server" clientidmode="static" style="color: white; font-weight: bold; font-size: 35px; text-align: right; margin-top: 5px; vertical-align: middle">Production Status</label>
                        <div style="float: right; position: relative; display: inline-flex">
                            <div style="padding-right: 15px;">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ClientIDMode="Static" ID="lblShift" CssClass="headerRight"></asp:Label>
                                            <label class="headerRight" runat="server" id="lblDateTime" clientidmode="static" style="display: inline-block"></label>
                                        </td>
                                    </tr>
                                    <tr style="padding-bottom: 3px;">
                                        <td runat="server" id="tdchkDesktop" style="width: 20%; text-align: left;">
                                            <asp:CheckBox runat="server" ID="chkDesktopView" OnCheckedChanged="chkDesktopView_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td style="padding-top: 0px; padding-bottom: 5px; display: contents; text-align: left; width: 80%;">
                                            <asp:DropDownList ID="ddlScreenName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScreenName_SelectedIndexChanged" CssClass="form-control" Style="display: inline-block; width: 150px;"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <div style="padding-top: 5px; padding-right: 0px;">
                                <table id="tableSettings" style="width: 60px;">
                                    <tr>
                                        <td runat="server" id="tdAndonSettings" onclick="location.href='AndonSettings.aspx';">
                                            <i class="glyphicon glyphicon-cog" style="font-size: 20px; color: white"></i>
                                        </td>
                                        <td runat="server" id="tdHomeReset" onclick="location.href='AndonPage.aspx';">
                                            <i class="glyphicon glyphicon-home" style="font-size: 20px; color: white;"></i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdPanelFilters" onclick="ShowPanelFilter();">
                                            <i class="glyphicon glyphicon-filter" style="font-size: 20px; color: white;"></i>
                                        </td>
                                        <td>
                                            <span style="font-size: 16px; cursor: pointer; color: white; vertical-align: text-top" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <asp:HiddenField ID="hdnFilterShowHide" runat="server" ClientIDMode="Static" />
                            <div class="panel panel-default panel-subitems panel-table-style panel-filter" id="panelFilter" style="width: 500px;">
                                <div class="triangle-right"></div>
                                <div class="panel-heading">
                                    <span class="filter-header-name">Filter</span>
                                    <button type="button" class="close" aria-label="Close" onclick="hidePanelFilter();">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>

                                <div class="panel-body">
                                    <div>
                                        <table style="width: 100%; table-layout: fixed;" id="tblfilter">
                                            <tr>
                                                <td style="padding-right: 2px; padding-bottom: 2px;">
                                                    <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" Style="margin-top: 0px" ToolTip="Plant">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="padding-right: 2px; padding-bottom: 2px;">
                                                    <asp:DropDownList runat="server" ID="ddlCellID" ClientIDMode="Static" CssClass="form-control" Style="margin-top: 0px" ToolTip="Cell">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="padding-bottom: 2px;">
                                                    <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control" Style="margin-top: 0px" ToolTip="Frequency">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <table style="width:100%">
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button runat="server" ID="btnOKSetting" OnClick="btnOKSetting_Click" ClientIDMode="Static" CssClass="btn btn-primary" Text="Save" />
                                                <input type="button" onclick="hidePanelFilter();" value="Cancel" class="btn btn-secondary" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="userControlContainer" runat="server" clientidmode="static" style="margin-top: 15px;">
                        <uc:Cockpit runat="server" ID="cockpitControl"></uc:Cockpit>
                        <uc:SlideShow runat="server" ID="slideShowControl" />
                        <uc:ScheduleKTA runat="server" ID="ScheduleKTAControl" />
                        <uc:PoojaCastingMelting runat="server" ID="PoojaCastingMeltingControl" />
                    </div>
                    <asp:Button runat="server" ID="btnPost" ClientIDMode="Static" OnClick="btnPost_Click" Style="display: none" />
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="dataRefreshInterval" />--%>
                <asp:AsyncPostBackTrigger ControlID="imageVideoInterval" />
                <asp:PostBackTrigger ControlID="ddlFrequency" />
            </Triggers>
        </asp:UpdatePanel>

        <footer style="display: <%= settings.ShowFooterBlock %>;">
            <div class="navbar navbar-default navbar-fixed-bottom footerBottom" style="padding: 0px 5px; background-color: #3777bc; height: 2px; text-align: center" id="footerDiv">
                <div style="float: left;">
                    <p style="color: #fcefef; font-style: italic; margin-top: 10px; font-size: 16px; display: inline-block">Powered by TPM-Trak®</p>
                </div>
                <div id="scrollTextDiv" style="display: <%= settings.ShowMsgBox %>; margin-top: 4px; width: 85%">
                    <marquee style="font-family: Book Antiqua; color: #FFFFFF; font-size: 25px; background-color: #0f4987" scrollamount="10" loop="infinite" runat="server" id="scrollingText"></marquee>
                </div>
                <div style="float: right;">
                    <img src="../GEA/Andon_GEA/Images/AMiT.jpg" height="43" width="70" />
                </div>
            </div>
        </footer>

        <div class="" id="ComputerDiv" runat="server">
            <div class="modal-dialog">
                <fieldset style="border-color: black" id="lblfieldset">
                    <legend class="legendStyleSetting">Computer Name</legend>
                    <table id="tblComputer" style="width: 95%; margin-top: 10px;">

                        <tr>
                            <td><span style="font-size: medium; font-weight: 600;">Computer Name</span></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtComputerName" CssClass="form-control focus"></asp:TextBox>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button runat="server" ID="BtnSave" Text="Save" CssClass="btnSave" OnClick="BtnSave_Click1" />
                            </td>
                        </tr>
                    </table>

                </fieldset>
            </div>
        </div>
    </form>
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setDivWidthHeight();
                $('[data-toggle="tooltip"]').tooltip({
                    placement: 'bottom'
                });
                $("#hdnFilterShowHide").val("Hide");
                ShowPanelFilter();
            });
            $("video").each(function () {
                this.addEventListener('ended', myHandler, false);
                this.addEventListener('error', myHandler, false);
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
</body>
</html>
