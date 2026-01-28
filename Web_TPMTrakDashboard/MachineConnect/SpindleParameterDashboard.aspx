<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SpindleParameterDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineConnect.SpindleParameterDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <style>
        .commontd {
            vertical-align: middle !important;
        }

        .chart-style {
            margin: 8px 0px;
            border-radius: 10px;
            height: 28vh;
            min-height: 100px;
        }
    </style>
    <table id="tblfilter" class="table table-bordered" style="display: inline-block; margin-bottom: 0px;">
        <tr>
            <td class="commontd"><b>From</b> </td>
            <td>
                <div class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtFromDateTime" runat="server" ClientIDMode="Static" CssClass="form-control date live-control" placeholder="From Date Time" MaxLength="15" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" AutoCompleteType="Disabled"></asp:TextBox>
                </div>
            </td>
            <td class="commontd"><b>To</b></td>
            <td>
                <div class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtToDateTime" runat="server" ClientIDMode="Static" CssClass="form-control date live-control" placeholder="From Date Time" MaxLength="15" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" AutoCompleteType="Disabled"></asp:TextBox>
                </div>
            </td>
            <td class="commontd"><b>Plant</b></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" ClientIDMode="Static" onchange="BindMachine();"></asp:DropDownList>
            </td>
            <td class="commontd"><b>Machine</b></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control" ClientIDMode="Static" onchange="BindAxisNo();"></asp:DropDownList>
            </td>
            <td class="commontd"><b>Axis No.</b></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlAxisNo" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
            </td>
            <td>
                <button type="button" class="btn btn-info btn-sm live-control" onclick="BindData();" id="btnView">View</button>

            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkLive" Text="-Auto Refresh" onclick="liveClick();" ClientIDMode="Static" ForeColor="White" Font-Size="11" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <div>
        <div id="spindleLoadChart" class="chart-style"></div>
        <div id="spindleSpeedChart" class="chart-style"></div>
        <div id="temperatureChart" class="chart-style"></div>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
            BindMachine();
            BindData();
        });
        function liveClick() {
            if ($('#chkLive').is(':checked')) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "SpindleParameterDashboard.aspx/getShiftDate",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var shiftDate = response.d;
                        $('#txtFromDateTime').val(shiftDate[0]);
                        $('#txtToDateTime').val(shiftDate[1]);
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                    }
                });
                $('.live-control').attr('disabled', 'disabled');
            }
            else {
                $('.live-control').removeAttr('disabled');
            }

            BindData();

        }
        function BindData() {
            $.ajax({
                async: false,
                type: "POST",
                url: "SpindleParameterDashboard.aspx/getChartData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{plant:"' + $('#ddlPlant').val() + '",machine:"' + $('#ddlMachine').val() + '",axisNo:"' + $('#ddlAxisNo').val() + '",fromDate:"' + $('#txtFromDateTime').val() + '",toDate:"' + $('#txtToDateTime').val() + '"}',
                success: function (response) {
                    var chartData = response.d;
                    var axisValue = "";
                    if ($('#ddlAxisNo').val().toLowerCase() != "spindle") {
                        axisValue = "-" + $('#ddlAxisNo').val();
                    }
                    BindChart("spindleLoadChart", "Load" + axisValue, chartData.SpindleLoad, '#103cde');
                    BindChart("spindleSpeedChart", "Speed", chartData.SpindleSpeed, '#a14910');
                    BindChart("temperatureChart", "Temp" + axisValue, chartData.Temperature, '#1cc90c');
                    if ($('#chkLive').is(':checked')) {
                        setTimeout(function () {
                            BindData();
                        }, 5000);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            return false;
        }
        function BindChart(containerID, chartTitle, data, seriesColor) {
            Highcharts.stockChart(containerID, {
                credits: {
                    enabled: false
                },
                rangeSelector: {
                    verticalAlign: 'top',
                    x: 0,
                    y: 0,
                    buttons: [
                        {
                            type: 'hour',
                            count: 2,
                            text: '2h'
                        },
                        {
                            type: 'hour',
                            count: 5,
                            text: '5h'
                        },
                        {
                            type: 'hour',
                            count: 10,
                            text: '10h'
                        },
                        {
                            type: 'hour',
                            count: 20,
                            text: '20h'
                        },
                        {
                            type: 'day',
                            count: 1,
                            text: '1d'
                        },
                        {
                            type: 'all',
                            text: 'All'
                        }],
                    selected: 10,
                    enabled: true,
                    allButtonsEnabled: true,

                    inputEnabled: false
                },
                title: {
                    text: chartTitle,
                    style: {
                        fontSize: '18px',
                        color: '#1f1f8c',
                        fontWeight: 'bold'
                    }
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 60000,
                    minRange: 5,
                    ordinal: false,
                    dateTimeLabelFormats: {
                        month: '%e. %b',
                        year: '%b'
                    },
                },
                navigator: {
                    height: 10,
                    xAxis: {
                        visible: false
                    }
                },
                series: [{
                    name: chartTitle,
                    data: data,
                    color: seriesColor
                }],
            });
        }
        function setControls() {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: 'en-US'
            });
        }
        function BindMachine() {
            $('#ddlMachine').empty();
            $.ajax({
                async: false,
                type: "POST",
                url: "SpindleParameterDashboard.aspx/getMachineData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{plant:"' + $('#ddlPlant').val() + '"}',
                success: function (response) {
                    var list = response.d;
                    var appendStr = "";
                    for (var i = 0; i < list.length; i++) {
                        appendStr += '<option value="' + list[i] + '">' + list[i] + '</option>';
                    }
                    $('#ddlMachine').append(appendStr);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            BindAxisNo();
        }
        function BindAxisNo() {
            $('#ddlAxisNo').empty();
            $.ajax({
                async: false,
                type: "POST",
                url: "SpindleParameterDashboard.aspx/getAxisNoData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{machine:"' + $('#ddlMachine').val() + '"}',
                success: function (response) {
                    var list = response.d;
                    var appendStr = "";
                    for (var i = 0; i < list.length; i++) {
                        appendStr += '<option value="' + list[i] + '">' + list[i] + '</option>';
                    }
                    $('#ddlAxisNo').append(appendStr);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
