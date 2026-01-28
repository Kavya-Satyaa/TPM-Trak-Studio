<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TargetScreenGEA.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.TargetScreenGEA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <%: Styles.Render("~/bundles/datecss") %>
            <%: Scripts.Render("~/bundles/datejs") %>
            <%: Styles.Render("~/bundles/toastrCss") %>
            <%: Scripts.Render("~/bundles/toastrJs") %>
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td>
                                <%--<asp:Label runat="server" ID="lblPlantID" Text="PlantID" ClientIDMode="Static" CssClass="form-control"></asp:Label>--%>
                                <span>PlantID</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlantID" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <span>Employee Name</span>
                            </td>
                            <td runat="server">
                                <div runat="server" id="divEmployeeSearch" style="display: inline-block;">
                                    <asp:TextBox runat="server" ID="txtEmployeeName" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 150px" placeholder="Contains.."></asp:TextBox>
                                    
                                </div>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="VIEW" OnClick="btnView_Click" ClientIDMode="Static" CssClass="bajaj-btn-style" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="SAVE" OnClick="btnSave_Click" ClientIDMode="Static" CssClass="bajaj-btn-style" />
                            </td>

                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 100%">
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px; min-width: 40%; display: inline-block">
                    <asp:ListView runat="server" ID="lvEmpTargets" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style">
                                <tr>
                                    <th>Employee ID</th>
                                    <th>Employee Name</th>
                                    <th>Target Value</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEmpID" runat="server" ClientIDMode="Static" Text='<%# Eval("EmployeeID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEmpName" runat="server" ClientIDMode="Static" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTargetValue" runat="server" ClientIDMode="Static" Text='<%# Eval("TargetValue") %>' CssClass="form-control"></asp:TextBox>

                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
