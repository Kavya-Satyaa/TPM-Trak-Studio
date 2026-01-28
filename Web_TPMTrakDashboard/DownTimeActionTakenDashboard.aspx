<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="DownTimeActionTakenDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.DownTimeActionTakenDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .OuterTable > tbody > tr:nth-child(1) {
            line-height: 40px;
        }
        .headergrid {
            width: 100%;
            border: 0;
            margin: 0;
            padding: 0;
        }

            .headergrid > tbody > tr > th {
                line-height: 40px;
                background-color: #ed9f72;
                color: white;
                text-align: center;
            }

            .headergrid > tbody > tr:nth-child(odd) {
                line-height: 35px;
                color: black;
                background-color: white;
            }

            .headergrid > tbody > tr:nth-child(even) {
                line-height: 35px;
                color: black;
                background-color: #e9e5e2;
            }

        .headergrid > tbody > tr:nth-child(1) {
            top: 0;
            position: sticky;
        }
    </style>
    <div class="text-center" style="width: 100%; display: inline-block;">
        <div style="font-size: 18px; font-weight: 900; margin-bottom: 5px; color: white;">
            MachineID: <%= Session["MachineId"]%>&nbsp;&nbsp;
            <asp:Label runat="server" ID="lblStartDate"></asp:Label>
            <asp:Label runat="server" ID="lblEndDate"></asp:Label>
            <span style="font-size: 10px"></span>
            <div style="float: right; width: 200px;">
                <asp:DropDownList runat="server" ID="ddlDownReasons" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlDownReasons_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
    </div>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="text-align: center; display: flex; flex-wrap: wrap; justify-content: center; align-content: center; margin-top: 10px;">
                <div class="col-lg-6">
                    <table class="OuterTable" style="width: 100%">
                        <tr>
                            <td colspan="3" class="text-center" style="background-color: #93b98b; color: black;">
                                <asp:Label runat="server" ID="lblDownReason" Text="Down Reason: " Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height: 80vh; overflow: auto">
                                    <asp:GridView runat="server" ID="gvDownTimeActionTaken" AutoGenerateColumns="false" CssClass="headergrid" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Down Start Time">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbldownStartTime" Text='<%# Eval("DownStartTime") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Down End Time">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDownEndTime" Text='<%# Eval("DownEndTime") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblRemarks" Text='<%# Eval("Remarks") %>' ClientIDMode="Static"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action Taken">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblActionTaken" Text='<%# Eval("ActionTaken") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
