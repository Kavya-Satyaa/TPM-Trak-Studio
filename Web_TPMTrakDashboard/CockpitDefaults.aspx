<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CockpitDefaults.aspx.cs" Inherits="Web_TPMTrakDashboard.CockpitDefaults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .Csslabel {
            color: white;
            font-size: medium;
            font-weight: bold;
            padding-left: 75px;
        }
        .csstd{
            padding-top:25px;
            padding-right:15px
        }
    </style>
    <link href="Scripts/Toast/jquery.toast.min.css" rel="stylesheet" />
    <script src="Scripts/Toast/jquery.toast.min.js"></script>
    <div style="background-color: #202648; width: 100%; height: 100%;">
        <div class="container rounded" style="width: 40%; height: 50%; border: 2px outset ghostwhite; padding: 5px; background-color: midnightblue; color: ghostwhite">
            <div>
                <h3 style="font-weight: bolder; color: ghostwhite; text-align: center;">Cockpit Defaults
                </h3>
            </div>
            <br />
            <table>
                <tr>
                    <td>
                        <label class="col-6 Csslabel"  id="timein" runat="server">Time in</label>
                    </td>
                    <td class="csstd">
                        <asp:DropDownList runat="server" ID="ddlTimeIn" CssClass="form-control input-sm" Width="200px" Height="28px">
                            <asp:ListItem Text="hh" Value="hh" />
                            <asp:ListItem Text="mm" Value="mm" />
                            <asp:ListItem Text="ss" Value="ss" />
                            <asp:ListItem Text="hh:mm:ss" Value="hh:mm:ss" />
                        </asp:DropDownList><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="col-6 Csslabel" id="DftDisplay" runat="server">Default Display</label>
                    </td>
                    <td class="csstd">
                        <asp:DropDownList runat="server" ID="ddlDefaultDisplay" CssClass="form-control input-sm" Width="200px" Height="28px">
                            <asp:ListItem Text="Current Shift" Value="CurrentShift" />
                            <asp:ListItem Text="Previous Shift" Value="PreviousShift" />
                            <asp:ListItem Text="Last 24Hrs" Value="Last 24Hrs" />
                        </asp:DropDownList><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="col-6 Csslabel" id="Machines" runat="server">Machines</label>
                    </td>
                    <td class="csstd">
                        <asp:DropDownList runat="server" ID="ddlMachines" CssClass="form-control input-sm" Width="200px" Height="28px">
                            <asp:ListItem Text="TPM Track Enabled Machines Only" Value="E" />
                            <asp:ListItem Text="All Machines" Value="D" />
                        </asp:DropDownList><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="col-6 Csslabel" id="VDG_PCD" runat="server">VDG-Prod. Cycle Definition</label>
                    </td>
                    <td class="csstd">
                        <asp:DropDownList runat="server" ID="ddlVDG_PCD" CssClass="form-control input-sm" Width="200px" Height="28px">
                            <asp:ListItem Text="CycleTime(Less ICD only)" Value="1" />
                            <asp:ListItem Text="CycleTime(Less ICD,Less PDT)" Value="2" />
                        </asp:DropDownList><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="col-6 Csslabel" id="VDG_CD" runat="server">VDG-Component Description</label>
                    </td>
                    <td class="csstd">
                        <asp:DropDownList runat="server" ID="ddlVDG_CD" CssClass="form-control input-sm" Width="200px" Height="28px">
                            <asp:ListItem Text="In VDG Grid - ComponentID without Description" Value="1" />
                            <asp:ListItem Text="In VDG Grid - ComponentID with Description" Value="2" />
                        </asp:DropDownList><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="col-6 Csslabel" id="DisplayTime" runat="server">Total Time</label>
                    </td>
                    <td class="csstd">
                        <asp:DropDownList runat="server" ID="ddlDisplayTime" CssClass="form-control input-sm" Width="200px" Height="28px">
                            <asp:ListItem Text="Display TotalTime" Value="Display TotalTime" />
                            <asp:ListItem Text="Display TotalTime - Less PDT" Value="Display TotalTime - Less PDT" />
                        </asp:DropDownList><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="col-6 Csslabel" id="IVDO" runat="server">Iconic View Display Option&nbsp; </label>
                    </td>
                    <td class="csstd">
                        <asp:DropDownList runat="server" ID="ddlIVDO" CssClass="form-control input-sm" Width="200px" Height="28px">
                            <asp:ListItem Text="Display Job Code" Value="Display Job Code" />
                            <asp:ListItem Text="Display Return per Hour Utilised" Value="Display ReturnperHourUtilised" />
                        </asp:DropDownList><br />
                    </td>
                </tr>

            </table>
            <asp:UpdatePanel runat="server" ID="pnlSaveButton">
                <ContentTemplate>
                    <div style="display: flex; justify-content: flex-end; margin-right: auto;">
                        <%--<p class="col"><button  type="button" id="Save_button" class="btn btn-primary" runat="server">Save</button></p>--%>
                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" Style="min-width: 100px;" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <%--<div class="toast" role="dialog" aria-live="assertive" aria-atomic="true" id="mySuccessToast" >
        <div class="toast-header">
            
            <strong class="mr-auto">Success!</strong>
            
            <button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="toast-body" id="toastMessage">
            Setting Updated Successfully.
        </div>
    </div>--%>


        <script type="text/javascript">
            function openSuccessToast() {
                $.toast({
                    text: "Settings Updated Successfully.",
                    heading: 'Success!',
                    icon: 'success',
                    showHideTransition: 'fade',
                    allowToastClose: true,
                    hideAfter: 3000,
                    stack: false,
                    position: 'top-right',
                    bgColor: '#2AB802',

                    textAlign: 'left',
                    loader: true,
                    loaderBg: '#9EC600',

                });
            };
            function openToast() {
                Command: toastr["success"]("Settings Successfully Updated")
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
        </script>
    </div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
