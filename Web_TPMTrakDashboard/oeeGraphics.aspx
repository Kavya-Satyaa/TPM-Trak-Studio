<%@ Page Title="oeeGraphics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="oeeGraphics.aspx.cs" Inherits="Web_TPMTrakDashboard.oeeGraphics" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>

    <%-- <script src="MyCssAndJS/Charts/highstock.js"></script>
    <script src="MyCssAndJS/Charts/exporting.js"></script>--%>
    <%: Scripts.Render("~/bundles/VDGjs") %>


    <style>
        .panel {
            margin-bottom: 11px;
            background-color: #ffffff;
            border: 0px solid transparent;
            -webkit-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
        }

        .panel {
            border: none;
            -webkit-border-radius: 0px;
            -moz-border-radius: 0px;
            border-radius: 0px;
            -webkit-box-shadow: none;
            -moz-box-shadow: none;
            box-shadow: none;
            /* margin-bottom: 30px; */
        }

        .row {
            margin-left: 0px;
            margin-right: 0px;
        }

        .table.table-bordered {
            background-color: white;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row text-center">
                <asp:Label ID="lblMessages" runat="server" EnableViewState="False" Style="font-weight: bold; font-family: Calibri; color: red;" meta:resourcekey="lblMessagesResource1"></asp:Label>
            </div>

            <div class="row text-center" style="font-size: 18px; font-weight: 900; color: white;">
                <%=GetLocalResourceObject("OEEGraphics") %> - <%= Session["MachineId"]%>
            </div>

            <div class="row">
                <div class="panel panel-primary">
                    <div class="panel-heading"><b><%=GetLocalResourceObject("CustomSearch") %></b></div>
                    <table class="table table-bordered">
                        <tr>
                            <td style="padding-top: 15px;">
                                <b><%=GetLocalResourceObject("From") %></b>
                            </td>
                            <td class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" placeholder="DD-MMM-YYYY" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                            </td>
                            <td style="padding-top: 15px;"><b><%=GetLocalResourceObject("ToDate") %></b>
                            </td>
                            <td class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" placeholder="DD-MMM-YYYY" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                            </td>
                            <td style="padding-top: 15px;"><b><%=GetLocalResourceObject("MachineID") %></b> </td>

                            <td>
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control input-sm" meta:resourcekey="ddlMachineIdResource1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnRefresh" CssClass="btn btn-info btn-sm" Text="Refresh" runat="server" OnClick="btnRefresh_Click" meta:resourcekey="btnRefreshResource1" />
                            </td>
                        </tr>
                    </table>
                </div>
                <table class="table table-bordered">
                    <tr>
                        <td><b><%=GetLocalResourceObject("TotalTime(TT)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblTTMin" meta:resourcekey="lblTTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("FromTimeToTime") %>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblTT" meta:resourcekey="lblTTResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Utilised Time (UT)</b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblUTMin" meta:resourcekey="lblOTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("UtilizedTime") %>=Actual Production Time
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("UtilizedTimeforAllComponents") %>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Actual Down Time (DT)</b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblDTMin" meta:resourcekey="lblDTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0">Actual Down Time
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblDT" meta:resourcekey="lblDTResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b><%=GetLocalResourceObject("PlannedLosses(PL)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblPLMin" meta:resourcekey="lblPLMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0">PL=Total Time-(Utilised Time+Actual Down Time)
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblPL" meta:resourcekey="lblPLResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Parts Produced</b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblPartProduced" meta:resourcekey="lblDTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0">[No.]</td>
                        <td style="background-color: #FFFFC0"></td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="Label2" meta:resourcekey="lblDTResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Rejected Parts</b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblRejectedParts" meta:resourcekey="lblDTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0">[No.]</td>
                        <td style="background-color: #FFFFC0"></td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="Label3" meta:resourcekey="lblDTResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Standard Production Time (SPT)</b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblSPTMin" meta:resourcekey="lblSLMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("SLOTNOT") %>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblSPT">
                                Σ (Standard Cycle Time * No.of Components)
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b><%=GetLocalResourceObject("AvailabilityEfficiency(AE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblAEMin" meta:resourcekey="lblAEMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblAEInPerc"></asp:Label>
                            [%]
                        </td>
                        <td style="background-color: #FFC080">AE=[(Total Time-Planned Loss) - (Actual Down Time) / (Total Time-Planned Loss)];
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblAE" meta:resourcekey="lblAEResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b><%=GetLocalResourceObject("ProductionEfficiency(PE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblPEMin" meta:resourcekey="lblPEMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblPEInPerc"></asp:Label>
                            [%]
                        </td>
                        <td style="background-color: #FFC080">PE=[(Standard Production Time)/Utilised Time];
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblPE" meta:resourcekey="lblPEResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b><%=GetLocalResourceObject("QualityEfficiency(QE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblQEMin"></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblQEInPerc"></asp:Label>
                            [%]
                        </td>
                        <td style="background-color: #FFC080">QE=[(Parts Produced - Rejection Parts)/Parts Produced];
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblQE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b><%=GetLocalResourceObject("OverAllEfficiency(OEE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblOEEMin" meta:resourcekey="lblOEEMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblOEEInPerc"></asp:Label>
                            [%]
                        </td>
                        <td style="background-color: #FFC080">OEE = [AE*PE*QE] * 100
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblOEE" meta:resourcekey="lblOEEResource1"></asp:Label>
                        </td>
                    </tr>
                </table>
                <%-- <table class="table table-bordered">
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("TotalTime(TT)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblTTMin" meta:resourcekey="lblTTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("FromTimeToTime") %>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblTT" meta:resourcekey="lblTTResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("PlannedLosses(PL)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblPLMin" meta:resourcekey="lblPLMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("PLTTOTDT") %>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblPL" meta:resourcekey="lblPLResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("PlannedProcedureTime(PPT)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblPPTMin" meta:resourcekey="lblPPTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("PPTTLPL") %>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblPPT" meta:resourcekey="lblPPTResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("Stoppage(DT)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblDTMin" meta:resourcekey="lblDTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("DTPPTOT") %>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblDT" meta:resourcekey="lblDTResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("OperatingTime(OT)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblOTMin" meta:resourcekey="lblOTMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("UtilizedTime") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("UtilizedTimeforAllComponents") %>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("AvailabilityEfficiency(AE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblAEMin" meta:resourcekey="lblAEMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">[%]
                        </td>
                        <td style="background-color: #FFC080"><%=GetLocalResourceObject("AEOT(OTDT)") %>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblAE" meta:resourcekey="lblAEResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("SpeedLoss(SL)") %></b>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblSLMin" meta:resourcekey="lblSLMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("mins") %>
                        </td>
                        <td style="background-color: #FFFFC0"><%=GetLocalResourceObject("SLOTNOT") %>
                        </td>
                        <td style="background-color: #FFFFC0">
                            <asp:Label runat="server" ID="lblSL" meta:resourcekey="lblSLResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("ProductionEfficiency(PE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblPEMin" meta:resourcekey="lblPEMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">[%]
                        </td>
                        <td style="background-color: #FFC080"><%=GetLocalResourceObject("PENOTOT") %>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblPE" meta:resourcekey="lblPEResource1"></asp:Label>
                        </td>
                    </tr>
                      <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("QualityEfficiency(QE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblQEMin" ></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">[%]
                        </td>
                        <td style="background-color: #FFC080"><%=GetLocalResourceObject("QEOTPPT") %>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblQE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b style="color: white"><%=GetLocalResourceObject("OverAllEfficiency(OEE)") %></b>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblOEEMin" meta:resourcekey="lblOEEMinResource1"></asp:Label>
                        </td>
                        <td style="background-color: #FFC080">[%]
                        </td>
                        <td style="background-color: #FFC080"><%=GetLocalResourceObject("OEEEOTPPT") %>
                        </td>
                        <td style="background-color: #FFC080">
                            <asp:Label runat="server" ID="lblOEE" meta:resourcekey="lblOEEResource1"></asp:Label>
                        </td>
                    </tr>
                    
                </table>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- <div id="OEEChart" style="visibility:hidden">
    </div>--%>
    <script type="text/javascript">
        $(document).ready(function () {

            //$("[id$=toggle]").click(function () {
            //    alert('hii');
            //    //$('#OEEChart').addClass("col-lg-10 col-lg-offset-1");
            //    $('#OEEChart').highcharts().redraw();
            //})

            $.unblockUI({});
            dateTimePicker();
            GetOEEChartData();
            $("[id$=btnRefresh]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("Please enter the From Date !!");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("Please enter the To Date !!");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });

        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            dateTimePicker();
            GetOEEChartData();
            $("[id$=btnRefresh]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("Please enter the From Date !!");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("Please enter the To Date !!");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //GetOEEChartData();
                //  GetDownTimeChartData();
            });

        });

        function dateTimePicker() {
            $('.date1').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetLocalResourceObject("DateLang")%>'
            });
            $('.date2').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                useCurrent: false,
                locale: '<%=GetLocalResourceObject("DateLang")%>'
            });
            $(".date1").on("dp.change", function (e) {
                $('.date2').data("DateTimePicker").minDate(e.date);
            });
            $(".date2").on("dp.change", function (e) {
                $('.date1').data("DateTimePicker").maxDate(e.date);
            });
        }


        function drawOEEChart(data) {

            //chart = Highcharts.chart('OEEChart', {
            $('#OEEChart').highcharts({
                //colors: ['#6666FF', '#AAAAFF'],
                credits: {
                    enabled: false
                },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    inverted: true
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: data.categories,
                    labels: {

                        style: {
                            fontSize: '10px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    },
                },
                yAxis: {
                    //min: 0,
                    title: {
                        text: ''
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            fontWeight: 'bold',
                            color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                        }
                    }
                },
                legend: {
                    align: 'center',
                    enabled: false,
                },
                //tooltip: {
                //    headerFormat: '<b>{point.x}</b><br/>',
                //    pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                //},
                plotOptions: {
                    showInLegend: false,
                    column: {
                        stacking: 'normal',

                        dataLabels: {
                            enabled: true,
                            color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                            formatter: function () {
                                if (this.y != 0) {
                                    return this.y;
                                } else {
                                    return null;
                                }
                            }
                        }

                    },

                },
                series: data.series
            });

            $("[id$=toggle]").click(function () {
                // alert('redd');
                var chart = $('#OEEChart').highcharts();
                chart.reflow();
            })
        }

        function GetOEEChartData() {
            $.ajax({
                type: "POST",
                url: "oeeGraphics.aspx/GetOEEChartData",
                contentType: "application/json; charset=utf-8",
                data: '{machineId:"' + $("[id$=ddlMachineId]").val() + '", strfromDate:"' + $("[id$=txtFromDate]").val() + '", strtoDate:"' + $("[id$=txtToDate]").val() + '"}',
                dataType: "json",
                success: OnSuccessGetOEEChartData,
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function OnSuccessGetOEEChartData(Result) {
            drawOEEChart(Result.d)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
