<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineComponentMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.MachineComponentMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .header-center {
            text-align: center;
        }

        table tr th {
            text-align: center !important;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }
        /*.headerFixerhere tr:last-child td {
            background-color: #5391CA;
        }*/
        .DataOperations {
          /*  bottom: 0;
            right: 0;
            float: right;
            margin-right: 5px;
            min-height: 40px;
            position: fixed;*/
           margin-top:20px;
           text-align:right
        }

        .btnStyle {
            margin-left: 2px;
            margin-right: 2px;
        }
    </style>

    <div class="container-fluid" style="margin-left: -5px;">
        <asp:UpdatePanel ID="UpdatePanelMaintChklist" runat="server">
            <ContentTemplate>
                   <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row" style="height: 60px;">
                    <table id="tblFilter" class="table table-bordered" style="width:auto;margin-left:15px;">
                        <tr>

                            <td class="commanTd" style="width: 122px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","MachineId") %></td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" meta:resourcekey="ddlMachineIdResource1" />
                            </td>

                            <%--   <td class="commanTd" style="width: 80px; vertical-align: middle;">Component</td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlComponent" runat="server" CssClass="form-control" meta:resourcekey="ddlFreqResource1" AutoPostBack="true" />
                            </td>--%>

                            <td style="width: 130px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" Style="min-width: 120px;" OnClick="btnView_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="gridviewdiv" style="overflow-x: auto; overflow-y: auto; height: 76vh;width:60%">
                    <asp:GridView ID="gvMachineComponentData" ClientIDMode="Static" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"  ShowFooter="true">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available
                       
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfUpdate" runat="server" />
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblMachine" runat="server" Text='<%#Bind("MachineID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlMachine_Footer" CssClass="form-control" OnSelectedIndexChanged="ddlMachine_Footer_SelectedIndexChanged" AutoPostBack="true" style="width: 217px"></asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Component">
                                <ItemTemplate>
                                    <asp:Label ID="lblComponent" runat="server" Text='<%#Bind("Component") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlComponet_Footer" CssClass="form-control" style="width: 217px"></asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>


                        </Columns>
                         <FooterStyle BackColor="#5391CA" HorizontalAlign="Center" />
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
                <div class="DataOperations" style="width:60%">

                    <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-primary btnStyle" OnClick="btnNew_Click" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary btnStyle" OnClick="btnCancel_Click" />
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary btnStyle" OnClick="btnSave_Click" />
                    <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary btnStyle" OnClientClick="return DeleteConfirmation();" OnClick="btnDelete_Click" />

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete records?")) {
                return true;
            } else {
                return false;
            }
        }
         var bigDiv = document.getElementById('gridviewdiv');        bigDiv.onscroll = function () {            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);            console.log("id scroll =" + $('[id*=hdnScrollPos]').val());        }        window.onload = function () {            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();            console.log("id load =" + bigDiv.scrollTop);        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('gridviewdiv');
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();

            bigDiv.onscroll = function () {                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);                console.log("id scroll =" + $('[id*=hdnScrollPos]').val());            }            window.onload = function () {                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();                console.log("id load =" + bigDiv.scrollTop);            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
