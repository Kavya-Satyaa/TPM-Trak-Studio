<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlanningSheet_Vulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.Vulkan.PlanningSheet_Vulkan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .tblSettings {
            width: 100%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 10px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: large;
            }

        /*.lvPlanningMaster > tbody > tr:first-child > td {
            background-color: #2e6886 !important;
            Color: white;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
        }

        .lvPlanningMaster > tbody > tr:nth-child(odd) > td {
            background-color: #ddd;
        }

        .lvPlanningMaster > tbody > tr:nth-child(even) > td {
            background-color: white;
        }

        .lvInnerView > tbody > tr > td {
            min-width: 80px;
            max-width: 80px;
            min-height: 100%;
            background-color: transparent;
        }*/

        .outer-table {
            height: 1px;
        }

            .outer-table > tbody > tr:first-child td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                text-align: center;
            }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }

        #gridContainer table tr td {
            background-color: #fcfcfc;
            border: 1px solid silver;
            padding: 2px 5px;
        }

        .td-actual-value {
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <div class="row" style="display: flex;">
                    <table class="tblSettings" style="width: 50% !important;margin-left: 10px;">
                        <tr>
                            <td>Frequency</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Width="150px" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="Daily" Text="Daily"></asp:ListItem>
                                    <asp:ListItem Value="15Days" Text="15 Days"></asp:ListItem>
                                    <asp:ListItem Value="Quarterly" Text="Quarterly"></asp:ListItem>
                                    <asp:ListItem Value="Yearly" Text="Yearly"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Year</td>
                            <td>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtYear" CssClass="form-control year" Style="width: 100px"></asp:TextBox>
                            </td>
                            <td runat="server" id="tdMonth">Month</td>
                            <td runat="server" id="tdMonthValue">
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMonth" CssClass="form-control month" Style="width: 100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" Text="View" CssClass="bajaj-btn-style btnclass" ID="btnView" OnClick="btnView_Click" />
                            </td>
                        </tr>
                    </table>
                    <table class="tblSettings" style="width: 40% !important; margin-left: 10px;">
                        <tr>
                            <td style="width: 120px">Start Date</td>
                            <td>

                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtStartDate" CssClass="form-control Date" Style="width: 120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnSave_Click" />
                            </td>

                            <td colspan="2">
                                <asp:Button runat="server" ID="btnPostPone" CssClass="bajaj-btn-style btnclass" Text="Postpone" OnClick="btnPostPone_Click" />
                                <asp:Button runat="server" ID="btnExport" CssClass="bajaj-btn-style btnclass btn-success" Text="Export" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="gridContainer" style="width: 100%; max-height: 80vh; overflow: auto; margin-top: 10px;">
                <asp:ListView runat="server" ID="lvPlanningMaster" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="outer-table" id="gridData">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text="Sl. No." Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                <asp:Label runat="server" ClientIDMode="Static" ID="lblslNo" Text='<%# Eval("SlNo") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text="Machine ID" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                <asp:Label runat="server" ClientIDMode="Static" ID="lblMachineID" Text='<%# Eval("MachineID") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>

                            <td style="padding: 0px; border: 0px;">
                                <asp:ListView runat="server" ID="lvFrequency" ItemPlaceholderID="itemplaceholder2" DataSource='<%# Eval("InnerListViewData") %>'>
                                    <LayoutTemplate>
                                        <table class="inner-table" style="width: 100%">
                                            <tr>
                                                <td runat="server" id="itemplaceholder2"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <td style="min-width: 190px;" runat="server">
                                            <asp:Label runat="server" ClientIDMode="Static" ID="lblDataHeader" Text='<%# Eval("DateColumn") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                            <asp:Label runat="server" ClientIDMode="Static" ID="lblData" Text='<%# Eval("DateValue") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div class="modal fade infoModal" id="postPoneModal" data-backdrop="static" data-keyboard="false" role="dialog" style="z-index: 2000">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content modalContent" style="width: 130%;">
                        <div class="modal-header modalHeader">
                            <h4 class="modal-title">POSTPONE</h4>
                        </div>
                        <div class="modal-body" style="height: 50vh; overflow: auto;">
                            <table style="border: 1px solid black; position: sticky; width: 100%;">
                                <tr style="position: sticky;">
                                    <%--<td>MachineID</td>
                                    <td>
                                        <asp:ListBox runat="server" ID="lbMachineID" ClientIDMode="Static" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                    </td>--%>
                                    <td>Frequency</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPostPoneFrequency" CssClass="form-control">
                                            <asp:ListItem Value="15Days" Text="15 Days"></asp:ListItem>
                                            <asp:ListItem Value="Daily" Text="30 days"></asp:ListItem>
                                            <asp:ListItem Value="Quaterly" Text="Quaterly"></asp:ListItem>
                                            <%--<asp:ListItem Value="Half-Yearly" Text="Half-Yearly"></asp:ListItem>--%>
                                            <asp:ListItem Value="Yearly" Text="Yearly"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Date</td>
                                    <td>
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar" style="color: black"></span>
                                            </span>
                                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtPostPoneDate" CssClass="form-control Date"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <asp:ListView runat="server" ClientIDMode="Static" ID="lvPostPoneGrid">
                                        <LayoutTemplate>
                                            <table class="table table-boardered headerFixer" style="height: 40vh; overflow: auto;">
                                                <tr style="background-color: #2E6886 !important; color: white; border: 1px solid black; font-weight: bold; position: sticky;">
                                                    <th>Machine ID</th>
                                                    <th>Activity Date</th>
                                                    <th>PostPone Action</th>
                                                </tr>
                                                <tr runat="server" id="itemplaceholder"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblPostMachineID" CssClass="form-control" Text='<%# Eval("MachineID") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblActivityDate" CssClass="form-control" Text='<%# Eval("ActivityDate") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkPostPone" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer modalFooter modal-footer">
                            <asp:Button runat="server" ID="btnPostPoneSave" Text="SAVE" OnClick="btnPostPoneSave_Click" CssClass="bajaj-btn-style btnclass" />
                            <input type="button" value="Close" class="bajaj-btn-style btnclass" onclick=" $('#postPoneModal').modal('hide');" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            debugger;
            freezeColumnFromLeft("gridData", 2);
            $('.year').datepicker({
                format: 'yyyy',
                viewMode: "years",
                minViewMode: "years",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('.month').datepicker({
                format: 'M',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#lbMachineID").multiselect({
                includeSelectAllOption: true
            });
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            freezeColumnFromLeft("gridData", 2);
            $('.year').datepicker({
                format: 'yyyy',
                viewMode: "years",
                minViewMode: "years",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('.month').datepicker({
                format: 'mm',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#lbMachineID").multiselect({
                includeSelectAllOption: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
