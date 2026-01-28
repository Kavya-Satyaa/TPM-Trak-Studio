<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaintenanceGroupMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.PrecisionEngineering.MaintenanceGroupMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        
        fieldset.Fieldsetborder {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            color: white;
            width: 50%;
        }
         fieldset.Fieldsetborder{
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            height: 250px;
            color: white;
            width: 100%;
        }

        fieldset.Fieldsetinnerborder{
        border: 1px groove #ddd !important;
        padding: 0.1em 0.5em 1.1em !important;
        margin: 0 0 1.5em 0 !important;
        -webkit-box-shadow: 0px 0px 0px 0px #000;
        box-shadow: 0px 0px 0px 0px #000;
        height: 235px;
        color: white;
        width: 50%;
         }

        legend.Fieldsetinnerborder {
            font-size: 1.1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
            margin-top: -4px;
        }
    </style>
    <asp:UpdatePanel runat="server">
      <ContentTemplate>
       <div class="col-lg-5">
        <fieldset class="Fieldsetborder" style="height:auto">
        <legend class="commontd Fieldsetinnerborder">Group Definition</legend>
        <div id="divGroup">
            <asp:HiddenField ID="hdnFieldType" runat="server" />
            <asp:GridView runat="server" ID="gvGroupMaster" CssClass="table headerFixer" AutoGenerateColumns="false" ShowFooter="false" ShowHeaderWhenEmpty="true" OnRowDataBound="gvGroupMaster_RowDataBound" OnRowCommand="gvGroupMaster_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnUpdate" runat="server" ClientIDMode="Static" />
                            <asp:Button ID="btnSelect" runat="server" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" Text="Select" CssClass="bajaj-btn-style" style="border-radius:30px;border:1px solid lightblue;color:white;background-color:#0000007d" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group ID">
                        <ItemTemplate>
                            <asp:Label ID="lblGroupID" runat="server" CssClass="form-control" Text='<%# Bind("GroupID") %>' ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                         <FooterTemplate>
                             <asp:TextBox ID="txtfooterGroupID" runat="server" CssClass="form-control"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group Description">
                        <ItemTemplate>
                            <asp:TextBox ID="txtGroupDesc" runat="server" CssClass="form-control txtUpdate" Text='<%# Bind("GroupDesc") %>' ClientIDMode="Static"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                             <asp:TextBox ID="txtfooterGroupDesc" runat="server" CssClass="form-control"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                 <%--<SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />--%>
            </asp:GridView>
        </div>
        <div>
            <asp:Button ID="btnNew" runat="server" CssClass="bajaj-btn-style" Text="New" style="background-color:#77f100;border-radius:5px;" OnClick="btnNew_Click" />
            <asp:Button ID="btnCancel" runat="server" CssClass="bajaj-btn-style" Text="Cancel" style="background-color:#FF9191;border-radius:5px;" Visible="false" OnClick="btnCancel_Click"/>
            <asp:Button ID="BtnSave" runat="server" CssClass="bajaj-btn-style" Text="Save" style="background-color:deepskyblue;border-radius:5px;" OnClick="BtnSave_Click"/>
        </div>
        </fieldset>
       </div>
        
        <div class="col-lg-7" style="">
             <fieldset class="Fieldsetborder" id="Machineassign" style="width: 798px; height: auto;">
                <legend class="commontd Fieldsetinnerborder" style="margin-bottom:10px;">Machine Association</legend>
                <div class="md-12 lg-12" style="margin:5px;">
                    <div class="col-md-5">
                        <div>
                            <div style="background-color: #2e6886; color: white; text-align: center;border-radius:5px;border:1px solid lightblue;padding:3px;"><%=GetGlobalResourceObject("CommanResource","AssignedMachines") %></div>
                            <div style="background-color: white; height: 450px; overflow: auto; color: black;margin-top:5px;">
                                <asp:CheckBoxList ID="chkassigned" runat="server" ForeColor="Black"></asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div style="margin-top: 50px;">
                            <asp:Button ID="btnassign" runat="server" Style="background-color: black; margin-top: 20px; color: white" Text="<<" OnClick="btnassign_Click" />
                            <asp:Button ID="btnunassign" Style="background-color: black; margin-top: 20px; color: white" runat="server" Text=">>" OnClick="btnunassign_Click" />
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div style="background-color: #2e6886; color: white; text-align: center;border-radius:5px;border:1px solid lightblue;padding:3px;"><%=GetGlobalResourceObject("CommanResource","AvailableMachines") %></div>
                        <div style="background-color: white; color: black; height:450px; overflow: auto;margin-top:5px;">
                            <asp:CheckBoxList ID="chkaveliable" runat="server" ForeColor="Black"></asp:CheckBoxList>
                        </div>
                    </div>
                </div>
              </fieldset>
         </div>
     </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".txtUpdate").focus(function () {
                $(this).closest('tr').find("#hdnUpdate").val("update");
            });
        });
        function openSuccessModal(msg) {
            $('#toast-container').empty();
            Command: toastr["success"](msg)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        function showWarningMsg(msg, title) {
            debugger;
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "4000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "toastClass": "toaster-position"
            }
            toastr['warning'](msg, title);
            return false;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(".txtUpdate").focus(function () {
                $(this).closest('tr').find("#hdnUpdate").val("update");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
