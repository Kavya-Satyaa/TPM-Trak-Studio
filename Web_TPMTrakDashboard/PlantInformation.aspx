<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlantInformation.aspx.cs" Inherits="Web_TPMTrakDashboard.PlantInformation" EnableEventValidation="false" %>

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

        #tblplant td {
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
                        <legend class="scheduler-border commontd" style="height: auto;"><%=GetGlobalResourceObject("CommanResource","PlantDefinition") %></legend>

                        <div id="gridPlant" style="overflow-x: auto; overflow-y: auto; max-width: 800px; max-height: 1000px;">
                            <asp:HiddenField ID="hdfgrdPlantInfo" runat="server" />
                            <asp:GridView runat="server" ID="GridPlantInfo" AutoGenerateColumns="False" CssClass="table table-bordered" HeaderStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="True" HeaderStyle-HorizontalAlign="Center" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" OnRowDataBound="GridPlantInfo_RowDataBound" OnRowCommand="GridPlantInfo_RowCommand">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:CommanResource,Select %>" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3" ControlStyle-Width="104">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" Text="<%$Resources:CommanResource,Select %>" runat="server" CssClass="btnStyle1" AutoCompleteType="Disabled" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" Font-Size="Medium"></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:CommanResource,PlantID %>" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3" ControlStyle-Width="200">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPlantID" Text='<%# Bind("PlantID") %>' runat="server" meta:resourcekey="txtPlantIDResource" AutoCompleteType="Disabled"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:CommanResource,PlantDescription %>" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource2" ControlStyle-Width="204">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPlantDesc" Text='<%# Eval("Description") %>' runat="server" meta:resourcekey="txtPlantDescResource" AutoCompleteType="Disabled"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:CommanResource,PlantCode %>" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3" ControlStyle-Width="204">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPlantCode" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" Text='<%# Bind("PlantCode") %>' runat="server" meta:resourcekey="txtPlantCodeResource" AutoCompleteType="Disabled"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" BackColor="#202648" Font-Bold="True" ForeColor="#CCCCFF"></HeaderStyle>
                                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                <RowStyle BackColor="White" ForeColor="#003399" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                <SortedDescendingHeaderStyle BackColor="#002876" />
                            </asp:GridView>
                        </div>

                        <div style="text-align: center">
                            <asp:Button ID="btnNew" runat="server" CssClass="btnStyle1" Text="<%$Resources:CommanResource,New %>" Width="80" OnClick="btnNew_Click" />
                            <asp:Button ID="btncancel" runat="server" CssClass="btnStyle1" Text="<%$Resources:CommanResource,Cancel %>" Width="80" OnClick="btncancel_Click" Visible="false" />
                            <asp:Button ID="btnSave" runat="server" Text="<%$Resources:CommanResource,Save %>" CssClass="btnStyle1" Width="80" OnClick="btnSave_Click" />
                        </div>
                    </fieldset>
                </div>

                <div class="row" style="margin-left: 1px; width: 800px;">
                    <fieldset class="scheduler-border" id="plantassign" style="width: 798px; height: auto;">
                        <%--<legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource","AssignUnassignMachineTofromplant") %></legend>--%>

                        <div class="md-12 lg-12">
                            <div class="col-md-5">
                                <div>
                                    <div style="background-color: white; color: black; text-align: center"><%=GetGlobalResourceObject("CommanResource","AssignedMachines") %></div>
                                    <br />
                                    <div style="background-color: white; height: 450px; overflow: auto; color: black">
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
                                <div style="background-color: white; color: black; text-align: center"><%=GetGlobalResourceObject("CommanResource","AvailableMachines") %></div>
                                <br />
                                <div style="background-color: white; color: black; height:450px; overflow: auto;">
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
            $('[id$=ddlplant]').editableSelect();

            function HideLabel() {
                var seconds = 2;
                setTimeout(function () {
                    document.getElementById("<%=lblMessages.ClientID %>").style.display = "none";
                }, seconds * 1000);
            };
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('[id$=ddlplant]').editableSelect();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
