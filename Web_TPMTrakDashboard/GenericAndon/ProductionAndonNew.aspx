<%@ Page Language="C#" Title="Production Andon" AutoEventWireup="true" CodeBehind="ProductionAndonNew.aspx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.ProductionAndonNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

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

        .Green {
            /*background-color: green;*/
            border-radius: 15px;
            border: 0.1px solid #cccccc;
        }

        .Red {
            /*background-color: red;*/
            border-radius: 15px;
            border: 0.1px solid #cccccc;
        }

        .Yellow {
            /*background-color: yellow;*/
            border-radius: 15px;
            border: 0.1px solid #cccccc;
        }

        .white {
            border-radius: 15px;
            border: 1px solid #cccccc;
        }

        .Running {
            -webkit-animation: cog-rotate 2s linear infinite;
            -moz-animation: cog-rotate 2s linear infinite;
            -o-animation: cog-rotate 2s linear infinite;
            animation: rotate 2s linear infinite;
            color: green;
        }

        .Stopped {
            color: red;
        }

        .PDT {
            color: blue;
        }

        .addBorder {
            border-radius: 15px;
            border: 0.5px solid #504e4e;
            background-color: white;
            box-shadow: 0px 0px 2px grey;
        }

        .removeBorder {
            border-radius: 5px;
            border: 0.5px solid #504e4e;
            background-color: white;
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

        .cockpit > tbody > tr > td {
            padding: 1px;
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
            width: 33%;
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
        #scrollTextDiv{
            width: 80%;
        }

        @media screen and (min-resolution: 125dpi) {
            .headerRight {
                color: white;
                font-weight: 600;
                font-size: 12px;
                margin: 0px;
            }
            #scrollTextDiv{
                width: 75%
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
                $('#MainContainer').css("display", "none");
            }
            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();

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
            //$("#panelFilter").show();
        });
        function setDivWidthHeight() {
            var width = $(window).width();
            //$('#scrollTextDiv').css("width", width - 70);
            var h = $('#footerDiv').height();
            if (h == undefined) {
                h = 0;
            }
            $('#MainContainer').css("height", $(window).height() - $('#headerDiv').height() - h - 30);
        }
        function setDataRefreshTimer() {
            //debugger;
            clearTimeout(dataRefreshTimer);
            dataRefreshTimer = setTimeout(insertLatestDataToMainCache,<%= settings.DataRefreshInterval %>);
            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();
        }
        function insertLatestDataToMainCache() {
            //debugger;
            $.ajax({
                async: true,
                type: "POST",
                url: "ProductionAndonNew.aspx/insertLatestDataToMainCacheMemory",
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

        var scalefactor = 1.0;
        if (window.devicePixelRatio >= 1.25) {
            scalefactor = 1.0 / window.devicePixelRatio;
        }

        function setFlipInterval() {
            $("#hdnWidth").val("");
            $("#hdntotalWidth").val("");

            let footerHeight = $('#footerDiv').height();
            if (footerHeight == undefined) {
                footerHeight = 0;
            }
            let cellLabelDivHeight = $(".cellLabelDiv").height();
            if (cellLabelDivHeight == undefined) {
                cellLabelDivHeight = 0;
            }
            $('#cockpitContainer').css("height", $(window).height() - $('#headerDiv').height() - cellLabelDivHeight - footerHeight - 10);
            $('#cockpitContainer').css("width", $(window).width() - 10);
            // $("#OuterDivContainer").css("height", $("#cockpitContainer").height());

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
            setCenterDiv();

            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionAndonNew.aspx/setFlipIntervalToSession",
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
            debugger;
            var divW;
            setDivWidthHeight();
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
            $("#txtBoxWidth").val("Box width= " + Math.round(Number(divW), 3).toString() + "px / " + (Number($('#cockpitContainer').width()) - 7).toString() + "px");
            $("#txtBoxWidth").attr("title", "Box width= Width of each Box / Total Available Width \n Box width=" + Math.round(Number(divW), 3).toString() + "px / " + (Number($('#cockpitContainer').width()) - 7).toString() + "px");

            if ($("#hdnHeight").val() == "" || $("#hdnHeight").val() == undefined) {
                var divH = Math.max.apply(null, $('.myItem .cockpit').map(function () {
                    return $(this).outerHeight(true);
                }).get());
                $("#hdnHeight").val(divH);
            }
            $("#cockpitContainer .myItem .cockpit").height($("#hdnHeight").val());
            if ($("#hdntotalWidth").val() != "") {
                setCenterDiv();
            }
        }

        function setCenterDiv() {
            //var divW = Math.max.apply(null, $('#cockpitContainer .myItem').map(function () {
            //    return $(this).outerWidth(true);
            //}).get());
            //var totalW = $("#hdntotalWidth").val();
            //$("#cockpitContainer").width((Math.ceil(divW) + 5) * totalW);
            $("#OuterDivContainer").addClass("OuterDivContainerStyle");
        }
        function ShowPanelFilter() {
            //debugger;
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

        function BindCellID() {
            debugger;
            var PlantID = $("#ddlPlantID").val();

            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionAndonNew.aspx/GetCellIDsPlantWise",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: '{plantID: "' + PlantID + '"}',
                success: function (response) {
                    $("#ddlCellID option").remove();
                    var selector = document.getElementById("ddlCellID");

                    if (response.d.length > 1) {
                        var option = document.createElement("option");
                        option.text = "Select All";
                        option.value = "All";
                        selector.add(option);
                    }

                    response.d.forEach(function (item, index, array) {
                        var option = document.createElement("option");
                        option.text = array[index];
                        option.value = array[index];
                        selector.add(option);
                    });
                },
                error: function (Result) {
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdnWidth" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnHeight" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdntotalWidth" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnUseCustomWidthFlag" ClientIDMode="Static" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Timer runat="server" ID="PageRefreshtimer" ClientIDMode="Static" OnTick="timer_Tick"></asp:Timer>
                <asp:HiddenField runat="server" ID="hdnRefreshTimer" ClientIDMode="Static" />
                <div class="row text-center">
                    <asp:Label runat="server" ID="lbltestMsg" ClientIDMode="Static"></asp:Label>
                    <div class="navbar navbar-default navbar-fixed-top text-center headerDiv" style="padding: 0px 5px; background-color: #3777bc; height: 65px;">
                        <asp:Label ID="headerName" runat="server" ClientIDMode="static" Style="color: white; font-weight: bold; font-size: 35px; text-align: right; margin-top: 5px; vertical-align: middle">Production Status</asp:Label>
                    </div>
                    <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 5px; background-color: transparent" id="headerDiv">
                        <div class="HeaderImage" style="height: 65px">
                            <asp:Image ID="customerLogo" runat="server" class="img-responsive img-rounded" Style="width: 200px; height: 56px; margin-top: 2px" />
                        </div>
                        <div style="float: right; position: relative; display: inline-flex;" runat="server" id="divHeader">
                            <div style="padding-right: 15px;">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ClientIDMode="Static" ID="lblShift" CssClass="headerRight"></asp:Label>
                                            <label class="headerRight" runat="server" id="lblDateTime" clientidmode="static" style="display: inline-block"></label>
                                        </td>
                                    </tr>
                                    <tr style="padding-bottom: 3px;">
                                        <td runat="server" id="tdchkDesktop" style="width: 20%; text-align: left;" title="Switch to Desktop Mode">
                                            <asp:CheckBox runat="server" ID="chkDesktopView" OnCheckedChanged="chkDesktopView_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" placeholder="Current box width" title="BoxWidth+Margin/Total Avail. Width" ClientIDMode="Static" ID="txtBoxWidth" AutoCompleteType="Disabled" ReadOnly="true" CssClass="form-control"></asp:TextBox>
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
                                        <td runat="server" id="tdHomeReset" onclick="location.href='ProductionAndonNew.aspx';">
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
                                    <div style="align-content: center; position: relative;">
                                        <table style="width: 100%; table-layout: fixed;" id="tblfilter">
                                            <tr>
                                                <td style="padding-right: 2px; padding-bottom: 2px;">
                                                    <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" onchange="return BindCellID();" AutoPostBack="true" Style="margin-top: 0px" ToolTip="Plant">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="padding-right: 2px; padding-bottom: 2px;">
                                                    <asp:DropDownList runat="server" ID="ddlCellID" ClientIDMode="Static" CssClass="form-control" Style="margin-top: 0px" ToolTip="Cell">
                                                    </asp:DropDownList>
                                                </td>
                                                <%-- <td style="padding-bottom: 2px;">
                                                    <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control" Style="margin-top: 0px" ToolTip="Frequency">
                                                    </asp:DropDownList>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <table style="width: 100%">
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
                    <div id="MainContainer" runat="server" clientidmode="static" style="margin-top: 10px;">
                        <div style="text-align: center" class="cellLabelDiv" runat="server" id="cellLabelDiv">
                            <asp:Label runat="server" ID="lblCellName" CssClass="cellLabel"></asp:Label>
                        </div>
                        <div id="ProductionAndonContainer" class="ProductionAndonContainer">
                            <%--style="margin-top: 10px;"--%>
                            <div id="OuterDivContainer">
                                <div id="cockpitContainer" class="OuterDivContainerStyle">
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
                                                                    <%-- <asp:LinkButton ID="lnkMachine" runat="server" CommandArgument='<%# Eval("MachineId") %>'><%# Eval("MachineId")%></asp:LinkButton>--%>
                                                                    <label id="lblMachineID" style="font-size: <%# Eval("MachineFontSize")%>px;" clientidmode="static"><%# Eval("MachineId") %></label>
                                                                </td>
                                                                <td style="background-color: transparent; width: 35px;">
                                                                    <%-- <asp:Image ImageUrl='<%# Eval("StatusImage") %>' runat="server"
                                                    Visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?true:false %>' meta:resourcekey="ImageResource1" />--%>
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
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="PageRefreshtimer" />
                <%--<asp:PostBackTrigger ControlID="ddlFrequency" />--%>
                <%--<asp:PostBackTrigger ControlID="ddlCellID" />--%>
            </Triggers>
        </asp:UpdatePanel>

        <footer style="display: <%= settings.ShowFooterBlock %>;">
            <div class="navbar navbar-default navbar-fixed-bottom footerBottom" style="padding: 0px 5px; background-color: #3777bc; height: 2px; text-align: center; width: 100%;" id="footerDiv">
                <div style="float: left;">
                    <p style="color: #fcefef; font-style: italic; margin-top: 10px; font-size: 16px; display: inline-block">Powered by TPM-Trak®</p>
                </div>
                <div id="scrollTextDiv" style="display: <%= settings.ShowMsgBox %>; margin-top: 4px;">
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
    </form>
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();
            $(document).ready(function () {
                setDivWidthHeight();
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

            $(document).mouseup(function (e) {
                var container = $("#panelFilter");
                if (!container.is(e.target) && container.has(e.target).length === 0) {
                    $("#hdnFilterShowHide").val("Hide")
                    ShowPanelFilter();
                }
            });
        })
    </script>
</body>
</html>
