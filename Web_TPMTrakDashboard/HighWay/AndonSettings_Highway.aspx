<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonSettings_Highway.aspx.cs" Inherits="Web_TPMTrakDashboard.HighWay.AndonSettings_Highway" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.min.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/Site.css" rel="stylesheet" />
    <link href="../GEA/Andon_GEA/Content/TextAnimation.css" rel="stylesheet" />
    <script src="../GEA/Andon_GEA/Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        /*   #menu {
            position: fixed;
            top: 0;
            background: #1a2732;
            box-shadow: 0 0 10px black;
        }*/

        .HeaderImage {
            flex: 1;
            float: left;
            width: 0px;
        }

        .legendStyle {
            /*display: block;*/
            /*width: 20%;*/
            /*padding: 0;*/
            margin: 0px;
            /*font-size: 18px;*/
            color: black;
            border: 1px solid black;
            text-align: center;
        }

        .tableStyle {
            width: 100%;
            max-width: 100%;
            font-size: 17px;
            margin-bottom: 0px;
            color: black;
            text-align: left;
        }
/**/
            .tableStyle tr td:first-child {
                font-weight:bold;
            }
            .tableStyle tr td{
                padding:10px;
            }
        .table > tbody > tr > td {
            padding: 4px;
            vertical-align: top;
            border-top: 1px solid white;
            color: black;
        }

        .headerFixer tr th {
            padding: 4px 6px;
            font-size: 14px;
            background-color: #2e6886;
            border: none;
            color: white;
            position: sticky;
            top: 0px;
            z-index: 20;
        }

        .headerFixer tr td {
            padding: 4px 6px;
            font-size: 14px;
            border: none;
            border-bottom: 1px solid #d6d7df;
        }

        .pick-a-color-markup .pick-a-color {
            height: 40px;
        }

        label {
            font-weight: unset;
        }
         .legendStyleSetting {
            display: block;
            padding: 4px 10px;
            width: 25%;
            font-size: 27px;
            color: #333;
            border: 0px !important;
            font-weight: bold;
            text-align:left;
        }


         fieldset {
            border: 1px solid black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12">
                            <div>
                                <div class="navbar navbar-fixed-top text-center" style="background-color: #106884;" id="menu">
                                    <div class="HeaderImage">
                                        <img runat="server" src="~/Images/logo/AMITLogo.png" id="toggle" class="img-responsive img-rounded" alt="Logo" style="cursor: pointer; padding-right: 1px; margin-top: 4px; height: 52px;" />
                                    </div>
                                    <span id="headerName" style="color: white; font-weight: bold; font-size: 30px; margin: auto; line-height: 60px;">Andon Settings</span>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-lg-4" style="margin-top: 20px;text-align: center;">
                                <fieldset id="lblfieldset" style="border-color: black">
                                    <legend class="legendStyleSetting">Settings</legend>
                                    <table class="tableStyle table">
                                        <tr>
                                            <td>Device Name</td>
                                            <td>
                                                <asp:Label runat="server" ID="lblComputerName" CssClass="form-control" ClientIDMode="Static"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Main Header Font Size</td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control allowNumber" ClientIDMode="Static" ID="txtMainHeaderfontsize"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>Sub-Header Font Size</td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control allowNumber" ClientIDMode="Static" ID="txtSubheaderFontsize"></asp:TextBox>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>Efficiency Header Font Size</td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control allowNumber" ClientIDMode="Static" ID="txtEfficiencyHeaderFontsize"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Efficiency Value Font Size</td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control allowNumber" ClientIDMode="Static" ID="txtEfficiencyFontsize"></asp:TextBox>
                                            </td>
                                        </tr>
                                       <%-- <tr>
                                            <td>Sub Header Text</td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control" ClientIDMode="Static" ID="txtSubheaderContent"></asp:TextBox>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>Data Refresh Interval</td>
                                            <td>
                                                <asp:TextBox runat="server" CssClass="form-control allowNumber" ClientIDMode="Static" ID="txtRefreshInterval"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align:center !important;">
                                                <asp:Button runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="Save" Style="height: 30px; padding: 3px 15px;" ID="btnSAve" OnClick="btnSAve_Click" />
                                                <asp:Button runat="server" CssClass="btn btn-success" Text="Return to Andon" ID="btnAndon" Style="height: 30px; padding: 3px 15px;" OnClick="btnAndon_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div class="col-lg-7" style="margin-top: 20px;text-align: center;">
                                <fieldset style="border-color: black">
                                    <legend class="legendStyleSetting">Help</legend>
                                    <table class="tableStyle table">
                                        <tr>
                                            <td style="width:30%;">Performance Efficiency (PE)</td>
                                            <td>PE=[(Standard Production Time)/Utilised Time];</td>
                                        </tr>
                                        <tr>
                                            <td style="width:30%;">Machine Availability (AE)</td>
                                            <td>AE=[(Total Time-Planned Loss) - (Actual Down Time) / (Total Time-Planned Loss)];</td>
                                        </tr>
                                        <tr>
                                            <td style="width:30%;">Over All Efficiency (OEE)</td>
                                            <td>OEE = [AE*PE*QE] * 100</td>
                                        </tr>
                                        <tr>
                                            <td style="width:30%;">Line OEE</td>
                                            <td>Line OEE is the average OEE of the Final Machine (End of Group) for all the Cells (Marked as EndOfGroupMachine in Cell Definition)</td>
                                        </tr>
                                        <tr></tr>
                                    </table>

                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSAve" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <script>
            $('.allowNumber').keypress(function (evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 48 && pos == 0) {
                    return false
                } else if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            });
            function showSuccessMsg(msg, title) {
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

                toastr['success'](msg, title);
                return false;
            }
            function toasterWarningMsg(msg, title) {
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": true,
                    "onclick": null,
                    "showDuration": "10000",
                    "hideDuration": "10000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut",
                    "toastClass": "toaster-position"
                }
                var d = Date();
                toastr.warning(msg, title);
                return false;
            }
        </script>
    </form>
</body>
</html>
