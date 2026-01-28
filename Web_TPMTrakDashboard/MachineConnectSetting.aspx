<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineConnectSetting.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineConnectSetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .table tbody>tr>th{
            border-bottom:1px solid white;
            background-color:lightblue;
            color:black;
            text-align:center;
        }
        .table tbody>tr>td,.table tbody>tr>th
        {
            border-top:0px;
        }
        .commonStyle
        {
            color:white;
        }
    </style>
    <div class="col-lg-12 container-fluid">
        <div class="col-lg-5 DashboarGrid" style="border:1px solid white;padding:0px;">
           <table class="table">
               <tr>
                   <th colspan="2">Dashboard Settings</th>
               </tr>
                <tr>
                    <td class="commonStyle">Stoppage Threshold (sec)</td>
                    <td>
                         <asp:DropDownList ID="ddlStoppageThreshold" runat="server" CssClass="form-control">
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="30" Value="20"></asp:ListItem>
                            <asp:ListItem Text="35" Value="35"></asp:ListItem>
                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="60" Value="60"></asp:ListItem>
                            <asp:ListItem Text="120" Value="120"></asp:ListItem>
                            <asp:ListItem Text="180" Value="180"></asp:ListItem>
                            <asp:ListItem Text="240" Value="240"></asp:ListItem>
                            <asp:ListItem Text="300" Value="300"></asp:ListItem>
                            <asp:ListItem Text="360" Value="360"></asp:ListItem>
                            <asp:ListItem Text="420" Value="420"></asp:ListItem>
                            <asp:ListItem Text="480" Value="480"></asp:ListItem>
                            <asp:ListItem Text="540" Value="540"></asp:ListItem>
                            <asp:ListItem Text="600" Value="600"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="commonStyle">Operation History Folder Path</td>
                    <td>
                         <asp:TextBox ID="txtOpHistoryFolderPath" CssClass="form-control" runat="server" meta:resourcekey="txtOpHistoryFolderPathResource1" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="commonStyle">Program Folder Path</td>
                    <td>
                         <asp:TextBox ID="txtProgramFolderPath" CssClass="form-control" runat="server" meta:resourcekey="txtProgramFolderPathResource1" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="commonStyle">ProgramFileExtension</td>
                    <td>
                        <asp:DropDownList ID="ddlProgramFileExtension" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                </tr>
               <tr>
                   <td colspan="2" style="text-align:center;" class="dashbtnheight">
                         <asp:Button data-toggle="tooltip" ID="btnSave" runat="server" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary toolTip" OnClick="btnSave_Click" />
                   </td>
               </tr>
            </table>
        </div>
        <div class="col-lg-5 serviceGrid" style="border:1px solid white;padding:0px;">
             <table class="table">
               <tr>
                   <th colspan="2">Service Settings</th>
               </tr>
                 <tr>
                     <td class="commonStyle">Live Data Interval (sec.)</td>
                     <td>
                          <asp:DropDownList ID="ddlLiveData" runat="server" CssClass="form-control" Width="250">
                               <asp:ListItem Text="10" Value="10"></asp:ListItem>
                               <asp:ListItem Text="20" Value="20"></asp:ListItem>
                               <asp:ListItem Text="30" Value="30"></asp:ListItem>
                               <asp:ListItem Text="40" Value="40"></asp:ListItem>
                               <asp:ListItem Text="50" Value="50"></asp:ListItem>
                               <asp:ListItem Text="60" Value="60"></asp:ListItem>
                               <asp:ListItem Text="70" Value="70"></asp:ListItem>
                               <asp:ListItem Text="80" Value="80"></asp:ListItem>
                               <asp:ListItem Text="90" Value="90"></asp:ListItem>
                               <asp:ListItem Text="100" Value="100"></asp:ListItem>
                               <asp:ListItem Text="120" Value="120"></asp:ListItem>
                        </asp:DropDownList>
                     </td>
                 </tr>
                 <tr>
                     <td class="commonStyle">Alarm Data Interval (min.)</td>
                     <td>
                          <asp:DropDownList ID="ddlalarmData" runat="server" CssClass="form-control" Width="250">
                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                            <asp:ListItem Text="02" Value="01"></asp:ListItem>
                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                            <asp:ListItem Text="17" Value="17"></asp:ListItem>
                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="60" Value="60"></asp:ListItem>
                        </asp:DropDownList>
                     </td>
                 </tr>
                 <tr>
                     <td class="commonStyle">Spindle Data Interval (sec.)</td>
                     <td>
                          <asp:DropDownList ID="ddlSpindleData" runat="server" CssClass="form-control" Width="250">
                            <asp:ListItem Text="None" Value="None"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                            <asp:ListItem Text="17" Value="17"></asp:ListItem>
                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                            <asp:ListItem Text="35" Value="35"></asp:ListItem>
                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="55" Value="55"></asp:ListItem>
                            <asp:ListItem Text="60" Value="60"></asp:ListItem>
                            <asp:ListItem Text="65" Value="65"></asp:ListItem>
                        </asp:DropDownList>
                     </td>
                 </tr>
                   <tr>
                     <td class="commonStyle">Operation History Data Interval (min.)</td>
                     <td>
                          <asp:DropDownList ID="ddlOperationHistoryData" runat="server" CssClass="form-control" Width="250">
                               <asp:ListItem Text="None" Value="None"></asp:ListItem>
                               <asp:ListItem Text="30" Value="30"></asp:ListItem>
                               <asp:ListItem Text="60" Value="60"></asp:ListItem>
                               <asp:ListItem Text="90" Value="90"></asp:ListItem>
                               <asp:ListItem Text="120" Value="120"></asp:ListItem>
                        </asp:DropDownList>
                     </td>
                 </tr>
                  <tr>
                   <td colspan="2" style="text-align:center;" class="btnheight">
                         <asp:Button data-toggle="tooltip" ID="btnServiceSetting" runat="server" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary toolTip" OnClick="btnServiceSetting_Click" />
                   </td>
               </tr>
             </table>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $(".DashboarGrid").height($(".serviceGrid").height());
        });

        function messageNotOk() {
            Command: toastr["error"]("Data Not Saved!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"

            }
        }
        function messageOk() {
            Command: toastr["success"]("Saved Successfully")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
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


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(".DashboarGrid").height($(".serviceGrid").height());
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
