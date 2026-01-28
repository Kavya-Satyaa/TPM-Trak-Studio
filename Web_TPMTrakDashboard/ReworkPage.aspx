<%@ Page Title="Rework Information" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="ReworkPage.aspx.cs" Inherits="Web_TPMTrakDashboard.ReworkPage" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/drilldownChartjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>
    <%--/-------------------toggal button----------------/--%>
    <link href="MyCssAndJS/toggel/bootstrap-toggle.min.css" rel="stylesheet" />
    <script src="MyCssAndJS/toggel/bootstrap-toggle.min.js"></script>
    <style>
        .highcharts-title {
            width: 650px;
            text-align: center;
        }
    </style>
    <script>
        $(document).ready(function () {
            var count=0;
            //-----------------Start Toggle Condition---------------------
            var chartType= <%= chartType %>;
            if(chartType==1){
                $('#btnToggal').bootstrapToggle("on")
            }else{
                $('#btnToggal').bootstrapToggle("off")}
            $('#btnToggal').change(function(){
                if($('#btnToggal').prop("checked")==true){
                      $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                    BindCategoryChart('ReworkCategory', 'column');
                    if($("#ddlCatagory").val()!="0"){
                        var category=$("#ddlCatagory").val();
                        BindCodeChart('ReworkCode','column',category);                       
                    }else{
                        BindCodeChart('ReworkCode','column','');
                    }
                    //BindCodeChart('ReworkCode', 'column');
                }else{
                      $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                    BindCategoryChart('ReworkCategory', 'line');
                    if($("#ddlCatagory").val()!="0"){
                        var category=$("#ddlCatagory").val();
                        BindCodeChart('ReworkCode','line',category);                       
                    }else{
                        BindCodeChart('ReworkCode','line','');
                    }
                    // BindCodeChart('ReworkCode', 'line');
                }
            });
            //-----------------End Toggle Condition----------------------

              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindCategoryChart('ReworkCategory', '');
            function BindCategoryChart(param, type) {
                $.ajax({
                    type: "POST",
                    url: "ReworkPage.aspx/GetRejectedCategoryChart",
                    contentType: "application/json; charset=utf-8",
                    data: '{param:"' + param + '",type:"' + type + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        if(count==0)
                            BindCatagory(itmData);
                        LoadCategoryChart(itmData, type, param);
                        count++;
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if(jqXHR.status==401)
                            window.location.href="SignIn.aspx"; 
                    }
                });
            }

            function BindCatagory(data) {
                var customers =data.series[0].data;
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
                                    chart.setTitle({
                                        text: data.XAxisTitle
                                    });
                                }else{
                                    chart.title.attr({
                                        text: e.seriesOptions.data[0].beforeTitle
                                    });
                                }
                            },
                            drilldown: function (e) {
                                //alert(this.series[0].data[e.point.x].name);
                                //if (xlabel == "") {
                                //    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].name;
                                //}
                                chart.setTitle({
                                    text: data.XAxisTitle + ' : ' + this.series[0].data[e.point.x].afterTitel
                                });

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
                            text: '<%=GetLocalResourceObject("Valueincount")%>',
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
                        enabled: false
                    },
                    xAxis: {
                        title: {
                            //text: data.XAxisTitle,
                            //style: {
                            //    color: '#525151',
                            //    fontSize: '15px',
                            //    fontFamily: 'Verdana, sans-serif',
                            //    fontWeight: 'bold'
                            //}
                        },
                        type: 'category'
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                    },
                    plotOptions: {
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
                                    click: function () {
                                        var index = this.category;
                                        if (this.drilldown != null) {                                                                      
                                            var str = this.drilldown;
                                            var res = str.split("#@$&*");
                                            //if (res[3] != undefined)
                                            //    window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                            //else 
                                            BindDynamicShiftChart(res[1], res[2], type, this.color, param, res[0], '');
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
            }

            BindCodeChart('ReworkCode', '', '');
            function BindCodeChart(param, type,category) {
                $.ajax({
                    type: "POST",
                    url: "ReworkPage.aspx/GetRejectedCodeChart",
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
                        if(jqXHR.status==401)
                            window.location.href="SignIn.aspx"; 
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
                                    chart.setTitle({
                                        text: data.XAxisTitle
                                    });
                                }else{
                                    chart.title.attr({
                                        text: e.seriesOptions.data[0].beforeTitle
                                    });
                                }
                            },
                            drilldown: function (e) {
                                //alert(this.series[0].data[e.point.x].name);
                                //if (xlabel == "") {
                                //    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].name;
                                //}
                                chart.setTitle({
                                    text: data.XAxisTitle + ' : ' + this.series[0].data[e.point.x].afterTitel
                                });

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
                            text: '<%=GetLocalResourceObject("Valueincount")%>',
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
                        enabled: false
                    },
                    xAxis: {
                        //title: {
                        //    text: data.XAxisTitle,
                        //    style: {
                        //        color: '#525151',
                        //        fontSize: '15px',
                        //        fontFamily: 'Verdana, sans-serif',
                        //        fontWeight: 'bold'
                        //    }
                        //},
                        type: 'category'
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                    },
                    plotOptions: {
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
                                    click: function () {
                                        var index = this.category;
                                        if (this.drilldown != null) {                                            
                                            var str = this.drilldown;
                                            var res = str.split("#@$&*");
                                            //if (res[3] != undefined)
                                            //    window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                            //else
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
                    }
                });
            }

            function BindDynamicShiftChart(month, day, type, color, param, catagory, code) {
                $.ajax({
                    type: "POST",
                    url: "ReworkPage.aspx/GetShiftChartData",
                    contentType: "application/json; charset=utf-8",
                    data: '{strMonth:"' + month + '", strDay:"' + day + '", param:"' + param + '", type:"' + type + '", catagory:"' + catagory + '", code:"' + code + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        LaodShiftWiseData(itmData, color);
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if(jqXHR.status==401)
                            window.location.href="SignIn.aspx"; 
                    }
                });
            }

            function LaodShiftWiseData(data, color) {
                Highcharts.chart('divShiftChart', {
                    colors: [color],
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
                            text: '<%=GetLocalResourceObject("Valueincount")%>',
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
                        enabled: false
                    },
                    xAxis: {
                        //title: {
                        //    text: data.XAxisTitle,
                        //    style: {
                        //        color: '#525151',
                        //        fontSize: '15px',
                        //        fontFamily: 'Verdana, sans-serif',
                        //        fontWeight: 'bold'
                        //    }
                        //},
                        type: 'category'
                    },
                    plotOptions: {
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

            $("#btnView").click(function(){
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                if($("#ddlCatagory").val()=="0"){
                    if($('#btnToggal').prop("checked")==true){
                        BindCodeChart('ReworkCode','column','');
                    }else{
                        BindCodeChart('ReworkCode','line','');
                    }
                }else{                   
                    var category=$("#ddlCatagory").val();
                    if($('#btnToggal').prop("checked")==true){
                        BindCodeChart('ReworkCode','column',category);
                    }else{
                        BindCodeChart('ReworkCode','line',category);
                    }
                }
            });

            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"><%=GetGlobalResourceObject("CommanResource","ShiftWiseChart") %></h4>
                </div>
                <div class="modal-body">
                    <div id="divShiftChart" style="width: 548px"></div>
                </div>
            </div>
        </div>
    </div>
    <%-- <div class="container">--%>
    <div class="row text-center" style="font-size: 18px; font-weight: 900; margin-bottom: 5px; color: white">
        <%=GetLocalResourceObject("ReworkInformation") %> - <%= Session["MachineId"]%>   &nbsp&nbsp;<input type="checkbox" id="btnToggal" data-toggle="toggle" data-on="  <%=GetGlobalResourceObject("CommanResource","Bar") %>" data-off=" <%=GetGlobalResourceObject("CommanResource","Line") %>">
    </div>
    <div id="divCategoryChart"></div>
    <div style="text-align: center; padding: 5px;">
        <span style="font-size: 14px; font-weight: 900; color: white"><%=GetGlobalResourceObject("CommanResource","FilterDownCodeby") %>  :</span>
        <select id="ddlCatagory" class="form-control" style="width: 200px; display: inline;" data-toggle="tooltip" title="<%=GetGlobalResourceObject("CommanResource","AllCategoryTooltip") %>"></select>
        <button id="btnView" class="btn btn-primary" type="button"><%=GetGlobalResourceObject("CommanResource","View") %></button>
    </div>
    <div id="divCodeChart"></div>
    <%--  </div>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
