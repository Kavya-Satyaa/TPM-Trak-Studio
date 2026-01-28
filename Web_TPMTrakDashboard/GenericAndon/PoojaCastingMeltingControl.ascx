<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PoojaCastingMeltingControl.ascx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.PoojaCastingMeltingControl" %>
<style>
    .status-icon {
        border: 1px solid #080843;
        border-radius: 50px;
        width: 30px;
        height: 30px;
    }

    .headerFixer tr th {
        position: sticky;
        text-align: center;
    }

    .headerFixer tr td {
        position: sticky;
        text-align: center;
    }

    table.bordertable {
        border: 1px black;
        margin-bottom: 10px;
        border-radius: 10px;
    }

    .bordertable tr th {
        border: 0.5px solid black;
        padding: 5px;
        /* border-radius: 10px;*/
    }

    .bordertable tr td {
        border: 0.5px solid black;
        padding: 5px;
    }

    .bordertable {
        width: 100%;
        border-spacing: 0;
        border-collapse: separate;
        text-align: center;
    }

        .bordertable tr:last-child td:last-child {
            border-bottom-right-radius: 10px;
        }

        .bordertable tr:last-child td:first-child {
            border-bottom-left-radius: 10px;
        }

        .bordertable tr:first-child th:first-child,
        .bordertable tr:first-child td:first-child {
            border-top-left-radius: 10px
        }

        .bordertable tr:first-child th:last-child,
        .bordertable tr:first-child td:last-child {
            border-top-right-radius: 10px
        }

    /*     .bordertable tr th:first-child,
        .bordertable tr td:first-child {
            border-left: 2px solid #B2DBF4;
        }

        .bordertable tr:first-child th,
        .bordertable tr:first-child td {
            border-top: 2px solid #B2DBF4;
        }
*/
    fieldset {
        border: 2px solid black;
        padding-left: 10px;
        padding-bottom: 5px;
        border-radius: 15px;
        width: auto;
        margin-bottom: 20px;
    }

    legend {
        color: black;
        width: auto;
        border-bottom: 0px;
        margin: 0px;
        border-radius: 10px;
    }

    .lblFurnaceStatusStyle {
        padding: 5px 15px;
        border-radius: 50%;
    }

    .furnace-param-table tr td:first-child, .furnace-param-table tr th:first-child {
        display: none;
    }

    .StatusLabel {
        padding: 15px 15px;
        border-radius: 50%;
        /*display:<%# Eval("Display1") %> ;*/
        /*border:2px solid black;*/
    }
</style>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:Timer runat="server" ID="flipInterval1" ClientIDMode="Static" OnTick="flipInterval1_Tick"></asp:Timer>
        <div style="text-align: center" class="cellLabelDiv">
            <asp:Label runat="server" ID="lblCellName" CssClass="cellLabel"></asp:Label>
        </div>
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnviewType" />
        <div id="PoojaCastingMeltingTable" style="margin-top: 30px;" runat="server">
            <asp:ListView runat="server" ClientIDMode="Static" ID="lvMachineDetails" ItemPlaceholderID="placeHolder">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="placeHolder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <fieldset class="myItem">
                        <legend>
                            <table style="border: 1px  black; margin-bottom: 10px; text-align: center;" class="bordertable">
                                <tr style="background-color: rgb(226 212 212 / 0.70); text-align: center;">
                                    <th>Furnace/MH</th>
                                    <th>Heat Code</th>
                                    <th>Heat No</th>
                                    <th>Grade ID</th>
                                    <th>Grade Norm</th>
                                    <th>Date Time</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hdnMachineID" Value='<%# Eval("Furnace") %>' />
                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblFurnace" Text='<%# Eval("Furnace") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblHeatCode" Text='<%# Eval("HeatCode") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblHeatNo" Text='<%# Eval("HeatNo") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblGradeID" Text='<%# Eval("GradeID") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblGradeNorm" Text='<%# Eval("GradeNorm") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblDate" Text='<%# Eval("Date") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <asp:GridView runat="server" ID="gvParameter" AutoGenerateColumns="true" ClientIDMode="Static" CssClass="table table-bordered headerFixer  alternate-table-style furnace-param-table " ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#0066cc" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White" HeaderStyle-Font-Size="20px" RowStyle-Font-Size="17px" RowStyle-Font-Bold="true" OnRowDataBound="gvParameter_RowDataBound" DataSource='<%# Eval("dtParameters") %>'>
                            <%-- <Columns>
                                <asp:TemplateField HeaderText="Status" Visible="false"> 
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdnMachineID" Value='<%# Eval("MachineID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>--%>
                        </asp:GridView>
                    </fieldset>
                </ItemTemplate>
            </asp:ListView>
        </div>

        <div id="PoojaCastingMeltingCockpit" runat="server" style="margin-top: 20px; display: inline-block; width: 100%; text-align: center;">
            <asp:ListView runat="server" ClientIDMode="Static" ItemPlaceholderID="placeHolder" ID="lvMeltingData">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="placeHolder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <div style="min-width: 300px; display: inline-block; padding: 10px; box-shadow: 2px 2px 4px rgba(128, 128, 128, 0.7); margin-left: 15px; background-color: #59e1c5; border-radius: 10px; border-color: black;  align-content: center;text-align:center;" class="myItem">
                        <table style='width: 100%;' class="outercockpit">
                            <tr>
                                <td style="text-align: center; color: black; font-weight: bold; font-size: 20px;">
                                    <label><%# Eval("Furnace") %></label>
                                </td>
                            </tr>
                        </table>
                        <table class="table table-bordered " style='margin-bottom: 4px; background-color: white; font-size: 15px; font-weight: bold;'>
                            <tr>
                                <td style="padding: 0px">
                                    <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("ParameterList") %>' ItemPlaceholderID="addressPlaceHolder">
                                        <LayoutTemplate>
                                            <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: left;"><%# Eval("ParameterText") %></td>
                                                <td style="text-align: left; background-color: <%# Eval("backColor") %>; display: <%# Eval("Display2") %>"><%# Eval("ParameterValue") %>  
                                                </td>
                                                <td style="display: <%# Eval("Display1") %>">
                                                    <label class="StatusLabel" style='background-color: <%# Eval("backColor") %>'></label>
                                                    <%--<asp:Label CssClass="StatusLabel" runat="server" style='background-color:<%# Eval("backColor") %> '></asp:Label>--%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="flipInterval1" />
    </Triggers>
</asp:UpdatePanel>
