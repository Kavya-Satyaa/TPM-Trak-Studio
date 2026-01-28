<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GEAChildPartMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.GEAChildPartMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <script src="../Cumi/Scripts/SearchableDropDown/select2.min.js"></script>
    <link href="../Cumi/Scripts/SearchableDropDown/select2.min.css" rel="stylesheet" />
    <style>
        #rbImportType td {
            border: 0px;
        }

            #rbImportType td label {
                color: white;
            }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <table class="table table-bordered" id="tblfilter" style="width: fit-content;">
                <tr>

                    <td class="commontd"><b>Production Order</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlProductioOrder" ClientIDMode="Static" CssClass="form-control searchable-dropdown-list" AutoPostBack="true" OnSelectedIndexChanged="ddlProductioOrder_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="commontd"><b>Fabrication Number</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFabricationNo" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" CssClass="btn btn-info" Text="View" OnClick="btnView_Click" />
                    </td>
                    <td>
                        <asp:FileUpload ID="fileUploader" runat="server" Style="width: 400px; height: 40px; display: inline-block; max-width: 200px;" CssClass="form-control input-sm" />
                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rbImportType" RepeatDirection="Horizontal" ForeColor="White" ClientIDMode="Static">
                            <asp:ListItem Text="Append" Value="Append"></asp:ListItem>
                            <asp:ListItem Text="Replace" Value="Replace"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnImport" CssClass="btn btn-info" Text="Import" OnClick="btnImport_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExportTemplate" CssClass="btn btn-info" Text="Export Template" OnClick="btnExportTemplate_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto">
                <asp:GridView ID="gvChildPart" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer  alternate-table-style" ShowHeaderWhenEmpty="true" ClientIDMode="Static">
                    <Columns>
                        <%--<asp:TemplateField HeaderText="Sl. No.">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSortOrder" ClientIDMode="Static" Text='<%# Eval("ItemSortorder") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Order">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("ProductionOrderNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pegged requirement">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("PagedRequirement") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Material">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("MaterialID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Material Description">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("MaterialDescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reservation item">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("ReservationItem") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requirement Quantity (EINHEIT)">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("RequirementQuantity") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base Unit of Measure (EINHEIT)">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("BaseUnitOfMeasure") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity withdrawn (EINHEIT)">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("QuantityWithDrawn") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shortage (EINHEIT)">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("Shortage") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prod. storage bin">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("ProdStorageBin") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Storage Location">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("StorageLocation") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item category">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("ItemCategory") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Procurement type">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("ProcurementType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Spare part indicator">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("SparePartIndicator") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Open Quantity (EINHEIT)">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("OpenQuantity") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fabrication No.">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompID" ClientIDMode="Static" Text='<%# Eval("FabricationNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImport" />
            <asp:PostBackTrigger ControlID="btnExportTemplate" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $(".searchable-dropdown-list").select2({
                placeholder: "Search",
                allowClear: true
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControls();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
