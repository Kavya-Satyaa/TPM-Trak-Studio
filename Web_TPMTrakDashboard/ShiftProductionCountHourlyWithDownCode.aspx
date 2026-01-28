<%@ Page Title="D" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShiftProductionCountHourlyWithDownCode.aspx.cs" Inherits="Web_TPMTrakDashboard.ShiftProductionCountHourlyWithDownCode" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }

        #tblfilter {
            margin-bottom: 12px;
        }

        #tblTargetActual tr td {
            vertical-align: middle;
        }

        /*#MainContent_grdValueContain tr:nth-child(even) {
				background-color: #797979;
			}*/


        #MainContent_grdValueContain tr th {
            color: white;
            text-align: center;
            background-color: #2E6886;
        }


        #MainContent_grdValueContain tr td {
            color: #797979;
        }

        #MainContent_myGrid {
            width: 74px;
            margin-left: -15px;
        }

            #MainContent_myGrid td {
                height: 36.8px;
            }

        .tbl td {
            height: 37px;
        }

        .tbl .time {
            width: 30%;
        }

        #container0, #container1, #container2, #container3, #container4, #container5 {
            margin-left: -14px;
        }

        .tblkwh td {
            /*width:20%;
			height:45px;*/
        }

        .tbl td {
            /*width: 12%;*/
            /*height:38px;*/
            /*padding-top:16px;*/
        }

        .col-lg-1, col-lg-6, col-lg-3, .row {
            text-align: center;
        }

        .col1, col-lg-6 {
            height: 80px;
            padding-top: 18px;
            border: 1px solid white;
        }

        .cl6 {
            height: 80px;
            padding-top: 29px;
            border: 0.5px solid white;
        }

        .cl2 {
            height: 80px;
            border: 0.5px solid white;
        }

        .cl66 {
            height: 40px;
            border: 1px solid white;
            border-width: 0.5px 0 0 1px;
            padding-top: 8px;
            padding-left: 2px;
        }

        .pad {
            padding-top: 8px;
        }

        .val {
            border: 0.5px solid white;
            width: 20%;
            height: 38px;
        }

        .kwh {
            border: 0.5px solid white;
        }

        .kwhcol {
            border: 0.5px solid white;
        }

        .tbl tr {
            transition: background 0.2s ease-in;
        }

        .tbl tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        .tbl tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .tbl tbody td span {
            color: black;
        }

        .tbl tr:hover {
            background-color: chartreuse;
        }

        .Rowcss {
            height: 40px;
            width: 280px;
            margin-left: -10px;
            font-size: x-small;
        }
        /*.input-group {
				border-collapse: collapse;
				margin-top: 12px;
			}*/
        .lbl-time-style {
            font-size: 12px;
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
    </style>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/VDGjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>
    <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div class="row text-center">
                <asp:HiddenField ID="hdfValue" runat="server" />
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                <table class="table table-bordered" id="tblfilter" style="width: 100%;">
                    <tr>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","PlantID")%></b></td>
                        <td>
                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" ToolTip="<%$Resources:CommanResource, PlantTooltip %>">
                            </asp:DropDownList></td>
                        <td class="commontd" style="width: 54px;"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                        <td style="min-width: 160px;">
                            <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control" meta:resourcekey="ddlCellIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","MachineId")%></b></td>
                        <td style="min-width: 160px;">
                            <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" data-toggle="tooltip" title="Machine ID !" ToolTip="<%$Resources:CommanResource, MachineTooltip %>"></asp:DropDownList></td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","Date")%></b></td>
                        <td class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                title="Date !" placeholder="Date" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" class="btn btn-info btn-sm" ID="btnProcess" OnClick="btnProcess_Click"></asp:Button>
                        </td>
                        <td>
                            <%--  <label>
                                <span class="checkbox commanTd">
                                    <asp:CheckBox ID="chkAutoBox" runat="server" type="checkbox" /><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></span>
                            </label>--%>
                            <label>
                               <b><span class="commontd"><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></span></b> 
                                <label class="switch">
                                    <asp:CheckBox ID="chkAutoBox" runat="server" />
                                    <span class="slider round"></span>
                                </label>
                        </td>
                    </tr>
                </table>
            </div>

            <div class="row text-center" style="padding: 0 0 2px 0; font-size: 20px; font-weight: 900; color: white">
                <asp:Label ID="Label1" runat="server" Text="Hourly Tracking - 18681 STUDER" meta:resourcekey="Label1Resource1"></asp:Label>
                <asp:Timer ID="timerDataChange" runat="server" OnTick="timerDataChange_Tick"></asp:Timer>
                <asp:Button ID="btnTrigger" runat="server" Style="display: none;" OnClick="btnTrigger_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row" style="padding: 0 0 2px 0; min-width: 700px;" id="tblGrid">
        <asp:UpdatePanel ID="updatePanel1" runat="server">
            <ContentTemplate>
                <div class="row" style="margin: 0; color: white; background-color: #2E6886; border-radius: 8px;">
                    <div class="col-lg-1 col1" style="border: 0.5px solid white;">
                        <div class="row"></div>
                        <div class="row  pad"><b><%=GetLocalResourceObject("Time") %></b></div>
                    </div>
                    <div class="col-lg-2 cl2">
                        <div class="row" style="height: 45px;">
                            <b><%=GetLocalResourceObject("Target") %></b>
                            <br />
                            <span style="font-size: 8px;"><%=GetLocalResourceObject("(Based on 100% OEE)") %></span>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 cl66"><b><%=GetLocalResourceObject("Pcs") %></b></div>
                            <div class="col-lg-6 cl66"><b><%=GetLocalResourceObject("Cumu") %></b></div>
                        </div>
                    </div>
                    <div class="col-lg-2 cl2">
                        <div class="row" style="height: 45px; padding-top: 12px;">
                            <b><%=GetLocalResourceObject("Actual") %></b>
                        </div>
                        <div class="row" style="height: auto">
                            <div class="col-lg-6 cl66"><b><%=GetLocalResourceObject("Pcs") %></b></div>
                            <div class="col-lg-6 cl66"><b><%=GetLocalResourceObject("Cumu") %></b></div>
                        </div>
                    </div>
                    <div class="col-lg-3 cl2" style="padding-top: 12px;">
                        <b><%=GetLocalResourceObject("Pieces / hour") %></b>
                    </div>

                    <div class="col-lg-4 cl2" runat="server" id="divDowntimeGraph">
                        <div class="row" style="padding-top: 12px;">
                            <b><%=GetLocalResourceObject("Losses [mins]") %></b>
                        </div>
                        <div class="row">
                            <asp:GridView ID="grdValueContain" runat="server"
                                CssClass="table table-bordered tbl" meta:resourcekey="grdValueContainResource1">
                                <HeaderStyle Height="45px" />
                                <RowStyle Height="45px" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row" style="margin: 0 2px 6px 2px; width: 100%">
            <asp:UpdatePanel ID="updatePanel2" runat="server">
                <ContentTemplate>
                    <div class="col-lg-5">
                        <div class="row">
                            <table class="table table-bordered tbl" id="tblTargetActual">
                                <tr style="height: 45px">
                                    <td style="font-size: 10px; width: 12%;" class="col-lg-1"><b>
                                        <label id="lblshift1hour1" class="lbl-time-style" runat="server" style="font-weight: 400;" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour1TPCS" meta:resourcekey="lbl67TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour1TCumu" meta:resourcekey="lbl67TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour1APCS" meta:resourcekey="lbl67APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour1ACumu" meta:resourcekey="lbl67ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour2" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour2TPCS" meta:resourcekey="lbl78TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour2TCumu" meta:resourcekey="lbl78TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour2APCS" meta:resourcekey="lbl78APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour2ACumu" meta:resourcekey="lbl78ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour3" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour3TPCS" meta:resourcekey="lbl89TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour3TCumu" meta:resourcekey="lbl89TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour3APCS" meta:resourcekey="lbl89APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour3ACumu" meta:resourcekey="lbl89ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour4" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour4TPCS" meta:resourcekey="lbl910TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour4TCumu" meta:resourcekey="lbl910TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour4APCS" meta:resourcekey="lbl910APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour4ACumu" meta:resourcekey="lbl910ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour5" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour5TPCS" meta:resourcekey="lbl1011TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour5TCumu" meta:resourcekey="lbl1011TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour5APCS" meta:resourcekey="lbl1011APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour5ACumu" meta:resourcekey="lbl1011ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour6" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour6TPCS" meta:resourcekey="lbl1112TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour6TCumu" meta:resourcekey="lbl1112TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour6APCS" meta:resourcekey="lbl1112APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour6ACumu" meta:resourcekey="lbl1112ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour7" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour7TPCS" meta:resourcekey="lbl1213TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour7TCumu" meta:resourcekey="lbl1213TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour7APCS" meta:resourcekey="lbl1213APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour7ACumu" meta:resourcekey="lbl1213ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour8" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour8TPCS" meta:resourcekey="lbl1314TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour8TCumu" meta:resourcekey="lbl1314TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour8APCS" meta:resourcekey="lbl1314APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour8ACumu" meta:resourcekey="lbl1314ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift1hour9" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour9" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour9TPCS" meta:resourcekey="lbl1455TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour9TCumu" meta:resourcekey="lbl1455TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour9APCS" meta:resourcekey="lbl1455APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour9ACumu" meta:resourcekey="lbl1455ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift1hour10" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour10" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour10TPCS" meta:resourcekey="lbl1455TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour10TCumu" meta:resourcekey="lbl1455TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour10APCS" meta:resourcekey="lbl1455APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour10ACumu" meta:resourcekey="lbl1455ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift1hour11" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour11" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour11TPCS" meta:resourcekey="lbl1455TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour11TCumu" meta:resourcekey="lbl1455TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour11APCS" meta:resourcekey="lbl1455APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour11ACumu" meta:resourcekey="lbl1455ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift1hour12" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift1hour12" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour12TPCS" meta:resourcekey="lbl1455TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour12TCumu" meta:resourcekey="lbl1455TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour12APCS" meta:resourcekey="lbl1455APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift1hour12ACumu" meta:resourcekey="lbl1455ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="background-color: yellow; height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>&Sigma; 1.S.</b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lbl1SsumTarget" meta:resourcekey="lbl1SsumTargetResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">&nbsp;</td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lbl1SsumActual" meta:resourcekey="lbl1SsumActualResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">&nbsp;</td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour1" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour1TPCS" meta:resourcekey="lbl1516TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour1TCumu" meta:resourcekey="lbl1516TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour1APCS" meta:resourcekey="lbl1516APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour1ACumu" meta:resourcekey="lbl1516ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour2" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour2TPCS" meta:resourcekey="lbl1617TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour2TCumu" meta:resourcekey="lbl1617TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour2APCS" meta:resourcekey="lbl1617APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour2ACumu" meta:resourcekey="lbl1617ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour3" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour3TPCS" meta:resourcekey="lbl1718TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour3TCumu" meta:resourcekey="lbl1718TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour3APCS" meta:resourcekey="lbl1718APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour3ACumu" meta:resourcekey="lbl1718ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour4" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour4TPCS" meta:resourcekey="lbl1819TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour4TCumu" meta:resourcekey="lbl1819TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour4APCS" meta:resourcekey="lbl1819APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour4ACumu" meta:resourcekey="lbl1819ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour5" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour5TPCS" meta:resourcekey="lbl1920TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour5TCumu" meta:resourcekey="lbl1920TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour5APCS" meta:resourcekey="lbl1920APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour5ACumu" meta:resourcekey="lbl1920ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour6" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour6TPCS" meta:resourcekey="lbl2021TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour6TCumu" meta:resourcekey="lbl2021TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour6APCS" meta:resourcekey="lbl2021APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour6ACumu" meta:resourcekey="lbl2021ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour7" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour7TPCS" meta:resourcekey="lbl2122TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour7TCumu" meta:resourcekey="lbl2122TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour7APCS" meta:resourcekey="lbl2122APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour7ACumu" meta:resourcekey="lbl2122ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour8" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour8TPCS" meta:resourcekey="lbl2223TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour8TCumu" meta:resourcekey="lbl2223TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour8APCS" meta:resourcekey="lbl2223APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour8ACumu" meta:resourcekey="lbl2223ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift2hour9" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour9" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour9TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour9TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour9APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour9ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift2hour10" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour10" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour10TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour10TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour10APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour10ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift2hour11" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour11" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour11TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour11TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour11APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour11ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift2hour12" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift2hour12" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour12TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour12TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour12APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; height: 45px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift2hour12ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="background-color: yellow; height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>&Sigma; 2.S.</b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lbl2SsumTarget" meta:resourcekey="lbl2SsumTargetResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">&nbsp;</td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lbl2SsumActual" meta:resourcekey="lbl2SsumActualResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">&nbsp;</td>
                                </tr>

                                <tr id="trshift3hour1" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour1" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour1TPCS" meta:resourcekey="lbl01TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour1TCumu" meta:resourcekey="lbl01TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour1APCS" meta:resourcekey="lbl01APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour1ACumu" meta:resourcekey="lbl01ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour2" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour2" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour2TPCS" meta:resourcekey="lbl12TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour2TCumu" meta:resourcekey="lbl12TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour2APCS" meta:resourcekey="lbl12APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour2ACumu" meta:resourcekey="lbl12ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour3" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour3" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour3TPCS" meta:resourcekey="lbl23TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour3TCumu" meta:resourcekey="lbl23TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour3APCS" meta:resourcekey="lbl23APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour3ACumu" meta:resourcekey="lbl23ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour4" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour4" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour4TPCS" meta:resourcekey="lbl34TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour4TCumu" meta:resourcekey="lbl34TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour4APCS" meta:resourcekey="lbl34APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour4ACumu" meta:resourcekey="lbl34ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour5" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour5" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour5TPCS" meta:resourcekey="lbl45TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour5TCumu" meta:resourcekey="lbl45TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour5APCS" meta:resourcekey="lbl45APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour5ACumu" meta:resourcekey="lbl45ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour6" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour6" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour6TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour6TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour6APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour6ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour7" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour7" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour7TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour7TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour7APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour7ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour8" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour8" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour8TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour8TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour8APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour8ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr id="trshift3hour9" runat="server" style="height: 45px">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>
                                        <label id="lblshift3hour9" class="lbl-time-style" runat="server" />
                                    </b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour9TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour9TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour9APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lblshift3hour9ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
                                </tr>

                                <tr style="background-color: yellow; height: 45px" id="trsummation3shift" runat="server">
                                    <td style="font-size: 10px;" class="col-lg-1"><b>&Sigma; 3.S.</b></td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lbl3SsumTarget" meta:resourcekey="lbl3SsumTargetResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">&nbsp;</td>
                                    <td style="font-size: 15px; width: 12.4899%;">
                                        <asp:Label runat="server" ID="lbl3SsumActual" meta:resourcekey="lbl3SsumActualResource1"></asp:Label></td>
                                    <td style="font-size: 15px; width: 12.4899%;">&nbsp;</td>
                                </tr>

                                <tr style="color: white; background-color: #2E6886;">
                                    <td colspan="2" style="color: white;"><b><%=GetLocalResourceObject("Total Per Day") %></b></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblTotalTarget" Style="color: white" meta:resourcekey="lblTotalTargetResource1"></asp:Label></td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblTotalActual" Style="color: white" meta:resourcekey="lblTotalActualResource1"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="col-lg-3" style="padding-right: 0;">
                <%--<table style="width:300px">
					<tr style="height:450px;width:100%">
						<td>
							<div id="container0" style="height: 426px; background-color: white; margin-right: -3px"></div>
						</td>
					</tr>
					<tr style="height:450px;width:100%">
						<td>
						<div id="container1" style="height: 361px; background-color: white; margin-right: -3px"></div>
							</td>
					</tr>
					<tr style="height:450px;width:100%">
						<td>
						<div id="container2" style="height: 337px; background-color: white; margin-right: -3px"></div>
							</td>
					</tr>
				</table>--%>
                <div id="container0" style="height: 450px; background-color: white; margin-right: -3px"></div>
                <div id="container1" style="height: 450px; background-color: white; margin-right: -3px"></div>
                <div id="container2" style="height: 450px; background-color: white; margin-right: -3px"></div>
                <%--<div id="container0" style="height: 467px; background-color: white;"></div>
					<div id="container1" style="height: 407px; background-color: white;"></div>
					<div id="container2" style="height: 390px; background-color: white;"></div>--%>
            </div>


            <%-- <div class="col-lg-2" style="width: 13%">

					<div id="container3" style="height: 330px; background-color: white;"></div>
					<div id="container4" style="height: 338.5px; background-color: white;"></div>
					<div id="container5" style="height: 345px; background-color: white;"></div>
				</div>--%>

            <%--   <asp:UpdatePanel ID="updatePanel3" runat="server">
					<ContentTemplate>--%>
            <%-- <div class="col-lg-1 cl2" style="width: 6%">
						</div>
						<div class="col-lg-1 cl2" style="width: 100%">
							<div class="row" style="height: 40px; padding-top: 12px;">
							</div>
							<div class="row" style="height: auto">
								<div class="col-lg-6 cl66"><b></b></div>
								<div class="col-lg-6 cl66"><b></b></div>
							</div>
						</div>--%>
            <%--                    <div class="col-lg-5 cl2" style="width: 28.85%">
							<div class="row" style="height: auto">
							</div>
						</div>--%>


            <%--          </ContentTemplate>
				</asp:UpdatePanel>--%>
        </div>

    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            //  $.unblockUI({});
            BindGraph();
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            //var newWidth = ($(window).width() - 180);
            //$("#tblGrid").width(newWidth);

            //$("#liMenu").click(function () {
            //    newWidth = $(window).width();
            //    var widthMenu = $("#sidebar").width();
            //    if (widthMenu == 180) {
            //        $("#liMenu").width($(window).width() - 46);
            //    } else {
            //        $("#liMenu").width($(window).width() - 180);
            //    }
            //});
        })
        function BindGraph() {
            if ($("[id$=hdfValue]").val() == "OK") {
                GetFirstShiftData();
                //GetSecondShiftData();
                //GetThirdShiftData();
            }
        }

        $(document).on("change", "[id$=chkAutoBox]", function () {
            if ($("[id$=txtDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterDate")%>")
                $("[id$=txtDate]").focus();
                return false;
            }
            $("[id$=btnTrigger]").trigger("click");
            //return false;
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {

            BindGraph();
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        });

        $(document).on("click", "[id$=btnProcess]", function () {
            if ($("[id$=txtDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterDate")%>")
                $("[id$=txtDate]").focus();
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        function CSSApply() {
            $("#Qualitytbl tr:nth-child(9)").css("height", "37px")
            $("#Performacetbl tr:nth-child(9)").css("height", "37px")
            $("#Otherstbl tr:nth-child(9)").css("height", "37px")
            $("#Availabilitytbl tr:nth-child(9)").css("height", "37px")
            $("#Qualitytbl tr:nth-child(9)").html("&nbsp;");
            $("#Performacetbl tr:nth-child(9)").html("&nbsp;");
            $("#Otherstbl tr:nth-child(9)").html("&nbsp;");
            $("#Availabilitytbl tr:nth-child(9)").html("&nbsp;");
            $("#Qualitytbl tr:nth-child(18)").css("height", "37px")
            $("#Performacetbl tr:nth-child(18)").css("height", "37px")
            $("#Otherstbl tr:nth-child(18)").css("height", "37px")
            $("#Availabilitytbl tr:nth-child(18)").css("height", "37px")
            $("#Qualitytbl tr:nth-child(18)").html("&nbsp;");
            $("#Performacetbl tr:nth-child(18)").html("&nbsp;");
            $("#Otherstbl tr:nth-child(18)").html("&nbsp;");
            $("#Availabilitytbl tr:nth-child(18)").html("&nbsp;");
        }

        function GetFirstShiftData() {
            $.ajax({
                type: "POST",
                url: "ShiftProductionCountHourly.aspx/GetFirstShiftGraph",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startDate:"' + $("[id$=txtDate]").val() + '"}',
                dataType: "json",
                success: OnSuccessGetFirst,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }
        //function GetSecondShiftData() {
        //    $.ajax({
        //        type: "POST",
        //        url: "ShiftProductionCountHourly.aspx/GetFirstShiftGraph",
        //        contentType: "application/json; charset=utf-8",
        //        data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startDate:"' + $("[id$=txtDate]").val() + '"}',
        //        dataType: "json",
        //        success: OnSuccessGetSecond,
        //        error: function (response) {
        //            console.log(response.d);
        //        }
        //    });
        //}
        //function GetThirdShiftData() {
        //    $.ajax({
        //        type: "POST",
        //        url: "ShiftProductionCountHourly.aspx/GetFirstShiftGraph",
        //        contentType: "application/json; charset=utf-8",
        //        data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startDate:"' + $("[id$=txtDate]").val() + '"}',
        //        dataType: "json",
        //        success: OnSuccessGetThird,
        //        error: function (response) {
        //            console.log(response.d);
        //        }
        //    });
        //}
        function OnSuccessGetFirst(Result) {
            drawFirstShiftChart(Result.d);
            drawSecondShiftChart(Result.d);
            drawThirdShiftChart(Result.d);
            //KWHFirstShiftChart(Result.d);
            //KWHSecondShiftChart(Result.d);
            //KWHThirdShiftChart(Result.d)
            $.unblockUI({});
        }

        var Container0 =  <%= Container0 %>;
        var Container1 =  <%= Container1 %>;
        var Container2 =  <%= Container2 %>;
        var Margin0 = <%= Margin0 %>;
        var Margin1 = <%= Margin1 %>;
        var Margin2 = <%= Margin2 %>;
        function drawFirstShiftChart(data1) {
            $('#container0').height(Container0);

            $('#container0').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    marginTop: Margin0,
                    // marginBottom:0,
                    marginLeft: 0,
                    paddingLeft: 0,
                    // columnWidth: 23,
                    backgroundColor: 'transparent',
                    style: {
                        overflow: 'visible',
                    },
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    },

                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    },
                    opposite: true,
                    labels: {
                        formatter: function () {
                            return '<span style="fill: white;">' + this.value + '</span>';
                        },
                        style: {
                            fontWeight: 'bold',
                            fontSize: 12,
                        }
                    },
                    //labels: {
                    //    enabled: true,
                    //    fill: 'red'
                    //},
                },
                legend: {
                    reversed: true,
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        pointWidth: 45,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        // pointWidth: 38,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    line: {
                        marker: {
                            enabled: false
                        },
                    }
                    //bar: {
                    //    borderColor: '#FF555F',
                    //    borderWidth: 2,
                    //    color: '#FFFFFF',
                    //},
                },
                series: [{
                    name: 'Target',
                    type: 'bar',
                    color: '#C8D5EE',
                    //barWidth: 4,
                    //  maxPointWidth: 40,
                    //  data: [1, 2, 8, 5, 2, 6, 7, 5],                
                    data: data1.TargetFirstShift,

                }, {
                    //  data: [0, 2, 3, 9, 6, 3, 7, 8, 6, 0],
                    data: data1.ActualFirstShift,
                    //  pointPadding: 0.4,
                    pointPlacement: 0.5,
                    lineWidth: 3,
                    color: 'green',
                    step: 'right',
                    name: 'Actual'
                }]
            });

        }
        //-------Second Chart-----              
        function drawSecondShiftChart(data1) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            //if (data1.ActualSecondShift != null) {
            //    if (data1.ActualSecondShift.length > 0) {
            //        data1.ActualSecondShift.splice(0, 0, 0);
            //        data1.ActualSecondShift[data1.ActualSecondShift.length] = 0;
            //    }
            //}
            $('#container1').height(Container1);
            $('#container1').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    marginTop: Margin1,
                    // marginBottom:0,              
                    marginLeft: 0,
                    paddingLeft: 0,
                    //  columnWidth: 23,
                    backgroundColor: 'transparent',
                    style: {
                        overflow: 'visible',
                    },

                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,

                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true,
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        pointWidth: 45,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    line: {
                        marker: {
                            enabled: false
                        }
                    }
                    //bar: {
                    //    borderColor: '#FF555F',
                    //    borderWidth: 2,
                    //    color: '#FFFFFF',
                    //},
                },
                series: [{
                    name: 'Target',
                    type: 'bar',
                    color: '#C8D5EE',
                    // data: [1, 3, 8, 9, 1, 9, 5, 5],
                    data: data1.TargetSecondShift,

                }, {
                    //data: [0, 2, 5, 8, 4, 3, 8, 6, 1, 0],
                    data: data1.ActualSecondShift,
                    //  pointPadding: 0.4,
                    pointPlacement: 0.5,
                    lineWidth: 3,
                    color: 'green',
                    step: 'right',
                    name: 'Actual'
                }]

            });

        }
        //------Third Chart-------
        function drawThirdShiftChart(data1) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);

            $('#container2').height(Container2);
            $('#container2').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    marginTop: Margin2,
                    // marginBottom:0,              
                    marginLeft: 0,
                    paddingLeft: 0,
                    //  columnWidth: 23,
                    backgroundColor: 'transparent',
                    style: {
                        overflow: 'visible',
                    },

                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    //height:40,
                    title: {
                        text: ''
                    },
                    opposite: true,
                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true,
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        pointWidth: 45,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    line: {
                        marker: {
                            enabled: false
                        }
                    }
                    //bar: {
                    //    borderColor: '#FF555F',
                    //    borderWidth: 2,
                    //    color: '#FFFFFF',
                    //},
                },
                series: [{
                    name: 'Target',
                    type: 'bar',
                    color: '#C8D5EE',
                    //data: [1, 3, 3, 8, 2, 9, 2, 5],
                    data: data1.TargetThirdShift,

                }, {
                    // data: [0, 7, 8, 6, 4, 5, 9, 6, 1, 0],
                    data: data1.ActualThirdShift,
                    //  pointPadding: 0.4,
                    pointPlacement: 0.5,
                    lineWidth: 3,
                    color: 'green',
                    step: 'right',
                    name: 'Actual'
                }]
            });
        }

        //--------KWH Chart ----------------
        function KWHFirstShiftChart(data) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container3').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'bar',
                    marginTop: -4,
                    marginLeft: 0,
                    paddingLeft: 0,
                    backgroundColor: 'transparent',
                    //style: {
                    //    overflow: 'visible',
                    //},
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    height: 45,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [{
                    color: '#C8D5EE',
                    data: data.KWHFirstShift,

                }]
            });
        }

        function KWHSecondShiftChart(data) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container4').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'bar',
                    marginTop: -4,
                    marginLeft: 0,
                    paddingLeft: 0,
                    backgroundColor: 'transparent',
                    //style: {
                    //    overflow: 'visible',
                    //},
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    height: 45,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [{
                    color: '#C8D5EE',
                    data: data.KWHSecondShift,

                }]
            });
        }

        function KWHThirdShiftChart(data) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container5').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'bar',
                    marginTop: -4,
                    marginLeft: 0,
                    paddingLeft: 0,
                    backgroundColor: 'transparent',
                    //style: {
                    //    overflow: 'visible',
                    //},
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    height: 45,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [{
                    color: '#C8D5EE',
                    data: data.KWHThirdShift,

                }]
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
