<%@ Page Title="FIR Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FIRTransDataDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.FIRTransDataDenso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .table-style1 {
            background-color: white;
            width: 100%;
        }

        .td-slno {
            width: 50px;
            background-color: #D9D9D9;
            color: blue;
            font-weight: 600;
            text-align: center;
        }

        .td-header-lbl {
            background-color: #D9D9D9;
            width: 300px;
            color: blue;
            font-weight: 600;
        }

        .table-style1 tr td {
            border: 1px solid black;
            padding: 10px;
        }
        /*  .table-style1 > tbody > tr > td, .tblLineSection tr td, #tblAttendees tr td, .tblNextAction tr td {
            border: 1px solid black;
            padding: 10px;
        }*/

        .tblLineSection {
            table-layout: fixed;
            width: 100%;
        }

        #tblAttendees, .tblNextAction {
            width: 100%;
        }

            #tblAttendees .td-attendees-header {
                width: 100px;
                color: blue;
            }

        .tdOrangeBackColor {
            background-color: #FCD5B5;
        }

        .red-back {
            background-color: #ff5b5b;
        }
    </style>
    <asp:HiddenField ClientIDMode="Static" runat="server" ID="hdnMachineID" />
    <asp:HiddenField ClientIDMode="Static" runat="server" ID="hdnDownCategory" />
    <asp:HiddenField ClientIDMode="Static" runat="server" ID="hdnDownID" />
    <asp:HiddenField ClientIDMode="Static" runat="server" ID="hdnStartTime" />
    <asp:HiddenField ClientIDMode="Static" runat="server" ID="hdnEndTime" />
    <asp:HiddenField ClientIDMode="Static" runat="server" ID="hdnShift" />
    <div>
        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" />
        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
        <table class="table-style1">
            <tr>
                <td colspan="10" style="text-align: center; font-size: 25px; font-weight: bold; color: #1212be;">First Information Report
                </td>
            </tr>
            <tr>
                <td class="td-slno">1
                </td>
                <td class="td-header-lbl">Problem
                </td>
                <td>
                    <asp:Label runat="server" ID="lblDownDesc" ClientIDMode="Static"></asp:Label>
                </td>
                <td style="width: 300px; padding: 0px">
                    <table style="width: 100%; table-layout: fixed; text-align: center;">
                        <tr>
                            <td id="tdHour1" runat="server">1hr</td>
                            <td id="tdHour2" runat="server">2hr</td>
                            <td id="tdHour3" runat="server">3hr</td>
                            <td id="tdHour4" runat="server">4hr</td>
                        </tr>
                        <tr>
                            <td id="tdHour5" runat="server">5hr</td>
                            <td id="tdHour6" runat="server">6hr</td>
                            <td id="tdHour7" runat="server">7hr</td>
                            <td id="tdHour8" runat="server">>8hr</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td-slno">2
                </td>
                <td class="td-header-lbl">Line Name / Section
                </td>
                <td style="padding: 0px;">
                    <table class="tblLineSection">
                        <tr>
                            <td rowspan="2">
                                <asp:Label runat="server" ID="lblPlant" ClientIDMode="Static"></asp:Label>
                                <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td class="td-header-lbl">Start Time / Shift / Date
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblStartData" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-header-lbl">Finish Time / Shift / Date
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblEndData" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td rowspan="5" class="tdOrangeBackColor"></td>
            </tr>
            <tr>
                <td class="td-slno">3
                </td>
                <td class="td-header-lbl">Route Cause
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtRouteCause" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-slno">4
                </td>
                <td class="td-header-lbl">Attendees 
                </td>
                <td style="padding: 0px;">
                    <table id="tblAttendees">
                        <tr>
                            <td class="td-attendees-header">PRD
                            </td>
                            <td>
                                <asp:ListBox ID="lbPRDEmpID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </td>
                            <td class="td-attendees-header">QAD
                            </td>
                            <td>
                                <asp:ListBox ID="lbQADEmpID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-attendees-header">MTD
                            </td>
                            <td>
                                <asp:ListBox ID="lbMTDEmpID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </td>
                            <td class="td-attendees-header">SQD
                            </td>
                            <td>
                                <asp:ListBox ID="lbSQDEmpID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-attendees-header">PED
                            </td>
                            <td>
                                <asp:ListBox ID="lbPEDEmpID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </td>
                            <td class="td-attendees-header">PCD
                            </td>
                            <td>
                                <asp:ListBox ID="lbPCDEmpID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td-slno"></td>
                <td colspan="2">
                    <asp:GridView ID="gvActionTaken" CssClass="table table-bordered headerFixer table-alternate-row" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static" ShowFooter="true" OnRowDataBound="gvActionTaken_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Action Taken">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                    <asp:Label ID="lblActionTaken" runat="server" Text='<%# Eval("ActionTaken") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtActionTaken" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="By Whoom">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnDepartment" ClientIDMode="Static" Value='<%# Eval("ActionTakenByWhom") %>' />
                                    <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" CssClass="form-control">
                                        <asp:ListItem Text="PRD" Value="PRD"></asp:ListItem>
                                        <asp:ListItem Text="MTD" Value="MTD"></asp:ListItem>
                                        <asp:ListItem Text="PED" Value="PED"></asp:ListItem>
                                        <asp:ListItem Text="QAD" Value="QAD"></asp:ListItem>
                                        <asp:ListItem Text="SQD" Value="SQD"></asp:ListItem>
                                        <asp:ListItem Text="PCD" Value="PCD"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" CssClass="form-control">
                                        <asp:ListItem Text="PRD" Value="PRD"></asp:ListItem>
                                        <asp:ListItem Text="MTD" Value="MTD"></asp:ListItem>
                                        <asp:ListItem Text="PED" Value="PED"></asp:ListItem>
                                        <asp:ListItem Text="QAD" Value="QAD"></asp:ListItem>
                                        <asp:ListItem Text="SQD" Value="SQD"></asp:ListItem>
                                        <asp:ListItem Text="PCD" Value="PCD"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Impact / Result:">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtResult" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("ActionTakenResult") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtResult" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" ID="lnkInsertActionTaken" ClientIDMode="Static" OnClick="lnkInsertActionTaken_Click" CssClass="glyphicon glyphicon-plus-sign"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="FooterRow" />
                    </asp:GridView>
                    <asp:Button runat="server" ID="btnSaveActionTaken" Text="Save" CssClass="btn btn-info" OnClick="btnSaveActionTaken_Click" />
                </td>
            </tr>
            <tr>
                <td class="td-slno">6
                </td>
                <td colspan="2" style="padding: 0px">
                    <table class="tblNextAction">
                        <tr>
                            <td class="td-header-lbl">Next Action Decided :</td>
                            <td class="td-header-lbl">By Whom</td>
                            <td class="td-header-lbl">Impact / Result:</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtNextActionDecided" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlNextActionDecidedDept" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Text="PRD" Value="PRD"></asp:ListItem>
                                    <asp:ListItem Text="MTD" Value="MTD"></asp:ListItem>
                                    <asp:ListItem Text="PED" Value="PED"></asp:ListItem>
                                    <asp:ListItem Text="QAD" Value="QAD"></asp:ListItem>
                                    <asp:ListItem Text="SQD" Value="SQD"></asp:ListItem>
                                    <asp:ListItem Text="PCD" Value="PCD"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtNextActionDecidedResult" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td-slno">7
                </td>
                <td class="td-header-lbl">Stock Status
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtStockStatus" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
                <td rowspan="2" class="tdOrangeBackColor">Details:
                      <asp:TextBox runat="server" ID="txtDetails" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-slno">8
                </td>
                <td class="td-header-lbl">Stock Impact
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtStockImpact" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-slno">9
                </td>
                <td class="td-header-lbl">Present Status
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPresentStatus" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                </td>
                <td style="padding: 0px" class="tdOrangeBackColor">
                    <table>
                        <tr>
                            <td>Part No. - 
                                <asp:TextBox runat="server" ID="txtPartNo" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Part Name - 
                                <asp:TextBox runat="server" ID="txtPartName" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Qty hold - 
                                <asp:TextBox runat="server" ID="txtQtyHold" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $('.listBoxControl').multiselect({
                includeSelectAllOption: true
            });
            $('#sidebar').hide();
            $('#main-content').css('margin-left', '0px');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
