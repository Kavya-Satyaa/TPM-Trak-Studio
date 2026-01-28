<%@ Page Title="Statistics Information" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Web_TPMTrakDashboard.Statistics" meta:resourcekey="PageResource1" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <%: Scripts.Render("~/bundles/drilldownChartjs") %>

    <%--  <link href="MyCssAndJS/css/bootstrap.min.css" rel="stylesheet" />--%>
    <%--<link href="MyCssAndJS/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="MyCssAndJS/js/jquery.js"></script>
    <script src="MyCssAndJS/js/bootstrap.min.js"></script>
    <script src="MyCssAndJS/js/bootstrap-datepicker.js"></script>--%>


    <%--    <script src="MyCssAndJS/DatePicker/jquery-2.1.1.min.js"></script>
    <script src="MyCssAndJS/DatePicker/moment-with-locales.js"></script>
    <script src="MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"></script>
    <link href="MyCssAndJS/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />--%>

    <%-----------Highchrat config-------------%>
    <%--<script src="ChartScript/jquery.min.js"></script>--%>


    <style>
        .col-lg-4, .col-lg-8 {
            padding-left: 1px;
            padding-right: 1px;
        }

        th {
            cursor: pointer;
        }

            th.headerSortUp {
                background-image: url(Image/asc.gif);
                background-position: right center;
                background-repeat: no-repeat;
            }

            th.headerSortDown {
                background-image: url(Image/desc.gif);
                background-position: right center;
                background-repeat: no-repeat;
            }

        .table thead > tr > th {
            /*vertical-align: top;*/
            /*padding-top: 30px;*/
        }

        .panel {
            /*margin-bottom: 2px;*/
        }

        .nav > li > a {
            /*padding: 7px 12px;*/
        }

        /*/--------------Boostrap css--------------/*/
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }

        #MainContent_gridLoadUnload, #MainContent_gridCuttingData {
            background-color: #ffffff;
        }

        /*.GridviewScrollHeader {
            position: fixed;
        }

     .myDatagrid td{
         pad]
     }*/
      #statisticChartContainer  .highcharts-container {
          border-radius:10px;
      }
      .headerFixer tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <div class="row text-center">
                <asp:Label ID="lblMessages" runat="server" EnableViewState="False" Style="font-weight: bold; font-family: Calibri; color: red;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                <asp:HiddenField runat="server" ID="hdfValue" Value="Production Data" />
            </div>
            <div class="row text-center" style="font-size: 18px; font-weight: 900; margin-bottom: 5px; color: white;">
                <%=GetLocalResourceObject("UpdatePanel5Heading") %>
                <br />
                <asp:Label ID="lblMachineName" runat="server" Text="---" meta:resourcekey="lblMachineNameResource1"></asp:Label>
            </div>



            <div class="row">
                <div class="col-lg-4 col-lg-offset-2">
                    <div class="panel panel-primary" style="margin-bottom:4px;">
                        <div class="panel-heading"><b><%=GetLocalResourceObject("bCTimeDetails") %></b></div>
                        <table class="table table-bordered">
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblStd") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStd" Text="---" meta:resourcekey="lblStdResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblMax") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMax" Text="---" meta:resourcekey="lblMaxResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblMin") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMin" Text="---" meta:resourcekey="lblMinResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblAvg") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAvg" Text="---" meta:resourcekey="lblAvgResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblRang") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblRang" Text="---" meta:resourcekey="lblRangResource1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="col-lg-4">
                    <div class="panel panel-primary" style="margin-bottom:4px;">
                        <div class="panel-heading"><b><%=GetLocalResourceObject("Panel1Heading") %></b></div>
                        <table class="table table-bordered">

                            <tr>
                                <td><b><%=GetLocalResourceObject("lblStd") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLoadstd" Text="---" meta:resourcekey="lblLoadstdResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblMax") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLoadMax" Text="---" meta:resourcekey="lblLoadMaxResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblMin") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLoadMin" Text="---" meta:resourcekey="lblLoadMinResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblAvg") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLoadAvg" Text="---" meta:resourcekey="lblLoadAvgResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("lblRang") %></b>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLoadRang" Text="---" meta:resourcekey="lblLoadRangResource1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-lg-12 text-center text-primary" style="font-size: 15px; font-weight: bold;">
                    <asp:Label ID="lblNote" runat="server" meta:resourcekey="lblNoteResource1"><sup>***</sup>Units are based on Cockpit Defaults Time format-hh:mm:ss Settings.</asp:Label>
                </div>

            </div>
            <div class="row" style="margin-top:20px;">
                <div class="col-lg-8 col-lg-offset-2">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <b><%=GetLocalResourceObject("Panel2Heading") %></b>
                        </div>
                        <table class="table table-bordered">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="VarianceType" runat="server" CssClass="form-control input-sm" meta:resourcekey="VarianceTypeResource1" ClientIDMode="Static" >
                                        <asp:ListItem Value="Cutting Time" meta:resourcekey="ListItemResource1">Cutting Time</asp:ListItem>
                                        <asp:ListItem Value="Load Unload" meta:resourcekey="ListItemResource2">Load Unload</asp:ListItem>
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="Variance" runat="server" CssClass="form-control input-sm" meta:resourcekey="VarianceResource1" ClientIDMode="Static" onchange="varianceChange();">
                                        <asp:ListItem Value="Above" meta:resourcekey="ListItemResource3">Above</asp:ListItem>
                                        <asp:ListItem Value="Below" meta:resourcekey="ListItemResource4">Below</asp:ListItem>
                                        <asp:ListItem Value="Most Frequent Occ" meta:resourcekey="ListItemResource5">Most Frequent Occ</asp:ListItem>
                                    </asp:DropDownList></td>
                                <td class="text-center">
                                    <asp:TextBox runat="server" ID="txtboxVal" AutoCompleteType="Disabled"  CssClass="numbersOnly form-control" meta:resourcekey="txtboxValResource1" Style="width:60%;display:inline-block" onkeypress="return allowNumberic(event);"></asp:TextBox>&nbsp;%
                                </td>
                                <td class="text-center">
                                    <asp:ImageButton runat="server" ID="imgBtnGo" OnClick="btnGo_Click" ImageUrl="Images/arrowright.jpg" Style="height: 32px;" meta:resourcekey="imgBtnGoResource1"></asp:ImageButton>

                                </td>
                            </tr>

                        </table>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-top:10px">
                <div class="col-lg-8 col-lg-offset-2 text-center text-primary" style="font-size: 18px; font-weight: bold; margin-top: -25px">
                    <asp:Label ID="lblNoOfComp" runat="server" meta:resourcekey="lblNoOfCompResource1">::&nbsp;------------&nbsp;::</asp:Label>
                      <asp:Button ID="btnRefreshHistogramChart" runat="server" Text="Refresh Histogram" CssClass="btn btn-primary" OnClick="btnRefreshHistogramChart_Click" Style="float:right" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-8 col-lg-offset-2" runat="server" id="divMostFreOccScroll" style="overflow-x: hidden; overflow-y: auto; min-height: 100px; max-height: 300px; margin-top: -25px">
                    <div class="text-center text-primary" style="font-size: 18px; font-weight: bold">
                        <asp:Label ID="lblMostFreOcc" runat="server" meta:resourcekey="lblMostFreOccResource1">::&nbsp;------------&nbsp;::</asp:Label>
                    
                    </div>
                </div>
            </div>
            <div class="row" style="margin-top:5px;">
                <div class=" col-lg-8 col-lg-offset-2 " runat="server" id="divcuttimeScroll" style="overflow-x: hidden; overflow-y: auto; min-height: 100px; max-height: 390px; padding-top: 2px;">
                    <asp:GridView ID="gridCuttingData" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" OnSorting="gridCuttingData_Sorting"
                        OnPageIndexChanging="OnPageIndexChangingCutting" PageSize="1000"
                        CssClass="table table-condensed border headerFixer" meta:resourcekey="gridCuttingDataResource1">
                        <HeaderStyle CssClass="GridviewScrollHeader" BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                        <RowStyle CssClass="GridviewScrollItem" />
                        <AlternatingRowStyle BackColor="#CCFFFF" />
                        <PagerStyle CssClass="pagination-ys" />
                        <Columns>
                            <asp:BoundField DataField="StTime" HeaderText="Start Time" SortExpression="StTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="NdTime" HeaderText="End Time" SortExpression="NdTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource2" />
                            <asp:BoundField DataField="CycleTime" HeaderText="Cycle Time(ss)" SortExpression="CycleTime" meta:resourcekey="BoundFieldResource3" />
                            <asp:TemplateField HeaderText="Select Data to Chart">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" ClientIDMode="Static" Checked="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gridLoadUnload" runat="server"  AutoGenerateColumns="False" AllowSorting="True" OnSorting="gridLoadUnload_Sorting"
                        AllowPaging="True" OnPageIndexChanging="OnPageIndexChangingLU" PageSize="1000" CssClass="table table-condensed border headerFixer" meta:resourcekey="gridLoadUnloadResource1">
                        <PagerStyle CssClass="pagination-ys" />
                        <AlternatingRowStyle BackColor="#CCFFFF" />
                        <Columns>
                            <asp:BoundField DataField="StTime" HeaderText="Start Time" SortExpression="StTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource4" />
                            <asp:BoundField DataField="NdTime" HeaderText="End Time" SortExpression="NdTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource5" />
                            <asp:BoundField DataField="Loadunload" HeaderText="LU Time(ss)" SortExpression="Loadunload" meta:resourcekey="BoundFieldResource6" />
                             <asp:TemplateField HeaderText="Select Data to Chart">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" ClientIDMode="Static" Checked="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </div>
            </div>

            <div class="row" style="margin-top: 10px;">
                <div class=" col-lg-8 col-lg-offset-2 " runat="server">
                      <div id="statisticChartContainer"></div>
                  
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
      /*  const { data } = require("jquery");*/

        $(document).ready(function () {
            debugger;
            $.unblockUI({});
        });
        function varianceChange() {
            if ($('#Variance').val() == "Most Frequent Occ") {
                $('[id$=txtboxVal]').attr('readonly', 'readonly');
                $('[id$=txtboxVal]').val("");
            }
            else {
                $('[id$=txtboxVal]').removeAttr('readonly');
            }
        }
        function BindChart() {
            BindHistogramChart();
        }
        function BindHistogramChart() {
            var chartData = [];
            var normalDistribution = [];
            var normalDistributionValue = [];
            var values = [];
            var dataObj = [];
            $.ajax({
                async: false,
                type: "POST",
                url: "Statistics.aspx/getHistogramValueFrequency",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                success: function (response) {

                    var dataitem = response.d;
                    if (dataitem.length == 0) {
                        chartData = [];
                        xchartData = [];
                    }
                    chartData = [];
                    values = [];
                    var hfrequency = [];
                    for (var i = 0; i < dataitem.length; i++) {

                        values[i] = dataitem[i].Value;
                        hfrequency[i] = parseFloat(dataitem[i].Frequency);
                        //var serie = new Array(values[i], hfrequency[i]);
                        //chartData.push(serie);

                        let nextPoint = "";
                        if (i != dataitem.length - 1) {
                            nextPoint = dataitem[i + 1].Value;
                        }

                        let obj = {
                            name: values[i],
                            y: hfrequency[i],
                            nextPoint: nextPoint
                        }
                        dataObj.push(obj);

                    }
                },
                error: function (Result) {
                    alert("Error" + Result);

                }
            });
            console.log("chart data =" + dataObj);
            $.ajax({
                async: false,
                type: "POST",
                url: "Statistics.aspx/getHistogramLineSeriesData",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                success: function (response) {

                    var dataitem = response.d;
                    if (dataitem.length == 0) {
                        chartData = [];
                        xchartData = [];
                    }
                    normalDistribution = [];
                    normalDistributionValue = [];
                    for (var i = 0; i < dataitem.length; i++) {
                        normalDistributionValue[i] = dataitem[i].Value;
                        normalDistribution[i] = dataitem[i].Frequency;
                        console.log("Value =" + normalDistributionValue[i] + "  ND = " + normalDistribution[i]);
                    }
                },
                error: function (Result) {
                    alert("Error" + Result);

                }
            });
            var xLbelText = $('#VarianceType').val();
            if (!xLbelText.includes("Time")) {
                xLbelText += " Time";
            }
            Highcharts.chart('statisticChartContainer', {
                title: {
                    text: xLbelText+" Histogram"
                },
                credits: {
                    enabled: false
                },

                xAxis: [{

                    title: { text: xLbelText + " (ss)"},
                    labels: {
                        //rotation: 45,
                        style: {
                            fontSize: '16px'
                        }
                    },
                    alignTicks: false,
                    categories: values
                }, {
                    title: { text: '' },
                    alignTicks: false,
                    opposite: true,
                    visible: false
                    // categories: value
                }],

                yAxis: [{
                    title: { text: 'Frequency' },
                    labels: {
                        style: {
                            fontSize: '16px'
                        }
                    }
                }, {
                    title: { text: '' },
                    opposite: true,
                    visible: false
                }],

                plotOptions: {
                    column: {
                        pointPadding: 0,
                        borderWidth: 1,
                        groupPadding: 0,
                        pointPlacement: 0.5
                    }
                },

                series: [{
                    name: '',
                    //  type: 'histogram',
                    type: 'column',
                    xAxis: 1,
                    yAxis: 1,
                    // data: chartData,
                    data: dataObj,
                    zIndex: -1,
                    showInLegend: false,
                    tooltip: {
                        pointFormatter: function () {
                            var point = this.point;
                            return `<b>${this.name} - ${this.nextPoint}:</b> <br/> ${this.y}`;
                        }
                    }
                }, {
                        name: ' ',
                        type: 'spline',
                        data: normalDistribution,
                        color: 'red',
                        showInLegend: false,
                        marker: {
                            radius: 1.5
                        },
                        tooltip: {
                            Formatter: function () {
                                return series.name + this.y;
                            }
                        }
                    }]
            });
            return false;
        }
        $(document).on("click", ".pagination-ys a", function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "th a", function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("keyup", ".numbersOnly", function () {
            this.value = this.value.replace(/[^0-9\.]/g, '');
        });

      
        function allowNumberic(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode == 48 && pos == 0) {
                return false
            } else if ((charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        $(document).on("click", "[id$=imgBtnGo]", function () {
            if ($('#Variance').val() != "Most Frequent Occ") {
                if ($("[id$=txtboxVal]").val() == "") {
                    alert("<%=GetLocalResourceObject("PleaseEnterNo%Value")%>");
                    $("[id$=txtboxVal]").focus();
                    return false;
                }
                if (!$.isNumeric($("[id$=txtboxVal]").val())) {
                    alert("Please enter Numeric Value.");
                    $("[id$=txtboxVal]").focus();
                    return false;
                }
                if ($("[id$=txtboxVal]").val() == "0") {
                    alert("Number sholud be greater than zero.");
                    $("[id$=txtboxVal]").focus();
                    return false;
                }
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        })

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            debugger;
            $.unblockUI({});
            varianceChange();
        });
    </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
