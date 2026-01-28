<%@ Page Title="Down Code By Station" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DownCodeByStation.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.DownCodeByStation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/editDropDownJs") %>
    <%: Styles.Render("~/bundles/editDropDownCss") %>

    <style>
        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 235px;
            color: white;
            width: 60%;
        }


        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 250px;
            color: white;
            width: 60%;
        }


        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
        }

        #tblHMI td {
            color: white;
        }

        ul {
            color: black;
        }

        .btnStyle1 {
            background: #202648;
            border-radius: 60px;
            border-color: white;
            color: #fff;
            height: 30px;
        }
    </style>
    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 1px;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
                <div class="row" style="margin-left: 1px; max-width: 800px;">
                    <fieldset class="scheduler-border1" id="stdtime" style="height: auto;">
                        <legend class="scheduler-border commontd" style="height: auto;">Machine Definition</legend>
                        <div >
                            <table>
                                <tr>
                                    <td style="margin:5px;height:40px">
                                        <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" />
                                        </td>
                                    <td style="margin:5px;height:40px">
                                        <asp:Button runat="server" Height="35" Width="100"  ID="btnView" OnClick="btnView_Click" CssClass="btn btn-info btn-sm displayCss" Text="View" style="margin:5px" />
                                    
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>

                <div class="row" style="margin-left: 1px; width: 800px;">
                    <fieldset class="scheduler-border" id="HMIassign" style="width: 798px; height: auto;">
                        <legend class="scheduler-border commontd">Assign/Unassign Down's To/ from Machine's</legend>

                        <div class="md-12 lg-12">
                            <div class="col-md-5">
                                <div>
                                    <div style="background-color: white; color: black; text-align: center">Assigned Down</div>
                                    <br />
                                    <div style="background-color: white; height: 120px; overflow: auto; color: black">
                                        <asp:CheckBoxList ID="chkassigned" runat="server" ForeColor="Black"></asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div style="margin-top: 25px;">
                                    <asp:Button ID="btnassign" runat="server" Style="background-color: black; margin-top: 20px; color: white" Text="<<" OnClick="btnassign_Click" />
                                    <asp:Button ID="btnunassign" Style="background-color: black; margin-top: 20px; color: white" runat="server" Text=">>" OnClick="btnunassign_Click" />
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div style="background-color: white; color: black; text-align: center">Available Down</div>
                                <br />
                                <div style="background-color: white; color: black; height: 120px; overflow: auto;">
                                    <asp:CheckBoxList ID="chkaveliable" runat="server" ForeColor="Black"></asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            $('[id$=ddlHMI]').editableSelect();

            function HideLabel() {
                var seconds = 2;
                setTimeout(function () {
                    document.getElementById("<%=lblMessages.ClientID %>").style.display = "none";
                }, seconds * 1000);
            };
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('[id$=ddlHMI]').editableSelect();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
