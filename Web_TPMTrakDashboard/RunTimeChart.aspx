<%@ Page Title="RunTimeChart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RunTimeChart.aspx.cs" Inherits="Web_TPMTrakDashboard.RunTimeChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%-- <script src="MyCssAndJS/Highchart10.3.2/highcharts.js"></script>--%>
    <script src="MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <script src="MyCssAndJS/Highchart10.3.2/accessibility.js"></script>
    <script src="MyCssAndJS/Highchart10.3.2/exporting.js"></script>
    <script src="MyCssAndJS/Highchart10.3.2/export-data.js"></script>
    <script src="MyCssAndJS/Highchart10.3.2/xrange.js"></script>
    <style>
        .lblDiv {
            width: 130px;
            color: white;
            padding-top: 15px;
            text-align: center;
        }

        .ValsDiv {
            width: 180px;
            text-align: center;
        }

        .timebar-btn {
            color: #484343;
            font-size: 28px;
            text-decoration: none;
            /*    vertical-align: bottom;*/
        }

            .timebar-btn:hover {
                color: #484343;
                text-decoration: none;
            }

        .lbls {
            padding: 7px;
            border-radius: 50%;
            vertical-align: middle;
        }

        .runtimeChartDiv {
            height: 100%;
            box-shadow: 0px 0px 5px 2px #dddbdb;
            border-radius: 10px;
        }
    </style>
    <div class="row" style="margin-bottom: 4px; margin-top: -11px;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table id="tblHeader" class="table table-bordered" style="background-color: #394A59; width: auto; margin: 0px">
                    <tr>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Plant") %></b>
                        </td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <asp:DropDownList ID="ddlPlantId" runat="server" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" CssClass="form-control disabled-control" data-toggle="tooltip" title="Plant ID !" ToolTip="<%$Resources:CommanResource, PlantTooltip %>" AutoPostBack="True" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Cell") %></b>
                        </td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control disabled-control" data-toggle="tooltip" title="Cell ID !" ToolTip="<%$Resources:CommanResource, CellId %>" AutoPostBack="True" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Machine") %></b>
                        </td>
                        <td class="ValsDiv disabled-control-listbox" style="min-width: 120px;">
                            <asp:ListBox ID="ddlMachineId" runat="server" SelectionMode="Multiple" CssClass="ddlMachineIdStyle" ClientIDMode="Static"></asp:ListBox>
                        </td>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Shift") %></b>
                        </td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-control disabled-control" data-toggle="tooltip" title="Plant ID !" ToolTip="<%$Resources:CommanResource,ShiftTooltip %>" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>

                        <td class="lblDiv" style="width: 60px;"><b>Date</b></td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <div class="input-group ">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date disabled-control" data-toggle="tooltip"
                                    title="From Date !" placeholder="From Date" ToolTip="<%$Resources:CommanResource, FromDate %>" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </td>
                        <td style="text-align: left" class="lblDiv">
                            <asp:Button runat="server" Text="<%$ Resources: CommanResource, View %>" class="btn btn-info btn-sm disabled-control" ID="btnProcess" OnClientClick="return BindRunTimeChart(false);"></asp:Button>
                        </td>
                        <td class="lblDiv">
                            <asp:CheckBox runat="server" ID="chkAutoRefresh" ClientIDMode="Static" CssClass=" checkbox-list " Text="Auto Refresh" onchange="autoRefreshClick();" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="background-color: white" id="paginationDiv">
            <table>
                <tr>
                    <td>
                        <label style="background-color: red" class="lbls"></label>
                        Down Reason
                    </td>
                    <td>
                        <label style="background-color: green; color: white" class="lbls"></label>
                        Running
                    </td>
                    <td>
                        <label style="background-color: maroon; color: white" class="lbls"></label>
                        ICD
                    </td>
                    <td>
                        <label style="background-color: yellow; color: white" class="lbls"></label>
                        Management Loss
                    </td>
                    <td>
                        <label style="background-color: blue; color: white" class="lbls"></label>
                        PDT
                    </td>
                    <td style="width: 100px;"></td>
                    <td id="tdPagination">
                        <a class="glyphicon glyphicon-backward timebar-btn" id="btnPrevious" onclick="setPaginationButton('previous')" title="Previous"></a>&nbsp;&nbsp;&nbsp;
                        <a class="glyphicon glyphicon-forward timebar-btn" id="btnNext" onclick="setPaginationButton('next')" title="Next"></a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="chartContainer" style="height: 79vh; margin-top: 10px">
        </div>
    </div>
    <script>
        var AutoRefreshInterval;
        var RunTimeCharts = [];
        var PageNumber = 0, TotalPageNumber = 0;
        $(document).ready(function () {
            setDateTimeControls();
            BindRunTimeChart(false);
        });
        function BindRunTimeChart(appendData) {
            debugger;
            clearInterval(AutoRefreshInterval);
            if ($('#ddlPlantId').val() == "" || $('#ddlPlantId').val() == null) {
                openWarningModal_1("Please select Plant.");
                return false;
            }
            if ($('#ddlMachineId').val() == "" || $('#ddlMachineId').val() == null) {
                openWarningModal_1("Please select Machine ID.");
                return false;
            }
            if ($('#ddlShift').val() == "" || $('#ddlShift').val() == null) {
                openWarningModal_1("Please select Shift.");
                return false;
            }

            if ($('#txtFromDate').val() == "") {
                openWarningModal_1("Please select Date.");
                return false;
            }
            debugger;
            var chartDivHeight = $(window).height() - $('#tblHeader').height() - $('#paginationDiv').height();
            let oneMachineHeight = 100;
            chartDivHeight = Math.ceil(chartDivHeight / oneMachineHeight);
            if (appendData == false) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            }
            setTimeout(function () {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "RunTimeChart.aspx/getRunTimeChartData",
                    contentType: "application/json; charset=utf-8",
                    data: '{plant:"' + $('#ddlPlantId').val() + '",machine:"' + $('#ddlMachineId').val() + '",shift:"' + $('#ddlShift').val() + '",date:"' + $('#txtFromDate').val() + '",appendData:' + appendData + ',totalPageNo:"' + chartDivHeight + '"}',
                    datatype: "json",
                    success: function (response) {
                        var itemdata = response.d;
                        debugger;
                        if (appendData == true) {
                            AppendChartData(itemdata);
                            AutoRefreshInterval = setInterval(function () {
                                BindRunTimeChart(true);
                            }, 30000);
                        }
                        else {
                            PageNumber = 0; TotalPageNumber = 0;
                            RunTimeCharts = [];
                            $('#chartContainer').empty();
                            var chartStr = "";
                            for (var i = 0; i < itemdata.length; i++) {
                                chartStr += ' <div id="chartContainerDiv' + i + '" class="runtimeChartDiv"></div>';
                            }
                            $('#chartContainer').append(chartStr);

                            if (itemdata.length > 1) {
                                PageNumber = 1;
                                TotalPageNumber = itemdata.length;
                                $('#tdPagination').css("display", "block");
                                $('#btnNext').css("visibility", "visible");
                                $('#btnPrevious').css("visibility", "hidden");
                                $(".runtimeChartDiv").css("display", "none");
                                $($(".runtimeChartDiv")[0]).css("display", "block");
                            }
                            else {

                                $('#btnNext').css("visibility", "hidden");
                                $('#btnPrevious').css("visibility", "hidden");
                                $('#tdPagination').css("display", "none");
                            }

                            for (var i = 0; i < itemdata.length; i++) {
                                BindChart(itemdata[i], "chartContainerDiv" + i, i);
                            }
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                        console.log(err);
                    }
                });
                $.unblockUI({});
            }, 500);
            return false;
        }
        function setPaginationButton(param) {
            $(".runtimeChartDiv").css("display", "none");
            if (param == "next") {
                PageNumber++;
                $('#btnPrevious').css("visibility", "visible");
                if (PageNumber == TotalPageNumber) {
                    $('#btnNext').css("visibility", "hidden");
                }
                else {
                    $('#btnNext').css("visibility", "visible");
                }
                $($(".runtimeChartDiv")[PageNumber - 1]).css("display", "block");
            }
            else {
                PageNumber--;
                $('#btnNext').css("visibility", "visible");
                if (PageNumber == 1) {
                    $('#btnPrevious').css("visibility", "hidden");
                }
                else {
                    $('#btnPrevious').css("visibility", "visible");
                }
                $($(".runtimeChartDiv")[PageNumber - 1]).css("display", "block");
            }
        }
        function BindChart(chartdata, chartid, chartindex) {
            var categoryCount = chartdata.Category.length;
            var dataCount = chartdata.data.length;
            debugger;
            var chartHeight = categoryCount * 300;
            var chartWidth = dataCount * 40;
            if (chartdata.data.length == 0) {
                console.log("Enter window width");
                chartWidth = $(window).width() - 200;
            }
            else if (chartWidth <= $(window).width()) {
                chartWidth = $(window).width();
            }
            console.log("Chart " + chartindex + " width" + chartWidth);
            RunTimeCharts[chartindex] = Highcharts.chart(chartid, {
                chart: {
                    type: 'xrange',
                    /* height: chartHeight,
                        width: chartWidth*/
                },
                title: {
                    text: ''
                },
                xAxis: {
                    type: 'datetime',
                    //tickInterval: 1
                    title: {
                        text: 'Time'
                    },
                    className: 'highchart-xyaxis-label',
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: '12px',
                            fontWeight: 'bold',
                            color: '#2d2652',
                        },
                    },
                    gridLineColor: 'gray',
                    gridLineWidth: 0.8,
                    //tickColor: 'black',
                    //tickInterval: 12,
                    //tickWidth:2,
                },
                yAxis: {

                    title: {
                        text: 'Machine'
                    },
                    categories: chartdata.Category,
                    className: 'highchart-xyaxis-label',
                    //plotLines: chartdata.plotLines,
                    gridLineColor: 'transparent',
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontWeight: 'bold',
                            color: '#2d2652',
                        },
                    }
                    //plotLines: [{
                    //    color: '#FF0000',
                    //    width: 1,
                    //    value: 5,
                    //    label: {
                    //        text: '4wW RTT  UUU U UU 7',
                    //        x: 30,
                    //        y: -50,
                    //        style: {
                    //            fontSize: 30,
                    //            color: '#1269c7'
                    //        }
                    //    }
                    //}]
                },

                tooltip: {
                    //borderWidth: 0,
                    // backgroundColor: "rgba(255,255,255,0)",
                    // borderRadius: 0,
                    // shadow: false,
                    useHTML: true,
                    followPointer: true,
                    //  percentageDecimals: 2,
                    backgroundColor: "rgba(255,255,255,1)",
                    formatter: function () {
                        let tmpTooltip = '<span style="color:' + this.point.color + '">\u25CF</span> ' + ': <b>' + this.series.chart.yAxis[0].categories[this.y] + '</b><br/> Start Time :  <b>' +
                            Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '</b><br/> End Time :  <b>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x2) + '</b><br/> Status :  <b>' +
                            this.point.status + '</b>';
                        if (this.point.status != "Running") {
                            tmpTooltip += '<br/> Reason :  <b>' + this.point.DownReason + '</b>';
                        }
                        return tmpTooltip;

                    },
                    // backgroundColor: '#FCFFC5',
                    style: {
                        zIndex: 99998
                    }
                },

                navigator: {
                    enabled: true,
                    height: 20,
                    xAxis: {
                        type: 'datetime',
                        //width: $('#chartContainer').width() - 130
                    }
                },

                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                credits: {
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
                    }, {
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
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                plotOptions: {
                    series: {
                        turboThreshold: 1000000,
                        // pointInterval: 24 * 3600 * 1000 // one day
                        pointInterval: 1000, // one day
                        //groupPadding: 0,
                        //pointPadding: 0,
                        borderWidth: 0,
                        borderRadius: 0,
                        gapSize: 0
                    }
                },
                series: [{
                    name: '',
                    // pointPadding: 0,
                    // groupPadding: 0,
                    //  borderColor: 'gray',
                    //  borderWidth: 0,
                    // borderRadius: 0,
                    pointWidth: 50,
                    //data: data.series.data,
                    //turboThreshold: data.series.data.length,
                    data: chartdata.data,
                    //// dataLabels: {
                    ////    enabled: true
                    ////} 
                }]

            });
        }
        function AppendChartData(itemdata) {
            if (itemdata != null) {
                for (var i = 0; i < itemdata.length; i++) {
                    var chartdata = itemdata[i].data;
                    debugger;
                    for (var datacount = 0; datacount < chartdata.length; datacount++) {
                        RunTimeCharts[i].series[0].addPoint(chartdata[datacount]);
                    }
                }
            }
        }
        function autoRefreshClick() {
            clearInterval(AutoRefreshInterval);
            if ($('#chkAutoRefresh').prop("checked")) {
                $('.disabled-control').attr('disabled', 'disabled');
                $('.disabled-control-listbox button').attr('disabled', 'disabled');

                var currentDate = new Date();
                $('#txtFromDate').val(get2DigitValue(currentDate.getDate()) + "-" + get2DigitValue(currentDate.getMonth() + 1) + "-" + currentDate.getFullYear());
                BindRunTimeChart(false);
                AutoRefreshInterval = setInterval(function () {
                    BindRunTimeChart(true);
                }, 30000);

            }
            else {
                $('.disabled-control').removeAttr('disabled');
                $('.disabled-control-listbox button').removeAttr('disabled');
                BindRunTimeChart(false);
            }
        }
        function get2DigitValue(value) {
            if (value.toString().length == 1) {
                value = "0" + value;
            }
            return value;
        }

        function setDateTimeControls() {
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setDateTimeControls();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
