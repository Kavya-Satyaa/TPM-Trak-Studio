<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyCheckpointTransactionDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.DailyCheckpointTransactionDenso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <style>
        #gridContainer table tr td {
            background-color: #fcfcfc;
            border: 1px solid silver;
            padding: 2px 5px;
        }

        .td-actual-value {
            text-align: center;
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

                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Year</td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>

                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Month</td>
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
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
            <div id="gridContainer" style="width: 100%; height: 80vh; overflow: auto">
                <asp:ListView runat="server" ID="lvChecklist" ItemPlaceholderID="itemplaceholder" OnItemDataBound="lvChecklist_ItemDataBound">
                    <LayoutTemplate>
                        <table class="outer-table" id="tblChecklist">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Sr."></asp:Label>
                                <asp:Label runat="server" ID="lblSlno" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("SlNo") %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnFrequency" ClientIDMode="Static" Value='<%# Eval("Frequency") %>' />
                                <asp:HiddenField runat="server" ID="hdnFormatNumber" ClientIDMode="Static" Value='<%# Eval("FormatNumber") %>' />
                                <asp:HiddenField runat="server" ID="hdnRevID" ClientIDMode="Static" Value='<%# Eval("RevID") %>' />
                                <asp:HiddenField runat="server" ID="hdnRevNo" ClientIDMode="Static" Value='<%# Eval("RevNo") %>' />
                                <asp:HiddenField runat="server" ID="hdnRevDate" ClientIDMode="Static" Value='<%# Eval("RevDate") %>' />
                                <asp:HiddenField runat="server" ID="hdnRevisedBy" ClientIDMode="Static" Value='<%# Eval("RevisedBy") %>' />
                                <asp:HiddenField runat="server" ID="hdnSortOrder" ClientIDMode="Static" Value='<%# Eval("SortOrder") %>' />
                                <asp:HiddenField runat="server" ID="hdnChecklistDesc" ClientIDMode="Static" Value='<%# Eval("ChecklistDesc") %>' />
                                <asp:HiddenField runat="server" ID="hdnChecklistType" ClientIDMode="Static" Value='<%# Eval("ChecklistType") %>' />
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Category"></asp:Label>
                                <asp:Label runat="server" ID="lblCategory" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("Category") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Inspection Item"></asp:Label>
                                <asp:Label runat="server" ID="lblChecklistID" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("ChecklistID") %>'></asp:Label>

                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Judgement Criteria"></asp:Label>
                                <asp:Label runat="server" ID="lblJudgementCriteria" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("JudgementCriteria") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Method"></asp:Label>
                                <asp:Label runat="server" ID="lblMethod" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("Method") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Cycle"></asp:Label>
                                <asp:Label runat="server" ID="lblCycle" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("Cycle") %>'></asp:Label>
                            </td>
                            <%--  <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Shift"></asp:Label>
                                <asp:Label runat="server" ID="lblShift" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("ShiftName") %>'></asp:Label>
                            </td>--%>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Person In Charge"></asp:Label>
                                <asp:Label runat="server" ID="lblPersonInCharge" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("PersonInCharge") %>'></asp:Label>
                            </td>
                            <td style="padding: 0px; border: 0px;">
                                <asp:ListView runat="server" ID="lvChecklistDayShift" ItemPlaceholderID="itemplaceholder2" DataSource='<%# Eval("ShiftDayList") %>'>
                                    <LayoutTemplate>
                                        <table style="width: 100%;" class="inner-table">
                                            <tr runat="server" id="itemplaceholder2"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
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
                                                        <td class="td-actual-value" runat="server" visible='<%# Eval("ShiftColumnVisibility") %>'>
                                                            <asp:Label runat="server" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' Text="Shift"></asp:Label>
                                                            <asp:Label runat="server" ID="lblShift" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>' Text='<%# Eval("ShiftName") %>'></asp:Label>
                                                        </td>
                                                        <td class="td-actual-value" style='<%# "background-color:" + Eval("CheckpointTypeBackColor") + "; color:" + Eval("CheckpointTypeForeColor") %>'>
                                                            <asp:HiddenField runat="server" ID="hdnActualValue" ClientIDMode="Static" Value='<%# Eval("ActualValue") %>' />
                                                            <asp:HiddenField runat="server" ID="hdnCheckpointType" ClientIDMode="Static" Value='<%# Eval("CheckpointType") %>' />
                                                            <asp:HiddenField runat="server" ID="hdnDay" ClientIDMode="Static" Value='<%# Eval("Day") %>' />
                                                            <asp:HiddenField runat="server" ID="hdnDate" ClientIDMode="Static" Value='<%# Eval("Date") %>' />
                                                            <asp:HiddenField runat="server" ID="hdnControlEnabled" ClientIDMode="Static" Value='<%# Eval("ControlEnabled") %>' />
                                                            <div runat="server" visible='<%# Eval("HeaderVisibility") %>' style="text-align: center;">
                                                                <asp:Label runat="server" ID="lblDay" ClientIDMode="Static" Text='<%# Eval("Day") %>'></asp:Label>
                                                            </div>
                                                            <div runat="server" visible='<%# Eval("ContentVisibility") %>'>
                                                                <div runat="server" visible='<%# Eval("CheckpointType").ToString()=="Checkbox"?true:false %>'>
                                                                    <asp:CheckBox runat="server" ID="chkValue" onclick='<%# Eval("ControlEnabled").ToString()=="True"?"return true;":"return false;" %>' />
                                                                </div>
                                                                <div runat="server" visible='<%# Eval("CheckpointType").ToString()=="Text"?true:false %>'>
                                                                    <asp:TextBox runat="server" ID="txtValue" ClientIDMode="Static" Text='<%# Eval("ActualValue") %>' CssClass="form-control" Enabled='<%# Eval("ControlEnabled") %>'></asp:TextBox>
                                                                </div>
                                                                <div runat="server" style='<%# "background-color:" + Eval("CheckpointTypeBackColor") + "; color:" + Eval("CheckpointTypeForeColor") %>' visible='<%# Eval("CheckpointType").ToString() == "NG-OK"|| Eval("CheckpointType").ToString() == "OK-NG" || Eval("CheckpointType").ToString() == "Yes-No" %>'>
                                                                    <asp:DropDownList runat="server" ID="ddlValue" ClientIDMode="Static" CssClass="form-control" Style='<%# "background-color:" + Eval("CheckpointTypeBackColor") + ";min-width:100px; color:" + Eval("CheckpointTypeForeColor") %>' Enabled='<%# Eval("ControlEnabled") %>'>
                                                                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                                                        <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                                                        <asp:ListItem Text="NG" Value="NG"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div runat="server" visible='<%# Eval("CheckpointType").ToString()=="Image"?true:false %>'>
                                                                    <asp:FileUpload runat="server" ID="imgFileUpload" ClientIDMode="Static" CssClass="form-control" onchange="FUChange(this);" Enabled='<%# Eval("ControlEnabled") %>' />
                                                                    <asp:LinkButton runat="server" ID="lnkImage" ClientIDMode="Static" Text='<%# Eval("FileName") %>' OnClientClick="return showImage(this);"></asp:LinkButton>
                                                                    <asp:HiddenField ID="hdnFileExistingFileInBase64" runat="server" ClientIDMode="Static" Value='<%# Eval("FileInBase64") %>' />
                                                                    <asp:HiddenField ID="hdnExistingFileName" runat="server" ClientIDMode="Static" Value='<%# Eval("FileName") %>' />
                                                                    <asp:HiddenField ID="hdnFileName" runat="server" ClientIDMode="Static" Value='<%# Eval("FileName") %>' />
                                                                    <asp:HiddenField ID="hdnFileInBase64" runat="server" ClientIDMode="Static" Value='<%# Eval("FileInBase64") %>' />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
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
        async function FUChange(ctrl) {
            const filePathsPromises = [];
            const fileName = [];
            var fileExtension = ['png', 'jpg', 'jpeg'];

            for (var i = 0; i < $(ctrl).get(0).files.length; ++i) {
                filePathsPromises.push(ToBase64($(ctrl).get(0).files[i]));
                fileName.push($(ctrl).get(0).files[i].name);

                if ($.inArray($(ctrl).get(0).files[i].name.split('.').pop().toLowerCase(), fileExtension) == -1) {
                    openWarningModal_1("Allowed file types are png, jpg and jpeg only!", "");
                    $(ctrl).val("");
                    return;
                }
            }

            debugger;
            const filePaths = await Promise.all(filePathsPromises);
            var mappedFiles = filePaths.map((base64File) => ({ file: base64File }));
            let document = "", documentName = "";
            if (mappedFiles.length > 0) {
                document = mappedFiles[0].file;
                documentName = fileName[0];
            }
            else {
                document = $(ctrl).closest('td').find("#hdnFileExistingFileInBase64").val();
                documentName = $(ctrl).closest('td').find("#lnkImage").text();
            }
            $(ctrl).closest('td').find("#hdnFileInBase64").val(document);
            $(ctrl).closest('td').find("#hdnFileName").val(documentName);
        }

        function ToBase64(file) {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result);
                reader.onerror = error => reject(error);
            });
        };
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

            freezeColumnFromLeft('tblChecklist', 7);
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
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                bigDiv.scrollLeft = $('[id*=hdnScrollLeftPos]').val();
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
