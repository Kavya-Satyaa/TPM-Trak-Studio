<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleKTAControl.ascx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.ScheduleKTAControl" %>
<style>
    .Green {
        background-color: #06cf06;
    }

    .Red {
        background-color: red;
    }

    .Yellow {
        background-color: yellow;
    }

    .white {
        background-color: white;
    }

    .RunningSchedule {
        /*   -webkit-animation: cog-rotate 2s linear infinite;
        -moz-animation: cog-rotate 2s linear infinite;
        -o-animation: cog-rotate 2s linear infinite;
        animation: rotate 2s linear infinite;*/
        color: green;
    }

    .StoppedSchedule {
        color: red;
    }

    .cellLabel {
        font-size: 22px;
    }

    .addBorder {
        border-radius: 10px;
        border: 0.5px solid #cccccc;
        background-color: white;
    }

    .removeBorder {
        border-radius: 5px;
        border: 0.5px solid #cccccc;
        background-color: white;
    }

    .cellLabelDiv {
        background-color: #0f4987;
        padding: 15px 2px 0px 2px;
    }

        .cellLabelDiv span {
            color: white;
            font-weight: bold;
            font-size: 25px;
            font-family: sans-serif;
        }

    #tblComponentList > tbody > tr > td {
        padding: 2px;
        text-align: left;
    }
</style>
<asp:HiddenField runat="server" ID="hdntdHeight" ClientIDMode="Static" />
<asp:HiddenField runat="server" ID="hdnWidth" ClientIDMode="Static" />
<asp:HiddenField runat="server" ID="hdntotalWidth" ClientIDMode="Static" />
<asp:HiddenField runat="server" ID="hdnWrapContent" ClientIDMode="Static" />
<asp:HiddenField runat="server" ID="hdnScreenHeight" ClientIDMode="Static" />
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:Timer runat="server" ID="flipInterval" ClientIDMode="Static" OnTick="flipInterval_Tick"></asp:Timer>
        <div style="text-align: center" class="cellLabelDiv" runat="server" id="cellLabelDiv">
            <asp:Label runat="server" ID="lblCellName" CssClass="cellLabel"></asp:Label>
        </div>
        <div id="OuterDivContainer">
            <div id="scheduleKTAContainer" class="OuterDivContainerStyle">
                <asp:ListView runat="server" ID="lvScheduleKTA" ItemPlaceholderID="placeHolder">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="placeHolder" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="myItem" style="margin: 2px; min-width: 100px; display: inline-block; vertical-align: top; white-space: nowrap;">
                            <div class="<%= settings.BorderClass %>">
                                <div class="<%# Eval("BackColor") %> <%= settings.BorderClass %>" style="padding: 4px; background-color: <%# Eval("BackColor") %>">
                                    <table style='width: 100%;' class="outercockpit">
                                        <tr>
                                            <td style="text-align: center; color: black; font-weight: bold;" id="tdMachineID">
                                                <%--padding-left: 30px; padding-bottom: 5px;--%>
                                                <label style="font-size: <%# Eval("MachineNameFontSize") %>px;" id="lblMachineID"><%# Eval("MachineId") %></label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table table-bordered " style="background-color: white; margin-bottom: 4px; max-width: 99%; overflow: hidden;" id="tblComponentList">
                                        <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("ComponentList") %>' ItemPlaceholderID="addressPlaceHolder">
                                            <LayoutTemplate>
                                                <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="min-width: 100px;  white-space: nowrap; font-weight: unset;width: 85%; /*padding: 2px; */ /*width: 85%;*/" id="tdComponentStatus">
                                                        <label id="lblComponentStatus" clientidmode="static" style="font-size: <%# Eval("FontSizeInnerData") %>px; margin-bottom: 1px;"><%# Eval("ComponentID")%></label>
                                                        <div id="divScrolling" style="overflow: hidden; display: inline-flex;" class='<%# Eval("LabelScrolling") == "" ? "NoScrollContainer" : "ScrollContainer" %>'>
                                                            <label id="lblScrolling" style="font-size: <%# Eval("FontSizeInnerData")%>px;" clietidmode="static" class="ScrollTextSchedule"><%# Eval("LabelScrolling") %></label>
                                                        </div>
                                                        <asp:Image ImageUrl='<%# Eval("ComponentStatus") %>' Visible='<%# Eval("ComponentStatus").ToString() == ""?false:true %>' runat="server" meta:resourcekey="ImageResource1" Height="15" />
                                                    </td>
                                                    <td style='white-space: nowrap; font-size: <%# Eval("FontSizeInnerData") %>px; font-weight: unset; /*padding: 2px; */ width: 15%;'>
                                                        <div class="ellipsistooltip"><%# Eval("ScheduleDate") %></div>
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
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="flipInterval" />
    </Triggers>
</asp:UpdatePanel>


