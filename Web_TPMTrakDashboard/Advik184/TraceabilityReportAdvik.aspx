<%@ Page Title="Traceability Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TraceabilityReportAdvik.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.TraceabilityReportAdvik" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <style>
        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
            margin-bottom: 0px;
        }

        .gridCss tbody tr {
            background-color: white;
            color: black;
        }

        .lblDiv {
            padding: 3px;
            background: #2e6886;
            color: white;
            font-size: 18px;
            font-weight: bold;
            text-align: center;
            border: 1px solid white;
        }
    </style>


    <div class="container-fluid">
        <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnScrollPos2" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnViewType" ClientIDMode="Static" />
        <table>
            <tr>
                <td class="commanTd" style="vertical-align: middle;">Line</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td class="commanTd" style="padding-left: 10px; vertical-align: middle;">Station</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                </td>
                <td style="padding-left: 10px">
                    <fieldset class="scheduler-border">
                        <legend class="scheduler-border commontd">Search by QR Code</legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSlNoSearch" CssClass="form-control" Width="200" AutoCompleteType="Disabled" placeholder="Contains with..." onkeypress="return slnoControlKeyUp(event)" onkeydown="return slnoControlKeyDown(event)" ClientIDMode="Static" Style="display: inline-block"></asp:TextBox>
                                    <img src="Images/Scanner.jpg" style="width: 60px; height: 50px" />
                                    <img src="Images/Kayboard.png" style="width: 70px; height: 50px" />
                                </td>
                                <td>&nbsp;
                        <asp:LinkButton runat="server" ID="lnkRefreshbutton" ClientIDMode="Static" OnClick="lnkRefreshbutton_Click" CssClass="glyphicon glyphicon-refresh" Style="font-size: 22px; font-weight: bold; color: white"></asp:LinkButton>
                                </td>
                                <td>&nbsp;<asp:Button runat="server" ID="btnViewSlno" Text="View" CssClass="btn btn-info" OnClick="btnViewSlno_Click" Visible="false" />
                                </td>
                            </tr>
                        </table>

                    </fieldset>
                </td>
                <td style="padding-left: 10px">
                    <fieldset class="scheduler-border">
                        <legend class="scheduler-border commontd">Search by Date</legend>
                        <table>
                            <tr>
                                <td class="commanTd" style="vertical-align: middle;">From</td>
                                <td class="input-group" runat="server">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                                </td>
                                <td class="commanTd" style="padding-left: 10px; vertical-align: middle;">To</td>
                                <td class="input-group" runat="server">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;<asp:Button runat="server" ID="btnViewDate" Text="View" CssClass="btn btn-info" OnClick="btnViewDate_Click" />
                                </td>
                            </tr>
                        </table>

                    </fieldset>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div id="slnoDataContainer" style="height: 80vh; overflow: auto; margin-top: 10px;">
                    <asp:ListView runat="server" ID="lvSlnoData" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered headerFixer gridCss">
                                <tr>
                                    <th>QR Code</th>
                                    <th>Machine</th>
                                    <th>Model</th>
                                    <th>Start Time</th>
                                    <th>End Time</th>
                                    <th>Elapsed Time</th>
                                    <th>Value</th>
                                    <th>Result</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td rowspan='<%# Eval("RowSpan") %>' style='vertical-align: middle; display: <%# Eval("RowSpan").ToString()=="0"?"none":"" %>'><%# Eval("QRCode") %></td>
                                <td><%# Eval("Machine") %></td>
                                <td><%# Eval("ModelName") %></td>
                                <td><%# Eval("StartTime") %></td>
                                <td><%# Eval("EndTime") %></td>
                                <td><%# Eval("ElapsedTime") %></td>
                                <td><%# Eval("Value") %></td>
                                <td><%# Eval("Result") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnViewDate" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnViewSlno" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkRefreshbutton" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        $('#gvSlnoList tr td').click(function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        });
        $('#gvSlnoTraceability tr td').click(function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        });
        var firstSNCharCount = 0;
        var firstSNCharPosition = "";
        var completeSNScan = true;
        var inputval;
        var temptime;
        var enterCondition = 0;
        var completePNScan = true;
        var firstPNCharCount = 0;
        var firstPNCharPosition = "";
        var slnoinputvalTimeout;
        function slnoControlKeyUp(e) {
            if (e.keyCode == 13) {
                completeSNScan = true;
                firstSNCharCount = 0;
                firstSNCharPosition = "";
                if (enterCondition == 1) {
                    debugger;

                    enterCondition = 0;
                    temptime = "";
                    clearTimeout(slnoinputvalTimeout);
                    $('#txtSlNoSearch').blur();
                    e.preventDefault();
                    __doPostBack('<%= btnViewSlno.UniqueID%>', '');
                }
                else {
                    e.preventDefault();
                    console.log("Enter 13");
                    return false;
                }
            }
            return true;
        }

        function slnoControlKeyDown(e) {
            var timeout = 3000;
            clearTimeout(slnoinputvalTimeout);
            var timeDiff;
            var today = new Date();
            var time = today.getMilliseconds();
            if (temptime != undefined || temptime != "") {
                timeDiff = parseInt(temptime) - time;
                if (timeDiff < 0) {
                    timeDiff = timeDiff * -1;
                }
                if (timeDiff <= 10) {
                    timeout = 1000;
                }
                else {
                    timeout = 3000;
                    enterCondition = 1;
                }
            }
            if (timeout == 1000 && completeSNScan == true) { //scan slno number again and again. It will remove old data and take new data
                var txtSNValue = $('#txtSlNoSearch').val();
                console.log("TXT =" + txtSNValue);
                var newChar = txtSNValue.substring(txtSNValue.length - 1, txtSNValue.length);
                console.log("New Char" + newChar);
                $('#txtSlNoSearch').val("");
                $('#txtSlNoSearch').val(newChar);
                completeSNScan = false;
            }

            temptime = time;
            slnoinputvalTimeout = setTimeout(function () {
                completeSNScan = true;
                firstSNCharCount = 0;
                firstSNCharPosition = "";

                $('#txtSlNoSearch').blur();
                temptime = "";
                enterCondition = 0;
                __doPostBack('<%= btnViewSlno.UniqueID%>', '');
            }, timeout);
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                //bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                // bigDiv2.scrollTop = $('[id*=hdnScrollPos2]').val();
                setControls();
                $.unblockUI({});
            });
            $('#gvSlnoList tr td').click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                return true;
            });
            $('#gvSlnoTraceability tr td').click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                return true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
