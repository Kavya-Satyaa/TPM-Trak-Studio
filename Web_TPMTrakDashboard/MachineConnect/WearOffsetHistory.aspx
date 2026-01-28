<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WearOffsetHistory.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineConnect.WearOffsetHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <style>
        .date-control {
            width: 120px !important;
        }

        .alternate-table-style tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        .alternate-table-style tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnMachineType" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnChartView" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto;">
                <tr>
                    <td class="commanTd" style="vertical-align: middle;">From Date</td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date date-control" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">To Date</td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date date-control" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Machine</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Offset Type</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlOffsetType" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlOffsetType_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Offset</td>
                    <td>
                        <asp:ListBox runat="server" ID="lbOffsetRange" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkTopView" ClientIDMode="Static" CssClass=" checkbox-list" Text="View Top 20" ForeColor="White" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
            <div id="gridContainer" style="overflow: auto; margin-top: 10px;">
                <asp:GridView runat="server" ID="gvOffsetData" CssClass="table table-bordered table-hover headerFixer alternate-table-style" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Timestmap">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Timestamp") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Offset No.">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("OffsetNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Machine Mode">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("MachineMode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Program No.">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ProgramNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wear Offset X">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("OffsetX") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wear Offset Z">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("OffsetZ") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wear Offset R">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("OffsetR") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wear Offset T">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("OffsetT") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <div id="chartContainer" style="margin-top: 10px;">
        <div class="col-lg-6">
            <div id="chartLeftContainer" class="chartdiv"></div>
        </div>
        <div class="col-lg-6">
            <div id="chartRightContainer" class="chartdiv"></div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            setDateTimePicker();
        });
        function BindChart() {
            debugger;
            if ($('#hdnChartView').val() == "True") {
                $('#chartContainer').show();
                $('.chartdiv').css("height", "38vh");
                $('#gridContainer').css("height", "40vh");
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%= ResolveUrl("WearOffsetHistory.aspx/getChartData") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var chartData = response.d;
                        BindHighchart("chartLeftContainer", chartData.LeftChartTitle, chartData.LeftChartData);
                        BindHighchart("chartRightContainer", chartData.RightChartTitle, chartData.RightChartData);
                    },
                    error: function (Result) {
                        alert("Error" + Result);
                    }
                });
            }
            else {
                $('#chartContainer').hide();
                $('#gridContainer').css("height", "80vh");
            }
        }
        function BindHighchart(chartContainer, chartTitle, chartData) {

            Highcharts.chart(chartContainer, {
                chart: {
                    marginRight: 50
                },
                title: {
                    text: chartTitle,
                    style: {
                        fontSize: '18px',
                        fontWeight: 'bold',
                        color: '#4e2775',
                    }
                },

                yAxis: {
                    gridLineWidth: 0,
                    title: {
                        text: "Offset Values",
                        style: {
                            fontSize: '14px',
                            fontWeight: 'bold',
                            color: '#221473',
                        }
                    },
                    labels: {
                        style: {
                            fontSize: '12px',
                            //fontWeight: 'bold',
                            color: '#2d2652',
                        }
                    }
                },
                xAxis: {
                    type: 'datetime',
                    gridLineWidth: 0,
                    allowDecimals: false,
                    title: {
                        text: '',
                        style: {
                            fontSize: '14px',
                            fontWeight: 'bold',
                            color: '#221473',
                        }
                    },
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: '12px',
                            //fontWeight: 'bold',
                            color: '#2d2652',
                        },
                        formatter() {
                            return Highcharts.dateFormat('%Y-%m-%d %H:%M', this.value)
                        }
                    }
                },

                legend: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        turboThreshold: 1000000,
                        dataLabels: {
                            enabled: true,
                            style: {
                                //fontWeight: 'bold',
                                fontSize: '12px',
                            }
                        }
                    }
                },
                rangeSelector: {
                    verticalAlign: 'top',
                    x: 0,
                    y: -40,
                    buttons: [{
                        type: 'minute',
                        count: 5,
                        text: '5M'
                    }, {
                        type: 'minute',
                        count: 30,
                        text: '30M'
                    }, {
                        type: 'hour',
                        count: 5,
                        text: '5h'
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
                    enabled: false,
                    allButtonsEnabled: true,
                    inputEnabled: false
                },
                navigator: {
                    enabled: true,
                    height: 20,
                    xAxis: {
                        type: 'datetime',
                        //width: $('#RuntimeChartContainer').width() - 100
                    }
                },
                series: [{
                    name: chartTitle,
                    data: chartData,
                    color: 'blue',
                    marker: {
                        enabled: true,
                        radius: 2
                    }
                }],

            });
        }
        function setDateTimePicker() {
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });

            $('[id$=lbOffsetRange]').multiselect({
                includeSelectAllOption: true,
                maxHeight: 400,
            });
        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
