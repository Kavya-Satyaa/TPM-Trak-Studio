<%@ Page Title="Downtime Information" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="DownTimePieChart.aspx.cs" Inherits="Web_TPMTrakDashboard.DownTimePieChart" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <link href="css/bootstrap-theme.css" rel="stylesheet" />--%>

    <%: Scripts.Render("~/bundles/drilldownChartjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>
    <%--/-------------------toggal button----------------/--%>
    <link href="MyCssAndJS/toggel/bootstrap-toggle.min.css" rel="stylesheet" />
    <script src="MyCssAndJS/toggel/bootstrap-toggle.min.js"></script>
    <style>
        .btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .open .dropdown-toggle.btn-primary {
            color: #000000;
            background-color: #FAEBD7;
            border-color: #FAEBD7;
        }

        .highcharts-title {
            width: 650px;
            text-align: center;
        }
    </style>

    <script>
        $(document).ready(function () {
            var count = 0;
            $("#BtnActionTaken").css("display", "none");
            //-----------------Start Toggle Condition---------------------
            var chartType = <%= chartType %>;
            if (chartType == 1) {
                $('#btnToggal').bootstrapToggle("on")
            } else {
                $('#btnToggal').bootstrapToggle("off")
            }
            $('#btnToggal').change(function () {
                if ($('#btnToggal').prop("checked") == true) {
                    $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                    BindDownCategoryChart('DownCategory', 'pie');
                    if ($("#ddlCatagory").val() != "0") {
                        var category = $("#ddlCatagory").val();
                        BindDownCodeChart('DownCode', 'pie', category);
                    } else {
                        BindDownCodeChart('DownCode', 'pie', '');
                    }
                    // BindDownCodeChart('DownCode', 'column');
                } else {
                    $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                    BindDownCategoryChart('DownCategory', 'line');
                    if ($("#ddlCatagory").val() != "0") {
                        var category = $("#ddlCatagory").val();
                        BindDownCodeChart('DownCode', 'line', category);
                    } else {
                        BindDownCodeChart('DownCode', 'line', '');
                    }
                    // BindDownCodeChart('DownCode', 'line');
                }
            });
            //-----------------End Toggle Condition----------------------

            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDownCategoryChart('DownCategory', 'pie');
            function BindDownCategoryChart(param, type) {
                $.ajax({
                    type: "POST",
                    url: "DownTimePieChart.aspx/GetDownCategoryChart",
                    contentType: "application/json; charset=utf-8",
                    data: '{param:"' + param + '",type:"' + type + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        if (count == 0)
                            BindCatagory(itmData);
                        LoadCategoryChart(itmData, type, param);
                        count++;
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }
                });

                //document.getElementById("BtnActionTaken").style.visibility = "hidden";
            }

            function BindCatagory(data) {
                var customers = data.series[0].data;
                $("#ddlCatagory option").remove();
                $("#ddlCatagory").append('<option value="0"> <%=GetGlobalResourceObject("CommanResource","AllCategory")%> </option>');
                $(customers).each(function (customers, el) {
                    $("#ddlCatagory").append('<option value="'
                        + el.name + '">' + el.name + '</option>');
                });
            }

            function LoadCategoryChart(data, type, param) {
                var xlabel = "";
                var chart = Highcharts.chart('divCategoryChart', {
                    chart: {
                        events: {
                            drillup: function (e) {
                                if (e.seriesOptions.id == undefined) {
                                    xlabel = "";
                                    chart.setTitle({ text: data.XAxisTitle });
                                    //chart.xAxis[0].axisTitle.attr({
                                    //    text: data.XAxisTitle
                                    //});
                                } else {
                                    chart.title.attr({
                                        text: e.seriesOptions.data[0].beforeTitle
                                    });
                                }
                            },
                            drilldown: function (e) {
                                //if (xlabel == "") {
                                //    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].name;
                                //}
                                chart.setTitle({ text: data.XAxisTitle + ' : ' + this.series[0].data[e.point.x].afterTitel });
                                //chart.xAxis[0].axisTitle.attr({
                                //    text: xlabel
                                //});

                            },
                        }
                    },
                    legend: {
                        enabled: data.legend
                    },
                    title: {
                        text: data.XAxisTitle,
                        useHTML: true,
                        style: {
                            color: '#FFFFFF',
                            fontSize: '15px',
                            fontFamily: 'Verdana, sans-serif',
                            'background-color': '#000000',
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {
                        text: ''
                    },
                    yAxis: {
                        //alternateGridColor: '#FDFFD5',
                        title: {
                            text: data.YAxisTitle,
                            style: {
                                color: '#525151',
                                fontSize: '12px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        title: {
                            text: ''
                        },
                        type: 'category'
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> <br/>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: false,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true
                            },
                            showInLegend: true
                        },
                        series: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                //format: '{point.y:.1f}',
                                formatter: function () {
                                    return Highcharts.numberFormat(this.y, 0);
                                },
                                showInLegend: false
                            },
                            point: {
                                events: {
                                    click: function () {
                                        var index = this.category;
                                        if (this.drilldown != null) {

                                            var str = this.drilldown;
                                            var res = str.split("#@$&*");
                                            debugger
                                            BindDynamicShiftChart(res[1], res[2], type, this.color, param, res[0], '');

                                            //if (res[3] != undefined)
                                            //    window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                            //else 

                                        }
                                    }
                                }
                            }
                        },

                    },
                    series: data.series,
                    drilldown: {
                        series: data.drilldown,
                        drillUpButton: {
                            theme: {
                                fill: 'skyblue',
                                'stroke-width': 1,
                                stroke: 'white',
                                color: 'white',
                                r: 5,
                            }
                        },
                    }
                });
            }

            BindDownCodeChart('DownCode', 'pie', '');
            function BindDownCodeChart(param, type, category) {
                $.ajax({
                    type: "POST",
                    url: "DownTimePieChart.aspx/GetDownCodeChart",
                    contentType: "application/json; charset=utf-8",
                    data: '{param:"' + param + '",type:"' + type + '",category:"' + category + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        LoadCodeChart(itmData, type, param);
                        $.unblockUI({});
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }
                });
            }

            function LoadCodeChart(data, type, param) {
                var xlabel = "";
                var chart = Highcharts.chart('divCodeChart', {
                    chart: {
                        events: {
                            drillup: function (e) {
                                if (e.seriesOptions.id == undefined) {
                                    xlabel = "";
                                    //chart.xAxis[0].axisTitle.attr({
                                    //    text: data.XAxisTitle
                                    //});
                                    chart.setTitle({ text: data.XAxisTitle });
                                    CheckForActionTakenVisibility_DrillUp(e.seriesOptions.data[0].beforeTitle, e.seriesOptions.id);
                                } else {
                                    chart.title.attr({
                                        text: e.seriesOptions.data[0].beforeTitle
                                    });
                                    CheckForActionTakenVisibility_DrillUp(e.seriesOptions.data[0].beforeTitle, e.seriesOptions.id);
                                }
                            },
                            drilldown: function (e) {
                                if (xlabel == "") {
                                    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].name;
                                    SaveDownID(this.series[0].data[e.point.x].name);
                                }
                                //chart.xAxis[0].axisTitle.attr({
                                //    text: xlabel
                                //}); 
                                chart.setTitle({ text: data.XAxisTitle + ' : ' + this.series[0].data[e.point.x].afterTitel });
                                CheckForActionTakenVisibility(this.series[0].data[e.point.x].name);
                            },
                        }
                    },
                    title: {
                        text: data.XAxisTitle,
                        useHTML: true,
                        style: {
                            color: '#FFFFFF',
                            fontSize: '15px',
                            fontFamily: 'Verdana, sans-serif',
                            'background-color': '#000000',
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {
                        text: ''
                    },
                    yAxis: {
                        //alternateGridColor: '#FDFFD5',
                        title: {
                            text: data.YAxisTitle,
                            style: {
                                color: '#525151',
                                fontSize: '12px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    legend: {
                        enabled: data.legend
                    },
                    xAxis: {
                        title: {
                            text: '',
                        },
                        type: 'category'
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: false,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true
                            },
                            showInLegend: true
                        },
                        series: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                //format: '{point.y:.1f}',
                                formatter: function () {
                                    return Highcharts.numberFormat(this.y, 0);
                                }
                            },
                            point: {
                                events: {
                                    click: function (e) {
                                        var index = this.category;
                                        if (this.drilldown != null) {
                                            var str = this.drilldown;
                                            var res = str.split("#@$&*");
                                            //if (res[3] != undefined)
                                            //    window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                            //else BindDynamicShiftChart(res[0], res[1], res[2], "column", this.color)
                                            BindDynamicShiftChart(res[1], res[2], type, this.color, param, '', res[0]);
                                        }
                                    }
                                }
                            }
                        }
                    },
                    series: data.series,
                    drilldown: {
                        series: data.drilldown,
                        drillUpButton: {
                            theme: {
                                fill: 'skyblue',
                                'stroke-width': 1,
                                stroke: 'white',
                                color: 'white',
                                r: 5,
                            }
                        },
                    }
                });
            }

            function CheckForActionTakenVisibility(ChartTitle) {
                if (ChartTitle != undefined) {
                    if (["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"].includes(ChartTitle) || !(ChartTitle.includes("Day"))) {
                        $("#BtnActionTaken").css("display", "block");
                    }
                    else {
                        $("#BtnActionTaken").css("display", "none");
                    }

                    if (["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"].includes(ChartTitle)) {
                        $.ajax({
                            type: "POST",
                            url: "DownTimePieChart.aspx/SaveMonth",
                            contentType: "application/json; charset=utf-8",
                            data: '{Month:"' + ChartTitle + '"}',
                            dataType: "json",
                            success: function (response) {
                                $.unblockUI({});
                            },
                            error: function (jqXHR, textStatus, err) {
                                alert('Error: ' + err);
                                if (jqXHR.status == 401)
                                    window.location.href = "SignIn.aspx";
                            }
                        });
                    }
                }
            }


            function CheckForActionTakenVisibility_DrillUp(ChartTitle, ChartID) {
                if (ChartTitle != undefined) {
                    /*if (["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"].includes(ChartTitle) || ChartTitle.includes("Day"))*/
                    if (ChartTitle != ChartID && ChartID != undefined) {
                        $("#BtnActionTaken").css("display", "block");
                    }
                    else {
                        $("#BtnActionTaken").css("display", "none");
                    }
                }
            }

            function SaveDownID(index) {
                $.ajax({
                    type: "POST",
                    url: "DownTimePieChart.aspx/SaveDownIDLabel",
                    contentType: "application/json; charset=utf-8",
                    data: '{DownID:"' + index + '"}',
                    dataType: "json",
                    success: function (response) {
                        $.unblockUI({});
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }
                });
            }


            function BindDynamicShiftChart(month, day, type, color, param, catagory, code) {
                $.ajax({
                    type: "POST",
                    url: "DownTimePieChart.aspx/GetShiftChartData",
                    contentType: "application/json; charset=utf-8",
                    data: '{strMonth:"' + month + '", strDay:"' + day + '", param:"' + param + '", type:"' + type + '", catagory:"' + catagory + '", code:"' + code + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        LaodShiftWiseData(itmData, color);
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }
                });
            }

            function LaodShiftWiseData(data, color) {
                Highcharts.chart('divShiftChart', {
                    //colors: [color],
                    title: {
                        text: data.XAxisTitle,
                        useHTML: true,
                        style: {
                            color: '#FFFFFF',
                            fontSize: '15px',
                            fontFamily: 'Verdana, sans-serif',
                            'background-color': '#000000',
                            fontWeight: 'bold',
                        }
                    },
                    xAxis: {
                        type: ''
                    },
                    yAxis: {
                        //alternateGridColor: '#FDFFD5',
                        title: {
                            text: data.YAxisTitle,//data.XAxisTitle
                            style: {
                                color: '#525151',
                                fontSize: '12px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        title: {
                            text: '',//data.XAxisTitle,
                            style: {
                                color: '#525151',
                                fontSize: '15px',
                                fontFamily: 'Verdana, sans-serif',
                                fontWeight: 'bold'
                            }
                        },
                        type: 'category'
                    },
                    plotOptions: {
                        pie: {
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true
                            },
                            showInLegend: true
                        },
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    return Highcharts.numberFormat(this.y, 0);
                                }
                            },
                            point: {
                                events: {
                                    click: function () {
                                        var index = this.category;
                                        if (this.drilldown != null) {
                                            var str = this.drilldown;
                                            var res = str.split("/");
                                            // window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                        }
                                    }
                                }
                            }
                        }
                    },
                    series: data.series
                });
                $("#myModal").modal('show');
            }

            $("#btnView").click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                if ($("#ddlCatagory").val() == "0") {
                    if ($("#hdfChart").val() == 'pie') {
                        BindDownCodeChart('DownCode', 'pie', '');
                    } else if ($("#hdfChart").val() == 'bar') {
                        BindDownCodeChart('DownCode', 'column', '');
                    } else {
                        BindDownCodeChart('DownCode', 'line', '');
                    }
                } else {
                    var category = $("#ddlCatagory").val();
                    if ($("#hdfChart").val() == 'pie') {
                        BindDownCodeChart('DownCode', 'pie', category);
                    } else if ($("#hdfChart").val() == 'line') {
                        BindDownCodeChart('DownCode', 'line', category);
                    } else {
                        BindDownCodeChart('DownCode', 'column', category);
                    }
                }
            });

            $('input[type=radio][name=options]').change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                if (this.value == 'Pie') {
                    BindDownCategoryChart('DownCategory', 'pie');
                    if ($("#ddlCatagory").val() != "0") {
                        var category = $("#ddlCatagory").val();
                        BindDownCodeChart('DownCode', 'pie', category);
                    } else {
                        BindDownCodeChart('DownCode', 'pie', '');
                    }
                    $("#hdfChart").val('pie');
                }
                else if (this.value == 'Bar') {
                    BindDownCategoryChart('DownCategory', 'column');
                    if ($("#ddlCatagory").val() != "0") {
                        var category = $("#ddlCatagory").val();
                        BindDownCodeChart('DownCode', 'column', category);
                    } else {
                        BindDownCodeChart('DownCode', 'column', '');
                    }
                    $("#hdfChart").val('bar');
                }
                else if (this.value == 'Line') {
                    BindDownCategoryChart('DownCategory', 'line');
                    if ($("#ddlCatagory").val() != "0") {
                        var category = $("#ddlCatagory").val();
                        BindDownCodeChart('DownCode', 'line', category);
                    } else {
                        BindDownCodeChart('DownCode', 'line', '');
                    }
                    $("#hdfChart").val('line');
                }
            });
           
            //var winHeight=$(window).height();  
            //console.log(winHeight);
            //winHeight=winHeight-138;
            //winHeight=winHeight/2;
            //$("#divCodeChart").css({"min-width": ""+winHeight+"px"});
            //$("#divCategoryChart").css({"min-width": ""+winHeight+"px"});
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="CloseModal();">&times;</button>
                    <h4 class="modal-title"><%=GetGlobalResourceObject("CommanResource","ShiftWiseChart") %></h4>
                </div>
                <div class="modal-body">
                    <div id="divShiftChart" style="width: 548px"></div>
                    <input type="hidden" id="hdfChart" value="pie">
                </div>
            </div>
        </div>
    </div>

    <div class="row text-center" style="font-size: 18px; font-weight: 900; margin-bottom: 5px; color: white">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    <%=GetLocalResourceObject("DownTimeAnalysis") %> - <%= Session["MachineId"]%>
        <span style="font-size: 10px">(Excluding TPM-Trak Standard Losses)</span>
        <div class="btn-group btn-toggle" data-toggle="buttons" style="float: right; padding-right: 15px;">
            <label class="btn btn-primary active">
                <input type="radio" name="options" id="option1" value="Pie" checked>
                <%=GetLocalResourceObject("Pie") %>
            </label>
            <label class="btn btn-primary">
                <input type="radio" name="options" value="Bar" id="option2">
                <%=GetGlobalResourceObject("CommanResource","Bar") %>
            </label>
            <label class="btn btn-primary">
                <input type="radio" name="options" value="Line" id="option3">
                <%=GetGlobalResourceObject("CommanResource","Line") %>
            </label>
        </div>

        
        <asp:Button runat="server" ID="BtnActionTaken" ClientIDMode="Static" Text="Action Taken" OnClientClick="return OpenActionTakenPage();" CssClass="btn btn-primary" Style="float: right; margin-right: 20px;" />
    </div>

    <div class="row text-center">
        <div id="divCategoryChart"></div>
    </div>

    <div style="text-align: center; padding: 5px;">
        <span style="font-size: 14px; font-weight: 900; color: white"><%=GetGlobalResourceObject("CommanResource","FilterDownCodeby") %> :</span>
        <select id="ddlCatagory" class="form-control" style="width: 200px; display: inline;" data-toggle="tooltip" title="<%=GetGlobalResourceObject("CommanResource","AllCategoryTooltip") %>"></select>
        <button id="btnView" class="btn btn-primary" type="button"><%=GetGlobalResourceObject("CommanResource","View") %></button>
    </div>

    <div class="row text-center">
        <div id="divCodeChart"></div>
    </div>

    <script>
        function OpenActionTakenPage() {
            window.open('DownTimeActionTakenDashboard.aspx', '_blank');
            return false;
        }

        function CloseModal() {
            $('#myModal').modal("hide");
            $("#BtnActionTaken").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
