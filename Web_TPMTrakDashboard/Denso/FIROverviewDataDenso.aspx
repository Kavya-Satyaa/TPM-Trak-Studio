<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FIROverviewDataDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.FIROverviewDataDenso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <style>
        .date1 {
            width: 120px !important;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">From Date</td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">To Date</td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine ID</td>
                    <td>
                        <asp:ListBox ID="lbMachineID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Down Category</td>
                    <td id="tdDownCategory">
                        <asp:ListBox ID="lbDownCategory" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl" AutoPostBack="true" OnSelectedIndexChanged="lbDownCategory_SelectedIndexChanged"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Down Id</td>
                    <td>
                        <asp:ListBox ID="lbDownID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 75vh; overflow: auto;" id="gridContainer">
                <asp:GridView ID="gvFIRData" CssClass="table table-bordered headerFixer alternate-table-style" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static" ShowFooter="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Plant ID">
                            <ItemTemplate>
                                <asp:Label ID="lblPlantID" runat="server" Text='<%# Eval("PlantID") %>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Machine ID">
                            <ItemTemplate>
                                <asp:Label ID="lblMachineID" runat="server" Text='<%# Eval("MachineID") %>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Down Category">
                            <ItemTemplate>
                                <asp:Label ID="lblDownCategory" runat="server" Text='<%# Eval("DownCategory") %>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Down ID">
                            <ItemTemplate>
                                <asp:Label ID="lblDownID" runat="server" Text='<%# Eval("DownID") %>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Down Desc">
                            <ItemTemplate>
                                <asp:Label ID="lblDownDesc" runat="server" Text='<%# Eval("DownDesc") %>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblStartTime" runat="server" Text='<%# Eval("StartTime") %>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Time">
                            <ItemTemplate>
                                <asp:Label ID="lblEndTime" runat="server" Text='<%# Eval("EndTime") %>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkAction" ClientIDMode="Static" CssClass="glyphicon glyphicon-arrow-right" OnClientClick="moveToFIRTransScreen(this);"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle CssClass="FooterRow" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            setControls();
        });
        function moveToFIRTransScreen(ctrl) {
            var row = $(ctrl).closest('tr');
            var machineid = $(row).find('#lblMachineID').text();
            var downCategory = $(row).find('#lblDownCategory').text();
            var downID = $(row).find('#lblDownID').text();
            var startTime = $(row).find('#lblStartTime').text();
            var endTime = $(row).find('#lblEndTime').text();
            window.open("FIRTransDataDenso.aspx?plantID=" + $(row).find('#lblPlantID').text() + "&machineID=" + machineid + "&downCategory=" + downCategory + "&downID=" + downID + "&startTime=" + startTime + "&endTime=" + endTime + "&downDesc=" + $(row).find('#lblDownDesc').text(), "FIR Data");
            return false;
        }
        function setControls() {

            $('.date1').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                endDate: '1d'
            });
            $('.listBoxControl').multiselect({
                includeSelectAllOption: true
            });

        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        var multiselctListExpand = false;
        function openMultiselectPopup() {
            debugger;
            multiselctListExpand = true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                setTimeout(function () {
                    if (multiselctListExpand) {
                        $("#tdDownCategory .btn-group").addClass('open');
                    } else {
                        $("#tdDownCategory .btn-group").removeClass('open');
                    }
                    multiselctListExpand = false;
                }, 30);
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
