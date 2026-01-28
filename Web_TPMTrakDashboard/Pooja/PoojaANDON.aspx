<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PoojaANDON.aspx.cs" Inherits="Web_TPMTrakDashboard.Pooja.PoojaANDON" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <%-- <script src="../Scripts/jquery-1.8.2.js"></script>
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <%: Styles.Render("~/bundles/mastercss") %>--%>
    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <link href="../Scripts/ColorPickerJs/css/pick-a-color-1.2.2.min.css" rel="stylesheet" />
    <script src="../Scripts/ColorPickerJs/dependencies/jquery-1.9.1.min.js"></script>
    <script src="../Scripts/ColorPickerJs/dependencies/tinycolor-0.9.15.min.js"></script>
    <script src="../Scripts/ColorPickerJs/js/pick-a-color-1.2.2.min.js"></script>
    <link href="../AndonContent/bootstrap-glyphicons.css" rel="stylesheet" />

    <title>Production Status ANDON</title>

    <style>
        * {
            font-family: <%= settings.FontFamily %>;
        }

        body {
            margin: 0px;
        }

        .header {
            display: inline-block;
            width: 100%;
            background-color: #1a2732;
            color: white;
            font-weight: bold;
            height: 74px;
            padding: 2px;
            position: sticky;
            top: 0px;
            text-align: center;
        }

        .div-logo {
            /*display: inline-block;*/
            float: left;
            /* position: relative;
            top: 6px;*/
        }

            .div-logo img {
                border-radius: 5px;
            }

        .div-headername {
            /*text-align: center;*/
            /* display: inline-block;
            width: 75%;
            height: 100%;
            position: relative;
            top: -21px;*/
            font-size: 30px;
            margin: auto;
        }

        .div-dateshift {
            /*display: inline-block;*/
            /*text-align: right;*/
            /*width: 13%;*/
            float: right;
            min-width: 410px;
        }

            .div-dateshift tr td {
                font-size: 16px;
                text-align: left;
            }

        .content {
            margin: 10px auto;
            /*border: 1px solid;*/
        }

        .table-style {
            width: 99%;
            border-collapse: collapse;
            margin: auto;
        }

            .table-style tr td, .table-style tr th {
                border: 1px solid #ddd;
            }

            .table-style tr th {
                background-color: #2e6886;
                padding: 10px 10px;
                color: white;
                font-size: <%= settings.TableHeaderFontSize + "px" %>;
                font-weight: bold;
            }

            .table-style tr td {
                padding: 9px 10px;
                font-size: <%= settings.TableContentFontSize + "px" %>;
                font-weight: <%= settings.FontBold %>;
            }

        .status-icon {
            border: 1px solid #080843;
            border-radius: 50px;
            width: <%= settings.TableContentFontSize + "px" %>;
            height: <%= settings.TableContentFontSize + "px" %>;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div id="productionHeader" class="header">
                        <div class="div-logo">
                            <%--height:48px;--%>
                            <asp:Image ID="Image1" runat="server" class="img-responsive img-rounded" Style="height: 70px; width: 130px;" AlternateText="Company Logo" />
                            <%--width: 200px;--%>
                        </div>
                        <div class="div-dateshift">
                            <div style="float: left;">
                                <div style="display: inline-block">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 2px">
                                                <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div style="display: inline-block; position: relative; top: -9px;">
                                    <table>
                                        <tr>
                                            <td>Date: </td>
                                            <td>
                                                <span id="lblDate" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Shift: </td>
                                            <td>
                                                <span id="lblShift" runat="server"></span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div style="float: right;">
                                <div onclick="location.href='ANDONSettings.aspx';">
                                    <i class="glyphicon glyphicon-cog" style="font-size: 25px; color: #49b8d3"></i>
                                </div>
                                <div onclick="FullScreenClicked();">
                                    <i class="glyphicon glyphicon-fullscreen" style="font-size: 25px; color: #49b8d3"></i>
                                </div>
                            </div>
                        </div>
                        <div class="div-headername"><span style="vertical-align: -webkit-baseline-middle"><%= settings.MainHeaderName %></span></div>

                    </div>


                    <div id="productionConatiner" class="content">
                        <asp:Timer runat="server" ID="flipInterval" ClientIDMode="Static" OnTick="flipInterval_Tick"></asp:Timer>
                        <asp:ListView runat="server" ID="lvProductionDetails" ClientIDMode="Static">
                            <LayoutTemplate>
                                <table class="table-style" id="tblProductionDetails">
                                    <tr>
                                        <th runat="server" id="thMachineHeaderName"></th>
                                        <th runat="server" id="thComponentAndOpnHeaderName"></th>
                                        <th runat="server" id="thOEEHeaderName"></th>
                                        <th runat="server" id="thDownTimeHeaderName"></th>
                                        <th runat="server" id="thOperatorHeaderName"></th>
                                        <th runat="server" id="thProductionTargetHeaderName"></th>
                                        <th runat="server" id="thActualHeaderName"></th>
                                        <th runat="server" id="thStatusHeaderName"></th>

                                    </tr>
                                    <tr runat="server" id="itemplaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server" id="itemplaceholder">
                                    <td>
                                        <%#  Eval("Machine") %>
                                    </td>
                                    <td>
                                        <%#  Eval("Component")  + (String.IsNullOrEmpty(Eval("Operation").ToString()) ?"": ", " + Eval("Operation"))  %>
                                    </td>
                                    <td>
                                        <%# Eval("OEE") %>
                                    </td>
                                    <td>
                                        <%#  Eval("Downtime") %>
                                    </td>
                                    <td>
                                        <%#  Eval("Operator") %>
                                    </td>
                                    <td>
                                        <%#  Eval("ProductionTarget") %>
                                    </td>
                                    <td>
                                        <%#  Eval("Actual") %>
                                    </td>
                                    <td style="text-align: center">
                                        <div class="status-icon" style="background-color: <%#  Eval("StatusColor") %>"></div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <script>
            function FullScreenClicked() {
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
            }
            function SetFlipInterval() {
                let totalRows;
                if (<%= settings.NoOfMachinesToDisplay %> <= 0) {
                    $('#productionConatiner').css("height", ($(window).height() - $('#productionHeader').height() - $('#tblProductionDetails tr th').outerHeight()));

                    /*   $('#productionConatiner').css("height", ($(window).height() - $('#productionHeader').height() - 20));*/
                    var tblMaxHeight = Math.max.apply(null, $('#tblProductionDetails tr:not(:first-child)').map(function () {
                        return $(this).outerHeight(true);
                    }).get());

                    let screenH = $('#productionConatiner').height();
                    let totalH = Math.floor(screenH / (tblMaxHeight));
                    totalRows = Math.floor(totalH);

                } else {
                    totalRows =<%= settings.NoOfMachinesToDisplay %>;
                }
                var rows = $('#tblProductionDetails tr:not(:first-child)');
                for (var i = (rows.length - 1); i >= totalRows; i--) {
                    rows[i].remove();
                }

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "PoojaANDON.aspx/SetFlipIntervalToSession",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: '{displayRowsCount:' + totalRows + '}',
                    success: function (response) {
                    },
                    error: function (Result) {
                    }
                });
            }
        </script>
    </form>

</body>
</html>
