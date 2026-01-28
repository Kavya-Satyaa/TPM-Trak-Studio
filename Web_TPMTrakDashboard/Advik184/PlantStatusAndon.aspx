<%@ Page Title="Plant Status Andon" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlantStatusAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.PlantStatusAndon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .dot {
            height: 20px;
            width: 20px;
            background-color: gray;
            border-radius: 50%;
            display: inline-block;
            color: green
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Timer runat="server" ID="timer1" ClientIDMode="Static" Interval="5000" OnTick="timer1_Tick"></asp:Timer>
            <div style="    display: flex;justify-content: space-between;">
                <table style="margin-top: 10px; width: 100px; float: left">
                    <tr>
                        <td class="commanTd" style="vertical-align: middle;">Plant&nbsp;</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" Style="width: auto"></asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                </table>
                <div style="float: right;">
                    <label id="lblRefreshTime" style="margin-top: 10px; font-size: 20px; color: white" class="subheaderlbl">Refresh Time : <%: DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt")%></label>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-9">
                    <div style="margin-top: 20px; height: 70vh;">
                        <asp:ListView runat="server" ID="lvStatusData" ItemPlaceholderID="placeHolder">
                            <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="placeHolder" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class="myItem" style="margin: 4px; min-width: 100px; display: inline-block; vertical-align: top">
                                    <div>
                                        <div class="" style="padding: 6px; border-radius: 5px; border-top: 5px solid #f39c12; background-color: <%# Eval("BackColor") %>">
                                            <table style='width: 100%;'>
                                                <tr>
                                                    <td style="text-align: center; color: black; font-weight: bold; padding-bottom: 5px;">

                                                        <label style="font-size: 16px; color: <%# Eval("ForeColor") %>;"><%# Eval("MachineID") %></label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered " style='background-color: white; margin-bottom: 4px; text-align: left'>
                                                <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("StatusList") %>' ItemPlaceholderID="addressPlaceHolder">
                                                    <LayoutTemplate>
                                                        <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="min-width: 100px; height: 37px;">
                                                                <asp:Label runat="server" Text='<%# Eval("Label")+" : " %>' Style="font-weight: bold">:</asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div class="col-md-3">
                    <div style="width: 100px; display: inline-block;margin-top:25vh">
                        <table>
                            <tr>
                                <td style="white-space: nowrap"><span class="dot" style='background-color: #92d050' runat="server"></span>&nbsp;<span style="color: white; vertical-align: super;font-size:16px">Running</span></td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap"><span class="dot" style='background-color: Yellow' runat="server"></span>&nbsp;<span style="color: white; vertical-align: super;font-size:16px">Before Threshold</span></td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap"><span class="dot" style="background-color: <%= thresholdColor %>"></span>&nbsp;<span style="color: white; vertical-align: super;font-size:16px">After Threshold</span></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlPlant" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="timer1" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
