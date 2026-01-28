<%@ Page Language="C#" Title="JagadevaAndon" AutoEventWireup="true" CodeBehind="JagdevDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.JagdevDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/Checkbox/jquery-3.3.1.js"></script>
    <link href="Scripts/Checkbox/bootstrap4-toggle.min.css" rel="stylesheet" />
    <script src="Scripts/Checkbox/bootstrap4-toggle.min.js"></script>
    <script src="Scripts/Checkbox/DatePickerFor331/bootstrap.js"></script>
    <link href="Scripts/Checkbox/DatePickerFor331/bootstrap.css" rel="stylesheet" />
    <%-- <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>--%>

    <script src="Scripts/Checkbox/DatePickerFor331/moment.js"></script>
    <script src="Scripts/Checkbox/DatePickerFor331/bootstrap-datetimepicker.min.js"></script>
    <link href="Scripts/Checkbox/DatePickerFor331/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <style>
        body {
            /*font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;*/
            font-family: system-ui;
            font-size: 13px;
        }

        .commonstyle-fortable {
            width: 100%;
            border-collapse: collapse;
        }

        .header-table > tbody > tr > td {
            border: 2px solid black;
        }

        .header-table > tbody > tr > th {
            border-left: 2px solid black;
            border-right: 2px solid black;
        }

        .shift-table > tbody > tr > td, .shift-table > tbody > tr > th {
            border: 2px solid black;
        }
        .shift-table > tbody> tr >td:last-child{
            display: none;
        }
        .cell-table > tbody > tr > td {
            border: 1px solid black;
        }

        .cell-table > tbody > tr > th {
            border-top: 1px solid black;
        }

        .cell-table > tbody > tr > td:first-child {
            border-left: unset;
        }

        .cell-table > tbody > tr > td:last-child {
            border-right: unset;
        }

        .machine-table > tbody > tr > td {
            border: 1px solid black;
            width: 40px;
            max-width: 40px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            line-height:1.1;
        }

        .machine-table > tbody > tr:first-child > td {
            border-top: unset;
        }

        .machine-table > tbody > tr:last-child > td {
            border-bottom: unset;
        }

        .machine-table > tbody > tr > td:first-child {
            border-left: unset;
        }

        .machine-table > tbody > tr > td:last-child {
            border-right: unset;
        }

        .legend-table {
            border-collapse: collapse;
            width: 80%;
            margin: auto;
        }

            .legend-table > tbody > tr > td {
                border: 1px solid black;
            }

        .toggle {
            width: 100px;
        }

            .toggle label {
                font-size: 13px;
            }

           .btn-warning{
               height: 15px;
           }
        .toggle-group .btn {
            padding: 0px 12px;
        }
        .toggle label {
            font-size: 10px;
            font-weight: bold;
        }
        .toggle.btn{
            min-height: 15px;
        }
       #tt > .btn-warning{
            height: 15px !important;
        }
    </style>
</head>
<body style="background-color: #060623;">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <asp:Timer runat="server" ID="dashboardInterval" OnTick="dashboardInterval_Tick"></asp:Timer>
            <div>
                <table style="margin-left: 4%; margin-top: 5px">
                    <tr>
                        <td id="tt">
                            <%--  <input type="checkbox" runat="server" class="btn btn-lg" autopostback="true" data-toggle="toggle" data-on="ANDON" data-off="DESKTOP" id="modeToggle" data-onstyle="warning" data-offstyle="success" enableviewstate="true" onclick="javascript:form1.submit();" onserverchange="modeToggle_ServerChange" />--%>
                            <input type="checkbox" runat="server" class="btn btn-lg" data-toggle="toggle" data-off="ANDON" data-on="DESKTOP" id="modeToggle" data-onstyle="warning" data-offstyle="success" style="  height: 15px;" />
                            <%--enableviewstate="true" onclick="javascript:form1.submit();"--%>
                            <asp:Button runat="server" ID="btnmodaldeToggle" ClientIDMode="Static" OnClick="btnmodaldeToggle_Click" Style="display: none" />

                        </td>
                        <td id="tdDate" runat="server" class="input-group" style="min-width: 150px; border: 0; margin-left: 20px">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Style="min-width: 130px; min-height: 25px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>

                        </td>
                        <td id="tdButton" runat="server">
                            <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" Style="margin-left: 20px; min-height: 25px" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                        </td>

                    </tr>
                </table>
            </div>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>


                    <div style="height: 98vh;" id="mainDiv">
                        <asp:ListView runat="server" ID="lvAndonData">
                            <EmptyDataTemplate>
                                <div style="text-align: center; font-size: 25px; font-weight: bold; color: white">
                                    No Data Found
                                </div>

                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div style="text-align: center;">
                                    <div runat="server" id="itemplaceholder"></div>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div style="width: <%# Eval("DivWidth") %>; display: inline-block; margin-left: 15px; margin-top: 2px; border: 2px solid black; padding: 5px; background-color: white;">
                                    <table class="commonstyle-fortable header-table">
                                        <tr>
                                            <td style="text-align: center; padding: 2px; position: relative; height: 50px">
                                                <img runat="server" src="~/Images/JagdevLogo.png" style="height: 40px; position: absolute; left: 20px; top: 2px" />
                                                <span style=" font-weight: bold; color: #2d00e1; vertical-align: super" class="company-name">JAGDEV ENGG SOLUTIONS PVT LTD</span>
                                            </td>

                                        </tr>
                                        <tr>
                                            <th style="text-align: center; padding: 5px">
                                                <span style="float: left">Date: <%# Eval("Date") %></span>
                                                <span><%# Eval("ParameterId") %></span>
                                            </th>

                                        </tr>
                                        <tr>
                                            <asp:ListView runat="server" ID="lvShiftDetails" DataSource='<%# Eval("JShiftDetails") %>'>
                                                <LayoutTemplate>
                                                    <table class="commonstyle-fortable shift-table">
                                                        <tr>
                                                            <td runat="server" id="itemplaceholder"></td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <td style="text-align: center; padding: 0px">
                                                        <span style="font-weight: bold" class="shift-name"><%# Eval("ShiftId") %></span>
                                                        <asp:ListView runat="server" ID="lvCelltDetails" DataSource='<%# Eval("JCellDetails") %>'>
                                                            <LayoutTemplate>
                                                                <table class="commonstyle-fortable cell-table">
                                                                    <tr runat="server" id="itemplaceholder"></tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <th style="text-align: center; padding: 0px"><span><%# Eval("CellId") %></span> </th>
                                                                </tr>
                                                                <tr>
                                                                    <td style="padding: 0px">
                                                                        <asp:ListView runat="server" ID="lvMachineDetails" DataSource='<%# Eval("JMachineDetails") %>'>
                                                                            <LayoutTemplate>
                                                                                <table class="commonstyle-fortable machine-table" style="height: <%# Eval("JCellDetails.JMachineDetailsHeight") %>">
                                                                                    <tr runat="server" id="itemplaceholder"></tr>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td style="background-color: <%# Eval("BackColor1") %>">
                                                                                        <span style="color: <%# Eval("ForeColor1") %>" title="<%# Eval("MachineID1") %>"><%# Eval("MachineID1") %></span>
                                                                                    </td>
                                                                                    <td style="background-color: <%# Eval("BackColor2") %>">
                                                                                        <span style="color: <%# Eval("ForeColor2") %>" title="<%# Eval("MachineID2") %>"><%# Eval("MachineID2") %></span>
                                                                                    </td>
                                                                                    <td style="background-color: <%# Eval("BackColor3") %>">
                                                                                        <span style="color: <%# Eval("ForeColor3") %>" title="<%# Eval("MachineID3") %>"><%# Eval("MachineID3") %></span>
                                                                                    </td>
                                                                                    <td style="background-color: <%# Eval("BackColor4") %>">
                                                                                        <span style="color: <%# Eval("ForeColor4") %>" title="<%# Eval("MachineID4") %>"><%# Eval("MachineID4") %></span>
                                                                                    </td>
                                                                                    <td style="background-color: <%# Eval("BackColor5") %>">
                                                                                        <span style="color: <%# Eval("ForeColor5") %>" title="<%# Eval("MachineID5") %>"><%# Eval("MachineID5") %></span>
                                                                                    </td>
                                                                                    <td style="background-color: <%# Eval("BackColor6") %>">
                                                                                        <span style="color: <%# Eval("ForeColor6") %>" title="<%# Eval("MachineID6") %>"><%# Eval("MachineID6") %></span>
                                                                                    </td>
                                                                                    <%--<td><span><%# Eval("MachineID") %></span> </td>--%>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:ListView>
                                                                    </td>

                                                                </tr>
                                                                <tr style="height: 5px">
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </td>
                                                    <td></td>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </tr>
                                    </table>

                                    <div style="padding-top: 8px">
                                        <table class="legend-table">
                                            <tr style="display: <%# Eval("EffLegend") %>">
                                                <td style="background-color: green; color: black; font-weight: bold">Prod. Eff. >90%</td>
                                                <td style="background-color: yellow; font-weight: bold">Prod Eff. b/w 60% to 90%</td>
                                                <td style="background-color: red; color: white; font-weight: bold">Prod Eff. < 60%</td>
                                                <td style="background-color: orange; color: black; font-weight: bold">No data capturing</td>
                                                <td rowspan="2" style="font-weight: bold">Legend</td>
                                            </tr>
                                            <tr style="display: <%# Eval("OEELegend") %>">
                                                <td style="background-color: green; color: black; font-weight: bold">OEE >85%</td>
                                                <td style="background-color: yellow; font-weight: bold">OEE b/w 60% to 85%</td>
                                                <td style="background-color: red; color: white; font-weight: bold">OEE < 60%</td>
                                                <td style="background-color: orange; color: black; font-weight: bold">No data capturing</td>
                                                <td rowspan="2" style="font-weight: bold">Legend</td>
                                            </tr>
                                            <tr style="display: <%# Eval("DowntimeLegend") %>">
                                                <td style="background-color: green; color: black; font-weight: bold">Downtime < 5 Mins</td>
                                                <td style="background-color: white"></td>
                                                <td style="background-color: red; color: white; font-weight: bold">Downtime > 5 mins</td>
                                                <td style="background-color: orange; color: black; font-weight: bold">No data capturing</td>
                                                <td rowspan="2" style="font-weight: bold">Legend</td>
                                            </tr>
                                            <tr style="display: <%# Eval("RejLegend") %>">
                                                <td style="background-color: green; color: black; font-weight: bold">Rej % < 3%</td>
                                                <td style="background-color: white"></td>
                                                <td style="background-color: red; color: white; font-weight: bold">Rej %  > 3%</td>
                                                <td style="background-color: orange; color: black; font-weight: bold">No data capturing</td>
                                                <td rowspan="2" style="font-weight: bold">Legend</td>
                                            </tr>
                                            <tr style="display: <%# Eval("DesktopDowntimeLegend") %>">
                                                <td style="background-color: green; color: black; font-weight: bold">Downtime < 1.5 Hrs</td>
                                                <td style="background-color: white"></td>
                                                <td style="background-color: red; color: white; font-weight: bold">Downtime > 1.5 Hrs</td>
                                                <td style="background-color: orange; color: black; font-weight: bold">No data capturing</td>
                                                <td rowspan="2" style="font-weight: bold">Legend</td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #7e6991; color: white; font-weight: bold">M/c breakdown</td>
                                                <td style="background-color: #008ae6; font-weight: bold">Plan Stoppage</td>
                                                <td style="background-color: #fcd5b4; font-weight: bold">No TPM Software</td>
                                                <td style="background-color: #ff00ff; color: white; font-weight: bold">M/c under correction</td>
                                            </tr>
                                        </table>
                                    </div>

                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="dashboardInterval" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
      <%: Scripts.Render("~/bundles/masterjs") %>
    <script>
        $(document).ready(function () {
            $('[id$=txtDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });

            //var Height = $(window).height() - (180);
            //$('#serialNoDetialsContainer').css('height', Height);
            setMachineTableHeight();
        });
        function setMachineTableHeight() {
                if ($('#mainDiv > div').children().length == 1) {
                    // $('#mainDiv').find('.machine-table').css('height', '200px');
                    // $('#mainDiv').find('.machine-table tr td').css('padding', '20px 10px');

                    $('#mainDiv').find('.company-name').css({ 'font-size': '32px' });
                    $('#mainDiv').find('.header-table tr th').css({ 'padding': ' 0px 10px', 'font-size': '28px', 'font-weight': '1000' });
                    $('#mainDiv').find('.shift-name').css({ 'font-size': '28px', 'font-weight': '1000' });
                    $('#mainDiv').find('.cell-table tr th').css({ 'font-size': '28px', 'padding': 'unset', 'font-weight': '1000' });
                    $('#mainDiv').find('.machine-table tr td').css({ 'padding': '0px 2px', 'font-size': '60px', 'font-weight': '1000' });
                    $('#mainDiv').find('.legend-table tr td').css({ 'padding': '0px', 'font-size': '28px','font-weight': '1000' });
                } else {
                    //$('#mainDiv').find('.machine-table').css('height', 'unset');
                    // $('#mainDiv').find('.machine-table tr td').css('padding', '3px 1px');
                    $('#mainDiv').find('.company-name').css({ 'font-size': '22px' });
                    $('#mainDiv').find('.header-table tr th').css({ 'padding': 'unset', 'font-size': '20px', 'font-weight': '800' });
                    $('#mainDiv').find('.shift-name').css({ 'font-size': '20px', 'font-weight': '800' });
                    $('#mainDiv').find('.cell-table tr th').css({ 'font-size': '20px', 'padding': 'unset', 'font-weight': '800' });
                    $('#mainDiv').find('.machine-table tr td').css({ 'padding': '0px 1px', 'font-size': '21px', 'font-weight': '800' });
                    $('#mainDiv').find('.legend-table tr td').css({ 'padding': '0px', 'font-size': '15px','font-weight': '800' });
                }
            }
        $('#modeToggle').change(function () {
            debugger;

            //_doPostBack('btnmodaldeToggle');
            __doPostBack('<%= btnmodaldeToggle.UniqueID%>', '');
        });
        function modeChange() {
            return true;
        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
       
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
         
            $(document).ready(function () {
                $('[id$=txtDate]').datetimepicker({
                    format: 'DD-MM-YYYY',
                    locale: 'en-US'
                });

                //var Height = $(window).height() - (180);
                //$('#serialNoDetialsContainer').css('height', Height);
                setMachineTableHeight();
            });
         function setMachineTableHeight() {
                if ($('#mainDiv > div').children().length == 1) {
                    // $('#mainDiv').find('.machine-table').css('height', '200px');
                    // $('#mainDiv').find('.machine-table tr td').css('padding', '20px 10px');

                    $('#mainDiv').find('.company-name').css({ 'font-size': '32px' });
                    $('#mainDiv').find('.header-table tr th').css({ 'padding': ' 0px 10px', 'font-size': '28px', 'font-weight': '1000' });
                    $('#mainDiv').find('.shift-name').css({ 'font-size': '28px', 'font-weight': '1000' });
                    $('#mainDiv').find('.cell-table tr th').css({ 'font-size': '28px', 'padding': 'unset', 'font-weight': '1000' });
                    $('#mainDiv').find('.machine-table tr td').css({ 'padding': '0px 2px', 'font-size': '60px', 'font-weight': '1000' });
                    $('#mainDiv').find('.legend-table tr td').css({ 'padding': '0px', 'font-size': '28px','font-weight': '1000' });
                } else {
                    //$('#mainDiv').find('.machine-table').css('height', 'unset');
                    // $('#mainDiv').find('.machine-table tr td').css('padding', '3px 1px');
                    $('#mainDiv').find('.company-name').css({ 'font-size': '22px' });
                    $('#mainDiv').find('.header-table tr th').css({ 'padding': 'unset', 'font-size': '20px', 'font-weight': '800' });
                    $('#mainDiv').find('.shift-name').css({ 'font-size': '20px', 'font-weight': '800' });
                    $('#mainDiv').find('.cell-table tr th').css({ 'font-size': '20px', 'padding': 'unset', 'font-weight': '800' });
                    $('#mainDiv').find('.machine-table tr td').css({ 'padding': '0px 1px', 'font-size': '21px', 'font-weight': '800' });
                    $('#mainDiv').find('.legend-table tr td').css({ 'padding': '0px', 'font-size': '15px','font-weight': '800' });
                }
            }
            $('#modeToggle').change(function () {
                debugger;

                //_doPostBack('btnmodaldeToggle');
                __doPostBack('<%= btnmodaldeToggle.UniqueID%>', '');
            });
            function modeChange() {
                return true;
            }
        });

    </script>
</body>

</html>
