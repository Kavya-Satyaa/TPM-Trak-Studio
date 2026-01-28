<%@ Page Title="Employee-Cell" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlantCellEmployeeAssociationKTA.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.PlantCellEmployeeAssociationKTA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .headerFixer tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        .headerFixer tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <asp:HiddenField runat="server" ID="hdnCheckboxClick" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Plant</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto;">
                <asp:GridView runat="server" ID="gvEmployeeData" ClientIDMode="Static" EmptyDataText="No records" CssClass="table table-bordered headerFixer" OnRowDataBound="gvEmployeeData_RowDataBound" AutoGenerateColumns="false" ShowFooter="false"></asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $('input[type=checkbox]').change(function () {
            $('[id*=hdnCheckboxClick]').val("update");
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
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('input[type=checkbox]').change(function () {
                $('[id*=hdnCheckboxClick]').val("update");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
