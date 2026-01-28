<%@ Page Title="DGDashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DGDashboardAllied.aspx.cs" Inherits="Web_TPMTrakDashboard.Allied.DGDashboardAllied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/accessibility.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/exporting.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/export-data.js"></script>
    <script src="../MyCssAndJS/Highchart10.3.2/xrange.js"></script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td>Machine ID</td>
                            <td>
                                <asp:DropDownList ID="ddlMachineID" runat="server" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>From Date</td>
                            <td>
                                <asp:TextBox ID="txtFromdate" runat="server" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>To Date</td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="bajaj-btn-style" Text="VIEW" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="height: 40vh; overflow: auto; width: 100%">
                <div id="scrollMaintainDiv">
                    <asp:ListView ID="lvDashboardDetails" runat="server" ClientIDMode="Static">
                        <EmptyDataTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style">
                                <tr>
                                    <th>Machine</th>
                                    <th>DG StartTime</th>
                                    <th>DG EndTime</th>
                                    <th>Actual Time (hh:mm:ss)</th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="tblDashboardDetails" class="table table-bordered table-hover headerFixer bajaj-table-style">
                                <tr>
                                    <th>Machine</th>
                                    <th>DG StartTime</th>
                                    <th>DG EndTime</th>
                                    <th>Actual Time (hh:mm:ss)</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblEventStartTime" Text='<%# Eval("EventStarttime")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblEventEndTime" Text='<%# Eval("EventEndtime") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("ActualTime") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div id="chartContainer" style="height: 40vh; margin-top: 20px; border-radius: 10px; overflow: auto">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControl();
            DGDashboardData();
        });

        function setControl() {
            $("#txtFromdate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#txtToDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        function DGDashboardData() {
            debugger;

            $.ajax({
                type: "POST",
                url: "DGDashboardAllied.aspx/GetDashboardData",
                contentType: "application/json; charset=utf-8",
                data: '{machineid:"' + $("[id$=ddlMachineID]").val() + '",fromdate:"' + $("[id$=txtFromdate]").val() + '", todate:"' + $("[id$=txtToDate]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    debugger;
                    var itemdata = response.d;
                    for (var i = 0; i < itemdata.length; i++) {
                        drawDashboardChart(itemdata[i]);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function drawDashboardChart(chartdata) {
            debugger;
            var dataCount = chartdata.data.length;
            $("#chartContainer").highcharts({
                chart: {
                    type: 'xrange'
                },
                title: {
                    text: 'Run Chart'
                },
                xAxis: {
                    type: 'datetime',
                    title: {
                        text: 'Time',
                        style: {
                            fontWeight: 'bold',
                            fontSize: '20px'
                        },
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
                    //gridLineColor: 'gray',
                    //gridLineWidth: 0.1,
                },
                yAxis: {
                    title: {
                        text: chartdata.Category,
                        //text: ["kk"],
                        style: {
                            fontWeight: 'bold',
                            fontSize: '15px'
                        },
                    },
                    //categories: chartdata.Category,
                    //className: 'highchart-xyaxis-label',
                    gridLineColor: 'black',
                    labels: {
                        enabled: false,
                        style: {
                            fontSize: '12px',
                            fontWeight: 'bold',
                            color: '#2d2652',
                        },
                    }
                },
                tooltip: {
                    useHTML: true,
                    backgroundColor: "rgba(255,255,255,1)",
                    formatter: function () {
                        debugger;
                        /* let tmpTooltip = '<span style="color:' + this.point.color + '">\u25CF</span> ' + ': <b>' + this.series.chart.yAxis[0].categories[this.y] + '</b><br/> Start Time :  <b>' +*/
                        let tmpTooltip = '<span style="color:' + this.point.color + '">\u25CF</span> <br/> Start Time :  <b>' +
                            Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '</b><br/> End Time :  <b>' + Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x2);
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
                //rangeSelector: {
                //    verticalAlign: 'top',
                //    x: 0,
                //    y: 0,
                //    buttons: [{
                //        type: 'minute',
                //        count: 5,
                //        text: '5M'
                //    }, {
                //        type: 'minute',
                //        count: 10,
                //        text: '10M'
                //    }, {
                //        type: 'minute',
                //        count: 20,
                //        text: '20M'
                //    }, {
                //        type: 'minute',
                //        count: 30,
                //        text: '30M'
                //    }, {
                //        type: 'minute',
                //        count: 45,
                //        text: '45M'
                //    }, {
                //        type: 'hour',
                //        count: 5,
                //        text: '5h'
                //    }, {
                //        type: 'hour',
                //        count: 8,
                //        text: '8h'
                //    }, {
                //        type: 'hour',
                //        count: 10,
                //        text: '10h'
                //    }, {
                //        type: 'hour',
                //        count: 20,
                //        text: '20h'
                //    }, {
                //        type: 'all',
                //        text: 'All'
                //    }],
                //    selected: 10,
                //    enabled: true,
                //    allButtonsEnabled: true,

                //    inputEnabled: false
                //},
                plotOptions: {
                    series: {
                        turboThreshold: 1000000,
                        pointInterval: 1000, // one day
                        borderWidth: 0,
                        borderRadius: 0,
                        gapSize: 0
                    }
                },
                series: [{
                    name: '',
                    pointWidth: 50,
                    borderColor: 'gray',
                    data: chartdata.data
                }]
            })
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControl();
                DGDashboardData();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
