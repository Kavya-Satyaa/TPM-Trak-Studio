<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TargetOEE.aspx.cs" Inherits="Web_TPMTrakDashboard.TargetOEE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        .txtcolor {
            color: white;
        }

        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
            height: 38px;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        .textboxcss {
            border: none;
            background-color: transparent;
            font-style: italic;
            color: black;
        }

        .addtextcss {
            border: initial;
            background-color: none;
        }
        #grdOEE tr td:first-child{
             min-width:200px;
        }
        #grdOEE > tbody > tr:nth-child(odd) > td{
            background: #F2F2F2
        }
        #grdOEE > tbody > tr:nth-child(even) > td{
            background: #FFFFFF;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <div class="row" id="griddata">
                <table class="table table-bordered " id="tblheader" style="margin-top: 0px; color: white; width: 45%; width: auto;margin-left:15px;">
                    <tbody>
                        <tr>
                            <td>
                                <div style="margin-top: 6px; min-width: 60px;" class="txtcolor">Year</div>
                            </td>
                            <td class="input-group" style="border: none; margin-top: 5px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" data-toggle="tooltip"
                                    title="Year here !" placeholder="Year here" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <div style="margin-top: 6px; min-width: 60px;" class="txtcolor">Machine ID</div>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control"/>
                            </td>
                            <td>
                                <div style="margin-top: 6px; min-width: 60px;" class="txtcolor">Efficiencies</div>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlEfficiencies" CssClass="form-control">
                                    <asp:ListItem Text="%AE" Value="AE" />
                                    <asp:ListItem Text="%PE" Value="PE" />
                                    <asp:ListItem Text="%QE" Value="QE" />
                                    <asp:ListItem Text="%OEE" Value="OE" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <%--<div style="margin-top: 5px;">--%>
                                    <asp:Button runat="server" Text="Search" Style="float: left" class="btn btn-info" ID="btnsearch" OnClick="btnsearch_Click"></asp:Button>&nbsp;&nbsp;
                             <asp:Button runat="server" Text="Save" class="btn btn-info" ID="btnsave" OnClick="btnsave_Click"></asp:Button>&nbsp;
                              <%--  </div>--%>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="Oeediv" style="margin-top: 0px; text-align: center; overflow: auto;height:82vh">
                <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; text-align-last: center; font-family: Calibri;"></asp:Label>
                <asp:GridView ID="grdOEE" runat="server" AutoGenerateColumns="false" Width="100%"
                    HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true"
                    HeaderStyle-CssClass="blue" BackColor="#FFFFFF" CssClass="headerFixer" ClientIDMode="Static">
                    <Columns>
                        <asp:TemplateField HeaderText="Machine">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfcheckupdate" runat="server" />
                                <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("Machineid") %>'></asp:Label>
                               <%-- <asp:TextBox runat="server" ID="machine" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("Machineid") %>'></asp:TextBox>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="January">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbljan" Visible="false" Text='<%# Eval("January") %>'></asp:Label>
                                <asp:TextBox runat="server" ID="txtjan" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("January") %>'></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" Type="Integer" ID="pinvalidate" ControlToValidate="txtjan" ValidationExpression="\d{1,4}" ErrorMessage="Enter 4 Dig No" ForeColor="Red" />
                                <%--<asp:RegularExpressionValidator runat="server" ControlToValidate="txtjan" ValidationExpression="\d{1,4}" SetFocusOnError="true"  ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="February">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblfeb" Style="text-align: left" CssClass="textboxcss" Visible="false" Text='<%# Eval("February") %>'></asp:Label>
                                <asp:TextBox runat="server" ID="txtfeb" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("February") %>'></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtfeb" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="March">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtmar" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("March") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lblmar" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("March") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtmar" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="April">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtapr" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("April") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lblapr" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("April") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtapr" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="May">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtmay" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("May") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lblmay" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("May") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtmay" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="June">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtjune" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("June") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lbljune" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("June") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtjune" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="July">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtjuly" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("July") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lbljuly" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("July") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtjuly" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="August">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtaug" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("August") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lblaug" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("August") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtaug" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="September">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtsept" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("September") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lblsept" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("September") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtsept" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="October">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtoct" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("October") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lbloct" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("October") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtoct" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="November">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtnov" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("November") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lblnov" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("November") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtnov" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="December">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtdec" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("December") %>'></asp:TextBox>
                                <asp:Label runat="server" ID="lbldec" Visible="false" Style="text-align: left" CssClass="textboxcss" Text='<%# Eval("December") %>'></asp:Label>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtdec" ValidationExpression="\d{1,4}" SetFocusOnError="true" ErrorMessage="Enter 4 Dig No" ForeColor="Red"></asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle Font-Bold="True" ForeColor="White" />
                </asp:GridView>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>

        $(document).ready(function () {
            freezeColumnFromLeft('grdOEE', 1);
        });

        $('[id$=txtToDate]').datetimepicker({
        /*format: 'DD-MMM-YYYY',*/
            format: 'YYYY',
            useCurrent: true

        });

        $("[id$=grdOEE]").on("click", "td", function () {
            $(this).closest('tr').find('input[type=hidden]').val("updated");
            $("[id$=grdOEE] tr td").find('input').removeClass("form-control");
            $("[id$=grdOEE] tr td").find('input').addClass("textboxcss");
            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");

        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                freezeColumnFromLeft('grdOEE', 1);
            });
            $('[id$=txtToDate]').datetimepicker({
                //format: 'DD-MMM-YYYY',
                format: 'YYYY',
                useCurrent: true
            });
            $("[id$=grdOEE]").on("click", "td", function () {
                $(this).closest('tr').find('input[type=hidden]').val("updated");
                $("[id$=grdOEE] tr td").find('input').removeClass("form-control");
                $("[id$=grdOEE] tr td").find('input').addClass("textboxcss");
                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
