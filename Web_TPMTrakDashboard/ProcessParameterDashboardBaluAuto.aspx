<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterDashboardBaluAuto.aspx.cs" Inherits="Web_TPMTrakDashboard.ProcessParameterDashboardBaluAuto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/Ionic.css" rel="stylesheet" />
    <style>

        
        .headerFixera tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background: black;
            color: white;
            text-align: center;
            font-size: larger;
            overflow-wrap: break-word;
            height: 40px;
        }

        td {
            background-color: white;
            color: blue;
            text-align: center;
            height: 35px;
            font-size: larger;
            overflow-wrap: break-word;
        }

        .Running {
			-webkit-animation: cog-rotate 2s linear infinite;
			-moz-animation: cog-rotate 2s linear infinite;
			-o-animation: cog-rotate 2s linear infinite;
			animation: rotate 2s linear infinite;
			color: green;
		}

		.Stopped {
			color: red;
		}

    </style>
    <div style="width: 100%">
        <asp:GridView runat="server" ID="gridProcessparamwter" OnRowDataBound="gridProcessparamwter_RowDataBound" AutoGenerateColumns="false" Style="width: 100%" CssClass="headerFixera">
            <HeaderStyle />

            <Columns>
                <asp:BoundField HeaderText="Machine ID" DataField="MachineID" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <div>
                            <asp:Label runat="server" ID="status" Text='<%# Eval("McStatus") %>' style="vertical-align:-webkit-baseline-middle" />
                             <div runat="server" class="loaders-container" style="display: inline-block;float:right" visible='<%# Eval("Visibility").ToString() == "false"?false:true %>'>
						    <div class="la-cog la-2x" style="float: right;">
							    <div class="<%# Eval("OKNOT") %>"></div>
						    </div>
						</div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OEE">
                    <ItemTemplate>
                        <div style="background-color:<%# Eval("OEEColor") %>;height:100%;text-align:center;color:black">
                            <asp:Label runat="server" ID="OEE" Text='<%# Eval("OverallEfficiency") %>' style="text-align:center;vertical-align:-webkit-baseline-middle" />
                            
                        </div>
                        <%--<asp:Image runat="server" ID="image" ImageUrl='<%# Bind("Image") %>' />--%>
                       
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Part Count">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="Compoenets" Text='<%# Eval("Components") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtspToolLifeStatus" Text="Tool life status" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("ToolLifeStatus").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valspl" Text='<%# Eval("ToolLifeStatus") %>' ForeColor='<%# Eval("ToolLifeStatus").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>' /></br>
                        <asp:Label runat="server" ID="valdatetime1" Text='<%# Eval("ToolLifeTs") %>' ForeColor='<%# Eval("ToolLifeStatus").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp1" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P1Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valspl" Text='<%# Eval("P1Status") %>' ForeColor='<%# Eval("P1Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>' /></br>
                        <asp:Label runat="server" ID="valdatetime1" Text='<%# Eval("P1Ts") %>' ForeColor='<%# Eval("P1Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp2" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P2Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp2" Text='<%# Eval("P2Status") %>' ForeColor='<%# Eval("P2Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/></br>
                        <asp:Label runat="server" ID="valdatetime2" Text='<%# Eval("P2Ts") %>' ForeColor='<%# Eval("P2Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/>
                        </div>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp3" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P3Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp3" Text='<%# Eval("P3Status") %>' ForeColor='<%# Eval("P3Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/></br>
                        <asp:Label runat="server" ID="valdatetime3" Text='<%# Eval("P3Ts") %>' ForeColor='<%# Eval("P3Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp4" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P4Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp4" Text='<%# Eval("P4Status") %>' ForeColor='<%# Eval("P4Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>' /></br>
                        <asp:Label runat="server" ID="valdatetime4" Text='<%# Eval("P4Ts") %>' ForeColor='<%# Eval("P4Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp5" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P5Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp5" Text='<%# Eval("P5Status") %>' ForeColor='<%# Eval("P5Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/></br>
                        <asp:Label runat="server" ID="valdatetime5" Text='<%# Eval("P5Ts") %>' ForeColor='<%# Eval("P5Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp6" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P6Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp6" Text='<%# Eval("P6Status") %>' ForeColor='<%# Eval("P6Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  /></br>
                        <asp:Label runat="server" ID="valdatetime6" Text='<%# Eval("P6Ts") %>' ForeColor='<%# Eval("P6Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp7" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P7Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp7" Text='<%# Eval("P7Status") %>' ForeColor='<%# Eval("P7Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  /></br>
                        <asp:Label runat="server" ID="valdatetime7" Text='<%# Eval("P7Ts") %>' ForeColor='<%# Eval("P7Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp8" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P8Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp8" Text='<%# Eval("P8Status") %>' ForeColor='<%# Eval("P8Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  /></br>
                        <asp:Label runat="server" ID="valdatetime8" Text='<%# Eval("P8Ts") %>' ForeColor='<%# Eval("P8Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp9" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P9Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valsp9" Text='<%# Eval("P9Status") %>' ForeColor='<%# Eval("P9Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  /></br>
                        <asp:Label runat="server" ID="valdatetime9" Text='<%# Eval("P9Ts") %> ' ForeColor='<%# Eval("P9Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'  />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="txtsp10" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div  <%# Eval("P10Status").Equals("Not Ok")? "style='background-color:Red'" : "style='background-color:White'" %> ">
                            <asp:Label runat="server" ID="valspl0" Text='<%# Eval("P10Status") %>' ForeColor='<%# Eval("P10Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/></br>
                        <asp:Label runat="server" ID="valdatetime10" Text='<%# Eval("P10Ts") %>' ForeColor='<%# Eval("P10Status").Equals("Not Ok")? System.Drawing.Color.White : System.Drawing.Color.Black %>'/>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
    <asp:Timer runat="server" ID="timer1" Enabled="false" OnTick="timer1_Tick"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
