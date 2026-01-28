<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkOrderTrackingPitti.aspx.cs" Inherits="Web_TPMTrakDashboard.Pitti.WorkOrderTrackingPitti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <style>
        .p1table tr td {
            background-color: #fcfcfc;
            padding: 2px 5px;
        }

        .p1table > tbody > tr > td {
            border: 1px solid silver;
        }

        .tbl-opn tr td {
            border: 1px solid silver;
        }

        .tbl-machinestatus tr td {
            border: 1px solid silver;
        }

        .outer-table {
            height: 1px;
        }

            .outer-table > tbody > tr:first-child td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                position: sticky;
                top: 0px;
            }
            
            .outer-table > tbody > tr:nth-child(2) td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                position: sticky;
                height: 40px;
                top: 0px;
            }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }


        .opn-header {
            width: 600px;
            max-width: 600px;
            min-width: 600px;
            text-align: center;
            background-color: #2E6886;
        }

        .opn-content {
            /*  width: 180px;
            max-width: 180px;
            min-width: 180px;*/
            border: 0px !important;
        }

        .tbl-machinestatus tr td {
            width: 120px;
            max-width: 120px;
            min-width: 120px;
        }

        .tbl-machinestatus {
            /*table-layout: fixed;*/
        }

        .manual-status-icon {
            font-size: 20px;
            margin-left: 5px;
        }

        .manual-status-green {
            color: green;
        }

        .manual-status-red {
            color: red;
        }
    </style>

    <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnScrollPosLeft" ClientIDMode="Static" />
    <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
        <tr>
            <td class="commanTd" style="vertical-align: middle;">From Date</td>
            <td>
                <div class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                </div>
            </td>
            <td class="commanTd" style="vertical-align: middle;">To Date</td>
            <td>
                <div class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                </div>
            </td>
            <td>
                <asp:LinkButton ClientIDMode="Static" runat="server" CssClass="glyphicon glyphicon-remove-sign" ToolTip="Clear Date" Font-Size="18" ID="lnkClearDate" OnClick="lnkClearDate_Click" ForeColor="White"></asp:LinkButton>
            </td>
            <td class="commanTd" style="vertical-align: middle;">Work Order</td>
            <td>
                <asp:TextBox ID="txtWorkOrderSearch" runat="server" CssClass="form-control" placeholder="Search.." ClientIDMode="Static"></asp:TextBox>
            </td>
            <td class="commanTd" style="vertical-align: middle;">Serial No.</td>
            <td>
                <asp:TextBox ID="txtSerialNoSearch" runat="server" CssClass="form-control" placeholder="Contain search.." ClientIDMode="Static"></asp:TextBox>
            </td>
            <td>
                <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return viewValidation();" />
                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="max-height: 80vh; overflow: auto; margin-top: 20px;" id="gridContainer">
                <asp:ListView runat="server" ID="lvWokrOrder" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="p1table outer-table">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="WorkOrder" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <asp:Label runat="server" Text="" Visible='<%# Eval("HeaderVisible2") %>'></asp:Label>
                                <asp:Label runat="server" ID="lblWorkOrder" Text='<%# Eval("WorkOrder") %>' Visible='<%# Eval("ContentVisible") %>' ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Component ID" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <asp:Label runat="server" Text="" Visible='<%# Eval("HeaderVisible2") %>'></asp:Label>
                                <asp:Label runat="server" ID="lblComponentID" Text='<%# Eval("ComponentID") %>' Visible='<%# Eval("ContentVisible") %>' ClientIDMode="Static"></asp:Label>
                            </td>
                            <td style="min-width: 200px;">
                                <asp:Label runat="server" Text="Serial Number" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <asp:Label runat="server" Text="" Visible='<%# Eval("HeaderVisible2") %>'></asp:Label>
                                <asp:Label runat="server" ID="lblSerialNumber" Text='<%# Eval("SerialNumber") %>' Visible='<%# Eval("ContentVisible") %>' ClientIDMode="Static"></asp:Label>
                            </td>
                            <td style="padding: 0px; border: 0px;">
                                <asp:ListView runat="server" ID="lvOperation" ClientIDMode="Static" DataSource='<%# Eval("OperationList") %>' ItemPlaceholderID="itemplaceholder1">
                                    <LayoutTemplate>
                                        <table class="inner-table tbl-opn">
                                            <tr>
                                                <td runat="server" id="itemplaceholder1"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <td style="padding: 0px;" class='<%# Eval("HeaderVisible").ToString()=="True" || Eval("HeaderVisible2").ToString() == "True" ?"opn-header":"opn-content" %>'>
                                            <div runat="server" style="width: 100%;" visible='<%# Eval("HeaderVisible") %>'>
                                                <asp:Label runat="server" ID="lblOperationNo" Text='<%# Eval("OperationNo") %>' ClientIDMode="Static"></asp:Label>
                                            </div>
                                            <%--  <div runat="server" style="width: 100%;" visible='<%# Eval("ContentVisible") %>'>--%>
                                            <%-- <div runat="server" visible='<%# Eval("Template1Visible") %>'>--%>
                                            <table class="tbl-machinestatus inner-table " runat="server" visible='<%# Eval("Template1Visible") %>'>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# Eval("MachineID") %>' Visible='<%# !Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                        <asp:Label runat="server" Text="MC" Visible='<%# Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# Eval("UpdatedBy") %>' Visible='<%# !Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                        <asp:Label runat="server" Text="OPR" Visible='<%# Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# Eval("StartTime") %>' Visible='<%# !Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                        <asp:Label runat="server" Text="StTime" Visible='<%# Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# Eval("EndTime") %>' Visible='<%# !Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                        <asp:Label runat="server" Text="NdTime" Visible='<%# Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" Text='<%# Eval("CycleTime") %>' Visible='<%# !Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                        <asp:Label runat="server" Text="CycTime" Visible='<%# Convert.ToBoolean(Eval("HeaderVisible2")) %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</div>--%>
                                            <%--  <div runat="server" visible='<%# Eval("Template2Visible") %>' style="display: inline-grid; width: 300px; border: 1px solid silver;">--%>
                                            <table class="inner-table opn-header" runat="server" visible='<%# Eval("Template2Visible") %>'>
                                                <tr>
                                                    <td runat="server" visible='<%# Eval("HeaderVisible2") %>'></td>
                                                    <td runat="server" visible='<%# !Convert.ToBoolean(Eval("HeaderVisible2")) %>'>Manual Cycle <i class="manual-status-icon glyphicon <%# Eval("ManualStatus").ToString()=="Ok"?"glyphicon-ok-circle manual-status-green":"glyphicon-remove-circle manual-status-red" %>"></i>
                                                    </td>
                                                </tr>
                                            </table>

                                            <%-- </div>--%>
                                            <%--  </div>--%>
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Rejection" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <div runat="server" visible='<%# Eval("ContentVisible") %>'>
                                    <asp:Button runat="server" ID="btnRejection" CssClass="btn btn-primary" OnClick="btnRejection_Click" Visible='<%# Eval("RejectionVisible") %>' OnClientClick="return rejectionValidation(this);" Text="Reject" />
                                </div>
                            </td>
                            <td style="min-width: 200px;">
                                <asp:Label runat="server" Text="Rejection Remarks" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <asp:TextBox runat="server" ID="txtRejectionRemarks" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("RejectionRemarks") %>' Visible='<%# Eval("ContentVisible") %>' Style="max-width: unset;" Enabled='<%# Eval("RejectionVisible") %>'></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Rejection By" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <asp:Label runat="server" Text='<%# Eval("RejectionBy") %>' Visible='<%# Eval("ContentVisible") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            setControls();
        });
        function viewValidation() {
            if ($('#txtWorkOrderSearch').val() == "" && $('#txtSerialNoSearch').val() == "") {
                openWarningModal_1("Please enter WorkOrder or Serial No.");
                return false;
            }
            return true;
        }
        function rejectionValidation(ctrl) {
            if ($(ctrl).closest('tr').find('#txtRejectionRemarks').val() == "") {
                openWarningModal_1("Please enter Remarks.");
                return false;
            }
            return true;
        }
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
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            $('[id*=hdnScrollPosLeft]').val(bigDiv.scrollLeft);
        }
        window.onload = function () {
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            bigDiv.scrollLeft = $('[id*=hdnScrollPosLeft]').val();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                bigDiv.scrollLeft = $('[id*=hdnScrollPosLeft]').val();
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
                $('[id*=hdnScrollPosLeft]').val(bigDiv.scrollLeft);
            }
            window.onload = function () {
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                bigDiv.scrollLeft = $('[id*=hdnScrollPosLeft]').val();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
