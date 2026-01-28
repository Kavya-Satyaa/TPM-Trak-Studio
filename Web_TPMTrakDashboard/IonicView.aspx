<%@ Page Title="Cockpit View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IonicView.aspx.cs" Inherits="Web_TPMTrakDashboard.IonicView" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/Ionic.css" rel="stylesheet" />
    <%--<link href="MyCssAndJS/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />--%>

    <%--    <script src="MyCssAndJS/DatePicker/jquery-2.1.1.min.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/moment-with-locales.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"></script>--%>

    <%-- <%: Styles.Render("~/bundles/tablecss") %>--%>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 15px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        .table .lbl {
            padding-top: 15px;
        }

        .table {
            margin-bottom: 0px;
        }

        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }

        .Green {
            /*background-color: green;*/
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .Red {
            /*background-color: red;*/
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .Yellow {
            /*background-color: yellow;*/
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .white {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
        }

        .Running {
            -webkit-animation: cog-rotate 2s linear infinite;
            -moz-animation: cog-rotate 2s linear infinite;
            -o-animation: cog-rotate 2s linear infinite;
            animation: rotate 2s linear infinite;
            color: green;
        }

        .Stopped {
            color: red;
        }

        .PDT {
            color: blue;
        }

        a {
            color: black;
        }

        .HyperLink {
            text-decoration: underline;
            cursor: pointer;
            color: #547CFF;
        }

        .pager li > a:hover, .pager li > a:focus {
            background-color: #428bca;
            color: #ffffff;
            margin: 0px 0;
        }

        .pager li > a, .pager li > span {
            background-color: #428bca;
            color: #ffffff;
            margin: 0px 0;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .ellipsistooltip {
            text-overflow: ellipsis;
            overflow: hidden;
            width: 160px;
            white-space: nowrap;
        }

        .fixHeader {
            position: sticky;
            top: 70px;
            background-color: #202648;
            z-index: 999;
        }

        .myItem {
            vertical-align: top;
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

        .legend-color {
            border-radius: 50%;
            padding: 0px 7px;
        }

        #tblLegends tr {
            font-size: 10px;
        }
    </style>
    <asp:UpdatePanel ID="updatePanal" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnView" ClientIDMode="Static" />
            <div class="row" style="text-align: center; color: red;">
                <asp:HiddenField ID="hdfMode" runat="server" />
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
            </div>

            <div class="row fixHeader">
                <table id="tblfilter" class="table table-bordered" style="display: inline-block;">
                    <tr style="height: 40px;">
                        <td class="commontd" style="width: 90px;"><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>
                        <td style="width: 160px; height: 60px">
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Style="width: 160px; height: 42px" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </div>
                        </td>
                        <td class="commontd" style="width: 60px;"><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b></td>
                        <td style="width: 160px; height: 60px;">
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" Style="width: 160px; height: 42px;" runat="server" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </div>
                        </td>
                        <td class="commontd" style="width: 60px;"><b><%=GetGlobalResourceObject("CommanResource","Plant") %></b></td>
                        <td style="min-width: 100px;">
                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <%--<td class="commontd" style="width: 50px;"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control" meta:resourcekey="ddlCellIdResource1">
                            </asp:DropDownList>
                        </td>--%>
                        <td class="commontd" style="min-width: 125px">
                            <span><b><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></b></span>
                            <label class="switch">
                                <%--<span class="checkbox">--%>
                                <asp:CheckBox ID="chkAutoBox" runat="server" />
                                <%--type="checkbox"--%>
                                <%--meta:resourcekey="chkAutoBoxResource1"--%>
                                <%--<b><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></b>--%>
                                <%--  </span>--%>
                                <span class="slider round"></span>
                            </label>
                        </td>
                        <td class="commontd" style="min-width: 130px; display: none">

                            <label>
                                <span class="checkbox" style="min-width: 120px;">
                                    <input id="chkBox" type="checkbox" value=""><b><%=GetGlobalResourceObject("CommanResource","EnablePaging") %></b></span>
                            </label>

                        </td>
                        <td>
                            <asp:LinkButton runat="server" ID="lnkSwitch" CssClass="btn btn-info btn-sm glyphicon glyphicon-random" Style="font-weight: bold; color: white; font-size: 15px; height: 30px" Text=" SwitchToTable" OnClick="lnkSwitch_Click" OnClientClick="return showLoader();"></asp:LinkButton>
                        </td>
                        <%--<td colspan="2" class="commontd" style="min-width: 80px;"></td>--%>
                    </tr>
                    <tr style="height: 40px">
                        <td class="commontd" runat="server" visible="false" style="width: 90px;"><b><%=GetGlobalResourceObject("CommanResource","Order") %></b></td>
                        <td style="width: 160px;" runat="server" visible="false">
                            <asp:DropDownList ID="ddlMachineOrder" runat="server" CssClass="form-control" meta:resourcekey="ddlMachineOrderResource1">
                                <asp:ListItem Value="MACHINE ID" meta:resourcekey="ListItemResource1">MACHINE ID</asp:ListItem>
                                <asp:ListItem Value="AE - ASC" meta:resourcekey="ListItemResource2">AE - ASC</asp:ListItem>
                                <asp:ListItem Value="PE - ASC" meta:resourcekey="ListItemResource3">PE - ASC</asp:ListItem>
                                <asp:ListItem Value="OE - ASC" meta:resourcekey="ListItemResource4">OE - ASC</asp:ListItem>
                                <asp:ListItem Value="COUNT - ASC" meta:resourcekey="ListItemResource5">COUNT - ASC</asp:ListItem>
                                <asp:ListItem Value="AE - DESC" meta:resourcekey="ListItemResource6">AE - DESC</asp:ListItem>
                                <asp:ListItem Value="PE - DESC" meta:resourcekey="ListItemResource7">PE - DESC</asp:ListItem>
                                <asp:ListItem Value="OE - DESC" meta:resourcekey="ListItemResource8">OE - DESC</asp:ListItem>
                                <asp:ListItem Value="COUNT - DESC" meta:resourcekey="ListItemResource9">COUNT - DESC</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","PredefinedTime") %></b></td>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlDayShift" runat="server" Style="width: 200px"
                                CssClass="form-control displayCss" AutoPostBack="True" OnSelectedIndexChanged="ddlDayShift_SelectedIndexChanged" meta:resourcekey="ddlDayShiftResource1">
                            </asp:DropDownList>
                        </td>
                        <td class="commontd" style="width: 50px;"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                        <td style="min-width: 120px;">
                            <%-- <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control" meta:resourcekey="ddlCellIdResource1">
                            </asp:DropDownList>--%>
                            <asp:ListBox runat="server" ID="lbCellID" CssClass="form-control" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddlView" runat="server" CssClass="form-control" meta:resourcekey="ddlCellIdResource1" Style="background-color: #39b3d7; color: white; font-weight: bold; width: auto" OnSelectedIndexChanged="ddlView_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                <asp:ListItem Value="Machinewise">Machine View</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="commontd" style="width: 50px;">Sort Order</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSortOrder" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        </td>

                        <td style="min-width: 60px;">
                            <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" class="btn btn-info btn-sm displayCss" ID="btnProcess" OnClick="btnProcess_Click" OnClientClick="return showLoader();"></asp:Button>
                        </td>


                        <td runat="server" id="tdlbBackButton" style="width: 120px; background-color: #e7e7e7">
                            <asp:LinkButton runat="server" ID="lbBackButton" CssClass="	glyphicon glyphicon-chevron-left" Style="font-weight: bold; color: #115d9f; font-size: 15px; height: 30px" OnClick="lbBackButton_Click1" OnClientClick="return showLoader();"></asp:LinkButton>
                        </td>


                        <td colspan="2" class="commontd" style="min-width: 120px;">
                            <button id="btnPrevious" type="button" style="display: none" class="btn btn-primary btn-round-sm btn-sm"><%=GetGlobalResourceObject("CommanResource","Previous") %></button>
                            &nbsp;&nbsp;
                            <button id="btnNext" type="button" style="display: none" class="btn btn-primary btn-round-sm btn-sm"><%=GetGlobalResourceObject("CommanResource","Next") %></button>
                        </td>
                    </tr>
                </table>

            </div>

            <div class="row">
                <div id="divCustomerList" style="overflow-x: auto; width: 100%;">
                    <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="LstCustomers">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="myItem" style="margin: 15px; min-width: 100px; display: inline-block;">
                                <div class="border">
                                    <div class="<%# Eval("MachineOEE") %> oeeBackDiv" style="padding: 10px; background-color: <%# Eval("BackColor") %>;">
                                        <table style='width: 100%;' class="outercockpit">
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                                    <asp:LinkButton ID="lnkMachine" runat="server" CommandArgument='<%# Eval("MachineId") %>'><%# Eval("MachineId")%></asp:LinkButton>
                                                    <asp:HiddenField runat="server" ID="hdnAERed" ClientIDMode="Static" Value='<%# Eval("AERed") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnAEGreen" ClientIDMode="Static" Value='<%# Eval("AEGreen") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnPERed" ClientIDMode="Static" Value='<%# Eval("PERed") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnPEGreen" ClientIDMode="Static" Value='<%# Eval("PEGreen") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnQERed" ClientIDMode="Static" Value='<%# Eval("QERed") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnQEGreen" ClientIDMode="Static" Value='<%# Eval("QEGreen") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnOEERed" ClientIDMode="Static" Value='<%# Eval("OEERed") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnOEEGreen" ClientIDMode="Static" Value='<%# Eval("OEEGreen") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnBadColor" ClientIDMode="Static" Value='<%# Eval("BadColor") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnModerateColor" ClientIDMode="Static" Value='<%# Eval("ModerateColor") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnGoodColor" ClientIDMode="Static" Value='<%# Eval("GoodColor") %>' />
                                                </td>
                                                <td style="padding-bottom: 8px; background-color: transparent; width: 35px;">
                                                    <asp:Image ImageUrl='<%# Eval("StatusImage") %>' runat="server"
                                                        Visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?true:false %>' meta:resourcekey="ImageResource1" />
                                                    <div class="loaders-container" runat="server" visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?false:true %>'>
                                                        <div class="la-cog la-2x" style="float: right;">
                                                            <div class="<%# Eval("MachineStatus") %>">
                                                                <input type="hidden" runat="server" clientidmode="static" id="hdnMachineStatus" value='<%# Eval("MachineStatus") %>' />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white;'>
                                            <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>;">
                                                            <%# Eval("LabelText")%>                                          
                                                        </td>
                                                        <td style='text-align: left; width: 170px; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                            <div class="ellipsistooltip" title='<%# Eval("LabelValueToolTip") %>'><%# Eval("LabelValue")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </div>
                                    <div style='text-align: center; padding: 1px; display: <%= settings.ShowSmileyBlock %>;'>
                                        <img src="<%# Eval("SmileyImagePath") %>" style='height: <%= settings.SmileyBlockSize %>; width: auto;' onmouseover="showLegendPanel(event,this);" onmouseout="$('#legendPanel').hide();" />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="lvPlantDetails" OnItemUpdating="lvPlantDetails_ItemUpdating">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="myItem" style="margin: 15px; min-width: 100px; display: inline-block;">
                                <div class="border">
                                    <div class="white" style="padding: 10px; background-color: <%# Eval("BackColor") %>;">
                                        <table style='width: 100%;' class="outercockpit">
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                                    <asp:HiddenField runat="server" ID="hfPlantId" Value=' <%# Eval("PlantID") %>' />
                                                    <%-- <asp:LinkButton ID="lnkMachine" runat="server" CommandArgument='<%# Eval("MachineId") %>'><%# Eval("MachineId")%></asp:LinkButton>--%>
                                                    <asp:Button runat="server" ID="btnPlant" Text='<%# "Plant : "+ Eval("PlantID") %>' Style="border: unset; background-color: unset; background-color: #0072c6; color: white; font-weight: bold; padding: 5px 15px; border-radius: 8px; cursor: pointer" CommandName="Update" OnClientClick="return showLoader();" />
                                                </td>
                                                <%-- <td style="padding-bottom: 8px; background-color: transparent; width: 35px;">
                                                    <asp:Image ImageUrl='<%# Eval("StatusImage") %>' runat="server"
                                                        Visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?true:false %>' meta:resourcekey="ImageResource1" />
                                                    <div class="loaders-container" runat="server" visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?false:true %>'>
                                                        <div class="la-cog la-2x" style="float: right;">
                                                            <div class="<%# Eval("MachineStatus") %>"></div>
                                                        </div>
                                                    </div>
                                                </td>--%>
                                            </tr>
                                        </table>
                                        <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white;'>
                                            <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>;">
                                                            <%# Eval("LabelText")%>                              
                                                        </td>
                                                        <td style='text-align: left; width: 170px; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                            <div class="ellipsistooltip" title='<%# Eval("LabelValueToolTip") %>'><%# Eval("LabelValue")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </div>
                                    <div style='text-align: center; padding: 1px; display: <%= settings.ShowSmileyBlock %>;'>
                                        <img src="<%# Eval("SmileyImagePath") %>" style='height: <%= settings.SmileyBlockSize %>; width: auto;' />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="lvCellDetails" OnItemUpdating="lvCellDetails_ItemUpdating">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="myItem" style="margin: 15px; min-width: 100px; display: inline-block;">
                                <div class="border">
                                    <div class="white" style="padding: 10px; background-color: <%# Eval("BackColor") %>;">
                                        <table style='width: 100%;' class="outercockpit">
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                                    <asp:HiddenField runat="server" ID="hdnCellPlantID" Value=' <%# Eval("PlantID") %>' />
                                                    <asp:HiddenField runat="server" ID="hfCellId" Value=' <%# Eval("GroupName") %>' />

                                                    <asp:Button runat="server" ID="btnPlant" Text='<%# "Cell : "+Eval("GroupName") %>' Style="border: unset; background-color: unset; background-color: #0072c6; color: white; font-weight: bold; padding: 5px 15px; border-radius: 8px; cursor: pointer" CommandName="Update" OnClientClick="return showLoader();" />
                                                    <%--   <asp:LinkButton ID="lnkMachine" runat="server" CommandArgument='<%# Eval("MachineId") %>'><%# Eval("MachineId")%></asp:LinkButton>--%>
                                                </td>
                                                <%--  <td style="padding-bottom: 8px; background-color: transparent; width: 35px;">
                                                    <asp:Image ImageUrl='<%# Eval("StatusImage") %>' runat="server"
                                                        Visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?true:false %>' meta:resourcekey="ImageResource1" />
                                                    <div class="loaders-container" runat="server" visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?false:true %>'>
                                                        <div class="la-cog la-2x" style="float: right;">
                                                            <div class="<%# Eval("MachineStatus") %>"></div>
                                                        </div>
                                                    </div>
                                                </td>--%>
                                            </tr>
                                        </table>
                                        <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white;'>
                                            <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>;">
                                                            <%# Eval("LabelText")%>                                      
                                                        </td>
                                                        <td style='text-align: left; width: 170px; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                            <div class="ellipsistooltip" title='<%# Eval("LabelValueToolTip") %>'><%# Eval("LabelValue")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </div>
                                    <div style='text-align: center; padding: 1px; display: <%= settings.ShowSmileyBlock %>;'>
                                        <img src="<%# Eval("SmileyImagePath") %>" style='height: <%= settings.SmileyBlockSize %>; width: auto;' />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <asp:Button ID="btnTrigger" runat="server" Style="display: none;" OnClick="btnTrigger_Click" />
            <asp:Timer ID="timerDataChange" runat="server" OnTick="timerDataChange_Tick"></asp:Timer>

            <div class="panel panel-default panel-filter" id="legendPanel" style="z-index: 9999; display: none;">
                <%--   <div class="panel-heading" style="background-color: #28b1b1; color: white; font-size: 16px">
                    <span class="filter-header-name">Legends</span>
                </div>--%>

                <div class="panel-body" style="padding: 0px">
                    <div style="padding: 0px;">
                        <table class="table table-bordered cockpit headerFixer" id="tblLegends">
                            <tr>
                                <th>Color</th>
                                <th>AE</th>
                                <th>PE</th>
                                <th>QE</th>
                                <th>OEE</th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblGoodColor" ClientIDMode="Static" CssClass="legend-color"></asp:Label>
                                </td>
                                <td>>= 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblAEGood" CssClass="lblAEGreen"></asp:Label>
                                </td>
                                <td>>= 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblPEGood" CssClass="lblPEGreen"></asp:Label>
                                </td>
                                <td>>= 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblQEGood" CssClass="lblQEGreen"></asp:Label>
                                </td>
                                <td>>=  
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblOEEGood" CssClass="lblOEEGreen"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblModerateColor" ClientIDMode="Static" CssClass="legend-color"></asp:Label>
                                </td>
                                <td>>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblAEModerate" CssClass="lblAERed"></asp:Label>
                                    &
                                    < 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="Label1" CssClass="lblAEGreen"></asp:Label>
                                </td>
                                <td>>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblPEModerate" CssClass="lblPERed"></asp:Label>
                                    &
                                    < 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="Label2" CssClass="lblPEGreen"></asp:Label>
                                </td>
                                <td>> 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblQEModerate" CssClass="lblQERed"></asp:Label>
                                    &
                                    < 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="Label3" CssClass="lblQEGreen"></asp:Label>
                                </td>
                                <td>>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblOEEModerate" CssClass="lblOEERed"></asp:Label>
                                    &
                                    < 
                                    <asp:Label runat="server" ClientIDMode="Static" ID="Label4" CssClass="lblOEEGreen"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblBadColor" ClientIDMode="Static" CssClass="legend-color"></asp:Label>
                                </td>
                                <td><=
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblAEBad" CssClass="lblAERed"></asp:Label>
                                </td>
                                <td><=
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblPEBad" CssClass="lblPERed"></asp:Label>
                                </td>
                                <td><=
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblQEBad" CssClass="lblQERed"></asp:Label>
                                </td>
                                <td><=
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblOEEBad" CssClass="lblOEERed"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
                <%--  <div class="panel-footer" style="padding: 5px; border-top: 1px solid #5D7B9D; text-align: center; background-color: #28b1b1; color: white; font-size: 16px">
                    <input type="button" value="CLOSE" class="btn-style btn-panel-cancel" onclick=" $('#legendPanel').hide();" />
                </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var showItem = NaN, divH, divW, screenH, screenW, totalH, totalW, totalBox;
        var currentTab = 0;
        var timer;
        var datarefreshInterval = 1000 * 30;
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
            //$("#divCustomerList").height(winHeight - 288);

            $.unblockUI({});
            setControl();
            $('.ellipsistooltip').tooltipOnOverflow();
            $('[data-toggle="tooltip"]').tooltip();
            // showFirstNItems();

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            $("[id$=btnProcess]").click(function () {
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
                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();

                <%--if (Date.parse(from) > Date.parse(to)) {
                    alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                    $('[id$=txtToDate]').val('');
                    $('[id$=txtToDate]').focus();
                    return false;
                }--%>
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                RefreshMachineStatus();
            });

            $("[id$=ddlDayShift]").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            RefreshMachineStatus();
        });
        function showLegendPanel(event, ctrl) {
            debugger;
            var div = $(ctrl).closest('.myItem');
            $('#lblGoodColor').css("background-color", $(div).find('#hdnGoodColor').val());
            $('#lblModerateColor').css("background-color", $(div).find('#hdnModerateColor').val());
            $('#lblBadColor').css("background-color", $(div).find('#hdnBadColor').val());

            $('.lblAEGreen').text($(div).find('#hdnAEGreen').val());
            $('.lblPEGreen').text($(div).find('#hdnPEGreen').val());
            $('.lblQEGreen').text($(div).find('#hdnQEGreen').val());
            $('.lblOEEGreen').text($(div).find('#hdnOEEGreen').val());

            $('.lblAERed').text($(div).find('#hdnAERed').val());
            $('.lblPERed').text($(div).find('#hdnPERed').val());
            $('.lblQERed').text($(div).find('#hdnQERed').val());
            $('.lblOEERed').text($(div).find('#hdnOEERed').val());

            $('#legendPanel').css({
                'left': event.pageX - 150,
                'top': event.pageY - 170,
                'display': 'inline-block',
                "position": "absolute",
                "visibility": "visible"
            }).show();
        }
        function setControl() {
            $('[id$=lbCellID]').multiselect({
                includeSelectAllOption: true
            });
        }
        function initTimer() {
            console.log("Timer " + timer);
            if ($('#hdnView').val() == "Machinewise") {
                clearTimeout(timer);
                timer = setTimeout(function () {
                    RefreshMachineStatus();
                }, datarefreshInterval);
            }
            else {
                clearTimeout(timer);
            }
        }
        function RefreshMachineStatus() {
            clearTimeout(timer);
            var wiproPageEnabled = IsPageEnabled("WiproPages");
            if (wiproPageEnabled == true) {
                $("#divCustomerList").find(".oeeBackDiv").css("background-color", "white");
            }
            if ($('#hdnView').val() == "Machinewise") {
                $.ajax({
                    type: "POST",
                    url: "tableView.aspx/GetMachineStatusData",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '"}',
                    dataType: "json",
                    success: function (response) {
                        var machineStatusData = response.d;
                        if (machineStatusData.length > 0) {
                            debugger;

                            $("#divCustomerList").find(".myItem").each(function (index, tblRow) {
                                var machineId = $(this).find(".outercockpit").find("a").html();
                                var statdata = machineStatusData.filter(x => x.MachineID == machineId);
                                tblRow = $(this).find(".outercockpit tr")[0];

                                if (wiproPageEnabled == true) {
                                    if (statdata != null && statdata != undefined && statdata.length > 0) {
                                        $(tblRow).find("td:eq(1)").hide();
                                        $(tblRow).find("td:eq(0)").css("background-color", statdata[0].MachineStatusColor);
                                        $(tblRow).find("td:eq(0)").css("padding", "15px");
                                        $(tblRow).find("td:eq(0)").css("border-radius", "6px");
                                        $(tblRow).find("td:eq(0) a").css("color", "white");
                                        $(tblRow).find("td:eq(0)").attr("title", statdata[0].MachineLiveStatus);
                                    }
                                    else {
                                        var BGColor;
                                        $(tblRow).find("td:eq(1)").hide();
                                        var MachineStatus = $($(tblRow).find("td:eq(1)")).find("#MainContent_LstCustomers_hdnMachineStatus_0").val();
                                        if (MachineStatus.toLowerCase() == "stopped")
                                            BGColor = "Red";
                                        else if (MachineStatus.toLowerCase() == "running")
                                            BGColor = "Green";
                                        else if (MachineStatus.toLowerCase() == "pdt")
                                            BGColor = "Yellow";
                                        else
                                            BGColor = "white";

                                        $(tblRow).find("td:eq(0)").css("background-color", BGColor);
                                        $(tblRow).find("td:eq(0)").css("padding", "15px");
                                        $(tblRow).find("td:eq(0)").css("border-radius", "6px");
                                        $(tblRow).find("td:eq(0) a").css("color", "white");
                                        $(tblRow).find("td:eq(0)").attr("title", MachineStatus);
                                    }
                                }
                                else {
                                    if (statdata != null && statdata != undefined && statdata.length > 0) {
                                        if (statdata[0].MachineLiveStatus == "Running") {
                                            $(tblRow).find("td:eq(1)").html('<div class="loaders-container1" title="Running" style="display:inline-block;"><div class="la-cog la-2x" style="float: left;"><div class="Running"></div></div></div><div style="font-size: x-small; color: green;white-space: nowrap;">Running</div>');
                                            //<div style="float: right;">Running</div>
                                        }
                                        else {
                                            var color = '';
                                            var status = '';
                                            if (statdata[0].MachineLiveStatus == 'Down') {
                                                color = '#E65C00';
                                            } else {
                                                color = 'black';
                                            }
                                            if (statdata[0].MachineLiveStatus != 'NoData') {
                                                status = statdata[0].MachineLiveStatus;
                                            } else {
                                                status = '&nbsp';
                                            }
                                            $(tblRow).find("td:eq(1)").html(('<div class="loaders-container1" style="display:inline-block;" title="' + statdata[0].MachineLiveStatus + '"><div class="la-cog la-2x" style="float: left;"><div style="color:' + statdata[0].MachineStatusColor + ';"></div></div></div><div style="font-size: x-small; color:' + color + ';white-space: nowrap;">' + status + '</div>'));
                                            //<div style="display:inline-block;font-size:14px;position:relative;top:-13px">' + (statdata[0].MachineLiveStatus == "NoData" ? "" : statdata[0].MachineLiveStatus) + '</div>
                                        }
                                    }
                                }

                            });
                        }
                        initTimer();
                    },
                    error: function (jqXHR, textStatus, err) {
                        //  alert('Error: ' + err);
                        initTimer();
                    }
                });
            }
            else {
                clearTimeout(timer);
            }
        }
        function IsPageEnabled(paggeName) {
            var pageEnabled = false;
            $.ajax({
                async: false,
                type: "POST",
                url: "IonicView.aspx/isPageEnabled",
                contentType: "application/json; charset=utf-8",
                data: '{pageName:"' + paggeName + '"}',
                dataType: "json",
                success: function (response) {
                    debugger;
                    pageEnabled = response.d;

                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            return pageEnabled;
        }

        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        $.fn.tooltipOnOverflow = function (options) {
            $(this).on("mouseenter", function () {
                if (this.offsetWidth < this.scrollWidth) {
                    options = options || { placement: "auto" }
                    options.title = $(this).text();
                    $(this).tooltip(options);
                    $(this).tooltip("show");
                } else {
                    if ($(this).data("bs.tooltip")) {
                        $tooltip.tooltip("hide");
                        $tooltip.removeData("bs.tooltip");
                    }
                }
            });
        };

        function CountNumberOfBox() {
            divH = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());

            divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            $("#divCustomerList .myItem").width(divW);
            screenH = $(window).height() - 170;//screen.availHeight - 100;
            screenW = $(window).width() - 180;//screen.availWidth - 5;
            totalH = Math.floor(screenH / (divH));
            totalW = Math.floor(screenW / (divW));
            totalBox = Math.floor(totalH * totalW);
            showItem = totalBox;
            console.log(showItem);
        }

        function SetIconicBoxWidth() {
            divH = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());

            divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            $("#divCustomerList .myItem").width(divW);
        }

        function showFirstNItems() {
            $("#divCustomerList .myItem").hide();
            $("#divCustomerList .myItem").slice(0, showItem).show();
        }

        function NextAndPreviusFun() {
            $("#btnNext").show();
            showFirstNItems();
        }

        $("#btnPrevious").click(function () {
            var c = $("#divCustomerList .myItem").length;
            if (currentTab == c - 1 || currentTab != 0) {
                currentTab = currentTab - showItem;
                showNextNItems();
            }
            if (currentTab <= 0) {
                $("#btnPrevious").hide();
                $("#btnNext").show();
            }
            else {
                $("#btnNext").show();
            }
        });

        $("#btnNext").click(function () {
            var c = $("#divCustomerList .myItem").length;
            if (currentTab < c - 1) {
                currentTab = currentTab + showItem;
                showNextNItems();
            }
            if (currentTab >= c - showItem) {
                $("#btnNext").hide();
                $("#btnPrevious").show();
            }
            else {
                $("#btnPrevious").show();
                $("#btnNext").show();
            }
        });

        function showNextNItems() {
            var nextItems = $("#divCustomerList .myItem:visible ~ .myItem:hidden").slice(0, showItem);
            if (nextItems.length == 0) {
                // if Next items ended
                showFirstNItems();
                //nextItems = $("#divCustomerList .myItem").slice(0, showItem); // then start from first again
            }
            if (nextItems.length > 0) {
                $("#divCustomerList .myItem:visible").hide();
                nextItems.show();
            }
        }

        $("#chkBox").click(function () {
            if (this.checked) {
                currentTab = 0;
                $("#divCustomerList .myItem").show();
                $("#divCustomerList .myItem").css('display', 'inline-block');
                CountNumberOfBox();
                NextAndPreviusFun();
            }
            else {
                $("#divCustomerList .myItem").show();
                $("#btnPrevious").hide();
                $("#btnNext").hide();
            }
        });

        $("[id$=chkAutoBox]").click(function () {
            $("[id$=btnTrigger]").trigger("click");
            // return false;
        });

        $(document).on("click", "[id^='MainContent_LstCustomers_lnkMachine_']", function () {
            var machineId = $(this).text();
            window.open("VDGScreen.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val() + "&Page=table", "VDGScreen.aspx");
            return false;
        });

        $(document).on("click", ".HyperLink", function (e) {
            var classList = $(this).attr("class");
            var parameterName = "";
            if (classList.includes("HL_")) {
                var splitData = classList.split(" ");
                for (var i = 0; i < splitData.length; i++) {
                    if (splitData[i].includes("HL_")) {
                        let split1 = splitData[i].split("_");
                        parameterName = split1[split1.length - 1];
                        break;
                    }
                }
            }
            var machine = $(this).attr('machineName');
            if (parameterName == "" || parameterName == "OEE") {
                window.open("oeeGraphics.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val());
            }
            else if (parameterName == "AirPressure") {
                PopupCenter("AirPressureData.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val(), "Air Pressure", 900, 500);
            }
            else if (parameterName == "SpindleRuntime") {
                PopupCenter("SpindleRunTimeInfoLG.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val(), "Spindle RunTime", 900, 500);
            }
        })
        function PopupCenter(url, title, w, h) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=yes,toolbar=no,resizable=yes,width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            setControl();
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

            //$("#divCustomerList").height(winHeight - 280);

            $('[data-toggle="tooltip"]').tooltip();

            RefreshMachineStatus();
            function CountNumberOfBox() {
                divH = Math.max.apply(null, $('.myItem').map(function () {
                    return $(this).outerHeight(true);
                }).get());

                divW = Math.max.apply(null, $('.myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                screenH = $(window).height() - 170;//screen.availHeight - 100;
                screenW = $(window).width() - 180;//screen.availWidth - 5;
                totalH = Math.floor(screenH / (divH));
                totalW = Math.floor(screenW / (divW));
                totalBox = Math.floor(totalH * totalW);
                showItem = totalBox;
                console.log(showItem);
            }

            function showFirstNItems() {
                $("#divCustomerList .myItem").hide();
                $("#divCustomerList .myItem").slice(0, showItem).show();
            }

            function NextAndPreviusFun() {
                $("#btnNext").show();
                showFirstNItems();
            }


            $("#btnPrevious").click(function () {
                var c = $("#divCustomerList .myItem").length;
                if (currentTab == c - 1 || currentTab != 0) {
                    currentTab = currentTab - showItem;
                    showNextNItems();
                }
                if (currentTab <= 0) {
                    $("#btnPrevious").hide();
                    $("#btnNext").show();
                }
                else {
                    $("#btnNext").show();
                }
            });

            $("#btnNext").click(function () {
                var c = $("#divCustomerList .myItem").length;
                if (currentTab < c - 1) {
                    currentTab = currentTab + showItem;
                    showNextNItems();
                }
                if (currentTab >= c - showItem) {
                    $("#btnNext").hide();
                    $("#btnPrevious").show();
                }
                else {
                    $("#btnPrevious").show();
                    $("#btnNext").show();
                }
            });

            function showNextNItems() {
                var nextItems = $("#divCustomerList .myItem:visible ~ .myItem:hidden").slice(0, showItem);
                if (nextItems.length == 0) {
                    // if Next items ended
                    showFirstNItems();
                    //nextItems = $("#divCustomerList .myItem").slice(0, showItem); // then start from first again
                }
                if (nextItems.length > 0) {
                    $("#divCustomerList .myItem:visible").hide();
                    nextItems.show();
                }
            }

            $("#chkBox").click(function () {
                if (this.checked) {
                    currentTab = 0;
                    $("#divCustomerList .myItem").show();
                    $("#divCustomerList .myItem").css('display', 'inline-block');
                    CountNumberOfBox();
                    NextAndPreviusFun();
                }
                else {
                    $("#divCustomerList .myItem").show();
                    $("#btnPrevious").hide();
                    $("#btnNext").hide();
                }
            });

            $("#chkBox").click(function () {
                if (this.checked) {
                    currentTab = 0;
                    $("#divCustomerList .myItem").show();
                    $("#divCustomerList .myItem").css('display', 'inline-block');
                    CountNumberOfBox();
                    NextAndPreviusFun();
                }
                else {
                    $("#divCustomerList .myItem").show();
                    $("#btnPrevious").hide();
                    $("#btnNext").hide();
                }
            });
            $('.ellipsistooltip').tooltipOnOverflow();
            $("[id$=chkAutoBox]").click(function () {
                $("[id$=btnTrigger]").trigger("click");
                // return false;
            });
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            $("[id$=ddlDayShift]").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            $("[id$=btnProcess]").click(function () {
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
                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();

                <%--if (Date.parse(from) > Date.parse(to)) {
                    alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                    $('[id$=txtToDate]').val('');
                    $('[id$=txtToDate]').focus();
                    return false;
                }--%>
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                initTimer();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
