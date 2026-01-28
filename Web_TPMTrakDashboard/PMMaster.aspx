<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.PMMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .table-style tr td {
            color: white;
            padding: 4px;
        }

        /* .table-style tr td a {
                color: #0abaff;
            }*/

        .table-style tr th {
            background-color: #5391CA;
            color: white;
            font-weight: bold;
            padding: 5px;
        }

        .textboxcss {
            border: none;
            background-color: transparent;
            /*font-style: italic;*/
            /* color: white;*/
        }

        .select {
            /*border-radius: 5px;*/
            -webkit-appearance: none;
        }

        select option {
            color: black;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }
    </style>
    <div>
        <div class="ui segment">
            <table style="display: inline-block;" class="table-style">
                <tr>
                    <td style="text-align: center; vertical-align: central; align-content: center;">Machine ID:
                            </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlMachineId" CssClass="form-control" Width="140px" />
                    </td>
                    <td style="text-align: center; vertical-align: central; align-content: center;">Frequency:
                            </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Width="140px" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="btn btn-info" Text="View" Width="80" />

                    </td>
                </tr>
            </table>
        </div>

        <div style="height: 80vh; overflow: auto" id="container">
            <asp:GridView ID="gvActivityDetails" runat="server" AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found." ShowHeaderWhenEmpty="true" ShowHeader="true" ShowFooter="true" ClientIDMode="Static" CssClass="table table-bordered table-hover headerFixer bajaj-table-style" OnRowDataBound="gvActivityDetails_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="SL No." AccessibleHeaderText="SlNo">
                        <ItemTemplate>
                            <asp:Label ID="lblSlno" runat="server" Text='<%# Eval("SerialNumber") %>' ClientIDMode="Static"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Activity" AccessibleHeaderText="Activity">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtActvity" Style="text-align: left; width: 96%; display: inline-block" CssClass="textboxcss" Text='<%# Eval("Activity") %>' AutoCompleteType="Disabled"></asp:TextBox>
                            <%-- <asp:LinkButton runat="server" ID="lbUploadedFile" ClientIDMode="Static" CssClass="glyphicon glyphicon-save" OnClick="lbUploadedFile_Click" ToolTip='<%# Eval("FileName") %>' ></asp:LinkButton>--%>
                            <asp:HiddenField runat="server" ID="hfIsActivityHasFile" Value='<%# Eval("ActivityHasFile") %>' />
                            <asp:HiddenField runat="server" ID="hfActivityID" Value='<%# Eval("ActivityID") %>' />
                            <asp:HiddenField runat="server" ID="hfMachineID" Value='<%# Eval("MachineID") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox runat="server" ID="txtActvity" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Frequency" AccessibleHeaderText="Frequency">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hdnFrequency" ClientIDMode="Static" Value='<%# Eval("Frequency") %>' />
                            <asp:HiddenField runat="server" ID="hdnFrequencyID" ClientIDMode="Static" Value='<%# Eval("FrequencyID") %>' />
                            <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="textboxcss select" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChangedFooter"  AutoPostBack="true"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Shift" AccessibleHeaderText="Shift">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hdnShifts" ClientIDMode="Static" Value='<%# Eval("Shifts") %>' />
                            <asp:ListBox ID="lbShift" runat="server" SelectionMode="Multiple" CssClass="textboxcss select listbox-control"></asp:ListBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:ListBox ID="lbShift" runat="server" SelectionMode="Multiple" CssClass="form-control listbox-control"></asp:ListBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Category" AccessibleHeaderText="Category">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hdnCategory" ClientIDMode="Static" Value='<%# Eval("Category") %>' />
                            <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" CssClass="textboxcss select">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" CssClass="form-control">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Criteria" AccessibleHeaderText="Criteria">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtCriteria" Style="text-align: left; width: 96%; display: inline-block" CssClass="textboxcss" Text='<%# Eval("Criteria") %>' AutoCompleteType="Disabled"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox runat="server" ID="txtCriteria" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Upload File" AccessibleHeaderText="UploadFile">
                        <ItemTemplate>
                            <%--   <asp:HiddenField runat="server" ID="hdnFileID" Value='<%# Eval("SOPFileID") %>' />
                            <asp:HiddenField runat="server" ID="hdnFileName" Value='<%# Eval("SOPFileName") %>' />--%>
                            <asp:FileUpload runat="server" ID="fileUpload" CssClass="textboxcss" ClientIDMode="Static" Style="display: inline-block" onchange="fileValidation(this);" Width="300px" />
                            <%-- <asp:LinkButton runat="server" ID="lnkSOPFile" Text='<%# Eval("SOPFileName") %>' ClientIDMode="Static" OnClientClick="return showSOPFiles(this);"></asp:LinkButton>--%>
                            <%--<asp:Label runat="server" ID="lblFileName"  Text='<%# Eval("FileName") %>'></asp:Label>--%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:FileUpload runat="server" ID="fileUpload" CssClass="form-control" ClientIDMode="Static" Style="display: inline-block" onchange="fileValidation(this);" Width="300px" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File" AccessibleHeaderText="UploadFile">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lbUploadedFile1" ClientIDMode="Static" Text='<%# Eval("FileName") %>' OnClick="lbUploadedFile_Click" ToolTip='<%# Eval("FileName") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkDeleteSelection" />
                        </ItemTemplate>
                        <%-- <FooterTemplate>
                            <asp:Button runat="server" ID="btnModelLvlAdd" ClientIDMode="Static" Text="Add" CssClass="Btns" OnClientClick="return insertMTBLvlPMChecklist(this);" OnClick="btnModelLvlAdd_Click" />
                        </FooterTemplate>--%>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#5391CA" />
            </asp:GridView>
        </div>
        <div style="margin-top: 5px; text-align: right;">

            <asp:Button runat="server" ID="btnNew" OnClick="btnNew_Click" CssClass="btn btn-info" Text="New" Width="80" />
            <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" CssClass="btn btn-info" Text="Cancel" Width="80" />
            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-info" Text="Save" Width="80" />
            <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" OnClientClick="if(!deleteConfirmation()){return false};" CssClass="btn btn-info" Text="Delete" Width="80" />
        </div>
    </div>
    <div class="modal infoModal" id="FilesModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog modal-dialog-centered" style="width: 90%;">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title">Document</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right"></i>
                    <%--position: relative; top: -22px;--%>
                </div>
                <div class="modal-body">
                    <div style="height: 70vh">
                        <iframe id="iframeDocument" style="width: 100%; height: 100%" runat="server"></iframe>
                    </div>

                </div>
                <div class="modal-footer">

                    <input type="button" value="Close" class="btn btn-info" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $('.listbox-control').multiselect({
                includeSelectAllOption: true
            });
            var isShiftEnabled = false;            var options = $("[id$=ddlFrequency] option");            for (var i = 0; i < options.length; i++) {                if ($(options)[i].text == "Shift") {                    isShiftEnabled = true;                    break;                }            }            if (isShiftEnabled == false) {                $('#gvActivityDetails tr td:nth-child(4)').hide();                $('#gvActivityDetails tr th:nth-child(4)').hide();            }
        }
        function deleteConfirmation() {
            let isConfirmed = confirm("Are you sure, you want to delete selected rows?");
            return isConfirmed;
        }
        $("[id$=gvActivityDetails]").on("click", "td", function () {
            //$(this).closest('tr').find('#hdnUpdate').val("updated");
            var tblID = $(this).closest('table').prop('id');
            var tbl = document.getElementById(tblID);
            var tblRowCount = tbl.rows.length - 1;
            var currentTR = $(this).closest('tr');
            var currentClickRowIndex = $(currentTR).index();
            //if (tblRowCount == currentClickRowIndex) {
            //    return;
            //}
            //$("[id$=gvActivityDetails] tr:not(:last-child) td").find('input').removeClass("form-control");
            //$("[id$=gvActivityDetails] tr:not(:last-child) td").find('input').addClass("textboxcss");
            //$("[id$=gvActivityDetails] tr:not(:last-child) td").find('select').addClass("select");
            //$("[id$=gvActivityDetails] tr:not(:last-child) td").find('select').addClass("textboxcss");
            //$("[id$=gvActivityDetails] tr:not(:last-child) td").find('select').removeClass("form-control");
            $("[id$=gvActivityDetails] tr td").find('input').removeClass("form-control");
            $("[id$=gvActivityDetails] tr td").find('input').addClass("textboxcss");
            $("[id$=gvActivityDetails] tr td").find('select').addClass("select");
            $("[id$=gvActivityDetails] tr td").find('select').addClass("textboxcss");
            $("[id$=gvActivityDetails] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");

            $("[id$=gvActivityDetails] tr td").find('input[type="checkbox"]').removeClass("form-control");
        });
        function openFileUploadModal() {
            $("#FilesModal").modal('show');
        }
        function setScrollToBottotm() {
            window.onload = function () {                // $('#container').animate({ scrollTop: $(document).height()}, 10);                $("#container").animate({ scrollTop: $("#container")[0].scrollHeight }, 1000);                //$('#container').scrollTop($('#container').height()); 
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
