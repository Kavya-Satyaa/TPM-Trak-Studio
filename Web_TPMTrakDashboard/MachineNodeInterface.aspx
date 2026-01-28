<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineNodeInterface.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineNodeInterface" %>

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

        .button2 {
            background: #202648;
            border-radius: 60px;
            border-color: white;
            color: #fff;
            height: 30px;
        }

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
    </style>

    <asp:UpdatePanel ID="updatepanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            
            <div style="display: flex">
                <div style="margin: 15px">
                    <table class="table table-bordered" style="height: auto; width: auto">
                        <tr>
                            <td>
                                <asp:Label runat="server" ForeColor="White" Text="Machine ID" CssClass="FaltuStyle1" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlmachineID" CssClass="FaltuStyle2" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnview" OnClick="btnview_Click" Text="<%$Resources:CommanResource, View %>" CssClass="button2" Width="80" Height="30" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin: 15px">
                <table class="table table-bordered" style="height: auto; width: auto">
                    <tr>
                        <td>
                            <asp:Label runat="server" ForeColor="White" Text="PLC IP Address" CssClass="FaltuStyle1" />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlIPAddress" CssClass="FaltuStyle2" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnIPAddressView" OnClick="btnIPAddressView_Click" Text="<%$Resources:CommanResource, View %>" CssClass="button2" Width="80" Height="30" />
                        </td>
                    </tr>
                </table>
                </div>
            </div>

            <div style="margin-bottom: 5px; margin-top: 5px; margin-left: 5px; text-align: center">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center" Font-Size="X-Large"></asp:Label>
            </div>
            <div id="griddataview" style="height: 620px; overflow: auto"  >
                <div id="griddata" style="overflow-x: auto; overflow-y: auto; margin: 15px; max-width: 98%;" >
                    <asp:HiddenField ID="hdfgrdcustomer" runat="server" />
                    <asp:GridView runat="server" OnRowDeleting="gridviewmachnode_RowDeleting" ID="gridviewmachnode" AutoGenerateColumns="False" CssClass="table table-bordered " meta:resourcekey="grdViewAndonViewResource1" HeaderStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="True" HeaderStyle-HorizontalAlign="Center" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableViewState="true" ViewStateMode="Enabled" OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Select" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource8">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" Checked="false" Width="60" runat="server" meta:resourcekey="chkSelectResource1" AutoCompleteType="Disabled"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="60" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Machine Id" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="labelmachineId" Text='<%# Eval("MachineId") %>' ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                                    <asp:DropDownList ID="ddlMachineID" runat="server" Width="100" />
                                </ItemTemplate>
                                <HeaderStyle Width="100" CssClass="text-center" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SubSystem" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblSubSystem" Text='<%# Bind("SubSystem") %>' Width="110" runat="server" meta:resourcekey="lblSubSystemTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="110" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Node ID" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblnodeOrder" Text='<%# Eval("NodeID") %>' Width="90" runat="server" meta:resourcekey="lblNodeIDTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="90" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EM Node Id" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblEMNodeInterfaceid" Text='<%# Bind("EM_NodeId") %>' Width="140" runat="server" meta:resourcekey="lblNodeInterfaceTextResource2" AutoCompleteType="Disabled" type="number"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="140" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Node Interface Id" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblNodeInterface" Text='<%# Bind("NodeInterface") %>' Width="140" runat="server" meta:resourcekey="lblNodeInterfaceTextResource1" AutoCompleteType="Disabled" type="number"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="140" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Em Model No." HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmModelNumber" Text='<%# Eval("EM_ModelNumber") %>' Width="180" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                                    <asp:DropDownList ID="ddlEmModelNumber" runat="server" Width="180">
                                        <asp:ListItem Text="EM6436/EM6400" Value="1" />
                                        <asp:ListItem Text="EM6436H/EM6400NG" Value="2" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="130" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PLC IP Address" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource5">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblPlcIpAddress" Text='<%# Bind("PLC_IP_Address") %>' Width="130" runat="server" meta:resourcekey="lblPlcIpAddressTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Width="130" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                <HeaderStyle CssClass="text-center" Width="60" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PLC Address Index" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:TextBox ID="lblsortOrder" ToolTip="The assigned DB Number (Node Interface) will be set to this index" Text='<%# Eval("SortOrder") %>' runat="server" meta:resourcekey="lblsortorderTextResource1" AutoCompleteType="Disabled" Width="100"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" Wrap="false" Width="100" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:ButtonField ButtonType="Button"
                                CommandName="Delete" Text="Delete" ControlStyle-CssClass="button2" HeaderStyle-Width="100" ControlStyle-Width="80" />
                        </Columns>
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" BackColor="#202648" Font-Bold="True" ForeColor="#CCCCFF"></HeaderStyle>
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
                    <asp:Button runat="server" ID="btnnew" OnClick="btnnew_Click" Text="New" CssClass="button2" Width="100" Height="30" />
                    <asp:Button runat="server" ID="btnsave" OnClick="btnSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="button2" Width="100" Height="30" />
                    <asp:Button runat="server" ID="btnSendToPlc" OnClick="btnSendToPlc_Click" Text="<%$Resources:CommanResource, SendToPlc %>" CssClass="button2" Width="120" Height="30" />
                    <asp:Button runat="server" ID="btnGetPlcConfig" OnClick="btnGetPlcConfig_Click" Text="Get PLC Config" CssClass="button2" Width="150" Height="30" />
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
