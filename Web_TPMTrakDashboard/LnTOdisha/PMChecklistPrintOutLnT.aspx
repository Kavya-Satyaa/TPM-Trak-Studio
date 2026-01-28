<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMChecklistPrintOutLnT.aspx.cs" Inherits="Web_TPMTrakDashboard.LnTOdisha.PMChecklistPrintOutLnT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <style>
        .checklist-tbl {
            width: 100%;
        }

            /*.checklist-tbl tr td:nth-child() {
                width: 50%;
            }*/

            .checklist-tbl tr td {
                border: 1px solid black;
                background-color: white;
                padding: 5px;
                font-size: 14px;
                font-weight: bold;
            }

                .checklist-tbl tr td label {
                    font-size: 14px;
                    font-weight: bold;
                }

        #printArea .manhourtd {
            font-size: 16px;
        }

        .tr-actualdata td, .tr-actualdata td label {
            font-weight: unset !important;
        }

        @media print {
            body * {
                visibility: hidden;
            }

            #printArea,
            #printArea * {
                visibility: visible;
                font-size: 9px;
                /*aspect-ratio: 2/3;*/
            }

                #printArea .form-control {
                    height: auto;
                    line-height: normal;
                }

                #printArea .manhourtd {
                    font-size: 9px;
                }

            #printArea {
                position: absolute;
                /*width: 100%;*/
                /*text-align: center;*/
                left: 15px;
                right:15px;
                top: 5px;
                bottom: 5px;
            }
        }

        .tblSettings {
            width: 100%;
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
    </style>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div tabindex="0">
                <table id="tblFilter" class="tblSettings" style="width: auto;">
                    <tr>
                        <td class="commanTd" style="vertical-align: middle;">Year</td>
                        <td>
                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>
                        </td>
                        <td class="commanTd" style="vertical-align: middle;">Month</td>
                        <td>
                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" Style="width: 70px; display: inline;"></asp:TextBox>
                        </td>
                        <td class="commanTd" style="vertical-align: middle;">Machine</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" OnClientClick="return saveValidation();" OnClick="btnSave_Click" TabIndex="1" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnPrint" Text="Print" CssClass="btn btn-primary" OnClientClick="return printChecklist();" TabIndex="2" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="printArea" style="margin-top: 10px;" tabindex="0">
                <asp:ListView runat="server" ID="lvChecklist" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="checklist-tbl">
                            <tr>
                                <td colspan="6" style="font-size: 20px; font-weight: bold; color: blue; text-align: center;">
                                    <asp:Label runat="server" ID="lblMachineID" Style="text-decoration-line: underline;" ClientIDMode="Static"></asp:Label>
                                    </br>
                                    <asp:Label runat="server" ID="lblMachineNumber" Style="text-decoration-line: underline;" ClientIDMode="Static"></asp:Label>
                                </td>
                               <%-- <td colspan="4" style="font-weight: bold; color: blue; text-align: center;" class="manhourtd">MAN HOUR ( EVERY PM ACTIVITY) - 170 min<br />
                                    MAN HOUR ( ALL ACTIVITY) - 425 min
                                </td>--%>
                            </tr>
                            <tr>
                                <td colspan="2">ISO Doc. No. : PP/PMD/PM/1
                                </td>
                                <td rowspan="2" style="color: #922962;">Frequency
                                </td>
                                <td rowspan="6">
                                    <label style="transform: rotate(-90deg); color: #922962;">
                                        Alloted Time for
                                        <br />
                                        executing the activity</label>
                                </td>
                                <td rowspan="2" style="color: #922962;">Last Checked
                                </td>
                                <td rowspan="2" style="color: #922962;">Today's PM</td>
                            </tr>
                            <tr>
                                <td colspan="2">Revision-1 Dated 25/05/2013</td>
                            </tr>
                            <tr>
                                <td colspan="2" style="min-height: 30px; height: 30px"></td>
                                <td></td>
                                <td>
                                    <asp:Label runat="server" ID="lblLastCheckedDate" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblScheduleDate" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="color: red;">TEAM LEADER</td>
                                <td></td>
                                <td>
                                    <asp:Label runat="server" ID="lblLastCheckedTLEntry" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td style="background-color: #e3b3b3;">
                                    <asp:TextBox runat="server" ID="txtTeamLeaderEntry" ClientIDMode="Static" focus="true" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="color: green;">CRAFTS :</td>
                                <td></td>
                                <td>
                                    <asp:Label runat="server" ID="lblLastCheckedCrew" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td style="background-color: #e3b3b3;">
                                    <asp:TextBox runat="server" ID="txtCrew" ClientIDMode="Static" CssClass="form-control" TabIndex="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Sl No.</td>
                                <td>ACTIVITY :</td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr runat="server" visible='<%#Eval("ActivityVisible") %>' class="tr-actualdata">
                            <td><%#Eval("SlNo") %></td>
                            <td><%# Eval("Activity")  %></td>
                            <td><%# Eval("Frequency")  %></td>
                            <td><%# Eval("AllotedTime")  %></td>
                            <td><%# Eval("LastCheckedRemark")  %></td>
                            <td><%# Eval("TodayRemark")  %></td>
                        </tr>
                        <tr runat="server" visible='<%#Eval("CategoryVisible") %>'>
                            <td colspan="10" style="text-decoration: underline;">
                                <%# Eval("Category")  %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setDateTimePicker();
            callUnLoader();
            if ($("#txtTeamLeaderEntry").val() == "")
                $("#txtTeamLeaderEntry").focus();
            else
                $("#txtCrew").focus();
        });
        function saveValidation() {
            if ($('#lblScheduleDate').text() == "") {
                openWarningModal_1("Schedule Date is required.");
                return false;
            }
            if ($('#txtTeamLeaderEntry').val().trim() == "") {
                openWarningModal_1("TL entry is required.");
                return false;
            }
            if ($('#txtCrew').val().trim() == "") {
                openWarningModal_1("Crew Details is required.");
                return false;
            }
            callLoader();
            return true;
        }
        function printChecklist() {
            window.print();
            var printContents = document.getElementById("printArea").innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            document.body.innerHTML = originalContents
            return false;
        }
        function setDateTimePicker() {
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
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtPlanDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();
                if ($("#txtTeamLeaderEntry").val() == "")
                    $("#txtTeamLeaderEntry").focus();
                else
                    $("#txtCrew").focus();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
