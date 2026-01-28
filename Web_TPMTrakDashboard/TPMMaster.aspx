<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TPMMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        #tblsearch tr td
        {
            width:1%;
            white-space:nowrap;
            text-align:center;
        }
    </style>
     <div class="row" style="margin:10px;">
        <asp:HiddenField runat="server" ID="hdnCompList" ClientIDMode="Static" />
        <table class="table table-bordered" id="tblsearch" style="display: inline-block; width: 70%">
            <tr>
                <td>
                    <div class="commontd" style="margin-top: 5px;">Plant ID</div>
                </td>
                <td>
                    <div style="min-width: 116px;">
                         <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                          </asp:DropDownList>                    
                    </div>
                </td>

                <td>
                    <div class="commontd" style="margin-top: 5px;">Cell ID</div>
                </td>
                <td>
                    <div>
                        <asp:ListBox ID="ddlMultiCellID" runat="server" SelectionMode="Multiple" CssClass="form-control" OnSelectedIndexChanged="ddlMultiCellID_SelectedIndexChanged"  AutoPostBack="true"></asp:ListBox>
                    </div>
                </td>
                <td>
                   <div class="commontd" style="margin-top: 5px;">Machine ID</div>
                </td>
                <td>
                    <div>
                          <asp:ListBox ID="ddlMultiDownID" runat="server" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                    </div>
                </td>
                <td>
                    <asp:Button runat="server" Text="<%$ Resources:CommanResource, Export %>" class="btn btn-info" ID="btnexport" OnClick="btnexport_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
    <script>
        $(document).ready(function () {
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=ddlMultiCellID]').multiselect({
                includeSelectAllOption: true
            });

        });


        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=ddlMultiCellID]').multiselect({
                includeSelectAllOption: true
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
