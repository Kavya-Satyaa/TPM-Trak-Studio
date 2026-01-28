<%@ Page Title="Historical Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AggregatedReports.aspx.cs" Inherits="Web_TPMTrakDashboard.AggregatedReports" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<link href="Scripts/MultiCheckBox/bootstrap-multiselect.css" rel="stylesheet" />--%>
    <%--<script src="Scripts/MultiCheckBox/bootstrap-multiselect.js"></script>--%>
    <!--Font Awesome (added because you use icons in your prepend/append)-->
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>

    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }

        #tblfilter tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #tblfilter tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

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

        .cssclass {
            min-width: 350px;
        }
    </style>
    <div class="row" style="text-align: center; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
        <asp:HiddenField ID="width" runat="server" />
        <asp:HiddenField ID="height" runat="server" />
    </div>
    <div class="row">
        <div class="col-md-9">
            <h1 class="text-center login-title commontd"><%=GetLocalResourceObject("Report") %></h1>
            <div class="account-wall">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-signin">
                        <asp:UpdatePanel ID="upadetPanalRepor" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnGenerate" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblfilter" class="table table-bordered table-striped" style="width: auto">
                                    <tr>
                                        <td>
                                            <b><%=GetLocalResourceObject("Report Type") %></b> </td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                <%--  <asp:ListItem Value="ProductionReportMachinewise" meta:resourcekey="ListItemResource1">Production Report - Machinewise</asp:ListItem>--%>
                                                <%--<asp:ListItem Value="DowntimeReport" meta:resourcekey="ListItemResource3">Downtime Report</asp:ListItem>
                                                <asp:ListItem Value="DailyRejectionReport">Daily Rejection Report</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trview">
                                        <td runat="server">
                                            <b>View</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlview" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlview_SelectedIndexChanged">
                                                <asp:ListItem Value="CycleTime" meta:resourcekey="ListItemViewResourceType3">Cycle Time</asp:ListItem>
                                                <asp:ListItem Value="MachiningTime" meta:resourcekey="ListItemViewResourceType4">Machining Time</asp:ListItem>
                                                <asp:ListItem Value="LoadUnLoadTime" meta:resourcekey="ListItemViewResourceType5">LoadUnload Time</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trType" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Type") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                <asp:ListItem Value="Hour">Hour</asp:ListItem>
                                                <asp:ListItem Value="Shift">Shift</asp:ListItem>
                                                <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                                <asp:ListItem Value="Month">Month</asp:ListItem>
                                                <asp:ListItem Value="Time-Consolidated">Time - Consolidated</asp:ListItem>
                                                <%-- <asp:ListItem Value="MachineDownTimeMatrix" meta:resourcekey="ListItemResourceType6">Machine DownTime Matrix</asp:ListItem>--%>
                                                <asp:ListItem Value="OEEReportKiswok">OEE Report</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trShift" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","Shift") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="select form-control" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                       <tr id="trShiftMultiSelect" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","Shift") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:ListBox ID="lbShiftMultiSelect" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trFormat" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Format") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlFormat" runat="server" CssClass="select form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlFormat_SelectedIndexChanged">
                                                <asp:ListItem Value="Format1CockpitDataReport" meta:resourcekey="ListItemResourceFormat1">Format - 1(Cockpit Data Report)</asp:ListItem>
                                                <asp:ListItem Value="Format1" meta:resourcekey="ListItemResourceFormat2">Format - 1</asp:ListItem>
                                                <asp:ListItem Value="Format1EXCEL" meta:resourcekey="ListItemResourceFormat3">Format - 1(EXCEL)</asp:ListItem>
                                                <asp:ListItem Value="OEEGraphicalReport" meta:resourcekey="ListItemResourceFormat4">OEE Graphical Report</asp:ListItem>
                                                <asp:ListItem Value="Format3" meta:resourcekey="ListItemResourceFormat5">Format - 3</asp:ListItem>
                                                <asp:ListItem Value="MachinewiseDownTimeDetails" meta:resourcekey="ListItemResourceType5">Machinewise Downtime Details</asp:ListItem>
                                                <asp:ListItem Value="TimeWise" meta:resourcekey="ListItemResourceType9">Time Wise (Format 1)</asp:ListItem>
                                                <asp:ListItem Value="TimeAndFreqWise" meta:resourcekey="ListItemResourceType19">Time & Freq Wise (Format 2)</asp:ListItem>
                                                <asp:ListItem Value="DownTimeFormat3" meta:resourcekey="ListItemResourceType19">Format 3</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trFromDate" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trToDate" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trfromdatetimeconsolidate" runat="server">
                                        <td style="min-width: 80px;" runat="server"><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>
                                        <td class="input-group" style="min-width: 220px;" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txttimeconsolidate_fromdate" runat="server" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trtodatetimeconsolidate" runat="server">
                                        <td runat="server"><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b></td>
                                        <td class="input-group" style="min-width: 220px;" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txttimeconsolidate_todate" runat="server" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%--  <tr id="trComponentId" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Component ID") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlComponentId" runat="server" CssClass="select form-control loadData" AutoPostBack="True" OnSelectedIndexChanged="ddlComponentId_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trOperationID" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Operation No.") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperationID" runat="server" CssClass="select form-control"></asp:DropDownList></td>
                                    </tr>--%>
                                    <tr id="trPlant" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","PlantID") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trCellId" runat="server" clientidmode="static">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","CellId") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlCellID" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:ListBox ID="lbCellID" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="lbCellID_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trMachine" runat="server" clientidmode="static">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlMachineId" runat="server" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged" CssClass="  form-control cssclass" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:ListBox ID="lbMachineID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" OnSelectedIndexChanged="lbMachineID_SelectedIndexChanged" AutoPostBack="true" ></asp:ListBox>
                                            <asp:ListBox ID="ddlMultiMachineId" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trShiftAll" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","Shift") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="drpShiftAll" runat="server" CssClass="  form-control cssclass" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trComponentId" runat="server">
                                        <td runat="server">
                                            <b>Component ID</b> </td>
                                        <td runat="server">
                                             <asp:TextBox runat="server" ID="txtComponent" placeholder="Component Search" />
                                            <asp:LinkButton ID="btnComponent" runat="server" CssClass="btn btn-primary" OnClick="btnComponent_Click">
														<span aria-hidden="true" class="glyphicon glyphicon-refresh"></span>
                                            </asp:LinkButton>
                                            <asp:DropDownList ID="ddlComponentId" runat="server" CssClass="select form-control loadData" OnSelectedIndexChanged="ddlComponentId_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:ListBox ID="ddlMultiComponentID" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="ddlMultiComponentID_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </td>

                                    </tr>
                                    <tr id="trOperationID" runat="server">
                                        <td runat="server">
                                            <b>Operation ID</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperationID" runat="server" CssClass="select form-control cssclass"></asp:DropDownList>
                                            <asp:ListBox ID="ddlMultiOperationID" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>

                                    <tr id="trOperatorID" runat="server">
                                        <td runat="server">
                                            <b>Operator ID</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperatorID" runat="server" CssClass="select form-control cssclass"></asp:DropDownList></td>
                                    </tr>
                                    <tr id="trOperatorName" runat="server">
                                        <td runat="server">
                                            <b>Operation Name</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperatorName" runat="server" CssClass="select form-control cssclass"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trMultiOperator" runat="server">
                                        <td runat="server">
                                            <b>Operator</b> </td>
                                        <td runat="server">
                                            <asp:ListBox ID="ddlMultiOperator" runat="server" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                                        </td>
                                    </tr>

                                    <tr id="trCycleTimeType" runat="server">
                                        <td runat="server">
                                            <b>Cycle Time</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlCycleTimeType" runat="server" CssClass="select form-control">
                                                <asp:ListItem Value="StdCycleTime" Text="Standard"></asp:ListItem>
                                                <asp:ListItem Value="AvgCycleTime" Text="Average"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trCategory" runat="server">
                                        <td runat="server">
                                            <b>Category</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlCatogery" runat="server" CssClass="select  form-control cssclass" OnSelectedIndexChanged="ddlCatogery_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trRejectionID" runat="server">
                                        <td runat="server">
                                            <b>Rejection ID</b> </td>
                                        <td runat="server">
                                            <asp:ListBox ID="ddlMultiRejectionID" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trDownId" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Down ID") %></b> </td>
                                        <td runat="server">
                                            <asp:ListBox ID="ddlMultiDownID" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            &nbsp&nbsp                                     
                                            <label class="checkbox" style="display: inline-block; margin-left: 15px;">
                                                <asp:CheckBox ID="chkExclude" runat="server" /><%=GetLocalResourceObject("Exclude") %></label>
                                        </td>
                                    </tr>
                                    <tr id="trBreakDown" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("BreakDown ID") %></b> </td>
                                        <td runat="server">
                                            <asp:ListBox ID="ddlMultiBreakDownID" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            &nbsp&nbsp                                     
                                            <label class="checkbox" style="display: inline-block; margin-left: 15px;">
                                                <asp:CheckBox ID="BreakDownchkExclude" runat="server" /><%=GetLocalResourceObject("ExcludeBreakDown") %></label>
                                        </td>
                                    </tr>
                                    <tr id="trSubsystem" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("SubsystemID") %></b> </td>
                                        <td runat="server">
                                            <asp:ListBox ID="ddlMultisubsystem" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            &nbsp&nbsp                                     
                                            <label class="checkbox" style="display: inline-block; margin-left: 15px;">
                                                <asp:CheckBox ID="CheckBox1" runat="server" /><%=GetLocalResourceObject("ExcludeSubsystem") %></label>
                                        </td>
                                    </tr>
                                    <tr id="trsubsystemoneorall" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("SubsystemID") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlsubsystem" runat="server" CssClass="  form-control cssclass" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trDownReason" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Show Top Down Reasons") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlTopDownReasons" runat="server" CssClass="select form-control cssclass">
                                                <asp:ListItem Value="1" meta:resourcekey="ListItemResourceReasons1">Top 1</asp:ListItem>
                                                <asp:ListItem Value="2" meta:resourcekey="ListItemResourceReasons2">Top 2</asp:ListItem>
                                                <asp:ListItem Value="3" meta:resourcekey="ListItemResourceReasons3">Top 3</asp:ListItem>
                                                <asp:ListItem Value="4" meta:resourcekey="ListItemResourceReasons4">Top 4</asp:ListItem>
                                                <asp:ListItem Value="5" meta:resourcekey="ListItemResourceReasons5">Top 5</asp:ListItem>
                                                <asp:ListItem Value="6" meta:resourcekey="ListItemResourceReasons6">Top 6</asp:ListItem>
                                                <asp:ListItem Value="7" meta:resourcekey="ListItemResourceReasons7">Top 7</asp:ListItem>
                                                <asp:ListItem Value="8" meta:resourcekey="ListItemResourceReasons8">Top 8</asp:ListItem>
                                                <asp:ListItem Value="9" meta:resourcekey="ListItemResourceReasons9">Top 9</asp:ListItem>
                                                <asp:ListItem Value="10" meta:resourcekey="ListItemResourceReasons10">Top 10</asp:ListItem>
                                                <asp:ListItem Value="11">Top 11</asp:ListItem>
                                                <asp:ListItem Value="12">Top 12</asp:ListItem>
                                                <asp:ListItem Value="13">Top 13</asp:ListItem>
                                                <asp:ListItem Value="14">Top 14</asp:ListItem>
                                                <asp:ListItem Value="15">Top 15</asp:ListItem>
                                                <asp:ListItem Value="16">Top 16</asp:ListItem>
                                                <asp:ListItem Value="17">Top 17</asp:ListItem>
                                                <asp:ListItem Value="18">Top 18</asp:ListItem>
                                                <asp:ListItem Value="19">Top 19</asp:ListItem>
                                                <asp:ListItem Value="20">Top 20</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trBreakdownReason" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Show Top Breakdown Reasons") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlTopBreakDownreason" runat="server" CssClass="select form-control cssclass">
                                                <asp:ListItem Value="1" meta:resourcekey="ListItemResourceReasons1">Top 1</asp:ListItem>
                                                <asp:ListItem Value="2" meta:resourcekey="ListItemResourceReasons2">Top 2</asp:ListItem>
                                                <asp:ListItem Value="3" meta:resourcekey="ListItemResourceReasons3">Top 3</asp:ListItem>
                                                <asp:ListItem Value="4" meta:resourcekey="ListItemResourceReasons4">Top 4</asp:ListItem>
                                                <asp:ListItem Value="5" meta:resourcekey="ListItemResourceReasons5">Top 5</asp:ListItem>
                                                <asp:ListItem Value="6" meta:resourcekey="ListItemResourceReasons6">Top 6</asp:ListItem>
                                                <asp:ListItem Value="7" meta:resourcekey="ListItemResourceReasons7">Top 7</asp:ListItem>
                                                <asp:ListItem Value="8" meta:resourcekey="ListItemResourceReasons8">Top 8</asp:ListItem>
                                                <asp:ListItem Value="9" meta:resourcekey="ListItemResourceReasons9">Top 9</asp:ListItem>
                                                <asp:ListItem Value="10" meta:resourcekey="ListItemResourceReasons10">Top 10</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trTimeFormat" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("TimeFormat") %></b></td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlTimeFormat" runat="server" CssClass="select form-control cssclass">
                                                <asp:ListItem Value="hh:mm:ss" meta:resourcekey="ListItemResourceTimeFormat1">hh:mm:ss</asp:ListItem>
                                                <asp:ListItem Value="hh" meta:resourcekey="ListItemResourceTimeFormat2">hh</asp:ListItem>
                                                <asp:ListItem Value="mm" meta:resourcekey="ListItemResourceTimeFormat3">mm</asp:ListItem>
                                                <asp:ListItem Value="ss" meta:resourcekey="ListItemResourceTimeFormat4">ss</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>

                                    <tr id="trNodeID" runat="server">
                                        <td runat="server">
                                            <b>Node-ID</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlNodeid" runat="server" CssClass="select form-control" />
                                        </td>
                                    </tr>
                                    <tr id="trmonthlydate" runat="server">
                                        <td><b>From Year-Month</b></td>
                                        <td>
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="tryearlywithweek" runat="server">
                                        <td><b>Select Year</b></td>
                                        <td>
                                            <asp:TextBox ID="txtYearforWeek" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" AutoPostBack="true" OnTextChanged="txtYearforWeek_TextChanged" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                            <asp:DropDownList runat="server" ID="ddlWeekOfYear" CssClass="select form-control" Style="width: 120px; display: inline;"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trmonthlytodate" runat="server">
                                        <td><b>To Year-Month</b></td>
                                        <td>
                                            <asp:TextBox ID="txttoyear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource2" Style="width: 70px; display: inline;"></asp:TextBox>
                                            <asp:TextBox ID="txttomonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource2" Style="width: 70px; display: inline;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trReportFormat" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Format") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlReportFormat" runat="server" CssClass="select form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlReportFormat_SelectedIndexChanged">
                                                <asp:ListItem Value="Format1" meta:resourcekey="ListItemResourceFormat6">Format - 1</asp:ListItem>
                                                <asp:ListItem Value="Format2" meta:resourcekey="ListItemResourceFormat6">Format - 2</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trEfficiency" runat="server">
                                        <td runat="server">
                                            <b>Efficiency</b>
                                        </td>
                                        <td runat="server">
                                            <asp:CheckBoxList runat="server" ID="cblEfficiency" RepeatDirection="Horizontal" CssClass="check-box-list">
                                                <asp:ListItem Text="AE" Value="AE"></asp:ListItem>
                                                <asp:ListItem Text="PE" Value="PE"></asp:ListItem>
                                                <asp:ListItem Text="QE" Value="QE"></asp:ListItem>
                                                <asp:ListItem Text="OE" Value="OE"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" meta:resourcekey="btnGenerateResource1" />
                                            <br />
                                            <asp:Label runat="server" ID="lblNOte" ForeColor="Red"></asp:Label>
                                            <%-- <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:CommanResource, btnCancel %>" CssClass="btn btn-primary" OnClick="btnCancel_Click" meta:resourcekey="btnCancelResource1" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Extra JavaScript/CSS added manually in "Settings" tab -->

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <!-- Include Date Range Picker -->
    <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <script type="text/javascript">
        var multiselctListExpand = false;
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        $(document).ready(function () {
            $.unblockUI({});
            $('[id$=ddlMultiOperator]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=txttoyear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYearforWeek]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txttomonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txttimeconsolidate_fromdate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txttimeconsolidate_todate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txttimeconsolidate_fromdate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_todate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txttimeconsolidate_todate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_fromdate]').data("DateTimePicker").maxDate(e.date);
            });
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiBreakDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultisubsystem]').multiselect({
                includeSelectAllOption: true 
            });
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiComponentID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiOperationID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiRejectionID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbShiftMultiSelect]').multiselect({
                includeSelectAllOption: true
            });
            setControls();
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());

            $(".loadData").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            setInterval(function () {
                Showhide();
            }, 1000);
        });
        function StayMultiselectedList() {
            multiselctListExpand = true;
        }
        function setControls() {
            $('[id$=lbMachineID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbCellID]').multiselect({
                includeSelectAllOption: true
            });
        }
        function stayMultiselectedList(param) {
            debugger;
            setControls();
            if (param == "cell") {
                $("#trCellId .btn-group").addClass('open');
            }
            if (param == "machine") {
                $("#trMachine .btn-group").addClass('open');
            }
        }

        function Alert(msg) {
            alert(msg);
        }
        function Showhide() {

          <%-- var value = '<%= Session["ReportGenerated"]%>' ;
            if(value =="Ended")
            {
                HideLoader();
            }
            else if(value =="Started")
            {
                ShowLoader();
            }--%>

            //var value ="";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/AggregatedReports.aspx/GetVal",
                data: '{}',
                dataType: "json",
                success: function (result) {

                    OnSuccess(result);
                },
                error: function (Result) {

                }
            });
           
        }
        function OnSuccess(result) {

            if (result.d == "Ended") {
                HideLoader();
            }
            else if (result.d == "Started") {
                ShowLoader();
            }
        }

        function ShowLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }

        function HideLoader() {

            $.unblockUI({});
        }

        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        function compareDates(date1, date2) {
            return invert(date1).localeCompare(invert(date2));
        }

        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }
        $(document).on("click", "[id$=btnGenerate]", function () {
            if ($("[id$=ddlReportType]").val() == null) {
                alert("<%=GetLocalResourceObject("PleaseSelectReportType")%>");
                $("[id$=ddlReportType]").focus();
                return false;
            }
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            debugger;
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            // date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
            if (from != undefined && to != undefined) {
                var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
                var dateCom = compareDates(from, to);
                if (dateCom == 1) {
                    alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
                var diffeval = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
                if ($("[id$=ddlReportType]").val() == "DailyRejectionReport") {
                    if (diffeval > 365) {
                        alert("Difference between to date and from date cannot be more than 365 days.");
                        return false;
                    }
                }
                else if ($("[id$=ddlReportType]").val() == "ProductionAnalysisReportRTPL" || $("[id$=ddlReportType]").val() == "IncentiveReport_RNGupta" ||
                    $("[id$=ddlReportType]").val() == "DailyProductionReport_RNGupta") {
                    if (diffeval > 30) {
                        alert("Difference between to date and from date cannot be more than 31 days.");
                        return false;
                    }
                }
            }
            
            //if (diffe > 31) {
            //    alert("Difference between to date and from date cannot be more than 31 days.");
            //    return false;
            //}

            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            setTimeout(function () {
                $.unblockUI({});
            }, 1000);


        });

        function messageNotOk() {

            Command: toastr["error"]("Try Again!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"

            }
        }
        function messageOk() {

            Command: toastr["success"]("Report Generated")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        function messageNodata() {

            Command: toastr["error"]("No Data Found!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"

            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $('[id$=ddlMultiOperator]').multiselect({
                includeSelectAllOption: true
            });
            if (multiselctListExpand) {
                $(".td-machineid .btn-group").addClass('open');
            } else {
                $(".td-machineid .btn-group").removeClass('open');
            }
            multiselctListExpand = false;
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());
            $('[id$=txttoyear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                orientation: "top",
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYearforWeek]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                orientation: "top",
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txttomonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txttimeconsolidate_fromdate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txttimeconsolidate_todate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txttimeconsolidate_fromdate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_todate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txttimeconsolidate_todate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_fromdate]').data("DateTimePicker").maxDate(e.date);
            });

            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiBreakDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultisubsystem]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiComponentID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiOperationID]').multiselect({
                includeSelectAllOption: true
            });
            $(".loadData").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            $('[id$=ddlMultiRejectionID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbShiftMultiSelect]').multiselect({
                includeSelectAllOption: true
            });
            setControls();
            function messageOk() {

                Command: toastr["success"]("Report Generated")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }

            function messageNotOk() {

                Command: toastr["error"]("Try Again!")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "2000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }
            function messageNodata() {

                Command: toastr["error"]("Try Again!")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "2000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
