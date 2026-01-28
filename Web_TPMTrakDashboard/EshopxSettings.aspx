<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EshopxSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.EshopxSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .btn-style {
            color: #fff !important;
            background-color: #0f0f105e;
            border-color: #0481ed;
        }
        .Csslabel {
            color: white;
            font-size: medium;
            font-weight: bold;
            font-size:20px;
        }
        .lblStyle{
            background: #78aabd;
            height: 39px;
            text-align: center;
            padding-top: 7px;
            font-size: 16px;
            font-weight: bolder;
            color: black;
        }
        .table tr td{
            border-top:none !important;
        }
        #txtPathVal
        {
            width: 100%;
            height: 34px;
        }
        .settingLbl
        {
            color: white;
            font-size: medium;
            font-weight: bold;
            margin-top:5px;
        }
    </style>
      <div style="background-color: #202648; width: 100%; height: 100%;">
        <div class="container rounded" style="width: 60%; height: 50%; border: 2px outset ghostwhite; padding: 5px; background-color: #22b8e559; color: ghostwhite;margin-top: 20px;">
            <div class="lblStyle">
                <label class="Csslabel">EShopx Settings</label>
            </div>
            <br />
            <asp:UpdatePanel runat="server" ID="pnlSaveButton">
            <ContentTemplate>
                <table class="table">
                    <tr>
                        <td style="width:107px;">
                            <label class="settingLbl"  id="lblRootPath" runat="server">Root Path</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPathVal" runat="server" CssClass="form-control" ClientIDMode="Static" ></asp:TextBox>
                        </td>
                        <td style="width:100px;">
                             <asp:Button runat="server" ID="btnSave" CssClass="btn btn-style" Text="Save" OnClick="btnSave_Click" Style="min-width: 100px;" />
                        </td>
                    </tr>
                </table>
          
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
     </div>
     <script type="text/javascript">
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

     </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
