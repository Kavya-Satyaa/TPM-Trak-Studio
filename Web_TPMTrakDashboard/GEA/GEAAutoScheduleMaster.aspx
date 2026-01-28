<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GEAAutoScheduleMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.GEAAutoScheduleMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <table class="table table-bordered" id="tblfilter" style="width: fit-content;">
                <tr>
                    <td class="commontd" style="vertical-align: middle"><b>Type</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem Text="Auto Schedule" Value="AutoSchedule"></asp:ListItem>
                            <asp:ListItem Text="Send To Store" Value="StoresSetting"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="commontd" style="vertical-align: middle"><b>Machine ID</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachineId" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="commontd" style="vertical-align: middle"><b>Component ID</b></td>
                    <td id="tdComponentID">
                        <asp:TextBox runat="server" ID="txtCompSearch" ClientIDMode="Static" placeholder="Search.." CssClass="form-control" Style="display: inline-block; width: 200px"></asp:TextBox>
                        <asp:LinkButton runat="server" ID="lnkCompSearch" ClientIDMode="Static" OnClick="lnkCompSearch_Click" CssClass="glyphicon glyphicon-search"></asp:LinkButton>&nbsp;&nbsp;
                        <asp:ListBox ID="lbCompID" runat="server" SelectionMode="Multiple" Width="150" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="lbCompID_SelectedIndexChanged"></asp:ListBox>

                    </td>
                    <td class="commontd" style="vertical-align: middle"><b>Operation No.</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlOpnNo" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" CssClass="btn btn-info" Text="View" OnClick="btnView_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-info" Text="Save" OnClick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnClear" CssClass="btn btn-info" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto;">
                <asp:GridView ID="gvAutoSchedule" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer alternate-table-style" ShowHeaderWhenEmpty="true" OnRowDataBound="gvAutoSchedule_RowDataBound" ClientIDMode="Static">
                    <Columns>
                        <asp:TemplateField HeaderText="Component ID">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("ComponentID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation No.">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOpnNo" ClientIDMode="Static" Text='<%# Eval("OperationNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Machine ID">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnExistMachineID" ClientIDMode="Static" Value='<%# Eval("MachineID") %>' />
                                <asp:HiddenField runat="server" ID="hdnAutoScheduleMachineID" ClientIDMode="Static" Value='<%# Eval("MachineID") %>' />
                                <asp:RadioButtonList runat="server" ID="rbAutoScheduleMachineID" ClientIDMode="AutoID" CssClass="entryControl" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvStoresSetting" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer alternate-table-style" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Component ID">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("ComponentID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation No.">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOpnNo" ClientIDMode="Static" Text='<%# Eval("OperationNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Machine ID">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To be sent to Stores">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkToBeSentToStores" ClientIDMode="Static" Checked='<%# Eval("ToBeSentToStores") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lbCompID" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <script>

        $(document).ready(function () {
            setControls();
            $('#tdComponentID .multiselect-container.dropdown-menu').on('scroll', function () {
                $('[id*=hdnScrollPos]').val($('#tdComponentID .multiselect-container.dropdown-menu').scrollTop());
            });
        });
        function setControls() {
            $('[id$=lbCompID]').multiselect({
                includeSelectAllOption: true,
                //enableFiltering: true,
                maxHeight: 500,
                buttonWidth: '300px',
                //enableCaseInsensitiveFiltering: true
            });
        }
        $("#gvAutoSchedule").on("click", "td .entryControl", function () {
            $(this).closest('tr').find("#hdnUpdate").val("update");
        });
        var multiselctListExpand = false;
        function openMultiselectPopup() {
            debugger;
            multiselctListExpand = true;
        }
        $('#tdComponentID ul.dropdown-menu').on('mousedown', 'li', function (event) {
            debugger;
            if ($('#tdComponentID ul.dropdown-menu li.active').length >= 20 && (!$(this).hasClass("active"))) {
                alert("Parameter Cannot be greater than 20");
                event.preventDefault();
            }
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControls();
                debugger;
                setTimeout(function () {
                    if (multiselctListExpand) {
                        $(".btn-group").addClass('open');
                    } else {
                        $(".btn-group").removeClass('open');
                    }
                    multiselctListExpand = false;
                    $('#tdComponentID .multiselect-container.dropdown-menu').scrollTop($('[id*=hdnScrollPos]').val());
                }, 30);

                $('#tdComponentID .multiselect-container.dropdown-menu').on('scroll', function () {
                    $('[id*=hdnScrollPos]').val($('#tdComponentID .multiselect-container.dropdown-menu').scrollTop());
                });
            });
            $('#tdComponentID ul.dropdown-menu').on('mousedown', 'li', function (event) {
                debugger;
                if ($('#tdComponentID ul.dropdown-menu li.active').length >= 20 && (!$(this).hasClass("active"))) {
                    alert("Parameter Cannot be greater than 20");
                    event.preventDefault();
                }
            });
            $("#gvAutoSchedule").on("click", "td .entryControl", function () {
                $(this).closest('tr').find("#hdnUpdate").val("update");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
