<%@ Page Title="" Language="C#" MasterPageFile="~/WebAndon/AndonMaster.Master" AutoEventWireup="true" CodeBehind="RunTimeChartAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.WebAndon.RunTimeChartAndon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/highstock.js"></script>
    <script src="../js/moment.js"></script>
    <script src="../js/offline-exporting.js"></script>
    <script src="../js/xrange.js"></script>
    <div>
        <div style="height: 150px;">
            <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
            <asp:HiddenField ID="hdfMode" runat="server" />
            <asp:HiddenField ID="hdfGroupId" runat="server" />
        </div>
        <div id="container-col" style="min-height: 600px; max-height: 100%"></div>
        <div id="pagination" style="height: 10%">
        </div>
    </div>
    <script>
        var parameterTimer = null;
        var mode = "";
        $(document).ready(function () {
            var mode = "<%=mode%>";
            check(mode);
        });
        function BindChart() {
            if($("[id$=txtDate]").val()=="")
            {
                alert("Please Select Date");
                return false;
            }
            var Height = getHeight();
            console.log(JSON.stringify('{PlantID:"' + $("[id$=ddlPlantName] option:selected").val() + '",Date:"' + $("[id$=txtDate]").val() + '",Shift:"",LineID:"' + $("[id$=ddlLine] option:selected").val() + '",Height:"' + Height + '"}'));
            clearInterval(parameterTimer);
            $.ajax({
                type: "POST",
                url: "RunTimeChartAndon.aspx/GetRuntimeData",
                contentType: "application/json;chartset=utf-8",
                data: '{PlantID:"' + $("[id$=ddlPlantName] option:selected").val() + '",Date:"' + $("[id$=txtDate]").val() + '",Shift:"",LineID:"' + $("[id$=ddlLine] option:selected").val() + '",Height:"'+Height+'"}',
                datatype: "json",
                success: function (response) {
                    
                    BuildChart(response.d);
                    paging(response.d.MaxValue);
                },
                error: function (result) {
                    alert("Error" + result.d);
                },
            });
        }

        function getHeight()
        {
            
            var Height = ($("#container-col").height())/50;
            return Height;
        }

        function check(Mode)
        {
            if(Mode=="DESKTOP")
            {
                BindChart()
            }
        }
        function paging(machineval) {
            
            var i = machineval; var j = 1
            while (i > 0) {
                var abc = '<input type="button" value="' + j + '" id="' + j + '" class="paginationbutton" onclick="getdatabyID('+j+')"/>';
                console.log(abc);
                i--; j++;
                $("#pagination").append(abc);
            }
        }

        function getdatabyID(id)
        {
            console.log(id);
            BindChartByID(id);
        }

        function BindChartByID(ID)
        {
            var Height = getHeight();
            
            $.ajax({
                type: "POST",
                url: "RunTimeChartAndon.aspx/GetRuntimeDatabyID",
                contentType: "application/json;chartset=utf-8",
                data: '{PlantID:"' + $("[id$=ddlPlantName] option:selected").val() + '",Date:"' + $("[id$=txtDate]").val() + '",Shift:"",LineID:"' + $("[id$=ddlLine] option:selected").val() + '",ID:"'+ID+'",Height:"'+Height+'"}',
                datatype: "json",
                success: function (response) {
                    BuildChart(response.d);
                },
                error: function (result) {
                    alert("Error" + result.d);
                },
            });
        }
       
        function BindTimerChart(){
            var Height = getHeight();
            var Timerval =<%=TimerVal%>;
            parameterTimer = setInterval(function(){
                $.ajax({
                    type: "POST",
                    url: "RunTimeChartAndon.aspx/GetRuntimeDataAndon",
                    contentType: "application/json;chartset=utf-8",
                    data: '{PlantID:"' + $("[id$=ddlPlantName] option:selected").val() + '",Shift:"",LineID:"' + $("[id$=ddlLine] option:selected").val() + '",Timerval:"'+Timerval+'",Height:"'+Height+'"}',
                    datatype: "json",
                    success: function (response) {
                        BuildChart(response.d);
                    },
                    error: function (result) {
                        alert("Error" + result.d);
                    },
                });
            },5000)
            
        }

        $("ddlPlantName").change(function(){
            
            mode = <%=mode%>;
            check(mode);
            
        });

        $("ddlLine").change(function(){
            
            mode = <%=mode%>;
            check(mode);
        });

        function getchart() {
            
            var id = $(this).text();
            mode = <%=mode%>;
            Bindchart(mode);
        }


        $("[id$=]").change(function () {
            
            mode = <%=mode%>;
            check(mode);
        });

        function BuildChart(data) {
            console.log(data);
            if(data!=null)
            {
               
                Highcharts.chart('container-col', {
                    chart: {
                        type: 'xrange'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        type: 'datetime'
                    },
                    yAxis: {
                        title: {
                            text: ''
                        },

                        categories: data.category,
                    },

                    tooltip: {
                        formatter: function () {
                            let tmpTooltip = '<span style="color:' + this.point.color + '">\u25CF</span> ' + ': <b>' + this.series.chart.yAxis[0].categories[this.y] + '</b><br/> Start Time :  <b>' +
                              Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '</b><br/> End Time :  <b>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x2) + '</b><br/> Status :  <b>' +
                              this.point.status + '</b>';

                            return tmpTooltip;

                        }
                    },

                    navigator: {
                        enabled: true,
                        xAxis: {

                            type: 'datetime'
                        }
                    },

                    exporting: {
                        enabled: false
                    },
                    rangeSelector: {
                        verticalAlign: 'top',
                        x: 0,
                        y: 0,
                        buttons: [{
                            type: 'minute',
                            count: 5,
                            text: '5M'
                        }, {
                            type: 'minute',
                            count: 10,
                            text: '10M'
                        }, {
                            type: 'minute',
                            count: 20,
                            text: '20M'
                        }, {
                            type: 'minute',
                            count: 30,
                            text: '30M'
                        }, {
                            type: 'minute',
                            count: 45,
                            text: '45M'
                        }, {
                            type: 'hour',
                            count: 5,
                            text: '5h'
                        },{
                            type: 'hour',
                            count: 8,
                            text: '8h'
                        }, {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        }, {
                            type: 'hour',
                            count: 20,
                            text: '20h'
                        }, {
                            type: 'all',
                            text: 'All'
                        }],
                        selected: 6,
                        enabled: true,
                        allButtonsEnabled: true,

                        inputEnabled: false
                    },
                    //series: data.series,
                    series: [{
                        name: '',
                        // pointPadding: 0,
                        // groupPadding: 0,
                        borderColor: 'gray',
                        pointWidth: 50,
                        data: data.series.data,
                        turboThreshold: data.series.data.length,
                        //data: [ {"y":1,"x":1516349457000,"x2":1516357084000},{"y":2,"x":1516347595000,"x2":1516350061000},{"y":2,"x":1516367206000,"x2":1516372249000},{"y":4,"x":1516357424000,"x2":1516362888000},{"y":2,"x":1516349405000,"x2":1516353312000},{"y":4,"x":1516352416000,"x2":1516356200000},{"y":4,"x":1516349449000,"x2":1516351853000},{"y":5,"x":1516349235000,"x2":1516352661000},{"y":3,"x":1516350622000,"x2":1516354105000}], 
                        //// dataLabels: {
                        ////    enabled: true
                        ////} 
                    }]

                });
            }
        }

    </script>
</asp:Content>
