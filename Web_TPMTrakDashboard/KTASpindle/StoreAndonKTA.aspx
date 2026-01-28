<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreAndonKTA.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.StoreAndonKTA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Andon</title>
    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.min.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/Site.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/TextAnimation.css" rel="stylesheet" />
    <script src="../GEA/Andon_GEA/Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <style>
        * {
            font-family: <%= andonSetting.FontFamily %>;
            font-style: <%= andonSetting.FontStyle %>;
        }

        .cellLabelDiv {
            background-color: #0f4987;
            padding: 5px 2px 0px 2px;
            box-shadow: 0 0 10px black;
        }

            .cellLabelDiv span {
                color: white;
                font-weight: bold;
                font-size: 25px;
                font-family: sans-serif;
            }

        .innertable tr td {
            font-weight: bold;
            padding: 2px !important;
            vertical-align: middle;
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

        .headerDiv {
            position: fixed;
            top: 0;
            background: #1a2732;
            box-shadow: 0 0 10px black;
        }

        .OuterDivContainerStyle {
            display: flex;
            justify-content: center;
            align-content: center;
        }

        .ScrollContainer {
            white-space: nowrap;
            width: 150px;
            overflow: hidden;
            display: inline-flex;
        }

        .NoScrollContainer {
            width: 0px;
            overflow: hidden;
            display: inline-flex;
            white-space: nowrap;
        }

        .ScrollText {
            -moz-transform: translateX(100%);
            -webkit-transform: translateX(100%);
            transform: translateX(100%);
            -moz-animation: my-animation 10s linear infinite;
            -webkit-animation: my-animation 10s linear infinite;
            animation: my-animation 10s linear infinite;
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

        .panel-filter {
            position: absolute;
            top: 65px;
            right: 86px;
            width: 200px;
            padding: 10px;
            background-color: #fff;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.3);
            transition: opacity 0.2s ease-in-out, visibility 0.2s ease-in-out;
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

        #chkDesktopView {
            width: 20px;
            height: 20px;
        }

        .innerDIV::-webkit-scrollbar {
            border-bottom-right-radius: 20px;
            border-bottom-left-radius: 20px;
            width: 5px;
            padding-bottom: 10px;
        }

        .innerDIV::-webkit-scrollbar-thumb {
            background-color: transparent;
            transition: background-color 0.3s;
            border-bottom-right-radius: 15px;
            border-bottom-left-radius: 15px;
        }

        .innerDIV::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            border-bottom-right-radius: 15px;
            border-bottom-left-radius: 15px;
        }

        .innerDIV:hover::-webkit-scrollbar {
            width: 8px;
            background-color: #f1f1f1;
            border-bottom-right-radius: 15px;
            border-bottom-left-radius: 15px;
        }

        .innerDIV:hover::-webkit-scrollbar-thumb {
            background-color: #b9b9b9;
            transition: background-color 0.2s;
            border-bottom-right-radius: 15px;
            border-bottom-left-radius: 15px;
        }

        .innerDIV::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            border-bottom-right-radius: 10px;
            border-bottom-left-radius: 10px;
        }
    </style>
    <script>
        var dataRefreshTimer;
        $(document).ready(function () {
            setDivWidthHeight();
            setcolor();
            //setDataRefreshTimer();

            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();

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


        function setDivWidthHeight() {
            var width = $(window).width();
            $('#scrollTextDiv').css("width", width - 70 - 200);
            $('#Container').css("height", $(window).height() - $('#headerDiv').height());
        }
        function setDataRefreshTimer() {
            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();
            clearTimeout(dataRefreshTimer);
            dataRefreshTimer = setTimeout(insertLatestDataToMainCache,<%= andonSetting.DataDisplayInterval %>);
        }
        function insertLatestDataToMainCache() {
            $.ajax({
                async: true,
                type: "POST",
                url: "StoreAndonKTA.aspx/insertLatestDataToMainCacheMemory",
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
        var flipInterval = 5000;

        function setFlipInterval() {
            $("#hdnWidth").val("");
            $("#hdntdHeight").val("");
            $("#hdntotalWidth").val("");

            $('#StoreContainer').css("height", $(window).height() - $('#headerDiv').height()) - 15;
            SetIconicBoxWidth();
            setIconicBoxtdHeight();

            var divH = Math.max.apply(null, $('#StoreContainer .myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());
            var divW = Math.max.apply(null, $('#StoreContainer .myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());

            $("#hdnWidth").val(divW);
            var tdH = Math.max.apply(null, $('#StoreContainer .myItem .tdComponentStatus').map(function () {
                return $(this).outerHeight(true);
            }).get());
            $("#hdntdHeight").val(parseFloat(tdH) - 25);


            let screenH = $('#StoreContainer').height();
            let screenW = $('#StoreContainer').width();
            let totalH = Math.floor(screenH / (divH));
            let totalW = Math.floor(screenW / (divW));
            let totalBox = Math.floor(totalH * totalW);
            $("#StoreContainer .myItem").hide();
            $("#StoreContainer .myItem").slice(0, totalBox).show();
            $("#hdntotalWidth").val(totalW);
            setCenterDivKTA();

            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: "StoreAndonKTA.aspx/setFlipIntervalToSession",
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

        function SetIconicBoxWidth() {
            if ($("#hdntype").val() == "DestopView") {
                var divW = Math.max.apply(null, $('#StoreContainer .myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                $("#StoreContainer .myItem").width(divW);
            }
            else {
                if ($("#hdnWidth").val() == "") { //first flip
                    var divW = Math.max.apply(null, $('#StoreContainer .myItem').map(function () {
                        return $(this).outerWidth(true);
                    }).get());
                    $("#StoreContainer .myItem").width(divW);
                }
                else {
                    $("#StoreContainer .myItem").width($("#hdnWidth").val());
                }
            }

            if ($("#hdntotalWidth").val() != "") {
                //setCenterDivStore(); //To center Schedule Container
            }

            //var divW = Math.max.apply(null, $('#StoreContainer .myItem').map(function () {
            //    return $(this).outerWidth(true);
            //}).get());
            //$("#StoreContainer .myItem").width(divW);
        }
        function setIconicBoxtdHeight() {
            debugger;
            if ($("#hdntdHeight").val() == "") {
                var tdH = Math.max.apply(null, $('#StoreContainer .myItem .tdComponentStatus').map(function () {
                    return $(this).outerHeight(true);
                }).get());

                $('#StoreContainer .myItem .tdComponentStatus').height(parseFloat(tdH) - 25);
            }
            else {
                $('#StoreContainer .myItem .tdComponentStatus').height($("#hdntdHeight").val());
            }
        }
        function setCenterDivKTA() {
            debugger;
            var divW = Math.max.apply(null, $('#StoreContainer .myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            var totalW = $("#hdntotalWidth").val();
            $("#StoreContainer").width(Math.ceil(divW) * totalW + 20);

            $("#OuterDivContainer").addClass("OuterDivContainerStyle");
        }

        function setcolor() {
            var fontfamily ="<%=fontfamily %>"
            var fontstyle = "<%=fontstyle %>"
            $("#Container").css('font-family', fontfamily);
            $("#Container").css('font-style', fontstyle);
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

        $(document).mouseup(function (e) {
            var container = $("#panelFilter");

            // if the target of the click isn't the container nor a descendant of the container
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                $("#hdnFilterShowHide").val("Hide")
                ShowPanelFilter();
            }
        });
        function OpenCompOpDocumentView(URL) {
            window.open(URL, "ComponentOpDocumentView");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdntdHeight" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnWidth" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdntotalWidth" ClientIDMode="Static" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdntype" ClientIDMode="Static" />
                <asp:Timer runat="server" ID="timer" ClientIDMode="Static" OnTick="flipInterval_Tick"></asp:Timer>
                <div class="row text-center">
                    <div class="navbar navbar-fixed-top text-center" style="padding: 0px 5px; background-color: #3777bc; box-shadow: 0 0 10px black;" id="headerDiv">
                        <div class="HeaderImage" style="height: 60px">
                            <asp:Image ID="customerLogo" runat="server" class="img-responsive img-rounded" Style="width: 200px; height: 56px; margin-top: 2px" />
                        </div>
                        <label id="headerName" runat="server" clientidmode="static" style="color: white; font-weight: bold; font-size: 33px; text-align: right; margin-top: 5px; margin-left: 180px;">Store Andon</label>
                        <div style="float: right; position: relative; display: inline-flex">
                            <div style="text-align: left">
                                <asp:Label runat="server" ClientIDMode="Static" ID="lblShift" CssClass="headerRight"></asp:Label>&nbsp;&nbsp;&nbsp;
                                <p class="headerRight" runat="server" id="lblDateTime" clientidmode="static" style="display: inline-block">&nbsp;</p>

                                <div style="text-align: right;">
                                    <div style="display: inline-block; padding-right: 10px;">
                                        <asp:CheckBox runat="server" ID="chkDesktopView" OnCheckedChanged="chkDesktopView_CheckedChanged" AutoPostBack="true" ClientIDMode="Static" Enabled="false" />
                                    </div>
                                    <div runat="server" id="tdPanelFilters" onclick="ShowPanelFilter();" style="display: inline-block; padding-right: 10px">
                                        <i class="glyphicon glyphicon-filter" style="font-size: 20px; color: white;"></i>
                                    </div>
                                    <div runat="server" id="divAndonSettings" style="display: inline-block; padding-right: 10px">
                                        <div style="margin: 1px; height: 29px;" onclick="location.href='AndonSettingsKTA.aspx';">
                                            <p id="settingContainer" runat="server" clientidmode="static">
                                                <i class="glyphicon glyphicon-cog" style="font-size: 25px; color: white"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <p class="headerRight" style="display: inline-block;"><span style="font-size: 18px; cursor: pointer; color: white; vertical-align: text-top; padding-right: 10px" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span></p>
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
                                            <table>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td style="padding-right: 10px;">
                                                        <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" Style="margin-top: 0px" ToolTip="Plant">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlCellID" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged" AutoPostBack="true" Style="margin-top: 0px" ToolTip="Cell">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            &nbsp;&nbsp;
                       
                        </div>
                    </div>
                    <div id="Container" runat="server" clientidmode="static" style="margin-top: 7px;">
                        <div style="text-align: center" class="cellLabelDiv" runat="server" id="cellLabelDiv">
                            <asp:Label runat="server" ID="lblCellName" CssClass="cellLabel"></asp:Label>
                        </div>

                        <div id="OuterDivContainer">
                            <div id="StoreContainer">
                                <asp:ListView runat="server" ID="lvStoreAndonKTA" ItemPlaceholderID="placeHolder">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder runat="server" ID="placeHolder" />
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <div class="myItem" style="margin: 6px; min-width: 100px; display: inline-block; vertical-align: top">
                                            <div style="border: 1.9px solid #504e4e; border-radius: 8px;">
                                                <div style="padding: 4px;">
                                                    <table style='width: 100%;' class="outercockpit">
                                                        <tr style="background-color: #34b5df;">
                                                            <td style="text-align: center; color: black; font-weight: bold;">
                                                                <%--padding-left: 30px; padding-bottom: 5px;--%>
                                                                <label style="font-size: <%# Eval("MachineFontSize") %>px;"><%# Eval("Machine") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%--                                <table class="table table-bordered " style='background-color: white; margin-bottom: 4px; border-radius:8px;'>--%>
                                                    <asp:ListView ID="lvStoreAndon" runat="server" DataSource='<%# Eval("ComponentList") %>' ItemPlaceholderID="addressPlaceHolder">
                                                        <LayoutTemplate>
                                                            <div style="overflow: auto; max-height: 200px;" class="innerDIV">
                                                                <table class="table table-bordered innertable headerFixer" style="background-color: white; margin-bottom: 3px; text-align: left;">
                                                                    <tr runat="server" id="addressPlaceHolder"></tr>
                                                                </table>
                                                            </div>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="min-width: 100px; height: 30px; white-space: nowrap; font-weight: bold;" class="tdComponentStatus" clientidmode="static">

                                                                    <div style="display: inline-flex; font-size: <%# Eval("FontSizeInnerData") %>px">
                                                                        <asp:Label runat="server" ID="lblComponent" Visible='<%# Eval("LabelVisible")%>' Text='<%# Eval("Component")%>'></asp:Label>
                                                                    </div>
                                                                    <div style="display: inline-flex; font-size: <%# Eval("FontSizeInnerData") %>px">
                                                                        <asp:LinkButton runat="server" ID="lnkComponent" Text='<%# Eval("Component")%>' Visible='<%# Eval("LinkVisible")%>' OnClick="lnkComponent_Click"> </asp:LinkButton>
                                                                    </div>

                                                                   <%-- <div id="divScrolling" style="overflow: hidden; display: inline-flex;" class='<%# Eval("LabelScrolling") == "" ? "NoScrollContainer" : "ScrollContainer" %>'>
                                                                        <label id="lblScrolling" style="font-size: <%# Eval("FontSizeInnerData")%>px;" clietidmode="static" class="ScrollText"><%# Eval("LabelScrolling") %></label>
                                                                    </div>--%>
                                                                </td>
                                                                <td style="height: 30px; white-space: nowrap; font-weight: bold; min-width: 40px;" class="tdComponentStatus" clientidmode="static">
                                                                    <div style="display: inline-flex; font-size: <%# Eval("FontSizeInnerData") %>px">
                                                                        <asp:Label runat="server" ID="OperationID" Text='<%# Eval("Operation")%>'></asp:Label>
                                                                    </div>
                                                                </td>
                                                                <td style="min-width: 55px; height: 30px; white-space: nowrap; font-weight: bold;" class="tdComponentStatus" clientidmode="static">
                                                                    <div style="display: inline-flex; font-size: <%# Eval("FontSizeInnerData") %>px">
                                                                        <asp:Label runat="server" ID="OperationDesc" Text='<%# Eval("OpDesc")%>'></asp:Label>
                                                                    </div>
                                                                </td>

                                                                <td style="min-width: 55px; height: 30px; white-space: nowrap; font-weight: bold;" class="tdComponentStatus">
                                                                    <label style="font-size: <%# Eval("FontSizeInnerData") %>px;"><%# Eval("Status")%></label>
                                                                </td>
                                                                <td style='white-space: nowrap; font-weight: bold; min-width: 50px;' class="tdComponentStatus">
                                                                    <label style="font-size: <%# Eval("FontSizeInnerData") %>px;"><%# Eval("Priority")%></label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                    <%--</table>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="timer" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#hdnFilterShowHide").val("Hide");
            ShowPanelFilter();

            $(document).ready(function () {
                setDivWidthHeight();
                setcolor();

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

                // if the target of the click isn't the container nor a descendant of the container
                if (!container.is(e.target) && container.has(e.target).length === 0) {
                    $("#hdnFilterShowHide").val("Hide")
                    ShowPanelFilter();
                }
            });
        });
    </script>
</body>
</html>
