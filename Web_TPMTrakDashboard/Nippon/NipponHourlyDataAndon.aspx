<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NipponHourlyDataAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.Nippon.NipponHourlyDataAndon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <%--   <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/TextAnimation.css" rel="stylesheet" />--%>
    <link href="Content/NipponStyleSheet.css" rel="stylesheet" />
    <%--<script src="Scripts/Highchart8/highcharts.js"></script>
    <script src="Scripts/Highchart8/pareto.js"></script>
    <script src="Scripts/Highchart8/exporting.js"></script>
    <script src="Scripts/Highchart8/export-data.js"></script>
    <script src="Scripts/Highchart8/accessibility.js"></script>--%>

    <script src="Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <style>
       
    </style>
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="row" style="color: aliceblue; background: maroon 1em">
                <div class="col-lg-2">
                    <img src="../CompanyLogo/ALogo.png" style="object-fit: fill" />
                </div>
                <div class="col-lg-8" style="font-size: 30px; font-weight: bolder; font-family: monospace; text-align: center; vertical-align: central">
                    <span><b>INDIA NIPPON ELECTRICAL  LIMITED
                        <br />
                        REAL TIME OUTPUT MONITORING SYSTEM                    </b></span>
                </div>
                <div class="col-lg-2" style="float: right; font: 20px bolder monospace">
                    <table>
                        <tr style="height: 20px">
                            <td>
                                <span><b>Cell:</b></span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="drpcellid" ClientIDMode="Static" />
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                <span><b>Date :</b></span>
                            </td>
                            <td>
                                <span id="lblDate"></span>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                <span>Shift :</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblShift" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="panel">
                <table id="AndonTable" style="width: 90%; border: solid 1px black; margin: 50px; border-width: 5px;">
                    <thead>
                        <tr>
                            <th class="headerstyle">MACHINE-ID</th>
                            <th class="headerstyle">Target</th>
                            <th class="headerstyle">Actual</th>
                            <th class="headerstyle">PRODUCTION</th>
                            <th class="headerstyle">OEE</th>
                            <th class="headerstyle">PRODUCTIVITY</th>
                            <th class="headerstyle">AVALIBILITY</th>
                            <th class="headerstyle">QUALITY</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </form>
    <script>
        $(document).ready(function () {
            setInterval(function () {
                BindData();
            }, 10000);
        });

        function BindData() {
            var cell = $("#drpcellid").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "NipponHourlyDataAndon.aspx/GETDATA",
                data: '{CellID:"' + cell + '"}',
                dataType: "json",
                success: function (result) {
                    var itmData = result.d;
                    var body = "<tbody>";
                    $("#AndonTable tbody tr").each(function () {
                        $(this).remove();
                    });
                    if ($(itmData).length > 0) {
                        $(itmData).each(function (index, ent) {
                            debugger;
                            body += "<tr><td class=" + "headerstyle" + ">" + ent.MachineID + "</td>";
                            body += "<td> <table>";
                            var actual = ent.ActualData;
                            var Target = ent.TargetData;
                            debugger;
                            $(actual).each(function (innerindex, ent2) {
                                debugger;
                                body += "<tr><td class=" + "rowstyle" + " > " + ent2 + "</td></tr>";
                            });
                            body += "</table></td><td><table>"
                            $(Target).each(function (innerindex, ent2) {
                                body += "<tr><td class=" + "rowstyle" + " > " + ent2 + "</td></tr>";
                            });
                            body += "</table> </td><td class =" + "rowstyle" + ">" + ent.Production + "</td>";
                            body += "<td class =" + "rowstyle" + ">" + ent.OEE + "</td>";
                            body += "<td class =" + "rowstyle" + ">" + ent.Productivity + "</td>";
                            body += "<td class =" + "rowstyle" + ">" + ent.Avaliability + "</td>";
                            body += "<td class =" + "rowstyle" + ">" + ent.Quality + "</td></tr>";
                        });
                        body += "</tbody>";
                        $("#AndonTable").append(body);
                    }
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

    </script>
</body>
</html>
