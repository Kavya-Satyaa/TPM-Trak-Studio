<%@ Page Language="C#" Title="Energy Dashboard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnergyDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.EnergyDashboard" meta:resourcekey="PageResource1" Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
     <%: Scripts.Render("~/bundles/drilldownChartjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>

    <style>
        th {
            cursor: pointer;
        }

        .table thead > tr > th {
            vertical-align: top;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 2px solid;
            margin: 0 auto 1em auto; /* centers */
            border-color: darkgray;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        .table tbody > tr > th {
            color: white;
            text-align: center;
        }

        .chartHeader {
            background-color: #2E6886 !important;
        }

        .tbl th {
            width: 10%;
        }

        .tbl td {
            width: 10%;
        }

        .displayCss {
            display: inline;
        }

        .footerCss {
            background-color: #2E6886 !important;
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

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        text {
            text-decoration: none !important;
        }

        .ui-datepicker .ui-datepicker-prev,
        .ui-datepicker .ui-datepicker-next {
            display: none;
        }

        #tblDashboardInfo tbody tr:nth-child(odd) {
            background-color: white;
        }

        #tblDashboardInfo tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .chartColor {
            margin-top: 3px;
            font-size: 22px;
            cursor: pointer;
            color: white;
        }

        .highcharts-title {
            width: 600px;
            text-align: center;
            top: 1px;
        }

        .multiselect-container {
            overflow-y: auto;
        }

        #tblHeader {
            margin-bottom: 0px;
        }

        .dropdowncss {
            font-weight: bold;
            font-size: 1em;
        }

        .HeaderCss {
            color: white;
            background-color: #428bca;
            font-weight: bold;
            min-width: 100px;
        }

        @media screen and (min-width: 1920px) {
            .enrgyDashbordHourlycss {
                height: 420px;
                overflow-x: auto;
                overflow-y: auto;
                border-style: solid;
                border-color: DarkGray;
            }
        }

        @media screen and (min-width: 1200px) {
            .enrgyDashbordHourlycss {
                height: 365px;
                overflow-x: auto;
                overflow-y: auto;
                border-style: solid;
                border-color: DarkGray;
            }
        }

        @media screen and (min-width: 1600px) {
            .enrgyDashbordcss {
                height: 350px;
                overflow-x: auto;
                overflow-y: auto;
                border-style: solid;
                border-color: DarkGray;
            }
        }
    </style>

    <div class="row" style="text-align: center; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
    </div>
    <div class="row" style="margin-bottom: 4px; margin-top: -3px;">
        <table id="tblHeader" class="table table-bordered" style="background-color: #394A59; height: 40px;">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlView" Style="min-width: 40px; max-width: 150px;" runat="server" CssClass="select form-control dropdowncss" ToolTip="Select View" OnSelectedIndexChanged="ddlView_SelectedIndexChanged">
                        <asp:ListItem Text="Node View" Value="Node View" />
                        <asp:ListItem Text="Machine View" Value="Machine View" />
                    </asp:DropDownList>
                </td>
               <td class="commontd" style="width: 60px;"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                <td class="input-group" style="margin-bottom: 0px;height:40px">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox style="max-width: 200px;height:40px" ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="From DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                </td>
                <td class="commontd" style="width:100px"><b>Quick Selection</b></td>
                <td>
                    <asp:DropDownList ID="ddlShift" runat="server" CssClass="select form-control dropdowncss" ToolTip="Predefine Time Period" Style="min-width: 100px; max-width: 150px;" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged" AutoPostBack="True" />
                </td>
				<td>

				</td>
                
                <%--<td style="padding-left: 4px; cursor: pointer;">
                    <button type="button" class="btn btn-info btn-sm" id="btnGenerate" runat="server" visible="false">
                        <span class="glyphicon glyphicon-export"></span>Export
                    </button>
                </td>--%>
            </tr>
			<tr>
			 <td>
                    <asp:DropDownList ID="ddlPlantId" Style="min-width: 40px; max-width: 150px;" runat="server" CssClass="select form-control dropdowncss" ToolTip="Select Plant" />
                </td>
                
				

                <td class="commontd" style="width: 60px;"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                <td class="input-group" style="margin-bottom: 0px;height:40px;">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox style="max-width: 200px;height:40px" ID="txtToDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="To DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                </td>
				<td class="commontd">
					<b>Param</b>
				</td>
				<td>
					<asp:DropDownList ID="ddlcolumnselection" runat="server" CssClass="bootstrap-select form-control dropdowncss"  Style="min-width: 100px; max-width: 150px;" >
						 <asp:ListItem Text="Phase2Neutral(R2N, Y2N, G2N)" Value="Phase2Neutral (R2N, Y2N, G2N)" />
						<asp:ListItem Text="Phase2Phase (R2Y, Y2G, G2R)" Value="Phase2Phase (R2Y, Y2G, G2R)" />
						<asp:ListItem Text="Current (A1, A2, A3)" Value="Current (A1, A2, A3)" />
                        <asp:ListItem Text="PF (PF1, PF2, PF3)" Value="PF (PF1, PF2, PF3)" />
						</asp:DropDownList>
				</td>
				<td style="padding-left: 2px;">
                    <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="btn btn-primary" OnClick="btnProcess_Click" />
                </td>
				<%--<td>
					  <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
				</td>--%>
			</tr>
        </table>
    </div>
    <div class="container-fluid" style='font-family: Calibri; font-style: normal; font-weight: 700;'>
        <div class="row enrgyDashbordHourlycss headerFixer" style="background-color: #DCE7F5">
            <asp:UpdatePanel ID="updateEnergyViewData" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="EnergyGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="#5391CA" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Large" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true" OnRowDataBound="EnergyGridView_RowDataBound" >
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <%--<Columns>

                            <asp:HyperLinkField HeaderText="Machine ID" DataNavigateUrlFields="MachineID" DataTextField="MachineID" Target="_blank" />
                        </Columns>--%>
                        <EmptyDataTemplate>
                            No data available for selected plant and date time period.
                        </EmptyDataTemplate>
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Large" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div id="CumulativeEnergyChart" style="width: 100%;"></div>
            <div style='font-size: 15px; font-weight: 900;'>
                <asp:Label ID="lblCostDescription" runat="server" Text="* indicates - Cost is considered as INR xx/unit"></asp:Label>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var chart = null;
        $(document).ready(function () {
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
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            // date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
            GetEnergyChartData();
        });

        var EnergyData = {};
        function GetEnergyChartData() {
            showLoader();
            var plantId = $("[id$=ddlPlantId]").val() == "All" || $("[id$=ddlPlantId]").val() == null ? "" : $("[id$=ddlPlantId]").val();
            var fromDate = $("[id$=txtFromDate]").val();
            var todate = $("[id$=txtToDate]").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "EnergyDashboard.aspx/GetEnergyChartData",
                data: '{plantId:"' + plantId + '", fromDate:"' + fromDate + '", todate:"' + todate + '"}',
                dataType: "json",
                success: function (result) {
                    EnergyData = result.d;
                    PlotEnergyChart(EnergyData);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function PlotEnergyChart(energyData) {
            chart = Highcharts.chart('CumulativeEnergyChart', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Energy Data'
                },
                subtitle: {
                    text: ''
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
                    headerFormat: '<span style="font-size:11px; font-weight:bold;">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> kWH<br/>'
                },

                "series": [
                    {
                        "name": "Machine ID/Node ID",
                        "colorByPoint": true,
                        "data": energyData
                    }
                ]
            });
            hideLoader();
        }

        //$("[id$=btnProcess]").click(function () {
        //    var view = $("[id$=ddlView]").val();
        //    if (view == "Machine View") {
        //        chart.setTitle({
        //            text: 'Machinewise Energy Data'
        //        });
        //        //chart.title.text = "Machinewise Energy Data";
        //        //chart.subtitle.text = "Machinewise energy data for every machine";
        //        //chart.series.name = "Machine ID";
        //    }
        //    if (view == "Node View") {
        //        chart.setTitle({
        //            text: 'Nodewise Energy Data'
        //        });
        //        //chart.title.text = "Nodewise Energy Data";
        //        //chart.subtitle.text = "Nodewise energy data for every node";
        //        //chart.series.name = "Node ID";
        //    }
        //});

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
