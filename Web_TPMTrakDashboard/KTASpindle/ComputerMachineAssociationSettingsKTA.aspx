<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComputerMachineAssociationSettingsKTA.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.ComputerMachineAssociationSettingsKTA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <style>
        .Btn {
            width: 95px;
        }

        .gridview-styling {
            margin-top: 10px;
            min-height: 100px;
            border-color: white;
        }

        .gridview-lbl-style {
            height: max-content;
            width: max-content;
        }

        .header1 {
            background-color: #2E6886;
            color: white;
            text-align: center;
        }

        .gridview-styling > tbody > tr > td {
            height: 50px;
            padding-left: 5px;
            width: 20vw;
        }

        .gridview-styling > tbody > tr:nth-child(odd) > td {
            background-color: #DCDCDC;
        }

        .gridview-styling > tbody > tr:nth-child(even) > td {
            background-color: white;
        }

        .gridview-styling > tbody > tr > th {
            height: 50px;
        }

        .gridview-styling > tbody > tr > td:last-child {
            width: 5vw;
            text-align: center;
        }

        #dlComputerName {
            height: 25px;
            width: 80px;
        }
        #tblDisplay > tbody > tr > td {
            line-height: 30px;
        }

        #tblDisplay > tbody > tr:nth-child(odd) {
            background-color: white;
        }
        #tblDisplay > tbody > tr:nth-child(even) {
            background-color: #DCDCDC;
        }
        #tblDisplay > tbody > tr > th {
            line-height: 30px;
            background-color: #2E6886;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table runat="server" class="bajaj-filter-tbl">
                <tr>
                    <td><span>Computer Name</span></td>
                    <td>
                        <asp:TextBox ID="txtComputerName" runat="server" Text="" CssClass="form-control" AutoCompleteType="Disabled" list="dlComputerName"></asp:TextBox>
                        <datalist runat="server" id="dlComputerName" clientidmode="static"></datalist>
                    </td>
                    <td><span>Plant ID</span></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlantID" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td><span>Cell ID</span></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCellID" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td><span>Run Options</span></td>
                    <td>
                        <asp:HiddenField runat="server" ID="hdnVisible" />
                        <asp:DropDownList ID="ddlRunOptions" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlRunOptions_SelectedIndexChanged">
                            <asp:ListItem Text="Run By Cell" Value="RunByCell"></asp:ListItem>
                            <asp:ListItem Text="Run By Machine" Value="RunByMachine"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="BtnView" CssClass="bajaj-btn-style Btn" OnClick="BtnView_Click" Text="View" ForeColor="White" Font-Bold="true" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="BtnSave" CssClass="bajaj-btn-style Btn" OnClick="BtnSave_Click" Text="Save" ForeColor="White" Font-Bold="true" />
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-7">
                            <asp:GridView runat="server" ID="gvMachineDetails" AutoGenerateColumns="false" ClientIDMode="Static" CssClass="gridview-styling">
                                <Columns>
                                    <asp:TemplateField HeaderText="Plant ID" HeaderStyle-CssClass="header1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPlantID" Text='<%# Eval("PlantID")  %>' CssClass="gridview-lbl-style"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cell ID" HeaderStyle-CssClass="header1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCellID" Text='<%# Eval("GroupID") %>' CssClass="gridview-lbl-style"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Machine ID" HeaderStyle-CssClass="header1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>' CssClass="gridview-lbl-style"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Assigned" HeaderStyle-CssClass="header1">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkAssigned" Checked='<%# Eval("Checked") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView runat="server" ID="gvMachineDetailsCellWise" AutoGenerateColumns="false" ClientIDMode="Static" CssClass="gridview-styling">
                                <Columns>
                                    <asp:TemplateField HeaderText="Plant ID" HeaderStyle-CssClass="header1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPlantID" Text='<%# Eval("PlantID")  %>' CssClass="gridview-lbl-style"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cell ID" HeaderStyle-CssClass="header1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCellID" Text='<%# Eval("GroupID") %>' CssClass="gridview-lbl-style"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Assigned" HeaderStyle-CssClass="header1">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkAssigned" Checked='<%# Eval("Checked") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="displayOptions" class="col-lg-3">
                            <asp:ListView runat="server" ID="lvDisplayOptions" ItemPlaceholderID="itemplaceholder">
                                <LayoutTemplate>
                                    <table id="tblDisplay" class=" table table-bordered" style="margin-top: 10px;">
                                        <th style="color: white; text-align: center;">Display Screen</th>
                                        <th style="color: white; text-align: center;">Assigned</th>
                                        <tr>
                                            <asp:PlaceHolder runat="server" ID="itemplaceholder"></asp:PlaceHolder>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblOption" Text='<%# Eval("DisplayScreen") %>'></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:CheckBox runat="server" ID="chkAssigned" Checked='<%# Convert.ToBoolean(Eval("Checked")) %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlPlantID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlRunOptions" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="BtnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
