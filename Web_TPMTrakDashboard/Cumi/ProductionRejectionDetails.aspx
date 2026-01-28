<%@ Page Title="Production Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ProductionRejectionDetails.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.ProductionRejectionDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/pareto.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>

    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>

    <style>
        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .radio-btn-list tr td {
            padding: 0px 10px;
        }

            .radio-btn-list tr td label {
                color: white;
                margin-left: 2px;
            }

        #AggLiveSelectionPanel .radio-btn-list tr td label {
            color: black;
        }

        .selected-menu-style {
            background-color: #0e94cb !important;
            /* border: 2px solid #0e94cb !important;*/
            color: white !important;
            font-weight: bold;
        }

            .selected-menu-style a {
                color: #555;
                font-weight: bold;
            }

        .submenuData {
            padding: 0px 20px;
            color: #555;
            background-color: white;
            /*border: 2px solid white;*/
            border-right: 1px solid #e1e1e1
        }

        .cumi-tbl-report-details .inner-tbl, .value-content-div {
            width: 100%;
        }

        /* .cumi-tbl-report-details .value-content-tbl tr td, .cumi-tbl-report-details .value-content-tbl tr th, .cumi-tbl-report-details .value-header-tbl tr td, .cumi-tbl-report-details .value-header-tbl tr th, .value-content-div*/
        .cumi-tbl-report-details .value-tbl tr td, .cumi-tbl-report-details .value-tbl tr th {
            min-width: 150px;
            width: 150px;
            max-width: 150px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .cumi-tbl-report-details .plan-actual, .cumi-tbl-report-details .machine {
            min-width: 150px;
            max-width: 150px;
            width: 150px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .cumi-tbl-report-details tr td {
            background-color: white;
        }

        .cumi-tbl-report-details > tbody > tr:first-child {
            position: sticky;
            top: 0px;
            background-color: white;
            z-index: 4;
        }

        .cumi-tbl-report-details tbody > tr > th, .cumi-tbl-report-details tbody > tr > td {
            border: 1px solid #ddd;
        }

        fieldset {
            border: 1px groove #d5d5d5 !important;
            /*     padding: 0.1em 0.5em 1em !important;*/
            margin: 0 0 1.5em 1em !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 2px 2px 6px 3px #060606;
            /*font-weight: bold;*/
            height: 93px;
            padding: 0px 8px;
            width: fit-content;
            background-color: #222e70;
            /*border-radius: 2px;*/
        }

        legend {
            font-size: 1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
            margin-top: -4px;
            color: white;
            margin-bottom: 0px;
        }

        .cumi-filter-tbl tr td {
            color: white;
            border: unset;
            padding: 0px 10px;
        }

        .filter-panel {
            padding: 10px;
            width: 30%;
            z-index: 8;
            background-color: #efefef;
            box-shadow: 2px 2px 4px 2px #606060;
            height: 100vh;
            visibility: hidden;
        }

        .show-filter-panel {
            position: absolute;
            top: 70px;
            visibility: visible;
        }

        .row-bakc-color td {
            background-color: antiquewhite !important;
        }

        .chart-icon {
        }

        .value-content-tbl tr td {
            border-bottom: unset !important;
        }

        .machine-column-header {
            position: sticky;
            left: 0px;
            z-index: 6 !important;
            background-color: #2e6886;
        }

        .machine-column {
            position: sticky;
            left: 0px;
            z-index: 5 !important;
        }

        .switch {
            position: relative;
            display: inline-block;
            vertical-align: middle;
            width: 50px;
            height: 30px;
            /*float: right;*/
            margin: 5px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 22px;
                width: 22px;
                left: 3px;
                bottom: 3px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(23px);
            -ms-transform: translateX(23px);
            transform: translateX(23px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 30px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .highcharts-data-table table {
            border-collapse: collapse;
            border-spacing: 0;
            background: white;
            min-width: 100%;
            margin-top: 10px;
            font-family: sans-serif;
            font-size: 0.9em;
        }

        .highcharts-data-table td, .highcharts-data-table th, .highcharts-data-table caption {
            border: 1px solid silver;
            padding: 0.5em;
        }

        .highcharts-data-table caption {
            background-color: #efefef;
            color: black;
        }

        /* .highcharts-data-table tr:nth-child(even), .highcharts-data-table thead tr {
            background: #f8f8f8;
        }*/

        .highcharts-data-table tr:hover {
            background: #eff;
        }

        .highcharts-data-table caption {
            border-bottom: none;
            font-size: 1.1em;
            font-weight: bold;
        }
    </style>
    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="bajaj-outer-div-filter-section">
                    <div class="bajaj-inner-div-filter-section left-content-filter-section">
                        <div>
                            <div class="navbar-collapse collapse" style="margin-bottom: 10px; padding-left: 0px">
                                <ul id="masterul" class="nav navbar-nav " style="margin-right: 20px">
                                    <li><a runat="server" class="submenuData" id="A15" clientidmode="static" data-toggle="tab" href="HourlyDataMenu">Hourly Data</a>
                                        <i></i>
                                    </li>
                                    <li><a runat="server" class="submenuData" id="A14" clientidmode="static" data-toggle="tab" href="ShiftDataMenu">Shift Data</a>
                                        <i></i>
                                    </li>
                                    <li><a runat="server" class="submenuData" id="A1" clientidmode="static" data-toggle="tab" href="DayDataMenu">Day Data</a>
                                        <i></i>
                                    </li>
                                    <li><a runat="server" class="submenuData" id="A2" clientidmode="static" data-toggle="tab" href="WeekDataMenu">Week Data</a>
                                        <i></i>
                                    </li>
                                    <li><a runat="server" class="submenuData" id="A3" clientidmode="static" data-toggle="tab" href="MonthDataMenu">Month Data</a>
                                        <i></i>
                                    </li>
                                </ul>
                                <asp:Button runat="server" ID="btnMenu" OnClick="btnMenu_Click" Visible="false" />
                                <asp:HiddenField runat="server" ID="hfSelectedMenu" />
                                <div>
                                    <asp:RadioButtonList runat="server" ID="rblViewType" RepeatDirection="Horizontal" CssClass="radio-btn-list" OnSelectedIndexChanged="rblViewType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Dashboard View" Value="Dashboard" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Report View" Value="Report"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>

                            </div>

                        </div>

                        <fieldset>
                            <legend>Filters</legend>
                            <table class="cumi-filter-tbl">
                                <tr>
                                    <%--  <td>Plant</td>--%>
                                    <td>
                                        <span class="filter-lbl-name">Plant</span>
                                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <%--<td>Machine</td>--%>
                                    <td>
                                        <span class="filter-lbl-name">Machine</span><br />
                                        <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <%--<td id="tdYear" runat="server">Year</td>--%>
                                    <td id="tdYearControl" runat="server">
                                        <span class="filter-lbl-name">Year</span>
                                        <asp:TextBox runat="server" ID="txtYear" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--<td id="tdMonth" runat="server">Month</td>--%>
                                    <td id="tdMonthControl" runat="server">
                                        <span class="filter-lbl-name">Month</span>
                                        <asp:TextBox runat="server" ID="txtMonth" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%-- <td id="tdFromDate" runat="server">
                                    <span runat="server" id="spanDate">From Date</span>

                                </td>--%>
                                    <td id="tdFromDateControl" runat="server">
                                        <span runat="server" id="spanDate" class="filter-lbl-name">From Date</span>
                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control from-date" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--<td id="tdToDate" runat="server">To Date </td>--%>
                                    <td id="tdToDateControl" runat="server">
                                        <span class="filter-lbl-name">To Date</span>
                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                    </td>

                                    <%--<td id="tdShift" runat="server">Shift</td>--%>
                                    <td id="tdShiftControl" runat="server">
                                        <span class="filter-lbl-name">Shift</span>
                                        <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control"></asp:DropDownList>
                                    </td>
                                    <td id="tdAggLiveControl" runat="server">
                                        <br />
                                        <i class="glyphicon glyphicon-filter" onclick="AggLiveClick(event);"></i>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:Button runat="server" ID="btnView" OnClientClick="return CallLoader();" OnClick="btnView_Click" CssClass="bajaj-btn-style" Text="View" />
                                        <asp:Button runat="server" ID="btnExport" CssClass="bajaj-btn-style" Text="Export" OnClick="btnExport_Click" />
                                    </td>
                                    <td runat="server" id="tdAutorefresh" style="position: relative; top: 10px;">
                                        <span>Auto Refresh</span>
                                        <label class="switch">
                                            <asp:CheckBox runat="server" ID="cbAutorefresh" AutoPostBack="true" OnCheckedChanged="cbAutorefresh_CheckedChanged" />
                                            <span class="slider round"></span>
                                        </label>
                                        <asp:Timer runat="server" ID="timer" OnTick="timer_Tick"></asp:Timer>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>
                <div id="scrollMaintainDiv" style="max-height: 73vh; overflow: auto; margin-top: 12px">
                    <asp:ListView runat="server" ID="lvDetails" ClientIDMode="Static">
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style cumi-tbl-report-details" id="tblFirstProductReportDetails">
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class=' <%# "tr-for-closest " + Eval("RowBackColor") %>'>
                                <asp:HiddenField runat="server" ID="hfMachineID" Value='<%# Eval("MachineID") %>' />
                                <asp:HiddenField runat="server" ID="hfDate" Value='<%# Eval("Date") %>' />
                                <asp:HiddenField runat="server" ID="hfShift" Value='<%# Eval("Shift") %>' />
                                <th class="machine" style="display: <%# Eval("DateHeaderVisibility") %>">Date
                                </th>
                                <th class="machine" style="display: <%# Eval("ShiftHeaderVisibility") %>">Shift
                                </th>
                                <th class="machine machine-column-header" style="display: <%# Eval("HeaderVisibility") %>">Machine ID
                                </th>
                                <th class="plan-actual" style="display: <%# Eval("PlanActualHeaderVisibility") %>"></th>

                                <td class="machine" rowspan=' <%# Eval("DateRowSpan") %>' style="display: <%# Eval("DateConentVisibility") %>">
                                    <%# Eval("Date") %>
                                </td>
                                <td class="machine" rowspan=' <%# Eval("ShiftRowSpan") %>' style="display: <%# Eval("ShiftConentVisibility") %>">
                                    <%# Eval("Shift") %>
                                </td>
                                <td class="machine machine-column" style="display: <%# Eval("ContentVisibility") %>"><%# Eval("MachineID") %></td>
                                <td style="padding: 0; border: unset; display: <%# Eval("PlanActualConentVisibility") %>" class="plan-actual">
                                    <table class="inner-tbl plan-acual-tbl">
                                        <tr>
                                            <td class="processno"><%# Eval("PlanName") %></td>

                                        </tr>
                                        <tr>
                                            <td class="processno"><%# Eval("ActualName") %></td>
                                        </tr>
                                        <tr style="display: <%# Eval("RowCompletionNameVisibility") %>">
                                            <td class="processno"><%# Eval("RowCompletionName") %></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="padding: 0; border-top: unset; border-right: unset; border-left: unset">
                                    <asp:ListView runat="server" ID="lvShiftDetails" DataSource='<%# Eval("DynamicColumnDetails") %>'>
                                        <LayoutTemplate>
                                            <table class="inner-tbl value-tbl">
                                                <tr>
                                                    <td id="itemplaceholder" runat="server"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td style="padding: 0; border: unset; vertical-align: inherit">

                                                <table class="inner-tbl value-header-tbl" style="display: <%# Eval("DynamicHeaderVisibility") %>">
                                                    <tr>
                                                        <th>
                                                            <%# Eval("HeaderName") %>
                                                            <i class="glyphicon glyphicon-signal chart-icon" style='display: <%# Eval("HeaderChartVisibility") %>; float: right;' onclick="BindChart(this,'HeaderChart');"></i>
                                                            <asp:HiddenField runat="server" ID="hfHeaderID" Value='<%# Eval("HeaderID") %>' />
                                                        </th>

                                                    </tr>
                                                </table>

                                                <table class="inner-tbl value-content-tbl" style="display: <%# Eval("DynamicTwoRowsVisibility") %>">
                                                    <tr>
                                                        <td style="color: <%# Eval("PlanContentColor")  %>;"><span title='<%# Eval("Plan") %>'><%# Eval("Plan") %></span></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: <%# Eval("ActualContentColor")  %>;"><span title='<%# Eval("Actual") %>'><%# Eval("Actual") %></span></td>
                                                    </tr>
                                                    <tr style="display: <%# Eval("RowCompletionVisibility") %>">
                                                        <td style="color: <%# Eval("RowCompletionContentColor")  %>;"><span title='<%# Eval("RowCompletion") %>'><%# Eval("RowCompletion") %></span></td>
                                                    </tr>
                                                </table>

                                                <div style="display: <%# Eval("DynamicMergedRowVisibility") %>; color: <%# Eval("CompletionContentColor")  %>" class="value-content-div">
                                                    <span title='<%# Eval("Completion") %>'><%# Eval("Completion") %></span>
                                                </div>

                                                <table class="inner-tbl value-content-tbl" style="display: <%# Eval("DynamicOneRowVisibility") %>">
                                                    <tr>
                                                        <td style="color: <%# Eval("ValueContentColor")  %>;"><span title='<%# Eval("Value") %>'><%# Eval("Value") %></span></td>
                                                    </tr>

                                                </table>
                                                <div style="display: <%# Eval("DynamicChartVisibility") %>; text-align: center" class="value-content-div">
                                                    <i class="glyphicon glyphicon-signal chart-icon" onclick="BindChart(this, 'ColumnChart');" style="display: <%# Eval("DynamicChartIconVisibility") %>;"></i>

                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

                <div id="chartDiv" runat="server" style="margin-top: 20px;">
                    <div id="chartConatiner"></div>
                </div>

                <div class="panel panel-default panel-subitems" id="AggLiveSelectionPanel" style="padding: 10px; width: 10%; z-index: 8; visibility: hidden; background-color: #efefef; box-shadow: 2px 2px 4px 2px #606060;">
                    <i class="glyphicon glyphicon-remove" style="float: right" onclick="HidePanels(this,'AggLiveSelectionPanel')"></i>
                    <div class="panel-body" style="padding-top: 10px; border-style: unset">
                        <div>
                            <asp:RadioButtonList runat="server" ID="rblAggLiveSSelection" CssClass="radio-btn-list">
                                <asp:ListItem Value="Default" Text="Default"></asp:ListItem>
                                <asp:ListItem Value="Aggregate" Text="Aggregate"></asp:ListItem>
                                <asp:ListItem Value="Live" Text="Live" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>

                <div class="modal infoModal bajaj-info-modal" id="ChartModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered" style="width: 50%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Chart</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="closeChartModal();"></i>
                                <asp:HiddenField runat="server" ID="hdnIsChartModalOpen" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="overflow: auto; max-height: 90vh">
                                <div style="text-align: center; color: black">
                                    <span id="chartHeader"></span>
                                </div>
                                <div style="height: 80vh">
                                    <div id="pieChartOuterConatiner">
                                        <div id="pieChartContainer" style="height: 100%">
                                        </div>
                                    </div>
                                    <div id="paretoChartOuterConatiner">
                                        <div id="paretoChartContainer" style="height: 100%; margin-top: 10px"></div>
                                    </div>

                                </div>
                            </div>
                            <div class="modal-footer">
                                <input type="button" value="Close" class="bajaj-btn-style" onclick="closeChartModal();" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnMenu" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script>
        $(document).ready(function () {
            //localStorage.removeItem("CumiSelectedMenu");
            ControlSetter();
            activeSubMenu();
        });
        function ControlSetter() {
            $('[id$=lbMachine]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
            });
            $('[id$=txtFromDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
                endDate: '1d'
            });
            $('[id$=txtToDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                endDate: '1d'
            });
        }

        function setActiveSubmenuValue() {
            localStorage.removeItem("CumiSelectedMenu");
            var lilist = $("#masterul li");
            for (let i = 0; i < lilist.length; i++) {
                let li = lilist[i];
                let display = $(li).css('display');
                if (display == "block") {
                    localStorage.setItem("CumiSelectedMenu", $(li).find('a').attr('href'));
                    activeSubMenu();
                    break;
                }
            }
        }
        function activeSubMenu() {
            if (localStorage.getItem("CumiSelectedMenu")) {
                if (localStorage.getItem("CumiSelectedMenu")) {
                    submenu = localStorage.getItem("CumiSelectedMenu");
                }
                $(submenu).addClass("in active");
                $("a[href$='" + submenu + "']").addClass("selected-menu-style");
            }
        }
        $(".submenuData").click(function () {
            $(".submenuData").removeClass("selected-menu-style").addClass("other-menu-style");
            $(".submenuData").closest('li').find('i').removeClass();
            $(this).removeClass("other-menu-style").addClass("selected-menu-style");
            submenu = $(this).attr('href');
            $("[id$=hfSelectedMenu]").val(submenu);
            __doPostBack('<%= btnMenu.UniqueID%>', "OnClick");
            localStorage.setItem("CumiSelectedMenu", submenu);
            CallLoader();
        });

        $(".from-date").on("click", function () {
            let element = $(this);
            element.datepicker('setStartDate', '');
            if ($("[id$=hfSelectedMenu]").val() == "HourlyDataMenu") {
                element.datepicker('setStartDate', '-7d');
            } else if ($("[id$=hfSelectedMenu]").val() == "ShiftDataMenu" && $("[id$=rblViewType] input:checked").val() != "Report") {
                if ($("[id$=rblAggLiveSSelection] input:checked").val() == "Live") {
                    element.datepicker('setStartDate', '-7d');
                }
            }
            //let element = $(this);
            //if ($("[id$=rblViewType] input:checked").val() == "Report") {
            //    element.datepicker('setStartDate', '-7d');
            //} else {
            //    element.datepicker('setStartDate', '');
            //}
        });
        function CallLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function CallUnLoader() {
            $.unblockUI({});
        }
        function AggLiveClick(event) {
            if (event.pageX == null && event.clientX != null) {
                var doc = document.documentElement, body = document.body;
                event.pageX = event.clientX + (doc && doc.scrollLeft || body && body.scrollLeft || 0) - (doc && doc.clientLeft || body && body.clientLeft || 0);
                event.pageY = event.clientY + (doc && doc.scrollTop || body && body.scrollTop || 0) - (doc && doc.clientTop || body && body.clientTop || 0);
            }
            $('#AggLiveSelectionPanel').css({
                'left': event.pageX - 50,
                'top': event.pageY + 20,
                'display': 'inline-block',
                "position": "absolute",
                "visibility": "visible"
            }).show();
            //$('#FilterPanel').css({
            //    //'left': event.pageX - 50,
            //    //'top': event.pageY + 20,
            //    'display': 'inline-block',
            //    "position": "absolute",
            //    "visibility": "visible"
            //}).show();
            //debugger;
            //$('#FilterPanel').addClass('show-filter-panel');
        }
        function HidePanels(element, panelid) {
            $('#' + panelid).css('visibility', 'hidden');
        }
        var ChartRowElement, ChartParam;
        function BindChart(element, param) {
            ChartRowElement = element;
            ChartParam = param;
            let headerID = "", machineID = "", date = "", shift = "", title = "", fromdate = "", todate = "", multiselectMachines = "";
            let screenType = "<%= (String)ViewState["ScreenType"] %>";
            if (param == "HeaderChart") {
                headerID = $(element).closest('th').find("[id$=hfHeaderID]").val();
                title = headerID;
                if ($("[id$=hfSelectedMenu]").val() == "DayDataMenu" || $("[id$=hfSelectedMenu]").val() == "WeekDataMenu") {
                  <%--  fromdate = "01" + "-" + "<%= (String) ViewState["Month"] %>" + "-" + "<%= (String) ViewState["Year"] %>";
                    todate = "31" + "-" + "<%= (String) ViewState["Month"] %>" + "-" + "<%= (String) ViewState["Year"] %>";--%>
                    fromdate = "01" + "-" + $("[id$=txtMonth]").val() + "-" + $("[id$=txtYear]").val();
                    todate = "31" + "-" + $("[id$=txtMonth]").val() + "-" + $("[id$=txtYear]").val();

                    title = "<b>Year: </b>" + $("[id$=txtYear]").val() + "<b> - Month: </b>" + $("[id$=txtMonth]").val();

                    if ($("[id$=hfSelectedMenu]").val() == "DayDataMenu") {
                        title += "<b> - Day: </b>" + headerID;
                    } else if ($("[id$=hfSelectedMenu]").val() == "WeekDataMenu") {
                        title += "<b> - Week: </b>" + headerID;
                    }

                } else if ($("[id$=hfSelectedMenu]").val() == "MonthDataMenu") {
                    fromdate = "01" + "-" + "01" + "-" + $("[id$=txtYear]").val();
                    todate = "31" + "-" + "12" + "-" + $("[id$=txtYear]").val();

                    title = "<b>Year: </b>" + $("[id$=txtYear]").val() + "<b> - Month: </b>" + headerID;;
                }
                else {
                    if ($("[id$=rblViewType] input:checked").val() == "Report") {
                        fromdate = $("[id$=txtFromDate]").val();
                        todate = $("[id$=txtToDate]").val();

                        title = "<b>From Date: </b>" + fromdate + "<b> - To Date: </b>" + todate;

                    } else {
                        fromdate = $("[id$=txtFromDate]").val();
                        todate = $("[id$=txtFromDate]").val();
                        //shift = $("[id$=ddlShift]").val();
                        shift = headerID;

                        title = "<b>Date: </b>" + fromdate + "<b> - Shift: </b>" + headerID;
                    }
                }
                //multiselectMachines = "<%= (String) ViewState["Machine"] %>";
                for (let i = 0; i < $("[id$=lbMachine]").val().length; i++) {
                    if (multiselectMachines == "") {
                        multiselectMachines = "'" + $("[id$=lbMachine]").val()[i] + "'";
                    } else {
                        multiselectMachines += ",'" + $("[id$=lbMachine]").val()[i] + "'";
                    }
                }
                title += "<b> - Machine: </b>" + $("[id$=lbMachine]").val();

            } else {
                machineID = $(element).closest('.tr-for-closest').find("[id$=hfMachineID]").val();

                title = machineID;
                if ($("[id$=hfSelectedMenu]").val() == "DayDataMenu" || $("[id$=hfSelectedMenu]").val() == "WeekDataMenu") {
                    fromdate = "01" + "-" + $("[id$=txtMonth]").val() + "-" + $("[id$=txtYear]").val();
                    todate = "31" + "-" + $("[id$=txtMonth]").val() + "-" + $("[id$=txtYear]").val();

                    title = "<b>Year: </b>" + $("[id$=txtYear]").val() + "<b> - Month: </b>" + $("[id$=txtMonth]").val();

                } else if ($("[id$=hfSelectedMenu]").val() == "MonthDataMenu") {
                    fromdate = "01" + "-" + "01" + "-" + $("[id$=txtYear]").val();
                    todate = "31" + "-" + "12" + "-" + $("[id$=txtYear]").val();

                    title = "<b>Year: </b>" + $("[id$=txtYear]").val();
                }
                else if ($("[id$=hfSelectedMenu]").val() == "HourlyDataMenu") {
                    if ($("[id$=rblViewType] input:checked").val() == "Report") {
                        date = $(element).closest('.tr-for-closest').find("[id$=hfDate]").val();
                        fromdate = $(element).closest('.tr-for-closest').find("[id$=hfDate]").val();
                        todate = $(element).closest('.tr-for-closest').find("[id$=hfDate]").val();

                        //title = "<b>Date: </b>" + date;

                    } else {
                        fromdate = $("[id$=txtFromDate]").val();
                        todate = $("[id$=txtFromDate]").val();
                        date = $("[id$=txtFromDate]").val();

                        //title = "<b>Date: </b>" + date;

                    }
                    shift = $(element).closest('.tr-for-closest').find("[id$=hfShift]").val();

                    title = "<b>Date: </b>" + date + "<b> - Shift: </b>" + shift;

                } else if ($("[id$=hfSelectedMenu]").val() == "ShiftDataMenu") {
                    if ($("[id$=rblViewType] input:checked").val() == "Report") {
                        date = $(element).closest('.tr-for-closest').find("[id$=hfDate]").val();
                        fromdate = $(element).closest('.tr-for-closest').find("[id$=hfDate]").val();
                        todate = $(element).closest('.tr-for-closest').find("[id$=hfDate]").val();

                        title = "<b>Date: </b>" + date;

                    } else {
                        fromdate = $("[id$=txtFromDate]").val();
                        todate = $("[id$=txtFromDate]").val();
                        date = $("[id$=txtFromDate]").val();
                        shift = $("[id$=ddlShift]").val();

                        title = "<b>Date: </b>" + date + "<b> - Shift: </b>" + shift;

                    }


                }
                multiselectMachines = "'" + machineID + "'";

                title += "<b> - Machine: </b>" + machineID;
            }
            $("#chartHeader").html(title);
            $.ajax({
                async: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ProductionRejectionDetails.aspx/GetChartDetails",
                data: '{param:"' + param + '",headerID:"' + headerID + '", machineID:"' + machineID + '", date:"' + date + '", shift:"' + shift + '", selectedMenu:"' + $("[id$=hfSelectedMenu]").val() + '", screenType:"' + screenType + '", fromDate:"' + fromdate + '", toDate:"' + todate + '",multiselectMachines:"' + multiselectMachines + '", plant:"' + $("[id$=ddlPlant]").val() + '"}',
                dataType: "json",
                success: function (result) {
                    dataitem = result.d;

                    if (screenType == "RejectionScreen") {
                        $("#pieChartOuterConatiner").css("height", "49%").css("overflow", "auto");
                        $("#paretoChartOuterConatiner").css("height", "49%").css("overflow", "auto");

                    } else {
                        $("#pieChartOuterConatiner").css("height", "90%").css("overflow", "auto");
                        $("#paretoChartOuterConatiner").css("height", "0%");
                    }

                    Highcharts.chart('pieChartContainer', {
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie',
                        },
                        credits: {
                            enabled: false
                        },
                        exporting: {
                            tableCaption: 'Data Table',
                            chartOptions: {
                                title: {
                                    text: title,
                                    style: {
                                        fontSize: '8px'
                                    },
                                }
                            },
                        },
                        title: {
                            // text: '<b>Date:</b> 25-08-2022 <b>- Shift:</b> First <b>- Machine:</b> Machine1'
                            // text: '<b>From Date:</b> 24-08-2022 <b>- To Date:</b> 25-08-2022<b> - Shift:</b> All <b>- Machine:</b> Machine1, Machine2'
                            text: '',
                            //style: {
                            //    fontSize: '15px'
                            //},
                        },
                        tooltip: {
                            // pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b><b>  ({point.positive?: point.y: point.y*-1})</b>'
                            formatter: function () {
                                var val = this.point.positive ? this.y : this.y * (-1);
                                var percentage = this.point.positive ? this.point.percentage.toFixed(2) : '-' + this.point.percentage.toFixed(2);
                                return '<b>' + this.point.name + ': ' + percentage + '% (' + val + ')';
                                // return '{series.name}: <b>{point.percentage:.1f}%</b><b>  ({point.positive?: point.y: point.y*-1})</b>';
                            }
                        },
                        accessibility: {
                            point: {
                                valueSuffix: '%'
                            }
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    // format: '<b>{point.name}</b>: {point.percentage:.1f} %  ({point.y})'
                                    formatter: function () {
                                        var val = this.point.positive ? this.y : this.y * (-1);
                                        var percentage = this.point.positive ? this.point.percentage.toFixed(2) : '-' + this.point.percentage.toFixed(2);
                                        return '<b>' + this.point.name + ': ' + percentage + '% (' + val + ')';
                                    },
                                }
                            }
                        },
                        series: [{
                            name: 'Production Count',
                            colorByPoint: true,
                            data: dataitem.PieChartDetails

                        }]
                    });
                    //&& param == "ColumnChart"

                    if (screenType == "RejectionScreen") {
                        Highcharts.chart('paretoChartContainer', {
                            chart: {
                                renderTo: 'paretoChartContainer',
                                type: 'column'
                            },
                            exporting: {
                                chartOptions: {
                                    title: {
                                        text: title,
                                        style: {
                                            fontSize: '8px'
                                        },
                                    }
                                },
                                tableCaption: 'Data Table',
                            },
                            title: {
                                text: '',
                                //style: {
                                //    fontSize: '15px'
                                //},
                            },
                            tooltip: {
                                shared: true
                            },
                            credits: {
                                enabled: false
                            },
                            xAxis: {
                                categories: dataitem.ParetoChartDetails.Categories,
                                crosshair: true
                            },
                            yAxis: [{
                                title: {
                                    text: ''
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
                                    format: "{value}%"
                                }
                            }],
                            series: [{
                                type: 'pareto',
                                name: 'Pareto',
                                yAxis: 1,
                                zIndex: 10,
                                baseSeries: 1,
                                tooltip: {
                                    valueDecimals: 2,
                                    valueSuffix: '%'
                                }
                            }, {
                                name: 'Column',
                                type: 'column',
                                zIndex: 2,
                                data: dataitem.ParetoChartDetails.Values,
                            }]
                        });

                        //pieChart.update({
                        //    chart: {
                        //        height: "49%",
                        //    }
                        //});
                        //paretoChart.update({
                        //    chart: {
                        //        height: "49%",
                        //    }
                        //});


                    }
                    //else {
                    //    //pieChart.update({
                    //    //    chart: {
                    //    //        height: "90%",
                    //    //    }
                    //    //});
                    //    //paretoChart.update({
                    //    //    chart: {
                    //    //        height: "0%",
                    //    //    }
                    //    //});

                    //}

                    $('.highcharts-data-table').css('display', 'none');
                    $(".modal-backdrop").removeClass("modal-backdrop in");
                    $("#ChartModal").modal("show");
                    $('#hdnIsChartModalOpen').val("true");
                },
                error: function (Result) {
                    alert("Error");
                }
            });


        }
        function openChartModal() {
            debugger;
            BindChart(ChartRowElement, ChartParam);
        }
        function closeChartModal() {

            $("#ChartModal").modal("hide");
            $('#hdnIsChartModalOpen').val("false");
        }
        function BindDownChart() {
            let screenType = "<%= (String)ViewState["ScreenType"] %>";
            let selectedMenu = $("[id$=hfSelectedMenu]").val();
            if (selectedMenu == "DayDataMenu" || selectedMenu == "WeekDataMenu" || selectedMenu == "MonthDataMenu") {
                $.ajax({
                    async: false,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "ProductionRejectionDetails.aspx/GetDownChartDetails",
                    data: '{selectedMenu:"' + selectedMenu + '", screenType:"' + screenType + '"}',
                    dataType: "json",
                    success: function (result) {

                        dataitem = result.d;

                        Highcharts.chart('chartConatiner', {
                            //chart: {
                            //    type: 'spline'
                            //},
                            title: {
                                text: dataitem.title,
                            },
                            xAxis: {
                                categories: dataitem.Categories,
                                //accessibility: {
                                //    description: 'Months of the year'
                                //}
                            },
                            yAxis: {
                                title: {
                                    text: 'Value'
                                },
                                //labels: {
                                //    formatter: function () {
                                //        return this.value + '°';
                                //    }
                                //}
                            },
                            tooltip: {
                                //crosshairs: true,
                                //shared: true
                                enabled: true,
                                pointFormat: '{series.name}: <b>  ({point.y})</b>',
                            },
                            plotOptions: {
                                spline: {
                                    marker: {
                                        radius: 4,
                                        lineColor: '#666666',
                                        lineWidth: 1
                                    }
                                }
                            },
                            series: dataitem.LineChartSeriesDetails,
                        });
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                });
            }


        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            //alert("<%= (String) ViewState["FromDate"] %>");
            $(document).ready(function () {
                ControlSetter();
                activeSubMenu();
                CallUnLoader();
                //var chart = $('#chartConatiner').highcharts();
                //chart.reflow();
                // charts.reflow();
                BindDownChart();
            });
            $(".submenuData").click(function () {
                $(".submenuData").removeClass("selected-menu-style").addClass("other-menu-style");
                $(".submenuData").closest('li').find('i').removeClass();
                $(this).removeClass("other-menu-style").addClass("selected-menu-style");
                submenu = $(this).attr('href');
                $("[id$=hfSelectedMenu]").val(submenu);
                __doPostBack('<%= btnMenu.UniqueID%>', "OnClick");
                localStorage.setItem("CumiSelectedMenu", submenu);
                CallLoader();
            });

            $(".from-date").on("click", function () {
                let element = $(this);
                element.datepicker('setStartDate', '');
                if ($("[id$=hfSelectedMenu]").val() == "HourlyDataMenu") {
                    element.datepicker('setStartDate', '-7d');
                } else if ($("[id$=hfSelectedMenu]").val() == "ShiftDataMenu" && $("[id$=rblViewType] input:checked").val() != "Report") {
                    if ($("[id$=rblAggLiveSSelection] input:checked").val() == "Live") {
                        element.datepicker('setStartDate', '-7d');
                    }
                }
                //if ($("[id$=rblViewType] input:checked").val() == "Report") {
                //    element.datepicker('setStartDate', '-7d');
                //} else {
                //    element.datepicker('setStartDate', '');
                //}
            });
        });
    </script>
</asp:Content>


<%--   <td style="padding: 0; border: unset">
                                    <table style="display: <%# Eval("HeaderVisibility") %>" class="inner-tbl">
                                        <tr>
                                            <th class="machine" style="display: <%# Eval("DateVisibility") %>">Date</th>
                                            <th class="machine">Machine ID</th>
                                            <th class="plan-actual"></th>
                                        </tr>
                                    </table>
                                    <table style="display: <%# Eval("ContentVisibility") %>" class=" inner-tbl">
                                        <tr>
                                            <td class="machine" style="display: <%# Eval("DateVisibility") %>">
                                                <%# Eval("Date") %>
                                            </td>
                                            <td class="machine"><%# Eval("MachineID") %></td>
                                            <td style="padding: 0; border: unset" class="plan-actual">
                                                <table class="inner-tbl plan-acual-tbl">
                                                    <tr>
                                                        <td class="processno">Plan</td>

                                                    </tr>
                                                    <tr>
                                                        <td class="processno">Actual</td>
                                                    </tr>
                                                </table>
                                            </td>

                                        </tr>

                                    </table>
                                </td>--%>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>--%>
