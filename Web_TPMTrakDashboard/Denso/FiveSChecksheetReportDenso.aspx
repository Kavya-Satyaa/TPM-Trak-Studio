<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FiveSChecksheetReportDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.FiveSChecksheetReportDenso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <style>
        #gridContainer table tr td {
            background-color: #fcfcfc;
            border: 1px solid silver;
            padding: 5px 5px;
        }

        .td-actual-value {
            text-align:center;
        }

        .outer-table {
            height: 1px;
        }

            .outer-table > tbody > tr:first-child td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                position: sticky;
                top: 0px;
                z-index: 1;
            }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnScrollLeftPos" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>

                    <td class="commanTd" style="vertical-align: middle;">Machine ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Year</td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>

                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Month</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMonth" ClientIDMode="Static" CssClass="form-control">
                            <asp:ListItem Value="01" Text="Jan"></asp:ListItem>
                            <asp:ListItem Value="02" Text="Feb"></asp:ListItem>
                            <asp:ListItem Value="03" Text="Mar"></asp:ListItem>
                            <asp:ListItem Value="04" Text="Apr"></asp:ListItem>
                            <asp:ListItem Value="05" Text="May"></asp:ListItem>
                            <asp:ListItem Value="06" Text="Jun"></asp:ListItem>
                            <asp:ListItem Value="07" Text="Jul"></asp:ListItem>
                            <asp:ListItem Value="08" Text="Aug"></asp:ListItem>
                            <asp:ListItem Value="09" Text="Sep"></asp:ListItem>
                            <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                            <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                            <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle; display: none">Week No.</td>
                    <td style="display: none">
                        <asp:DropDownList runat="server" ID="ddlWeekNo" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" OnClientClick="return showLoader();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
            <div id="gridContainer" style="width: 100%; height: 80vh; overflow: auto">
                <asp:ListView runat="server" ID="lvChecklist" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="outer-table" id="tblChecklist">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style='display: <%# Eval("RowDisplay") %>' rowspan='<%# Eval("RowSpan") %>'>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Sr."></asp:Label>
                                <asp:Label runat="server" ID="lblSlno" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("SlNo") %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnCycle" ClientIDMode="Static" Value='<%# Eval("Cycle") %>' />
                                <asp:HiddenField runat="server" ID="hdnSortOrder" ClientIDMode="Static" Value='<%# Eval("SortOrder") %>' />
                                <asp:HiddenField runat="server" ID="hdnChecklistType" ClientIDMode="Static" Value='<%# Eval("CheckpointType") %>' />
                                <asp:HiddenField runat="server" ID="hdnShiftID" ClientIDMode="Static" Value='<%# Eval("Shift") %>' />
                            </td>
                            <td style='min-width: 250px; display: <%# Eval("RowDisplay") %>' rowspan='<%# Eval("RowSpan") %>'>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Check Item"></asp:Label>
                                <asp:Label runat="server" ID="lblChecklistID" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("ChecklistID") %>'></asp:Label>
                            </td>
                            <td style='display: <%# Eval("RowDisplay") %>; white-space: nowrap' rowspan='<%# Eval("RowSpan") %>'>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Cycle"></asp:Label>
                                <asp:Label runat="server" ID="lblCycle" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("CycleTimeDisplay") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Shift"></asp:Label>
                                <asp:Label runat="server" ID="lblShift" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("ShiftName") %>'></asp:Label>
                            </td>
                            <td style="padding: 0px; border: 0px;">
                                <asp:HiddenField runat="server" ID="hdnShift" ClientIDMode="Static" Value='<%# Eval("Shift") %>' />
                                <asp:ListView runat="server" ID="lvCheckpointValue" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder3" DataSource='<%# Eval("ValueList") %>' OnItemDataBound="lvCheckpointValue_ItemDataBound">
                                    <LayoutTemplate>
                                        <table class="inner-table">
                                            <tr>
                                                <td runat="server" id="itemplaceholder3"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <td class="td-actual-value">
                                            <asp:HiddenField runat="server" ID="hdnActualValue" ClientIDMode="Static" Value='<%# Eval("ActualValue") %>' />
                                            <asp:HiddenField runat="server" ID="hdnCheckpointType" ClientIDMode="Static" Value='<%# Eval("CheckpointType") %>' />
                                            <asp:HiddenField runat="server" ID="hdnDate" ClientIDMode="Static" Value='<%# Eval("Date") %>' />
                                            <asp:HiddenField runat="server" ID="hdnControlEnabled" ClientIDMode="Static" Value='<%# Eval("ControlEnabled") %>' />
                                            <div runat="server" visible='<%# Eval("HeaderVisibility") %>' style="text-align: center;">
                                                <asp:Label runat="server" ID="lblDay" ClientIDMode="Static" Text='<%# Eval("Day") %>'></asp:Label>
                                            </div>
                                            <div runat="server" style='<%# "background-color:" + Eval("CheckpointTypeBackColor") + "; color:" + Eval("CheckpointTypeForeColor") %>' visible='<%# Eval("ContentVisibility") %>'>
                                                <%--<div runat="server" visible='<%# Eval("CheckpointType").ToString()=="Checkbox"?true:false %>'>
                                                    <asp:CheckBox runat="server" ID="chkValue" onclick='<%# Eval("ControlEnabled").ToString()=="True"?"return true;":"return false;" %>' />
                                                </div>
                                                <div runat="server" visible='<%# Eval("CheckpointType").ToString()=="Text"?true:false %>'>
                                                    <asp:TextBox runat="server" ID="txtValue" ClientIDMode="Static" Text='<%# Eval("ActualValue") %>' CssClass="form-control" Enabled='<%# Eval("ControlEnabled") %>'></asp:TextBox>
                                                </div>
                                                <div runat="server" visible='<%# Eval("CheckpointType").ToString()=="Yes-No"?true:false %>'>
                                                    <asp:DropDownList runat="server" ID="ddlValue" ClientIDMode="Static" CssClass="form-control" Style="width: auto" Enabled='<%# Eval("ControlEnabled") %>'>
                                                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>--%>
                                                <div runat="server" visible='<%# Eval("CheckpointType").ToString()!="Image"?true:false %>'>
                                                    <asp:Label runat="server" ID="lblValue" ClientIDMode="Static" Text='<%# Eval("ActualValue") %>'></asp:Label>
                                                </div>
                                                <div runat="server" visible='<%# Eval("CheckpointType").ToString()=="Image"?true:false %>'>
                                                    <asp:LinkButton runat="server" ID="lnkImage" ClientIDMode="Static" Text='<%# Eval("FileName") %>' OnClientClick="return showImage(this);"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnFileExistingFileInBase64" runat="server" ClientIDMode="Static" Value='<%# Eval("FileInBase64") %>' />
                                                    <asp:HiddenField ID="hdnExistingFileName" runat="server" ClientIDMode="Static" Value='<%# Eval("FileName") %>' />
                                                </div>
                                            </div>
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div class="modal infoModal bajaj-info-modal" id="imageDisplayModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 70%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Image</h4>
                            <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                        </div>
                        <div class="modal-body">
                            <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                <div style="height: 70vh">
                                    <img id="imgLarge" src="" style="width: 100%; height: 100%" />
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" value="Close" class="infoModal-btn" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            setControls();
            $.unblockUI({});
        });
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        };

        function showImage(ctrl) {
            $('#imageDisplayModal').modal('show');
            $('#imgLarge').attr('src', "data:image/png;base64," + $(ctrl).closest('td').find('#hdnFileExistingFileInBase64').val());
            return false;
        }
        function setControls() {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            let maxWidth = Math.max.apply(null, $('.td-actual-value').map(function () {
                return $(this).outerWidth(true);
            }).get());
            if (maxWidth >= 150) {
                maxWidth = 150;
            }
            $('.td-actual-value').css('width', maxWidth);
            $('.td-actual-value').css('min-width', maxWidth);
            $('.td-actual-value').css('max-width', maxWidth);

            freezeColumnFromLeft('tblChecklist', 4);
        }
        //bigDiv.onscroll = function () {
        //    $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        //    $('[id*=hdnScrollLeftPos]').val(bigDiv.scrollLeft);
        //}
        //window.onload = function () {
        //    bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        //    bigDiv.scrollLeft = $('[id*=hdnScrollLeftPos]').val();
        //}
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                //bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                //bigDiv.scrollLeft = $('[id*=hdnScrollLeftPos]').val();
                $.unblockUI({});
            });
            //bigDiv.onscroll = function () {
            //    $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            //    $('[id*=hdnScrollLeftPos]').val(bigDiv.scrollLeft);
            //}
            //window.onload = function () {
            //    bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            //    bigDiv.scrollLeft = $('[id*=hdnScrollLeftPos]').val();
            //}
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
