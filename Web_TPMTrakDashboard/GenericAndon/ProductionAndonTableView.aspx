<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductionAndonTableView.aspx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.ProductionAndonTableView" %>

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

        body {
            background-color: #ffffff;
        }

        .HeaderImage {
            flex: 1;
            float: left;
            display: flex;
            padding: 5px;
        }

        .headerRight {
            color: white;
            font-weight: 600;
            font-size: 20px;
            margin: 0px;
        }

        .add-border {
            border-radius: 15px;
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


        .header-grid {
            min-height: 100px;
            background: #3777bc;
            color: white;
            padding: 0px;
            white-space: pre-line;
        }

        /*.ProductionAndonContainer {
            margin: 5px 10px 20px 10px;
        }*/

        .gvProductionContainer {
            width: 100%;
            border: 0;
            box-shadow: 2px 2px 5px #a5a2a2;
        }

        .DivCellLabel {
            background-color: #0f4987;
            padding: 5px 2px 0px 2px;
        }

            .DivCellLabel span {
                color: white;
                font-weight: bold;
                font-size: 25px;
                font-family: sans-serif;
            }

        .gvProductionContainer > tbody > tr > th {
            border-bottom: 2px solid white !important;
            border: 1px solid white;
            border-collapse: collapse;
            text-align: center;
            padding: 0px 4px;
            min-width: 100px;
        }

        .gvProductionContainer > tbody > tr > td {
            /*border-bottom: 1px solid grey !important;*/
            border: 1px solid #d3d1d1;
            border-collapse: collapse;
            padding: 0px 5px;
            font-weight: bold;
        }

        .lblStatus {
            padding: 5px 20px;
            border-radius: 80%;
            margin-left: 5px;
        }

        .StatusImag {
            height: 44px;
            width: 44px;
            text-align: center;
        }

        .legendStyleSetting {
            display: block;
            padding: 4px;
            width: 25%;
            margin-bottom: 0px;
            font-size: 18px;
            color: #333;
        }

        #tblComputer > tbody > tr > td {
            padding: 2px;
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

        .Running {
            -webkit-animation: cog-rotate 2s linear infinite;
            -moz-animation: cog-rotate 2s linear infinite;
            -o-animation: cog-rotate 2s linear infinite;
            animation: cog-rotate 2s linear infinite;
            color: green;
        }

        .Stopped {
            color: red;
        }

        .Down {
            color: <%=machineStatusColors.ColorDown%>;
        }

        .ICD {
            color: <%=machineStatusColors.ColorICD%>;
        }

        .Alarm {
            color: <%=machineStatusColors.ColorAlarm%>;
        }

        .LoadUnload {
            color: <%=machineStatusColors.ColorLoadUnload%>;
        }

        .Disconnected {
            color: <%=machineStatusColors.ColorDisconnected%>;
        }

        .PowerOff {
            color: <%=machineStatusColors.ColorPowerOff%>;
        }

        .NoData {
            color: white;
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
        
        #tdchkDesktop input {
            width: 20px;
            height: 20px;
        }
    </style>

    <script>
        var dataRefreshTimer;;
        $(document).ready(function () {
            debugger;
            setDivWidthHeight();
            if (!($('#ComputerDiv').is(':visible'))) {
                setDataRefreshTimer();
            }
            else {
                $('#Container').css("display", "none");
            }
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
            //$('#scrollTextDiv').css("width", width - 70 - 200);
            var headerheight = 0;
            if ($('#headerDiv').height() == undefined)
                headerheight = 0;
            else
                headerheight = $('#headerDiv').height();
            $('#Container').css("height", $(window).height() - headerheight);
        }
        function setDataRefreshTimer() {
            debugger;
            $("#hdnFilterShowHide").val("Hide");
            clearTimeout(dataRefreshTimer);
            dataRefreshTimer = setTimeout(insertLatestDataToMainCache,<%= settings.DataRefreshInterval %>);
        }

        function insertLatestDataToMainCache() {
            $.ajax({
                async: true,
                type: "POST",
                url: "ProductionAndonTableView.aspx/insertLatestDataToMainCacheMemory",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                success: function (response) {
                    setDataRefreshTimer();
                },
                error: function (Result) {
                    setDataRefreshTimer();
                }

            })
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

        function setFlipInterval() {
            debugger;
            $("#hdntdHeight").val("");
            $("#hdnThHeight").val("");
            var FooterHeight = $('#footerDiv').height();
            if (FooterHeight == undefined) {
                FooterHeight = 0;
            }
            var CellLabelHeight = $("#DivCellLabel").height();
            if (CellLabelHeight == undefined) {
                CellLabelHeight = 0;
            }
            var headerheight = 0;
            if ($('#headerDiv').height() == undefined)
                headerheight = 0;
            else
                headerheight = $('#headerDiv').height();
            $('#ProductionAndonContainer').css("height", $(window).height() - 60 - CellLabelHeight - FooterHeight - 30);
            //$('#gvProductionContainer').css("height", $('#ProductionAndonContainer').height() - CellLabelHeight - 40);


            setThStyle();
            var thHeight = Math.max.apply(null, $('#gvProductionContainer').find("th").map(function () {
                return $(this).outerHeight(true);
            }).get());
            var tdHeight = Math.max.apply(null, $('#gvProductionContainer').find("td").map(function () {
                return $(this).outerHeight(true);
            }).get());
            $("#hdntdHeight").val(tdHeight);
            //var trWidth = Math.max.apply(null, $('#gvProductionContainer').find("tr").map(function () {
            //    return $(this).outerWidth(true);
            //}).get());

            //$('#gvProductionContainer').width(trWidth);
            $('#gvProductionContainer').find("th").height(thHeight - 150);
            let screenH = $('#ProductionAndonContainer').height();
            screenH = screenH - thHeight;
            let totalRows = Math.ceil(screenH / tdHeight);
            $('#gvProductionContainer').find('tr:gt(1)').hide();
            $('#gvProductionContainer').find('tr:gt(0)').slice(0, totalRows).show();
            //$('#gvProductionContainer').css("height", totalRows * tdHeight);

            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionAndonTableView.aspx/setFlipIntervalToSession",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: '{displayedItemCount:' + totalRows + '}',
                success: function (response) { },
                error: function (Result) { }
            });

        }

        function setdtHeight() {
            debugger;
            //if ($("#hdntdHeight").val() != null) {
            //    $('#gvProductionContainer').find("td").height($("#hdntdHeight").val());
            //}
            if ($("#hdnThHeight").val() != null) {
                $('#gvProductionContainer').find("th").height($("#hdnThHeight").val() - 150);
            }
            $('#gvProductionContainer').width($(window).width() - 20)
        }

        function setThStyle() {
            debugger;
            var list = $('#gvProductionContainer').find("th");
            if (list.length > 0) {
                var maxLength = 10;
                for (var i = 0; i < list.length; i++) {
                    var HeaderName = list[i].innerHTML;
                    if (list[i].innerHTML.length <= maxLength) {
                        continue;
                    }
                    var lastSpaceIndex = HeaderName.lastIndexOf(' ', maxLength);
                    if (lastSpaceIndex > 0) {
                        list[i].innerHTML = '';
                        list[i].innerHTML = HeaderName.slice(0, lastSpaceIndex) + '<br>' + HeaderName.slice(lastSpaceIndex);
                    }
                    else {
                        list[i].innerHTML = '';
                        list[i].innerHTML = HeaderName.slice(0, maxLength) + '<br>' + HeaderName.slice(maxLength);
                    }
                }
            }
        }

        function hidePanelFilter() {
            $("#panelFilter").hide();
            $("#hdnFilterShowHide").val("Show")
        }

        $(document).mouseup(function (e) {
            var container = $("#panelFilter");
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                $("#hdnFilterShowHide").val("Hide")
                ShowPanelFilter();
            }
        });

        function BindCellID() {
            debugger;
            var PlantID = $("#ddlPlantID").val();

            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionAndonTableView.aspx/GetCellIDsPlantWise",
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

            return false;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdntdHeight" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnThHeight" ClientIDMode="Static" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Timer runat="server" ID="timer" ClientIDMode="Static" OnTick="timer_Tick"></asp:Timer>
                <div class=" text-center">
                    <div class="navbar navbar-default navbar-fixed-top text-center headerDiv" clientidmode="static" style="padding: 0px 5px; background-color: #3777bc; height: 60px;">
                        <asp:Label ID="headername" runat="server" ClientIDMode="Static" Style="font-weight: bold; color: white; font-size: 33px; text-align: right; margin-top: 5px; vertical-align: middle;">.......</asp:Label>
                    </div>
                    <div class="navbar navbar-default navbar-fixed-top text-center" id="headerDiv" clientidmode="static" style="padding: 0px 5px; background-color: transparent; height: 60px;">
                        <div class="HeaderImage" style="height: 60px">
                            <asp:Image runat="server" ID="CompanyLogo" class="img-responsive img-rounded" />
                        </div>
                        <div style="float: right; position: relative; display: inline-flex" runat="server" id="divHeader">
                            <div style="padding-right: 20px;">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ClientIDMode="Static" ID="lblShift" CssClass="headerRight"></asp:Label>
                                            <label class="headerRight" runat="server" id="lblDateTime" clientidmode="static" style="display: inline-block"></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdchkDesktop" style="width: 20%; text-align: left;" title="Switch to Desktop Mode">
                                            <asp:CheckBox runat="server" ID="chkDesktopView" OnCheckedChanged="chkDesktopView_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="padding-top: 5px; padding-right: 0px;">
                                <table id="tableSettings" style="width: 60px;">
                                    <tr>
                                        <td runat="server" id="tdAndonSettings" onclick="location.href='AndonSettingTableView.aspx';">
                                            <i class="glyphicon glyphicon-cog" style="font-size: 20px; color: white"></i>
                                        </td>
                                        <td runat="server" id="tdHomeReset" onclick="location.href='ProductionAndonTableView.aspx';">
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
                                                <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" onchange="return BindCellID();" AutoPostBack="true" Style="margin-top: 0px" ToolTip="Plant">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-right: 2px; padding-bottom: 2px;">
                                                <asp:DropDownList runat="server" ID="ddlCellID" ClientIDMode="Static" CssClass="form-control" Style="margin-top: 0px" ToolTip="Cell">
                                                </asp:DropDownList>
                                            </td>
                                            <%-- <td style="padding-bottom: 2px;">
                                                    <asp:DropDownList runat="server" ID="ddlFrequency" Visible="false" ClientIDMode="Static" CssClass="form-control" Style="margin-top: 0px" ToolTip="Frequency">
                                                    </asp:DropDownList>
                                                </td>--%>    <%--Frequency is changed to settings page: 21-12-2023 by Bindu as it is one time setting.--%>
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
                <div id="Container" runat="server" clientidmode="static" style="margin-top: 10px">
                    <div style="text-align: center;" class="DivCellLabel" runat="server" id="DivCellLabel">
                        <asp:Label runat="server" ID="lblCellName" CssClass="cellLabel" Style="font-size: 26px; font-weight: bold;"></asp:Label>
                    </div>
                    <div id="ProductionAndonContainer" class="ProductionAndonContainer">
                        <asp:GridView runat="server" ID="gvProductionContainer" AutoGenerateColumns="false" HeaderStyle-CssClass="header-grid text-center" CssClass="gvProductionContainer" OnRowDataBound="gvProductionContainer_RowDataBound" AlternatingRowStyle-BackColor="#d5d5d5">
                        </asp:GridView>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="timer" />
            </Triggers>
        </asp:UpdatePanel>

        <footer style="display: <%= settings.ShowFooterBlock %>;">
            <div class="navbar navbar-default navbar-fixed-bottom footerBottom" style="padding: 0px 5px; background-color: #3777bc; height: 2px; text-align: center" id="footerDiv">
                <div style="float: left;">
                    <p style="color: #fcefef; font-style: italic; margin-top: 10px; font-size: 16px; display: inline-block">Powered by TPM-Trak®</p>
                </div>
                <div id="scrollTextDiv" style="display: <%= settings.ShowMsgBox %>; margin-top: 4px; width: 80vw">
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
