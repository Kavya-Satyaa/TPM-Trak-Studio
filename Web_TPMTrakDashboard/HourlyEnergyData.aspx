<%@ Page Language="C#" Title="Hourly Energy Data" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="HourlyEnergyData.aspx.cs" Inherits="Web_TPMTrakDashboard.HourlyEnergyData" meta:resourcekey="PageResource1" Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/stock/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/no-data-to-display.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>

    <style>
        td[colspan="4"] {
            text-align: center;
        }

        .table thead > tr > th {
            vertical-align: top;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 2px solid;
            margin: 0 auto 1em auto;
            border-color: darkgray;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        .table tbody > tr > th {
            color: white;
            text-align: center;
        }

        .ajax-loader {
            display: none;
            background-color: rgba(0, 0, 0, 0.6);
            position: absolute;
            z-index: +100 !important;
            width: 100%;
            height: 100%;
            margin-left: -15px;
            margin-top: -16px;
        }

        #load-div {
            position: fixed;
            padding-right: 100px;
            width: 30%;
            top: 40%;
            left: 35%;
            text-align: center;
            border: 3px solid rgb(170, 170, 170);
            background-color: rgb(255, 255, 255);
            cursor: wait;
        }

        .ajax-loader img {
            position: relative;
            left: 50%;
        }

        .lblMargin {
            margin-left:25px;
        }
    </style>

    <div class="container">
        <div class="row text-center" style="font-size: 18px; font-weight: 900; color: white">
            <asp:Label runat="server" ID="lblTitle" Text='<%=GetLocalResourceObject("HourlyEnergyData") %>' meta:resourcekey="lblTitleResource1"></asp:Label>
            <asp:Label runat="server" ID="lblDate" CssClass="lblMargin"></asp:Label>
            <asp:Label runat="server" ID="lblShift" CssClass="lblMargin"></asp:Label>
            <br />
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False" meta:resourcekey="lblMessageResource1"></asp:Label>
        </div>
    </div>
    <div style='font-family: Calibri; font-style: normal; font-weight: 700; margin-right: 20px; margin-left: 20px;'>
        <div style="max-height: 430px; overflow-y: auto; margin-top: 5px;">
            <asp:GridView ID="gridViewHourlyEnergyData" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" ShowHeaderWhenEmpty="True" EmptyDataText="<%$ Resources:CommanResource, Nodataavailable %>" HorizontalAlign="Center" meta:resourcekey="gridViewHourlyEnergyDataResource1" HeaderStyle-BackColor="#5391CA" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Large" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <AlternatingRowStyle BackColor="#DCE7F5" />
                <Columns>
                    <asp:BoundField HeaderText="Shift Hour ID" DataField="ShiftHourID" meta:resourcekey="BoundFieldResource4" />
                    <asp:BoundField HeaderText="Power Factor(PF)" DataField="PF" meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField HeaderText="Cost" DataField="Cost" DataFormatString="{0:N0}" HtmlEncode="false" meta:resourcekey="BoundFieldResource3" />
                    <asp:BoundField HeaderText="Energy" DataField="Energy" meta:resourcekey="BoundFieldResource7" />
                    <asp:BoundField HeaderText="VLN-R(Min./Max.)" DataField="Volt1" meta:resourcekey="BoundFieldResource8" />
                    <asp:BoundField HeaderText="VLN-Y(Min./Max.)" DataField="Volt2" meta:resourcekey="BoundFieldResource9" />
                    <asp:BoundField HeaderText="VLN-B(Min./Max.)" DataField="Volt3" meta:resourcekey="BoundFieldResource10" />
                </Columns>
                <EmptyDataTemplate>
                    No data available for selected day and shift.
                </EmptyDataTemplate>
            </asp:GridView>
        </div>

        <div style='font-family: Calibri; font-style: normal; font-weight: 700; margin-top: 5px;'>
            <div id="HourlyEnergyChart" style="width: 100%;" />
        </div>
    </div>

    <script type="text/javascript">
        var chart = null;
        $(document).ready(function () {
            $.unblockUI({});
            $('[data-toggle="tooltip"]').tooltip();
            $('[id$=txtFromDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: true,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txtToDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            GetHourlyEnergyChartData();
        });

        var HourlyEnergyData = {};
        function GetHourlyEnergyChartData() {
            showLoader();
            var param = "Hour";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "HourlyEnergyData.aspx/GetHourlyEnergyChartData",
                data: '{param:"' + param + '"}',
                dataType: "json",
                success: function (result) {
                    HourlyEnergyData = result.d;
                    PlotHourlyEnergyChart(HourlyEnergyData);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function PlotHourlyEnergyChart(energyData) {
            chart = Highcharts.chart('HourlyEnergyChart', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Hourly Energy Data'
                },
                subtitle: {
                    text: 'Hourly energy data for Shift'
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Energy (in kWh)'
                    }
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: true
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true
                        }
                    }
                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> kWH<br/>'
                },

                "series": [
                    {
                        "name": "Hour",
                        "colorByPoint": true,
                        "data": energyData
                    }
                ]
            });
            hideLoader();
        }

        function showLoader() {
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }

        function hideLoader() {
            $.unblockUI({});
            $('.ajax-loader').hide();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
