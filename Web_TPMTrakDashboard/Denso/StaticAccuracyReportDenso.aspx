<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaticAccuracyReportDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.StaticAccuracyReportDenso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        #gridContainer table tr td {
            background-color: #fcfcfc;
            border: 1px solid silver;
            padding: 2px 5px;
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
                text-align: center;
                border: 1px solid silver !important;
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

        .txt-control {
            max-width: 150px;
            min-width: unset;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>

                    <td class="commanTd" style="vertical-align: middle;">Machine ID</td>
                    <td>
                        <asp:ListBox ID="ddlMultiMachineId" runat="server" SelectionMode="Multiple" CssClass="form-control cssclass"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Year</td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>

                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Month</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMonth" ClientIDMode="Static" CssClass="form-control">
                            <asp:ListItem Text="All"></asp:ListItem>
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
                            <td rowspan='<%# Eval("RowSpan") %>' style='display: <%# Eval("MachineDisplay") %>'>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Machine"></asp:Label>
                                <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("MachineID") %>'></asp:Label>
                            </td>
                            <td style="width: 50px;">
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Sr."></asp:Label>
                                <asp:Label runat="server" ID="lblSortOrder" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("SortOrder") %>'></asp:Label>
                            </td>
                            <td style="min-width: 150px;">
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Inspection Item"></asp:Label>
                                <asp:Label runat="server" ID="lblCheckpoint" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("Checkpoint") %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnCheckpointType" ClientIDMode="Static" Value='<%# Eval("CheckpointType") %>' />
                            </td>
                            <td style="padding: 0px; border: 0px;">
                                <asp:ListView runat="server" ID="lvChecksheetMonth" ItemPlaceholderID="itemplaceholder2" DataSource='<%# Eval("MonthList") %>'>
                                    <LayoutTemplate>
                                        <table style="width: 100%;" class="inner-table">
                                            <tr>
                                                <td runat="server" id="itemplaceholder2"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <%--<tr>--%>
                                        <td style="padding: 0px; border: 0px;">
                                            <asp:HiddenField runat="server" ID="hdnMonth" ClientIDMode="Static" Value='<%# Eval("Month") %>' />
                                            <asp:Label runat="server" ID="lblMonth" ClientIDMode="Static" Text='<%# Eval("DisplayMonth") %>'></asp:Label>
                                            <asp:ListView runat="server" ID="lvCheckpointValue" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder3" DataSource='<%# Eval("ValueList") %>' OnItemDataBound="lvCheckpointValue_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table class="inner-table">
                                                        <tr>
                                                            <td runat="server" id="itemplaceholder3"></td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <td class="td-actual-value" style="min-width: 60px;">
                                                        <asp:HiddenField runat="server" ID="hdnActualValue" ClientIDMode="Static" Value='<%# Eval("ActualValue") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnCheckpointType" ClientIDMode="Static" Value='<%# Eval("CheckpointType") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnWeekNo" ClientIDMode="Static" Value='<%# Eval("WeekNumber") %>' />

                                                        <asp:HiddenField runat="server" ID="hdnDate" ClientIDMode="Static" Value='<%# Eval("Date") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnControlEnabled" ClientIDMode="Static" Value='<%# Eval("ControlEnabled") %>' />
                                                        <div runat="server" visible='<%# Eval("HeaderVisibility") %>' style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblWeekNo" ClientIDMode="Static" Text='<%# Eval("WeekNumber") %>'></asp:Label>
                                                        </div>
                                                        <div runat="server" style='<%# "background-color:" + Eval("CheckpointTypeBackColor") + "; color:" + Eval("CheckpointTypeForeColor") %>' visible='<%# Eval("ContentVisibility") %>'>
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
                                        <%--</tr>--%>
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
        $(document).ready(function () {
            setControls();
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
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
            $('.td-actual-value').css('width', maxWidth);
            $('.td-actual-value').css('min-width', maxWidth);
            $('.td-actual-value').css('max-width', maxWidth);

            freezeColumnFromLeft('tblChecklist', 3);
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControls();
                $('[id$=ddlMultiMachineId]').multiselect({
                    includeSelectAllOption: true
                });
                $.unblockUI({});
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
