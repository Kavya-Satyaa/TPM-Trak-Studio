<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="PPMGraph.aspx.cs" Inherits="Web_TPMTrakDashboard.PPMGraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <%: Scripts.Render("~/bundles/paretoandDrillDownChartJs") %>
    <%: Scripts.Render("~/bundles/paretoChartJs") %>
    <style>
        #container {
            max-width: 100%;
            height: 100%;
            margin: 1em auto;
        }
    </style>
    <div id="Container">
    </div>
    <script>

        $(document).ready(function () {
            GetGraphdetails();
        });
        function GetGraphdetails() {
            var param = "";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PPMGraph.aspx/Getdata",
                data: '{}',
                dataType: "json",
                success: Graph,
                error: function (Result) {
                    alert("Error");
                }
            });
            
        }

        function Graph(Result) {
            

            var PPm = new Array();
            var Componentid = new Array();;
            for (var i = 0; i < Result.d.length; i++)
            {
                PPm.push(Result.d[i]["PPM"]);
                Componentid.push(Result.d[i]["ComponentID"]);
            }
            console.log(PPm);
            console.log(Componentid);
            Highcharts.chart('container', {
                chart: {
                    renderTo: 'container',
                    type: 'column'
                },
                title: {
                    text: 'PPM'
                },
                tooltip: {
                    shared: true
                },
                xAxis: {
                    categories: Componentid,
                    crosshair: true
                },
                yAxis: [{
                    title: {
                        text: ''
                    }
                }, {
                    title: {
                        text: ''
                    },
                    minPadding: 0,
                    maxPadding: 0,
                    max: 100,
                    min: 0,
                    opposite: false,
                    //labels: {
                    //    format: "{value}%"
                    //}
                }],
                series: [{
                    type: 'pareto',
                    name: 'Pareto',
                    yAxis: 1,
                    zIndex: 10,
                    baseSeries: 1
                }, {
                    name: 'PPM',
                    type: 'column',
                    zIndex: 2,
                    data: PPm,
                }]
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
