<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpRequestProductionAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.Andon_GEA.HelpRequestProductionAndon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Pragma" content="no-cache">
    <title>Production Andon</title>
    <%--  <script src="/AndonScripts/jquery-3.1.0.min.js"></script>
    <script src="/AndonScripts/bootstrap.min.js"></script>
    <link href="/AndonContent/bootstrap.min.css" rel="stylesheet" />--%>
    <%--   <link href="/Content/Site.css" rel="stylesheet" />--%>
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/TextAnimation.css" rel="stylesheet" />

    <script src="Scripts/Highchart8/highcharts.js"></script>
    <script src="Scripts/Highchart8/pareto.js"></script>
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
            overflow: hidden;
            background-color: white;
        }

        #div3 {
            grid-column: 2/ 4;
            grid-row: 3/5;
            overflow: hidden;
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
            margin-top: 18px;
        }

            .machineList tr th, .machineList > tbody > tr > td:first-child {
                color: darkblue;
                font-size: 17px;
                font-weight: 600;
                padding: 8px;
                border: 1px solid #a4aab0;
            }

            .machineList > tbody > tr:first-child > td {
                color: darkblue;
                height: 37px;
                border: 1px solid #a4aab0;
            }

                .machineList > tbody > tr:first-child > td:first-child {
                    border: none;
                }

        #decanterShifDayTbl tr:first-child td:first-child {
            border: 1px solid #a4aab0;
        }

        #shiftDayContainer .machineList {
            margin-top: 5px;
        }

            #shiftDayContainer .machineList tr td {
                white-space: nowrap
            }

        .shiftDayTD {
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
            width: 53px;
            height: 50px;
        }

        .poGreenBg {
            background-color: #90ee90;
        }

        .poPinkBg {
            background-color: #ffc0c0;
        }

        .poYellowBg {
            background-color: #ffffc0;
        }

        #decanterTbl {
            width: 100%;
        }

            #decanterTbl tr th {
                padding: 8px;
                font-size: 37px;
                border: 1px solid #c7bebe;
                text-align: center;
                font-weight: bold;
                color: darkblue;
            }

            #decanterTbl tr td {
                padding: 10px;
                font-size: 35px;
                border: 1px solid #c7bebe;
                text-align: center;
                font-weight: bold;
            }

        .hideShiftDayData {
            display: none;
        }

        .innerTbl {
            width: 100%;
        }

            .innerTbl tr td {
                width: 50%;
                border: 1px solid #a4aab0 !important;
                padding: 8px;
                line-height: 1;
            }

        /* video::-webkit-media-controls-volume-slider {
            display: none;
        }

        video::-webkit-media-controls-mute-button {
            display: none;
        }*/
        .carousel-indicators {
            visibility: hidden;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdnChartsCount" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCachInterval" ClientIDMode="Static" />
        <div class=" container-fluid">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row text-center">
                        <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 5px; background-color: #3777bc">
                            <div class="HeaderImage" style="height: 60px">
                                <%-- <img src="Images/SPFLogo.PNG" height="60" style="padding: 3px;" />--%>
                                <asp:Image ID="Image2" runat="server" class="img-responsive img-rounded" Style="width: 200px; height: 56px; margin-top: 2px" />
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
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" Style="margin-top: 0px">
                                                </asp:DropDownList>
                                            </td>
                                            <%--    <td>
                                                <asp:DropDownList runat="server" ID="ddlShiftDayType" ClientIDMode="Static" CssClass="form-control" Style="margin-top: 0px" OnSelectedIndexChanged="ddlShiftDayType_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="Shift">Current Shift</asp:ListItem>
                                                    <asp:ListItem Value="Day">Current Day</asp:ListItem>
                                                    <asp:ListItem Value="Week">Current Week</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnShiftDayType"  ClientIDMode="Static"/>
                                            </td>--%>
                                        </tr>
                                    </table>

                                </div>
                                &nbsp;&nbsp;
                               
                                <div>
                                    <div runat="server" id="divAndonSettings">
                                        <div style="margin: 1px; height: 29px;"> <%--onclick="location.href='AndonSetting.aspx';"--%>
                                            <%--   <p id="settingContainer" runat="server" clientidmode="static">
                                                <img src="Images/List-Icon.jpg" height="29" /></p>--%>


                                            <asp:ImageButton runat="server" ID="ImgBtnSettings" ImageUrl="Images/List-Icon.jpg" OnClick="ImgBtnSettings_Click" ToolTip="Switch to ANDON Mode" Style="width: 32px; height: 29px; position: relative;" />

                                        </div>
                                    </div>
                                    <p class="headerRight"><span style="font-size: 18px; cursor: pointer; color: white; vertical-align: text-top" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span></p>
                                    <%--  <p style="margin: 0px">
                                <img src="Images/Power1.jpg" height="29" />
                            </p>--%>
                                </div>
                            </div>
                            &nbsp;&nbsp;
                       
                        </div>
                    </div>
                    <asp:Timer runat="server" ID="poInterval" OnTick="poInterval_Tick"></asp:Timer>

                    <div class="row" style="margin-top: 15px">
                        <div class="col-lg-12 col-sm-12 col-md-12" style="padding: 0px;">
                            <asp:Label runat="server" ID="lblErrorMsg" ClientIDMode="Static" Style="color: red; margin-left: 40%; font-size: 30px; margin-top: 10px;"></asp:Label>
                            <div id="shiftDayContainer" runat="server" style="height: 85vh; overflow: hidden">
                                <div class="row decanterMachineDiv" style="margin-top: 5px;">
                                    <%--height: 38vh--%>
                                    <div class="col-lg-1"></div>
                                    <div class="col-lg-4">
                                        <div>
                                            <div style="background-color: #90ee90; padding: 5px; border-radius: 6px; text-align: center;"><span style="font-size: 25px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Decanter Output</span></div>
                                            <div>
                                                <asp:ListView runat="server" ID="lvDecanterShiftDay">
                                                    <LayoutTemplate>
                                                        <table class="machineList" id="decanterShifDayTbl">
                                                            <tr>
                                                                <td style="text-align: center; padding: 0px" class="tdGreenColor">
                                                                    <div style="padding: 0px; border: 1px solid #a4aab0; text-align: center">
                                                                        <label class="machineDecanterHeader machineDecanterHeaderName">Current Shift Output</label>
                                                                    </div>
                                                                    <%-- <table class="innerTbl">
                                                                        <tr>
                                                                            <td  class="machineDecanterHeader">Machine
                                                                            </td>
                                                                            <td  class="machineDecanterHeader">Quality
                                                                            </td>
                                                                        </tr>
                                                                    </table>--%>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="itemplaceholder"></tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="font-size: 30px !important; color: #004000; text-align: center" class="machineDecanterHeader">
                                                                <%# Eval("Col1") %>
                                                                <%--  <table class="innerTbl">
                                                                    <tr>
                                                                        <td style="font-size: 26px !important; color: #004000"><%# Eval("Col1") %>
                                                                        </td>
                                                                        <td style="font-size: 26px !important; color: #004000"><%# Eval("Col2") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>--%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div style="width: 80%; margin: auto">
                                            <div style="background-color: #90ee90; padding: 5px; border-radius: 6px; text-align: center; margin-bottom: 5px;"><span style="font-size: 25px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Machine Shop Output</span></div>
                                            <div>
                                                <asp:ListView runat="server" ID="lvMachineShopShiftDay">
                                                    <LayoutTemplate>
                                                        <table class="machineList tblDecanterMachineMonthView" id="currentPartOutputTbl">
                                                            <tr>
                                                                <td class="shiftDayTD"></td>
                                                                <td class="tdGreenColor shiftDayTD" style="padding: 0px; border: none">
                                                                    <div style="padding: 0px; border: 1px solid #a4aab0; text-align: center">
                                                                        <label class="machineDecanterHeader machineDecanterHeaderName">Current Shift Output</label>
                                                                    </div>

                                                                    <table class="innerTbl">
                                                                        <tr>
                                                                            <td class="machineDecanterHeader">Machine
                                                                            </td>
                                                                            <td class="machineDecanterHeader">Quality
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="itemplaceholder"></tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="shiftDayTD"><%# Eval("Col1") %></td>
                                                            <td class="tdGreenColor shiftDayTD " style="padding: 0px; border: none">
                                                                <table class="innerTbl">
                                                                    <tr>
                                                                        <td class="shiftDayTD" style="color: #004000; text-align: center"><%# Eval("Col2") %>
                                                                        </td>
                                                                        <td class="shiftDayTD" style="text-align: center"><%# Eval("Col3") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-1"></div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-lg-1"></div>
                                    <div class="col-lg-10">
                                        <div style="background-color: #ffc0c0; padding: 5px; border-radius: 6px; text-align: center; margin-bottom: 5px; width: 94%"><span style="font-size: 25px; font-weight: 800; color: #ff0000; font-family: Helvetica, Arial, sans-serif">Month View</span></div>
                                    </div>
                                    <div class="col-lg-1"></div>
                                </div>
                                <div class="row decanterMachineDiv" style="margin-top: 5px;">
                                    <div class="col-lg-1"></div>
                                    <div class="col-lg-4">
                                        <div>
                                            <div style="background-color: #90ee90; padding: 5px; border-radius: 6px; text-align: center; margin-bottom: 5px;"><span style="font-size: 25px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Decanter Output</span></div>
                                            <div>
                                                <asp:ListView runat="server" ID="lvDecanterMonthView">
                                                    <LayoutTemplate>
                                                        <table class="machineList">
                                                            <tr runat="server" id="itemplaceholder"></tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="machineDecanterHeader"><%# Eval("Col1") %></td>
                                                            <td class="tdGreenColor machineDecanterHeader" style="text-align: center"><%# Eval("Col2") %></td>
                                                            <td class="tdGreenColor machineDecanterHeader" style="min-width: 120px; text-align: center"><%# Eval("Col3") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div style="width: 80%; margin: auto">
                                            <div style="background-color: #90ee90; padding: 5px; border-radius: 6px; text-align: center; margin-bottom: 5px;"><span style="font-size: 25px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Machine Shop Output</span></div>
                                            <div>
                                                <asp:ListView runat="server" ID="lvMachineShopMonth">
                                                    <LayoutTemplate>
                                                        <table id="machineShoptbl" class="machineList tblDecanterMachineMonthView">
                                                            <tr runat="server" id="itemplaceholder"></tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="shiftDayTD"><%# Eval("Col1") %></td>
                                                            <td class="tdGreenColor shiftDayTD" style="text-align: center"><%# Eval("Col2") %></td>
                                                            <td class="tdGreenColor shiftDayTD" style="min-width: 150px; text-align: center"><%# Eval("Col3") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-1"></div>
                                </div>
                            </div>
                            <div id="poContainer" runat="server" clientidmode="static" style="height: 85vh">
                                <div id="div1">
                                    <asp:ListView runat="server" ID="lvPOData">
                                        <LayoutTemplate>
                                            <table id="lvPODataTbl" runat="server" clientidmode="static">
                                                <tr>
                                                    <th id="thMachine" runat="server">Machine</th>
                                                    <th id="thStatus" runat="server">Satus</th>
                                                    <th id="thComponent" runat="server">
                                                        <%--<img src="Images/Printer1.png" class="imgInHeader" runat="server" />--%>
                                                        <img src="Images/Maintenance1.png" class="imgInHeader" runat="server" />
                                                    </th>
                                                    <th id="thSetting" runat="server">
                                                        <%--<img src="Images/Settings-icon.png" class="imgInHeader" runat="server" />--%>
                                                        <img src="Images/Quality.png" class="imgInHeader" runat="server" />
                                                    </th>
                                                    <th id="thTimer" runat="server">
                                                        <%--<img src="Images/Alarm-clock.png" class="imgInHeader" runat="server" />--%>
                                                        <img src="Images/Production.jpg" class="imgInHeader" runat="server" />
                                                    </th>
                                                    <th id="thUser" runat="server">
                                                        <%--<img src="Images/UserL.png" class="imgInHeader" runat="server" />--%>
                                                        <img src="Images/Logistics.jpg" class="imgInHeader" runat="server" />
                                                    </th>
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
                                                    <img src='<%# Eval("Status") %>' class="poBall" /></td>
                                                <td id="tdComponent" runat="server">
                                                    <img src='<%# Eval("Component") %>' class="poBall" /></td>
                                                <td id="tdSetting" runat="server">
                                                    <img src='<%# Eval("Setting") %>' class="poBall" /></td>
                                                <td id="tdTimer" runat="server">
                                                    <img src='<%# Eval("Alaram") %>' class="poBall" /></td>
                                                <td id="tdUser" runat="server">
                                                    <img src='<%# Eval("User") %>' class="poBall" /></td>
                                                <td id="tdAvailabilityEfficiency" runat="server" class='<%# Eval("AEBackColor") %>'><%# Eval("AE") %></td>
                                                <td id="tdProductionEfficiency" runat="server" class='<%# Eval("PEBackColor") %>'><%# Eval("PE") %></td>
                                                <td id="tdOverAllEfficiency" runat="server" class='<%# Eval("OEEBackColor") %>'><%# Eval("OEE") %></td>
                                                <td id="tdPlan" runat="server"><%# Eval("Plan") %></td>
                                                <td id="tdAct" runat="server"><%# Eval("Act") %></td>
                                                <td id="tdEmoji" runat="server">
                                                    <img src='<%# Eval("Emoji") %>' class="poBall" /></td>

                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                                <div id="div2">
                                    <div id="leadingMachineContainer" runat="server">
                                        <div style="background-color: #90ee90; padding: 6px; border-radius: 6px; text-align: center"><span style="font-size: 24px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Leading Machines</span></div>
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
                                                        <%-- <td class="tdGreenColor"><%# Eval("Col4") %></td>--%>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                    <div id="downTimeContainer" runat="server">
                                        <div style="background-color: #90ee90; padding: 5px; border-radius: 6px; text-align: center"><span style="font-size: 24px; font-weight: 800; color: black; font-family: Helvetica, Arial, sans-serif">Down Time</span></div>
                                        <div id="DownTimeChartContainer" style="margin-top: 16px">
                                        </div>
                                    </div>
                                </div>
                                <div id="div3">
                                    <div id="laggingMachineContainer" runat="server">
                                        <div style="background-color: #ffc0c0; padding: 6px; border-radius: 6px; text-align: center"><span style="font-size: 24px; font-weight: 800; color: #ff0000">Lagging Machines</span></div>
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
                                                        <%-- <td class="tdRedColor"><%# Eval("Col4") %></td>--%>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                    <div id="oeeConatiner" runat="server">
                                        <div style="background-color: #ffc0c0; padding: 5px; border-radius: 6px; text-align: center"><span style="font-size: 24px; font-weight: 800; color: #ff0000">This Month OEE</span></div>
                                        <div id="OEEChartContainer" style="margin-top: 16px">
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-top: 5px" id="imageVideoConatiner" runat="server" clientidmode="static">

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
                        <p style="color: #fcefef; font-style: italic; margin-top: 10px; font-size: 16px; display: inline-block">Powered by TPM-Trak®</p>
                        <asp:UpdatePanel runat="server" style="display: inline-block">
                            <ContentTemplate>
                                &nbsp;&nbsp;&nbsp;
                              
                                <asp:ImageButton runat="server" ID="imgBtnSwitch" ImageUrl="Images/andon.jpg" OnClick="imgBtnSwitch_Click" ToolTip="Switch to ANDON Mode" Style="width: 32px; height: 27px; position: relative; top: 7px" OnClientClick="return blockUI();" />
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
                    <div style="display: inline-block; margin-top: 4px; width: 84%">
                        <marquee style="font-family: Book Antiqua; color: #FFFFFF; font-size: 25px; background-color: #0f4987" scrollamount="10" loop="infinite" runat="server" id="spanWelcome"></marquee>
                    </div>
                    <div style="float: right;">
                        <img src="Images/AMiT.jpg" height="43" width="70" />
                    </div>
                </div>
            </footer>
        </div>
        <div class="modal fade" id="settingLoginModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog  modal-dialog-centered" style="width: 500px">
                <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                    <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                        <h4 class="modal-title" style="color: white;">Enter Username and Password!</h4>
                    </div>
                    <div class="modal-body">
                        <table style="margin: auto">
                            <tr>
                                <td>User Name&nbsp;</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Password&nbsp;</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: center">
                        <%--<asp:Button runat="server" ID="btnSettingLogin" Width="80px" OnClick="btnSettingLogin_Click" />--%>
                        <button type="button" id="btnSettingLogin" style="width: 80px;" onclick="loginVerification();">Login</button>
                        <%--<button type="button" style="width: 80px;" onclick="location.href='HelpRequestProductionAndon.aspx'">Cancel</button>--%>
                        <asp:Button runat="server" ID="btnCancel" OnClientClick="clearAllModals();" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
        <script>
            var countItem = 0;
            $('.carousel').carousel({
                pause: "false"
            });
            var cacheInterval = 30000;
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
                cacheInterval = getCacheInterval();
                setInterval(addDataToCache, parseInt($('#hdnCachInterval').val()));
                setShiftDayFonts();
            });
            function loginVerification() {
                var userName = $("#txtUserName").val();
                var passWord = $("#txtPassword").val();

                var Values = { "UserName": userName, "Password": passWord }

                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("HelpRequestProductionAndon.aspx/ValidateUserForSettings") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: JSON.stringify(Values),
                    success: function (response) {
                        var dataitem = response.d;
                        if (dataitem)
                            location.href = 'AndonSetting.aspx';
                        else
                        alert("UnAuthorised.")
                    },
                    error: function (Result) {
                        alert("Invalid Credentials.");
                    }
                });
            }
            function setMachineShopHeader(headername) {
                $('.machineDecanterHeaderName').text(headername);
            }
            function getCacheInterval() {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("HelpRequestProductionAndon.aspx/getCacheIntervalValue") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    success: function (response) {
                        var dataitem = response.d;
                        cacheInterval = dataitem;
                        $('#hdnCachInterval').val(dataitem);
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
            }
            function addDataToCache() {
                /* var plantId = $('#ddlPlantID').val();*/
                //console.log("ShiftDayType : " + $('#hdnShiftDayType').val());
                //if ($('#hdnShiftDayType').val() == "") {
                //    $('#hdnShiftDayType').val("Shift");
                //    console.log("ShiftDayType : " + $('#hdnShiftDayType').val());
                //}
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("HelpRequestProductionAndon.aspx/addDataToCacheMemory") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    /*data: '{plant:"' + plantId + '"}',*/
                    success: function (response) {

                        var dataitem = response.d;
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
            }
            //$("video").each(function () {
            //    this.addEventListener('ended', myHandler, false);
            //    this.addEventListener('error', myHandler, false);
            //});

            //function myHandler(e) {
            //    // What you want to do after the event
            //    $("#myCarousel").carousel("cycle");
            //}
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
                    url: "HelpRequestProductionAndon.aspx/setScreenName",
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (response) {
                        var itmdata = response.d;
                    },
                    error: function (jqXHR, textStatus, err) {
                    }
                });
            }

            function OpenLoginModal() {
                $("#settingLoginModal").modal("show");
                return false;
            }

            $("[id$=btnFullScreen]").click(function () {
                if ((document.fullScreenElement && document.fullScreenElement !== null) ||
                    (!document.mozFullScreen && !document.webkitIsFullScreen)) {
                    if (document.documentElement.requestFullScreen) {
                        document.documentElement.request
                        reen();
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
                    url: "HelpRequestProductionAndon.aspx/getFontSize",
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (response) {
                        var itmdata = response.d;
                        $('#lvPODataTbl tr th').css('font-size', parseInt(itmdata[0]));
                        $('#lvPODataTbl tr td').css('font-size', parseInt(itmdata[1]));
                        $('.poBall').css("width", parseInt(itmdata[1]) + 35);
                        //  $('.poBall').css("height", parseInt(itmdata[1]) + 35);
                        $('.machineList tr td').css('font-size', 23);
                        $('.machineList tr:nth-child(5) td:first-child').css('font-size', 20);
                        //debugger;
                        //if ($('#leadingMachineContainer').height() >= $('#div2').height()) {
                        //    while ($('#leadingMachineContainer').height() >= $('#div2').height()) {
                        //         $('.machineList tr td').css('font-size', parseInt(itmdata[1]) - 2);
                        //    }
                        //}



                        $('.machineList tr:first-child td').css('font-size', 30);


                    },
                    error: function (jqXHR, textStatus, err) {
                    }
                });
            }
            function setPORowsBasedOnHeight() {

                setHeaderContentSize();
                var POContainerHeight = $('#poContainer').height();
                var headerHeight = $('#lvPODataTbl tr:first-child').height();
                POContainerHeight = POContainerHeight - headerHeight;
                // var contentHeight = $('#lvPODataTbl tr:nth-child(2)').height();
                var contentHeight = $('.poBall').width() + 10 + 1;
                var fittedRows = Math.floor(POContainerHeight / contentHeight);
                var rows = $('#lvPODataTbl tr:not(:first-child)');
                var rowCount = rows.length;
                debugger;
                if ($('#imgBtnSwitch').attr('title') == "Switch to ANDON Mode") {
                    $('#div1').css("overflow-y", "auto");
                } else {
                    $('#div1').css("overflow-y", "hidden");
                    for (var i = (rows.length - 1); i >= fittedRows; i--) {
                        rows[i].remove();
                    }
                }

                //'<%= ResolveUrl("HelpRequestProductionAndon.aspx/getDownTimeChartData") %>',
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%=ResolveUrl("HelpRequestProductionAndon.aspx/setNoOfRowsToSession")%>',
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
                //  BindOEEChart();
                //  BindDownTimeChart();
                BindOEEdownCharts();
            }
            function BindOEEdownCharts() {
                //var plantId = $('#ddlPlantID').val();
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("HelpRequestProductionAndon.aspx/getChartData") %>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    //data: '{plant:"' + plantId + '"}',
                    success: function (response) {

                        var dataitem = response.d;
                        BindOEEChart(dataitem.OEEData);
                        BindDownTimeChart(dataitem.DownTimeData);
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
            }
            function BindOEEChart(dataitem) {
                var XCategoty = [];
                var Value = [];
                var OEEData = [];
                OEEData = dataitem;
                debugger;
                for (var i = 0; i < OEEData.length; i++) {
                    XCategoty[i] = OEEData[i].Category;
                    Value[i] = OEEData[i].Value;
                }

                console.log("Cat =" + OEEData.Category);
                Highcharts.chart('OEEChartContainer', {
                    chart: {
                        type: 'bar',
                        height: '55%'
                    },
                    title: {
                        text: '',
                        //style: {
                        //    color: 'black',
                        //    fontSize: '20px',
                        //    fontWeight: '600'
                        //}
                    },
                    exporting: { enabled: false },
                    xAxis: {

                        categories: XCategoty,
                        title: {
                            /* text: "Plant ID",*/
                            text: "",
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
                        max: 100,
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
                        animation: false
                    }]
                });
            }
            function BindDownTimeChart(dataitem) {
                var downTimeData;
                downTimeData = dataitem;
                console.log("DownTime Series =" + downTimeData.series);

                var val = downTimeData.series[1].data;
                console.log("Down Value " + val);
                Highcharts.chart('DownTimeChartContainer', {
                    chart: {
                        renderTo: 'DownTimeChartContainer',
                        type: 'column',
                        height: '54%',
                    },
                    title: {
                        text: ''
                    },
                    exporting: { enabled: false },
                    tooltip: {
                        shared: true
                    },
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
                                fontSize: '15px',
                                fontWeight: '600'
                            }
                        },
                        gridLineWidth: 1,
                        crosshair: true
                    },
                    yAxis: [{ //--- Primary yAxis
                        gridLineWidth: 0,
                        title: {
                            text: 'Down Time (min)',
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
                            format: "{value}%",
                            style: {
                                color: 'black',
                                fontSize: '15px',
                                fontWeight: '600'
                            }
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                    },
                    ],
                    legend: {
                        enabled: false
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        type: 'pareto',
                        name: 'Pareto',
                        yAxis: 1,
                        zIndex: 10,
                        color: '#86ba35',
                        baseSeries: 1,
                        tooltip: {
                            valueDecimals: 2,
                            valueSuffix: '%'
                        },
                        animation: false
                    }, {
                        name: 'Down',
                        type: 'column',
                        color: '#1ba1e2',
                        zIndex: 2,
                        data: downTimeData.series[1].data,
                        animation: false
                    }]
                });
            }
            //function BindDownTimeChart(dataitem) {
            //    var downTimeData;
            //    downTimeData = dataitem;
            //    console.log("DownTime Series =" + downTimeData.series);
            //    Highcharts.chart('DownTimeChartContainer', {
            //        chart: {
            //            height: '55%',
            //            type: 'column'
            //        },
            //        title: {
            //            text: '',
            //            //style: {
            //            //    color: 'black',
            //            //    fontSize: '20px',
            //            //    fontWeight: '600'
            //            //}
            //        },
            //        exporting: { enabled: false },
            //        xAxis: {
            //            categories: downTimeData.Category,
            //            //title: {
            //            //    text: "Down Time ID",
            //            //    style: {
            //            //        color: 'black',
            //            //        fontSize: '17px',
            //            //        fontWeight: '600'
            //            //    }
            //            //},
            //            //labels: {
            //            //    style: {
            //            //        color: 'black',
            //            //        fontSize: '12px',
            //            //        fontWeight: '600'
            //            //    }
            //            //},
            //            gridLineWidth: 1,
            //             crosshair: true
            //        },
            //        yAxis: [{ //--- Primary yAxis
            //            gridLineWidth: 0,
            //            title: {
            //                text: 'Down Time',
            //                style: {
            //                    color: '#1ba1e2',
            //                    fontSize: '17px',
            //                    fontWeight: '600'
            //                }
            //            },
            //            labels: {
            //                overflow: 'justify',
            //                style: {
            //                    color: 'black',
            //                    fontSize: '15px',
            //                    fontWeight: '600'
            //                }
            //            }
            //        }, { //--- Secondary yAxis
            //                gridLineWidth: 0,
            //                title: {
            //                    text: 'Percentage',
            //                    style: {
            //                        color: '#86ba35',
            //                        fontSize: '17px',
            //                        fontWeight: '600'
            //                    }
            //                },
            //                labels: {
            //                    overflow: 'justify',
            //                    format: "{value}%",
            //                    style: {
            //                        color: 'black',
            //                        fontSize: '15px',
            //                        fontWeight: '600'
            //                    }
            //                },
            //                minPadding: 0,
            //                maxPadding: 0,
            //                max: 100,
            //                min: 0,
            //                opposite: true,
            //            },
            //        ],
            //        legend: {
            //            enabled: false
            //        },
            //        credits: {
            //            enabled: false
            //        },
            //        series: downTimeData.series,
            //    });
            //}
            function setShiftDayFonts() {                //var divHeight = $('#shiftDayContainer').height();                //divHeight = divHeight - 300 - 18;                //var rowLength = $('.tblDecanterMachineMonthView tr').length;                //divHeight = divHeight - (18 * rowLength);                //var size = divHeight / (rowLength);                //console.log("div h= " + divHeight);                //console.log("size =" + size);                //if (size > 30) {                //    size = 30;                //}                //var sizeHeader = size + 7;                //console.log("div h= " + divHeight);                //console.log("size =" + size);                //console.log("sizeH =" + sizeHeader);                //$('.tblDecanterMachineMonthView > tbody > tr > td.shiftDayTD').css('font-size', size);                var partLength = $('#machineShoptbl tr').length - 1;                $('#machineShoptbl tr:first-child .tdGreenColor').css('font-size', 30);                $('.machineDecanterHeader').css('font-size', 30);                var divHeight = $('#shiftDayContainer').height() - 45 - $('#currentPartOutputTbl tr:first-child').height() - 50 - 50 - 140;                divHeight = divHeight / 2;                var tdHeight = divHeight / partLength;                $('#currentPartOutputTbl tr:not(:first-child)  td').css("height", tdHeight);                $('#machineShoptbl tr:not(:first-child) td').css("height", tdHeight);                let fontSize = tdHeight - 35;                if (fontSize > 45) {                    fontSize = 45;                }                $('#currentPartOutputTbl tr:not(:first-child) td').css("font-size", fontSize);                $('#machineShoptbl tr:not(:first-child) td').css("font-size", fontSize);                //for (var i = 0; i < $('.tblDecanterMachineMonthView').length; i++) {                //    $($('.tblDecanterMachineMonthView')[i]).find('tr:first > td').css("font-size", sizeHeader)                //}            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                var countItem = 0;

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
                    var flipInterval = getImageVideoFlipInterval();
                    $('.carousel').carousel({
                        pause: "false",
                        interval: flipInterval
                    });
                    //var c = $('.carousel');
                    //opt = c.data()['bs.carousel'].options;
                    //opt.interval = flipInterval;
                    //c.data({ options: opt });
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

                                var c = $('.carousel');
                                opt = c.data()['bs.carousel'].options;
                                opt.interval = 3000;
                                c.data({ options: opt });
                            }
                            debugger;
                            if (countItem == $("#myCarousel .item").length) {
                                if (!isVideo) {
                                    // $( "#btnSave" ).trigger( "click" );
                                    //  window.location.href = "GEA.aspx";
                                    // _doPostBack('btnSave');
                                    setTimeout(function () {
                                        setscreenName();
                                        $("#myCarousel").carousel("pause");
                                        __doPostBack('<%= btnPost.UniqueID%>', '');
                                    }, flipInterval - 1);

                                }
                            }
                        });
                        debugger;
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

                    //if (document.getElementById('downTimeContainer') != null) {
                    //    if ($('#hdnChartsCount').val() == "firstentry") {
                    //        $('#hdnChartsCount').val("completeFirstEntry");
                    //        setPORowsBasedOnHeight();  // called after setHeaderContentSize()
                    //    }
                    //}
                    setShiftDayFonts();
                });
                function pauseTheVideo() {
                    var videos = $("#myCarousel video");
                    for (var i = 0; i < videos.length; i++) {
                        let videoCtrl = videos[i];
                        videoCtrl.pause();
                    }
                }
                function getImageVideoFlipInterval() {
                    var ivFilpInterval = "";
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: '<%= ResolveUrl("HelpRequestProductionAndon.aspx/getImageVideoFlipIntervalValue") %>',
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
                //function setShiftDayFonts() {
                //    var div = $('.decanterMachineDiv')[0];
                //    var divHeight = $(div).height();
                //    divHeight = divHeight - 50 - 10;
                //    var rowLength = $(div).find('.tblDecanterMachineMonthView  tr').length;
                //    divHeight = divHeight - (10 * rowLength);
                //    var size = divHeight / (rowLength);
                //    console.log("div h= " + divHeight);
                //    console.log("size =" + size);
                //    if (size > 30) {
                //        size = 30;
                //    }
                //    var sizeHeader = size + 7;
                //    console.log("div h= " + divHeight);
                //    console.log("size =" + size);
                //    console.log("sizeH =" + sizeHeader);
                //    $('.tblDecanterMachineMonthView td.shiftDayTD').css('font-size', size);

                //    for (var i = 0; i < $('.tblDecanterMachineMonthView').length; i++) {
                //        $($('.tblDecanterMachineMonthView')[i]).find('tr:first td').css("font-size", sizeHeader)
                //    }
                //    var divH
                //}
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

            function clearAllModalScreen() {
                $(".modal-backdrop").removeClass("modal-backdrop in");
                $(".modal").modal("hide");
                $('body').removeClass('modal-open'); // after save/update records in popup, avoid scrollbar disapearing 
                return true;
            }
        </script>
    </form>

</body>
</html>
