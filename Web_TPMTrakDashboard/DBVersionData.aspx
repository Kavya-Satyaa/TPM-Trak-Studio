<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DBVersionData.aspx.cs" Inherits="Web_TPMTrakDashboard.DBVersionData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #gvDBVersion tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #gvDBVersion tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        #gvDBVersion tbody tr:last-child table {
            margin: auto;
            font-size: 20px;
        }

        fieldset {
            border: 1px solid white !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            margin: 0px;
            vertical-align: top;
        }

        legend {
            font-size: 1.1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            color: white;
            border-bottom: none;
            margin-top: -4px;
            margin: 0px;
        }

        .regVTbl tr td {
            color: white;
        }
    </style>

    <fieldset class="masterFS" style="margin-left: 5px !important; margin-right: 4px !important; display: inline-block">
        <legend>Required Version</legend>
        <table class="regVTbl">
            <tr>
                <td><b>Web Version:</b></td>
                <td>
                    <asp:Label runat="server" ID="lblPackageVersion" ClientIDMode="Static"></asp:Label>
                </td>
                <td><b>&nbsp;&nbsp;&nbsp;DB Version:</b></td>
                <td>
                    <asp:Label runat="server" ID="lblDBVersion" ClientIDMode="Static"></asp:Label>
                </td>
                <td><b>&nbsp;&nbsp;&nbsp;Script Name:</b></td>
                <td>
                    <asp:Label runat="server" ID="lblScriptName" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
    <div style="height: 80vh; overflow: auto;width:900px">
        <asp:GridView runat="server" ID="gvDBVersion" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" Width="100%" CssClass="table table-bordered table-hover headerFixer" ClientIDMode="Static" BorderStyle="NotSet" GridLines="None" AllowPaging="true" OnPageIndexChanging="gvDBVersion_PageIndexChanging" OnPreRender="gvDBVersion_PreRender" PageSize="100">
            <Columns>
                <asp:TemplateField HeaderText="Script Name">
                    <ItemTemplate>
                        <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("ScriptName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Script Date">
                    <ItemTemplate>
                        <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("ScriptDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DB VersionNumber">
                    <ItemTemplate>
                        <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("DbVersionNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Software Version Number" Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("SoftwareVersionNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
