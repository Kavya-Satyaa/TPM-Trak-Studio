<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateComponentStore.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.UpdateComponentStore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .headerFixer tbody tr {
            background-color: #FFFFFF;
            color: black;
        }

        /*.headerFixer tbody tr:nth-last-child(2) {
            background-color: #DCDCDC;
            color: black;
        }*/

    </style>
    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="col-lg-10">
                    <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                        <tr>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Cell</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCell" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                            </td>
                       
                        </tr>
                    </table>
                </div>
                <div class="col-lg-2" style="margin-top: 10px;margin-left: -22px;text-align:right">
                    <asp:Button runat="server" ID="btnSave" Text="Update Store" CssClass="btn btn-info" OnClick="btnSave_Click" />
                </div>
                  <div class="col-lg-12" style="height: 80vh; overflow: auto;" id="gridContainer">
                    <asp:GridView ID="gvNextCompOpnSelection" CssClass="table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static">
                        <Columns>
                            <asp:TemplateField HeaderText="Plant" HeaderStyle-Width="220">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                    <asp:Label ID="lblPlant" runat="server" Text='<%# Eval("PlantID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cell" HeaderStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblCell" runat="server" Text='<%# Eval("GroupID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine">
                                <ItemTemplate>
                                    <asp:Label ID="lblMachine" runat="server" Text='<%# Eval("Machineid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Component">
                                <ItemTemplate>
                                    <asp:Label ID="lblComponent" runat="server" Text='<%# Eval("ComponentID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Component Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentDesc" runat="server" Text='<%# Eval("ComponentDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:Label ID="lblOperation" runat="server" Text='<%# Eval("OperationNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Priority No">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPriority" runat="server" CssClass="form-control allowNumber entryVal" Text='<%# Eval("Priority") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Update Store">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkNxtPart" runat="server" CssClass="CheckVal entryVal" Checked='<%# Eval("NextComponent") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlPlant" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlCell" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
  <script>
      $(document).ready(function () {
          $(".entryVal").blur(function () {
              $(this).closest('tr').find("#hdnUpdate").val("update");
          });

          $(".entryVal #chkNxtPart").blur(function () {
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


      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
          $(document).ready(function () {
              $(".entryVal").blur(function () {
                  $(this).closest('tr').find("#hdnUpdate").val("update");
              });

              $(".entryVal #chkNxtPart").blur(function () {
                  $(this).closest('tr').find("#hdnUpdate").val("update");
              });
          });
      });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
