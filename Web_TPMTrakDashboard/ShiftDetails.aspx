<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShiftDetails.aspx.cs" Inherits="Web_TPMTrakDashboard.ShiftDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
        }

        #MainContent_gridShiftDetails tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        #MainContent_gridShiftDetails tbody tr:nth-child(even) {
            background-color: #FFFFFF;
        }
        .auto-style7 {
            width: 120px;
        }

        .table-bordered {
        }

        .auto-style15 {
            color: white;
            width: 133px;
        }

        .auto-style17 {
            color: white;
            width: 133px;
            height: 36px;
        }

        .auto-style18 {
            width: 120px;
            height: 36px;
        }

        .auto-style20 {
            height: 36px;
        }

        .auto-style21 {
            width: 250px;
            height: 36px;
        }

        .auto-style24 {
            width: 219px;
        }

        .auto-style27 {
            color: white;
            width: 136px;
            height: 36px;
        }

        .auto-style28 {
            color: white;
            width: 136px;
        }
         .tblSettings {
            width: 90%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 5px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: medium;
                /*box-shadow: 2px 2px 2px black;*/
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }
    </style>

    <asp:UpdatePanel ID="updatePnal" runat="server">
        <ContentTemplate>
            <div style="margin-left: 10px;">
                <div class="row text-center">
                    <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
                </div>

                <div class="row">
                    <table class="table table-bordered" style="width: 99%;">
                        <tr>
                            <td class="auto-style17" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ShiftID") %></td>
                            <td class="auto-style18">
                                <asp:DropDownList ID="ddlShiftID" runat="server" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlShiftID_SelectedIndexChanged" AutoPostBack="true" Height="30px" Width="57px">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                </asp:DropDownList></td>

                            <td class="auto-style27" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","FromDay") %></td>
                            <td class="auto-style20">
                                <asp:DropDownList ID="ddlFromDay" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="Today">Today</asp:ListItem>
                                    <asp:ListItem Value="Tomorrow">Tomorrow</asp:ListItem>
                                    <asp:ListItem Value="Yesterday">Yesterday</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style27" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ToDay") %></td>
                            <td class="auto-style20">
                                <asp:DropDownList ID="ddlToDay" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="Today">Today</asp:ListItem>
                                    <asp:ListItem Value="Tomorrow">Tomorrow</asp:ListItem>
                                    <asp:ListItem Value="Yesterday">Yesterday</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style21">
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources: CommanResource, Save %>" CssClass="btn btn-info" OnClick="btnSave_Click" Style="display: inline" />&nbsp;
                                <asp:Button ID="btnHour" runat="server" Text="<%$ Resources: CommanResource, HourDefinition %>" CssClass="btn btn-info" Style="display: inline" />
                                &nbsp;</td>
                        </tr>

                        <tr>
                            <td class="auto-style15" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ShiftName") %></td>
                            <td class="auto-style7">
                                <asp:TextBox ID="txtShiftName" runat="server" CssClass="form-control" Width="100px" AutoCompleteType="Disabled"></asp:TextBox></td>
                            <td class="auto-style28" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","FromTime") %></td>
                            <td class="input-group">
                                <div class="input-group-addon" style="height: 34px">
                                    <i class="glyphicon glyphicon-time"></i>
                                </div>
                                <asp:TextBox ID="txtFromTime" runat="server" CssClass="form-control date" Height="34px" AutoCompleteType="Disabled"></asp:TextBox></td>
                            <td class="auto-style28" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ToTime") %></td>
                            <td class="input-group">
                                <div class="input-group-addon" style="height: 34px">
                                    <i class="glyphicon glyphicon-time"></i>
                                </div>
                                <asp:TextBox ID="txtToTime" runat="server" CssClass="form-control date" Height="34px" AutoCompleteType="Disabled"></asp:TextBox></td>
                            <td class="auto-style24">
                                <asp:Button ID="btnClear" runat="server" Text="<%$ Resources: CommanResource, ClearAll %>" CssClass="btn btn-info" OnClick="btnClear_Click" />&nbsp;&nbsp;</td>
                        </tr>

                    </table>

                    <asp:GridView ID="gridShiftDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" Width="54%">
                        <Columns>
                            <asp:BoundField DataField="ShiftID" HeaderText="<%$ Resources: CommanResource, ShiftID %>" />
                            <asp:BoundField DataField="ShiftName" HeaderText="<%$ Resources: CommanResource, ShiftName %>" />
                            <asp:BoundField DataField="FromDay" HeaderText="<%$ Resources: CommanResource, FromDay %>" />
                            <asp:BoundField DataField="FromTime" HeaderText="<%$ Resources: CommanResource, FromTime %>" />
                            <asp:BoundField DataField="ToDay" HeaderText="<%$ Resources: CommanResource, ToDay %>" />
                            <asp:BoundField DataField="ToTime" HeaderText="<%$ Resources: CommanResource, ToTime %>" />
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <script type="text/javascript">
        $(document).ready(function () {

            $.unblockUI({});
            $(".date").datetimepicker({
                format: 'HH:mm:ss',
                // format: 'LT'
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $(".date").datetimepicker({
                format: 'HH:mm:ss',
                // format: 'LT'
            });
        });

        $(document).on("click", "[id$=btnClear]", function (e) {
            if (confirm("Do you want to clear all data?")) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            }
            else {
                return false;
            }
        });

        $(document).on("change", "[id$=ddlShiftID]", function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "[id$=btnSave]", function () {

            // var dateFormate = parseDMY(previousDate);

            var fromTime = $("[id$=txtFromTime]").val();
            var toTime = $('[id$=txtToTime]').val();

            var stt = new Date("November 13, 2000 " + fromTime);
            stt = stt.getTime();
            if (($("[id$=ddlToDay]").val() == "Tomorrow")) {
                var endt = new Date("November 14, 2000 " + toTime);
                endt = endt.getTime();
            }
            else {
                var endt = new Date("November 13, 2000 " + toTime);
                endt = endt.getTime();
            }



            if ($("[id$=txtShiftName]").val() == "") {
                alert("Please enter the Shift Name !!");
                $("[id$=txtShiftName]").focus();
                return false;
            }
            if ($("[id$=txtFromTime]").val() == "") {
                alert("Please enter the From Time !!");
                $("[id$=txtFromTime]").focus();
                return false;
            }
            if ($("[id$=txtToTime]").val() == "") {
                alert("Please enter the To Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }
            if ((($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Today")) ||
                (($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Yesterday")) ||
                (($("[id$=ddlFromDay]").val() == "Yesterday") && ($("[id$=ddlToDay]").val() == "Tomorrow")) ||
                (($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Yesterday")) ||
                (($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Today"))) {
                alert("Please enter Valid Days. !!");
                $("[id$=ddlFromDay]").focus();
                return false;
            }
            if ((($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Today")) && (stt > endt)) {
                alert("Please enter Valid Timings. From Time Cannot Be Greater than To Time !!");
                $("[id$=txtFromTime]").focus();
                return false;
            }
            if (stt == endt) {
                alert("Please enter Valid Timings. To Time Cannot Be  Equal to  the From Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }
            if (endt < stt) {
                alert("Please enter Valid Timings. From Time Cannot Be  Less Than the To Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }
            if (($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Tomorrow")) {

                var difference = endt - stt;
                if (difference > 86400000) {
                    alert("Please enter Valid Timings. Time Is Greater than 24 hrs. Please Check the End time !!");
                    $("[id$=txtToTime]").focus();
                    return false;
                }
            }
            if ((($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Today")) && (stt == endt) ||
                (($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Tomorrow")) && (stt == endt) ||
                (($("[id$=ddlFromDay]").val() == "Yesterday") && ($("[id$=ddlToDay]").val() == "Yesterday")) && (stt == endt)) {
                alert("Please enter Valid Timings. From Time Cannot Be Equal To the To Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }

            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        })

        $(document).on("click", "[id$=btnHour]", function () {
            let shiftID = $("[id$=ddlShiftID]").val();
            let ShiftName = $("[id$=txtShiftName]").val();
            PopupCenter("HourDefinitionPage.aspx?ShiftID=" + shiftID + "&ShiftName=" + ShiftName, "Shift Working Hour", 900, 500);
            return false;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
