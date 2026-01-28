<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlannedDownTime.aspx.cs" Inherits="Web_TPMTrakDashboard.PlannedDownTime" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <script src="PDTScript/jquery-3.3.1.js"></script>
 
    <script src="PDTScript/bootstrap-select.min.js"></script>
    <link href="PDTScript/bootstrap-select.min.css" rel="stylesheet" />--%>
    <%--    <script src="PDTScript/bootstrap-multiselect.js"></script>
    <link href="PDTScript/bootstrap-multiselect.css" rel="stylesheet" />--%>
    <%--   <script src="PDTScript/moment.js"></script>--%>
    <%--  <script src="PDTScript/bootstrap-datetimepicker.js"></script>
    <link href="PDTScript/bootstrap-datetimepicker.css" rel="stylesheet" />--%>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
       <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>

    <%--   <link href="PDTScript/style.css" rel="stylesheet" />--%>

    <style>
        * {
            box-sizing: border-box;
            padding: 0;
            margin: 0;
        }

        ::-webkit-scrollbar {
            width: 5px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 5px;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 7px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        a:hover {
            text-decoration: none;
        }
        /*body {
  font-family: 'Open Sans', sans-serif;
  color: #50555a;
  padding: 0;
}*/
        #iFilter, #iFilter2, #iFilter3 {
            color: #337AB7;
            cursor: pointer;
        }

            /*  tr {
            margin-top: 10px;
        }*/

            #iFilter:hover {
                color: darkblue;
            }

        nav {
            z-index: 9;
            border-bottom: 1px solid rgba(0, 0, 0, 0.1);
            color: white;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            padding: 20px 0;
            text-align: center;
        }

        .grid {
            width: 100%;
            color: black;
            border: none;
        }

            .grid tr th {
                border: none;
                text-align: left;
                height: 35px;
                padding: 0px 6px;
            }

            .grid tr {
                cursor: pointer;
            }

                .grid tr td {
                    color: black;
                    height: 34px;
                    font-size: 16px;
                    padding: 0px 6px;
                    white-space: nowrap;
                    border: none;
                    text-align: center;
                }

                .grid tr:first-child:hover {
                    background-color: #5D7B9D !important;
                }

                .grid tr:hover {
                    background-color: #ced8db !important;
                }

        .bg-color {
            background-color: #CCD5F0;
            transition-duration: .5s;
        }

        .text-color {
            color: #CCD5F0;
            transition-duration: .5s;
        }

        footer {
            padding: 40px 0;
            text-align: center;
            opacity: .33;
            color: white;
        }

        .wrapper {
            min-width: 600px;
            max-width: 100%;
            margin: 0 auto;
        }

        .tabs {
            display: table;
            table-layout: fixed;
            width: 100%;
            /*border-bottom: 5px solid #393E46;*/
            -webkit-transform: translateY(5px);
            transform: translateY(5px);
            background-color: #393E46;
        }

            .tabs > li {
                transition-duration: .25s;
                display: table-cell;
                list-style: none;
                text-align: center;
                padding: 20px 20px 25px 20px;
                position: relative;
                overflow: hidden;
                cursor: pointer;
                color: white;
                font-size: 18px;
                border: 0.1px solid #4a5656;
                border-top: none;
                border-bottom: none;
            }

                .tabs > li:before {
                    z-index: -1;
                    position: absolute;
                    content: "";
                    width: 100%;
                    height: 120%;
                    top: 0;
                    left: 0;
                    background-color: rgba(255, 255, 255, 0.3);
                    -webkit-transform: translateY(100%);
                    transform: translateY(100%);
                    transition-duration: .25s;
                    border-radius: 5px 5px 0 0;
                }

                .tabs > li:hover {
                    /*-webkit-transform: translateY(70%);
        transform: translateY(70%);*/
                    background-color: #05b0e4;
                }

                .tabs > li.active {
                    color: white;
                    /*background-color: #09176E;*/
                    background-color: #158eb3;
                    border: 1px solid #158eb3;
                }

                    .tabs > li.active:before {
                        transition-duration: .5s;
                        background-color: #00ADB5;
                        -webkit-transform: translateY(70%);
                        transform: translateY(70%);
                    }

        hr {
            margin-top: 5px;
            margin-bottom: 5px;
        }

        .tab__content {
            background-color: white;
            position: relative;
            width: 100%;
            border-radius: 5px;
        }

            .tab__content > li {
                width: 100%;
                position: absolute;
                top: 0;
                left: 0;
                display: none;
                list-style: none;
            }

                .tab__content > li .content__wrapper {
                    text-align: center;
                    border-radius: 5px;
                    width: 100%;
                    padding: 15px 0px 15px 0px;
                    background-color: white;
                }

        .multiselect-container {
            height: auto;
            max-height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 0px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .btn-group {
            margin-right: 10px;
        }

        /* table tr th {
            text-align: center;
        }*/

        .CentreHeader {
            text-align: center !important;
            margin-top: 14px;
        }

        .content__wrapper h2 {
            width: 100%;
            text-align: center;
            padding-bottom: 20px;
            font-weight: 300;
        }

        .content__wrapper img {
            width: 100%;
            height: auto;
            border-radius: 5px;
        }

        .closeButton {
            display: inline-block;
            font-size: 30px;
            color: red;
            vertical-align: middle;
            margin-right: 5px;
            cursor: pointer;
            text-align: center;
            height: auto;
        }

            .closeButton:hover {
                text-shadow: 0 0 3px red;
                display: inline-block;
                font-size: 30px;
                color: red;
                vertical-align: middle;
                margin-right: 5px;
                cursor: pointer;
                text-align: center;
                height: auto;
            }

        /*  .Drpstyle, .Txtstyle, .Optstyle, .chkstyle {
            font-family: Tahoma,Verdana,Times New Roman;
            font-size: 16pt;
            font-style: normal;
            color: #16387c;
        }
*/
        .CentreHeader {
            text-align: center !important;
            margin-top: 14px;
        }

        /*table tr th {
            text-align: center;
            height: 35px;
            font-size: 16px;
        }*/

        .headerFixer tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #05b0e4;
            color: black;
            text-align:center;
            font-size: 16px;
        }

        /*table tr td {
            height: 30px;
            font-size: 14px;
            text-align: center;
        }*/

        #divAllDownsGrid::-webkit-scrollbar {
            display: none;
        }

        #divAllDownsGrid {
            -ms-overflow-style: none;
        }

        /*.ui-datepicker .ui-datepicker-prev,
        .ui-datepicker .ui-datepicker-next {
            display: block;
        }*/

        .tab__content li.active {
            display: block !important;
        }
        /* .active {
            display: block !important;
        }*/

        .tblAction {
            background-color: white;
            /*padding:3px 5px;*/
        }

            .tblAction > tbody > tr td {
                padding: 2px 5px;
            }

        #gvHolidays tr td:first-child {
            padding-left: 0px;
        }

        #rejectedItemsContainer {
            height: 60vh;
            overflow: auto;
        }

            #rejectedItemsContainer table {
                width: 99%;
                margin: auto;
            }

                #rejectedItemsContainer table tr td {
                    border: 1px solid gray;
                    padding: 5px;
                    text-align: left;
                }

                #rejectedItemsContainer table tr th {
                    padding: 5px;
                    text-align: left;
                    background-color: #423986;
                    color: white;
                    position: sticky;
                    top: -1px;
                }
                #shiftFilterMachineTbl  button{
                    width:100px;
                    overflow:hidden;
                }
                .griContainer table tr:last-child td{
                    position:sticky;
                    bottom:0px;
                    background:#05b0e4;
                  
                }
                .griContainer table tr:last-child td a{
                      font-size:16px;
                      color:white;
                      font-weight:600;
                      padding:0px 0px 0px 8px;
                }
                 .griContainer table tr:last-child td span{
                      font-size:16px;
                      color:#082f59;
                      font-weight:600;
                      padding:0px 0px 0px 8px;
                }
                 #divShiftDownFilterContainer table tr td{
                     padding:0px;
                 }
                 .tab__content span{
                     color:black;
                     font-weight:500;
                     font-size:16px !important;
                 }
                 .tab__content .form-control{
                     color:black;
                     font-weight:500;
                 }
    </style>




    <div class="container-fluid" style="width: 95%; margin: auto; margin-top: 60px; height: 88vh">
        <div style="margin-top: 30px; margin-bottom: 10px; width: auto;">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hdnActiveTab" ClientIDMode="Static" />
                    <div runat="server" id="plantMachineFilterContainer">
                        <asp:Label runat="server" Text="Plant ID" Style="vertical-align: middle; font-size: 18px; display: inline-block; margin: auto; color: white" />
                        <span data-toggle="tooltip" title="Plant" style="display: inline-block; width: auto;">
                            <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control selectpicker" data-live-search-style="begins" data-live-search="true" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" Style="display: block !important">
                            </asp:DropDownList>
                        </span>
                      
                        <asp:Label ID="lblMachinename" runat="server" CssClass="Labelstyle" Text="Machine" Style="vertical-align: auto; font-size: 18px; display: inline-block; margin: auto; color: white"></asp:Label>
                        <asp:ListBox ID="ddlMachineIDs" runat="server" SelectionMode="Multiple" ToolTip="Machine!" ClientIDMode="Static" Width="300" Style="display: inline-block;" Rows="1"></asp:ListBox>
                        <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" Text="View" ClientIDMode="Static" Visible="false" CssClass="btn btn-info btn-sm displayCss" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlPlantID" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnDeleteWeeklyOffData" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnDeleteShiftDownData" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnDeleteAllDown" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <div style="display: none">
                    <asp:Button runat="server" ID="btnHodiday" OnClick="btnHodiday_Click" />
                    <asp:Button runat="server" ID="btnWeeklyOff" OnClick="btnWeeklyOff_Click" />
                    <asp:Button runat="server" ID="btnDailyDown" OnClick="btnDailyDown_Click" />
                    <asp:Button runat="server" ID="btnShiftDown" OnClick="btnShiftDown_Click" />
                    <asp:Button runat="server" ID="btnAllDown" OnClick="btnAllDown_Click" />
                </div>
                <section class="wrapper">
                    <ul class="tabs">
                        <li runat="server" clientidmode="static" id="tabHoliday" onclick="tabsClick('btnHodiday')">Holidays </li>
                        <li runat="server" clientidmode="static" id="tabWeeklyOff" onclick="tabsClick('btnWeeklyOff')">Weekly Offs</li>
                        <li runat="server" clientidmode="static" id="tabDailyDown" onclick="tabsClick('btnDailyDown')">Daily Downs</li>
                        <li runat="server" clientidmode="static" id="tabShiftDown" onclick="tabsClick('btnShiftDown')">Shift Downs</li>
                        <li runat="server" clientidmode="static" id="tabAllDown" onclick="tabsClick('btnAllDown')">All Downs</li>
                    </ul>

                    <ul class="tab__content">
                        <li runat="server" clientidmode="static" id="tabHolidayContent">

                            <div class="content__wrapper">
                                <div>
                                    <asp:Label runat="server" Text="From" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin: auto;" />
                                  <%--  <asp:DropDownList runat="server" ID="ddlFromYear" CssClass="form-control" Style="margin-left: 5px; display: inline-block; margin-right: 5px; width: auto;">
                                    </asp:DropDownList>--%>
                                     <asp:TextBox ID="txtFromYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip"  placeholder="Year"  Style="width: 70px; display: inline;"></asp:TextBox>
                                      
                                    <asp:Label runat="server" Text="To" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin: auto;" />
                                   <%-- <asp:DropDownList runat="server" ID="ddlToYear" CssClass="form-control" Style="margin-left: 5px; display: inline-block; margin-right: 5px; width: auto;">
                                    </asp:DropDownList>--%>
                                     <asp:TextBox ID="txtToYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip"  placeholder="Year"  Style="width: 70px; display: inline;"></asp:TextBox>
                                    <asp:Label runat="server" Text="Month" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin: auto;" />
                                    <asp:DropDownList runat="server" ID="ddlMonth" CssClass="form-control" Style="margin-left: 5px; display: inline-block; margin-right: 5px; width: auto;" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:Label runat="server" Text="Day" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin: auto;" />
                                    <asp:DropDownList runat="server" ID="ddlDay" CssClass="form-control" Style="margin-left: 5px; display: inline-block; margin-right: 5px; width: auto;">
                                    </asp:DropDownList>
                                    <asp:Label runat="server" Text="Reason" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin: auto;" />
                                    <input id="ddlReason" runat="server" class="form-control" style="display: inline-block; width: 200px; margin: auto;" onkeypress="return restrictSpecialCharacter(event);"  clientidmode="static">
                                    <%-- <datalist id="ddlHolidayReason">
                                        <option value="Republic day" />
                                        <option value="Independence day" />
                                        <option value="Christmas" />
                                    </datalist>--%>
                                    <asp:Button runat="server" ID="btnSaveHoliday" Text="Define PDT" CssClass="btn btn-primary" Style="display: inline-block; width: auto; margin: auto" OnClick="btnSaveHoliday_Click" OnClientClick="return showLoaderWithValidation('ddlReason');"  />
                                </div>
                            </div>
                            <table style="margin-top: 10px; border: none; width: 100%;" class="tblAction">
                                <tr>
                                    <td style="width: 42px; text-align: center; padding: 0px 0px 0px 10px;">
                                        <asp:CheckBox runat="server" ID="chkSelectAllHoliday" CssClass="chkstyle" OnCheckedChanged="chkSelectAll_CheckedChanged" ClientIDMode="Static" Visible="false" AutoPostBack="true" Onchange="return showLoader();" />
                                    </td>
                                    <td id="tdFilter" style="width: 30px;">
                                        <%-- <i class="glyphicon glyphicon-filter" id="iFilter" style="font-size: 18px" title="Filter" />--%>
                                        <asp:LinkButton runat="server" ID="lnkFilter" CssClass="glyphicon glyphicon-filter" Font-Size="18" ToolTip="Filter" OnClick="lnkFilter_Click" />
                                    </td>
                                    <td style="text-align: left;">
                                        <div id="divHolidayFilter" runat="server" clientidmode="static" style="">
                                            <asp:DropDownList runat="server" ID="ddlFilter" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; height: unset; width: auto;">
                                                <asp:ListItem Value="ByDate" Text="By Date" />
                                                <asp:ListItem Value="ByReason" Text="By Reason" />
                                                <asp:ListItem Value="ByMachine" Text="By Machine" />
                                            </asp:DropDownList>


                                            <%-- <asp:DropDownList runat="server" ID="ddlFilterByDate" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 10px; display: inline-block; height: 30px; width: 200px;">
                                                <asp:ListItem Text="01-Jan-2019" />
                                                <asp:ListItem Text="26-Jan-2019" />
                                                <asp:ListItem Text="15-Aug-2019" />
                                            </asp:DropDownList>--%>
                                            <div id="holidayDateContainer" runat="server" style="display: inline-block; vertical-align: middle">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDate" ClientIDMode="Static" Text="From Date" Style="font-size: 14px; vertical-align: middle; margin-left: 10px;" /></td>
                                                        <td>
                                                            <asp:TextBox ID="txtFilterByDateFrom" ClientIDMode="Static" runat="server" Style="margin-left: 10px; display: inline-block; height: 30px; width: 110px;" CssClass="form-control date2" AutoCompleteType="Disabled"></asp:TextBox></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text="To Date" Style="font-size: 14px; vertical-align: middle; margin-left: 10px;" /></td>
                                                        <td>
                                                            <asp:TextBox ID="txtFilterByDateTo" ClientIDMode="Static" runat="server" Style="margin-left: 10px; display: inline-block; height: 30px; width: 110px;" CssClass="form-control date2" AutoCompleteType="Disabled"></asp:TextBox></td>
                                                    </tr>

                                                </table>
                                            </div>

                                            <div id="holidayReasonContainer" runat="server" style="display: inline-block; vertical-align: middle">
                                                <asp:Label runat="server" ID="lblReason" Text="Reason" ClientIDMode="Static" Style="font-size: 14px; vertical-align: middle; margin-left: 10px;" />
                                                <asp:DropDownList runat="server" ID="ddlFilterByReason" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 10px; height: unset; display: inline-block;min-width:120px;width:auto">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="holidayMachineContainer" runat="server" style="display: inline-block; vertical-align: middle">
                                                <asp:Label runat="server" ID="lblMachine" Text="Machine" ClientIDMode="Static" Style="font-size: 14px; vertical-align: middle; margin-left: 10px;" />
                                                <asp:ListBox ID="ddlFilterByMachines" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Style="margin-left: 10px; height: 30px; width: 200px; display: inline-block;" Rows="1"></asp:ListBox>
                                            </div>

                                            <%--  <asp:DropDownList runat="server" ID="ddlFilterByMachine" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 10px; height: 30px; width: 200px;display: inline-block;">
                                            </asp:DropDownList>--%>
                                            <%-- <asp:ListBox ID="ddlFilterByMachines" runat="server" SelectionMode="Multiple" ClientIDMode="Static" Style="margin-left: 10px; height: 30px; width: 200px;display: inline-block;" Rows="1" ></asp:ListBox>--%>
                                        <asp:Button runat="server" ID="btnApplyHoliday" OnClick="btnApplyHoliday_Click" CssClass="btn btn-primary" ClientIDMode="Static" Text="View PDT" Width="100" Style="margin-left: 10px; display: inline-block; height: unset;" OnClientClick="return showLoader();"/>

                                        <asp:LinkButton runat="server" ID="lbtnReload" CssClass="glyphicon glyphicon-repeat" Font-Size="16" style="top:8px;" ToolTip="Reload" OnClick="lbtnReload_Click" />
                                        <asp:LinkButton runat="server" ID="lbtnDelete" CssClass="glyphicon glyphicon-trash" style="top:8px;" Font-Size="16" ToolTip="Delete" OnClientClick="return openDeleteConfirmModal('holidayListConfirmModal','gvHolidays','chkSelectHoliday')" />

                                            <%--  <i id="divFilterClose" title="close" class="closeButton" style="">&times;</i>--%>
                                        <asp:LinkButton runat="server" ID="lnkHolidayFilterClose" CssClass="closeButton" ToolTip="Close" Text="&times;" OnClick="lnkHolidayFilterClose_Click" />
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPopMsgHolidays" runat="server" Text="" Style="float: right; color: darkgray; font-size: 16px; font-weight: bold; font-style: oblique;" />
                                    </td>
                                </tr>
                            </table>

                            <div id="divHolidayGrid" style="background-color: white; overflow: auto; color: black;" class="grid griContainer" >
                                <asp:GridView runat="server" ID="gvHolidays" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" Width="100%" CssClass="grid headerFixer table-bordered" ClientIDMode="Static" BorderStyle="NotSet" AllowPaging="true" OnPageIndexChanging="gvHolidays_PageIndexChanging" OnPreRender="gvHolidays_PreRender" PageSize="1000">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelectHoliday" ClientIDMode="Static" onclick="checkclick(this)" Checked='<%# Eval("CkdValue") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Left" ForeColor="Black" />
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnDate" Value='<%# Eval("Date") %>' />
                                                <asp:Label runat="server" ID="lblHoliday" Text='<%# Eval("Holiday") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Down Reason">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblReason" Text='<%# Eval("Reason") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Machine">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblMachineId" Text='<%# Eval("MachineID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="Black" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                </asp:GridView>
                            </div>

                        </li>
                        <li runat="server" clientidmode="static" id="tabWeeklyOffContent">

                            <div class="content__wrapper">

                                <table style="margin: auto; width: auto;">
                                    <tr>

                                        <td>
                                            <asp:Label runat="server" Text="From" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" /></td>
                                        <td class="input-group">

                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtFromWeeklyOffs" CssClass="form-control date" Style="min-width: 130px;" AutoCompleteType="Disabled" ClientIDMode="Static" />

                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="To" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;margin-left:10px" />
                                        </td>
                                        <td class="input-group" style="width: 200px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtToWeeklyOffs" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                        <td>
                                            <asp:Label runat="server" Text="Days" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                            <asp:ListBox ID="ddlDays" runat="server" SelectionMode="Multiple" ToolTip="Days" ClientIDMode="Static" Style="display: inline-block; width: auto; margin-right: 10px;">
                                                <asp:ListItem Text="Sun" />
                                                <asp:ListItem Text="Mon" />
                                                <asp:ListItem Text="Tue" />
                                                <asp:ListItem Text="Wed" />
                                                <asp:ListItem Text="Thu" />
                                                <asp:ListItem Text="Fri" />
                                                <asp:ListItem Text="Sat" />
                                            </asp:ListBox>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Week" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                            <asp:ListBox ID="ddlWeek" runat="server" SelectionMode="Multiple" ToolTip="Week" ClientIDMode="Static" Style="display: inline-block; width: auto;">
                                                <asp:ListItem Value="1" Text="1st Week" />
                                                <asp:ListItem Value="2" Text="2nd Week" />
                                                <asp:ListItem Value="3" Text="3rd Week" />
                                                <asp:ListItem Value="4" Text="4th Week" />
                                                <asp:ListItem Value="5" Text="5th Week" />
                                            </asp:ListBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtWeeklyOffReason" Style="display: inline-block; width: 200px; margin: auto;" CssClass="form-control" onkeypress="return restrictSpecialCharacter(event);" ClientIDMode="Static" > </asp:TextBox>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnSaveWeekOffs" Text="Define PDT" CssClass="btn btn-primary" Style="display: inline-block; width: auto; margin: auto" OnClick="btnSaveWeekOffs_Click"  OnClientClick="return showLoaderWithValidation('txtWeeklyOffReason');"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <table style="margin-top: 10px; border: none; width: 100%;" class="tblAction">
                                <tr>
                                    <td style="width: 42px; text-align: center; padding: 0px 0px 0px 10px;">
                                        <asp:CheckBox runat="server" ID="chkSelectAllWeekOffs"  OnCheckedChanged="chkSelectAll_CheckedChanged" CssClass="chkstyle" ClientIDMode="Static" Visible="false" AutoPostBack="true" Onchange="return showLoader();"/>
                                    </td>
                                    
                                    <td id="tdFilter2" style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lnkWeeklyFilter" CssClass="glyphicon glyphicon-filter" Font-Size="18" ToolTip="Filter" OnClick="lnkWeeklyFilter_Click" />
                                    </td>
                                    <td>
                                        <table id="divWeeklyFilter" runat="server" clientidmode="static" style="">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlWeeklyOffFilterType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlWeeklyOffFilterType_SelectedIndexChanged">
                                                        <asp:ListItem Value="ByDate" Text="By Date" />
                                                        <asp:ListItem Value="ByReason" Text="By Reason" />
                                                        <asp:ListItem Value="ByMachine" Text="By Machine" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <table id="weeklyOffFilterDateContainer" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" Text="From" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td class="input-group" style="border: 0; width: 200px; margin-right: 10px; vertical-align: middle; margin-top: 4px">
                                                                <div class="input-group-addon">
                                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                                </div>
                                                                <asp:TextBox runat="server" ID="txtFromWeekOffs" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" Text="To" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td class="input-group" style="border: 0; width: 200px; margin-right: 10px; vertical-align: middle">
                                                                <div class="input-group-addon">
                                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                                </div>
                                                                <asp:TextBox runat="server" ID="txtToWeekOffs" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table id="weeklyOffFilterMachineContainer" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" Text="Machine" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td>
                                                                <asp:ListBox ID="lbFilterWeekByMachine" runat="server" SelectionMode="Multiple" ToolTip="" ClientIDMode="Static" Style="display: inline-block; margin-right: 5px;"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table id="weeklyOffFilterReasonContainer" runat="server">
                                                        <tr>
                                                            <td>

                                                                <asp:Label runat="server" Text="Reason" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlWeeklyOffFilterReason" CssClass="form-control"  Style="margin-right: 5px; display: inline-block; vertical-align: super;min-width:120px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <%--  <td>
                                                    <asp:Label runat="server" Text="From" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                </td>
                                                <td class="input-group" style="border: 0; width: 200px; margin-right: 10px; vertical-align: middle; margin-top: 4px">
                                                    <div class="input-group-addon">
                                                        <i class="glyphicon glyphicon-calendar"></i>
                                                    </div>
                                                    <asp:TextBox runat="server" ID="txtFromWeekOffs" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Text="To" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                </td>
                                                <td class="input-group" style="border: 0; width: 200px; margin-right: 10px; vertical-align: middle">
                                                    <div class="input-group-addon">
                                                        <i class="glyphicon glyphicon-calendar"></i>
                                                    </div>
                                                    <asp:TextBox runat="server" ID="txtToWeekOffs" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Text="Machine" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="lbFilterWeekByMachine" runat="server" SelectionMode="Multiple" ToolTip="" ClientIDMode="Static" Style="display: inline-block; margin-right: 5px;"></asp:ListBox>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Text="Reason" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlWeeklyOffFilterReason" CssClass="form-control" Width="100" Style="margin-right: 5px; display: inline-block; vertical-align: super">
                                                    </asp:DropDownList>
                                                </td>--%>
                                                <td>
                                                    <asp:Button runat="server" ID="btnApplyWeekOffsFilter" CssClass="btn btn-primary" Text="View PDT" Width="100" Style="margin-left: 10px; display: inline-block; font-size: 14px;" OnClick="btnApplyWeekOffsFilter_Click" OnClientClick="return showLoader();"/>
                                                </td>
                                                <td id="tdRepeat2" style="width: 30px;">
                                                    <asp:LinkButton runat="server" ID="lbtnReloadWeekOffs" CssClass="glyphicon glyphicon-repeat" Font-Size="16" ToolTip="Reload" OnClick="lbtnReloadWeekOffs_Click" style="top:3px;" />
                                                </td>
                                                <td id="tdDelete2" style="width: 30px;">
                                                    <asp:LinkButton runat="server" ID="lbtnDeleteWeekOffs" CssClass="glyphicon glyphicon-trash" Font-Size="16" ToolTip="Delete" OnClientClick="return openDeleteConfirmModal('weeklyOffDeleteConfirmModal','gvWeekOffs','chkSelectWeekOffs')" style="top:3px;"/>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkWeeklyFilterClose" CssClass="closeButton" ToolTip="Close" Text="&times;" OnClick="lnkWeeklyFilterClose_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <%-- <asp:Label ID="Label1" runat="server" Text="Populated Till: 24-Nov-2019" Style="float: right; color: darkgray; font-size: 16px; font-weight: bold; font-style: oblique;" />--%>
                                    </td>
                                </tr>
                            </table>
                            <div id="divWeeklyOffsGrid" style="background-color: white; overflow: auto; height: 60vh" class="griContainer" >
                                <asp:GridView runat="server" ID="gvWeekOffs" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" Width="100%" CssClass="grid headerFixer table-bordered" ClientIDMode="Static" AllowPaging="true" OnPageIndexChanging="gvWeekOffs_PageIndexChanging" OnPreRender="gvWeekOffs_PreRender" PageSize="1000">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelectWeekOffs" onclick="checkclick(this);" Checked='<%# Eval("CkdValue") %>'/>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" ForeColor="Black" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID") %>' />
                                                <asp:Label runat="server" ID="lblWeekHoliday" Text='<%# Eval("FromDateTime") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Day">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDay" Text='<%# Eval("Day") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Down Reason">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblOffReason" Text='<%# Eval("Reason") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Machine">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnStartDate" Value='<%# Eval("FromDateTime1") %>' />
                                                <asp:HiddenField runat="server" ID="hdnEndDate" Value='<%# Eval("ToDateTime1") %>' />
                                                <asp:Label runat="server" ID="lblMachineId" Text='<%# Eval("MachineID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="Black" Width="20%"  />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </li>
                        <li runat="server" clientidmode="static" id="tabDailyDownContent">

                            <div class="content__wrapper">
                                <table style="margin: auto; width: auto;" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="From Date" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />

                                        </td>

                                        <td class="input-group" style="border: 0; width: 150px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtFromDailyDowns" CssClass="form-control date2" Style="min-width: 100px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="To Date" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />

                                        </td>
                                        <td class="input-group" style="border: 0; width: 150px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtToDailyDowns" CssClass="form-control date2" Style="min-width: 100px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="From Time" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />

                                        </td>

                                        <td class="input-group" style="border: 0; width: 150px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-time"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtFromTimeDailydown" CssClass="form-control date" Style="min-width: 100px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="To" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />

                                        </td>
                                        <td class="input-group" style="border: 0; width: 150px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-time"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtToTimeDailydown" CssClass="form-control date" Style="min-width: 100px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Reason" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />

                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDailyDownReason" CssClass="form-control" onkeypress="return restrictSpecialCharacter(event);" ClientIDMode="Static"></asp:TextBox>
                                            <%--    <input list="ddlDailyDownReason" name="ddlDailyDownReason"  runat="server" class="form-control" style="display: inline-block; width: 200px; margin-right: 10px;">
                                            <datalist id="ddlDailyDownReason">
                                                <option value="Tea Break">
                                                <option value="Lunch Break">
                                                <option value="Coffee Break">
                                                <option value="Snack Break">
                                                <option value="No Component">
                                            </datalist>--%>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnDailyDownSave" Text="Define PDT" CssClass="btn btn-primary" Style="width: 100%; margin-left: 10px" OnClick="btnDailyDownSave_Click"  OnClientClick="return showLoaderWithValidation('txtDailyDownReason');"/>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <table style="margin-top: 10px; border: none; width: 100%;" class="tblAction">
                                <tr>
                                    <td style="width: 42px; text-align: center; padding: 0px 0px 0px 10px;">
                                        <asp:CheckBox runat="server" ID="chkSelectAllDailyDowns" OnCheckedChanged="chkSelectAll_CheckedChanged"  CssClass="chkstyle" ClientIDMode="Static" Visible="false" AutoPostBack="true" Onchange="return showLoader();"/>
                                    </td>
                                   
                                    <td id="tdFilter3" style="width: 30px;">

                                        <asp:LinkButton runat="server" ID="lnkDailyDownFilter" CssClass="glyphicon glyphicon-filter" Font-Size="18" ToolTip="Filter" OnClick="lnkDailyDownFilter_Click" />
                                    </td>
                                    <td>

                                        <table id="divDailyDownFilter" style="" runat="server" clientidmode="static">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlDailyDownFilterType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDailyDownFilterType_SelectedIndexChanged">
                                                        <asp:ListItem Value="ByDate" Text="By Date" />
                                                        <asp:ListItem Value="ByReason" Text="By Reason" />
                                                        <asp:ListItem Value="ByMachine" Text="By Machine" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <table id="dailyDownFilterDateTbl" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblDailyDownsDateFrom" Text="From" Style="font-size: 14px; vertical-align: middle; margin-right: 5px; display: inline-block;" />
                                                            </td>
                                                            <td class="input-group" style="border: 0; width: 150px; margin-right: 5px; margin-top: 4px">
                                                                <div class="input-group-addon">
                                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                                </div>
                                                                <asp:TextBox runat="server" ID="txtFilterFromDailyDowns" CssClass="form-control date2" Style="min-width: 100px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                            </td>

                                                            <td>
                                                                <asp:Label runat="server" Text="To" Style="font-size: 14px; vertical-align: middle; margin-right: 5px; display: inline-block;" />
                                                            </td>
                                                            <td class="input-group" style="border: 0; width: 150px; margin-right: 5px; margin-top: 4px">
                                                                <div class="input-group-addon">
                                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                                </div>
                                                                <asp:TextBox runat="server" ID="txtFilterToDailyDowns" CssClass="form-control date2" Style="min-width: 100px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table id="dailyDownFilterMachineTbl" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" Text="Machine" Style="font-size: 14px; vertical-align: middle; margin-right: 5px; display: inline-block;" />
                                                            </td>
                                                            <td>
                                                                <asp:ListBox ID="ddlDailyDownFilterMachines" runat="server" SelectionMode="Multiple" ToolTip="" ClientIDMode="Static" Style="display: inline-block; margin-right: 5px;"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table id="dailyDownFilterReasonTbl" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" Text="Reason" Style="font-size: 14px; vertical-align: middle; margin-right: 5px; display: inline-block;" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlDailyDownsByReason" CssClass="form-control"  Style="margin-right: 5px; display: inline-block; vertical-align: super;min-width:120px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>



                                                <td>
                                                    <asp:Button runat="server" ID="btnDailyDownFilterAppy" CssClass="btn btn-primary" Text="View PDT" Width="100" Style="margin-right: 5px; display: inline-block;" OnClick="btnDailyDownFilterAppy_Click" OnClientClick="return showLoader();"/>
                                                 </td>
                                                 <td id="tdRepeat3" style="width: 30px;">
                                                    <asp:LinkButton runat="server" ID="lnkDailyDownsReload" CssClass="glyphicon glyphicon-repeat" Font-Size="16" ToolTip="Reload" OnClick="lnkDailyDownsReload_Click" style="top:3px;" />
                                                </td>
                                                <td id="tdDelete3" style="width: 30px;">
                                                    <asp:LinkButton runat="server" ID="lnkDailyDownDelete" CssClass="glyphicon glyphicon-trash" Font-Size="16" ToolTip="Delete" OnClientClick="return openDeleteConfirmModal('dailyDownDeleteConfirmModal','gvDailyDowns','chkSelectDailyDowns')" style="top:3px;" />
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkDailyDownFilterClose" CssClass="closeButton" ToolTip="Close" Text="&times;" OnClick="lnkDailyDownFilterClose_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <%-- <td>
                                        <asp:Label ID="lblPopMsgDailyDowns" runat="server" Text="Populated Till: 21-Nov-2019" Style="float: right; color: darkgray; font-size: 16px; font-weight: bold; font-style: oblique;" />
                                    </td>--%>
                                </tr>
                            </table>
                            <div id="divDailyDownsGrid" style="background-color: #EDEEF5; overflow: auto; height: 60vh" class="griContainer">
                                <asp:GridView runat="server" ID="gvDailyDowns" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" Width="100%" CssClass="grid headerFixer table-bordered" ClientIDMode="Static" AllowPaging="true" OnPageIndexChanging="gvDailyDowns_PageIndexChanging" OnPreRender="gvDailyDowns_PreRender" PageSize="1000">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID") %>' />
                                                <asp:CheckBox runat="server" ID="chkSelectDailyDowns" onclick="checkclick(this);" Checked='<%# Eval("CkdValue") %>'/>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start DateTime">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDailyHoliday" Text='<%# Eval("FromDateTime") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End DateTime">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBreakReason" Text='<%# Eval("ToDateTime") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Down Reason">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblMachineId" Text='<%# Eval("Reason") %>' />
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Machine">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblStartTime" Text='<%# Eval("MachineID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="Black" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="End Datetime">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEndTime" Text='<%# Eval("ToDateTime") %>' />
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="Black" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </li>
                        <li runat="server" clientidmode="static" id="tabShiftDownContent">

                            <div class="content__wrapper">
                                <table style="width: auto; margin: auto; padding-top: 15px;">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="From" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td class="input-group" style="border: 0; width: 200px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtFromShiftDowns" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="To" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td class="input-group" style="border: 0; width: 200px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtToShiftDowns" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Days" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                            <asp:ListBox ID="ddlOffDays" runat="server" SelectionMode="Multiple" ToolTip="Days" ClientIDMode="Static" Style="display: inline-block; width: auto; margin-right: 10px;">
                                                <asp:ListItem Text="Sun" />
                                                <asp:ListItem Text="Mon" />
                                                <asp:ListItem Text="Tue" />
                                                <asp:ListItem Text="Wed" />
                                                <asp:ListItem Text="Thu" />
                                                <asp:ListItem Text="Fri" />
                                                <asp:ListItem Text="Sat" />
                                            </asp:ListBox>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Shift" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td>
                                            <asp:ListBox runat="server" ID="ddlshiftsDowns" SelectionMode="Multiple" ClientIDMode="Static" Width="50" Style="display: inline-block; margin-right: 10px;"></asp:ListBox>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Down Reason" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px" />
                                        </td>
                                        <td>
                                            <%-- <input list="ddlShiftDownReason" name="ddlShiftDownReason" runat="server" class="form-control" style="display: inline-block; width: 160px; margin-right: 10px;">
                                            <datalist id="ddlShiftDownReason">
                                                <option value="Reason-sample01" />
                                                <option value="Reason-sample02" />
                                                <option value="Reason-sample03" />
                                            </datalist>--%>
                                            <asp:TextBox runat="server" ID="txtShiftDownReason" ClientIDMode="Static" CssClass="form-control" onkeypress="return restrictSpecialCharacter(event);" ></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                            <asp:Button runat="server" ID="btnSaveShiftDown" CssClass="btn btn-primary" Text="Define PDT" Style="margin-right: 10px; display: inline-block; width: auto;" OnClick="btnSaveShiftDown_Click"  OnClientClick="return showLoaderWithValidation('txtShiftDownReason');"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table style="margin-top: 10px; border: none; width: 100%;" class="tblAction">
                                <tr>
                                    <td style="width: 42px; text-align: center; padding: 0px 0px 0px 10px;">
                                        <asp:CheckBox runat="server" ID="chkAllShiftDown" OnCheckedChanged="chkSelectAll_CheckedChanged" CssClass="chkstyle" ClientIDMode="Static" Visible="false" AutoPostBack="true" Onchange="return showLoader();"/>
                                    </td>
                                   
                                    <td style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lnkShiftDownFilter" CssClass="glyphicon glyphicon-filter" Font-Size="18" ToolTip="Filter" OnClick="lnkShiftDownFilter_Click" />
                                    </td>
                                    <td>
                                        <div>
                                            <table id="divShiftDownFilterContainer" runat="server" clientidmode="static" style="">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlShiftFilterType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlShiftFilterType_SelectedIndexChanged" Style="width:auto">
                                                        <asp:ListItem Value="ByDate" Text="By Date" />
                                                        <asp:ListItem Value="ByReason" Text="By Reason" />
                                                        <asp:ListItem Value="ByMachine" Text="By Machine" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <table runat="server" id="shiftFilterDateTbl">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" Text="From" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td class="input-group" style="border: 0;  margin-right: 10px; vertical-align: middle; margin-top: 4px">
                                                                <div class="input-group-addon">
                                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                                </div>
                                                                <asp:TextBox runat="server" ID="txtFromShiftDownFilter" CssClass="form-control date2" Style="min-width: 100px;width:115px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" Text="To" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td class="input-group" style="border: 0;  margin-right: 10px; vertical-align: middle">
                                                                <div class="input-group-addon">
                                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                                </div>
                                                                <asp:TextBox runat="server" ID="txtToShiftDownFilter" CssClass="form-control date2" Style="min-width: 100px;width:115px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table runat="server" id="shiftFilterDayTbl">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" Text="Days" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td>
                                                                <asp:ListBox ID="lbDayShiftDownFilter" runat="server" SelectionMode="Multiple" ToolTip="Days" ClientIDMode="Static" Style="display: inline-block; width: auto; margin-right: 10px;">
                                                                    <asp:ListItem Text="Sun" />
                                                                    <asp:ListItem Text="Mon" />
                                                                    <asp:ListItem Text="Tue" />
                                                                    <asp:ListItem Text="Wed" />
                                                                    <asp:ListItem Text="Thu" />
                                                                    <asp:ListItem Text="Fri" />
                                                                    <asp:ListItem Text="Sat" />
                                                                </asp:ListBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" Text="Shift" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td>
                                                                <asp:ListBox runat="server" ID="lbShiftNameShiftDownFilter" SelectionMode="Multiple" ClientIDMode="Static" Width="50" Style="display: inline-block; margin-right: 10px;"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table runat="server" id="shiftFilterMachineTbl" clientidmode="static">
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" Text="Machine" Style="font-size: 14px; vertical-align: middle; margin-right: 10px;" />
                                                            </td>
                                                            <td>
                                                                <asp:ListBox ID="ddlShiftDownFilterMachines" runat="server" SelectionMode="Multiple" ToolTip="" ClientIDMode="Static" Style="display: inline-block; margin-right: 5px;"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table runat="server" id="shiftFilterReasonTbl">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlShiftFilterReason" CssClass="form-control"  Style="margin-right: 5px; display: inline-block; vertical-align: super;min-width:120px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>



                                                <td>
                                                    <asp:Button runat="server" ID="btnApplyShiftDownFilter" CssClass="btn btn-primary" Text="View PDT" Width="100" Style="margin-left: 10px; display: inline-block; font-size: 14px;" OnClick="btnApplyShiftDownFilter_Click" OnClientClick="return showLoader();"/>
                                                </td>
                                                 <td style="width: 30px;">
                                                    <asp:LinkButton runat="server" ID="lnkReloadShiftDown" CssClass="glyphicon glyphicon-repeat" Font-Size="16" ToolTip="Reload" OnClick="lnkReloadShiftDown_Click" style="top:3px;" />
                                                </td>
                                                <td style="width: 30px;">
                                                    <asp:LinkButton runat="server" ID="lnkShiftDownDelete" CssClass="glyphicon glyphicon-trash" Font-Size="16" ToolTip="Delete" OnClientClick="return openDeleteConfirmModal('shiftDownDeleteConfirmModal','gvMachineShift','chkSelectShiftDown')" style="top:3px;" />
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkShiftDownFilterClose" CssClass="closeButton" ToolTip="Close" Text="&times;" OnClick="lnkShiftDownFilterClose_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        </div>
                                        
                                    </td>
                                    <td>
                                        <%-- <asp:Label ID="Label1" runat="server" Text="Populated Till: 24-Nov-2019" Style="float: right; color: darkgray; font-size: 16px; font-weight: bold; font-style: oblique;" />--%>
                                    </td>
                                </tr>
                            </table>

                            <%--  <table style="width: 100%;" class="tblAction">
                                <tr>
                                    <td style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lbtnPrev" CssClass="glyphicon glyphicon-chevron-left" ToolTip="Prev" Style="margin-right: 5px; display: inline-block; width: auto; font-size: 28px; vertical-align: middle;" />
                                    </td>
                                    <td style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lbtnNext" CssClass="glyphicon glyphicon-chevron-right" ToolTip="Next" Style="margin-right: 5px; display: inline-block; width: auto; font-size: 28px; vertical-align: middle;" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPopMsgShiftDown" runat="server" Text="Populated Till: 05-Apr-2020" Style="float: right; color: darkgray; font-size: 16px; font-weight: bold; font-style: oblique; vertical-align: bottom; line-height: 28pt; height: 28px;" />
                                    </td>
                                </tr>
                            </table>--%>
                            <div id="gvMachineShiftContainer" style="overflow: auto; height: 60vh; background-color: white"  class="griContainer">
                                <asp:GridView runat="server" ID="gvMachineShift" AutoGenerateColumns="false" CssClass="grid headerFixer table-bordered" Style="text-align: center; width: 100%;" ShowFooter="false" ClientIDMode="Static" AllowPaging="true" OnPageIndexChanging="gvMachineShift_PageIndexChanging" OnPreRender="gvMachineShift_PreRender" PageSize="1000">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelectShiftDown" onclick="checkclick(this);" Checked='<%# Eval("CkdValue") %>'/>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Center" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Machine">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID") %>' />
                                                <asp:Label runat="server" ID="lblMachineId" Text='<%# Eval("MachineID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shift">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblShift" Text='<%# Eval("ShiftName") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Down Reason">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblShiftDownReason" Text='<%# Eval("Reason") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Day">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblShiftDay" Text='<%# Eval("Day") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" ForeColor="Black" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblShiftDate" Text='<%# Eval("FromDateTime") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" ForeColor="Black" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>

                            </div>

                        </li>
                        <li runat="server" clientidmode="static" id="tabAllDownContent">

                            <div class="content__wrapper">
                                <table style="margin: auto;">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="From" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td class="input-group" style="border: 0; width: 200px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtFromAllDowns" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="To" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td class="input-group" style="border: 0; width: 200px; margin-right: 10px;">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtToAllDowns" CssClass="form-control date2" Style="min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" />
                                        </td>
                                        <td style="display:none;">
                                            <asp:Label runat="server" Text="Plant" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td  style="display:none;">
                                            <asp:DropDownList runat="server" ID="ddlPlantAllDown" CssClass="form-control" Style="display: inline-block; width: auto; min-width: 50px; margin-right: 10px;" OnSelectedIndexChanged="ddlPlantAllDown_SelectedIndexChanged" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Machine" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlMachineAllDown" CssClass="form-control" Style="display: inline-block; width: auto; min-width: 50px; margin-right: 10px;" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Downs" Style="vertical-align: middle; font-size: 16px; display: inline-block; margin-right: 10px;" />
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAllDowns" CssClass="form-control" Style="display: inline-block; width: auto; min-width: 50px; margin-right: 10px;">
                                                <asp:ListItem Value="Holidays" Text="Holidays" />
                                                <asp:ListItem Value="WeeklyOff" Text="Weekly Offs" />
                                                <asp:ListItem Value="DailyDown" Text="Daily Downs" />
                                                <asp:ListItem Value="Shift" Text="Shift Downs" />
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnViewAllDowns" CssClass="btn btn-primary" Text="view" Style="margin-right: 10px; display: inline-block; width: auto;" OnClick="btnViewAllDowns_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table style="margin-top: 10px; border: none; width: 100%;" class="tblAction">
                                <tr>
                                    <td style="width: 50px; text-align: center;">
                                        <asp:CheckBox runat="server" ID="chkSelectAllDowns" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" CssClass="chkstyle" ClientIDMode="Static" Onchange="return showLoader();"/>
                                    </td>
                                    <td id="tdRepeat4" style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lnkAllDownReload" CssClass="glyphicon glyphicon-repeat" Font-Size="16" ToolTip="Reload" OnClick="lnkAllDownReload_Click" />
                                    </td>
                                    <td id="tdDelete4" style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lnkAllDownDelete" CssClass="glyphicon glyphicon-trash" Font-Size="16" ToolTip="Delete" OnClientClick="return openDeleteConfirmModal('allDownDeleteConfirmModal','gvAllDowns','chkSelectDowns')" />
                                    </td>
                                    <%--  <td style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lbtnPrevDowns" CssClass="glyphicon glyphicon-chevron-left" ToolTip="Prev" Style="margin-right: 5px; display: inline-block; width: auto; font-size: 26px; vertical-align: middle;" />
                                    </td>
                                    <td style="width: 30px;">
                                        <asp:LinkButton runat="server" ID="lbtnNextDowns" CssClass="glyphicon glyphicon-chevron-right" ToolTip="Next" Style="margin-right: 5px; display: inline-block; width: auto; font-size: 26px; vertical-align: middle;" />
                                    </td>--%>
                                    <td>
                                        <%-- <asp:Label ID="lblPopMsgAllDowns" runat="server" Text="Populated Till: 24-Nov-2019" Style="color: darkgray; font-size: 16px; font-weight: bold; font-style: oblique; vertical-align: bottom; line-height: 28pt; height: 28px; float: right;" />--%>
                                    </td>
                                </tr>
                            </table>
                            <div id="divAllDownsGrid" style="background-color: white; overflow: auto; height: 60vh" class="griContainer">
                                <asp:GridView runat="server" ID="gvAllDowns" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" Width="100%" CssClass="grid headerFixer table-bordered" ClientIDMode="Static" AllowPaging="true" OnPageIndexChanging="gvAllDowns_PageIndexChanging" OnPreRender="gvAllDowns_PreRender" PageSize="1000">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelectDowns" onclick="checkclick(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Holiday">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAllHolidays" Text='<%# Eval("FromDateTime") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Down Category">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAllDowns" Text='<%# Eval("DownType") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Down Reason">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAllReason" Text='<%# Eval("Reason") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" ForeColor="Black" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Machine">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID") %>' />
                                                <asp:Label runat="server" ID="lblAllMachineId" Text='<%# Eval("MachineID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="Black" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                </tr>
                            </table>

                        </li>
                    </ul>
                </section>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDeleteRecordsYes" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDeleteWeeklyOffData" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDeleteShiftDownData" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDeleteAllDown" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDeleteDailyDownData" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="chkSelectAllHoliday" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkSelectAllWeekOffs" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkSelectAllDailyDowns" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkAllShiftDown" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkSelectAllDowns" EventName="CheckedChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="holidayListConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to delete this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <asp:Button runat="server" Text="Yes" ID="btnDeleteRecordsYes" Style="width: 80px;" OnClick="btnDeleteRecordsYes_Click" OnClientClick="return closeDeletePopup();"/>
                    <input type="button" style="width: 80px;" value="No" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="weeklyOffDeleteConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to delete this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <asp:Button runat="server" Text="Yes" ID="btnDeleteWeeklyOffData" Style="width: 80px;" OnClick="btnDeleteWeeklyOffData_Click" OnClientClick="return closeDeletePopup();"/>
                    <input type="button" style="width: 80px;" value="No" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="rejectedItemsModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 65%">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Down Time already created for below list</h4>
                </div>
                <div class="modal-body">
                    <div id="rejectedItemsContainer">
                    </div>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <input type="button" style="width: 80px;" value="Close" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="shiftDownDeleteConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to delete this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <asp:Button runat="server" Text="Yes" ID="btnDeleteShiftDownData" Style="width: 80px;" OnClick="btnDeleteShiftDownData_Click" OnClientClick="return closeDeletePopup();"/>
                    <input type="button" style="width: 80px;" value="No" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="dailyDownDeleteConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to delete this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <asp:Button runat="server" Text="Yes" ID="btnDeleteDailyDownData" Style="width: 80px;" OnClick="btnDeleteDailyDownData_Click" OnClientClick="return closeDeletePopup();"/>
                    <input type="button" style="width: 80px;" value="No" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="allDownDeleteConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to delete this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <asp:Button runat="server" Text="Yes" ID="btnDeleteAllDown" Style="width: 80px;" OnClick="btnDeleteAllDown_Click" OnClientClick="return closeDeletePopup();"/>
                    <input type="button" style="width: 80px;" value="No" data-dismiss="modal" />
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
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black" id="warningMsg"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <input type="button" style="width: 80px;" value="OK" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <script>
         $('[id$=txtFromYear]').datepicker({
           minViewMode: 2,
           format: 'yyyy',
           todayHighlight: true,
           autoclose: true,
           language: 'en',
         });
         $('[id$=txtToYear]').datepicker({
           minViewMode: 2,
           format: 'yyyy',
           todayHighlight: true,
           autoclose: true,
           language: 'en',
        });

         function showLoaderWithValidation(txt) {
             debugger;
             if ($('#' + txt).val().trim() == "") {
                 openWarningModal("Please enter Down Reason.");
                 return false;
             }
            $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function showLoader() {
            debugger;
            $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function restrictSpecialCharacter(evt) {
        
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (pos == 0) {
                if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            if (charCode == 45 || charCode == 95) {
                return true;
            }
            if ((charCode >= 33 && charCode <= 47)) {
                return false;
            } else if ((charCode >= 58 && charCode <= 64)) {
                return false;
            } else if ((charCode >= 91 && charCode <= 96)) {
                return false;
            }
            return true;
        }
        function openWarningModal(msg) {
            $('#warningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        function closeDeletePopup() {
            $(".modal").modal("hide");
            showLoader();
        }
        function openConfirmModal() {
            debugger;
            let selectForDelete = false;
            for (let i = 1; i < $("#gvHolidays tr").length; i++) {
                let tr = $("#gvHolidays tr")[i];
                if ($(tr).find("#chkSelectHoliday").prop("checked")) {
                    selectForDelete = true;
                    break;
                }
            }
            if (selectForDelete) {
                $('[id*=holidayListConfirmModal]').modal('show');
            } else {
                openWarningModal("Select records for delete.");
            }
            return false;
        }
        function openDeleteConfirmModal(modalID, gridID, chkID) {
            //$('[id*=' + id + ']').modal('show');

            let selectForDelete = false;
            for (let i = 1; i < $("#" + gridID + " tr").length; i++) {
                let tr = $("#" + gridID + " tr")[i];
                if ($(tr).find("#" + chkID).prop("checked")) {
                    selectForDelete = true;
                    break;
                }
            }
            if (selectForDelete) {
                $('[id*=' + modalID + ']').modal('show');
            } else {
                openWarningModal("Select records for delete.");
            }

            return false;
        }
        function closeDeleteConfirmModal(id) {
            $('[id*=' + id + ']').modal('hide');
            return false;
        }
        function displayRejectedItems() {
            $.ajax({
                async: false,
                type: "POST",
                url: "PlannedDownTime.aspx/getPDTRejectedData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itemData = response.d;
                    $('#rejectedItemsContainer').empty();
                    if (itemData.length > 0) {
                        var appendString = "<table><tr><th>Start Time</th><th>End Time</th><th>Machine</th><th>Down Reason</th></tr>";
                        for (let i = 0; i < itemData.length; i++) {
                            appendString += '<tr><td>' + itemData[i].FromDateTime + '</td><td>' + itemData[i].ToDateTime + '</td><td>' + itemData[i].MachineID + '</td><td>' + itemData[i].Reason + '</td></tr>'
                        }
                        appendString += "</table>";
                        $('#rejectedItemsContainer').append(appendString);
                        $('[id*=rejectedItemsModal]').modal('show');
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }

        //--------------------- New -------------------------
        function tabsClick(btn) {
            debugger;
            if (btn == "btnHodiday") {
                __doPostBack('<%= btnHodiday.UniqueID%>', '');
            }
            else if (btn == "btnWeeklyOff") {
                __doPostBack('<%= btnWeeklyOff.UniqueID%>', '');
            }
            else if (btn == "btnDailyDown") {
                __doPostBack('<%= btnDailyDown.UniqueID%>', '');
            }
            else if (btn == "btnShiftDown") {
                __doPostBack('<%= btnShiftDown.UniqueID%>', '');
            }
            else if (btn == "btnAllDown") {
                __doPostBack('<%= btnAllDown.UniqueID%>', '');
            }
            $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
        }





        //--------------- New end ------------------

        $('[id$=txtFilterByDateFrom]').datetimepicker({
            format: 'DD-MM-YYYY',
            useCurrent: false,
            locale: 'en-US'
        });
        $('[id$=txtFilterByDateTo]').datetimepicker({
            format: 'DD-MM-YYYY',
            useCurrent: false,
            locale: 'en-US'
        });
        function clickAllcheck(obj) {
            debugger;
            var grid = null;
            if (obj == document.getElementById("chkSelectAllHoliday")) {
                grid = document.getElementById("gvHolidays");
            }
            else if (obj == document.getElementById("chkSelectAllWeekOffs")) {
                grid = document.getElementById("gvWeekOffs");
            }
            else if (obj == document.getElementById("chkSelectAllDailyDowns")) {
                grid = document.getElementById("gvDailyDowns");
            }
            else if (obj == document.getElementById("chkSelectAllDowns")) {
                grid = document.getElementById("gvAllDowns");
            }
            else if (obj == document.getElementById("chkAllShiftDown")) {
                grid = document.getElementById("gvMachineShift");
            }
            if (obj.checked) {
                if (grid.rows.length > 0) {
                    for (i = 1; i < grid.rows.length; i++) {
                        cell = grid.rows[i].cells[0];
                        for (j = 0; j < cell.childNodes.length; j++) {

                            if (cell.childNodes[j].type == "checkbox") {
                                cell.childNodes[j].checked = true;
                            }
                        }
                        grid.rows[i].style.backgroundColor = "#dbdde0";
                    }
                }
            }
            else {
                if (grid.rows.length > 0) {
                    for (i = 1; i < grid.rows.length; i++) {
                        cell = grid.rows[i].cells[0];
                        for (j = 0; j < cell.childNodes.length; j++) {
                            if (cell.childNodes[j].type == "checkbox") {
                                cell.childNodes[j].checked = false;
                            }
                        }
                        grid.rows[i].style.backgroundColor = "#EDEEF5";
                    }
                }
            }
        }
        function checkclick(obj) {
            debugger;
            var row = obj.parentNode.parentNode;
            if (obj.checked) {
                row.style.backgroundColor = "#dbdde0";
            }
            else {
                row.style.backgroundColor = "#EDEEF5";
            }
            var GridView = row.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var checked = true;
                if (inputList[i].type == "checkbox") {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            var selectAllCheckbox = null;
            if (GridView == document.getElementById("gvHolidays")) {
                selectAllCheckbox = document.getElementById("chkSelectAllHoliday");
            }
            else if (GridView == document.getElementById("gvWeekOffs")) {
                selectAllCheckbox = document.getElementById("chkSelectAllWeekOffs");
            }
            else if (GridView == document.getElementById("gvDailyDowns")) {
                selectAllCheckbox = document.getElementById("chkSelectAllDailyDowns");
            }
            else if (GridView == document.getElementById("gvMachineShift")) {
                selectAllCheckbox = document.getElementById("chkAllShiftDown");
            }
            else if (GridView == document.getElementById("gvAllDowns")) {
                selectAllCheckbox = document.getElementById("chkSelectAllDowns");
            }

            selectAllCheckbox.checked = checked;
        }
        function resize() {

            var heights = window.innerHeight - 370;
            document.getElementById("divHolidayGrid").style.height = heights + "px";
            //document.getElementById("divWeeklyOffsGrid").style.height = heights + "px";
            //document.getElementById("divDailyDownsGrid").style.height = heights + "px";
            //document.getElementById("divAllDownsGrid").style.height = heights + "px";
        }
        $(document).ready(function () {
            //var clickedTab = $(".tabs > .active");
            //var tabWrapper = $(".tab__content");
            //var activeTab = tabWrapper.find(".active");
            //var activeTabHeight = activeTab.outerHeight();

            //activeTab.show();

            //tabWrapper.height(activeTabHeight);

            //$(".tabs > li").on("click", function () {
            //    $(".tabs > li").removeClass("active");
            //    $(this).addClass("active");
            //    clickedTab = $(".tabs .active");
            //    activeTab.fadeOut(50, function () {
            //        $(".tab__content > li").removeClass("active");
            //        var clickedTabIndex = clickedTab.index();
            //        $(".tab__content > li").eq(clickedTabIndex).addClass("active");
            //        activeTab = $(".tab__content > .active");
            //        activeTabHeight = activeTab.outerHeight();
            //        tabWrapper.stop().delay(50).animate({
            //            height: activeTabHeight
            //        }, 200, function () {
            //            activeTab.delay(0).fadeIn(50);
            //        });
            //    });
            //});
            var d = new Date();
            console.log("date =" + d);

            tabsClick("btnHodiday");

            resize();
            window.onresize = function () {
                resize();
            };
            //$('#txtFromWeeklyOffs').datetimepicker();
            $('[id$=txtFromWeeklyOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                //useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=TextBox1]').datetimepicker({
                format: 'DD-MM-YYYY',
                //useCurrent: true,
                locale: 'en-US'
            });

            $('[id$=txtToWeeklyOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromTimeDailydown]').datetimepicker({
                format: 'HH:mm:ss',
                locale: 'en-US'
            });
            $('[id$=txtToTimeDailydown]').datetimepicker({
                format: 'HH:mm:ss',
                locale: 'en-US'
            });
            $('[id$=txtFromShiftDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromShiftDownFilter]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToShiftDownFilter]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });

            $('[id$=txtToShiftDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromAllDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToAllDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromWeekOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToWeekOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFilterFromDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFilterToDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });

            $('[id$=ddlMachineIDs]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlFilterByMachines]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=ddlshiftsDowns]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbShiftNameShiftDownFilter]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=ddlDays]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlWeek]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlDailyDownFilterMachines]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlShiftDownFilterMachines]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbFilterWeekByMachine]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlOffDays]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbDayShiftDownFilter]').multiselect({
                includeSelectAllOption: true
            });
            // document.getElementById("divFilter").style.display = "none";
            // document.getElementById("divFilter2").style.display = "none";
            // document.getElementById("divFilter3").style.display = "none";

            $('#gvHolidays tr:not(:last-child)').click(function (event) {
                debugger;
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);

            });
            $('#gvWeekOffs tr:not(:last-child)').on('click', function (event) {
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });
            $('#gvDailyDowns tr:not(:last-child)').on('click', function (event) {
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });
            $('#gvMachineShift tr:not(:last-child)').on('click', function (event) {
                debugger;
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });
            $('#gvAllDowns tr:not(:last-child)').on('click', function (event) {
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });

            //$("#iFilter").click(function () {
            //    document.getElementById("divFilter").style.display = "block";
            //});
            //$("#iFilter2").click(function () {
            //    document.getElementById("divFilter2").style.display = "block";
            //});
            //$("#iFilter3").click(function () {
            //    document.getElementById("divFilter3").style.display = "block";
            //});

            //$('[id$=ddlFilter]').change(function () {
            //    debugger;
            //    var selection = $('[id$=ddlFilter]').val();
            //    if (selection == "By Date") {
            //        document.getElementById("lblDate").style.display = "inline-block";
            //        document.getElementById("ddlFilterByDate").style.display = "inline-block";
            //        document.getElementById("lblReason").style.display = "none";
            //        document.getElementById('ddlFilterByReason').style.display = "none";
            //        document.getElementById("lblMachine").style.display = "none";
            //        document.getElementById('ddlFilterByMachine').style.display = "none";
            //    }
            //    else if (selection == "By Reason") {
            //        document.getElementById("lblDate").style.display = "none";
            //        document.getElementById('ddlFilterByDate').style.display = "none";
            //        document.getElementById("lblReason").style.display = "inline-block";
            //        document.getElementById('ddlFilterByReason').style.display = "inline-block";
            //        document.getElementById("lblMachine").style.display = "none";
            //        document.getElementById('ddlFilterByMachine').style.display = "none";
            //    }
            //    else {
            //        document.getElementById("lblDate").style.display = "none";
            //        document.getElementById('ddlFilterByDate').style.display = "none";
            //        document.getElementById("lblReason").style.display = "none";
            //        document.getElementById('ddlFilterByReason').style.display = "none";
            //        document.getElementById("lblMachine").style.display = "inline-block";
            //        document.getElementById('ddlFilterByMachine').style.display = "inline-block";
            //    }
            //});


            //$("#divFilterClose").click(function () {
            //    document.getElementById("divFilter").style.display = "none";
            //});
            //$("#divFilter2Close").click(function () {
            //    document.getElementById("divFilter2").style.display = "none";
            //});
            //$("#divFilter3Close").click(function () {
            //    document.getElementById("divFilter3").style.display = "none";
            //});
        });

        var bigDiv = document.getElementById('gvMachineShiftContainer');
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
         var bigDivAllDown = document.getElementById('divAllDownsGrid');
        bigDivAllDown.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivAllDown.scrollTop);
        }
         var bigDivDaily = document.getElementById('divDailyDownsGrid');
        bigDivDaily.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivDaily.scrollTop);
        }
         var bigDivWeekly = document.getElementById('divWeeklyOffsGrid');
        bigDivWeekly.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivWeekly.scrollTop);
        }
         var bigDivHoliday = document.getElementById('divHolidayGrid');
        bigDivHoliday.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivHoliday.scrollTop);
        }
        window.onload = function () {
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivAllDown.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivDaily.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivWeekly.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivHoliday.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        //function dateTimePicker() {
        //    $('.date1').datetimepicker({
        //        format: 'YYYY-MM-DD HH:mm:ss',
        //        locale: 'en-US'
        //    });
        //    $('.date2').datetimepicker({
        //        format: 'YYYY-MM-DD HH:mm:ss',
        //        useCurrent: false,
        //        locale: 'en-US'
        //    });
        //    $(".date1").on("dp.change", function (e) {
        //        $('.date2').data("DateTimePicker").minDate(e.date);
        //    });
        //    $(".date2").on("dp.change", function (e) {
        //        $('.date1').data("DateTimePicker").maxDate(e.date);
        //    });
        //}


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
             $('[id$=txtFromYear]').datepicker({
           minViewMode: 2,
           format: 'yyyy',
           todayHighlight: true,
           autoclose: true,
           language: 'en',
         });
         $('[id$=txtToYear]').datepicker({
           minViewMode: 2,
           format: 'yyyy',
           todayHighlight: true,
           autoclose: true,
           language: 'en',
         });
            var bigDiv = document.getElementById('gvMachineShiftContainer');
            var bigDivAllDown = document.getElementById('divAllDownsGrid');
            var bigDivDaily = document.getElementById('divDailyDownsGrid');
            var bigDivWeekly = document.getElementById('divWeeklyOffsGrid');
             var bigDivHoliday = document.getElementById('divHolidayGrid');
            $(document).ready(function () {
                $.unblockUI({}); $('.ajax-loader').hide();
                var d = new Date();
                console.log("date =" + d);
                //alert(d);
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                bigDivAllDown.scrollTop = $('[id*=hdnScrollPos]').val();
                bigDivDaily.scrollTop = $('[id*=hdnScrollPos]').val();
                bigDivWeekly.scrollTop = $('[id*=hdnScrollPos]').val();
                bigDivHoliday.scrollTop = $('[id*=hdnScrollPos]').val();
            });
              
              
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
       
        bigDivAllDown.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivAllDown.scrollTop);
        }
       
        bigDivDaily.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivDaily.scrollTop);
        }
         
        bigDivWeekly.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivWeekly.scrollTop);
        }
        
        bigDivHoliday.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDivHoliday.scrollTop);
        }
        window.onload = function () {
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivAllDown.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivDaily.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivWeekly.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDivHoliday.scrollTop = $('[id*=hdnScrollPos]').val();
        }
            function tabsClick(btn) {
                debugger;
                if (btn == "btnHodiday") {
                    __doPostBack('<%= btnHodiday.UniqueID%>', '');
                }
                else if (btn == "btnWeeklyOff") {
                    __doPostBack('<%= btnWeeklyOff.UniqueID%>', '');
                }
                else if (btn == "btnDailyDown") {
                    __doPostBack('<%= btnDailyDown.UniqueID%>', '');
                }
                else if (btn == "btnShiftDown") {
                    __doPostBack('<%= btnShiftDown.UniqueID%>', '');
                }
                else if (btn == "btnAllDown") {
                    __doPostBack('<%= btnAllDown.UniqueID%>', '');
                }
                $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
            }

            resize();
            window.onresize = function () {
                resize();
            };
            // document.getElementById("divFilter").style.display = "none";
            //  document.getElementById("divFilter2").style.display = "none";
            //   document.getElementById("divFilter3").style.display = "none";
            $('[id$=txtFromWeeklyOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtToWeeklyOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromShiftDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromShiftDownFilter]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToShiftDownFilter]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToShiftDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromAllDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToAllDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromWeekOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtToWeekOffs]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFilterFromDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFilterToDailyDowns]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: 'en-US'
            });
            $('[id$=txtFromTimeDailydown]').datetimepicker({
                format: 'HH:mm:ss',
                locale: 'en-US'
            });
            $('[id$=txtToTimeDailydown]').datetimepicker({
                format: 'HH:mm:ss',
                locale: 'en-US'
            });
            $('[id$=txtFilterByDateFrom]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: 'en-US'
            });
            $('[id$=txtFilterByDateTo]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: 'en-US'
            });
            //function dateTimePicker() {

            //    $('.date2').datetimepicker({
            //        format: 'YYYY-MM-DD HH:mm:ss',
            //        useCurrent: false,
            //        locale: 'en-US'
            //    });
            //    $(".date1").on("dp.change", function (e) {
            //        $('.date2').data("DateTimePicker").minDate(e.date);
            //    });
            //    $(".date2").on("dp.change", function (e) {
            //        $('.date1').data("DateTimePicker").maxDate(e.date);
            //    });
            //}
            //dateTimePicker();
            $('[id$=ddlMachineIDs]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlFilterByMachines]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlshiftsDowns]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbShiftNameShiftDownFilter]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlDays]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlWeek]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlDailyDownFilterMachines]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlShiftDownFilterMachines]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbFilterWeekByMachine]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=ddlOffDays]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbDayShiftDownFilter]').multiselect({
                includeSelectAllOption: true
            });

            //$("#iFilter").click(function () {
            //    document.getElementById("divFilter").style.display = "block";
            //});
            //$("#iFilter2").click(function () {
            //    document.getElementById("divFilter2").style.display = "block";
            //});
            //$("#iFilter3").click(function () {
            //    document.getElementById("divFilter3").style.display = "block";
            //});
            //$('[id$=ddlFilter]').change(function () {
            //    var selection = $('[id$=ddlFilter]').val();
            //    if (selection == "By Date") {
            //        document.getElementById("lblDate").style.display = "inline-block";
            //        document.getElementById("ddlFilterByDate").style.display = "inline-block";
            //        document.getElementById("lblReason").style.display = "none";
            //        document.getElementById('ddlFilterByReason').style.display = "none";
            //        document.getElementById("lblMachine").style.display = "none";
            //        document.getElementById('ddlFilterByMachine').style.display = "none";
            //    }
            //    else if (selection == "By Reason") {
            //        document.getElementById("lblDate").style.display = "none";
            //        document.getElementById('ddlFilterByDate').style.display = "none";
            //        document.getElementById("lblReason").style.display = "inline-block";
            //        document.getElementById('ddlFilterByReason').style.display = "inline-block";
            //        document.getElementById("lblMachine").style.display = "none";
            //        document.getElementById('ddlFilterByMachine').style.display = "none";
            //    }
            //    else {
            //        document.getElementById("lblDate").style.display = "none";
            //        document.getElementById('ddlFilterByDate').style.display = "none";
            //        document.getElementById("lblReason").style.display = "none";
            //        document.getElementById('ddlFilterByReason').style.display = "none";
            //        document.getElementById("lblMachine").style.display = "inline-block";
            //        document.getElementById('ddlFilterByMachine').style.display = "inline-block";
            //    }
            //});
            //$("#divFilterClose").click(function () {
            //    document.getElementById("divFilter").style.display = "none";
            //});
            //$("#divFilter2Close").click(function () {
            //    document.getElementById("divFilter2").style.display = "none";
            //});
            //$("#divFilter3Close").click(function () {
            //    document.getElementById("divFilter3").style.display = "none";
            //});
            $('#gvHolidays tr:not(:last-child)').click(function (event) {
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });
            $('#gvWeekOffs tr:not(:last-child)').on('click', function (event) {
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });
            $('#gvDailyDowns tr:not(:last-child)').on('click', function (event) {
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });
            $('#gvMachineShift tr:not(:last-child)').on('click', function (event) {
                 debugger;
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });
            $('#gvAllDowns tr:not(:last-child)').on('click', function (event) {
                if ($(event.target).is(':checkbox')) {
                    return;
                }
                var chkboxSelection = $(this).find('input:checkbox')[0].checked;
                if (chkboxSelection == true) {
                    $(this).closest('tr').css("background-color", "#EDEEF5");
                    $(this).find('input:checkbox')[0].checked = false;

                }
                else if (chkboxSelection == false) {
                    $(this).closest('tr').css("background-color", "#dbdde0");
                    $(this).find('input:checkbox')[0].checked = true;

                }
                checkclick($(this).find('input:checkbox')[0]);
            });


        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
