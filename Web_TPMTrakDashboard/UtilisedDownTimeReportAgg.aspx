<%@ Page Title="Utilised-Downtime" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UtilisedDownTimeReportAgg.aspx.cs" Inherits="Web_TPMTrakDashboard.UtilisedDownTimeReportAgg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/paretoandDrillDownChartJs") %>
    <%: Scripts.Render("~/bundles/paretoChartJs") %>
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>
                    <td class="commanTd" style="vertical-align: middle;">Plant</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Cell</td>
                    <td id="tdCellID">
                        <asp:ListBox ID="lbCell" runat="server" SelectionMode="Multiple" Width="150" ClientIDMode="Static" OnSelectedIndexChanged="lbCell_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Machine</td>
                    <td>
                        <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple" Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Type</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem Text="Shift" Value="Shift"></asp:ListItem>
                            <asp:ListItem Text="Month" Value="Month"></asp:ListItem>
                            <asp:ListItem Text="Time Consolidated" Value="Consolidated"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td runat="server" id="tdFromDateHeader" class="commanTd" style="width: 100px; vertical-align: middle;">From Date</td>
                    <td runat="server" id="tdFromDateContent" class="input-group" style="min-width: 150px; border: 0">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td runat="server" id="tdToDateHeader" class="commanTd" style="width: 100px; vertical-align: middle;">To Date</td>
                    <td runat="server" id="tdToDateContent" class="input-group" style="min-width: 150px; border: 0">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td runat="server" id="tdShiftHeader" class="commanTd" style="width: 100px; vertical-align: middle;">Shift</td>
                    <td runat="server" id="tdShiftContent">
                        <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td runat="server" id="tdYearMonthHeader" class="commanTd" style="vertical-align: middle;">Year-Month</td>


                    <td  runat="server" id="tdYearMonthContent" >
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year"  Style="width: 70px; display: inline;"></asp:TextBox>
                        <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month"  Style="width: 70px; display: inline;"></asp:TextBox>
                    </td>

                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:AsyncPostBackTrigger ControlID="lbCell" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <div id="chartContainer" style="height: 80vh"></div>
    </div>
     <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <script>
        $(document).ready(function () {
            setControls();
            BindChart();
        });
        function BindChart() {
            $.ajax({
                type: "POST",
                url: "UtilisedDownTimeReportAgg.aspx/getChartData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var chartData = response.d;
                    Highcharts.chart('chartContainer', {
                        chart: {
                            type: 'column',
                        },
                        title: {
                            text: 'Utilised and Downtime Chart',
                            style: {
                                color: '#2d1e8a',
                                fontWeight: 'bold',
                                fontSize: 20
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        xAxis: {
                            categories: chartData.Category,
                            title: {
                                text: 'Machine ID',
                                style: {
                                    color: 'black',
                                    fontWeight: 'bold',
                                    fontSize: 14
                                }
                            },
                            labels: {
                                style: {
                                    color: '#2d1e8a',
                                    fontWeight: 'bold',
                                }
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Time (hh:mm:ss)',
                                style: {
                                    color: 'black',
                                    fontWeight: 'bold',
                                    fontSize: 14
                                }
                            },
                            stackLabels: {
                                enabled: true,
                                formatter: function () {
                                    return getFormattedTime(this.total);
                                },
                                y: -30
                            },
                            labels: {
                                style: {
                                    fontSize: 13
                                },
                                formatter: function () {
                                    return getFormattedTime(this.value);
                                }
                            }
                        },
                        tooltip: {
                            enabled: true,
                            formatter: function () {
                                debugger;
                                let value = getFormattedTime(this.y);
                                return '<b>' + this.point.category + '</b><br/>' + this.series.name + ':' + value;
                                //+ '<br/>Total: ' + this.point.stackTotal;
                            }
                        },
                        legend: {

                        },
                        plotOptions: {
                            column: {
                                stacking: 'normal',
                                dataLabels: {
                                    enabled: true,
                                    formatter: function () {
                                        return getFormattedTime(this.y);
                                    },
                                }
                            },
                            //series: {
                            //    animation: false
                            //}
                        },
                        series: chartData.series,
                    });
                },
                error: function (response) {
                    console.log(response.d);
                }
            });
        }
        function getFormattedTime(time) {
            var hours1 = parseInt(time / (60 * 60));
            var mins1 = parseInt((parseInt(time / 60)) % 60);
            var secs1 = parseInt(time % (60));
            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
        }
        function setControls() {
            $('[id$=lbMachine]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbCell]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        function stayMultiselectedList(param) {
            debugger;
            setControls();
            if (param == "cell") {
                $("#tdCellID .btn-group").addClass('open');
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                setControls();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
