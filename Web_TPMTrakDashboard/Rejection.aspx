<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rejection.aspx.cs" Inherits="Web_TPMTrakDashboard.Rejection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>

    <%: Scripts.Render("~/bundles/paretoandDrillDownChartJs") %>
    <%: Scripts.Render("~/bundles/paretoChartJs") %>

    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }


        .colorTotal tr td:last-child {
            background-color: #FFFF00;
            font-weight: bold;
        }

        .colorTotal tbody tr:last-child {
            background-color: #FFFF00;
            font-weight: bold;
        }

        .multiselect {
            width: 150px;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
            text-align: left;
            width: 250px;
        }

        .multiselect-selected-text {
            padding-right: 181px;
            white-space: normal;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .commontd {
            min-width: 120px;
        }

        .drp {
            min-width: 120px;
        }

        .cal {
            min-width: 100px;
        }

        .multiselect-selected-text {
            padding-right: 88px;
        }

        @media only print {
            body * {
                visibility: hidden;
            }

            #container-col, #container-col * {
                visibility: visible;
            }

            #container-col {
                position: absolute;
                left: 0;
                top: 0;
            }
        }
    </style>

    <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <div>
        <asp:UpdatePanel ID="update" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnHideExport" />
            </Triggers>
            <ContentTemplate>


                <asp:HiddenField ID="hdfValue" runat="server" />
                <asp:Button ID="btnHideExport" runat="server" OnClick="btnHideExport_Click" Style="display: none" />
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
                <table class="table table-bordered" id="tblfilter">

                    <tr>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                        <td class="input-group" style="width: 200px">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date cal" data-toggle="tooltip"
                                title="From Date !" placeholder="From Date" ToolTip="<%$Resources:CommanResource, FromDate %>"></asp:TextBox>
                        </td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                        <td class="input-group" style="width: 200px">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date cal" data-toggle="tooltip"
                                title="To Date !" placeholder="To Date" ToolTip="<%$Resources:CommanResource, ToDate %>"></asp:TextBox>
                        </td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","PlantID")%></b></td>
                        <td>
                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Plant ID !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" ToolTip="<%$Resources:CommanResource, PlantTooltip %>">
                            </asp:DropDownList></td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                        <td>
                            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control drp" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList></td>
                        <td colspan="4" style="text-align: left"></td>
                    </tr>

                    <tr>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","MachineId")%></b></td>
                        <td style="text-align: left; width: 130px;">
                            <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control drp" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged"></asp:DropDownList></td>

                        <td class="commontd">
                            <b>Component ID</b>
                        </td>
                        <td class="commontd">
                            <asp:DropDownList ID="ddlComponentID" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="ComponentID !" AutoPostBack="true" OnSelectedIndexChanged="ddlComponentID_SelectedIndexChanged" />
                        </td>
                        <td class="commontd">
                            <b>Operation</b>
                        </td>
                        <td class="commontd">
                            <asp:DropDownList ID="ddlOperation" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Operation !" />
                        </td>

                        <td class="commontd">
                            <b>Show Top Reasons </b></td>
                        <td>
                            <asp:DropDownList ID="ddlTopDownReasons" runat="server" CssClass="select form-control">
                                <asp:ListItem Value="5">Top 5</asp:ListItem>
                                <asp:ListItem Value="10">Top 10</asp:ListItem>
                                <asp:ListItem Value="15">Top 15</asp:ListItem>
                                <asp:ListItem Value="20" Selected="True">Top 20</asp:ListItem>
                                <asp:ListItem Value="25">Top 25</asp:ListItem>
                                <asp:ListItem Value="30">Top 30</asp:ListItem>
                                <asp:ListItem Value="35">Top 35</asp:ListItem>
                                <asp:ListItem Value="40">Top 40</asp:ListItem>
                                <asp:ListItem Value="45">Top 45</asp:ListItem>
                                <asp:ListItem Value="50">Top 50</asp:ListItem>
                            </asp:DropDownList></td>

                        <td colspan="4" style="text-align: left"></td>
                    </tr>

                    <tr>
                        <td class="commontd">
                            <b>Category</b>
                        </td>
                        <td class="commontd" style="width: 200px">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Category !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        <%-- <td class="commontd">
                            <b>Sub Category</b>
                        </td>
                        <td class="commontd" style="width: 200px">
                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Category !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        <td class="commontd">
                            <b>Sub Sub Category</b>
                        </td>
                        <td class="commontd" style="width: 200px">
                            <asp:DropDownList ID="ddlDesCategory" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Category !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlDesCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>--%>
                        <td class="commontd">
                            <b>Rejection ID  </b>
                        </td>
                        <td class="commontd" style="text-align: left; width: 200px;">
                            <asp:ListBox ID="ddlMultiDownID" Style="z-index: 100" runat="server" SelectionMode="Multiple" CssClass="form-control drp" ToolTip="<%$Resources:CommanResource, ALL %>"></asp:ListBox>
                        </td>

                        <td colspan="4" style="text-align: left">
                            <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" class="btn btn-info btn-md" ID="btnProcess" OnClick="btnProcess_Click"></asp:Button>
                        </td>
                    </tr>

                </table>
                </div>
            
           
            </ContentTemplate>
        </asp:UpdatePanel>

        <span id="print" style="float: right; margin: 3px;">
            <button class="btn btn-info btn-sm glyphicon glyphicon-print" onclick="window.print();return false;"></button>
            &nbsp;
        <button class="btn btn-info btn-sm glyphicon glyphicon-export" id="btnExport"></button>
        </span>

        <%--  <div>
        <div id="container-col" style="min-height: 600px; max-height: 100%; width:50%;float:left;margin-top:30px;padding:10px"></div>
        <div id="container-col1" style="min-height: 600px; max-height:100%; width:50%;float:right;padding:10px"></div>
    </div>--%>

        <div id="container-col" style="min-height: 600px; padding: 10px"></div>




    </div>
    <script>
        var countDrillDown = 0;
        $(document).ready(function () {
            //$('[id$=ddlMachineId]').multiselect({
            //    includeSelectAllOption: true
            //});
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            //BindGraph();
        });

        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        function compareDates(date1, date2) {
            return invert(date1).localeCompare(invert(date2));
        }

        $(document).on("click", "[id$=btnProcess]", function () {
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            var dateCom = compareDates(from, to);
            if (dateCom == 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            $.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif" />' });
            GetRejectionParetoData();
            // GetRejectionDrillDownData();
        });

        $(document).on("click", "#liMenu", function () {

            var chart6 = $('#container-col').highcharts();
            chart6.reflow();

        });

        function BindGraph() {
            if ($("[id$=hdfValue]").val() == "OK") {
                GetRejectionParetoData();
                //GetRejectionDrillDownData();
            }
        }

        function GetRejectionDrillDownData() {
            $.ajax({
                type: "POST",
                url: "Rejection.aspx/RejectionDrillDownInfo",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=ddlComponentID]").val() + '",Operation:"' + $("[id$=ddlOperation]").val() + '",Category:"' + $("[id$=ddlCategory]").val() + '",SubCategory:"",DesCategory:"",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '"}',
                dataType: "json",
                success: OnSuccessDrillDownData,
                //success:OnSuccessLoadDrillDownData,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }
        var DDchart = null;

        function OnSuccessDrillDownData(Result) {
            
            var data = Result.d;
            console.log("chart result" + Result);
            var chart = Highcharts.chart('container-col', {
                chart: {
                    type: 'column',
                    credits: {
                        enabled: false
                    },
                    height: 600,
                    events: {
                        drillup: function (e) {
                            
                            //chart.setTitle({ text: e.point.beforeTitle });
                            if (e.seriesOptions.id == undefined) {
                                xlabel = "";
                                countDrillDown = 0;
                                chart.setTitle({ text: data.XAxisTitle });
                                //chart.title.attr({
                                //    text: data.XAxisTitle
                                //});
                            } else {
                                chart.setTitle({ text: e.seriesOptions.data[0].beforeTitle });
                                //chart.title.attr({
                                //    text: e.seriesOptions.data[0].beforeTitle
                                //});
                            }
                        },
                        drilldown: function (e) {
                            
                            chart.setTitle({ text: e.point.Title });
                            //chart.XAxisTitle = e.point.afterTitle;
                            //chart.title.text = e.point.afterTitle;
                            //countDrillDown++;
                            //chart.title.attr({
                            //    text: e.point.afterTitel
                            //});
                        },

                    }
                },
                title: {
                    text: 'Rejection Chart',
                    useHTML: true,
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Rejection Qty'
                    }

                },
                legend: {
                    enabled: false
                },
                tooltip: {
                    headerFormat: '<span style="font-size:15px"><b>{series.name}</b></span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b><br/>'
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y}'
                            //format: '{point.y:.1f}'
                        },
                        point: {
                            events: {
                                click: function () {
                                    
                                    var str = this.afterTitle;
                                    chart.setTitle({ str });
                                    if (this.drilldown == "") {
                                        var color = this.color;
                                        var category = str.split("*-*");
                                        BindDrillDownChart("ByRejcode", category[0], category[1], category[2], color);
                                    }
                                }
                            }
                        }

                    }

                },

                series: data.series,
                drilldown: {
                    series: data.drilldown,
                }

            });
            // BindColumnAndPieChart(data);
            $.unblockUI({});
        }




        function GetRejectionParetoData() {
            
            var abc = JSON.stringify('{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=ddlComponentID]").val() + '",Operation:"' + $("[id$=ddlOperation]").val() + '",Category:"' + $("[id$=ddlCategory]").val() + '",SubCategory:"",RejDescription:"",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '"}');
            console.log(abc);
            $.ajax({
                type: "POST",
                url: "Rejection.aspx/RejectionChartInfo",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=ddlComponentID]").val() + '",Operation:"' + $("[id$=ddlOperation]").val() + '",Category:"' + $("[id$=ddlCategory]").val() + '",SubCategory:"",RejDescription:"",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '"}',
                dataType: "json",
                success: OnSuccessDownTimePareto,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function OnSuccessDownTimePareto(Result) {
            
            var data = Result.d;
            BindColumnAndPieChart(data);
            $.unblockUI({});
        }

        function BindColumnAndPieChart(data) {
            // 

            var splite = "Rejection Code";
            
            var catValue = '';
            var cont = 0;
            var increas = 0;
            var charts = '';
            var drilldown = function (e, point, type) {
                if (type == "col") {
                    var charts = [$('#container-col').highcharts()];//Highcharts.charts;
                    pointName = e.point.name;
                    point.options.chart.drilled = true;
                    Highcharts.each(charts, function (c, i) {
                        if (!c.options.chart.drilled) {
                            c.options.chart.drilled = true;
                            Highcharts.each(c.series[0].data, function (p, j) {
                                if (p.name === pointName) {
                                    p.doDrilldown();
                                }
                            });
                        }
                    });
                }
            };

            var chartCol = Highcharts.chart('container-col', {
                colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce',
                    '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a', '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4',
                    '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                    '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: true },
                chart: {
                    type: 'column',
                    drilled: false,
                    events: {
                        drilldown: function (e) {
                            drilldown(e, this, "col");
                            cont = $("[id$=ddlTopDownReasons]").val();
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(cont);
                            chartCol.xAxis[0].setCategories(catValue);
                        },
                        drillup: function (e) {
                            //chartCol.series[0].show(true);
                            
                            cont = $("[id$=ddlTopDownReasons]").val();
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(0, cont);
                            chartCol.xAxis[0].setCategories(catValue);
                            chartCol.series[1].setData(data.series);
                        }
                    }
                },
                title: {
                    text: ''
                }, xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: '12px'
                        }
                    },
                    //type: 'category',
                }, tooltip: {
                    enabled: true,
                    //format: '{point.y:.1f}',
                    formatter: function () {
                        var value = 0;
                        if (this.series.name == "Pareto") {
                            value = (this.y).toFixed(2);
                            return '<b>' + this.series.name + '</b><br/>' +
                                "Cumulative Value (%)" + ': ' + value;
                        }
                        else {
                            value = (this.y);
                            return '<b>Rejection Quantity: ' + value;
                        }

                    },
                },
                yAxis: [{
                    title: {
                        text: 'Rejection Quantity',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    //labels: {
                    //    format: "{value}%"
                    //}                   
                }, {
                    title: {
                        text: 'Cum %',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    minPadding: 0,
                    maxPadding: 0,
                    max: 100,
                    min: 0,
                    opposite: true,
                    labels: {
                        format: "{value}"
                    }
                }],
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                var value = '';
                                if (this.series.name != 'Pareto') {
                                    var value = this.y;
                                }
                                return value;
                            },
                            //format: '{point.y:.1f}',                            
                        }, style: {
                            fontSize: '12px'
                        }
                    }
                },
                series: [{
                    type: 'pareto',//pareto
                    name: 'Pareto',
                    yAxis: 1,
                    zIndex: 10,
                    baseSeries: 1

                }, {
                    name: splite,
                    type: 'column',
                    colorByPoint: true,
                    zIndex: 2,
                    data: data.series,
                }],
                drilldown: {
                    series: data.drilldown,
                }
            });
        }

        $(document).on("click", "#btnExport", function () {
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            var dateCom = compareDates(from, to);
            if (dateCom == 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            $("[id$=btnHideExport]").trigger("click");
            return false;
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // $.unblockUI({});
            //$('[id$=ddlMachineId]').multiselect({
            //    includeSelectAllOption: true
            //});
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            //BindGraph();
        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
