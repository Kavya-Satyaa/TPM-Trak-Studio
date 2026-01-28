<%@ Page Title="Plan VS Actual" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="TafePlanVsActual.aspx.cs" Inherits="Web_TPMTrakDashboard.TAFE.TafePlanVsActual" %>

<asp:Content ID="MainContentArea" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <script src="/Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="/Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>

    <style type="text/css">
        .ui-datepicker .ui-datepicker-prev, .ui-datepicker .ui-datepicker-next {
            display: none;
        }

        .header-center {
            text-align: center;
        }

        table tr th {
            text-align: center !important;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }
    </style>

    <div class="container-fluid" style="margin-left: -5px;">
        <asp:UpdatePanel ID="UpdatePanelPlanVsActual" runat="server">
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row" style="height: 60px;">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="width: 80px;"><%=GetGlobalResourceObject("CommanResource","PlantID") %></td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                            <td class="commontd" style="width: 100px;"><b><%=GetGlobalResourceObject("CommanResource","LineID") %></b></td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlLineId" runat="server" CssClass="form-control" meta:resourcekey="ddlLineIdResource1" />
                            </td>
                            <td style="width: 120px;">
                                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 100px; display: inline; height: 35px;"></asp:TextBox>
                            </td>
                            <td style="width: 120px;">
                                <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" Style="width: 100px; display: inline; height: 35px;"></asp:TextBox>
                            </td>
                            <td style="width: 140px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View Data" Style="min-width: 120px;" OnClientClick="ValidateData()" OnClick="btnView_Click" />
                            </td>
                            <td style="width: 170px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnExport" Text="Export Data" Style="min-width: 150px;" OnClick="btnExport_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>

                <div style="overflow-x: auto; overflow-y: auto; height: 41vh;">
                    <asp:GridView ID="GridPlanVsActualDaywise" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected plant, line and date time period.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="Pdate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="220" />
                            <asp:BoundField DataField="Line" HeaderText="Line ID" />
                            <asp:BoundField DataField="PartName" HeaderText="Part Name" />
                            <asp:BoundField DataField="PartID" HeaderText="Part ID" />
                            <asp:BoundField DataField="StdCycletime" HeaderText="Std.Cycle Time" />
                            <asp:BoundField DataField="ScheduledQty" HeaderText="Schedule Qty." />
                            <asp:BoundField DataField="ActualQty" HeaderText="Actual Qty." />
                            <asp:BoundField DataField="HoldQty" HeaderText="Hold Qty." />
                            <asp:BoundField DataField="DelayQty" HeaderText="Delay Qty" />
                            <asp:BoundField DataField="RejMaterial" HeaderText="Rej. Material" />
                            <asp:BoundField DataField="RejProcess" HeaderText="Rej. Process" />
                        </Columns>
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>

                <div style="overflow-x: auto; overflow-y: auto; height: 40vh; margin-top: 5px;">
                    <asp:GridView ID="GridPlanVsActualCumulative" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true" OnDataBound="GridPlanVsActualCumulative_DataBound">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected plant, line and date time period.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="PartName" HeaderText="Part Name" />
                            <asp:BoundField DataField="PartID" HeaderText="Part ID" />
                            <asp:BoundField DataField="StdCycletime" HeaderText="Std. Cycle Time" />
                            <asp:BoundField DataField="ScheduledQtyMTD" HeaderText="Schedule Qty." />
                            <asp:BoundField DataField="ActualQtyMTD" HeaderText="Actual Qty." />
                            <asp:BoundField DataField="HoldQtyMTD" HeaderText="Hold Qty." />
                            <asp:BoundField DataField="DelayQtyMTD" HeaderText="Delay Qty." />
                            <asp:BoundField DataField="RejMaterialMTD" HeaderText="Rej. Material" />
                            <asp:BoundField DataField="RejProcessMTD" HeaderText="Rej. Process" />
                            <asp:BoundField DataField="ScheduledQtyYTD" HeaderText="Schedule Qty." />
                            <asp:BoundField DataField="ActualQtyYTD" HeaderText="Actual Qty." />
                            <asp:BoundField DataField="HoldQtyYTD" HeaderText="Hold Qty." />
                            <asp:BoundField DataField="DelayQtyYTD" HeaderText="Delay Qty." />
                            <asp:BoundField DataField="RejMaterialYTD" HeaderText="Rej. Material" />
                            <asp:BoundField DataField="RejProcessYTD" HeaderText="Rej. Process" />
                        </Columns>
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                orientation: "top left"
            });

            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                orientation: "top left"
            });
        });

        function ValidateData() {
            if ($("[id$=ddlPlantId]").val() == "" || $("[id$=ddlPlantId]").val() == undefined || $("[id$=ddlPlantId]").val() == null) {
                alert("Select a Plant ID first.")
                return false;
            }
            else if ($("[id$=ddlLineId]").val() == "" || $("[id$=ddlLineId]").val() == undefined || $("[id$=ddlLineId]").val() == null) {
                alert("Select a Line ID first.")
                return false;
            }
            else if ($("[id$=txtYear]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource", "PleaseEnterYear")%>");
                $("[id$=txtYear]").focus();
                return false;
            }
            else if ($("[id$=txtMonth]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource", "PleaseEnterMonth")%>");
                $("[id$=txtMonth]").focus();
                return false;
            }
            else
                return true;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                orientation: "top left"
            });

            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                orientation: "top left"
            });

            function ValidateData() {
                if ($("[id$=ddlPlantId]").val() == "" || $("[id$=ddlPlantId]").val() == undefined || $("[id$=ddlPlantId]").val() == null) {
                    alert("Select a Plant ID first.")
                    return false;
                }
                else if ($("[id$=ddlLineId]").val() == "" || $("[id$=ddlLineId]").val() == undefined || $("[id$=ddlLineId]").val() == null) {
                    alert("Select a Line ID first.")
                    return false;
                }
                else if ($("[id$=txtYear]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource", "PleaseEnterYear")%>");
                    $("[id$=txtYear]").focus();
                    return false;
                }
                else if ($("[id$=txtMonth]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource", "PleaseEnterMonth")%>");
                    $("[id$=txtMonth]").focus();
                    return false;
                }
                else
                    return true;
            }
        });

    </script>
</asp:Content>
<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
