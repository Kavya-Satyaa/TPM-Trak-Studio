<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PoojaParameterMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.Pooja.PoojaParameterMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <style>
        #gvParameterList > tbody > tr > th{
            text-align: center;
        }
        #gvParameterList > tbody > tr > td:nth-child(3){
            width: 9%;
        }
         #gvParameterList > tbody > tr > td:nth-child(4){
            width: 15%;
            text-align: center;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table class="table table-bordered" id="tblfilter" style="width: fit-content;">
                    <tr>
                        <td>
                            <input value="Cancel" type="button" class="btn btn-info" id="btnCancel" style="display: inline; float: right; margin-right: 3px;" runat="server" onserverclick="btnCancel_ServerClick" />
                            <input value="New"  type="button" class="btn btn-info" id="btnNew" style="display: inline; float: right; margin-right: 3px;" runat="server" onserverclick="btnNew_ServerClick" />
                        </td>
                        <td>
                            <asp:Button ID="btnSaveParamSetting" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveParamSetting_Click" />
                        </td>
                    </tr>
                </table>
                <div style="height: 80vh; overflow: auto;" runat="server">
                    <asp:GridView runat="server" ID="gvParameterList" ClientIDMode="Static" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer  alternate-table-style" ShowHeaderWhenEmpty="true" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Parameter ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblParameterID"  ClientIDMode="Static" Text='<%# Eval("ParameterID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtParameterID" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parameter Desc">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtParamDesc" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("ParameterDesc") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtParameterDesc" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sort Order">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtSortOrder" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("SortOrder") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtSort" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Is Andon Enabled">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkIsEnabled" Checked='<%# Eval("IsEnabled") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox runat="server" ID="chkIsEnable" Checked="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lbDelete" CssClass="action-icons delete-icons" ToolTip="Delete" OnClick="lbDelete_Click">
                                        <i class="glyphicon glyphicon-trash "></i>
                                        <span>DELETE</span> 
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" ID="lbAdd" CssClass="action-icons delete-icons" ToolTip="Add" OnClick="lbAdd_Click">
                                        <i class="glyphicon glyphicon-plus "></i>
                                        <span>ADD</span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#006699" Font-Bold="true" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
