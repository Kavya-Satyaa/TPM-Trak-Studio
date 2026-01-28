<%@ Page Title="Report Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportForm.aspx.cs" Inherits="Web_TPMTrakDashboard.ReportForm" meta:resourcekey="PageResource1" %>

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
            min-width: 400px;
        }

        .cssclass2 {
            min-width: 200px;
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
                                <table id="tblfilter" class="table table-bordered table-striped" style="border: 2px solid #5b5656 !important; box-shadow: 0px 0px 4px white;">
                                    <tr>
                                        <td>
                                            <b><%=GetLocalResourceObject("Report Type") %></b> </td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" meta:resourcekey="ddlReportTypeResource1">
                                                <asp:ListItem Value="ProductionReportMachinewise" meta:resourcekey="ListItemResource1">Production Report - Machinewise</asp:ListItem>
                                                <asp:ListItem Value="ProductionReportComponentwise" meta:resourcekey="ListItemResource2">Production Report - Componentwise</asp:ListItem>
                                                <asp:ListItem Value="DowntimeReport" meta:resourcekey="ListItemResource3">Downtime Report</asp:ListItem>
                                                <asp:ListItem Value="ProductionAndDowntimeReportMachinewise" meta:resourcekey="ListItemResource4">Production and Downtime Report - Machinewise</asp:ListItem>
                                                <asp:ListItem Value="ProductionAndDowntimeReportDailyByHour">Production and Rejection Report - Daily By Hour</asp:ListItem>
                                                <asp:ListItem Value="DailyRejectionReport" meta:resourcekey="ListItemResource6">Daily Rejection Report </asp:ListItem>
                                                <asp:ListItem Value="HelpRequest" meta:resourcekey="ListItemResource7">Help-Request Report </asp:ListItem>
                                                <asp:ListItem Value="ShiftwiseOperatorPerformance" meta:resourcekey="ListItemResource8">Operator Performance Report - Shiftwise</asp:ListItem>

                                                <%-- <asp:ListItem Value="HelpRequest" meta:resourcekey="ListItemResource7">Help Request Report </asp:ListItem>
                                                <asp:ListItem Value="AVGToolChangeReport" meta:resourcekey="ListItemResource8">Avg ToolChange Report </asp:ListItem>--%>
                                                <%--<asp:ListItem Value="ToolChangeFrequencyReport" meta:resourcekey="ListItemResource6">Tool Change Frequency Report</asp:ListItem>--%>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr runat="server" id="trview">
                                        <td>
                                            <b>View</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlview" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlview_SelectedIndexChanged">
                                                <asp:ListItem Value="NodeView" meta:resourcekey="ListItemViewResourceType1">Node View</asp:ListItem>
                                                <asp:ListItem Value="MachineView" meta:resourcekey="ListItemViewResourceType2">Machine View</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trType" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Type") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                <asp:ListItem Value="Hour" meta:resourcekey="ListItemResourceType1">Hour</asp:ListItem>
                                                <asp:ListItem Value="Shift" meta:resourcekey="ListItemResourceType2">Shift</asp:ListItem>
                                                <asp:ListItem Value="Daily" meta:resourcekey="ListItemResourceType3">Daily</asp:ListItem>
                                                <asp:ListItem Value="Time-Consolidated" meta:resourcekey="ListItemResourceType4">Time - Consolidated</asp:ListItem>
                                                <asp:ListItem Value="MachinewiseDownTimeDetails" meta:resourcekey="ListItemResourceType5">Machinewise Downtime Details</asp:ListItem>
                                                <asp:ListItem Value="MachineDownTimeMatrix">Machine DownTime Matrix (Time & Freq wise)</asp:ListItem>
                                                <asp:ListItem Value="ProdAndDownPie" meta:resourcekey="ListItemResourceType7" Enabled="false">Production and Down Pie</asp:ListItem>
                                                <asp:ListItem Value="TimeWise" meta:resourcekey="ListItemResourceType9">Time Wise (Format 1)</asp:ListItem>
                                                <asp:ListItem Value="TimeAndFreqWise" meta:resourcekey="ListItemResourceType19">Machine DownTime Matrix</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trReportType" runat="server">
                                        <td runat="server" style="font-weight:bold;">Type</td>
                                        <td runat="server">
                                            <asp:DropDownList runat="server" ID="ddlTypee" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTypee_SelectedIndexChanged">
                                                <asp:ListItem Value="Machinewise" Text="Machinewise"></asp:ListItem>
                                                <asp:ListItem Value="Operatorwise" Text="Operatorwise"></asp:ListItem>
                                                <asp:ListItem Value="Particularwise" Text="Particularwise"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trShift" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","Shift") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="select form-control cssclass" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trFormat" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Format") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlFormat" runat="server" CssClass="select form-control cssclass" AutoPostBack="true" OnSelectedIndexChanged="ddlFormat_SelectedIndexChanged">
                                                <asp:ListItem Value="Format1CockpitDataReport" meta:resourcekey="ListItemResourceFormat1">Format - 1(Cockpit Data Report)</asp:ListItem>
                                                <asp:ListItem Value="Format1" meta:resourcekey="ListItemResourceFormat2">Format - 1</asp:ListItem>
                                                <asp:ListItem Value="Format1EXCEL" meta:resourcekey="ListItemResourceFormat3">Format - 1(EXCEL)</asp:ListItem>
                                                <asp:ListItem Value="OEEGraphicalReport" meta:resourcekey="ListItemResourceFormat4">OEE Graphical Report</asp:ListItem>
                                                <asp:ListItem Value="Format3" meta:resourcekey="ListItemResourceFormat5">Format - 3</asp:ListItem>
                                                <asp:ListItem Value="Format4" meta:resourcekey="ListItemResourceFormat6">Format - 4</asp:ListItem>
                                                <asp:ListItem Value="Format2" meta:resourcekey="ListItemResourceFormat7">Format - 2</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trFromDate" runat="server">
                                        <td>
                                            <%--<%=GetGlobalResourceObject('CommanResource','FromDate') %>--%>
                                            <b>
                                                <asp:Label runat="server" ID="lblFromDate" Text="From Date"></asp:Label></b> </td>
                                        <td class="input-group">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trToDate" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trfromdatetimeconsolidate" runat="server">
                                        <td style="min-width: 80px;"><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>
                                        <td class="input-group" style="min-width: 220px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txttimeconsolidate_fromdate" runat="server" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trtodatetimeconsolidate" runat="server">
                                        <td><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b></td>
                                        <td class="input-group" style="min-width: 220px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txttimeconsolidate_todate" runat="server" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trComponentId" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Component ID") %></b> </td>
                                        <td runat="server">
                                            <asp:TextBox runat="server" ID="txtComponent" placeholder="Component Search" />
                                            <asp:LinkButton ID="btnComponent" runat="server" CssClass="btn btn-primary" OnClick="btnComponent_Click">
														<span aria-hidden="true" class="glyphicon glyphicon-refresh"></span>
                                            </asp:LinkButton>
                                            <asp:DropDownList ID="ddlComponentId" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlComponentId_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trMoID" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("MOID") %></b> </td>
                                        <td runat="server">
                                            <asp:TextBox runat="server" ID="txtMOID" placeholder="MO No Search" Width="120px" />
                                            <asp:Button ID="btmMosearch" runat="server" CssClass="btn btn-primary" OnClick="btmMosearch_Click" Text="Like Search"></asp:Button>
                                            <asp:Button ID="btnMoExactSearch" runat="server" CssClass="btn btn-primary" OnClick="btnMoExactSearch_Click" Text="Exact search"></asp:Button>
                                            <asp:DropDownList ID="ddlMoWise" runat="server" CssClass="select form-control cssclass">
                                            </asp:DropDownList>

                                        </td>
                                    </tr>
                                     <tr id="trSerialNumber" runat="server">
                                        <td runat="server">
                                            <b>Serial No.</b> </td>
                                        <td runat="server">
                                            <asp:TextBox runat="server" ID="txtSlnoSearch" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 200px" placeholder="Contains search.."></asp:TextBox>&nbsp;&nbsp;
                                            <asp:LinkButton runat="server" ClientIDMode="Static" CssClass="glyphicon glyphicon-search" ID="lnkSlnoSearch" OnClick="lnkSlnoSearch_Click" Style="font-size: 18px; vertical-align: middle;"></asp:LinkButton>
                                            <asp:DropDownList ID="ddlSlno" runat="server" OnSelectedIndexChanged="ddlSlno_SelectedIndexChanged" AutoPostBack="true" CssClass="  form-control cssclass">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trOperationID" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Operation No.") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperationID" runat="server" CssClass="select form-control cssclass"></asp:DropDownList>
                                             <asp:ListBox ID="ddlMultiOperationID" runat="server" SelectionMode="Multiple" CssClass="form-control cssclass" ClientIDMode="Static"></asp:ListBox>
                                        </td>
                                    </tr>
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
                                     <tr id="trGroupID" runat="server" clientidmode="static">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","GroupID") %></b>
                                        </td>
                                        <td runat="server">
                                             <asp:DropDownList ID="ddlGroupID" ClientIDMode="Static" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupID_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trMachine" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlMachineId" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged" CssClass="  form-control cssclass">
                                            </asp:DropDownList>
                                            <asp:ListBox ID="lbMachineID" runat="server" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                                            <asp:ListBox ID="ddlMultiMachineId" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trOperator" runat="server">
                                        <td runat="server">
                                            <b>Operator</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperator" runat="server" CssClass="select form-control cssclass"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trMultiOperator" runat="server">
                                        <td runat="server">
                                            <b>Operator</b> </td>
                                        <td runat="server">
                                            <asp:ListBox ID="ddlMultiOperator" runat="server" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trMultiComponent" runat="server">
                                        <td runat="server">
                                            <b>Component ID</b>
                                        </td>
                                        <td runat="server">
                                            <asp:ListBox ID="lstComponentID" runat="server" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                                        </td>
                                    </tr>

                                    <tr id="trToolNumber" runat="server">
                                        <td runat="server">
                                            <b>Tool Number</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlToolNumber" runat="server" CssClass="  form-control cssclass">
                                            </asp:DropDownList>
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
                                    <tr id="trTimeFormat" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Down ID") %></b></td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlTimeFormat" runat="server" CssClass="select form-control cssclass">
                                                <asp:ListItem Value="hh:mm:ss" meta:resourcekey="ListItemResourceTimeFormat1">hh:mm:ss</asp:ListItem>
                                                <asp:ListItem Value="hh" meta:resourcekey="ListItemResourceTimeFormat2">hh</asp:ListItem>
                                                <asp:ListItem Value="mm" meta:resourcekey="ListItemResourceTimeFormat3">mm</asp:ListItem>
                                                <asp:ListItem Value="ss" meta:resourcekey="ListItemResourceTimeFormat4">ss</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trNodeID" runat="server">
                                        <td>
                                            <b>Node-ID</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlNodeid" runat="server" CssClass="select form-control cssclass" />
                                        </td>
                                    </tr>
                                    <tr id="trmonthlydate" runat="server">
                                        <%-- <td><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>--%>
                                        <td><b>Year-Month</b></td>
                                        <td>
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trYear" runat="server">
                                        <td><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>
                                        <td>
                                            <asp:TextBox ID="txtYearOnly" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtMonthResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trmivintype" runat="server" visible="false">
                                        <td>
                                            <b>Type</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlMivinType">
                                                <asp:ListItem Text="First Off Inspection" Value="FIR" />
                                                <asp:ListItem Text="Production monitoring" Value="SPC" />
                                                <asp:ListItem Text="Process Inspection Report" Value="PIR" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trSeperateSheet" runat="server" visible="false">
                                        <td>
                                            <b>Sheet</b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkSeperateSheet" runat="server" />Machinewise Sheet</label>
                                        </td>
                                    </tr>
                                    <tr id="trModel" runat="server" visible="false">
                                        <td>
                                            <b>Model</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlModel" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trProcessType" runat="server" visible="false">
                                        <td>
                                            <b>Process</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlProcess" CssClass="select form-control cssclass">
                                                <asp:ListItem Text="Casing" Value="Casing" />
                                                <asp:ListItem Text="MPV" Value="MPV" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trQRCode" runat="server" visible="false">
                                        <td>
                                            <b>Serial Number</b>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtQRCodeSearch" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="reportTypeForVulkanAM" runat="server">
                                        <td>Report Type</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rdnReportType" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="PDF" Value="PDF"></asp:ListItem>
                                                <asp:ListItem Text="Excel" Value="Excel" Selected="true"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                   <tr id="trrevid" runat="server">
                                       <td>Rev ID</td>
                                       <td><asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlRevID"></asp:DropDownList></td>
                                   </tr>
                                    <tr id="trHeatNo" runat="server">
                                        <td>Heat No</td>
                                        <td><asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlHeatNo"></asp:DropDownList></td>
                                    </tr>
                                    <tr id="trDieNo" runat="server">
                                        <td>Die No</td>
                                        <td> <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlDieNo"></asp:DropDownList></td>
                                    </tr>
                                    <tr id="trCheckpoints" runat="server">
                                        <td style="font-weight:bold;">Checklist</td>
                                        <td>
                                             <asp:ListBox ID="lbCheckpointID" runat="server" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                                            <%--<asp:DropDownList runat="server" ID="ddlCheckpointID" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" meta:resourcekey="btnGenerateResource1" />
                                            <%-- <asp:Button runat="server" ID="btnCancel" Text="<%$Resources:CommanResource, btnCancel %>" CssClass="btn btn-primary" OnClick="btnCancel_Click" />--%>
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
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        $(document).ready(function () {
            $.unblockUI({});
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYearOnly]').datepicker({
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
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=ddlMultiOperator]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiOperationID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbCheckpointID]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=lstComponentID]').multiselect({
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
                url: "ReportForm.aspx/GetVal",
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


        function messageOk() {
            Command: toastr["success"]("Report Generated successfully.")
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
            Command: toastr["error"]("Error generating report. Please Try Again!")
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

        function messageNodataformo() {

            Command: toastr["error"]("No Data Found For Selected MO!")
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
            debugger;
            if ($("[id$=ddlReportType]").val() == null) {
                alert("<%=GetLocalResourceObject("PleaseSelectReportType")%>");
                $("[id$=ddlReportType]").focus();
                return false;
            }
            if ($("[id$=txtFromDate]").val() == "") {

                if ($("[id$=ddlReportType]").val() == "MOWISEREPORT") {
                    if ($("[id$=ddlMoWise]").val() == "ALL") {
                        alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                        $("[id$=txtFromDate]").focus();
                        return false;
                    }
                }

                else {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }

            }
            if ($("[id$=txtToDate]").val() == "") {
                if ($("[id$=ddlReportType]").val() == "MOWISEREPORT") {
                    if ($("[id$=ddlMoWise]").val() == "ALL") {
                        alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                        $("[id$=txtToDate]").focus();
                        return false;
                    }
                }

                else {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }



            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            var dateInterval = 15;
            $.ajax({
                async: false,
                type: "POST",
                url: "ReportForm.aspx/getReportDateInterval",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    dateInterval = response.d;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            var dateIntervalValidation = dateInterval - 1;
            // date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
            var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
            if ($("[id$=ddlReportType]").val() == "MOWISEREPORT") {
                if ($("[id$=ddlMoWise]").val() == "ALL") {
                    if (diffe > dateIntervalValidation) {
                        alert("Difference between to date and from date cannot be more than " + dateInterval + " days.");
                        return false;
                    }
                }
            }
            else if ($("[id$=ddlReportType]").val() == "TrelleborgOEEReport" || $("[id$=ddlReportType]").val() == "HourwiseOperatorIncentiveReport" || $("[id$=ddlReportType]").val() == "HelpRequest" || $("[id$=ddlReportType]").val() == "MachineUtilizationReportACE") {
                if (diffe > dateIntervalValidation) {
                    alert("Difference between to date and from date cannot be more than " + dateInterval + " days.");
                    return false;
                }
            }
            else if ($("[id$=ddlReportType]").val() == "MachinewiseScrapReport") {
                if (diffe > 31) {
                    alert("Difference between to date and from date cannot be more than 31 days.");
                    return false;
                }
            }
            else if ($("[id$=ddlReportType]").val() == "ComponentSetupReport") {
                if (diffe > 180) {
                    alert("Difference between to date and from date cannot be more than 180 days.");
                    return false;
                }
            }
            else if ($("[id$=ddlReportType]").val() == "KKPillarReport") {
                if (diffe > 7) {
                    alert("Difference between to date and from date cannot be more than 7 days.");
                    return false;
                }
            }
            else {
                if (diffe > dateIntervalValidation) {
                    alert("Difference between to date and from date cannot be more than " + dateInterval + " days.");
                    return false;
                }
            }
            var dateCom = compareDates(from, to);
            if (dateCom == 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            setTimeout(function () {
                $.unblockUI({});
            }, 10000);
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYearOnly]').datepicker({
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
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiOperationID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiOperator]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbCheckpointID]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=lstComponentID]').multiselect({
                includeSelectAllOption: true
            });

            setControls();
            $(".loadData").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });



        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
