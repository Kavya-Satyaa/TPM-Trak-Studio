<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="ToolWiseCycleTimes_Vulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.ToolWiseCycleTimes_Vulkan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/HighCharts9.1.0/highcharts.js"></script>
    <style>
        #tblToolWiseCycleTime th, #tblToolWiseCycleTime {
            color: white;
            background-color: #2E6886 !important;
        }

            #tblToolWiseCycleTime tbody tr td {
                background-color: #FFFFFF;
                color: black;
            }

            #tblToolWiseCycleTime tbody tr td {
                vertical-align: middle;
            }

        /* #tblToolWiseCycleTime tbody tr:last-child td {
                color: white;
                background-color: #2E6886 !important;
            }*/

        #filtertbl tr td {
            color: white;
        }
    </style>


    <div>
        <div style="margin-bottom: 10px;">
            <table style="width: auto" id="filtertbl" class="table table-bordered">
                <tr>
                    <td>Machine: 
                        <asp:Label runat="server" ID="lblMachine"></asp:Label></td>
                    <td>Cycel Start Time:
                        <asp:Label runat="server" ID="lblCycleStartTime"></asp:Label></td>
                    <td>Cycel End Time:
                        <asp:Label runat="server" ID="lblCycleEndTime"></asp:Label></td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" CssClass="btn btn-info btn-sm" Text="Export" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="chartContainer" style="height: 40vh;  margin-bottom: 20px;border-radius:10px;">
        </div>
        <div style="height: 40vh; overflow: auto; width: 60%">
            <asp:ListView runat="server" class="table table-bordered table-hover" ClientIDMode="Static" ID="lvToolWise">
                <LayoutTemplate>
                    <table class="table table-bordered" id="tblToolWiseCycleTime">
                        <tr>
                            <th rowspan="2" style="text-align: center">SI. No.</th>
                            <th rowspan="2" style="text-align: center">Tool</th>
                            <th rowspan="2" style="text-align: center">Start Time</th>
                            <th rowspan="2" style="text-align: center">End Time</th>
                            <th colspan="2" style="text-align: center">Tool Usage
                                <tr>
                                    <th>Ideal</th>
                                    <th>Actual</th>
                                </tr>
                            </th>
                        </tr>
                        <tr id="itemplaceholder" runat="server"></tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("slNo") %></td>
                        <td><%# Eval("Tool") %></td>
                        <td><%# Eval("StartTime") %></td>
                        <td><%# Eval("EndTime") %></td>
                        <td><%# Eval("Ideal") %></td>
                        <td><%# Eval("Actual") %></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            GetToolWiseChart();
        });
        function GetToolWiseChart() {
            $.ajax({
                async: false,
                type: "POST",
                url: "ToolWiseCycleTimes_Vulkan.aspx/GetToolData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itemdata = response.d;
                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    drawToolDataChart(itemdata, sizeLable)
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function drawToolDataChart(itemdata, sizeLable) {
            $("#chartContainer").highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                legend: {
                    enabled: false
                },
                xAxis: {
                    categories:
                        itemdata.Category,
                    labels: {
                        style: {
                            color: 'blue',
                            fontWeight: 'bold',
                            fontSize: 15
                        }
                    },
                    title: {
                        text: 'Tools',
                        style: {
                            color: 'blue',
                            fontWeight: 'bold',
                            fontSize: 15
                        },
                    }
                    //crosshair: true
                },
                yAxis: {
                    min: 200,
                    labels: {
                        style: {
                            color: 'blue',
                            fontWeight: 'bold',
                            fontSize: 15
                        },
                        formatter: function () {
                            var time = this.value;
                            var hours1 = parseInt(time / (60 * 60));
                            var mins1 = parseInt((parseInt(time / 60)) % 60);
                            var secs1 = parseInt(time % (60));
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                        }
                    },
                    title: {
                        text: 'Time (hh:mm:ss)',
                        style: {
                            color: 'blue',
                            fontWeight: 'bold',
                            fontSize: 15
                        }
                    }
                },
                tooltip: {
                    enabled: true,
                    formatter: function () {
                        var time = this.y;
                        var hours1 = parseInt(time / (3600));
                        var mins1 = parseInt((parseInt(time / 60)) % 60);
                        //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        //alert(hours1 + ':' + mins1);
                        var secs1 = parseInt(time % (60));
                        return '<b>' + this.series.name + '</b><br/>' +
                            this.x + ': ' + (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                    }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,

                    }
                },

                series: [{
                    color: 'blue',
                    name: 'Tool',
                    dataLabels: [{
                        //format: '{point.ToolName}',
                        formatter: function () {
                            var value = '';
                            var time = this.y;
                            var hours1 = parseInt(time / 3600);
                            var mins1 = parseInt((parseInt(time / 60)) % 60);
                            var secs1 = parseInt(time % (60));
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                            return value;
                        },
                        //rotation: 90,
                        style: {
                            fontWeight: 'bold',
                            fontSize: 15
                        },
                        enabled: true,
                        x: 3,
                        y: -20

                    }],
                    data: itemdata.Data,
                }]
            })
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
