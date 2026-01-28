<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="PMCategoryMasterLnT.aspx.cs" Inherits="Web_TPMTrakDashboard.LnTOdisha.PMCategoryMasterLnT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .grid div{
            width: 100%;
        }
         #gvPMcategoryMaster > tbody > tr > th{
             height: 40px;
             text-align: center;
         }
        #gvPMcategoryMaster > tbody > tr > td{
           padding: 10px;
        }
        #gvPMcategoryMaster{
            width: 100%;
        }
        #gvPMcategoryMaster tbody tr:last-child td{
            bottom:0;
            position: sticky;
            background-color: white;
        }
         .bajaj-table-style tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        .bajaj-table-style tbody tr:nth-child(even) {
            background-color: #FFFFFF;
        }
          .headerFixer tr th {
            position: sticky;
            top: -1px;
            z-index: 4;
            background-color: #2e6886;
            color: white;
        }
          .bajaj-table-style  tbody tr td{
              color: black;
          }
    </style>

    <div class="grid" style="height: 70vh; overflow: auto; display: flex; justify-content: center; align-content: center; vertical-align: middle;">
        <asp:GridView runat="server" ID="gvPMcategoryMaster" ShowHeaderWhenEmpty="true" CssClass="headerFixer bajaj-table-style" ClientIDMode="Static" AutoGenerateColumns="false" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="Category">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPMCategory" Text='<%# Eval("PMCategoryName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox runat="server" ID="txtPMCategory" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sort Order">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSortOrder" Text='<%# Eval("SortOrder") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox runat="server" ID="txtSortOrder" AutoCompleteType="Disabled" CssClass="form-control allowNumber"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" CssClass="btn btn-primary" OnClick="lnkDelete_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button runat="server" ID="btnAdd" Text="Add" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <script>
        $("#btnAdd").click(function () {
            debugger;
            return true;
        });
        $('.allowNumber').keypress(function (evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#btnAdd").click(function () {
                debugger;
                return true;
            });
            $('.allowNumber').keypress(function (evt) {
                debugger;
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode < 48 || charCode > 57) {
                    return false;
                }
                return true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
