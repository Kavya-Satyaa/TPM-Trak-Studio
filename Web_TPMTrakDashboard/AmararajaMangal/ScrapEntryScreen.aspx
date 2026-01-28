<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScrapEntryScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.AmararajaMangal.ScrapEntryScreen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>


    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        #tblScrapEntry tr td {
            background-color: #FFFFFF;
            color: black;
            vertical-align:middle;
        }

        .showRowSpan {
            vertical-align: middle !important;
        }

        .hideRowSpan {
            display: none;
        }
    </style>
    <div class="container-fluid" style="">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>


                <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                    <tr>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Date</td>
                        <td class="input-group" style="min-width: 150px; border: 0">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Shift</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                         <td class="commanTd" style="width: 100px; vertical-align: middle;">Plant</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine</td>
                        <td>
                            <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple" Width="150" ClientIDMode="Static"></asp:ListBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" OnClientClick="return saveValidation();" />
                            <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click"/>
                        </td>
                    </tr>
                </table>
                <div style="height: 80vh; overflow: auto;">
                    <asp:ListView runat="server" ID="lvScrapEntry" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table id="tblScrapEntry" class="table table-bordered  headerFixer" style="width: 100%">
                                <tr style="height:38px;">
                                    <th colspan="6" style="text-align: center">PRODUCTION DATA</th>
                                    <th colspan="6" style="text-align: center">SCRAP ENTRY BY MACHINE</th>
                                </tr>
                                <tr style="position:sticky;top:36px;">
                                    <th>Machine</th>
                                    <th>Part ID</th>
                                    <th>Total parts produced</th>
                                    <th>No. of Accepted Parts</th>
                                    <th>Unit Weight (in kgs)</th>
                                    <th>Weight of Accepted parts (in Kgs)</th>
                                    <th>Total Weight of Accepted parts (in Kgs)</th>
                                    <th># of Rejections</th>
                                    <th>Rejected Parts (Weight in Kgs)</th>
                                    <th>Design Scrap (Weight in Kgs)</th>
                                    <th>Layout Scrap (Weight in kgs)</th>
                                    <th>Total Scrap (weight in kgs)</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="white-space: nowrap" rowspan='<%# Eval("RowSpan") %>' class='<%# Eval("RowSpanClass") %>'>
                                    <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                    <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" ID="lblParts" ClientIDMode="Static" Text='<%# Eval("PartID") %>'></asp:Label></td>
                                <td><%# Eval("TotalPartProduced") %></td>
                                <td><%# Eval("AcceptedPart") %></td>
                                <td><%# Eval("UnitWeight") %></td>
                                <td><%# Eval("TotalAcceptedPart") %></td>
                                <td rowspan='<%# Eval("RowSpan") %>' class='<%# Eval("RowSpanClass") %>'>
                                   
                                      <asp:Label runat="server" ID="lblTotalAcceptedPart" ClientIDMode="Static" Text='<%# Eval("TotalWeight") %>'></asp:Label>
                                </td>
                                <td><%# Eval("Rejection") %></td>
                                <td rowspan='<%# Eval("RowSpan") %>' class='<%# Eval("RowSpanClass") %>'>
                                    <asp:Label runat="server" ID="lblRejectedParts" ClientIDMode="Static" Text='<%# Eval("RejectedParts") %>'></asp:Label>
                                </td>
                                <td rowspan='<%# Eval("RowSpan") %>' class='<%# Eval("RowSpanClass") %>'>
                                    <asp:TextBox ID="txtDesignScrap" runat="server" ClientIDMode="Static" CssClass="form-control entryControl allowDecimal" AutoCompleteType="Disabled" Text='<%# Eval("DesignScrap") %>'></asp:TextBox>
                                </td>
                                <td rowspan='<%# Eval("RowSpan") %>' class='<%# Eval("RowSpanClass") %>'>
                                    <asp:TextBox ID="txtLayoutScrap" runat="server" ClientIDMode="Static" CssClass="form-control entryControl allowDecimal" AutoCompleteType="Disabled" Text='<%# Eval("LayoutScrap") %>'></asp:TextBox>
                                </td>
                                <td rowspan='<%# Eval("RowSpan") %>' class='<%# Eval("RowSpanClass") %>'>
                                    <asp:HiddenField runat="server" ID="hdnTotalScrap" ClientIDMode="Static" Value='<%# Eval("TotalScrap") %>'/>
                                    <asp:Label runat="server" ID="lblTotalScrap" ClientIDMode="Static" Text='<%# Eval("TotalScrap") %>'></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Warning!</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="lblWarningMsg" style="color: black"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function saveValidation() {
            var rows = $("#tblScrapEntry tr");
            debugger;
            for (var i = 2; i < rows.length; i++) {
                var row = rows[i];
                if ($(row).find("#hdnUpdate").val() == "update") {
                    var machine = $(row).find("#lblMachine").text();
                    //if ($(row).find("#txtDesignScrap").val().trim() == "") {
                    //    openWarningModal("Please enter Design Scrap for Machine '" + machine + "'");
                    //    return false;
                    //}
                    //if ($(row).find("#txtLayoutScrap").val().trim() == "") {
                    //    openWarningModal("Please enter Layout Scrap for Machine '" + machine + "'");
                    //    return false;
                    //}
                    //if ($(row).find("#lblTotalScrap").text().trim() == "") {
                    //    openWarningModal("Please enter Total Scrap for Machine '" + machine + "'");
                    //    return false;
                    //}
                }
            }
            return true;
        }
        $("#tblScrapEntry").on("click", "td .entryControl", function () {
            $(this).closest('tr').find("#hdnUpdate").val("update");
        });
        $(".entryControl").blur(function () {
            var row = $(this).closest('tr');
            calculateTotalScrap(row);

        });
        $('.allowDecimal').keypress(function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            debugger;
            if (charCode == 45 && pos != 0) {
                return false;
            } else if (charCode == 43 && pos != 0) {
                return false;
            } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                return false;
            } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                return false;
            }
            var afterdecimalpt = $(this).val().split('.')[1];
            if (afterdecimalpt != undefined) {
                if (afterdecimalpt.length > 2) {
                    return false;
                }
            }
            return true;
        });
        function calculateTotalScrap(row) {
            debugger;
            let designScrap, layoutScrap, rejectedParts;
            designScrap = $(row).find("#txtDesignScrap").val().trim() == "" ? "0" : $(row).find("#txtDesignScrap").val().trim();
            layoutScrap = $(row).find("#txtLayoutScrap").val().trim() == "" ? "0" : $(row).find("#txtLayoutScrap").val().trim();
            rejectedParts = $(row).find("#lblRejectedParts").text().trim() == "" ? "0" : $(row).find("#lblRejectedParts").text().trim();
            var totalW = $(row).find("#lblTotalAcceptedPart").text().trim() == "" ? 0 : parseFloat($(row).find("#lblTotalAcceptedPart").text().trim());
            if (designScrap != "" && layoutScrap != "") {
                if (parseFloat(designScrap) + parseFloat(layoutScrap) > totalW) {
                    openWarningModal("Sum of Design and Layout Scrap value should be less then or equal to Total Weight of Accepted parts value.");
                    $(row).find("#lblTotalScrap").text("");
                    $(row).find("#txtDesignScrap").val("");
                    $(row).find("#txtLayoutScrap").val("");
                    $(row).find('#hdnTotalScrap').val("");
                    return false;
                }
                $(row).find("#lblTotalScrap").text(parseFloat(designScrap) + parseFloat(layoutScrap) + parseFloat(rejectedParts));
            }
            else {
                $(row).find("#lblTotalScrap").text("");
            }
            $(row).find('#hdnTotalScrap').val($(row).find("#lblTotalScrap").text());
        }
        function setControls() {
            $('[id$=lbMachine]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=txtDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
        }
        function openSuccessModal(msg) {
            $('#toast-container').empty();
            Command: toastr["success"](msg)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                setControls();
            });
            $("#tblScrapEntry").on("click", "td .entryControl", function () {
                $(this).closest('tr').find("#hdnUpdate").val("update");
            });
            $(".entryControl").blur(function () {
                var row = $(this).closest('tr');
                calculateTotalScrap(row);

            });
            $('.allowDecimal').keypress(function (evt) {
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 45 && pos != 0) {
                    return false;
                } else if (charCode == 43 && pos != 0) {
                    return false;
                } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                    return false;
                } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                    return false;
                }
                var afterdecimalpt = $(this).val().split('.')[1];
                if (afterdecimalpt != undefined) {
                    if (afterdecimalpt.length > 2) {
                        return false;
                    }
                }
                return true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
