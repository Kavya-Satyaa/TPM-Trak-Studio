<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineNodeInfo.aspx.cs" Inherits="Web_TPMTrakDashboard.EnergyModule.MachineNodeInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .button {
            color: #fff !important;
            text-transform: uppercase;
            background: aliceblue;
            padding: 05px;
            border-radius: 60px;
            display: inline-block;
            border: #ffd800;
        }

        /*   .button2 {
            background: #202648;
            border-radius: 60px;
            border-color: white;
            color: #fff;
            height: 30px;
        }*/

        .button3 {
            background: #202648;
            border-radius: 60px;
            border-color: white;
            color: #fff;
            height: 30px;
        }

        footerheight {
            height: 38px;
        }

        .removefootercss {
            display: none;
        }

        .FaltuStyle1 {
            font-size: 16px;
        }

        .FaltuStyle2 {
            margin-top: 3px;
            font-size: 16px;
        }

        .grid {
            width: 100%;
        }

            .grid tr th {
                position: sticky;
                top: -1px;
                z-index: 1;
                background-color: #2e6886;
                color: white;
                min-width: 150px;
                width: 50%;
                text-align: center;
                background-color: #2E6886 !important;
                height: 50px;
                font-size: 18px;
            }

            .grid tr td {
                height: 30px;
                width: 50%;
                text-align: center;
                background-color: white;
                padding: 3px;
                font-weight: bold;
            }

        fieldset {
            padding: 0px;
            border-radius: 4px;
            width: auto;
            border: 1px solid #ece3e3;
        }

        .masterFS {
            padding: 0 5px 10px 5px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            border-radius: 7px;
        }
    </style>

    <asp:UpdatePanel ID="updatepanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>

            <div style="display: flex">
                <div style="margin: 15px">
                    <fieldset class="masterFS">
                        <table class="filterTable" style="height: auto; width: auto">
                            <tr>
                                <td runat="server" id="tdPlant" class="FaltuStyle1" style="color: white; padding: 10px">Plant</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td id="tdCell" runat="server" style="color: white; padding: 10px" class="FaltuStyle1">Cell</td>
                                <td runat="server" id="tdCell1">
                                    <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td style="padding: 10px">
                                    <asp:Label runat="server" ForeColor="White" Text="Machine ID" CssClass="FaltuStyle1" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlmachineID" CssClass="FaltuStyle2  form-control" />
                                </td>
                                <td style="padding: 10px">
                                    <asp:Button runat="server" ID="btnview" OnClick="btnview_Click" Text="<%$Resources:CommanResource, View %>" CssClass="button2  btn btn-primary" Width="80" Height="30" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div style="margin: 15px">
                    <fieldset class="masterFS">
                        <table class="filterTable" style="height: auto; width: auto">
                            <tr>
                                <td style="padding: 10px">
                                    <asp:Label runat="server" ForeColor="White" Text="PLC IP Address" CssClass="FaltuStyle1" />
                                </td>
                                <td style="padding: 10px">
                                    <asp:DropDownList runat="server" ID="ddlIPAddress" CssClass="FaltuStyle2  form-control" />
                                </td>
                                <td style="padding: 10px;">
                                    <asp:Button runat="server" ID="btnIPAddressView" OnClick="btnIPAddressView_Click" Text="<%$Resources:CommanResource, View %>" CssClass="button2  btn btn-primary" Width="80" Height="30" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </div>

            <div style="margin-bottom: 5px; margin-top: 5px; margin-left: 5px; text-align: center">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center" Font-Size="X-Large"></asp:Label>
            </div>
            <div id="griddataview" style="height: 620px; overflow: auto;max-width:98%;overflow-x:auto">
                <div id="griddata" style="margin: 15px;">
                    <asp:HiddenField ID="hdfgrdcustomer" runat="server" />
                    <asp:GridView runat="server" OnRowDeleting="gridviewmachnode_RowDeleting" ID="gridviewmachnode" AutoGenerateColumns="False" CssClass="table table-bordered grid " meta:resourcekey="grdViewAndonViewResource1" HeaderStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="True" EnableViewState="true" ViewStateMode="Enabled" OnRowDataBound="GridView1_RowDataBound">
                        <%--    HeaderStyle-HorizontalAlign="Center" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" --%>
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Select" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource8">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Select" ClientIDMode="Static"></asp:Label></br>
                                    <asp:CheckBox ID="chkSelectAll" Checked="false" Width="60" runat="server" AutoCompleteType="Disabled" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" Checked="false" Width="60" runat="server" meta:resourcekey="chkSelectResource1" AutoCompleteType="Disabled"></asp:CheckBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%--<asp:CheckBox ID="chkSelectFooter" Checked="false" Width="60" runat="server" AutoCompleteType="Disabled"></asp:CheckBox>--%>
                                </FooterTemplate>
                                <%-- <HeaderStyle CssClass="text-center" Width="60" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Machine Id" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="labelmachineId" Text='<%# Eval("MachineId") %>' ItemStyle-HorizontalAlign="Center" CssClass="form-control" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                                    <asp:DropDownList ID="ddlMachineID" CssClass="form-control" runat="server" Width="133" />
                                </ItemTemplate>
                                 <FooterTemplate>
                                      <asp:DropDownList ID="ddlMachineID" CssClass="form-control" runat="server" Width="133" />
                                </FooterTemplate>
                                <%--  <HeaderStyle Width="100" CssClass="text-center" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SubSystem" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblSubSystem" CssClass="form-control" Text='<%# Bind("SubSystem") %>' Width="110" runat="server" meta:resourcekey="lblSubSystemTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </ItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox ID="lblSubSystem" CssClass="form-control" Width="110" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </FooterTemplate>
                                <%--  <HeaderStyle CssClass="text-center" Width="110" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Node ID" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblnodeOrder" min="1" CssClass="form-control" max="30" TextMode="Number" Text='<%# Eval("NodeID") %>' Width="90" runat="server" meta:resourcekey="lblNodeIDTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </ItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox ID="lblnodeOrder" min="1" CssClass="form-control" max="30" TextMode="Number" Width="90" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </FooterTemplate>
                                <%--   <HeaderStyle CssClass="text-center" Width="90" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EM Node Id" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblEMNodeInterfaceid" CssClass="form-control" min="1" Text='<%# Bind("EM_NodeId") %>' Width="140" runat="server" meta:resourcekey="lblNodeInterfaceTextResource2" AutoCompleteType="Disabled" TextMode="Number"></asp:TextBox>
                                </ItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox ID="lblEMNodeInterfaceid" CssClass="form-control" min="1" Width="140" runat="server" AutoCompleteType="Disabled" TextMode="Number"></asp:TextBox>
                                </FooterTemplate>
                                <%--      <HeaderStyle CssClass="text-center" Width="140" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Node Interface Id" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblNodeInterface" CssClass="form-control" Text='<%# Bind("NodeInterface") %>' Width="140" runat="server" meta:resourcekey="lblNodeInterfaceTextResource1" AutoCompleteType="Disabled" type="number"></asp:TextBox>
                                </ItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox ID="lblNodeInterface" CssClass="form-control" Width="140" runat="server" AutoCompleteType="Disabled" type="number"></asp:TextBox>
                                </FooterTemplate>
                                <%--   <HeaderStyle CssClass="text-center" Width="140" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Em Model No." HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmModelNumber" Text='<%# Eval("EM_ModelNumber") %>' Width="180" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                                    <asp:DropDownList ID="ddlEmModelNumber" CssClass="form-control" runat="server" Width="180">
                                        <asp:ListItem Text="EM6400/EM6436" Value="1" />
                                        <asp:ListItem Text="EM6400NG/EM6436H" Value="2" />
                                        <asp:ListItem Text="WL4400/WL4040" Value="3" />
                                        <asp:ListItem Text="EM1200" Value="4" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                                 <FooterTemplate>
                                     <asp:DropDownList ID="ddlEmModelNumber" CssClass="form-control" runat="server" Width="180">
                                        <asp:ListItem Text="EM6400/EM6436" Value="1" />
                                        <asp:ListItem Text="EM6400NG/EM6436H" Value="2" />
                                        <asp:ListItem Text="WL4400/WL4040" Value="3" />
                                        <asp:ListItem Text="EM1200" Value="4" />
                                    </asp:DropDownList>
                                </FooterTemplate>
                                <%--   <HeaderStyle CssClass="text-center" Width="130" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PLC IP Address" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource5">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblPlcIpAddress" CssClass="form-control" Text='<%# Bind("PLC_IP_Address") %>' Width="130" runat="server" meta:resourcekey="lblPlcIpAddressTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </ItemTemplate>
                                 <FooterTemplate>
                                      <asp:TextBox ID="lblPlcIpAddress" CssClass="form-control" Width="130" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </FooterTemplate>
                                <%--  <HeaderStyle CssClass="text-center" Width="130" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource6">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkEnabled" Checked='<%# Bind("Enabled") %>' Width="80" runat="server" meta:resourcekey="chkEnabledResource1" AutoCompleteType="Disabled"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="80" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>--%>
                            <%--<asp:CheckBoxField  ItemStyle-HorizontalAlign="Center" HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource6" HeaderStyle-CssClass="text-center" HeaderStyle-Wrap="false" HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" DataField="Enabled" ReadOnly="false" />--%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Enabled" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource6">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkEnabledEM" Checked='<%#bool.Parse(Eval("Enabled").ToString())%>' Width="60" runat="server" meta:resourcekey="chkSelectResource2" AutoCompleteType="Disabled"></asp:CheckBox>
                                </ItemTemplate>
                                 <FooterTemplate>
                                     <asp:CheckBox ID="chkEnabledEM" Width="60" runat="server" AutoCompleteType="Disabled"></asp:CheckBox>
                                </FooterTemplate>
                                <%--  <HeaderStyle CssClass="text-center" Width="60" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="DB Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblsortOrder" CssClass="form-control" TextMode="Number" min="1" max="30" ToolTip="The assigned DB Number (Node Interface) will be set to this index" Text='<%# Eval("SortOrder") %>' runat="server" meta:resourcekey="lblsortorderTextResource1" AutoCompleteType="Disabled" Width="100"></asp:TextBox>
                                </ItemTemplate>
                                 <FooterTemplate>
                                      <asp:TextBox ID="lblsortOrder" CssClass="form-control" TextMode="Number" min="1" max="30" ToolTip="The assigned DB Number (Node Interface) will be set to this index" runat="server" AutoCompleteType="Disabled" Width="100"></asp:TextBox>
                                </FooterTemplate>
                                <%-- <HeaderStyle CssClass="text-center" Wrap="false" Width="100" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                            </asp:TemplateField>

                            <asp:ButtonField ButtonType="Button"
                                CommandName="Delete" Text="Delete" ControlStyle-CssClass="button2" HeaderStyle-Width="100" ControlStyle-Width="80" />
                        </Columns>
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <%--  <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" BackColor="#202648" Font-Bold="True" ForeColor="#CCCCFF"></HeaderStyle>--%>
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" ForeColor="#003399" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <SortedAscendingCellStyle BackColor="#EDF6F6" />
                        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                        <SortedDescendingCellStyle BackColor="#D6DFDF" />
                        <SortedDescendingHeaderStyle BackColor="#002876" />
                    </asp:GridView>
                </div>

            </div>
            <div style="margin-bottom: 5px; margin-top: 5px; margin-left: 5px; text-align: center">
                <asp:Button runat="server" ID="btnnew" OnClick="btnnew_Click" Text="New" CssClass="button2  btn btn-primary" Width="100" Height="30" />
                <asp:Button runat="server" ID="btnsave" OnClick="btnSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="button2  btn btn-primary" Width="100" Height="30" />
                <asp:Button runat="server" ID="btnSendToPlc" OnClick="btnSendToPlc_Click" Text="<%$Resources:CommanResource, SendToPlc %>" CssClass="button2  btn btn-primary" Width="120" Height="30" />
                <asp:Button runat="server" ID="btnGetPlcConfig" OnClick="btnGetPlcConfig_Click" Text="Get PLC Config" CssClass="button2  btn btn-primary" Width="150" Height="30" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <script type="text/javascript">
        $(document).ready(function () {
            var winHeight = $(window).height();
            if (winHeight < 650) {
                winHeight = (winHeight - 200);
            } else {
                winHeight = (winHeight - 270);

            }
            $("#griddataview").height(winHeight);
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
