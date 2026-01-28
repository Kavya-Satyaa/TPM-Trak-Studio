<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cell_SupervisorScreen_PradeepMetals.aspx.cs" Inherits="Web_TPMTrakDashboard.PradeepMetals.Cell_SupervisorScreen_PradeepMetals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
          .searchCategory {
            border: 1px solid white;
        }
           .tblFilter > tbody > tr > td {
            color: white;
            padding: 10px;
            /*background:#8da0b1;*/
            font-weight: bold;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <fieldset class="searchCategory" style="border-color: #373c52; box-shadow: 2px 2px black; margin-left: 9px; width: 40%;">
                    <table class="tblFilter" style="width: 100%;">
                        <tr>
                            <td class="commontd">Plant ID</td>
                            <td style="width: 60%">
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlPlantID" AutoPostBack="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" CssClass="btn btn-info" OnClick="btnView_Click" Text="VIEW" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-info" OnClick="btnSave_Click" Text="SAVE" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>

            <div style="margin-top: 10px;">
                <div style="overflow: auto; height: 80vh; width: 55%;" id="scrollMaintainDiv">
                    <asp:GridView runat="server" ID="gvCellSupervisorDetails" CssClass="table table-bordered headerFixer" EmptyDataText="No Data Found" EmptyDataRowStyle-CssClass="no-data-found-td" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="gvCellSupervisorDetails_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Cell ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCellID" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("CellID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shift">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShift" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("Shift") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Supervisor">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlSupervisors" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
