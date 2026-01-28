<%@ Page Title="Final Inspection Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FinalInspectionReportAdvik.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.FinalInspectionReportAdvik" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <style>
        .dot {
            height: 20px;
            width: 20px;
            background-color: gray;
            border-radius: 50%;
            display: inline-block;
            color: green
        }

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

        #tblStationStatus tr td {
            text-align: center;
            min-width: 100px;
            background-color: white;
        }

        #tblParameter tr td {
            background-color: white;
            border: 1px solid silver;
        }
    </style>
    <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnScrollPos2" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnViewType" ClientIDMode="Static" />
    <table>
        <tr>
            <td class="commanTd" style="vertical-align: middle;">Line</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
            </td>
            <td class="commanTd" style="vertical-align: middle;">Status</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlStatus" ClientIDMode="Static" CssClass="form-control">
                    <asp:ListItem Value="All">All</asp:ListItem>
                    <asp:ListItem Value="OK">OK</asp:ListItem>
                    <asp:ListItem Value="NotOK">Not OK</asp:ListItem>
                    <asp:ListItem Value="Rework">Rework</asp:ListItem>
                    <asp:ListItem Value="Reject">Reject</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <fieldset class="scheduler-border">
                    <legend class="scheduler-border commontd">Search by Slno</legend>
                    <table style="margin: auto">
                        <tr>

                            <td>
                                <asp:TextBox runat="server" ID="txtSlNoSearch" CssClass="form-control" Width="200" AutoCompleteType="Disabled" placeholder="Serial Number..." onkeypress="return slnoControlKeyUp(event)" onkeydown="return slnoControlKeyDown(event)" ClientIDMode="Static" Style="display: inline-block"></asp:TextBox>
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

            <td>
                <fieldset class="scheduler-border">
                    <legend class="scheduler-border commontd">Search by Date</legend>
                    <table>
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">From</td>
                            <td class="input-group" runat="server">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">To</td>
                            <td class="input-group" runat="server">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>&nbsp;<asp:Button runat="server" ID="btnViewDate" Text="View" CssClass="btn btn-info" OnClick="btnViewDate_Click" />
                            </td>
                        </tr>
                    </table>

                </fieldset>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="width: 80%">
                <div style="float: right; margin-left: 20px">
                    <asp:LinkButton runat="server" CssClass="glyphicon glyphicon-download-alt" Style="font-size: 20px;" ID="lnkExportSlnoList" OnClick="lnkExportSlnoList_Click"></asp:LinkButton>
                </div>
                <div id="slnoListContainer" style="margin-top: 10px; height: 25vh; overflow: auto;" class="generalGrid">
                    <asp:GridView runat="server" ID="gvSlnoDetails" AutoGenerateColumns="false" OnRowDataBound="gvSlnoDetails_RowDataBound" OnSelectedIndexChanged="gvSlnoDetails_SelectedIndexChanged" ClientIDMode="Static" EmptyDataText="No Data Found" Style="width: 100%" CssClass="table table-bordered headerFixer gridCss">
                        <Columns>
                            <asp:BoundField DataField="SerialNumber" HeaderText="QR Code" />
                            <asp:BoundField DataField="Model" HeaderText="Model Name" />
                            <asp:BoundField DataField="PartNumber" HeaderText="Part Number" />
                            <asp:BoundField DataField="PlantID" HeaderText="Plant" />
                            <asp:BoundField DataField="Date" HeaderText="Date" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div runat="server" id="stationStatusDiv">
                <fieldset class="scheduler-border" style="width: max-content; display: inline-block">
                    <legend class="scheduler-border commontd">Station Status</legend>
                    <div id="statusContainer" style="text-align: center; margin-top: 20px">
                        <asp:ListView runat="server" ID="lvStatusData" ItemPlaceholderID="placeHolder">
                            <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="placeHolder" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class="myItem" style="margin: 4px; min-width: 100px; display: inline-block; vertical-align: top">
                                    <div>
                                        <div class="" style="padding: 6px; border-radius: 5px; border-top: 5px solid #f39c12; background-color: <%# Eval("BackColor") %>">
                                            <table style='width: 100%;'>
                                                <tr>
                                                    <td style="text-align: center; color: black; font-weight: bold; padding-bottom: 5px;">

                                                        <label style="font-size: 16px; color: <%# Eval("ForeColor") %>;"><%# Eval("MachineID") %></label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered " style='background-color: white; margin-bottom: 4px; text-align: left'>
                                                <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("StatusList") %>' ItemPlaceholderID="addressPlaceHolder">
                                                    <LayoutTemplate>
                                                        <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="min-width: 100px; height: 37px;">
                                                                <asp:Label runat="server" Text='<%# Eval("Label") + " : " %>' Style="font-weight: bold">:</asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </fieldset>
                <div style="vertical-align: top; display: inline-block; margin-left: 20px">
                    <asp:LinkButton runat="server" CssClass="glyphicon glyphicon-download-alt" Style="font-size: 20px;" ID="lnkExportStausParam" OnClick="lnkExportStausParam_Click"></asp:LinkButton>
                </div>
            </div>
            <div style="margin-top: 20px; width: 50%">

                <asp:ListView runat="server" ID="lvFinalData" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table id="tblParameter" class="table table-bordered gridCss">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblLabel" Text='<%# Eval("Label") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnType" ClientIDMode="Static" Value='<%# Eval("Type") %>' />
                                <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("Value") %>' Visible='<%# Eval("Type").ToString()=="PartNo"?true:false %>'></asp:Label>
                                <asp:Label runat="server" ID="lblSlno" Text='<%# Eval("Value") %>' Visible='<%# Eval("Type").ToString()=="Slno"?true:false %>'></asp:Label>

                                <asp:Label runat="server" Text='<%# Eval("Value") %>' Visible='<%# Eval("Type").ToString()=="Label"?true:false %>'></asp:Label>
                                <asp:CheckBox runat="server" ClientIDMode="Static" ID="chkSelect" Checked='<%# Eval("Value").ToString()=="1"?true:false %>' Visible='<%# Eval("Type").ToString()=="Chk"?true:false %>' onclick="return false;" />
                                <asp:HiddenField runat="server" ID="hdnParameterName" ClientIDMode="Static" Value='<%# Eval("ParameterName") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div class="modal fade" id="confirmModal" role="dialog">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content modalContent confirm-modal-content">
                        <div class="modal-header modalHeader confirm-modal-header">
                            <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                        </div>
                        <div>
                            <br />
                            <h4 class="confirm-modal-title">Confirmation!</h4>
                            <br />
                            <span class="confirm-modal-msg" id="confirmMsg">Are you sure you want to delete Records?</span>
                            <asp:HiddenField runat="server" ID="hdnConfirmType" ClientIDMode="Static" />
                        </div>
                        <div class="modal-footer modalFooter modal-footer">
                            <asp:Button runat="server" Text="Yes" ID="btnConfirm" CssClass="confirm-modal-btn" OnClientClick="return clearModalScreen();" />
                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnViewSlno" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="gvSlnoDetails" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="lnkExportSlnoList" />
            <asp:PostBackTrigger ControlID="lnkExportStausParam" />
            <asp:AsyncPostBackTrigger ControlID="lnkRefreshbutton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function openConfirmationModal(param) {
            var msg = "";
            if (param == "approve") {
                msg = "Are you sure you want to approve Records?";
            }
            else if (param == "reject") {
                msg = "Are you sure you want to reject Records?";
            }
            $('#hdnConfirmType').val(param);
            $('#confirmMsg').text(msg);
            $('#confirmModal').modal('show');
        }
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
        function clearModalScreen() {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            return true;
        }
        function setControls() {
            $("#txtFromDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US'
            });
            $("#txtToDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: 'en-US'
            });
            $("#TextBox1").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: 'en-US'
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        var bigDiv = document.getElementById('slnoListContainer');
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            var bigDiv = document.getElementById('slnoListContainer');
            $(document).ready(function () {
                setControls();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
