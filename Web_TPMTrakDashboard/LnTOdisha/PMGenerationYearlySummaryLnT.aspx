<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMGenerationYearlySummaryLnT.aspx.cs" Inherits="Web_TPMTrakDashboard.LnTOdisha.PMGenerationYearlySummaryLnT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <style>
        .tblSettings {
            width: 100%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 5px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: medium;
                /*box-shadow: 2px 2px 2px black;*/
            }

        .outer-table tr td {
            background-color: #fcfcfc;
            border: 1px solid silver;
            padding: 2px 5px;
        }

        .outer-table {
            height: 1px;
        }

            .outer-table > tbody > tr:first-child td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                text-align: center;
                height: 40px;
            }

                .outer-table > tbody > tr:first-child td > .inner-table > tbody > tr > td {
                    border: 1px solid silver !important;
                }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }

        .month-td {
            width: 150px;
        }

        .modal-tbl tr td {
            padding: 5px;
        }

        .btnClass {
            background-color: #1c701c;
            color: white;
            border: 1px solid black;
        }
    </style>
    <table id="tblFilter" class="" style="width: 45%;">
        <%--tblSettings--%>
        <tr>
            <td class="commanTd" style="vertical-align: middle;">Year</td>
            <td>
                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>

            </td>
            <td class="commanTd" style="vertical-align: middle;">Month</td>
            <td>
                <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" Style="width: 70px; display: inline;"></asp:TextBox>
            </td>
            <td class="commanTd" style="vertical-align: middle;">Machine</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control" Style="width: 180px;"></asp:DropDownList>
            </td>
            <td>
                <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" />
            </td>
            <td>
                <asp:Button runat="server" ID="btnSave" Text="Save" Visible="false" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </td>
            <td colspan="2">
                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btnClass" OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="height: 80vh; overflow: auto; margin-top: 10px;">
                <asp:ListView runat="server" ID="lvPMData" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="outer-table">
                            <tr runat="server" id="itemplaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="machine-tr" style="height: 60px;">
                            <td style="text-align: center;">
                                <asp:Label runat="server" ClientIDMode="Static" Text="Slno" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <asp:Label runat="server" ID="lblSlno" ClientIDMode="Static" Text='<%# Eval("Slno") %>' Visible='<%# Eval("ContentVisible") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text="Machine" Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("Machine") %>' Visible='<%# Eval("ContentVisible") %>'></asp:Label>
                            </td>
                            <td style="padding: 0px; border: 0;">
                                <asp:ListView runat="server" ID="lvMonthData" ItemPlaceholderID="itemplaceholder2" DataSource='<%# Eval("MonthList") %>'>
                                    <LayoutTemplate>
                                        <table class="inner-table month-tbl">
                                            <tr>
                                                <td runat="server" id="itemplaceholder2"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <td class="month-td" style="padding: 0px; border: 0; border-collapse: collapse; min-width: 120px; max-width: 120px; text-align: center;">
                                            <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                            <asp:HiddenField runat="server" ID="hdnMachineID" ClientIDMode="Static" Value='<%# Eval("Machine") %>' />
                                            <%--<asp:HiddenField runat="server" ID="hdnPlanType" ClientIDMode="Static" Value='<%# Eval("PlanType") %>' />--%>
                                            <asp:HiddenField runat="server" ID="hdnPlanDate" ClientIDMode="Static" Value='<%# Eval("PlanDate") %>' />
                                            <asp:HiddenField runat="server" ID="hdnMonthName" ClientIDMode="Static" Value='<%# Eval("MonthName") %>' />
                                            <asp:HiddenField runat="server" ID="hdnMonthNo" ClientIDMode="Static" Value='<%# Eval("MonthNo") %>' />
                                            <asp:HiddenField runat="server" ID="hdnYear" ClientIDMode="Static" Value='<%# Eval("Year") %>' />
                                            <asp:Label runat="server" ClientIDMode="Static" Text='<%#Eval("MonthName") %>' Visible='<%# Eval("HeaderVisible") %>'></asp:Label>
                                            <table runat="server" visible='<%# Eval("ContentVisible") %>' class="inner-table">
                                                <tr>
                                                    <td onclick="openDateModal(this);" class='<%# Eval("CssClass") %>' style="padding: 5px;">
                                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblPlanStatus" Text='<%#Eval("PlanStatus") %>'></asp:Label><br />
                                                        <asp:Label runat="server" ID="lblPlanDate" ClientIDMode="Static" Text='<%# Eval("PlanDate") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="modal infoModal" id="planDateModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 500px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="newEditModalTitle" runat="server">Select Plan</h4>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                        <asp:HiddenField runat="server" ID="hdnRowIndex" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnColIndex" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnMachineID" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnMonthName" ClientIDMode="Static" />
                        <table style="width: 100%; margin: auto" class="modal-tbl" clientidmode="static">
                            <tr>
                                <td style="padding-left: 20px">Plan Type</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlanType" CssClass="form-control" onchange="ddlPlanTypeChange();" ClientIDMode="Static">
                                        <asp:ListItem Text="Under Plan" Value="UnderPlan"></asp:ListItem>
                                        <asp:ListItem Text="No Plan" Value="NoPlan"></asp:ListItem>
                                        <asp:ListItem Text="Not Released for PM" Value="NotReleasedForPM"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 20px">Plan Date</td>
                                <td>
                                    <div class="input-group" runat="server">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtPlanDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnPlanSave" ClientIDMode="Static" Text="OK" CssClass="btn btn-primary btn-style" OnClick="btnSave_Click" OnClientClick="return savePlanData();" /><%----%>
                    <button type="button" class="btn btn-primary btn-style cancel-btn" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            setDateTimePicker();
            freezeColumnFromLeft('outer-table', '2');
        });
        function openDateModal(ctrl) {
            debugger;
            var maxDate = "";
            $('#hdnColIndex').val($(ctrl).closest('.month-td').index());
            $('#hdnRowIndex').val($(ctrl).closest('.machine-tr').index());
            ddlPlanTypeChange();

            $("[id$=txtPlanDate]").data("DateTimePicker").maxDate("01-01-9999");
            $("[id$=txtPlanDate]").data("DateTimePicker").minDate("01-01-1000");
            var monthNo = $(ctrl).closest('.month-td').find('#hdnMonthNo').val();
            monthNo = monthNo.length == 1 ? "0" + monthNo : monthNo;
            var year = $(ctrl).closest('.month-td').find('#hdnYear').val();
            var minDate = "01-" + monthNo + "-" + year;

            maxDate = new Date(year, monthNo, 0);
            $("[id$=txtPlanDate]").data("DateTimePicker").maxDate(maxDate);
            $("[id$=txtPlanDate]").data("DateTimePicker").minDate(minDate);
            debugger;
            $("[id$=txtPlanDate]").val($(ctrl).closest('.month-td').find('#lblPlanDate').text().replace('(', '').replace(')', ''));
            if ($(ctrl).closest('.month-td').find('#lblPlanStatus').text().toUpperCase() != "COMPLETED") {
                if ($(ctrl).closest('.month-td').find('.inner-table tr td').hasClass('PlanAllowed')) {
                    debugger;
                    $(ctrl).closest('.month-td').find('#hdnUpdate').val("Update");
                    $("#ddlPlanType").val($(ctrl).closest('.month-td').find('#lblPlanStatus').text().replaceAll(' ', ''));
                    if ($("#txtPlanDate").val() == '' || $("#txtPlanDate").val() == undefined)
                        $("#txtPlanDate").val(minDate);
                    $('#txtPlanDate').removeAttr("disabled");
                    $('#planDateModal').modal("show");
                }
                else {
                    debugger;
                    toasterWarningMsg('Plan is not allowed for previous months.', '');
                }
            }
            else
                toasterWarningMsg('PM Activity Is Completed for that month.', '');
            $("#hdnMachineID").val($(ctrl).closest(".machine-tr").find("#lblMachine").text());
        }
        function savePlanData() {
            debugger;
            var colIndex = $('#hdnColIndex').val();
            //var rowIndex = $('#hdnRowIndex').val();
            var monthName = $($($(".outer-table").find(".machine-tr")[0]).find('.month-td')[colIndex]).find("#hdnMonthName").val();
            $("#hdnMonthName").val(monthName);
            //var td = $($(".outer-table").find(".machine-tr")[rowIndex]).find('.month-td')[colIndex];
            //$(td).find('#hdnPlanDate').val($('#txtPlanDate').val());
            //$(td).find('#hdnPlanType').val($('#ddlPlanType').val());
            //$(td).find('#lblPlanDate').html($('#txtPlanDate').val());
            //$(td).find('#lblPlanStatus').html($('#ddlPlanType').val());
            $('#planDateModal').modal("hide");
            return true;
        }
        function ddlPlanTypeChange() {
            if ($('#ddlPlanType').val() == "NoPlan") {
                $('#txtPlanDate').val("");
                $('#txtPlanDate').attr("disabled", "disabled");
            }
            else {
                $('#txtPlanDate').removeAttr("disabled");
            }
        }

        function closePopUp() {
            $('#planDateModal').modal('hide');
            return true;
        }
        function setDateTimePicker() {
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
            $('[id$=txtPlanDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();
                freezeColumnFromLeft('outer-table', '2');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
