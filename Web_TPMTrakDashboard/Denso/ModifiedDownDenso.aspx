<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifiedDownDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.ModifiedDownDenso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <link href="../Scripts/Toastr/toastr.min.css" rel="stylesheet" />
    <script src="../Scripts/Toastr/toastr.min.js"></script>
    <style>
        .textboxcss {
            border: none;
            background-color: transparent;
            /*  font-style: italic;*/
            color: black;
            width: 230px;
        }

        .select {
            /*border-radius: 5px;*/
            -webkit-appearance: none;
        }

        #downdatagrid tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #downdatagrid tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .label {
            /*background-color:black;*/
            border-color: white;
            border-width: 3px;
            color: black;
            font-size: larger;
            font-family: 'Microsoft Sans Serif';
            /*font-style:oblique;*/
            font-weight: 600;
        }

        .splitTbl tr td {
            padding: 15px 3px;
            border-bottom: 1px solid silver;
        }

        .splitTbl .Span {
            font-weight: bold;
            color: #0b0b4d;
        }
        #downdatagrid > tbody > tr:last-child{
            bottom: 0px;
            position: sticky;
            background-color: aliceblue;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table class="table table-bordered" style="width: fit-content;">
                <tr>
                    <td class="commontd">Machine ID</td>
                    <td>
                        <asp:DropDownList ID="ddlMachine" CssClass="form-control" runat="server"></asp:DropDownList>
                    </td>
                    <td class="commontd">From Time
                    </td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" data-toggle="tooltip"
                                title="From Date !" placeholder="From Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commontd">To Time
                    </td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" data-toggle="tooltip"
                                title="To Date !" placeholder="To Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </td>
                    <td>&nbsp;  
                        <asp:Button runat="server" Text="View" class="btn btn-info" ID="btnview" OnClick="btnview_Click" OnClientClick="ShowLoader()"></asp:Button>&nbsp;
                    </td>
                    <td>&nbsp;
                        <asp:Button runat="server" Text="Save" class="btn btn-info" ID="btnSave" OnClick="btnSave_Click"></asp:Button>&nbsp;
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div class="col-lg-12">
                <div style="width: 100%; height: 80vh; overflow: auto" id="gridContainer">
                    <asp:HiddenField ID="hdnindex" runat="server" />
                    <asp:GridView ID="downdatagrid" runat="server" AutoGenerateColumns="false" Width="100%" AllowPaging="true" PageSize="100" ShowHeaderWhenEmpty="true" OnRowDataBound="downdatagrid_RowDataBound" OnPageIndexChanging="downdatagrid_PageIndexChanging" ClientIDMode="Static" CssClass="table table-bordered headerFixer alternate-table-style">
                        <Columns>
                            <asp:TemplateField HeaderText="ComponentName">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfdownsavecheck" runat="server" />

                                    <asp:Label ID="lblComponentName" runat="server" Text='<%# Eval("Component") %>'></asp:Label>
                                    <%--<asp:DropDownList ID="ComponentName" runat="server" CssClass="textboxcss select"></asp:DropDownList>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="OperationID" Style="width: 80px; text-align: left" CssClass="textboxcss" Text='<%# Eval("Operation") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperator" Text='<%# Eval("Operator") %>' ClientIDMode="Static"></asp:Label>
                                    <%--<asp:HiddenField ID="hdfoperator" runat="server" Value='<%# Eval("OperatorInterfaceid") %>' />--%>
                                    <%--<asp:DropDownList ID="OperatorName" runat="server" CssClass="textboxcss select"></asp:DropDownList>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Down Category">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlDownCategory" runat="server" CssClass="textboxcss select" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlDownCategory_SelectedIndexChanged"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Down Code">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfdowncode" runat="server" Value='<%# Eval("DownId") %>' ClientIDMode="Static" />
                                    <asp:DropDownList ID="DownCode" runat="server" CssClass="textboxcss select" ClientIDMode="Static"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From Time">
                                <ItemTemplate>
                                    <asp:Label ID="downFromTime" CssClass="splitfun" runat="server" Text='<%# Eval("TimeFrom") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Time">
                                <ItemTemplate>
                                    <asp:Label ID="downToTime" CssClass="splitfun" runat="server" Text='<%# Eval("TimeTo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WorkOrderNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="workorderno" Style="width: 70px; float: left" CssClass="textboxcss" Text='<%# Eval("WorkOrderNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="modal infoModal bajaj-info-modal" id="splitModel" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered " style="width: 1100px;">
                    <div class="modal-content modalThemeCss">
                        <div class="modal-header">
                            <h4 class="modal-title">Split Date</h4>
                            <i class="glyphicon glyphicon-remove modal-close-icon" style="float: right; color: white;" onclick="btnclose();"></i>
                        </div>
                        <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                            <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                <table style="width: 100%; margin: auto" class="splitTbl">
                                    <tr>
                                        <td>
                                            <span class="Span">Component-ID</span>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblComponentID" CssClass="" />
                                        </td>
                                        <td>
                                            <span class="Span">Operation No.</span>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblOperationNumber" CssClass="" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="Span">Down-ID</span>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblDownID" CssClass="" />
                                        </td>
                                        <td>
                                            <span class="Span">Operator</span>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblOperator" CssClass="" />
                                            <asp:Label runat="server" ID="lblIndex" Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="lblIDs" CssClass="textboxcss" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="Span">Down Time Start</span>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblDownTimeStart" CssClass="" />
                                        </td>
                                        <td>
                                            <span class="Span">Break At</span>
                                        </td>
                                        <td style="position: relative;" colspan="2">
                                            <asp:TextBox runat="server" ID="txtBreakDate" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control" Style="width: 120px; display: inline-block" />
                                            <asp:TextBox runat="server" ID="txtBreaktime" ClientIDMode="Static" TextMode="Time" AutoCompleteType="Disabled" CssClass="form-control" Style="width: 100px; display: inline-block" />
                                        </td>
                                        <td>
                                            <span class="Span">Down End Time</span>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblDownEndTime" CssClass="" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="bajaj-btn-style   bajaj-add-edit-btn-style" onclick="btnclose();">CANCEL</button>
                            <asp:Button runat="server" ID="btnSplit" Text="SPLIT" OnClick="btnSplit_Click" CssClass="bajaj-btn-style   bajaj-add-edit-btn-style" />
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>

    <script>
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            debugger;
            setControl();
            HideLoader();
        })
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        function ShowLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }

        function HideLoader() {
            $.unblockUI({});
        }
        function setControl() {
            $("#txtFromDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#txtToDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#txtBreakDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            //$("#txtFromDate").datetimepicker({
            //    format: 'DD-MM-YYYY',
            //    locale: 'en-US'
            //})
            //$("#txtToDate").datetimepicker({
            //    format: 'DD-MM-YYYY',
            //    locale: 'en-US'
            //})
            //$("#txtBreakDate").datetimepicker({
            //    format: 'DD-MM-YYYY HH:mm:ss',
            //    locale: 'en-US'
            //})
            //$("#txtBreaktime").datetimepicker({
            //    format: 'HH:mm:ss',
            //    locale: 'en-US'
            //})
        }
        $("[id$=downdatagrid]").on("click", "td", function () {

            //$(this).closest('tr').find('input[type=hidden]').val("updated");
            $(this).closest('tr').find("[id$=hdfdownsavecheck]").val("updated");
            $("[id$=downdatagrid] tr td").find('input').removeClass("form-control");
            $("[id$=downdatagrid] tr td").find('input').addClass("textboxcss");
            $("[id$=downdatagrid] tr td").find('select').addClass("select");
            $("[id$=downdatagrid] tr td").find('select').addClass("textboxcss");
            $("[id$=downdatagrid] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");
        });
        //function ShowLoader() {
        //    $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        //}

        //function HideLoader() {
        //    $.unblockUI({});
        //}

        //function getRow(row) {
        //    debugger;
        //    var rowIndex = row.rowIndex - 1; 
        //    $("[id$=hdnindex]").val() == rowIndex;
        //}
        $(document).on("click", "tr .splitfun", function (e) {

            debugger;
            //var rowIndex = $(this).closest("tr").index();
            $("[id$=hdnindex]").val(($(this).closest("tr").index()) - 1);
            $("[id$=lblComponentID]").text($(this).closest("tr").find("[id$=lblComponentName]").text());
            $("[id$=lblOperationNumber]").text($(this).closest("tr").find("[id$=OperationID]").val());
            $("[id$=lblDownTimeStart]").text($(this).closest("tr").find("[id$=downFromTime]").text());
            $("[id$=lblDownEndTime]").text($(this).closest("tr").find("[id$=downToTime]").text());
            $("[id$=lblDownID]").text($(this).closest("tr").find("[id$=hdfdowncode]").val());
            $("[id$=lblOperator]").text($(this).closest("tr").find("[id$=lblOperator]").text());
            $("[id$=txtBreakDate]").text($(this).closest("tr").find("[id$=downFromTime]").text());
            $('#splitModel').modal('show');

        });
        function btnclose() {
            $('#splitModel').modal('hide');
        }


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControl();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                HideLoader();
            });
            $("[id$=downdatagrid]").on("click", "td", function () {

                //$(this).closest('tr').find('input[type=hidden]').val("updated");
                $(this).closest('tr').find("[id$=hdfdownsavecheck]").val("updated");
                $("[id$=downdatagrid] tr td").find('input').removeClass("form-control");
                $("[id$=downdatagrid] tr td").find('input').addClass("textboxcss");
                $("[id$=downdatagrid] tr td").find('select').addClass("select");
                $("[id$=downdatagrid] tr td").find('select').addClass("textboxcss");
                $("[id$=downdatagrid] tr td").find('select').removeClass("form-control");

                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
                $(this).closest('td').find('select').addClass("form-control");
                $(this).closest('td').find('select').removeClass("textboxcss");
                $(this).closest('td').find('select').removeClass("select");
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {

                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
