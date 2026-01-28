<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="PPMPage.aspx.cs" Inherits="Web_TPMTrakDashboard.PPMPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/paretoandDrillDownChartJs") %>
    <%: Scripts.Render("~/bundles/paretoChartJs") %>
    <style>
        th {
            color: white !important;
            background-color: #2E6886 !important;
        }

        td[colspan="4"] {
            text-align: center;
        }

        #MainContent_gridPPM tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_gridPPM tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }

        .HeaderCss {
            background-color: #DCDCDC;
            /*//font-weight: bold;*/
            min-width: 100px;
        }

            .HeaderCss th {
                color: white;
            }

        .hyperlink {
            cursor: pointer;
        }

        .labels {
            text-align: center;
            vertical-align: middle;
            color: ghostwhite;
            font-size: 17px;
            font-weight: 500;
        }
    </style>

    <asp:UpdatePanel runat="server" ID="update">
        <ContentTemplate>
            <div class="row">
                <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white">
                    PPM - <%= Session["ppmMachineId"]%>
                    <br />
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False" meta:resourcekey="lblMessageResource1"></asp:Label>
                </div>
                <div class="row">
                    <div class="col-lg-4" style="display: inline-block; padding: 5px; text-align: center;">
                        <asp:CheckBox runat="server" ID="IgnoreMachineID" ClientIDMode="Static" Style="vertical-align: middle; font-size: 18px; color: ghostwhite; padding: 5px;" />
                        <asp:Label CssClass="labels" ID="lblMachineID" runat="server" Text="Ignore MachineID"></asp:Label>
                        <asp:CheckBox runat="server" ID="IgnoreComponentID" ClientIDMode="Static" Style="vertical-align: middle; font-size: 18px; color: ghostwhite; padding: 5px;" />
                        <asp:Label CssClass="labels" ID="lblComponentID" runat="server" Text="Ignore ComponentID"></asp:Label>
                        <asp:CheckBox runat="server" ID="IgnoreOperation" ClientIDMode="Static" Style="vertical-align: middle; font-size: 18px; color: ghostwhite; padding: 5px;" />
                        <asp:Label CssClass="labels" ID="lbloperation" runat="server" Text="Ignore Operation"></asp:Label>
                        <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" class="btn btn-info btn-sm displayCss plantDrop" Style="height: 90%; margin-left: 5px" />
                    </div>
                    <div class="row text-center col-lg-8" style="font-size: 16px; font-weight: 600; color: white; padding: 10px;">
                        <asp:Label ID="lblHeading" runat="server" EnableViewState="False" ClientIDMode="Static" meta:resourcekey="lblMessageResource1"></asp:Label>
                    </div>
                </div>
                <div runat="server" id="divMach" class="col-lg-4">
                    <div style="overflow: auto" id="Grid">
                        <asp:GridView runat="server" ID="gridPPM" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" ShowHeader="true" CssClass="table table-bordered">
                            <Columns>
                                <asp:BoundField HeaderText="ComponentID" DataField="ComponentID" />
                                <asp:BoundField HeaderText="MachineID" DataField="MachineID" />
                                <asp:BoundField HeaderText="Operation" DataField="Operationno" />
                                <asp:BoundField HeaderText="Accepted" DataField="AcceptedParts" />
                                <asp:BoundField HeaderText="Rejected" DataField="RejectedParts" />
                                <asp:TemplateField HeaderText="PPM">
                                    <ItemTemplate>
                                        <a><span class="Hyperlink hyperlink " style="text-decoration: underline;"><%# Eval("PPM") %></span> </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="HeaderCss" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="col-lg-8">

                    <div id="PPMchart">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        $(document).ready(function () {
            var winHeight = $(window).height();
            winHeight = screen.availHeight;
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (640);
                console.log('min');
            } else {
                //winHeight = (840);
                console.log('max');
            }

            $("#Grid").height(winHeight - 210);



            $(document).on("click", "tr .Hyperlink", function (e) {
                
                //$(this).closest("tr").css("background-color","black");
                //$(this).closest("tr").css("color","white");
                var newSelectedValue = $(this).closest("tr").find("td").eq(0).html();
                var newoperation = $(this).closest("tr").find("td").eq(1).html();
                var newAccepted = $(this).closest("tr").find("td").eq(2).html();
                var newRejected = $(this).closest("tr").find("td").eq(3).html();
                var newPPM = $(this).closest("tr").find("td").eq(4).html();
                var Type = "<%=Type%>";
                var newMachine = "";
                var newOperation= "";
                if(Type == "ComponentwiseView")
                {
                    if($('#IgnoreMachineID')[0].checked)
                    {
                        newMachine  = "All";
                        newSelectedValue = "";
                        var newoperation = $(this).closest("tr").find("td").eq(0).html();
                    }
                    else
                    {
                        newMachine = newSelectedValue;
                        newOperation = newoperation
                    }
                    if($('#IgnoreOperation')[0].checked)
                    {
                        newoperation = "";
                        newOperation = "All";
                    }
                    else
                    {
                        newOperation = newoperation
                    }
                    $('#lblHeading').text("Machine-ID: " +newMachine + "; Operation: " + " " + newOperation);
                }
                else
                {
                    if($('#IgnoreComponentID')[0].checked)
                    {
                        newMachine  = "All";
                        newSelectedValue = "";
                        var newoperation = $(this).closest("tr").find("td").eq(0).html();
                    }
                    else
                    {
                        newMachine = newSelectedValue;
                        newOperation = newoperation
                    }
                    if($('#IgnoreOperation')[0].checked)
                    {
                        newoperation = "";
                        newOperation = "All";
                    }
                    else
                    {
                        newOperation = newoperation
                    }
                    $('#lblHeading').text("Component-ID: " +newMachine + "; Operation: " + " " + newOperation);
                }
                //PopupCenter("PPMGraph.aspx?SelectedValue=" + SelectedValues + "&Opeartion=" + Opeartions + "&Accepted=" + Accepteds + "&Rejected=" + Rejecteds + "&PPM=" + PPMs, "PPM Graph", 1200, 800);
                //GetGraphdetails(newSelectedValue);
                var param = "";
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "PPMPage.aspx/Getdata",
                    data: '{SelectedValue:"' + newSelectedValue + '", OperationNo:"' + newoperation  + '", Type:"' + Type + '"}',
                    dataType: "json",
                    success: Graph,
                    error: function (Result) {
                        alert("Error");
                    }
                });
            });
        });

        function GetGraphdetails() {
            var param = "";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PPMGraph.aspx/Getdata",
                data: '{}',
                dataType: "json",
                success: second,
                error: function (Result) {
                    alert("Error");
                }
            });

        }

        function Graph(data) {
            var i=0;
            console.log(data);
            var chartCol =   Highcharts.chart('PPMchart', {
                chart: {
                    //renderTo: 'container',
                    type: 'column',
                    events: {
                        drilldown: function (e) {
                            drilldown(e, this, "col");
                            
                            cont = <%= maxvalue %>;
                            cont = parseInt(cont) + 1;
                            catValue = data.d.categories;
                            catValue = catValue.slice(cont);
                            chartCol.xAxis[0].setCategories(catValue);
                            //chartCol.series[0].hide(true);
                            //chartCol.yAxis[1].enabled(true);
                        },
                        drillup: function (e) {
                            
                            cont = <%= maxvalue %>;
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(0, cont);
                            chartCol.xAxis[0].setCategories(catValue);
                            chartCol.xAxis[0].setCategories(catValue);
                            chartCol.series[1].setData(data.d.series);
                            chartCol.series[2].setName("PPM");
                        }

                    }
                },
                title: {
                    text: 'PPM'
                },
                tooltip: {
                    shared: true,
                    formatter: function() {
                        
                        var value = '';var value2 = '';var value3 = '';
                        value3= data.d.RejQty[i];i++;
                        if(this["points"].length >1 )
                            return 'Pareto value: ' + Highcharts.numberFormat(this["points"][0].y,2) +'(%)<br/>'+this["points"][0].x+'(PPM): ' + this["points"][1].y +'<br/>' +'Rejection Qty: ' + data.d.RejQty[this["points"][0].point.index];
                        else
                            return  this["points"][0].x+'(PPM): ' + this["points"][0].y +'<br/>' +'Rejection Qty: ' + data.d.RejQty[this["points"][0].point.index];
                    }
                },
                xAxis: {
                    categories: data.d.categories,
                    crosshair: true,
                    title: {
                        text: 'Rejection Codes'
                    }
                },
                yAxis: [{
                    title: {
                        text: 'PPM'
                    }
                }, {
                    title: {
                        text: ''
                    },
                    minPadding: 0,
                    maxPadding: 0,
                    max: 100,
                    min: 0,
                    opposite: true,
                    labels: {
                        format: "{value}"
                    }
                }],
                series: [{
                    type: 'pareto',
                    name: 'Pareto',
                    yAxis: 1,
                    baseSeries: 1,
                    tooltip: {
                        valueDecimals: 2,
                        valueSuffix: '%'
                    }
                }, {
                    name: 'PPM',
                    type: 'column',
                    zIndex: 2,
                    data: data.d.series,
                },
                ],
                drilldown: {
                    series: data.d.Drilldown,
                }
            });
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
