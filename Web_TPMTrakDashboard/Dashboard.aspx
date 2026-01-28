<%@ Page Title="Dashboard Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.WebForm1" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script src="MyCssAndJS/DatePicker/jquery-2.1.1.min.js"></script>
    <%--<script src="MyCssAndJS/toggel/jquery.min.js"></script>--%>
    <%--<script src="MyCssAndJS/toggel/bootstrap.min.js"></script>--%>

    <%-- //SELECTION SERACH--%>
    <%--<link href="MyCssAndJS/select/bootstrap-select.min.css" rel="stylesheet" />--%>
    <%--<script src="MyCssAndJS/select/bootstrap-select.min.js"></script>--%>

    <%--   <%: Styles.Render("~/bundles/dashboardcss") %>--%>
    <%: Scripts.Render("~/bundles/dashboardjs") %>
    <%: Scripts.Render("~/bundles/dashboard2js") %>
    <%: Styles.Render("~/bundles/filtercss") %>
    <%: Scripts.Render("~/bundles/filterjs") %>
    <%--  <link href="Content/Multi/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Content/Multi/jquery/jquery-1.8.3.min.js"></script>
    <script src="Content/Multi/bootstrap/js/bootstrap.min.js"></script>
    <script src="Content/js/bootstrap-datetimepicker.min.js"></script>--%>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <%--<link href="MyCssAndJS/css/bootstrap.min.css" rel="stylesheet" />--%>
    <%--<link href="MyCssAndJS/css/bootstrap-datepicker.css" rel="stylesheet" />--%>
    <%--<script src="MyCssAndJS/js/jquery.js"></script>--%>
    <%--<script src="MyCssAndJS/js/bootstrap.min.js"></script>--%>
    <%--<script src="MyCssAndJS/js/bootstrap-datepicker.js"></script>--%>

    <%-----------Highchrat config-------------%>
    <%--<script src="ChartScript/jquery.min.js"></script>--%>
    <%--  <script src="MyCssAndJS/chartjs/highcharts.js"></script>
    <script src="MyCssAndJS/chartjs/data.js"></script>
    <script src="MyCssAndJS/chartjs/drilldown.js"></script>--%>
    <script src="MyCssAndJS/Charts/highstock.js"></script>
    <%--<%: Scripts.Render("~/bundles/VDGjs") %>--%>
    <%: Scripts.Render("~/bundles/drilldownChartjs") %>
    <%--<%: Scripts.Render("~/bundles/commanChartjs") %>--%>

    <%--  <script src="Scripts/Toastr/toastr.min.js"></script>
    <link href="Scripts/Toastr/toastr.min.css" rel="stylesheet" />--%>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
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

        /*tbody {
            background-color: white;
        }*/

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        /*tbody tr:nth-child(even) {
            background: #CCC;
        }

       tbody tr:nth-child(odd) {
            background: #FFF;
        }*/
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

        tfoot td {
            position: sticky;
            bottom: 10px;
            text-align: center;
            z-index: 5;
            background-color: #2E6886 !important;
        }

        .toast-warning > .toast-message {
            font-size: 18px !important;
            font-weight: bold !important;
        }

        .toast-warning {
            opacity: 1 !important;
        }
    </style>
    <%--<link href="MyCssAndJS/font-awesome.min.css" rel="stylesheet" />--%>
    <%--<link rel="stylesheet" type="text/css" href="LoginDemo/fonts/font-awesome-4.7.0/css/font-awesome.min.css" />--%>
    <div class="ajax-loader">
        <div id="load-div">
            <img runat="server" src="~/img/loadIcon/ajax-loader.gif" class="img-responsive" />
        </div>
    </div>

    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"><%=GetGlobalResourceObject("CommanResource","ShiftWiseChart") %></h4>
                </div>
                <div class="modal-body">
                    <div id="divShiftChart" style="width: 548px"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <asp:Label ID="lblMessages" runat="server" EnableViewState="False" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>

    <div class="row" style="margin-bottom: 4px; margin-top: -11px;">
        <%-- <table id="tblHeader" class="table table-bordered" style="background-color: #394A59">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control plantDrop" data-toggle="tooltip" ToolTip="<%$Resources:CommanResource, PlantTooltip %>" title="Plant !">
                    </asp:DropDownList></td>
                <td>
                    <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" AutoCompleteType="Disabled"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" AutoCompleteType="Disabled"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtDay" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Day !" placeholder="Day" meta:resourcekey="txtDayResource1" AutoCompleteType="Disabled"></asp:TextBox></td>
                <td>
                    <asp:DropDownList ID="ddlShift" runat="server" data-toggle="tooltip" ToolTip="<%$Resources:CommanResource,ShiftTooltip %>" title="Shift !" CssClass="form-control displayCss commanDrop">
                    </asp:DropDownList></td>
                <td><span data-toggle="tooltip" title="<%=Resources.CommanResource.ComonentTooltip %>">
                    <select id="ddlComponentInfo" runat="server" cssclass="form-control" class="selectpicker" data-live-search-style="begins"
                        data-live-search="true">
                    </select></span></td>
                <td><span class="shade" data-toggle="tooltip" title="<%=Resources.CommanResource.EmployeeTooltip %>">
                    <select id="ddlEmployeeInfo" runat="server" class="selectpicker" data-live-search-style="begins"
                        data-live-search="true" cssclass="form-control">
                    </select></span></td>
                <td>
                    <button type="button" class="btn btn-info btn-sm displayCss plantDrop" id="btnProcess"><%=GetGlobalResourceObject("CommanResource","Process") %>  </button>
                </td>
                <td>
                    <input type="text" id="search" data-toggle="tooltip" autocomplete="off" title="<%=GetGlobalResourceObject("CommanResource","SearchHere") %>" placeholder="<%=GetLocalResourceObject("search here...") %>" class="form-control plantDrop"></td>
            </tr>
        </table>--%>
        <div class="col-lg-10" style="display: block; padding-left: 3px; padding-right: 0px;">
            <asp:DropDownList ID="ddlViewType" runat="server" CssClass="form-control plantDrop" data-toggle="tooltip" title="View Type !" Style="width: 146px; display: inline; background-color: #39b3d7; font-weight: bold; color: #ffffff" ToolTip="<%$Resources:CommanResource, ViewType %>">
                <asp:ListItem Value="PlantwiseView" meta:resourcekey="PlantView" Text="<%$Resources:CommanResource, PlantView %>"></asp:ListItem>
                <asp:ListItem Value="MachinewiseView" meta:resourcekey="MachineView" Text="<%$Resources:CommanResource, MachineView %>"></asp:ListItem>
                <asp:ListItem Value="ComponentwiseView" meta:resourcekey="ComponentView" Text="<%$Resources:CommanResource, ComponentView %>"></asp:ListItem>
                <asp:ListItem Value="OperatorwiseView" meta:resourcekey="OperatorView" Text="<%$Resources:CommanResource, OperatorView %>"></asp:ListItem>
                <asp:ListItem Value="CellWiseView" meta:resourcekey="OperatorView" Text="<%$Resources:CommanResource, CellView %>"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control plantDrop" data-toggle="tooltip" ToolTip="<%$Resources:CommanResource, PlantTooltip %>" title="Plant !" Style="width: 132px; display: inline;"></asp:DropDownList>
            <asp:DropDownList ID="ddlCellId" runat="server" CssClass="form-control plantDrop" data-toggle="tooltip" ToolTip="<%$Resources:CommanResource, CellToolTip %>" title="Cell ID !" Style="width: 120px; display: inline;" ClientIDMode="Static"></asp:DropDownList>
            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>
            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" Style="width: 70px; display: inline;"></asp:TextBox>
            <asp:TextBox ID="txtDay" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Day !" placeholder="Day" meta:resourcekey="txtDayResource1" Style="width: 70px; display: inline;"></asp:TextBox>
            <asp:DropDownList ID="ddlShift" runat="server" data-toggle="tooltip" ToolTip="<%$Resources:CommanResource,ShiftTooltip %>" title="Shift !" CssClass="form-control displayCss commanDrop" Style="width: 130px; display: inline;">
            </asp:DropDownList>
            <span id="componentDiv" data-toggle="tooltip" title="<%=Resources.CommanResource.ComonentTooltip %>">
                <select id="ddlComponentInfo" runat="server" cssclass="form-control" class="selectpicker" data-live-search-style="begins"
                    data-live-search="true">
                </select></span>
            <span id="employeeDiv" class="shade" data-toggle="tooltip" title="<%=Resources.CommanResource.EmployeeTooltip %>">
                <select id="ddlEmployeeInfo" runat="server" class="selectpicker" data-live-search-style="begins"
                    data-live-search="true" cssclass="form-control">
                </select></span>
            <button type="button" class="btn btn-info btn-sm displayCss plantDrop" id="btnProcess"><%=GetGlobalResourceObject("CommanResource","Process") %>  </button>
        </div>

        <div class="col-lg-2" style="text-align: right; width: 150px; float: right; padding-right: 5px; padding-left: 3px;">
            <input type="text" id="search" data-toggle="tooltip" title="<%=GetGlobalResourceObject("CommanResource","SearchHere") %>" placeholder="<%=GetLocalResourceObject("search here...") %>" class="form-control plantDrop">
        </div>
    </div>

    <div class="row dashboardTbl" style="overflow-x: auto; overflow-y: auto;">
        <table id="tblDashboardInfo" class="table table-bordered table-hover tbl tblSetting headerFixer">
            <thead class="blue">
                <tr>
                    <th style="vertical-align: middle; /*text-align: center; ↕ */">Machine 
                    </th>
                    <th style="vertical-align: middle; /*text-align: center; ↕ */">Machine Description
                    </th>

                    <th style='vertical-align: middle;'><%=setting.Count > 0  ? setting[1].ValueInText2: string.Empty %>
                        <span style="display: none"><%= setting.Count > 0 ? setting[1].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle'><%=setting.Count > 0  ?  setting[2].ValueInText2 : string.Empty%>
                        <span style="display: none"><%=setting.Count > 0  ? setting[2].ValueInText: string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle'><%=setting.Count > 0  ?  setting[3].ValueInText2 : string.Empty%>
                        <span style="display: none"><%= setting.Count > 0 ? setting[3].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle;'><%= setting.Count > 0 ? setting[0].ValueInText2 : string.Empty %>
                        <span style="display: none"><%= setting.Count > 0 ? setting[0].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle;'><%= setting.Count > 0  ? setting[4].ValueInText2: string.Empty %>
                        <span style="display: none"><%= setting.Count > 0 ? setting[4].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle;'><%= setting.Count > 0  ? setting[5].ValueInText2: string.Empty %>
                        <span style="display: none"><%= setting.Count > 0 ? setting[5].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle'><%= setting.Count > 0  ? setting[6].ValueInText2 : string.Empty%>
                        <span style="display: none"><%= setting.Count > 0 ? setting[6].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle'><%= setting.Count > 0  ? setting[7].ValueInText2 : string.Empty%>
                        <span style="display: none"><%= setting.Count > 0 ? setting[7].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle; width: 14%;'><%=setting.Count > 0  ?  setting[8].ValueInText2 : string.Empty%>
                        <span style="display: none"><%= setting.Count > 0 ? setting[8].ValueInText : string.Empty %></span>
                    </th>
                    <th style='vertical-align: middle; width: 14%; white-space: nowrap;'><%=setting.Count > 0  ?  setting[9].ValueInText2 : string.Empty%>
                        <span style="display: none"><%= setting.Count > 0 ? setting[9].ValueInText : string.Empty %></span>
                    </th>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <td class="text-center">&nbsp;</td>
                    <td class="text-center">&nbsp;</td>

                    <td class="text-center" style='display: <%= setting[1].Display %>'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarAE" paramvalue="<%= setting[1].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[1].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                        <%--title="<%=GetLocalResourceObject("AE Bar Chart!") %>"--%>
                        <%--   <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarAE" data-toggle="tooltip" title="AE Bar Chart!" />
                        &nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineAE" data-toggle="tooltip" title="AE Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineAE" paramvalue="<%= setting[1].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[1].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("AE Line Chart!") %>"--%>
                    </td>
                    <td class="text-center" style='display: <%= setting[2].Display %>'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarPE" paramvalue="<%= setting[2].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[2].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                        <%--title="<%=GetLocalResourceObject("PE Bar Chart!") %>"--%>
                        <%-- <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarPE" data-toggle="tooltip" title="PE Bar Chart!" />
                        &nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLinePE" data-toggle="tooltip" title="PE Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLinePE" paramvalue="<%= setting[2].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[2].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("PE Line Chart!") %>"--%>
                    </td>
                    <td class="text-center" style='display: <%= setting[3].Display %>'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarQE" paramvalue="<%= setting[3].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[3].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                        <%--title="<%=GetLocalResourceObject("QE Bar Chart!") %>"--%>
                        <%--<img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarQE" data-toggle="tooltip" title="QE Bar Chart!" />
                        &nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineQE" data-toggle="tooltip" title="QE Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineQE" paramvalue="<%= setting[3].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[3].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("QE Line Chart!") %>"--%>
                    </td>
                    <td class="text-center" style='display: <%= setting[0].Display %>'>

                        <i class="fa fa-bar-chart chartColor" id="btnBarOEE" paramvalue="<%= setting[0].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[0].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                         <%--title="<%=GetLocalResourceObject("OEEBarChart") %>"--%>
                        <%--    <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarOEE" data-toggle="tooltip" title="OEE Bar Chart!" />
                        &nbsp;
                          <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineOEE" data-toggle="tooltip" title="OEE Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineOEE" paramvalue='<%= setting[0].ValueInText2 %>' data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[0].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("OEELineChart") %>"--%>

                    </td>
                    <td class="text-center" style='display: <%= setting[4].Display %>'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarAccepted" paramvalue="<%= setting[4].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[4].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                        <%--title="<%=GetLocalResourceObject("Accepted Bar Chart!") %>"--%>
                        <%-- <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarAccepted" data-toggle="tooltip" title="Accepted Bar Chart!" />
                        &nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineAccepted" data-toggle="tooltip" title="Accepted Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineAccepted" paramvalue="<%= setting[4].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[4].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("Accepted Line Chart!") %>"--%>
                    </td>
                    <td class="text-center" style='display: <%= setting[5].Display %>'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarRejected" paramvalue="<%= setting[5].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[5].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                         <%--title="<%=GetLocalResourceObject("Rejected Bar Chart!") %>"--%>
                        <%-- <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarRejected" data-toggle="tooltip" title="Rejected Bar Chart!" />
                        &nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineRejected" data-toggle="tooltip" title="Rejected Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineRejected" paramvalue="<%= setting[5].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[5].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("Rejected Line Chart!") %>"--%>
                    </td>
                    <td class="text-center" style='display: <%= setting[6].Display %>'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarRework" paramvalue="<%= setting[6].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[6].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                        <%--title="<%=GetLocalResourceObject("Rework Bar Chart!") %>"--%>
                        <%--  <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarRework" data-toggle="tooltip" title="Rework Bar Chart!" />
                        &nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineRework" data-toggle="tooltip" title="Rework Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineRework" paramvalue="<%= setting[6].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[6].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("Rework Line Chart!") %>"--%>
                    </td>
                    <td>
                        <i class="fa fa-bar-chart chartColor" id="btnBarPPM" paramvalue="<%= setting[7].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[7].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                         <%--title="<%=GetLocalResourceObject("PPM Bar Chart!") %>"--%>
                        <%--  <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarRework" data-toggle="tooltip" title="Rework Bar Chart!" />
                        &nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineRework" data-toggle="tooltip" title="Rework Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLinePPM" paramvalue="<%= setting[7].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[7].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("PPM Line Chart!") %>"--%>
                    </td>
                    <td class="text-center" style='display: <%= setting[8].Display %>;'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarStopp" paramvalue="<%= setting[8].ValueInText2 %>" timetext="<%= model.DownTime %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[8].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                        <%--title="<%=GetLocalResourceObject("Downtime Bar Chart!") %>"--%>
                        <%-- <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarStopp" data-toggle="tooltip" title="Downtime Bar Chart!" />&nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineStopp" data-toggle="tooltip" title="Downtime Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineStopp" paramvalue="<%= setting[8].ValueInText2 %>" timetext="<%= model.DownTime %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[8].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("Downtime Line Chart!") %>"--%>
                        <%--<i class="fa fa-bar-chart" style="cursor: pointer; color: white;" id="btnBarStopp" data-toggle="tooltip" title="Downtime Bar Chart!"></i>&nbsp;&nbsp;
                      <i class="fa fa-line-chart" style="cursor: pointer; color: white;" id="btnLineStopp" data-toggle="tooltip" title="Downtime Line Chart!"></i>--%>
                    </td>
                    <td class="text-center" style='display: <%= setting[9].Display %>;'>
                        <i class="fa fa-bar-chart chartColor" id="btnBarMcHrRate" paramvalue="<%= setting[9].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[9].ValueInText2 + " Bar Chart!") : string.Empty %>'></i>&nbsp;
                        <%--title="<%=GetLocalResourceObject("Machine Hour Rate Bar Chart!") %>"--%>
                        <%-- <img src="Image/ChartImg/barchart.png" style="cursor: pointer; color: white;" id="btnBarStopp" data-toggle="tooltip" title="Downtime Bar Chart!" />&nbsp;
                        <img src="Image/ChartImg/linechart.png" style="cursor: pointer; color: white;" id="btnLineStopp" data-toggle="tooltip" title="Downtime Line Chart!" />--%>
                        <i class="fa fa-line-chart chartColor" id="btnLineMcHrRate" paramvalue="<%= setting[9].ValueInText2 %>" data-toggle="tooltip" title='<%= setting.Count > 0 ? (setting[9].ValueInText2 + " Line Chart!") : string.Empty %>'></i>
                        <%--title="<%=GetLocalResourceObject("Machine Hour Rate Line Chart!") %>"--%>
                        <%--<i class="fa fa-bar-chart" style="cursor: pointer; color: white;" id="btnBarStopp" data-toggle="tooltip" title="Downtime Bar Chart!"></i>&nbsp;&nbsp;
                      <i class="fa fa-line-chart" style="cursor: pointer; color: white;" id="btnLineStopp" data-toggle="tooltip" title="Downtime Line Chart!"></i>--%>
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>


    <%------------------------OEE Chart Div-----------------%>
    <%--<div id="divlineOEEchart" style="width: 100%;"></div>--%>
    <input type="hidden" id="hdnChartType" />
    <input type="hidden" id="hdnCurrentChart" />
    <input type="hidden" id="hdnSelectedParam" />
    <div style="position: relative">
        <button type="button" class="btn" style="position: absolute; top: 10px; right: 0px; z-index: 50; visibility: hidden; border: 1px solid #688a7e" id="btnBack"><< Back to</button>
        <%-- <div class="row" id="divcolumnOEEchart" style="margin-top: -16px; min-height: 250px; min-width: 100%;"></div>--%>
        <div class="row" id="divcolumnOEEchart" style="margin-top: -16px; min-height: 250px;"></div>
    </div>
    <%--<div class="col-lg-12" id="divcolumnOEEchart" style="margin-top: -16px; min-height: 250px;"></div>--%>
    <div id="hdfChartName"></div>
    <div id="divChartType"></div>
    <div id="chartOrder"></div>
    <div id="SortColumn"></div>
    <div id="divParamValue"></div>
    <script>
        //------ Start Shorting Table Data---------    
        ////http://jsfiddle.net/Zhd2X/21/
        var countDrillDown = 0; var ChartType = ""; var CombinedCharts = "";
        $('th').each(function (col) {

            if ($(this).html().trim() != "Machine" && (($(this).html().trim() != "Machine Description"))) {
                $(this).hover(
                    function () { $(this).addClass('focus'); },
                    function () { $(this).removeClass('focus'); }

                );
                $(this).click(function () {
                    //                    var headerTxt = $(this).html().trim();
                    var headerTxt = $(this).children().text();
                    if ($(this).is('.asc')) {
                        $(this).removeClass('asc');
                        $(this).addClass('desc selected');
                        sortOrder = -1;
                        $("#chartOrder").val('desc');
                    }
                    else {
                        $(this).addClass('asc selected');
                        $(this).removeClass('desc');
                        sortOrder = 1;
                        $("#chartOrder").val('asc');
                    }
                    if (countDrillDown == 0)
                        checkingText(headerTxt);
                    $(this).siblings().removeClass('asc selected');
                    $(this).siblings().removeClass('desc selected');
                    //var arrData = $('table').find('tbody >tr:has(td)').get();
                    var arrData = $('#tblDashboardInfo').find('tbody >tr:has(td)').get();
                    arrData.sort(function (a, b) {
                        var val1 = $(a).children('td').eq(col).text().toUpperCase();
                        var val2 = $(b).children('td').eq(col).text().toUpperCase();
                        if ($.isNumeric(val1) && $.isNumeric(val2))
                            return sortOrder == 1 ? val1 - val2 : val2 - val1;
                        else
                            return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                    });
                    $("#tblDashboardInfo tbody tr").each(function () {
                        $(this).remove();
                    });
                    var tableContain = "<tbody>";
                    //$.each(arrData, function (index, row) {
                    //    console.log(row);
                    //    $('tbody').append(row);
                    //});
                    $.each(arrData, function (index, row) {
                        //console.log(row);
                        tableContain += '<tr>' + (row.innerHTML) + '</tr>';
                    });
                    tableContain += '</tbody>';
                    $("#tblDashboardInfo").append(tableContain);
                });
            }
        });
        $('[id$=ddlViewType]').change(function () {
            $('#componentDiv').css("display", "inline-block");
            $('#employeeDiv').css("display", "inline-block");
            if ($('[id$=ddlViewType]').val() == "OperatorwiseView") {
                $('#componentDiv').css("display", "none");
            }
            else if ($('[id$=ddlViewType]').val() == "ComponentwiseView") {
                $('#employeeDiv').css("display", "none");
            }
            GetChartType();
            GetCombinedChartType();
        });
        function getComponentID() {
            var comp = "All";
            if ($('[id$=ddlViewType]').val() == "OperatorwiseView") {
                comp = "All";
            }
            else {
                comp = $("[id$=ddlComponentInfo]").val();
            }
            return comp;
        }
        function getOperatorID() {
            var opr = "All";
            if ($('[id$=ddlViewType]').val() == "ComponentwiseView") {
                opr = "All";
            }
            else {
                opr = $("[id$=ddlEmployeeInfo]").val();
            }
            return opr;
        }
        function getYaxisMax(param) {
            if (param === 'OEE' || param === 'AE' || param === 'PE' || param === 'QE')
                return 100;
            else
                return null;
        }

        $("#SortColumn").val("MachineId");
        var winHeight = $(window).height();
        winHeight = screen.availHeight;
        if (winHeight < 650) {
            winHeight = (winHeight - 350);
            console.log('min');
        } else {
            winHeight = (winHeight - 625);
            console.log('max');
        }
        var paramValue = $("#btnBarOEE").attr("paramValue");
        $("#divParamValue").val(paramValue);
        $("#divcolumnOEEchart").height(winHeight);
        function checkingText(inputVal) {

            if ($("[id$=txtYear]").val() == "") {
                alert("Please enter Year !!");
                $("[id$=txtYear]").focus();
                return false;
            }
            //
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            if (inputVal == 'Machine Description') {
                $.unblockUI({});
            }
            else if (inputVal == ('OEE(%)')) {

                $("#SortColumn").val("OEffy");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarOEE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("OEE");
                    $("#hdfChartName").val("OEE");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarOEE").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLineOEE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("OEE");
                    $("#hdfChartName").val("OEE");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineOEE").css("color", "#f5a507");
                }
            }
            else if (inputVal == ('AE(%)')) {
                $("#SortColumn").val("AEffy");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarAE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("AE");
                    $("#hdfChartName").val("AE");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarAE").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLineAE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("AE");
                    $("#hdfChartName").val("AE");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineAE").css("color", "#f5a507");
                }
            } else if (inputVal == ('PE(%)')) {
                $("#SortColumn").val("PEffy");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarPE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("PE");
                    $("#hdfChartName").val("PE");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarPE").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLinePE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("PE");
                    $("#hdfChartName").val("PE");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLinePE").css("color", "#f5a507");
                }
            } else if (inputVal == ('QE(%)')) {
                $("#SortColumn").val("QEffy");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarQE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("QE");
                    $("#hdfChartName").val("QE");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarQE").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLineQE").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("QE");
                    $("#hdfChartName").val("QE");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineQE").css("color", "#f5a507");
                }
            } else if (inputVal == 'Accepted') {
                $("#SortColumn").val("AcceptedParts");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarAccepted").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("AcceptedParts");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","AcceptedParts") %>");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarAccepted").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLineAccepted").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("AcceptedParts");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","AcceptedParts") %>");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineAccepted").css("color", "#f5a507");
                }
            } else if (inputVal == 'Rejected') {
                $("#SortColumn").val("RejCount");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarRejected").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("RejCount");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","RejCount") %>");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarRejected").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLineRejected").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("RejCount");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","RejCount") %>");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineRejected").css("color", "#f5a507");
                }
            } else if (inputVal == 'Rework') {
                $("#SortColumn").val("ReworkPerformed");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarRework").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("ReworkPerformed");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","ReworkPerformed") %>");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarRework").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLineRework").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("ReworkPerformed");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","ReworkPerformed") %>");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineRework").css("color", "#f5a507");
                }
            } else if (inputVal == 'PPM') {
                //
                $("#SortColumn").val("PPM");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarPPM").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("PPM");
                    $("#hdfChartName").val("PPM");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarPPM").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLinePPM").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("PPM");
                    $("#hdfChartName").val("PPM");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLinePPM").css("color", "#f5a507");
                }
            }
            else if ((inputVal == 'Machine Hour Rate')) {

                $("#SortColumn").val("AvgMcHrRate");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarMcHrRate").attr("paramValue");
                    //   alert(paramValue);
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("AvgMcHrRate");
                    $("#hdfChartName").val("AvgMcHrRate");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarMcHrRate").css("color", "#f5a507");

                } else {
                    paramValue = $("#btnLineMcHrRate").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("AvgMcHrRate");
                    $("#hdfChartName").val("AvgMcHrRate");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineMcHrRate").css("color", "#f5a507");
                }
            }
            else {
                $("#SortColumn").val("DownTime");
                if ($("#divChartType").val() == 'column') {
                    paramValue = $("#btnBarStopp").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicColumnOEEChart("DownTime");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","DownTime") %>");
                    $("#divChartType").val("column");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnBarStopp").css("color", "#f5a507");
                } else {
                    paramValue = $("#btnLineStopp").attr("paramValue");
                    $("#divParamValue").val(paramValue);
                    BindDynamicLineOEEChart("DownTime");
                    $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","DownTime") %>");
                    $("#divChartType").val("line");
                    $(".chartColor").css("color", "#FFFFFF");
                    $("#btnLineStopp").css("color", "#f5a507");
                }
            }
        }
        $('[data-toggle="tooltip"]').tooltip();
        ///------------------------------------Searching Code------------------------------
        $(document).ready(function () {
            debugger;
            fillcolumnswidth();
            $('#search').keyup(function () {
                searchTable($(this).val());
            });

            $('[id$=ddlPlantId]').change(function () {
                var value = $('[id$=ddlPlantId]').val();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Dashboard.aspx/GetCellIdData",
                    data: '{PlantId:"' + value + '"}',
                    dataType: "json",
                    success: function (result) {
                        var cellIdList = result.d;
                        if (cellIdList.length > 0) {
                            $('#ddlCellId').empty();
                            for (var i = 0; i < cellIdList.length; i++) {
                                $("#ddlCellId").append('<option value="' + cellIdList[i] + '">' + cellIdList[i] + '</option>');
                            }
                        }
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                });
            });
            showLicenseExpireDay();
            GetChartType();
            GetCombinedChartType();

        });
        function showLicenseExpireDay() {
            $.ajax({
                timeout: 5000,
                async: false,
                type: "POST",
                url: "Dashboard.aspx/getLicenseExpireDayFromSession",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (response) {
                    var itemdata = response.d;
                    if (itemdata != "") {
                        licenseNsg("Software license will expire in " + itemdata + " day.", "");
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                }
            });
        }
        function licenseNsg(msg, title) {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "4000",
                "hideDuration": "1000",
                "timeOut": "4000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "slideDown",
                "hideMethod": "slideUp"
            }

            toastr['warning'](msg, title);
            return false;
        }
        function fillcolumnswidth() {
            var i = 0;
            $('#tblDashboardInfo tr th').each(function () {
                thWidth = $(this).width();
                $('#tblDashboardInfo tr th')[i].width = thWidth;
                //console.log($('#tblDashboardInfo tr td')[i].width);
                i++;
            });
        }

        function searchTable(inputVal) {
            var table = $('#tblDashboardInfo');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).text())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }


        //---------End Shorting Table Data-----------------------------

        function BindSettingInfo() {

            if (parseInt(<%= setting[0].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(6)").hide();
                $("#tblDashboardInfo td:nth-child(6)").hide();
            } if (parseInt(<%= setting[1].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(3)").hide();
                $("#tblDashboardInfo td:nth-child(3)").hide();
            } if (parseInt(<%= setting[2].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(4)").hide();
                $("#tblDashboardInfo td:nth-child(4)").hide();
            } if (parseInt(<%= setting[3].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(5)").hide();
                $("#tblDashboardInfo td:nth-child(5)").hide();
            } if (parseInt(<%= setting[4].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(7)").hide();
                $("#tblDashboardInfo td:nth-child(7)").hide();
            } if (parseInt(<%= setting[5].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(8)").hide();
                $("#tblDashboardInfo td:nth-child(8)").hide();
            } if (parseInt(<%= setting[6].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(9)").hide();
                $("#tblDashboardInfo td:nth-child(9)").hide();
            } if (parseInt(<%= setting[7].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(10)").hide();
                $("#tblDashboardInfo td:nth-child(10)").hide();
            } if (parseInt(<%= setting[8].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(11)").hide();
                $("#tblDashboardInfo td:nth-child(11)").hide();
            } if (parseInt(<%= setting[9].Setvalue %>) == 2) {
                $("#tblDashboardInfo th:nth-child(12)").hide();
                $("#tblDashboardInfo td:nth-child(12)").hide();
            }
            else {
                if ($("[id$=ddlViewType]").val() == "OperatorwiseView" || $("[id$=ddlViewType]").val() == "ComponentwiseView") {
                    $("#tblDashboardInfo th:nth-child(12)").hide();
                    $("#tblDashboardInfo td:nth-child(12)").hide();
                }
                else {
                    $("#tblDashboardInfo th:nth-child(12)").show();
                    $("#tblDashboardInfo td:nth-child(12)").show();
                }
            }
            if (getQEEnabled() == false) {
                $("#tblDashboardInfo th:nth-child(5)").hide();
                $("#tblDashboardInfo td:nth-child(5)").hide();
            }
            fillcolumnswidth();
        }

        function getQEEnabled() {
            var isEnabled = true;
            $.ajax({
                async: false,
                type: "POST",
                url: "Dashboard.aspx/getQEVisibility",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    isEnabled = itmData;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            return isEnabled;
        }
        function bindTableFristCol() {
            if ($("[id$=ddlViewType]").val() == "PlantwiseView") {
                $("table tr>th:first").html("<%=GetLocalResourceObject("Plant")%>");
                $('tr td:nth-child(2)').hide();
                $("table tr>th:nth-child(2)").hide();
            }
            else if ($("[id$=ddlViewType]").val() == "CellWiseView") {
                $("table tr>th:first").html("<%=GetLocalResourceObject("Cell")%>");
                $('tr td:nth-child(2)').hide();
                $("table tr>th:nth-child(2)").hide();
            }
            else if ($("[id$=ddlViewType]").val() == "ComponentwiseView") {
                $("table tr>th:first").html("<%=GetLocalResourceObject("Component")%>");
                $('tr td:nth-child(2)').hide();
                $("table tr>th:nth-child(2)").hide();
            }
            else if ($("[id$=ddlViewType]").val() == "OperatorwiseView") {
                $("table tr>th:first").html("<%=GetLocalResourceObject("Operator")%>");
                $('tr td:nth-child(2)').hide();
                $("table tr>th:nth-child(2)").hide();
            }
            else {
                $("table tr>th:first").html("<%=GetLocalResourceObject("Machine")%>");
                $('tr td:nth-child(2)').show();
                $("table tr>th:nth-child(2)").show();
            }
        }
        if (ChartType == "Combined") {
            debugger;
            if (CombinedCharts.includes('OEE')) {
                if (CombinedCharts.includes('AE')) {
                    $("#btnBarAE").css("color", "#f5a507");
                }
                if (CombinedCharts.includes('PE')) {
                    $("#btnBarPE").css("color", "#f5a507");
                }
                if (CombinedCharts.includes('QE')) {
                    $("#btnBarQE").css("color", "#f5a507");
                }
                if (CombinedCharts.includes('OEE')) {
                    $("#btnBarOEE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnBarOEE").css("color", "#f5a507");
            }
        }
        else {
            $("#btnBarOEE").css("color", "#f5a507");
        }
        $("#btnBarOEE").click(function () {
            debugger;
            if ($("[id$=txtYear]").val() == "") {
                alert("Please enter Year !!");
                $("[id$=txtYear]").focus();
                return false;
            }
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('OEE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnBarAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnBarPE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnBarQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnBarOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnBarOEE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnBarOEE").css("color", "#f5a507");
            }
            var paramValue = $(this).attr("paramValue");
            $("#divParamValue").val($(this).attr("paramValue"));
            // $("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            //if ($(this).attr("id").replace('btnBar', '') == $("#hdfChartName").val()) {
            //    chart.series[0].update({
            //        type: "line"
            //    });
            //} else {
            //countDrillDown = 0;
            BindDynamicColumnOEEChart("OEE");
            //}
            $("#hdfChartName").val("OEE");
            $("#divChartType").val("column");

        });
        $("#btnLineOEE").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //if ($(this).attr("id").replace('btnLine', '') == $("#hdfChartName").val()) {
            //    
            //    chart.series[0].update({
            //        type: "column"
            //    });
            //} else {
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart("OEE");
            //}
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('OEE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnLineAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnLinePE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnLineQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnLineOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnLineOEE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnLineOEE").css("color", "#f5a507");
            }
        });

        $("#btnBarAE").click(function () {
            debugger;
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart("AE");
            $("#hdfChartName").val("AE");
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('AE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnBarAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnBarPE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnBarQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnBarOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnBarAE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnBarAE").css("color", "#f5a507");
            }
        });
        $("#btnLineAE").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart("AE");
            $("#hdfChartName").val("AE");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('AE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnLineAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnLinePE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnLineQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnLineOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnLineAE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnLineAE").css("color", "#f5a507");
            }
        });

        $("#btnBarPE").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart("PE");
            $("#hdfChartName").val("PE");
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('PE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnBarAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnBarPE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnBarQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnBarOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnBarPE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnBarPE").css("color", "#f5a507");
            }
        });
        $("#btnLinePE").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart("PE");
            $("#hdfChartName").val("PE");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('PE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnLineAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnLinePE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnLineQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnLineOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnLinePE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnLinePE").css("color", "#f5a507");
            }
        });

        $("#btnBarQE").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart("QE");
            $("#hdfChartName").val("QE");
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('QE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnBarAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnBarPE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnBarQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnBarOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnBarQE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnBarQE").css("color", "#f5a507");
            }
        });
        $("#btnLineQE").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart("QE");
            $("#hdfChartName").val("QE");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            if (ChartType == "Combined") {
                if (CombinedCharts.includes('QE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnLineAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnLinePE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnLineQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnLineOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnLineQE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnLineQE").css("color", "#f5a507");
            }
        });

        $("#btnBarAccepted").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart_old("AcceptedParts");
            <%--$("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","AcceptedParts") %>");--%>
            $("#hdfChartName").val("AcceptedParts");
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnBarAccepted").css("color", "#f5a507");
        });
        $("#btnLineAccepted").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart_old("AcceptedParts");
        	<%--$("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","AcceptedParts") %>");--%>
            $("#hdfChartName").val("AcceptedParts");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnLineAccepted").css("color", "#f5a507");
        });

        $("#btnBarRejected").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart_old("RejCount");
        	<%--$("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","RejCount") %>");--%>
            $("#hdfChartName").val("RejCount");
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnBarRejected").css("color", "#f5a507");
        });
        $("#btnLineRejected").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart_old("RejCount");
        	<%-- $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","RejCount") %>");--%>
            $("#hdfChartName").val("RejCount");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnLineRejected").css("color", "#f5a507");
        });

        $("#btnBarRework").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart_old("ReworkPerformed");
            $("#hdfChartName").val("ReworkPerformed");
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnBarRework").css("color", "#f5a507");
        });
        $("#btnLineRework").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart_old("ReworkPerformed");
            $("#hdfChartName").val("ReworkPerformed");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnLineRework").css("color", "#f5a507");
        });

        $("#btnBarPPM").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart_old("PPM");
            $("#hdfChartName").val("PPM");
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnBarPPM").css("color", "#f5a507");
        });

        $("#btnLinePPM").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart_old("PPM");
            $("#hdfChartName").val("PPM");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnLinePPM").css("color", "#f5a507");
        });

        $("#btnBarStopp").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart_old("DownTime");
        	<%-- $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","DownTime") %>");--%>
            $("#hdfChartName").val("DownTime")
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnBarStopp").css("color", "#f5a507");
        });

        $("#btnBarMcHrRate").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicColumnOEEChart_old("AvgMcHrRate");
        	<%-- $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","DownTime") %>");--%>
            $("#hdfChartName").val("AvgMcHrRate")
            $("#divChartType").val("column");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnBarMcHrRate").css("color", "#f5a507");
        });

        $("#btnLineMcHrRate").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart_old("AvgMcHrRate");
        	<%-- $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","DownTime") %>");--%>
            $("#hdfChartName").val("AvgMcHrRate");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnLineMcHrRate").css("color", "#f5a507");
        });
        $("#btnLineStopp").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            $("#divParamValue").val($(this).attr("paramValue"));
            //countDrillDown = 0;
            //$("#chartOrder").val('');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            BindDynamicLineOEEChart_old("DownTime");
        	<%-- $("#hdfChartName").val("<%=GetGlobalResourceObject("CommanResource","DownTime") %>");--%>
            $("#hdfChartName").val("DownTime");
            $("#divChartType").val("line");
            $(".chartColor").css("color", "#FFFFFF");
            $("#btnLineStopp").css("color", "#f5a507");
        });

        $('[id$=txtYear]').datepicker({
            minViewMode: 2,
            format: 'yyyy',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });

        $('[id$=txtMonth]').datepicker({
            viewMode: "months",
            minViewMode: "months",
            format: 'mm',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });

        //$('[id$=txtMonth]').setDefaults(userLang);
        ///--------Frist Time Load Data----------
        var year = (new Date()).getFullYear();
        //$('[id$=txtYear]').val(year);
        $('.ajax-loader').show();
        // $.unblockUI({});
        //$('.ajax-loader').hide();
        //  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        DashboardDetails();
        $("#chartOrder").val('');
        BindDynamicColumnOEEChart(ChartType == "Combined" ? CombinedCharts : "OEE");
        $("#hdfChartName").val("OEE");
        //$("#hdfChartName").val(ChartType =="Combined"?CombinedCharts:"OEE");
        $("#divChartType").val("column");
        ///--------End Data-----------------

        $('[id$=txtDay]').datepicker({
            viewMode: "days",
            minViewMode: "days",
            //format: 'dd-mm-yyyy',
            format: 'dd',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });

        $("#btnProcess").click(function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            if ($("[id$=txtDay]").val() != "") {
                if ($("[id$=txtMonth]").val() == "") {
                    alert("<%=GetLocalResourceObject("PleaseEnterMonth")%>");
                    $("[id$=txtMonth]").focus();
                    return false;
                }
            }
            //if ($("[id$=txtMonth]").val() != "") {
            //    alert("Please enter Month !!");
            //    $("[id$=txtMonth]").focus();
            //    return false;
            //}  
            var paramValue = $("#btnBarOEE").attr("paramValue");
            $("#divParamValue").val(paramValue);
            $(".chartColor").css("color", "#FFFFFF");
            //$("#btnBarOEE").css("color", "#f5a507");
            if (ChartType == "Combined") {
                debugger;
                if (CombinedCharts.includes('OEE')) {
                    if (CombinedCharts.includes('AE')) {
                        $("#btnBarAE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('PE')) {
                        $("#btnBarPE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('QE')) {
                        $("#btnBarQE").css("color", "#f5a507");
                    }
                    if (CombinedCharts.includes('OEE')) {
                        $("#btnBarOEE").css("color", "#f5a507");
                    }
                }
                else {
                    $("#btnBarOEE").css("color", "#f5a507");
                }
            }
            else {
                $("#btnBarOEE").css("color", "#f5a507");
            }
            countDrillDown = 0;
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            $("#chartOrder").val('');
            $("#SortColumn").val("MachineId");
            DashboardDetails();
            BindDynamicColumnOEEChart('OEE');
            $("#hdfChartName").val("OEE");
            //$("#hdfChartName").val(ChartType == "Combined" ? CombinedCharts : "OEE");
            $("#divChartType").val("column");
        });

        function DashboardDetails() {
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/BindDashboardData",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + $("[id$=txtMonth]").val() + '", strDay:"' + $("[id$=txtDay]").val() + '", strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + getComponentID() + '", employeeId:"' + getOperatorID() + '", cellId:"' + $("[id$=ddlCellId]").val() + '",windowSize:"' + $(window).width() + '",viewType:"' + $("[id$=ddlViewType]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    $("#tblDashboardInfo tbody tr").each(function () {
                        $(this).remove();
                    });
                    if (itmData.length > 0) {
                        var tableContain = "<tbody>";

                        $(itmData).each(function (index, md) {

                            tableContain += ('<tr><td>' + md.MachineId + '</td><td>' + md.MachineDescription + '</td><td style="background-color:' + md.AEColor + ';">' + md.AE + '</td><td style="background-color:' + md.PEColor + ';">' + md.PE + '</td><td style=background-color:' + md.QEColor + ';">' + md.QE +
                                '</td><td style="background-color:' + md.OEEColor + ';">' + md.OEE + '</td></td><td><a class="accepted" style="padding: 0px 0px;color:#2991FF;text-decoration: underline;cursor:pointer">' + md.Accepted + '</a></td></td><td><a class="rejected" style="padding: 0px 0px;color:#2991FF;text-decoration: underline;cursor:pointer">'
                                + md.Rejected + '</a></td></td><td><a id="rework" style="padding: 0px 0px;color:#2991FF;text-decoration: underline;cursor:pointer">' + md.Rework + '</a></td><td><a class="ppm" id="ppm" style="padding: 0px 0px;color:#2991FF;text-decoration: underline;cursor:pointer">' + md.PPM + '</a></td><td><a class="downtime" style="padding: 0px 0px;color:#2991FF;text-decoration: underline;cursor:pointer">' + md.Downtime + '</a></td><td>' + md.McHrRate + '</td></tr>');
                            //	tableContain += ('<tr><td>' + md.MachineId + '</td><td>'+md.MachineDescription +'</td><td>' + md.OEE + '</td><td>' + md.AE + '</td><td>' + md.PE + '</td><td>' + md.QE +
                            //'</td></td><td><a class="accepted" style="padding: 0px 0px;color:#2991FF;text-decoration: underline;cursor:pointer">' + md.Accepted + '</a></td><td><a class="downtime" style="padding: 0px 0px;color:#2991FF;text-decoration: underline;cursor:pointer">' + md.Downtime + '</a></td></tr>');
                        });
                        tableContain += '</tbody>';
                        $("#tblDashboardInfo").append(tableContain);
                    }
                    else {
                        $('#tblDashboardInfo').append('<tr><td colspan="9" style="text-align: center;">No record found for given search criteria !!</td></tr>');
                    }
                    bindTableFristCol();
                    BindSettingInfo();
                    fillcolumnswidth();
                    // $('.ajax-loader').hide();
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        $('#btnBack').click(function () {
            debugger;
            $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
            if ($('#hdnCurrentChart').val() == "Line") {
                BindDynamicLineDrillDownChart($('#hdnSelectedParam').val(), "", "", "", "", "", "", "comingFromBack", "");
            }
            else {
                BindDynamicColumnDayChart($('#hdnSelectedParam').val(), "", "", "", "", "", "", "comingFromBack", "");
            }
            $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
        });
        function BindDynamicColumnOEEChart(param) {
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetColumnOEEChartData",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + $("[id$=txtMonth]").val() + '", strDay:"' + $("[id$=txtDay]").val() + '", param:"' + param + '",strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + getComponentID() + '", employeeId:"' + getOperatorID() + '", cellId:"' + $("[id$=ddlCellId]").val() + '", SortColumn:"' + $("#SortColumn").val() + '", chartOrder:"' + $("#chartOrder").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = chartData = response.d;
                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    LoadColumnOEEChart(itmData, sizeLable, sizeText, param);
                    $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function BindDynamicColumnOEEChart_old(param) {
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetColumnOEEChartData_old",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + $("[id$=txtMonth]").val() + '", strDay:"' + $("[id$=txtDay]").val() + '", param:"' + param + '",strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + getComponentID() + '", employeeId:"' + getOperatorID() + '", cellId:"' + $("[id$=ddlCellId]").val() + '", SortColumn:"' + $("#SortColumn").val() + '", chartOrder:"' + $("#chartOrder").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = chartData = response.d;
                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    LoadColumnOEEChart_old(itmData, sizeLable, sizeText, param);
                    $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function BindDynamicLineOEEChart(param) {
            console.log("Param:-" + param);
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetLineOEEChartData",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + $("[id$=txtMonth]").val() + '", strDay:"' + $("[id$=txtDay]").val() + '", param:"' + param + '",strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + getComponentID() + '", employeeId:"' + getOperatorID() + '", cellId:"' + $("[id$=ddlCellId]").val() + '", SortColumn:"' + $("#SortColumn").val() + '", chartOrder:"' + $("#chartOrder").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;

                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    LoadLineOEEChart(itmData, sizeLable, sizeText, param);
                    $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function BindDynamicLineOEEChart_old(param) {
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetLineOEEChartData_old",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + $("[id$=txtMonth]").val() + '", strDay:"' + $("[id$=txtDay]").val() + '", param:"' + param + '",strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + getComponentID() + '", employeeId:"' + getOperatorID() + '", cellId:"' + $("[id$=ddlCellId]").val() + '", SortColumn:"' + $("#SortColumn").val() + '", chartOrder:"' + $("#chartOrder").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;

                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    LoadLineOEEChart_old(itmData, sizeLable, sizeText, param);
                    $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function LoadColumnOEEChart(data, sizeLable, sizeText, param) {
            debugger;
            var ytext = "";
            if (param == "DownTime") {
                ytext ="<%= GetLocalResourceObject("graphAsixDown")%> (" + $("#btnBarStopp").attr("timeText") + ")";
            }
            else if (param == "RejCount" || param == "ReworkPerformed") {
                ytext ="<%= GetLocalResourceObject("graphAsixValue")%>";
            }
            else
                ytext ="<%= GetLocalResourceObject("graphAsixPercentage")%>";
            var barheight = screen.availHeight - 575;

            if (barheight < 400) {
                barheight = 400;
            }
            var seriesData = [];
            for (var i = 0; i < data.series.length; i++) {
                seriesData.push(data.series[i]);
            }
            var maxCategories = 0;
            if (data.series.length > 0) {
                if (data.series[0].currentParam == "shift") {
                    maxCategories = 2;
                }
                else {
                    if (parseInt(data.series[0].data.length) > 29) {
                        maxCategories = 29;
                    }
                    else {
                        maxCategories = parseInt(data.series[0].data.length - 1);
                    }
                }
            }
            //maxCategories = ChartType == "Combined" ? maxCategories : parseInt(data.series[0].data.length - 1);
            var xlabel = "";
            var xtext = "";
            $('#hdnCurrentChart').val("Column");
            $('#hdnSelectedParam').val(param);
            $('#btnBack').css("visibility", data.series[0].btnVisible);
            if (data.series[0].btnVisible == "visible") {
                $('#btnBack').text("<< " + data.series[0].btnText);
            }
            console.log(ChartType);
            var chart = Highcharts.chart('divcolumnOEEchart', {
                chart: {
                    type: 'column',
                    height: barheight,
                },
                title: {
                    //text: data.XAxisTitle,
                    text: data.Title,
                    useHTML: true,
                    style: {
                        color: '#FFFFFF',
                        fontSize: sizeText,
                        fontFamily: 'Verdana, sans-serif',
                        'background-color': '#000000',
                        fontWeight: 'bold',
                    }
                },
                subtitle: {
                    text: ''
                },
                yAxis: {
                    //alternateGridColor: '#FDFFD5',
                    max: getYaxisMax(param),
                    title: {
                        text: ytext,
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: true
                },
                xAxis: {
                    title: {
                    },
                    type: 'category',
                    categories: data.series[0].Category,
                    //min: 0,
                    //scrollbar: {
                    //    enabled: ChartType == "Combined" ? true : false
                    //},
                    scrollbar: {
                        enabled:true
                    },
                    crosshair: true,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                    max: maxCategories,
                },
                tooltip: {
                    headerFormat: '<span style="font-size:15px"><b>{series.name}</b></span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        dataLabels: {
                            //rotation: -45,
                            //y: 10,
                            enabled: ChartType == "Combined" ? false : true,
                            format: '{point.y:.2f}',
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0);
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        },
                        turboThreshold: 1000000,
                        point: {
                            events: {
                                click: function (e) {
                                    debugger;
                                    //if (this.drilldown != null) {
                                    if (this.series.options.drilldown!=null) {
                                        if ($("[id$=txtYear]").val() == "") {
                                            alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                                            $("[id$=txtYear]").focus();
                                            return false;
                                        }
                                        var str = this.series.options.drilldown[e.point.x];
                                        var res = str.split("/");
                                        if (res[3] != undefined)
                                            window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                        else { $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' }); BindDynamicShiftChart(res[0], res[1], res[2], "column", this.color) }
                                        chart.title.attr({
                                            text: data.XAxisTitle + ' : ' + e.point.afterTitel
                                        });
                                    }
                                    else {
                                        debugger;
                                        let machine = this.series.options.machine;
                                        if (machine == "") {
                                            //machine = this.series.data[e.point.x].name;
                                            machine =e.point.category;
                                        }
                                        let month = this.series.options.month;

                                        let day = "";
                                        let current = this.series.options.currentParam;
                                        let next = this.series.options.nextParam;

                                        if (next == "shift") {
                                            //day = this.series.data[e.point.x].name;
                                            day = e.point.category;
                                            day = day.replace("Day ", "");
                                        }
                                        if (current == "month") { //year chart
                                            //month = this.series.data[e.point.x].name;
                                            month = e.point.category;
                                        }
                                        let prev = this.series.options.previousParam;
                                        if (next == "") {
                                            return;
                                        }
                                        $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
                                        var color = $(this.graphic.element).attr('fill');
                                        BindDynamicColumnDayChart(param, current, next, machine, month, day, prev, "", color);
                                    }
                                }
                               <%-- click: function () {
                                    var index = this.category;
                                    if (this.drilldown != null) {
                                        if ($("[id$=txtYear]").val() == "") {
                                            alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                                            $("[id$=txtYear]").focus();
                                            return false;
                                        }
                                        var str = this.drilldown;
                                        var res = str.split("/");
                                        if (res[3] != undefined)
                                            window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                        else { $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' }); BindDynamicShiftChart(res[0], res[1], res[2], "column", this.color) }
                                    }
                                }--%>
                            }
                        }
                    }
                },
                series: seriesData,
            });
            $("[id$=toggle]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
            $("[id$=liMenu]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
        }
        function LoadColumnOEEChart_old(data, sizeLable, sizeText, param) {
            $('#btnBack').css("visibility", "hidden");
            var ytext = "";
            if (param == "DownTime") {
                ytext ="<%= GetLocalResourceObject("graphAsixDown")%> (" + $("#btnBarStopp").attr("timeText") + ")";
            }
            else if (param == "RejCount" || param == "ReworkPerformed") {
                ytext ="<%= GetLocalResourceObject("graphAsixValue")%>";
            }
            else
                ytext ="<%= GetLocalResourceObject("graphAsixPercentage")%>";
            var barheight = screen.availHeight - 575;

            if (barheight < 400) {
                barheight = 400;
            }

            var xlabel = "";
            var xtext = "";
            var chart = Highcharts.chart('divcolumnOEEchart', {
                chart: {
                    type: 'column',
                    height: barheight,
                    events: {
                        drillup: function (e) {
                            //if(xlabel!=""){
                            //    xtext=  xlabel.split('(');
                            //    if(xtext.length>1){
                            //        
                            //        xtext=xtext[0];
                            //    }
                            //}

                            if (e.seriesOptions.id == undefined) {
                                xlabel = "";
                                countDrillDown = 0;
                                chart.title.attr({
                                    text: data.XAxisTitle
                                });
                            } else {
                                chart.title.attr({
                                    text: e.seriesOptions.data[0].beforeTitle
                                });
                            }
                        },
                        drilldown: function (e) {
                            //
                            countDrillDown++;
                            //alert(this.series[0].data[e.point.x].name);
                            //if (xlabel == "") {
                            //    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].afterTitel;
                            //}
                            //else{
                            //    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].afterTitel;
                            //}

                            chart.title.attr({
                                text: data.XAxisTitle + ' : ' + e.point.afterTitel//this.series[0].data[e.point.name].afterTitel
                            });



                            //chart.showLoading('Loading ...');
                            //setTimeout(function () {
                            //    chart.hideLoading();
                            //    chart.addSeriesAsDrilldown(e.point, series);
                            //}, 1000);
                            //
                        },

                    }
                },
                title: {
                    text: data.XAxisTitle,
                    useHTML: true,
                    style: {
                        color: '#FFFFFF',
                        fontSize: sizeText,
                        fontFamily: 'Verdana, sans-serif',
                        'background-color': '#000000',
                        fontWeight: 'bold',
                    }
                },
                subtitle: {
                    text: ''
                },
                yAxis: {
                    //alternateGridColor: '#FDFFD5',
                    max: getYaxisMax(param),
                    title: {
                        text: ytext,
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {

                            fontSize: sizeLable
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                xAxis: {
                    title: {
                        //  text: data.XAxisTitle,                       
                        //style: {
                        //    color: '#525151',
                        //    fontSize: sizeText,
                        //    fontFamily: 'Verdana, sans-serif',
                        //    fontWeight: 'bold'
                        //}
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                    type: 'category'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:15px"><b>{series.name}</b></span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        turboThreshold: 1000000,
                        dataLabels: {
                            //rotation: -45,
                            //y: 10,
                            enabled: true,
                            format: '{point.y:.2f}',
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0);
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        },
                        point: {
                            events: {
                                click: function () {
                                    var index = this.category;
                                    if (this.drilldown != null) {
                                        if ($("[id$=txtYear]").val() == "") {
                                            alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                                            $("[id$=txtYear]").focus();
                                            return false;
                                        }
                                        var str = this.drilldown;
                                        var res = str.split("/");
                                        if (res[3] != undefined)
                                            window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                        else { $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' }); BindDynamicShiftChart_old(res[0], res[1], res[2], "column", this.color) }
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
            $("[id$=toggle]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
            $("[id$=liMenu]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
        }

        var countDD = 0;
        function BindDynamicShiftChart(machineId, month, day, type, color) {
            debugger;
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("Dashboard.aspx/GetShiftChartData") %>',///"Dashboard.aspx/GetShiftChartData",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + month + '", strDay:"' + day + '", param:"' + $("#hdfChartName").val() + '", machineId:"' + machineId + '", type:"' + type + '", strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + getComponentID() + '", employeeId:"' + getOperatorID() + '", cellId:"' + $("[id$=ddlCellId]").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;

                    var param = $("#hdfChartName").val();
                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    LaodShiftWiseData(itmData, color, param, sizeLable);
                    countDD = 0;
                    $.unblockUI({});
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function BindDynamicShiftChart_old(machineId, month, day, type, color) {
            debugger;
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("Dashboard.aspx/GetShiftChartData_old") %>',///"Dashboard.aspx/GetShiftChartData",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + month + '", strDay:"' + day + '", param:"' + $("#hdfChartName").val() + '", machineId:"' + machineId + '", type:"' + type + '", strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + getComponentID() + '", employeeId:"' + getOperatorID() + '", cellId:"' + $("[id$=ddlCellId]").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;

                    var param = $("#hdfChartName").val();
                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    LaodShiftWiseData(itmData, color, param, sizeLable);
                    countDD = 0;
                    $.unblockUI({});
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function LaodShiftWiseData(data, color, param, sizeLable) {
            if (param == "DownTime") {
                ytext ="<%= GetLocalResourceObject("graphAsixDown")%> (" + $("#btnBarStopp").attr("timeText") + ")";
            } else if (param == "RejCount" || param == "ReworkPerformed") {
                ytext ="<%= GetLocalResourceObject("graphAsixValue")%>";
            }
            else
                ytext = "<%= GetLocalResourceObject("graphAsixPercentage")%>";
            var seriesData = [];
            for (var i = 0; i < data.series.length; i++) {
                seriesData.push(data.series[i]);
            }
            var maxCategories = 0;

            if (data.series[0].currentParam == "shift") {
                maxCategories = 2;
            }
            else {
                if (parseInt(data.series.length) > 1) {
                    maxCategories = 9;
                }
                else {
                    maxCategories = 25;
                }
            }
            var xlabel = "";
            var xtext = "";
           // $('#hdnCurrentChart').val("Column");
            $('#hdnSelectedParam').val(param);
            $('#btnBack').css("visibility", data.series[0].btnVisible);
            if (data.series[0].btnVisible == "visible") {
                $('#btnBack').text("<< " + data.series[0].btnText);
            }

            Highcharts.chart('divShiftChart', {
                //colors: [color],
                title: {
                    text: data.XAxisTitle,
                    useHTML: true,
                    style: {
                        color: '#FFFFFF',
                        fontSize: '15px',
                        fontFamily: 'Verdana, sans-serif',
                        'background-color': '#000000',
                        fontWeight: 'bold',
                    }
                },

                yAxis: {
                    //alternateGridColor: '#FDFFD5',
                    title: {
                        text: ytext,
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: true
                },
                xAxis: {
                    title: {
                        //text: data.XAxisTitle,
                        //style: {
                        //    color: '#525151',
                        //    fontSize: '15px',
                        //    fontFamily: 'Verdana, sans-serif',
                        //    fontWeight: 'bold'
                        //}
                    },
                    type: 'category',
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                    categories: data.series[0].Category
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            //enabled: ChartType == "Combined" ? false : true,
                            enabled: true,
                            format: '{point.y:.2f}',
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0);
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        },
                        turboThreshold: 1000000,
                        point: {
                            events: {
                                click: function (e) {
                                    debugger;
                                    var index = this.category;
                                    //if (this.drilldown != null) {
                                    if (this.series.options.drilldown != null) {
                                        if ($("[id$=txtYear]").val() == "") {
                                            alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                                            $("[id$=txtYear]").focus();
                                            return false;
                                        }
                                        //var str = this.drilldown;
                                        var str = this.series.options.drilldown[e.point.x];
                                        var res = str.split("/");
                                        window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                    }
                                }
                            }
                        }
                    }
                },
                series: seriesData
            });
            $("#myModal").modal('show');
        }
        function LaodShiftWiseData_old(data, color, param, sizeLable) {
            $('#btnBack').css("visibility", "hidden");
            if (param == "DownTime") {
                ytext ="<%= GetLocalResourceObject("graphAsixDown")%> (" + $("#btnBarStopp").attr("timeText") + ")";
            } else if (param == "RejCount" || param == "ReworkPerformed") {
                ytext ="<%= GetLocalResourceObject("graphAsixValue")%>";
            }
            else
                ytext ="<%= GetLocalResourceObject("graphAsixPercentage")%>";
            Highcharts.chart('divShiftChart', {
                colors: [color],
                title: {
                    text: data.XAxisTitle,
                    useHTML: true,
                    style: {
                        color: '#FFFFFF',
                        fontSize: '15px',
                        fontFamily: 'Verdana, sans-serif',
                        'background-color': '#000000',
                        fontWeight: 'bold',
                    }
                },

                yAxis: {
                    //alternateGridColor: '#FDFFD5',
                    title: {
                        text: ytext,
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                xAxis: {
                    title: {
                        //text: data.XAxisTitle,
                        //style: {
                        //    color: '#525151',
                        //    fontSize: '15px',
                        //    fontFamily: 'Verdana, sans-serif',
                        //    fontWeight: 'bold'
                        //}
                    },
                    type: 'category',
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        turboThreshold: 1000000,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.2f}',
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0);
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        },
                        point: {
                            events: {
                                click: function () {
                                    var index = this.category;
                                    if (this.drilldown != null) {
                                        if ($("[id$=txtYear]").val() == "") {
                                            alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                                            $("[id$=txtYear]").focus();
                                            return false;
                                        }
                                        var str = this.drilldown;
                                        var res = str.split("/");
                                        window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                    }
                                }
                            }
                        }
                    }
                },
                series: data.series
            });
            $("#myModal").modal('show');
        }
        function LoadLineOEEChart(data, sizeLable, sizeText, param) {
            debugger;
            var ytext = "";
            if (param == "DownTime")
                ytext ="<%= GetLocalResourceObject("graphAsixDown")%> (" + $("#btnBarStopp").attr("timeText") + ")";
            else if (param == "RejCount" || param == "ReworkPerformed") {
                ytext ="<%= GetLocalResourceObject("graphAsixValue")%>";
            }
            else
                ytext ="<%= GetLocalResourceObject("graphAsixPercentage")%>";
            //
            var lineheight = screen.availHeight - 575;

            if (lineheight < 400) {
                lineheight = 400;
            }
            var seriesData = [];
            for (var i = 0; i < data.series.length; i++) {
                seriesData.push(data.series[i]);
            }
            var maxCategories = 0;
            if (data.series.length > 0) {
                if (data.series[0].currentParam == "shift") {
                    maxCategories = 2;
                }
                else {
                    if (parseInt(data.series[0].data.length) > 29) {
                        maxCategories = 29;
                    }
                    else {
                        maxCategories = parseInt(data.series[0].data.length - 1);
                    }
                }
                $('#btnBack').css("visibility", data.series[0].btnVisible);
                if (data.series[0].btnVisible == "visible") {
                    $('#btnBack').text("<< " + data.series[0].btnText);
                }
            }
            //maxCategories = ChartType == "Combined" ? maxCategories : parseInt(data.series[0].data.length - 1);
            var xlabel = "";
            var xtext = "";
            $('#hdnCurrentChart').val("Line");
            $('#hdnSelectedParam').val(param);


            var chart = Highcharts.chart('divcolumnOEEchart', {
                chart: {
                    type: 'line',
                    height: lineheight,
                },
                title: {
                    text: data.Title,
                    useHTML: true,
                    style: {
                        color: '#FFFFFF',
                        fontSize: sizeText,
                        fontFamily: 'Verdana, sans-serif',
                        'background-color': '#000000',
                        fontWeight: 'bold',
                    }
                },
                yAxis: {
                    //alternateGridColor: '#FDFFD5',
                    max: getYaxisMax(param),
                    title: {
                        text: ytext,
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: true
                },
                xAxis: {
                    title: {
                        //text: data.XAxisTitle,
                        //style: {
                        //    color: '#525151',
                        //    fontSize: sizeText,
                        //    fontFamily: 'Verdana, sans-serif',
                        //    fontWeight: 'bold'
                        //}
                    },
                    min: 0,
                    max: maxCategories,
                    //scrollbar: {
                    //    enabled: ChartType == "Combined" ? true : false
                    //},
                    scrollbar: {
                        enabled: true
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable

                        }
                    },
                    categories: data.series[0].Category,
                    type: 'category'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px"><b>{series.name}</b></span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        turboThreshold: 1000000,
                        dataLabels: {
                            //rotation: -90,
                            //y: 10,
                            enabled: ChartType == "Combined" ? false : true,
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0);
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        },
                        point: {
                            events: {
                                click: function (e) {
                                    debugger;
                                    //if (this.drilldown != null) {
                                    if (this.series.options.drilldown != null) {
                                        if ($("[id$=txtYear]").val() == "") {
                                            alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                                            $("[id$=txtYear]").focus();
                                            return false;
                                        }
                                        //var str = this.series.options.drilldown;
                                        var str = this.series.options.drilldown[e.point.x];
                                        var res = str.split("/");
                                        if (res[3] != undefined)
                                            window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                        else { $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' }); BindDynamicShiftChart(res[0], res[1], res[2], "column", this.color) }
                                        chart.title.attr({
                                            text: data.XAxisTitle + ' : ' + e.point.afterTitel
                                        });
                                    }
                                    else {
                                        debugger;
                                        let machine = this.series.options.machine;
                                        if (machine == "") {
                                            //machine = this.series.data[e.point.x].name;
                                            machine = e.point.category;
                                        }
                                        let month = this.series.options.month;

                                        let day = "";
                                        let current = this.series.options.currentParam;
                                        let next = this.series.options.nextParam;

                                        if (next == "shift") {
                                            //day = this.series.data[e.point.x].name;
                                            day = e.point.category;
                                            day = day.replace("Day ", "");
                                        }
                                        if (current == "month") { //year chart
                                            //month = this.series.data[e.point.x].name;
                                            month = e.point.category;
                                        }
                                        let prev = this.series.options.previousParam;
                                        if (next == "") {
                                            return;
                                        }
                                        $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
                                        var color = $(this.graphic.element).attr('fill');
                                        BindDynamicLineDrillDownChart(param, current, next, machine, month, day, prev, "", color);
                                    }
                                }
                            }
                        }
                    }
                },
                series: seriesData,
                drilldown: {
                    series: data.drilldown,
                }
            });

            $("[id$=toggle]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
            $("[id$=liMenu]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
        }
        function LoadLineOEEChart_old(data, sizeLable, sizeText, param) {
            $('#btnBack').css("visibility", "hidden");
            var ytext = "";
            if (param == "DownTime")
                ytext ="<%= GetLocalResourceObject("graphAsixDown")%> (" + $("#btnBarStopp").attr("timeText") + ")";
            else if (param == "RejCount" || param == "ReworkPerformed") {
                ytext ="<%= GetLocalResourceObject("graphAsixValue")%>";
            }
            else
                ytext ="<%= GetLocalResourceObject("graphAsixPercentage")%>";
            //
            var lineheight = screen.availHeight - 575;

            if (lineheight < 400) {
                lineheight = 400;
            }
            var xlabel = "";
            var xtext = "";
            var chart = Highcharts.chart('divcolumnOEEchart', {
                chart: {
                    type: 'line',
                    height: lineheight,
                    events: {
                        drillup: function (e) {
                            //
                            if (e.seriesOptions.id == undefined) {
                                xlabel = "";
                                countDrillDown = 0;
                                chart.title.attr({
                                    text: data.XAxisTitle
                                });
                            } else {
                                chart.title.attr({
                                    text: e.seriesOptions.data[0].beforeTitle
                                });
                            }
                        },
                        drilldown: function (e) {
                            //alert(this.series[0].data[e.point.x].name);
                            countDrillDown++;
                            //if (xlabel == "") {
                            //    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].afterTitel;
                            //}
                            //else{
                            //    xlabel = data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].afterTitel;
                            //}
                            chart.title.attr({
                                text: data.XAxisTitle + ' : ' + e.point.afterTitel//this.series[0].data[e.point.x].afterTitel
                            });
                        },
                    }
                },
                title: {
                    text: data.XAxisTitle,
                    useHTML: true,
                    style: {
                        color: '#FFFFFF',
                        fontSize: sizeText,
                        fontFamily: 'Verdana, sans-serif',
                        'background-color': '#000000',
                        fontWeight: 'bold',
                    }
                },
                yAxis: {
                    //alternateGridColor: '#FDFFD5',
                    max: getYaxisMax(param),
                    title: {
                        text: ytext,
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                xAxis: {
                    title: {
                        //text: data.XAxisTitle,
                        //style: {
                        //    color: '#525151',
                        //    fontSize: sizeText,
                        //    fontFamily: 'Verdana, sans-serif',
                        //    fontWeight: 'bold'
                        //}
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable

                        }
                    },
                    type: 'category'
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px"><b>{series.name}</b></span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        turboThreshold: 1000000,
                        dataLabels: {
                            //rotation: -90,
                            //y: 10,
                            format: '{point.y:.2f}',
                            enabled: true,
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0);
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        },
                        point: {
                            events: {
                                click: function () {
                                    var index = this.category;
                                    if (this.drilldown != null) {
                                        if ($("[id$=txtYear]").val() == "") {
                                            alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                                            $("[id$=txtYear]").focus();
                                            return false;
                                        }
                                        var str = this.drilldown;
                                        var res = str.split("/");
                                        if (res[3] != undefined)
                                            window.open("VDGScreen.aspx?machineId=" + res[0] + "&shiftId=" + res[3] + "&date=" + $("[id$=txtYear]").val() + '-' + res[1] + '-' + res[2], "MyTargetWindowName");
                                        else { $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' }); BindDynamicShiftChart_old(res[0], res[1], res[2], "line", this.color) }
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

            $("[id$=toggle]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
            $("[id$=liMenu]").click(function () {
                var chart = $('#divcolumnOEEchart').highcharts();
                chart.reflow();
            });
        }
        function BindDynamicColumnDayChart(param, currentStatus, nextStatus, machineid, month, day, prevStatus, btnStatus, color) {
            if (btnStatus == "comingFromBack") {
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetColumnChartDataFromSession",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        debugger;
                        var itmData = chartData = response.d;
                        globalChartData = itmData; //kkkk
                        var size =  <%= fontSize %>;
                        var sizeLable = (size - 3 + "px");
                        var sizeText = (size + "px");
                        LoadColumnOEEChart(itmData, sizeLable, sizeText, param);
                        $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }

                });
            }
            else {
                var plantid = "";
                var companyID = "";
                var empID = "";
                var cellId = "";
                if ($("[id$=ddlViewType]").val() == "PlantwiseView") {
                    plantid = machineid;
                    machineid = "";
                }
                else if ($("[id$=ddlViewType]").val() == "ComponentwiseView") {
                    companyID = machineid;
                    machineid = "";
                }
                else if ($("[id$=ddlViewType]").val() == "OperatorwiseView") {
                    empID = machineid;
                    machineid = "";
                }
                else if ($("[id$=ddlViewType]").val() == "CellWiseView") {
                    cellId = machineid;
                    machineid = "";
                }
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetColumnDayChartData",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantId:"' + plantid + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + $("[id$=txtMonth]").val() + '", strDay:"' + $("[id$=txtDay]").val() + '", param:"' + param + '",strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + companyID + '", employeeId:"' + empID + '", cellId:"' + cellId + '", SortColumn:"' + $("#SortColumn").val() + '", chartOrder:"' + $("#chartOrder").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '",machine:"' + machineid + '",currentParam:"' + currentStatus + '",nextParam:"' + nextStatus + '",selectedMonth:"' + month + '",selectedDay:"' + day + '",prevParam:"' + prevStatus + '",backBtnStatus:"' + btnStatus + '",columnColor:"' + color + '"}',
                    dataType: "json",
                    success: function (response) {
                        debugger;
                        var itmData = chartData = response.d;
                        globalChartData = itmData; //kkkk
                        var size =  <%= fontSize %>;
                        var sizeLable = (size - 3 + "px");
                        var sizeText = (size + "px");
                        LoadColumnOEEChart(itmData, sizeLable, sizeText, param);
                        $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }

                });
            }
        }

        function BindDynamicLineDrillDownChart(param, currentStatus, nextStatus, machineid, month, day, prevStatus, btnStatus, color) {
            debugger;
            console.log("Param:-" + param);
            if (btnStatus == "comingFromBack") {
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetColumnChartDataFromSession",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        debugger;
                        var itmData = chartData = response.d;
                        globalChartData = itmData; //kkkk
                        var size =  <%= fontSize %>;
                        var sizeLable = (size - 3 + "px");
                        var sizeText = (size + "px");
                        LoadLineOEEChart(itmData, sizeLable, sizeText, param);
                        $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }

                });
            }
            else {
                debugger;
                var plantid = "";
                var companyID = "";
                var empID = "";
                var cellId = "";
                if ($("[id$=ddlViewType]").val() == "PlantwiseView") {
                    plantid = machineid;
                    machineid = "";
                }
                else if ($("[id$=ddlViewType]").val() == "ComponentwiseView") {
                    companyID = machineid;
                    machineid = "";
                }
                else if ($("[id$=ddlViewType]").val() == "OperatorwiseView") {
                    empID = machineid;
                    machineid = "";
                }
                else if ($("[id$=ddlViewType]").val() == "CellWiseView") {
                    cellId = machineid;
                    machineid = "";
                }
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetLineDrillDownChartData",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantId:"' + plantid + '", strYear:"' + $("[id$=txtYear]").val() + '", strMonth:"' + $("[id$=txtMonth]").val() + '", strDay:"' + $("[id$=txtDay]").val() + '", param:"' + param + '",strShift:"' + $("[id$=ddlShift]").val() + '", componentId:"' + companyID + '", employeeId:"' + empID + '", cellId:"' + cellId + '", SortColumn:"' + $("#SortColumn").val() + '", chartOrder:"' + $("#chartOrder").val() + '",paramText:"' + $("#divParamValue").val() + '",viewType:"' + $("[id$=ddlViewType]").val() + '",machine:"' + machineid + '",currentParam:"' + currentStatus + '",nextParam:"' + nextStatus + '",selectedMonth:"' + month + '",selectedDay:"' + day + '",prevParam:"' + prevStatus + '",backBtnStatus:"' + btnStatus + '",columnColor:"' + color + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;

                        var size =  <%= fontSize %>;
                        var sizeLable = (size - 3 + "px");
                        var sizeText = (size + "px");
                        LoadLineOEEChart(itmData, sizeLable, sizeText, param);
                        $.unblockUI({}); $('.ajax-loader').hide();   //.css("visibility", "hidden");
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "SignIn.aspx";
                    }
                });
            }
        }
        function GetChartType() {
            debugger;
            var viewtype = $('[id$=ddlViewType]').val();
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetChartType",
                contentType: "application/json; charset=utf-8",
                data: '{viewtype:"' + viewtype+'"}',
                dataType: "json",
                success: function (response) {
                    debugger;
                    ChartType = response.d;
                    $("#hdnChartType").val(ChartType);
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function GetCombinedChartType() {
            debugger;
            var viewtype = $('[id$=ddlViewType]').val();
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/GetCombinedChartType",
                contentType: "application/json; charset=utf-8",
                data: '{viewtype:"' + viewtype + '"}',
                dataType: "json",
                success: function (response) {
                    debugger;
                    var result = response.d;
                    CombinedCharts = result.split(",");
                    if (ChartType == "Combined") {
                        debugger;
                        if (CombinedCharts.includes('OEE')) {
                            if (CombinedCharts.includes('AE')) {
                                $("#btnBarAE").css("color", "#f5a507");
                            }
                            if (CombinedCharts.includes('PE')) {
                                $("#btnBarPE").css("color", "#f5a507");
                            }
                            if (CombinedCharts.includes('QE')) {
                                $("#btnBarQE").css("color", "#f5a507");
                            }
                            if (CombinedCharts.includes('OEE')) {
                                $("#btnBarOEE").css("color", "#f5a507");
                            }
                        }
                        else {
                            $("#btnBarOEE").css("color", "#f5a507");
                        }
                    }
                    else {
                        $("#btnBarOEE").css("color", "#f5a507");
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        $(document).on("click", ".accepted", function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            var day;
            if ($("[id$=txtDay]").val() == "") day = "01";
            else day = $("[id$=txtDay]").val();

            var mon = $("[id$=txtMonth]").val();
            //var d = Date.parse(mon + "1, " + $("[id$=txtYear]").val() + "");
            //if (!isNaN(d)) {
            //    return new Date(d).getMonth() + 1;
            //}
            var ssss = "JanFebMarAprMayJunJulAugSepOctNovDec".indexOf(mon) / 3 + 1;
            var machineId = $(this).closest("tr").find("td").eq(0).html();
            var machinedescription = $(this).closest("tr").find("td").eq(1).html();
            //window.open("AcceptedPage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&date=" + $("[id$=txtYear]").val(), "MyTargetWindowName");
            PopupCenter("AcceptedPage.aspx?machineId=" + machineId + "&machinedescription=" + machinedescription + "&shiftId=" + $("[id$=ddlShift]").val() + "&year=" + $("[id$=txtYear]").val() + "&month=" + $("[id$=txtMonth]").val() + "&day=" + $("[id$=txtDay]").val() + "&componentId=" + getComponentID() + "&employeeId=" + getOperatorID() + "&plantId=" + $("[id$=ddlPlantId]").val() + "&CellId=" + $("[id$=ddlCellId]").val() + "&View=" + $("[id$=ddlViewType]").val(), "Acception Information", 1000, 600);
        });

        $(document).on("click", ".ppm", function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            var day;
            if ($("[id$=txtDay]").val() == "") day = "01";
            else day = $("[id$=txtDay]").val();

            var mon = $("[id$=txtMonth]").val();
            //var d = Date.parse(mon + "1, " + $("[id$=txtYear]").val() + "");
            //if (!isNaN(d)) {
            //    return new Date(d).getMonth() + 1;
            //}
            var ssss = "JanFebMarAprMayJunJulAugSepOctNovDec".indexOf(mon) / 3 + 1;
            var machineId = $(this).closest("tr").find("td").eq(0).html();
            var machinedescription = $(this).closest("tr").find("td").eq(1).html();
            //window.open("AcceptedPage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&date=" + $("[id$=txtYear]").val(), "MyTargetWindowName");
            PopupCenter("PPMPage.aspx?machineId=" + machineId + "&machinedescription=" + machinedescription + "&shiftId=" + $("[id$=ddlShift]").val() + "&year=" + $("[id$=txtYear]").val() + "&month=" + $("[id$=txtMonth]").val() + "&day=" + $("[id$=txtDay]").val() + "&componentId=" + getComponentID() + "&employeeId=" + getOperatorID() + "&plantId=" + $("[id$=ddlPlantId]").val() + "&View=" + $("[id$=ddlViewType]").val() + "&Cell=" + $("[id$=ddlCellId]").val(), "PPM", 1000, 600);
        });

        function PopupCenter(url, title, w, h) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            //width= parseInt(width)-20;
            //height=parseInt(height)-70;
            var newWindow = window.open(url, title, 'scrollbars=yes,toolbar=no,resizable=yes,width=' + width + ', height=' + height);
            //location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=no

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        $(document).on("click", ".rejected", function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            var machineId = $(this).closest("tr").find("td").eq(0).html();
            //window.open("RejectedPage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&year=" + $("[id$=txtYear]").val() + "&month=" + $("[id$=txtMonth]").val() + "&day=" + $("[id$=txtDay]").val(), "MyTargetWindowName");
            PopupCenter("RejectedPage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&year=" + $("[id$=txtYear]").val() + "&month=" + $("[id$=txtMonth]").val() + "&day=" + $("[id$=txtDay]").val() + "&componentId=" + getComponentID() + "&employeeId=" + getOperatorID() + "&plantId=" + $("[id$=ddlPlantId]").val() + "&CellId=" + $("[id$=ddlCellId]").val() + "&chartType=" + $("#divChartType").val() + "&View=" + $("[id$=ddlViewType]").val(), "Rejection Information", 1200, 800);
        });

        $(document).on("click", ".downtime", function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            var machineId = $(this).closest("tr").find("td").eq(0).html();
            //window.open("DowntimePage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&date=" + $("[id$=txtYear]").val(), "MyTargetWindowName");
            PopupCenter("DownTimePieChart.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&year=" + $("[id$=txtYear]").val() + "&month=" + $("[id$=txtMonth]").val() + "&day=" + $("[id$=txtDay]").val() + "&componentId=" + getComponentID() + "&employeeId=" + getOperatorID() + "&plantId=" + $("[id$=ddlPlantId]").val() + "&CellId=" + $("[id$=ddlCellId]").val() + "&chartType=" + $("#divChartType").val() + "&timeTex=" + $("#btnBarStopp").attr("timeText") + "&View=" + $("[id$=ddlViewType]").val(), "Downtime Information", 1200, 800);
        });

        $(document).on("click", "#rework", function () {
            if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            var chartName = $("#hdfChartName").val();
            var machineId = $(this).closest("tr").find("td").eq(0).html();
            //window.open("DowntimePage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&date=" + $("[id$=txtYear]").val(), "MyTargetWindowName");
            PopupCenter("ReworkPage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&year=" + $("[id$=txtYear]").val() + "&month=" + $("[id$=txtMonth]").val() + "&day=" + $("[id$=txtDay]").val() + "&componentId=" + getComponentID() + "&employeeId=" + getOperatorID() + "&plantId=" + $("[id$=ddlPlantId]").val() + "&CellID=" + $("[id$=ddlCellId]").val() + "&chartType=" + $("#divChartType").val() + "&View=" + $("[id$=ddlViewType]").val(), "Rework Information", 1200, 800);
        });

    </script>
</asp:Content>
