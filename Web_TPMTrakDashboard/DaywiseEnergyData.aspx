<%@ Page Language="C#" Title="Daywise Energy Data" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="DaywiseEnergyData.aspx.cs" Inherits="Web_TPMTrakDashboard.DaywiseEnergyData" meta:resourcekey="PageResource1" Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
     <%: Scripts.Render("~/bundles/drilldownChartjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>

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
    </style>

    <div class="container">
        <div class="row text-center" style="font-size: 18px; font-weight: 900; color: white">
            <asp:Label runat="server" ID="lblTitle" Text='<%=GetLocalResourceObject("HourlyEnergyData") %>' meta:resourcekey="lblTitleResource1"></asp:Label>
            <br />
            <asp:Label ID="lblMessages" runat="server" EnableViewState="False" meta:resourcekey="lblMessageResource1"></asp:Label>
        </div>
    </div>

    <div style='font-family: Calibri; font-style: normal; font-weight: 700;'>
        <div class="row" style="margin-top: 5px; margin-left: 20px; margin-right: 20px;">
            <table id="tblHeader" class="table table-bordered" style="background-color: #394A59; height: 50px; margin-bottom: 10px;">
                <tr>
                    <td class="commontd" id="tdMachineId" runat="server"><b style="font-size: medium;">Machine ID</b></td>
                    <td id="tdddlMachineId" runat="server">
                        <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="select form-control dropdowncss" ToolTip="Select Machine ID" Style="min-width: 100px; max-width: 168px;" AutoPostBack="True" meta:resourcekey="ddlMachineIdResource1" />
                    </td>
                    <td class="commontd"><b style="font-size: medium;"><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                    <td class="input-group" style="margin-bottom: 0px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="From DateTime !" placeholder="DateTime" ToolTip="<%$ Resources:CommanResource, Date %>" AutoCompleteType="Disabled" meta:resourcekey="txtFromDateResource1" />
                    </td>

                    <td class="commontd"><b style="font-size: medium;"><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                    <td class="input-group" style="margin-bottom: 0px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="To DateTime !" placeholder="DateTime" ToolTip="<%$ Resources:CommanResource, Date %>" AutoCompleteType="Disabled" meta:resourcekey="txtToDateResource1" />
                    </td>

                    <td class="commontd" id="tdNodeId" runat="server"><b style="font-size: medium;">Node ID</b></td>
                    <td id="tdddlNodeId" runat="server">
                        <asp:DropDownList ID="ddlNodeId" runat="server" CssClass="select form-control dropdowncss" ToolTip="Select Node ID" Style="min-width: 100px; max-width: 168px;" meta:resourcekey="ddlNodeIdResource1" />
                    </td>
					<td class="commontd">
					<b>Param</b>
				</td>
				<td>
					<asp:DropDownList ID="ddlcolumnselection" runat="server" CssClass="bootstrap-select form-control dropdowncss"  Style="min-width: 100px; max-width: 150px;">
						 <asp:ListItem Text="Phase2Neutral(R2N, Y2N, G2N)" Value="Phase2Neutral (R2N, Y2N, G2N)" />
						<asp:ListItem Text="Phase2Phase (R2Y, Y2G, G2R)" Value="Phase2Phase (R2Y, Y2G, G2R)" />
						<asp:ListItem Text="Current (A1, A2, A3)" Value="Current (A1, A2, A3)" />
                        <asp:ListItem Text="PF (PF1, PF2, PF3)" Value="PF (PF1, PF2, PF3)" />
						</asp:DropDownList>
				</td>
                    <td style="padding-left: 2px;">
                        <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="btn btn-primary" OnClick="btnProcess_Click" meta:resourcekey="btnProcessResource1" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div style='font-family: Calibri; font-style: normal; font-weight: 700; margin-right: 20px; margin-left: 20px;'>
        <div style="max-height: 440px; overflow-y: auto; margin-top: 5px;" class="headerFixer">
            <asp:GridView ID="gridViewDaywiseEnergyData" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" ShowHeaderWhenEmpty="True" EmptyDataText="<%$ Resources:CommanResource, Nodataavailable %>" HorizontalAlign="Center" meta:resourcekey="gridViewDaywiseEnergyDataResource1" HeaderStyle-BackColor="#5391CA" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Large" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gridViewDaywiseEnergyData_RowDataBound">
                <AlternatingRowStyle BackColor="#DCE7F5" />
                <Columns>
                    <asp:HyperLinkField HeaderText="Day" DataTextField="StartTime" DataNavigateUrlFields="StartTime" Target="_blank" meta:resourcekey="BoundFieldResource4" DataTextFormatString="{0:dd-MMM-yyyy}" />
                  <%--  <asp:BoundField HeaderText="Production Time" DataField="ProductionTime" meta:resourcekey="BoundFieldResource11" />
                    <asp:BoundField HeaderText="Parts Count" DataField="ProductionCount" meta:resourcekey="BoundFieldResource12" />
                    <asp:BoundField HeaderText="Energy" DataField="Energy" meta:resourcekey="BoundFieldResource7" />
                    <asp:BoundField HeaderText="VLN-R (Min./Max.)" DataField="Volt1" meta:resourcekey="BoundFieldResource8" />
                    <asp:BoundField HeaderText="VLN-Y (Min./Max.)" DataField="Volt2" meta:resourcekey="BoundFieldResource9" />
                    <asp:BoundField HeaderText="VLN-B (Min./Max.)" DataField="Volt3" meta:resourcekey="BoundFieldResource10" />
                    <asp:BoundField HeaderText="Power Factor (PF)" DataField="PF" meta:resourcekey="BoundFieldResource2" />--%>
                    <asp:BoundField HeaderText="Cost" DataField="Cost" DataFormatString="{0:N0}" HtmlEncode="false" meta:resourcekey="BoundFieldResource3" Visible="false" />
                </Columns>
                <EmptyDataTemplate>
                    No data available for selected machine ID.
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
        <div style='font-family: Calibri; font-style: normal; font-weight: 700; margin-top: 5px;'>
            <div id="DaywiseEnergyChart" style="width: 100%;" />
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

            GetDaywiseEnergyChartData();
        });

        var DaywiseEnergyData = {};
        function GetDaywiseEnergyChartData() {
            showLoader();
            var param = "Day";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "DaywiseEnergyData.aspx/GetDaywiseEnergyChartData",
                data: '{param:"' + param + '"}',
                dataType: "json",
                success: function (result) {
                    DaywiseEnergyData = result.d;
                    PlotDaywiseEnergyChart(DaywiseEnergyData);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        }

        function PlotDaywiseEnergyChart(energyData) {
            chart = Highcharts.chart('DaywiseEnergyChart', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Daywise Energy Data'
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
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> kWH<br/>'
                },

                "series": [
                    {
                        "name": "Day",
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
