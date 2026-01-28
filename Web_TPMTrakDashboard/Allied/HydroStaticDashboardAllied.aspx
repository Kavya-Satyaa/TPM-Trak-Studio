<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HydroStaticDashboardAllied.aspx.cs" Inherits="Web_TPMTrakDashboard.Allied.HydroStaticDashboardAllied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>

    <style>
        .outer-table {
            height: 1px;
        }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }

        .outer-table > tbody > tr:first-child td {
            background-color: #2E6886 !important;
            color: white;
            font-weight: bold;
        }

        .table {
            margin: 0px;
        }

        .tabletd {
            margin-bottom: 0px;
        }

        .tabletd > tbody > tr > td {
            /*min-width:135px !important;
        max-width: 135px !important;
        overflow-wrap: break-word;*/
            border-top: 0px;
            border-collapse: collapse;
            width: 120px;
            min-width: 120px;
            max-width: 120px;
        }

        .tableborder > tbody > tr > td {
            border-right: 1px solid #ddd;
            border-left: 1px solid #ddd;
            overflow-wrap: break-word;
        }

        .class1 {
            background-color: #2e6886;
        }

        .class2 {
            background-color: #fff;
        }

        .Headertbl{
            text-align:center;
        }
        #tblOuter > tbody > tr:first-child {
            position:sticky;
            top:-1px;
            z-index:1000;
        }
        .Btns{
            border-radius:2px;
        }
    </style>

    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>
        <ContentTemplate>
            <div class="bajaj-outer-div-filter-section" style="margin-bottom: 10px;">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl" style="width: 90%;">
                        <tr>
                            <td>Line ID</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlLineID" AutoPostBack="true" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="ddlLineID_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>Group ID</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlGroupID" AutoPostBack="false" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>Date</td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="BtnSearch" runat="server" CssClass="bajaj-btn-style Btns" Text="Search" ForeColor="White" OnClick="BtnSearch_Click" />
                            </td>
                            <td>UIN Number</td>
                            <td>
                                <asp:TextBox ID="txtUinNo" runat="server" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnsearch2" runat="server" CssClass="bajaj-btn-style Btns" Text="Search" ForeColor="White" OnClick="btnsearch2_Click" />
                            </td>
                            <td>
                                <asp:Button ID="BtnExport" runat="server" CssClass="bajaj-btn-style Btns" Text="Export" ForeColor="White" OnClick="BtnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div style="overflow: auto; height:80vh;">
                <asp:ListView runat="server" ID="lvHSDashboard" ClientIDMode="Static">
                    <LayoutTemplate>
                        <table class="table table-bordered outer-table" id="tblOuter" runat="server">
                            <tr runat="server" id="itemplaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class='<%# Eval("UinNo").ToString() == ""? "class1" : "class2" %>'>
                                <asp:Label runat="server" ID="lblHeader1" Text="UIN Number" Visible='<%# Eval("HeaderEnable") %>' ForeColor="White" CssClass="Headertbl"></asp:Label>
                                <asp:Label runat="server" ID="lblContent" Text='<%# Eval("UinNo") %>' Visible='<%# Eval("ContentEnable") %>' Font-Bold="true"></asp:Label>
                            </td>
                            <td class='<%# Eval("ComponentID").ToString() == ""? "class1" : "class2" %>'>
                                <asp:Label runat="server" ID="lblHeader2" Text="Component ID" Visible='<%# Eval("HeaderEnable") %>' ForeColor="White" CssClass="Headertbl"></asp:Label>
                                <asp:Label runat="server" ID="lblComponentID" Text='<%# Eval("ComponentID") %>' Visible='<%# Eval("ContentEnable") %>' Font-Bold="true"></asp:Label>
                            </td>
                            <td runat="server" style="padding: 0px !important;">
                                <asp:ListView runat="server" ID="lvMachineData" DataSource='<%# Eval("machineData") %>'>
                                    <LayoutTemplate>
                                        <table class="table inner-table" style="width: 100%;">
                                            <tr>
                                                <td id="itemplaceholder" runat="server"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <td style="padding: 0px; margin: 0px;">
                                            <table class="table inner-table tableborder tabletd" style="width: 100%;">
                                                <tr runat="server" visible='<%# Eval("HeaderEnable") %>' style="border-bottom: 1px solid #ddd; border-right: 1px solid #ddd;">
                                                    <th colspan="4" class="text-center class1">
                                                        <asp:Label runat="server" ID="lblOpeartionHeader" Text='<%# Eval("OperationNo")%>' ForeColor="White"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr runat="server" visible='<%# Eval("HeaderEnable") %>'>
                                                    <td class="class1">
                                                        <asp:Label runat="server" ID="lblMachineID" Text="Machine ID" ForeColor="White"></asp:Label></td>
                                                    <td class="class1">
                                                        <asp:Label runat="server" ID="lblOperatorName" Text="Operator Name" ForeColor="White"></asp:Label></td>
                                                    <td class="class1">
                                                        <asp:Label runat="server" ID="lblStartTime" Text="Start Time" ForeColor="White"></asp:Label></td>
                                                    <td class="class1">
                                                        <asp:Label runat="server" ID="lblEndTime" Text="End Time" ForeColor="White"></asp:Label></td>
                                                </tr>
                                                <tr runat="server" visible='<%# Eval("ContentEnable") %>'>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label1" Text='<%# Eval("MachineID") %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label2" Text='<%# Eval("OperatorName") %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label3" Text='<%# Eval("StartTime") %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label4" Text='<%# Eval("EndTime") %>'></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setDateControl();
            freezeColumnFromLeft('tblOuter', 2);
        });

        function setDateControl() {
            $("#txtDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        //function freezeColumnFromLeft() {        //    debugger;        //    let tblName = tblId;        //    let tblRow = $('[id$=tblOuter] tr');        //    if (tblRow.length > 0) {        //        let rowth = tblRow[0].children;        //        var width = 0;        //        let columnCount = freezColNo;        //        for (var i = 0; i < columnCount; i++) {
        //            let zindex = 800;
        //            $("#" + tblName + " > tbody > tr > td:nth-child(" + (i + 1) + ")").css({
        //                "position": "sticky", "left": width + "px", "z-index": (zindex - 1)
        //            });
        //            $("#" + tblOuter + "  > tbody > tr > th:nth-child(" + (i + 1) + ")").css({ "position": "sticky", "left": width + "px", "z-index": zindex });
        //            width = width + (parseInt($(rowth[i]).css('width').replace('px', '')) - 2);
        //        }        //        $("#" + tblId + " > tbody > tr:first-child td:lt(" + columnCount + ")").css({ "z-index": "999" });        //    }        //}

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setDateControl();
                freezeColumnFromLeft('tblOuter', 2);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
