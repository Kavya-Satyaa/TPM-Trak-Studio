<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rejection_Globe.aspx.cs" Inherits="Web_TPMTrakDashboard.Rejection_Globe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>

    <%--    <%: Scripts.Render("~/bundles/paretoandDrillDownChartJs") %>
    <%: Scripts.Render("~/bundles/paretoChartJs") %>--%>
  <%--  <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/pareto.js"></script>
    <script src="https://code.highcharts.com/modules/sankey.js"></script>
    <script src="https://code.highcharts.com/modules/organization.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>
    <script src="https://code.highcharts.com/modules/drilldown.js"></script>--%>
    <script src="Scripts/HighCharts9.1.0/highcharts.js"></script>
    <script src="Scripts/HighCharts9.1.0/pareto.js"></script>
    <script src="Scripts/HighCharts9.1.0/sankey.js"></script>
    <script src="Scripts/HighCharts9.1.0/organization.js"></script>
    <script src="Scripts/HighCharts9.1.0/data.js"></script>
    <script src="Scripts/HighCharts9.1.0/drilldown.js"></script>

    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }


        .colorTotal tr td:last-child {
            background-color: #FFFF00;
            font-weight: bold;
        }

        .colorTotal tbody tr:last-child {
            background-color: #FFFF00;
            font-weight: bold;
        }

        .multiselect {
            width: 150px;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
            text-align: left;
            width: 250px;
        }

        .multiselect-selected-text {
            padding-right: 181px;
            white-space: normal;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .commontd {
            min-width: 120px;
        }

        .drp {
            min-width: 120px;
        }

        .cal {
            min-width: 100px;
        }

        .multiselect-selected-text {
            padding-right: 88px;
        }

        @media only print {
            body * {
                visibility: hidden;
            }

            #container-col, #container-col * {
                visibility: visible;
            }

            #container-col {
                position: absolute;
                left: 0;
                top: 0;
            }
        }

        .multiselect.dropdown-toggle.btn {
            height: 35px;
            overflow: hidden;
        }
        /* .highcharts-container {
            height:800px !important;
            overflow:scroll !important;
        }*/
        .treeChartLbl {
            text-align: center;
            color: white;
            background-color: #0c0cb9;
            font-size: 20px;
            padding: 5px;
            /* border-radius: 4px;*/
        }

      .highcharts-container {
            border-radius: 10px;
        }
       #treeChartContainer .highcharts-container{
           /* overflow:auto !important;*/
       }
      .treeCss{
           overflow:auto !important;
      }
      #divDescCodeTreeChart{
          overflow:auto !important;
          width:100%;
          margin:20px auto;
      }
        .submenus-style a {
            background-color: #393e46;
            color: white;
            padding: 20px 30px !important;
            font-size: 20px;
        }

        .submenus-style > li:hover {
            background-color: #393e46;
        }

            .submenus-style > li:hover a {
                background-color: #393e46;
                color: white;
            }

        .submenus-style li.active a {
            background-color: #158eb3;
            color: white;
        }
    </style>

    <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <div>
        <asp:UpdatePanel ID="update" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnHideExport" />
            </Triggers>
            <ContentTemplate>


                <asp:HiddenField ID="hdfValue" runat="server" />
                <asp:Button ID="btnHideExport" runat="server" OnClick="btnHideExport_Click" Style="display: none" />
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
                <table class="table table-bordered" id="tblfilter">

                    <tr>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                        <td class="input-group" style="width: 200px">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date cal" data-toggle="tooltip"
                                title="From Date !" placeholder="From Date" ToolTip="<%$Resources:CommanResource, FromDate %>"></asp:TextBox>
                        </td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                        <td class="input-group" style="width: 200px">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date cal" data-toggle="tooltip"
                                title="To Date !" placeholder="To Date" ToolTip="<%$Resources:CommanResource, ToDate %>"></asp:TextBox>
                        </td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","PlantID")%></b></td>
                        <td>
                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Plant ID !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" ToolTip="<%$Resources:CommanResource, PlantTooltip %>">
                            </asp:DropDownList></td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                        <td>
                            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control drp" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList></td>
                        <td colspan="4" style="text-align: left"></td>
                    </tr>

                    <tr>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","MachineId")%></b></td>
                        <td style="text-align: left; width: 130px;">
                            <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control drp" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged"></asp:DropDownList></td>

                        <td class="commontd">
                            <b>Component ID</b>
                        </td>
                        <td class="commontd">
                            <asp:ListBox ID="lbComponentID" Style="z-index: 100" runat="server" SelectionMode="Multiple" CssClass="form-control drp" ToolTip="<%$Resources:CommanResource, ALL %>" OnSelectedIndexChanged="lbComponentID_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                            <%--  <asp:DropDownList ID="ddlComponentID" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="ComponentID !" AutoPostBack="true" OnSelectedIndexChanged="ddlComponentID_SelectedIndexChanged" />--%>
                        </td>
                        <td class="commontd">
                            <b>Operation</b>
                        </td>
                        <td class="commontd">
                            <asp:DropDownList ID="ddlOperation" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Operation !" />
                        </td>

                        <td class="commontd">
                            <b>Show Top Reasons </b></td>
                        <td>
                            <asp:DropDownList ID="ddlTopDownReasons" runat="server" CssClass="select form-control">
                                <asp:ListItem Value="5">Top 5</asp:ListItem>
                                <asp:ListItem Value="10">Top 10</asp:ListItem>
                                <asp:ListItem Value="15">Top 15</asp:ListItem>
                                <asp:ListItem Value="20" Selected="True">Top 20</asp:ListItem>
                                <asp:ListItem Value="25">Top 25</asp:ListItem>
                                <asp:ListItem Value="30">Top 30</asp:ListItem>
                                <asp:ListItem Value="35">Top 35</asp:ListItem>
                                <asp:ListItem Value="40">Top 40</asp:ListItem>
                                <asp:ListItem Value="45">Top 45</asp:ListItem>
                                <asp:ListItem Value="50">Top 50</asp:ListItem>
                            </asp:DropDownList></td>

                        <td colspan="4" style="text-align: left"></td>
                    </tr>

                    <tr>
                        <td class="commontd">
                            <b>Category</b>
                        </td>
                        <td class="commontd" style="width: 200px">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Category !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        <td class="commontd">
                            <b>Sub Category</b>
                        </td>
                        <td class="commontd" style="width: 200px">
                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Category !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        <td class="commontd">
                            <b>Sub Sub Category</b>
                        </td>
                        <td class="commontd" style="width: 200px">
                            <asp:DropDownList ID="ddlDesCategory" runat="server" CssClass="form-control drp" data-toggle="tooltip" title="Category !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlDesCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="commontd">
                            <b>Rejection ID  </b>
                        </td>
                        <td class="commontd" style="text-align: left; width: 200px;">
                            <asp:ListBox ID="ddlMultiDownID" Style="z-index: 100" runat="server" SelectionMode="Multiple" CssClass="form-control drp" ToolTip="<%$Resources:CommanResource, ALL %>"></asp:ListBox>
                        </td>

                        <td colspan="4" style="text-align: left">
                            <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" class="btn btn-info btn-md" ID="btnProcess" OnClick="btnProcess_Click"></asp:Button>
                        </td>
                    </tr>

                </table>
                </div>
            
           
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="print" style="margin: 3px;width:100%;display:inline-block">
            <div class="navbar-collapse collapse" style="height: 42px !important; padding-left: 0px; display: inline-block !important; width: 40%;">
                <ul class="nav navbar-nav nextPrevious submenus-style" id="chartsTab">
                    <li id="liParetoChartMenu"><a runat="server" class="submenuData" clientidmode="static" data-toggle="tab" href="#paretoChartMenu">Pareto Chart</a>
                        <i></i>
                    </li>
                    <li><a runat="server" class="submenuData" clientidmode="static" data-toggle="tab" href="#treeChartMenu">Tree Chart</a>
                        <i></i>
                    </li>
                </ul>
            </div>
            <div id="exportContainer" style="width: 58%; display: inline-block; vertical-align: top; margin-top: 4px; text-align: right">
                <button class="btn btn-info btn-sm glyphicon glyphicon-print" onclick="window.print();return false;"></button>
                &nbsp;
        <button class="btn btn-info btn-sm glyphicon glyphicon-export" id="btnExport"></button>
            </div>

        </div>

        <%--  <div>
        <div id="container-col" style="min-height: 600px; max-height: 100%; width:50%;float:left;margin-top:30px;padding:10px"></div>
        <div id="container-col1" style="min-height: 600px; max-height:100%; width:50%;float:right;padding:10px"></div>
    </div>--%>

        <div id="container-col" style="width: 100%;">
            <div class="tab-content themetoggle" id="processParamContainer" style="overflow: auto; width: 100%; margin: -10px auto; margin-top: 5px">
                <div id="paretoChartMenu" class="tab-pane fade">
                    <div id="container-col2" style="min-height: 600px; padding: 10px"></div>
                    <br />
                    <div id="container-col1" style="min-height: 600px; padding: 10px"></div>
                </div>
                <div id="treeChartMenu" class="tab-pane fade">
            <%--        <div style="border: 3px solid #1111ad; border-radius: 5px;">
                        <div class="treeChartLbl">Globe Analysis Tree Chart</div>
                        <div style="padding: 10px; padding-top: 0px">--%>
                            <div id="treeChartContainer" style="padding: 10px;"></div>
                     <%--   </div>
                    </div>--%>
                </div>
            </div>

        </div>


        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <a data-dismiss="modal" class="glyphicon glyphicon-remove" style="float: right; z-index: 5; color: black; font-size: 30px"></a>
                        <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                    </div>
                    <div class="modal-body">
                        <div id="divDescriptionChart" style="width: 548px"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="treeModal" role="dialog" style="">
            <div class="modal-dialog modal-lg" style="width:90%">
                <div class="modal-content" style="background-color:white">
                    <div class="modal-header" style="height: 40px">
                        <button type="button" class="close" data-dismiss="modal" style="float: right; z-index: 5; color: black; font-size: 2rem">&times;</button>
                    </div>
                    <div class="modal-body" style="margin:20px auto">

                        <div id="divDescCodeTreeChart"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Warning!</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="lblWarningMsg" style="color: black"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="loaderModal" role="dialog" style="min-width: 300px; margin-top: 19%">
        <div class="modal-dialog  modal-dialog-centered" style="width: 400px">
            <div class="modal-content" style="background-color: white">
                <div class="modal-body" style="text-align: center">
                    <img src="/img/loadIcon/ajax-loader.gif" />
                </div>
            </div>
        </div>
    </div>
    <script>
        var countDrillDown = 0;
        $(document).ready(function () {
            //$('[id$=ddlMachineId]').multiselect({
            //    includeSelectAllOption: true
            //});
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbComponentID]').multiselect({
                includeSelectAllOption: true
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
           
            BindGraph();
            debugger;
            $('#liParetoChartMenu').addClass("active");
            $('#paretoChartMenu').addClass("active in");
            $('#exportContainer').css("visibility", "visible");
            // $('#loaderModal').modal('show');
        });
        $('.submenuData').click(function () {
            debugger;
            if ($(this).attr('href') == "#paretoChartMenu") {
                $('#exportContainer').css("visibility", "visible");
            }
            else {
                $('#exportContainer').css("visibility", "hidden");
            }
        });
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        function compareDates(date1, date2) {
            return invert(date1).localeCompare(invert(date2));
        }

        $(document).on("click", "[id$=btnProcess]", function () {
            if ($("[id$=txtFromDate]").val() == "") {
                openWarningModal("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                openWarningModal("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            var dateCom = compareDates(from, to);
            if (dateCom == 1) {
                openWarningModal("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var message = filterValidation();
            if (message != "") {
                openWarningModal(message);
                return;
            }
            $.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif" />' });
            GetRejectionParetoData();
            GetRejectionDrillDownData();
            BindTreeGraph();
        });
        function BindTreeGraph() {
            debugger;
            //var obj = { plantID: $("[id$=ddlPlantId]").val(), Groupid: $("[id$=ddlGroup]").val(), machineId: $("[id$=ddlMachineId]").val(), startFromDate: $("[id$=txtFromDate]").val(), startToDate: $("[id$=txtToDate]").val(), ComponentID: $("[id$=lbComponentID]").val(), Operation: $("[id$=ddlOperation]").val(), Category: $("[id$=ddlCategory]").val(), SubCategory: $("[id$=ddlSubCategory]").val(), DesCategory: $("[id$=ddlDesCategory]").val(), downReasonsstring: $("[id$=ddlTopDownReasons]").val(), RejCode: $("[id$=ddlMultiDownID]").val() };
            //var opInputData = JSON.stringify(obj);
            var message = filterValidation();
            if (message != "") {
                openWarningModal(message);
                return;
            }
            $.ajax({
                type: "POST",
                url: "Rejection_Globe.aspx/getTreeData",
                contentType: "application/json; charset=utf-8",
                //  data: opInputData,
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=lbComponentID]").val() + '",Operation:"' + getOperation() + '",Category:"' + $("[id$=ddlCategory]").val() + '",SubCategory:"' + $("[id$=ddlSubCategory]").val() + '",DesCategory:"' + $("[id$=ddlDesCategory]").val() + '",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemdata = response.d;
                    if (itemdata != null) {
                        var appendStr = "";
                        $('#treeChartContainer').empty();
                        for (var i = 0; i < itemdata.length; i++) {
                            appendStr += '<div id="treeChartContainer' + i + '" style="margin-top:5px;" class="treeCss"></div>';
                        }
                        $('#treeChartContainer').append(appendStr);
                        for (var i = 0; i < itemdata.length; i++) {
                            BindOrganizationTreeChart(itemdata[i], 'treeChartContainer' + i);
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function getOperation() {
            var opn = $("[id$=ddlOperation]").val();
            if (opn != "" && opn != null) {
                if (opn == "All") {
                    opn = "";
                    for (var i = 1; i < $("[id$=ddlOperation] option").length; i++) {
                        if (opn == "") {
                            opn += $("[id$=ddlOperation] option")[i].value;
                        }
                        else {
                            opn += "," + $("[id$=ddlOperation] option")[i].value;
                        }
                    }
                }
            }
            return opn;
        }
        function BindOrganizationTreeChart(itemdata, containerid) {
            var chartHeight = 200;
            if (itemdata.Hieght != null && itemdata.Hieght != "") {
                chartHeight = parseInt(itemdata.Hieght) + 2;
                chartHeight = (chartHeight * 60) + 80;

            }
            else {
                chartHeight = null;
            }
            console.log(containerid + " H =" + chartHeight);
            var chartWidth = null;
            var minwidth = 200;
            if (itemdata.Width != null && itemdata.Width != "") {
                chartWidth = parseInt(itemdata.Width) * (minwidth + 50);
                if (chartWidth < ($(window).width() - 200)) {
                    chartWidth = null;
                }
            }
            Highcharts.chart(containerid, {
                chart: {
                    renderTo: containerid,
                    height: chartHeight,
                    width: chartWidth,
                    inverted: true,
                    //events: {
                    //    load() {
                    //        let chart = this,
                    //            series = chart.series[0],
                    //        /* newWidth = series.options.nodeWidth * series.nodeColumns[2].length;*/
                    //            newWidth = series.options.nodeWidth * 20;

                    //        chart.update({
                    //            chart: {
                    //                width: newWidth
                    //            }
                    //        })
                    //    }
                    //}
                },

                title: {
                    text: itemdata.title + " Analysis",
                    useHTML: true,
                    style: {
                        fontWeight: 'bold',
                        textDecoration: "underline",
                    }
                },
                xAxis: {
                    scrollbar: {
                        enabled: true
                    },
                    tickLength: 0
                },
                accessibility: {
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    debugger;
                                    if (this.level == 2 && this.Categoty != null && this.Component != null) {
                                        let catagory = this.Categoty;
                                        let component = this.Component;
                                        showDescriptionCodeTreeChart(catagory, component, this.name, this.title);
                                    }
                                }
                            }
                        }
                    }
                },

                series: [{
                    name: "",
                    type: "organization",
                    keys: ['from', 'to'],
                    minLinkWidth: minwidth,
                    data: itemdata.data,
                    levels: itemdata.levels,
                    nodes: itemdata.nodes,
                    colorByPoint: false,
                   // nodeWidth: 100,
                    nodeHeight: 60,
                    nodePadding: 10,
                    dataLabels: {
                        allowOverlap: false,
                    }
                }],
                credits: {
                    enabled: false
                },
                tooltip: {
                    outside: true
                }


            });
        }
        function showDescriptionCodeTreeChart(category, component, subcategory, SubCatQty) {
            var obj = { Category: category, Component: component, SubCategory: subcategory, SubCategoryQty: SubCatQty };
            var opInputData = JSON.stringify(obj);
            $.ajax({
                type: "POST",
                url: "Rejection_Globe.aspx/getDescCodeTreeData",
                contentType: "application/json; charset=utf-8",
                data: opInputData,
                dataType: "json",
                success: function (response) {
                    var itemdata = response.d;
                    if (itemdata != null) {
                        for (var i = 0; i < itemdata.length; i++) {
                            BindOrganizationTreeChart(itemdata[i], 'divDescCodeTreeChart');
                            break;
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
            $("#treeModal").modal('show');
        }
        function filterValidation() {
            debugger;
            var message = "";
            if ($("[id$=txtFromDate]").val() == "" || $("[id$=txtFromDate]").val() == null) {
                message = "Please Select From Date.";
                return message;
            }
            if ($("[id$=txtToDate]").val() == "" || $("[id$=txtToDate]").val() == null) {
                message = "Please Select To Date.";
                return message;
            }
            if ($("[id$=ddlPlantId]").val() == "" || $("[id$=ddlPlantId]").val() == null) {
                message = "Please Select Plant ID.";
                return message;
            }
            if ($("[id$=ddlGroup]").val() == "" || $("[id$=ddlGroup]").val() == null) {
                message = "Please Select Group ID.";
                return message;
            }
            if ($("[id$=ddlMachineId]").val() == "" || $("[id$=ddlMachineId]").val() == null) {
                message = "Please Select Machine ID.";
                return message;
            }
            if ($("[id$=ComponentID]").val() == "" || $("[id$=ComponentID]").val() == null) {
                message = "Please Select Component ID.";
                return message;
            }
            if ($("[id$=ddlOperation]").val() == "" || $("[id$=ddlOperation]").val() == null) {
                message = "Please Select Operation.";
                return message;
            }
            if ($("[id$=ddlCategory]").val() == "" || $("[id$=ddlCategory]").val() == null) {
                message = "Please Select Category.";
                return message;
            }
            if ($("[id$=ddlSubCategory]").val() == "" || $("[id$=ddlSubCategory]").val() == null) {
                message = "Please Select Sub Category.";
                return message;
            }
            if ($("[id$=ddlDesCategory]").val() == "" || $("[id$=ddlDesCategory]").val() == null) {
                message = "Please Select Sub Sub Category.";
                return message;
            }
            if ($("[id$=ddlMultiDownID]").val() == "" || $("[id$=ddlMultiDownID]").val() == null) {
                message = "Please Select Rejection ID.";
                return message;
            }
            return message;
        }
        $(document).on("click", "#liMenu", function () {
            debugger;
            var chart6 = $('#container-col').highcharts();
            chart6.reflow();

        });

        function BindGraph() {
            if ($("[id$=hdfValue]").val() == "OK") {
                debugger;
                var message = filterValidation();
                debugger;
                if (message != "") {
                    openWarningModal(message);
                    return;
                }
                GetRejectionParetoData();
                GetRejectionDrillDownData();
                BindTreeGraph();
            }
        }

        function GetRejectionDrillDownData() {
            $.ajax({
                type: "POST",
                url: "Rejection_Globe.aspx/RejectionDrillDownInfo",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=lbComponentID]").val() + '",Operation:"' + getOperation() + '",Category:"' + $("[id$=ddlCategory]").val() + '",SubCategory:"' + $("[id$=ddlSubCategory]").val() + '",DesCategory:"' + $("[id$=ddlDesCategory]").val() + '",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '"}',
                dataType: "json",
                success: OnSuccessDrillDownData,
                //success:OnSuccessLoadDrillDownData,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }
        var DDchart = null;

        function OnSuccessDrillDownData(Result) {

            var data = Result.d;
            console.log("chart result" + Result);
            var chart = Highcharts.chart('container-col1', {
                chart: {
                    type: 'column',
                    credits: {
                        enabled: false
                    },
                    height: 600,
                    events: {
                        drillup: function (e) {

                            //chart.setTitle({ text: e.point.beforeTitle });
                            if (e.seriesOptions.id == undefined) {
                                xlabel = "";
                                countDrillDown = 0;
                                chart.setTitle({ text: data.XAxisTitle });
                                //chart.title.attr({
                                //    text: data.XAxisTitle
                                //});
                            } else {
                                chart.setTitle({ text: e.seriesOptions.data[0].beforeTitle });
                                //chart.title.attr({
                                //    text: e.seriesOptions.data[0].beforeTitle
                                //});
                            }
                        },
                        drilldown: function (e) {

                            chart.setTitle({ text: e.point.Title });
                            //chart.XAxisTitle = e.point.afterTitle;
                            //chart.title.text = e.point.afterTitle;
                            //countDrillDown++;
                            //chart.title.attr({
                            //    text: e.point.afterTitel
                            //});
                        },

                    }
                },
                title: {
                    text: 'Rejection Chart',
                    useHTML: true,
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Rejection Qty'
                    }

                },
                legend: {
                    enabled: false
                },
                tooltip: {
                    headerFormat: '<span style="font-size:15px"><b>{series.name}</b></span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b><br/>'
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y}'
                            //format: '{point.y:.1f}'
                        },
                        point: {
                            events: {
                                click: function () {

                                    var str = this.afterTitle;
                                    chart.setTitle({ str });
                                    if (this.drilldown == "") {
                                        var color = this.color;
                                        var category = str.split("*-*");
                                        BindDrillDownChart("ByRejcode", category[0], category[1], category[2], color);
                                    }
                                }
                            }
                        }

                    }

                },

                series: data.series,
                drilldown: {
                    series: data.drilldown,
                }

            });
            // BindColumnAndPieChart(data);
            $.unblockUI({});
        }


        function BindDrillDownChart(param, category, Subcategory, Descategory, color) {

            $.ajax({
                type: "POST",
                url: "Rejection_Globe.aspx/GetCategoryDrillDownChart",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=lbComponentID]").val() + '",Operation:"' + getOperation() + '",Category:"' + category + '",SubCategory:"' + Subcategory + '",DesCategory:"' + Descategory + '",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '",Param:"' + param + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    OnSuccessLoadDrillDownData(itmData, color);
                },
                //success: OnSuccessLoadDrillDownData(result,color),
                //success:abc,
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function OnSuccessLoadDrillDownData(data, color) {
            //var data = Result.d;

            Highcharts.chart('divDescriptionChart', {
                colors: [color],

                title: {
                    text: data.series[0].name,
                    useHTML: true,

                    style: {
                        color: '#000000',
                        //color: '#FFFFFF',
                        fontSize: '15px',
                        fontFamily: 'Verdana, sans-serif',
                        //'background-color': '#000000',
                        fontWeight: 'bold',
                    }
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Rejection Qty',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                //xAxis: {
                //    title: {
                //    },
                //    type: 'category'
                //},
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0);
                            }
                        },
                        point: {
                            events: {
                                click: function () {
                                    var index = this.category;
                                    if (this.drilldown != null) {
                                        var str = this.drilldown;
                                        var res = str.split("/");
                                    }
                                }
                            }
                        }
                    }
                },
                series: data.series
            });
            $("#myModal").modal('show');
            //$.unblockUI({});
        }

        function GetRejectionParetoData() {

            var abc = JSON.stringify('{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=lbComponentID]").val() + '",Operation:"' + getOperation() + '",Category:"' + $("[id$=ddlCategory]").val() + '",SubCategory:"' + $("[id$=ddlSubCategory]").val() + '",RejDescription:"' + $("[id$=ddlDesCategory]").val() + '",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '"}');
            console.log(abc);
            $.ajax({
                type: "POST",
                url: "Rejection_Globe.aspx/RejectionChartInfo",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",Groupid:"' + $("[id$=ddlGroup]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",ComponentID:"' + $("[id$=lbComponentID]").val() + '",Operation:"' + getOperation() + '",Category:"' + $("[id$=ddlCategory]").val() + '",SubCategory:"' + $("[id$=ddlSubCategory]").val() + '",RejDescription:"' + $("[id$=ddlDesCategory]").val() + '",downReasonsstring:"' + $("[id$=ddlTopDownReasons]").val() + '",RejCode:"' + $("[id$=ddlMultiDownID]").val() + '"}',
                dataType: "json",
                success: OnSuccessDownTimePareto,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function OnSuccessDownTimePareto(Result) {
            var data = Result.d;
            BindColumnAndPieChart(data);
            $.unblockUI({});
        }

        function BindColumnAndPieChart(data) {
            // 

            var splite = "Rejection Code";

            var catValue = '';
            var cont = 0;
            var increas = 0;
            var charts = '';
            var drilldown = function (e, point, type) {
                if (type == "col") {
                    var charts = [$('#container-col2').highcharts()];//Highcharts.charts;
                    pointName = e.point.name;
                    point.options.chart.drilled = true;
                    Highcharts.each(charts, function (c, i) {
                        if (!c.options.chart.drilled) {
                            c.options.chart.drilled = true;
                            Highcharts.each(c.series[0].data, function (p, j) {
                                if (p.name === pointName) {
                                    p.doDrilldown();
                                }
                            });
                        }
                    });
                }
            };

            var chartCol = Highcharts.chart('container-col2', {
                colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce',
                    '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a', '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4',
                    '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                    '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: true },
                chart: {
                    type: 'column',
                    drilled: false,
                    events: {
                        drilldown: function (e) {
                            drilldown(e, this, "col");
                            cont = $("[id$=ddlTopDownReasons]").val();
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(cont);
                            chartCol.xAxis[0].setCategories(catValue);
                        },
                        drillup: function (e) {
                            //chartCol.series[0].show(true);

                            cont = $("[id$=ddlTopDownReasons]").val();
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(0, cont);
                            chartCol.xAxis[0].setCategories(catValue);
                            chartCol.series[1].setData(data.series);
                        }
                    }
                },
                title: {
                    text: ''
                }, xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: '12px'
                        }
                    },
                    //type: 'category',
                }, tooltip: {
                    enabled: true,
                    //format: '{point.y:.1f}',
                    formatter: function () {
                        var value = 0;
                        if (this.series.name == "Pareto") {
                            value = (this.y).toFixed(2);
                            return '<b>' + this.series.name + '</b><br/>' +
                                "Cumulative Value (%)" + ': ' + value;
                        }
                        else {
                            value = (this.y);
                            return '<b>Rejection Quantity: ' + value;
                        }

                    },
                },
                yAxis: [{
                    title: {
                        text: 'Rejection Quantity',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    //labels: {
                    //    format: "{value}%"
                    //}                   
                }, {
                    title: {
                        text: 'Cum %',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
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
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                var value = '';
                                if (this.series.name != 'Pareto') {
                                    var value = this.y;
                                }
                                return value;
                            },
                            //format: '{point.y:.1f}',                            
                        }, style: {
                            fontSize: '12px'
                        }
                    }
                },
                series: [{
                    type: 'pareto',//pareto
                    name: 'Pareto',
                    yAxis: 1,
                    zIndex: 10,
                    baseSeries: 1

                }, {
                    name: splite,
                    type: 'column',
                    colorByPoint: true,
                    zIndex: 2,
                    data: data.series,
                }],
                drilldown: {
                    series: data.drilldown,
                }
            });
        }

        $(document).on("click", "#btnExport", function () {
            if ($("[id$=txtFromDate]").val() == "") {
                openWarningModal("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                openWarningModal("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            var dateCom = compareDates(from, to);
            if (dateCom == 1) {
                openWarningModal("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            $("[id$=btnHideExport]").trigger("click");
            return false;
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // $.unblockUI({});
            //$('[id$=ddlMachineId]').multiselect({
            //    includeSelectAllOption: true
            //});
            $('[id$=lbComponentID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('.submenuData').click(function () {
                debugger;
                if ($(this).attr('href') == "#paretoChartMenu") {
                    $('#exportContainer').css("visibility", "visible");
                }
                else {
                    $('#exportContainer').css("visibility", "hidden");
                }
            });
            //BindGraph();
        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
