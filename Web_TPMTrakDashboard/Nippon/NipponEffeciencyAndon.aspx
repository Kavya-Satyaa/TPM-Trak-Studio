<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NipponEffeciencyAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.Nippon.NipponEffeciencyAndon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Content/HighChart.js"></script>
    <script src="Content/HighChart_More.js"></script>
    <script src="Content/Solid_gauge.js"></script>
    <script src="Content/Accessibility.js"></script>

    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <%--   <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/TextAnimation.css" rel="stylesheet" />--%>
    <link href="Content/NipponStyleSheet.css" rel="stylesheet" />
    <%--<script src="Content/Export_data.js"></script>--%>


    <script src="Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <style>
        * {
            font-family: Arial;
        }

        .HeaderImage {
            flex: 1;
            float: left;
        }


        #filterInfoTbl tr td {
            white-space: nowrap;
        }

        .hourlyDataTbl .rowstyle1 {
            padding: 0px;
        }

        #divTable .card-head {
            line-height: 1;
        }

        #divTable .rowstyle, #divTable .headerstyleMac {
            padding: 0px;
            height: unset;
        }

        .machineBigImg {
            height: 290px;
            width: 390px;
        }

        .machineSmallImg {
            height: 175px;
            width: 240px;
        }

        #machineDataTbl1 > tr > td ,#machineDataTbl2  > tr > td {
            padding: 0px 10px 30px 0px;
        }
        #machineDataTbl2  > tr > td {
            padding: 0px 10px 0px 0px;
        }
        #machineDataTbl1 > tr:last-child > td  {
            padding-bottom: 0px;
        }

        #divMachine {
            background-repeat: no-repeat;
            background-size: cover;
            width: 100%;
            height: 80vh;
        }

        /* #divMachine {
            z-index: 1;
        }

            #divMachine:before {
                content: "";
                position: absolute;
                z-index: -1;
                top: 0;
                bottom: 0;
                left: 0;
                right: 0;
                background-repeat: no-repeat;
                background-size: 100%;
                opacity: 0.2;
                filter: alpha(opacity=40);
                height: 100%;
                width: 100%;
            }


        #divMachineImage {
            z-index: -1;
            opacity: 1;
            filter: alpha(opacity=100);
        }*/

        #AndonTable1 .headerstyle {
            height: unset;
            font-size: 23px;
            padding: 7px;
        }
        .ThirdScreenred {
            color: red;
           
        }
        .ThirdScreenyellow {
            color: orange;
        }
        .ThirdScreengreen {
            color: #098e09;
        }
        .ProdOEElbl {
            background-color: white;
            margin-bottom: 0px;
            border: 1px solid #948d8d;
            padding: 0px 10px;
            border-radius:5px;
        }
    </style>
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="headerDiv" class="row" style="color: black; background: #BDD7EE 1em; margin: auto; margin-top: 5px; width: 99.9%">
                <div class="col-lg-2">
                    <div class="HeaderImage">
                        <%-- <img src="Images/SPFLogo.PNG" height="60" style="padding: 3px;" />--%>
                        <asp:Image ID="Image2" runat="server" class="img-responsive img-rounded" Style="width: 200px; height: 80px; margin-top: 2px" />
                    </div>
                </div>
                <div class="col-lg-7" style="font-size: 30px; font-weight: bolder; text-align: center; vertical-align: central">
                    <%--  <span style="vertical-align: sub;"><b>INDIA NIPPON ELECTRICAL  LIMITED
                        <br />
                        REAL TIME OUTPUT MONITORING SYSTEM
                    </b></span>--%>
                    <div>
                        INDIA NIPPON ELECTRICALS  LIMITED
                    </div>
                    <div>
                        REAL TIME OUTPUT MONITORING SYSTEM
                    </div>
                </div>
                <div class="col-lg-2" style="float: right; font: 20px bolder; float: right;">
                    <table>
                        <tr>
                            <td>
                                <table id="filterInfoTbl">
                                    <tr style="height: 20px">
                                        <td>
                                            <span><b>Cell:</b></span>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="drpcellid" ClientIDMode="Static" CssClass="form-control" Font-Bold="true" Font-Size="Medium" />
                                        </td>
                                        <td rowspan="3"></td>
                                    </tr>
                                    <tr style="height: 20px">
                                        <td>
                                            <span><b>Date :</b></span>
                                        </td>
                                        <td>
                                            <b><span id="lblDate"></span></b>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px">
                                        <td>
                                            <span><b>Shift :</b></span>
                                        </td>
                                        <td>
                                            <b><span id="ShiftName"></span></b>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <p style="text-align: center;display:none"><i class="glyphicon glyphicon-cog" onclick="return goToSetting();"></i></p>
                                <span style="height: 20px; margin: 5px; padding: 20px; cursor: pointer; color: white;" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span>
                            </td>

                        </tr>
                    </table>

                </div>
                <div class="col-lg-1" style="float: right">
                    <img runat="server" src="~/Images/logo/AMITLogo.png" id="toggle" class="img-responsive img-rounded" alt="Logo" style="cursor: pointer; padding-right: 1px; margin-top: 4px; float: right; height: 75px" />
                </div>
            </div>
            <div>
                <div class="row clearfix" id="divTable" style="width: 99%; margin: auto">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                        <div class=" card">
                            <div class="card-head">
                                <table id="AndonTable1" style="width: 95%; vertical-align: central; border: solid 2px black; margin: 10px auto; border-width: 2px; border-radius: 15px">
                                    <thead>
                                        <tr>
                                            <th class="headerstyle" rowspan="2">MACHINE-ID</th>
                                            <th class="headerstyle" colspan="3">HOURLY</th>
                                            <th class="headerstyle" colspan="6">CUMMULATIVE (Shift)</th>

                                        </tr>
                                        <tr>
                                            <th class="headerstyle">TARGET</th>
                                            <th class="headerstyle">ACTUAL</th>
                                            <th class="headerstyle">OEE (%)</th>
                                            <th class="headerstyle">TARGET</th>
                                            <th class="headerstyle">PRODUCTION ACTUAL</th>
                                            <th class="headerstyle">OEE (%)</th>
                                            <th class="headerstyle">PRODUCTIVITY (%)</th>
                                            <th class="headerstyle">AVALIBILITY (%)</th>
                                            <th class="headerstyle">QUALITY (%)</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row clearfix" id="divEfficiency" style="width: 99%; margin: auto">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                        <div class=" card">
                            <div class="card-head">
                                <table id="AndonTable" style="width: 95%; border: solid 1px black; margin: 10px auto; border-width: 2px;">
                                    <thead>
                                        <tr>
                                            <%-- <th class="headerstyle">MACHINE-ID</th>
                            <th class="headerstyle">Target</th>
                            <th class="headerstyle">Actual</th>
                            <th class="headerstyle">PRODUCTION</th>
                            <th class="headerstyle">OEE</th>
                            <th class="headerstyle">PRODUCTIVITY</th>
                            <th class="headerstyle">AVALIBILITY</th>
                            <th class="headerstyle">QUALITY</th>--%>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <%-- <div id="divMachine" style="width: 99%; margin: auto; height: 100%; background-size: 100% 100%">--%>
                <div class="row clearfix" id="divMachine" style="width: 99%; margin: auto; height: 100%; background-size: 100% 100%">
                    <%--  <div class="row clearfix" id="divMachineImage">--%>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-6">
                        <table id="machineDataTbl1" style="width: 95%; vertical-align: central; margin: 20px auto; border-width: 2px; border-radius: 15px">
                        </table>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-6">
                        <table id="machineDataTbl2" style="width: 95%; vertical-align: central; margin: 20px auto; border-width: 2px; border-radius: 15px">
                        </table>
                        <%-- </div>--%>
                    </div>
                </div>
            </div>
        </div>

    </form>
    <script>
        var FlipData = <%= FlipInterval%>;
        var RefreshData = <%= RefreshDataInt%>;
        function RefreshDataFun() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "NipponEffeciencyAndon.aspx/RefreshData",
                data: '',
                dataType: "json",
                success: function (result) {
                    BindData();
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }
        $(document).ready(function () {

            /* document.body.style.zoom = "60%";*/
            var CellID = getCookie("NipponEffAndon");
            $("#drpcellid").val(CellID);
            BindData();
            var now = new Date();
            $("#lblDate").text(now.getDate() + '/' + (now.toLocaleString('default', { month: 'short' })) + '/' + now.getFullYear() + " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds());
            setInterval(function () {
                BindData();
                var now = new Date();
                $("#lblDate").text(now.getDate() + '/' + (now.toLocaleString('default', { month: 'short' })) + '/' + now.getFullYear() + " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds());
            }, (FlipData * 1000));
            setInterval(function () {
                RefreshDataFun();
            }, (RefreshData * 60 * 1000));
        });
        function goToSetting() {
            location.href = 'AndonSetting.aspx';
        }
        $("#drpcellid").change(function () {
            var d = new Date();
            d.setTime(d.getTime() + (999 * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = "NipponEffAndon" + "=" + $("#drpcellid").val() + ";" + expires + ";path=/";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "NipponEffeciencyAndon.aspx/RefreshData",
                data: '',
                dataType: "json",
                success: function (result) {
                    BindData();
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        })
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
            //document.body.style.backgroundColor = "#202648";
            //document.body.style.background = "#202648";
        });
        function getCookie(cname) {
            var name = cname + "=";
            var decodedCookie = decodeURIComponent(document.cookie);
            var ca = decodedCookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }
        function BindData() {
            var cell = $("#drpcellid").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "NipponEffeciencyAndon.aspx/GETDATA",
                data: '{CellID:"' + cell + '", ScreenHeight:"' + screen.height + '"}',
                dataType: "json",
                success: function (result) {
                    var MainData = result.d;
                    if ((MainData).Screen == "FirstScreen") {
                        $("#divTable").css("display", "");
                        $("#divEfficiency").css("display", "none");
                        $("#divMachine").css("display", "none");
                        $("#AndonTable").css('visibility', 'collapse');
                        $("#AndonTable1").css('visibility', 'visible');
                        $("#machineDataTbl").css('visibility', 'collapse')
                        var itmData = (MainData).FirstScreen;
                        var body = "<tbody>";
                        $("#ShiftName").text($(itmData)[0].Shift);
                        $("#AndonTable1 tbody tr").each(function () {
                            $(this).remove();
                        });
                        if ($(itmData).length > 0) {
                            $(itmData).each(function (index, ent) {
                                body += "<tr style='background-color: #3a405b;'><td class=" + "headerstyleMac" + ">" + ent.MachineID + "</td>";
                                body += "<td> <table style='width:100%' class='actualDatatbl hourlyDataTbl' >";
                                var actual = ent.TargetData;
                                var Target = ent.ActualData;
                                var OEEData = ent.OEEData;
                                $(actual).each(function (innerindex, ent2) {
                                    body += "<tr><td  class=" + "rowstyle1" + "> " + ent2 + "</td></tr>";
                                });
                                body += "</table></td><td><table style='width:100%' class='targetDatatbl hourlyDataTbl'>"
                                $(Target).each(function (innerindex, ent2) {
                                    body += "<tr><td class=" + "rowstyle1" + " > " + ent2 + "</td></tr>";
                                });
                                body += "</table></td><td><table style='width:100%' class='OEEDatatbl hourlyDataTbl'>";
                                var OEEBackColor = ent.OEEDataBackColor;

                                if (OEEData.length > 0) {
                                    let OEEColor = "black";
                                    if (OEEBackColor[0] == "red") {
                                        OEEColor = "white";
                                    }
                                    body += '<tr colspan="' + OEEData.length + '"><td class="rowstyle1"  style="color:' + OEEColor + ';background:' + OEEBackColor[0] + ';border:2px solid #3a405b"> ' + OEEData[0] + '</td></tr>';
                                }
                                //$(OEEData).each(function (innerindex, ent2) {
                                //    let OEEColor = "black";
                                //    if (OEEBackColor[innerindex] == "red") {
                                //        OEEColor = "white";
                                //    }
                                //    body += '<tr><td class="rowstyle1"  style="color:' + OEEColor + ';background:' + OEEBackColor[innerindex] + ';border:2px solid #3a405b"> ' + ent2 + '</td></tr>';
                                //});
                                body += "</table>";
                                body += "<td class=" + " rowstyle" + " > " + ent.CummulativeTarget + "</td > ";
                                body += "<td class=" + " rowstyle" + " > " + ent.Production + "</td > ";
                                body += "<td class =" + "rowstyle" + " style=" + "color:" + ent.backcolor + ";background:" + ent.color + ">" + ent.OEE + "</td>";
                                body += "<td class =" + "rowstyle" + ">" + ent.Productivity + "</td>";
                                body += "<td class =" + "rowstyle" + ">" + ent.Avaliability + "</td>";
                                body += "<td class =" + "rowstyle" + ">" + ent.Quality + "</td></tr>";
                            });
                            body += "</tbody>";
                            $("#AndonTable1").append(body);

                            debugger;
                            var rowHeight = $("#AndonTable1 > tbody > tr").length + 1;
                            var actualTblRows = $('.actualDatatbl tr td').length - (rowHeight - 1);
                            var maxActualTargetTblRows = 0;
                            maxActualTargetTblRows = actualTblRows;
                            //var targetTblRows = $('.targetDatatbl tr td').length - (rowHeight - 1);
                            //
                            //if (actualTblRows >= targetTblRows) {
                            //    maxActualTargetTblRows = actualTblRows;
                            //}
                            //else {
                            //    maxActualTargetTblRows = targetTblRows;
                            //}
                            rowHeight += maxActualTargetTblRows;
                            var wHeight = $(window).height() - $('#headerDiv').height() - 140;
                            rowHeight = wHeight / rowHeight;
                            $(".hourlyDataTbl td").css("height", rowHeight);
                            let fontsize = rowHeight - 10;
                            if (fontsize > 30) {
                                fontsize = 30;
                            }
                            $('.hourlyDataTbl tr td').css("font-size", fontsize);
                            for (var i = 0; i < $('#AndonTable1 > tbody > tr').length; i++) {
                                var row = $('#AndonTable1 > tbody > tr')[i];
                                var hgt = $(row).find(".headerstyleMac").height();
                                $(row).find(".OEEDatatbl td").css("height", hgt + 4);
                            }
                            
                          
                        }
                    }
                    else if ((MainData).Screen == "SecondScreen") {
                        $("#divTable").css("display", "none");
                        $("#divEfficiency").css("display", "");
                        $("#divMachine").css("display", "none");
                        $("#AndonTable").css('visibility', 'visible');
                        $("#AndonTable1").css('visibility', 'collapse');
                        $("#machineDataTbl").css('visibility', 'collapse')
                        var itmData = (MainData).SecondScreen;
                        var body = "<thead><tr><th class=" + "headerstyle2" + ">Pattern<br/>( CUM )</th>";
                        $("#AndonTable thead th").each(function () {
                            $(this).remove();
                        });
                        var MachineList = $(itmData)[0].MachineList;
                        if ($(MachineList).length > 0) {
                            $(MachineList).each(function (index, ent) {
                                body += "<th class=" + "headerstyle3 " + ">" + ent + "</th >";
                            });
                            body += "</thead><tbody>";
                        }
                        $("#ShiftName").text($(itmData)[0].Shift);
                        $("#AndonTable tbody tr").each(function () {
                            $(this).remove();
                        });

                        body += "<tr><td class=" + "rowstyle2" + "> <b>AVAILABILITY<br/>" + $(itmData)[0].OverallAE + "(%)<b/></td>";
                        var AEList = $(itmData)[0].AEList;
                        if ($(AEList).length > 0) {
                            $(AEList).each(function (index, ent) {
                                body += "<td class=" + "rowstyle2" + ">" + "<div id=AE" + index + "></div>" + "</td>";
                            });
                        }
                        body += "</tr ><tr ><td class=" + "rowstyle2" + "> <b>PRODUCTIVITY<br/>" + $(itmData)[0].OverallPE + "(%)<b/></td>";
                        var PEList = $(itmData)[0].PEList;
                        if ($(PEList).length > 0) {
                            $(PEList).each(function (index, ent) {
                                body += "<td class=" + "rowstyle2" + ">" + "<div id=PE" + index + "></div>" + "</td>";
                            });
                        }
                        body += "</tr ><tr><td class=" + "rowstyle2" + "><b>QUALITY<br/>" + $(itmData)[0].OverallQE + "(%)<b/></td>";
                        var QEList = $(itmData)[0].QEList;
                        if ($(QEList).length > 0) {
                            $(QEList).each(function (index, ent) {
                                body += "<td class=" + "rowstyle2" + ">" + "<div id=QE" + index + "></div>" + "</td>";
                            });
                        }
                        body += "</tr><tr><td class=" + "rowstyle2" + "><b>OEE<br/>" + $(itmData)[0].OverallOEE + "(%)<b/></td>";
                        var OEEList = $(itmData)[0].OEEList;
                        if ($(OEEList).length > 0) {
                            $(OEEList).each(function (index, ent) {
                                body += "<td class=" + "rowstyle2" + ">" + "<div id=OEE" + index + "></div>" + "</td>";
                            });
                        }
                        body += "</tr></tbody>";
                        $("#AndonTable").append(body);
                        var chartWidth = $('#AndonTable .rowstyle2').width();
                        if ($(AEList).length > 0) {
                            $(AEList).each(function (index, ent) {
                                GetGraph(ent, "AE", index, chartWidth);
                            });
                        }
                        if ($(PEList).length > 0) {
                            $(PEList).each(function (index, ent) {
                                GetGraph(ent, "PE", index, chartWidth);
                            });
                        }
                        if ($(QEList).length > 0) {
                            $(QEList).each(function (index, ent) {
                                GetGraph(ent, "QE", index, chartWidth);
                            });
                        }
                        if ($(OEEList).length > 0) {
                            $(OEEList).each(function (index, ent) {
                                GetGraph(ent, "OEE", index, chartWidth);
                            });
                        }
                    }
                    else if ((MainData).Screen == "ThirdScreen") {
                        $("#divTable").css("display", "none");
                        $("#divEfficiency").css("display", "none");
                        $("#divMachine").css("display", "");
                        $("#AndonTable").css('visibility', 'collapse');
                        $("#AndonTable1").css('visibility', 'collapse');
                        $("#machineDataTbl1").css('visibility', 'visible');
                        $("#machineDataTbl2").css('visibility', 'visible');
                        $("#machineDataTbl1").empty();
                        $("#machineDataTbl2").empty();
                        var itmData = (MainData).ThirdScreen;
                        var MachineList = itmData.MachineDetails;
                        var bodystr1 = "", bodystr2 = "";
                        debugger;

                        if (MachineList.length > 0) {
                            for (var i = 0; i < MachineList.length; i++) {

                                if ((i + 1) % 2 != 0) {
                                    if (i == 0) {
                                        bodystr1 += '<tr>';
                                        bodystr2 += '<tr>';
                                    }
                                    else {
                                        bodystr1 += '</tr><tr>';
                                        bodystr2 += '</tr><tr>';
                                    }
                                }
                                var imagClass = "";
                                if (i <= 3) {
                                    imagClass = "machineBigImg";
                                }
                                else {
                                    imagClass = "machineSmallImg";
                                }
                                bodystr1 += '<td style="text-align:center;width:50%"><table  style="width:100%;margin:auto"><tr><td style="text-align:center;font-size:35px;font-weight:800">' + MachineList[i].MachineID + '</td></tr>';
                                bodystr1 += '<tr><td style="text-align:center;font-size:30px;font-weight:700" ><label class="ProdOEElbl ThirdScreen' + MachineList[i].FontColor + '">' + MachineList[i].ProdutionOEE + '</label></td></tr>';
                               /* color: ' + MachineList[i].FontColor + ';*/
                                bodystr1 += '<tr><td><img src="' + MachineList[i].ImagePath + '" class="' + imagClass + '" /></td></tr></table></td>';
                                if (i == 3) {
                                    $("#machineDataTbl1").append(bodystr1);
                                    bodystr1 = "";
                                }
                                if (i == MachineList.length - 1) {
                                    $("#machineDataTbl2").append(bodystr1);
                                }
                            }
                        }
                        var wHeight = $(window).height() - $('#headerDiv').height() - 265;
                        $('.machineBigImg').css("height", wHeight / 2);
                        wHeight = $(window).height() - $('#headerDiv').height() - 330;
                        $('.machineSmallImg').css("height", wHeight / 3);
                        $("#divMachine").css("background-image", "url('" + itmData.BackgroundImgPath + "')");
                        // $('head').append('<style>#divMachine:before{background-image:url("' + itmData.BackgroundImgPath + '");}</style>');
                        console.log("path" + itmData.BackgroundImgPath);
                        $("#ShiftName").text(itmData.Shift);
                    }
                },
                error: function (Result) {
                    alert("Error Retreving Data");
                }
            });
        }

        var gaugeOptions = {
            //chart: { type: 'gauge', height: 180, width: 280 },
            chart: { type: 'gauge', },
            title: null,
            pane: {
                size: '160%',
                center: ['50%', '90%'],
                startAngle: -100,
                endAngle: 100,
                background: {
                    backgroundColor: 'black',
                    innerRadius: '100%',
                    outerRadius: '100%',
                    shape: 'arc'
                }
            },
            tooltip: { enabled: false },

            // the value axis
            yAxis: {

                lineWidth: 0,
                minorTickInterval: 0,
                tickPixelInterval: 50,
                tickWidth: 1,
                labels: {
                    enabled: true,
                    distance: 10
                },
                plotBands: [{
                    from: 75,
                    to: 100,
                    color: '#00B050' // green
                }, {
                    from: 50,
                    to: 75,
                    color: '#FFFF00' // yellow
                }, {
                    from: 0,
                    to: 50,
                    color: '#FF0000' // red
                }]
            },
            plotOptions: {
                solidgauge: {
                    innerRadius: '82%',
                    dataLabels: {
                        y: 125,
                        borderWidth: 0,
                        useHTML: true
                    }
                }
            }
        };

        function GetGraph(Data, type, index, chartWidth) {
            var MachineID = Data.MachineID
            var div = type + index;
            var value = [];
            if (type == "AE") {
                var val = Data.AE[index]
                value[0] = val
            }
            else if (type == "PE") {
                var val = Data.PE[index]
                value[0] = val
            }
            else if (type == "QE") {
                var val = Data.QE[index]
                value[0] = val
            }
            else if (type == "OEE") {
                var val = Data.OEE[index]
                value[0] = val
            }
            var wHeight = $(window).height() - $('#headerDiv').height() - 270;
            var chartHeight = wHeight / 4;
            console.log(chartWidth);
            Highcharts.chart(div, Highcharts.merge(gaugeOptions, {
                chart: {
                    height: chartHeight,
                    width: chartWidth
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    style: {
                        fontsize: '12px',
                        color: '#000000'
                    }
                },
                title: {
                    text: type,
                    style: {
                        color: '#000000',
                        fontWeight: 'bold',
                        fontsize: 22,
                    },
                    margin: -100,
                    x: 0,
                    y: 50,
                },
                credits: {
                    enabled: false
                },

                series: [{
                    name: type,
                    data: value,
                    dataLabels: {
                        y: 100,
                        borderWidth: 0,
                        format:
                            '<div class="row" style="text-align:center;vertical-align: middle;margin-top:120px;">' +
                            '<span style="font-size:40px;">{y}%</span>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: '%'
                    }
                }]

            }));
        }
    </script>
</body>
</html>
