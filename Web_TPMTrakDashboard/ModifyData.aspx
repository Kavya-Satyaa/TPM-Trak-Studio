<%@ Page Title="Modify Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifyData.aspx.cs" ValidateRequest="false" EnableEventValidation="false" Debug="true" Inherits="Web_TPMTrakDashboard.ModifyData" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #downdatagrid tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #downdatagrid tr:nth-child(even) {
            background-color: #DCDCDC;
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

        .select {
            /*border-radius: 5px;*/
            -webkit-appearance: none;
        }

        #tblheader tbody > tr > td {
            padding-top: 3px;
            height: 10px;
            padding-bottom: 0px;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            padding: 1px;
        }

        td {
            text-align: center;
            height: 10px;
        }

        .txtcolor {
            color: white;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            height: 136px;
        }

        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: -14px 0 1.5em 4px !important;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            font-size: 1.1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
            margin-top: -4px;
            color: white;
        }

        /*/--------------Boostrap css--------------/*/
        .pagination-ys {
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }
    </style>

    <asp:UpdatePanel ID="updatePanalMain" runat="server">
        <ContentTemplate>
            <div class="row">
                <fieldset class="scheduler-border1" id="viewbox" style="height: 70px">
                    <legend class="scheduler-border">View Data</legend>
                    <table class="table table-bordered" id="tblheader" style="margin-top: -23px; column-width: auto; width: auto; color: white">
                        <tbody>
                            <tr>
                                <td>
                                    <div style="margin-top: 5px; min-width: 65px;" class="txtcolor">Plant ID</div>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlPlantID" runat="server" Style="min-width: 70px;" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-top: 5px; min-width: 65px;" class="txtcolor">Machine </div>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlMachineID" CssClass="form-control" Style="min-width: 70px;" runat="server"></asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-top: 5px; min-width: 72px;" class="txtcolor">Data Type</div>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlDataType" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="Production Data">Production Data</asp:ListItem>
                                            <asp:ListItem Value="Down Data"> Down Data </asp:ListItem>
                                            <asp:ListItem Value="Rejection Data"> Rejection Data </asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-top: 5px; min-width: 70px;" class="txtcolor">From Time</div>
                                </td>
                                <td class="input-group">
                                    <div class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></div>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" data-toggle="tooltip" title="From Date !" placeholder="From Date" AutoCompleteType="Disabled" />
                                </td>
                                <td>
                                    <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">To Time</div>
                                </td>
                                <td class="input-group">
                                    <div class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></div>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" data-toggle="tooltip" title="To Date !" placeholder="To Date" AutoCompleteType="Disabled" />
                                </td>
                                <td colspan="2">
                                    <div style="min-width: 130px;">
                                        <asp:Button runat="server" Text="View" Style="float: none; min-width: 60px; margin-left: 5px;" class="btn btn-info" ID="btnView" OnClick="btnView_Click"></asp:Button>
                                        <asp:Button runat="server" Text="Save" Style="float: none; min-width: 60px; margin-left: 5px;" class="btn btn-info" ID="btnSave"></asp:Button>&nbsp;
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </fieldset>
            </div>

            <div id="divDataGrids" style="height: 600px; overflow: auto; margin-top: 5px;">
                <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; color: white; text-align: center; font-family: Calibri;"></asp:Label>

                <%--Production Data Grid--%>
                <asp:GridView ID="datagridProductionData" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="100" AllowPaging="true" HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available." ShowHeaderWhenEmpty="true" ShowHeader="true" HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="datagridProductionData_RowDataBound" OnPageIndexChanging="datagridProductionData_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="WorkOrderNo">
                            <ItemTemplate>
                                <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="WorkOrderNo" Text='<%# Eval("WorkOrderNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Component">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfComponentProd" runat="server" Value='<%# Eval("Component") %>' />
                                <asp:DropDownList ID="ddlComponentProd" CssClass="textboxcss select" runat="server"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="Operation" Style="width: 70px; text-align: left" CssClass="textboxcss" Text='<%# Eval("Operation") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operator">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfOperatorProd" runat="server" Value='<%# Eval("Operator") %>' />
                                <asp:DropDownList ID="ddlOperatorProd" CssClass="textboxcss select" runat="server"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PartsCount">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="PartsCount" CssClass="textboxcss" Text='<%# Eval("PartsCount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TimeFrom">
                            <ItemTemplate>
                                <asp:Label ID="TimeFrom" runat="server" Text='<%# Eval("TimeFrom") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TimeTo">
                            <ItemTemplate>
                                <asp:Label ID="TimeTo" runat="server" Text='<%# Eval("TimeTo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="pagination-ys" />
                    <PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" />
                </asp:GridView>

                <asp:GridView ID="datagridDownData" runat="server" AutoGenerateColumns="false" Width="100%" HeaderStyle-Font-Italic="true" ShowHeaderWhenEmpty="true" ShowHeader="true" HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" EmptyDataText="No Data Available." Visible="false" OnRowDataBound="datagridDownData_RowDataBound" PageSize="100" AllowPaging="true" OnPageIndexChanging="datagridDownData_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="WorkOrderNo">
                            <ItemTemplate>
                                <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="WorkOrderNo" Text='<%# Eval("WorkOrderNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ComponentName">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfComponentDown" runat="server" Value='<%# Eval("Component") %>' />
                                <asp:DropDownList ID="ddlComponentDown" runat="server" CssClass="textboxcss select"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="Operation" Style="width: 70px; text-align: left" CssClass="textboxcss" Text='<%# Eval("Operation") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operator">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfOperatorDown" runat="server" Value='<%# Eval("Operator") %>' />
                                <asp:DropDownList ID="ddlOperatorDown" CssClass="textboxcss select" runat="server"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Down Code">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfDownCode" runat="server" Value='<%# Eval("downcode") %>' />
                                <asp:DropDownList ID="ddlDowncodeDown" runat="server" CssClass="textboxcss select"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TimeFrom">
                            <ItemTemplate>
                                <asp:Label ID="downFromTime" CssClass="splitfun" runat="server" ids='<%# Eval("ID") %>' ComponentID='<%# Eval("Component") %>' OperationNo='<%# Eval("Operation") %>' DownID='<%# Eval("downcode") %>' OperatorID='<%# Eval("Operator") %>' ndtime='<%# Eval("TimeTo") %>' sttime='<%# Eval("TimeFrom") %>' Text='<%# Eval("TimeFrom") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TimeTo">
                            <ItemTemplate>
                                <asp:Label ID="downToTime" CssClass="splitfun" runat="server" ids='<%# Eval("ID") %>' ComponentID='<%# Eval("Component") %>' OperationNo='<%# Eval("Operation") %>' DownID='<%# Eval("downcode") %>' OperatorID='<%# Eval("Operator") %>' ndtime='<%# Eval("TimeTo") %>' sttime='<%# Eval("TimeFrom") %>' Text='<%# Eval("TimeTo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle Font-Bold="True" ForeColor="White" />
                </asp:GridView>

                <asp:GridView ID="datagridRejectionData" runat="server" AutoGenerateColumns="false" Width="100%" HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true" HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" PageSize="100" AllowPaging="true">
                    <Columns>
                        <asp:TemplateField HeaderText="ComponentName">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfComponentRej" runat="server" Value='<%# Eval("Component") %>' />
                                <asp:DropDownList ID="ddlComponentRej" runat="server" CssClass="textboxcss select"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="Operation" Style="width: 70px; text-align: left" CssClass="textboxcss" Text='<%# Eval("Operation") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operator">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfOperatorRej" runat="server" Value='<%# Eval("Operator") %>' />
                                <asp:DropDownList ID="ddlOperatorRej" CssClass="textboxcss select" runat="server"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejection Code">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="RejectionCodeID" CssClass="textboxcss" Text='<%# Eval("RejectionCode") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejection Qty">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="RejectionQty" CssClass="textboxcss" Text='<%# Eval("RejectionQty") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CreatedTimeStamp">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="CreatedTs" CssClass="textboxcss" Text='<%# Eval("createdTs") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RejectionDate">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="RejectionDate" CssClass="textboxcss" Text='<%# Eval("RejectionDate") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RejectionShift">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="RejectionShift" CssClass="addtextcss" Text='<%# Eval("RejectionShift") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="mc" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="mc" runat="server" Text='<%# Eval("mc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MMM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtToDate]').datetimepicker({
            format: 'DD-MMM-YYYY HH:mm',
            useCurrent: false,
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $("[id$=txtFromDate]").on("dp.change", function (e) {
            $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
        });
        $("[id$=txtToDate]").on("dp.change", function (e) {
            $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() != undefined) {
                args.set_errorHandled(true);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="ContentFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
