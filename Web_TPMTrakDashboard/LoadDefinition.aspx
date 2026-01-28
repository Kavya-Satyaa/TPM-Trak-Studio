<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoadDefinition.aspx.cs" Inherits="Web_TPMTrakDashboard.LoadDefinition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.js"></script>
    <link href="Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.min.js"></script>
    <style>
        .multiselect-container {
            height: 300px;
            height: 300px;
            overflow-x: auto;
            width: 240px;
        }

        .multiselect-selected-text {
            padding-right: 181px;
            width: 240px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
            width: 240px;
        }

        th {
            text-align: center;
        }
        #gridview > tbody > tr:nth-child(odd) > td{
            background: #F2F2F2
        }
        #gridview > tbody > tr:nth-child(even) > td{
            background: #FFFFFF
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="ui segment">
                    <table>
                        <tr>
                            <td><b>Plant</b>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="cmbPlant" OnSelectedIndexChanged="cmbPlant_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" />
                            </td>
                            <td><b>Machine</b>
                            </td>
                            <td style="width: 240px;">
                                <%--<asp:ListBox ID="cmbMachineID" runat="server" SelectionMode="Multiple" CssClass="form-control multiselect" ToolTip="<%$Resources:CommanResource, ALL %>" Style="width: 100px;" OnSelectedIndexChanged="cmbMachineID_SelectedIndexChanged" AutoPostBack="true" Width="235"></asp:ListBox>--%>
                                <asp:DropDownList runat="server" ID="cmbMachineID" OnSelectedIndexChanged="cmbMachineID_SelectedIndexChanged"  CssClass="form-control" AutoPostBack="true" Style="width: 200px;" />
                            </td>
                            <td><b>Component</b>
                            </td>
                            <td style="width: 240px;">
                                <asp:ListBox ID="cmbComponentID" runat="server" SelectionMode="Multiple" CssClass="form-control multiselect" ToolTip="<%$Resources:CommanResource, ALL %>" Style="width: 100px;" OnSelectedIndexChanged="ddlComponentID_SelectedIndexChanged" AutoPostBack="true" Width="235"></asp:ListBox>
                            </td>
                            <td><b>Operation</b>
                            </td>
                            <td>
                                <asp:ListBox ID="cmbOperation" runat="server" SelectionMode="Multiple" CssClass="form-control multiselect" ToolTip="<%$Resources:CommanResource, ALL %>" Style="width: 100px;" AutoPostBack="true" Width="235"></asp:ListBox>
                                <%--<asp:DropDownList runat="server" ID="cmbOperation" CssClass=" form-control" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="commontd" style="color: black;"><b>From Date:</b></td>
                            <td class="input-group" style="padding-bottom: 10px; padding-top: 10px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Style="width: 160px; height: 42px" CssClass="form-control date" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commontd" style="color: black;"><b>To Date:</b></td>
                            <td class="input-group" style="padding-bottom: 10px; padding-top: 10px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" Style="width: 160px; height: 42px;" runat="server" CssClass="form-control date" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="View" CssClass="ui violet button" OnClick="btnView_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height:65vh;overflow:auto">  
                    <asp:GridView runat="server" ID="gridview" ClientIDMode="Static" CssClass="headerFixer" OnRowDataBound="gridview_RowDataBound" AutoGenerateColumns="false" >
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="60" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkselect" Style="text-align: center" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SLNO" HeaderStyle-Width="60" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblslno" Text='<%# Bind("SLNO") %>' />
                                    <asp:HiddenField runat="server" ID="idd" Value='<%# Bind("idd") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine ID" HeaderStyle-Width="100" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%# Bind("MachineID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Component ID" HeaderStyle-Width="150" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblComponentID" Text='<%# Bind("ComponentID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Opn" HeaderStyle-Width="60" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperation" Text='<%# Bind("Operation") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Std. Cycle Time (mins)" HeaderStyle-Width="100" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStdCycleTime" Text='<%# Bind("StdCycleTime") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PDT(min)" HeaderStyle-Width="100" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table style="width:200px">
                                        <tr>
                                            <td  colspan="3" style="text-align: center; color: white; background-color: #2E6886">
                                                <asp:Label runat="server" ID="lblPDT" Text="PDT(min)"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; text-align: center; color: white; background-color: #2E6886">
                                                <asp:Label runat="server" ID="lblShift1PDT" ForeColor="white"/>
                                            </td>
                                            <td style="width: 100px; text-align: center; color: white; background-color: #2E6886">
                                                <asp:Label runat="server" ID="lblShift2PDT" ForeColor="white" />
                                            </td>
                                            <td style="width: 100px; text-align: center; color: white; background-color: #2E6886">
                                                <asp:Label runat="server" ID="lblShift3PDT" ForeColor="white" />
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                           <%-- <td style="width: 100px; text-align: center; color: white; background-color: transparent">
                                                <asp:Label runat="server" ID="lblPDTshift1" Text='<%# Bind("PDTShift1") %>' ForeColor="White"/>
                                            </td>
                                            <td style="width: 100px; text-align: center; color: white; background-color: transparent">
                                                <asp:Label runat="server" ID="lblPDTshift2" Text='<%# Bind("PDTShift2") %>' ForeColor="White"/>
                                            </td>
                                            <td style="width: 100px; text-align: center; color: white; background-color: transparent">
                                                <asp:Label runat="server" ID="lblPDTShift3" Text='<%# Bind("PDTShift3") %>' ForeColor="White"/>
                                            </td>--%>
                                             <td style="width: 100px; text-align: center; color: black; background-color: transparent">
                                                <asp:Label runat="server" ID="lblShift1PDT" Text='<%# Bind("PDTShift1") %>' ForeColor="black"/>
                                            </td>
                                            <td style="width: 100px; text-align: center; color: black; background-color: transparent">
                                                <asp:Label runat="server" ID="lblShift2PDT" Text='<%# Bind("PDTShift2") %>' ForeColor="black"/>
                                            </td>
                                            <td style="width: 100px; text-align: center; color: black; background-color: transparent">
                                                <asp:Label runat="server" ID="lblShift3PDT" Text='<%# Bind("PDTShift3") %>' ForeColor="black"/>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" HeaderStyle-Width="120" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDate" Text='<%# Bind("Date") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <td colspan="3" style="text-align: center; color: white; background-color: #2E6886">Shift Target
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; text-align: center; color: white; background-color: #2E6886">
                                                <asp:Label runat="server" ID="lblhdrShift1" ForeColor="White"/>
                                            </td>
                                            <td style="width: 100px; text-align: center; color: white; background-color: #2E6886">
                                                <asp:Label runat="server" ID="lblhdrShift2" ForeColor="White"/>
                                            </td>
                                            <td style="width: 100px; text-align: center; color: white; background-color: #2E6886">
                                                <asp:Label runat="server" ID="lblhdrShift3" ForeColor="White"/>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 100px; text-align: center; color: black; background-color: #2E6886">
                                                <asp:TextBox runat="server" ID="txtShift1" Text='<%# Bind("Shift1Target") %>' CssClass="form-control" OnTextChanged="txtShift1_TextChanged" AutoPostBack="true" MaxLength="4" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                                                <asp:HiddenField runat="server" ID="Calculatetgt1" value='<%# Bind("Calculatetgt1") %>' />
                                            </td>
                                            <td style="width: 100px; text-align: center; color: black; background-color: #2E6886">
                                                <asp:TextBox runat="server" ID="txtShift2" Text='<%# Bind("Shift2Target") %>' CssClass="form-control" OnTextChanged="txtShift1_TextChanged" AutoPostBack="true" MaxLength="4" onkeypress="return event.charCode >= 48 && event.charCode <= 57"/>
                                                <asp:HiddenField runat="server" ID="Calculatetgt2" value='<%# Bind("Calculatetgt2") %>' />
                                            </td>
                                            <td style="width: 100px; text-align: center; color: black; background-color: #2E6886">
                                                <asp:TextBox runat="server" ID="txtShift3" Text='<%# Bind("Shift3Target") %>' CssClass="form-control" OnTextChanged="txtShift1_TextChanged" AutoPostBack="true" MaxLength="4" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                                                <asp:HiddenField runat="server" ID="Calculatetgt3" value='<%# Bind("Calculatetgt3") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Target" HeaderStyle-Width="100" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="white" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-ForeColor="black">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTotal" Text='<%#Bind("TotalTarget") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="ui segment" style="text-align: right;">
                    <asp:Button runat="server" ID="btnGenerateShiftTarget" OnClick="btnGenerateShiftTarget_Click" CssClass="ui violet button" Text="Calculate Target" />
                    <asp:Button runat="server" OnClick="btnSave_Click" ID="btnSave" CssClass="ui violet button" Text="Save" />
                    <asp:Button runat="server" OnClick="btnDelete_Click" ID="btnDelete" CssClass="ui violet button" Text="Delete" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            $(".multiselect").multiselect({
                includeSelectAllOption: true
            });
            $(".date").datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(".multiselect").multiselect({
                includeSelectAllOption: true
            });
            $(".date").datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
