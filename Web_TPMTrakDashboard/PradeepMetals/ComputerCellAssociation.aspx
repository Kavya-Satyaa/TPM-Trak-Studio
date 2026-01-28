<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComputerCellAssociation.aspx.cs" Inherits="Web_TPMTrakDashboard.PradeepMetals.ComputerCellAssociation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        /* .tblHeader > tbody > tr > td:nth-child(odd) {
            color: white;
            text-align: left;
            vertical-align: middle;
            font-size: 15px;
            width: 10%;
            font-weight: bold;
        }

        .tblHeader > tbody > tr > td:nth-child(even) {
            width: 15%;
        }

        .tblHeader {
            border-color: #373c52;
            box-shadow: 2px 2px;
        }

            .tblHeader > tbody > tr > td {
                padding: 10px;
            }
           */
        .gridview-styling {
            margin-top: 10px;
            min-height: 100px;
            border-color: white;
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

        .header1 {
            background-color: #2E6886;
            color: white;
            text-align: center;
        }

        #dlComputerName {
            height: 25px;
            width: 80px;
        }

        .tblFilter > tbody > tr > td {
            color: white;
            padding: 10px;
            /*background:#8da0b1;*/
            font-weight: bold;
        }
    </style>
    <div>
        <fieldset class="searchCategory" style="border-color: #373c52; box-shadow: 2px 2px black; margin-left: 9px; width: 70%;">
            <table class="tblHeader tblFilter" style="width: 100%;">
                <tr>
                    <td>Device Name</td>
                    <td>
                        <asp:TextBox ID="txtComputerName" runat="server" Text="" CssClass="form-control" AutoCompleteType="Disabled" list="dlComputerName"></asp:TextBox>
                        <datalist runat="server" id="dlComputerName" clientidmode="static"></datalist>
                    </td>
                    <td>Plant ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlantID" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>Cell ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCellID" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td style="text-align: center;">
                        <asp:Button runat="server" CssClass="bajaj-btn-style" Text="View" ID="BtnView" OnClick="BtnView_Click" />
                    </td>
                    <td style="display: none"></td>
                    <td style="text-align: center;">
                        <asp:Button runat="server" CssClass="bajaj-btn-style" Text="Save" ID="BtnSave" OnClick="BtnSave_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>

    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:GridView runat="server" ID="gvCellInfo" AutoGenerateColumns="false" ClientIDMode="Static" CssClass="gridview-styling">
                <Columns>
                    <asp:TemplateField HeaderText="PlantID" AccessibleHeaderText="Plant ID" HeaderStyle-CssClass="header1">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hdnChecked" ClientIDMode="Static" />
                            <asp:Label runat="server" ID="lblPlantID" Text='<%# Eval("PlantID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CellID" AccessibleHeaderText="Cell ID" HeaderStyle-CssClass="header1">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCellID" Text='<%# Eval("GroupID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IsAssigned" AccessibleHeaderText="Is Assigned?" HeaderStyle-CssClass="header1">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkAssigned" Checked='<%# Eval("Checked") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
