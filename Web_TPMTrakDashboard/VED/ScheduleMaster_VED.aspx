<%@ Page Language="C#" Title="" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleMaster_VED.aspx.cs" Inherits="Web_TPMTrakDashboard.VED.ScheduleMaster_VED" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>

    <style>
        .footer-row {
        position: sticky;
        bottom: 0;
        //background-color: white; /* Set the background color of the footer row */
        z-index: 1; /* Ensure the footer row is above the scrolling content */
    }
        .lblDiv {
            width: 130px;
            color: white;
            padding-top: 15px;
            text-align: center;
        }

        .ValsDiv {
            width: 180px;
            text-align: center;
        }

        .headerFixer tbody tr {
            background-color: #FFFFFF;
            color: black;
        }

            .headerFixer tbody tr:nth-last-child(2) {
                background-color: #DCDCDC;
                color: black;
            }

        .searchField {
            width: 100px;
            display: inline-block;
        }

        .actionBtn {
            font-size: 17px;
        }

        .paginationCss table {
            margin: auto;
        }

            .paginationCss table tr td {
                border: 0;
                padding: 0px;
            }

                .paginationCss table tr td a {
                    padding: 5px 10px;
                    border: 1px solid silver;
                }

                .paginationCss table tr td span {
                    padding: 5px 10px;
                    background: #ddd;
                    border: 1px solid silver;
                }
    </style>

  



    <div class="row"  style="margin-bottom: 4px; margin-left: 0px; margin-top: -11px;  ">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <div class="container-fluid">
                    <div class=""  >
                        <div class="col-lg-12 col-sm-12 col-md-12" >
                            
                            <div runat="server" class="row" style="display:flex; margin-left: 0px;">
                            <table id="tblHeader" class="table table-bordered" style="background-color: #394A59; width: auto; margin: 0px;margin-left:-28px;">
                                <tr>
                                    <td class="lblDiv" style="width: 60px; vertical-align: middle;">
                                        <b><%=GetGlobalResourceObject("CommanResource","Plant") %></b>
                                    </td>
                                    <td class="ValsDiv" style="min-width: 120px; vertical-align: middle;">
                                        <asp:DropDownList ID="ddlPlantId" runat="server" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" CssClass="form-control disabled-control" data-toggle="tooltip" title="Plant ID !" ToolTip="<%$Resources:CommanResource, PlantTooltip %>" AutoPostBack="True"  ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblDiv" style="width: 60px; vertical-align: middle;">
                                        <b><%=GetGlobalResourceObject("CommanResource","Cell") %></b>
                                    </td>
                                    <td class="ValsDiv" style="min-width: 120px; vertical-align: middle;">
                                        <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control disabled-control" data-toggle="tooltip" title="Cell ID !" ToolTip="<%$Resources:CommanResource, CellId %>" AutoPostBack="True" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>

                                    <td class="lblDiv" style="width: 60px; vertical-align: middle;"><b>From Date</b></td>
                                    <td class="ValsDiv" style="min-width: 120px; vertical-align: middle;">
                                        <div class="input-group ">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date disabled-control" data-toggle="tooltip"
                                                title="From Date !" placeholder="From Date" ToolTip="<%$Resources:CommanResource, FromDate %>" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </td>

                                    <td class="lblDiv" style="width: 60px; vertical-align: middle;"><b>To Date</b></td>
                                    <td class="ValsDiv" style="min-width: 120px; vertical-align: middle;">
                                        <div class="input-group ">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date disabled-control" data-toggle="tooltip"
                                                title="From Date !" placeholder="To Date" ToolTip="<%$Resources:CommanResource, ToDate %>" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td class="lblDiv" style="width: 20px; vertical-align: middle;font-weight:bold">Status</td>
                                    <td>
                                        <asp:ListBox ID="lbStatus" runat="server" SelectionMode="Multiple" Width="150"  ClientIDMode="Static">
                                            <asp:ListItem Text="New" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Running" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Closed" Value="3"></asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                    <td style="text-align: center;width:20px;" class="lblDiv">
                                        <asp:Button runat="server" Text="<%$ Resources: CommanResource, View %>" class="btn btn-info btn-sm disabled-control" ID="btnView" Width="60px" Font-Bold="true" Height="40px" OnClick="btnView_Click"></asp:Button>
                                    </td>
                                    <td style="text-align: center;width:20px;" class="lblDiv">
                                        <asp:Button runat="server" Text="<%$ Resources: CommanResource, Save %>" class="btn btn-info btn-sm disabled-control" ID="btnSave" Width="60px" Font-Bold="true" Height="40px" OnClick="btnSave_Click"></asp:Button>
                                    </td>
                                </tr>
                            </table>


                            <div runat="server" style="margin:2px;" >
                            <fieldset class="masterFS" style="margin-left: 5px !important; margin-right: 4px !important; display: inline-block">
                                <%--<legend>Select Excel file to import Master</legend>--%>
                                <div style="display: inline-block">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="fuPumpImport" runat="server" Style="width: 400px; height: 40px; display: inline-block; max-width: 200px;" CssClass="form-control input-sm" />&nbsp;
			<asp:LinkButton ID="lnkImportPumpFile" CssClass="btn btn-info" runat="server" ToolTip="Import" Font-Size="16px" Style="display: inline-block; vertical-align: middle; width: 87px; height: 36px;" OnClick="lnkImportPumpFile_Click" meta:resourcekey="btnexportResource1" Text="Import" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lnkImportPumpFile" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:LinkButton ID="btnTemplateExport" CssClass="glyphicon glyphicon-download-alt" Text="DownloadTemplate" runat="server" ToolTip="Template" Font-Size="20px" Style="display: inline-block; vertical-align: top;" OnClick="btnTemplateExport_Click" meta:resourcekey="btnexportResource1" />
                                </div>
                            </fieldset>
                               </div>

                                </div>

                          

                            <div style="height: 80vh; overflow: auto; margin:4px;margin-left:-28px;margin-top:10px;" id="gridContainer">
                                <asp:GridView ID="gvScheduleCreate" CssClass="gvScheduleCreate table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static" OnRowDataBound="gvScheduleCreate_RowDataBound" ShowFooter="true" AllowPaging="true" OnPageIndexChanging="gvScheduleCreate_PageIndexChanging" OnPreRender="gvScheduleCreate_PreRender" PageSize="100">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Start Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div class="input-group" style="min-width: 150px; border: 0">
                                                    <div class="input-group-addon">
                                                        <i class="glyphicon glyphicon-calendar"></i>
                                                    </div>
                                                    <asp:TextBox ID="txtStartDateNew" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                                </div>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Plant">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlantID" runat="server" Text='<%# Eval("PlantID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList runat="server" ID="ddlPlantNew" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantNew_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cell">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCell" runat="server" Text='<%# Eval("CellID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList runat="server" ID="ddlCellNew" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCellNew_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ComponentID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComponent" runat="server" Text='<%# Eval("ComponentID") %>'></asp:Label>

                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtComponentNew" ClientIDMode="Static" placeholder="Search Component" AutoCompleteType="Disabled" list="ddlComponentNew" CssClass="form-control" Style="display: inline-block;" AutoPostBack="true" OnTextChanged="txtComponentNew_TextChanged"></asp:TextBox>
                                                <datalist id="ddlComponentNew" runat="server" clientidmode="static" autopostback="true"></datalist>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Component Desc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComponentDesc" runat="server" Text='<%# Eval("ComponentDesc") %>'></asp:Label>

                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblComponentDescNew" runat="server" Text='<%# Eval("ComponentDesc") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Op No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOperation" runat="server" Text='<%# Eval("OperationNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList runat="server" ID="ddlOperationNew" ClientIDMode="Static" CssClass="form-control">
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Priority">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPriority" runat="server" Text='<%# Eval("PriorityNumber") %>' ClientIDMode="Static" CssClass="form-control txtUpdate allowNumber txtPriority" Enabled='<%# Eval("PriorityEnabled") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtPriorityNew" ClientIDMode="Static" CssClass="form-control txtPriority allowNumber"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty.">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtQty" ClientIDMode="Static" CssClass="form-control allowDecimal txtUpdate" Text='<%# Eval("Quantity") %>' Enabled='<%# Eval("QtyEnabled") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtQtyNew" ClientIDMode="Static" CssClass="form-control allowDecimal txtUpdate" Text=""></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closed Date">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEndDate" ClientIDMode="Static" CssClass=" txtUpdate" Text='<%# Eval("EndDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblEndDateNew" ClientIDMode="Static" CssClass="txtUpdate" Text='<%# Eval("EndDate") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblStatus" ClientIDMode="Static" CssClass=" txtUpdate" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblStatusNew" ClientIDMode="Static" CssClass="txtUpdate" Text='<%# Eval("Status") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkDelete" ClientIDMode="Static" CssClass="glyphicon glyphicon-trash actionBtn" OnClick="lnkDelete_Click" ToolTip="Delete"></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="lnkClose" ClientIDMode="Static" CssClass="glyphicon glyphicon-remove actionBtn" OnClick="lnkClose_Click" ToolTip="Close"></asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton runat="server" ID="lnkInsert" ClientIDMode="Static" CssClass="glyphicon glyphicon-plus actionBtn" OnClick="lnkInsert_Click" ToolTip="Insert" OnClientClick="return insertValidation(this);"></asp:LinkButton>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                       
                                    </Columns>
                                    <PagerStyle CssClass="paginationCss" />
                                     <FooterStyle CssClass="footer-row" />
                                </asp:GridView>

                            </div>

                            <div class="modal fade" id="deleteConfirmModal" role="dialog">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content modalContent confirm-modal-content">
                                        <div class="modal-header modalHeader confirm-modal-header">
                                            <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                                        </div>
                                        <div>
                                            <br />
                                            <h4 class="confirm-modal-title">Confirmation!</h4>
                                            <br />
                                            <span class="confirm-modal-msg">Are you sure you want to delete Schedule?</span>
                                        </div>
                                        <div class="modal-footer modalFooter modal-footer">
                                            <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" OnClientClick="return clearModalScreen();" />
                                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="modal fade" id="closeConfirmModal" role="dialog">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content modalContent confirm-modal-content">
                                        <div class="modal-header modalHeader confirm-modal-header">
                                            <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                                        </div>
                                        <div>
                                            <br />
                                            <h4 class="confirm-modal-title">Confirmation!</h4>
                                            <br />
                                            <span class="confirm-modal-msg">Are you sure you want to Close Schedule?</span>
                                        </div>
                                        <div class="modal-footer modalFooter modal-footer">
                                            <asp:Button runat="server" Text="Yes" ID="btnCloseConfirm" CssClass="confirm-modal-btn" OnClick="btnCloseConfirm_Click" OnClientClick="return clearModalScreen();" />
                                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnTemplateExport" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="modal fade" id="warningModal" role="dialog" style="z-index: 2000">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content modalContent warning-modal-content">
                    <div class="modal-header modalHeader warning-modal-header">
                        <i class="glyphicon glyphicon-warning-sign modal-icons"></i>
                    </div>
                    <div>
                        <br />
                        <h4 class="warning-modal-title">Warning!</h4>
                        <br />
                        <span class="warning-modal-msg" id="lblWarningMsg">...</span>
                    </div>
                    <div class="modal-footer modalFooter modal-footer">
                        <input type="button" value="OK" class="warning-modal-btn" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="errorModal" role="dialog" style="z-index: 2000">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content modalContent error-modal-content">
                    <div class="modal-header modalHeader error-modal-header">
                        <i class="glyphicon glyphicon-remove-sign modal-icons"></i>
                    </div>
                    <br />
                    <h4 class="error-modal-title">Error</h4>
                    <br />
                    <span class="error-modal-msg" style="color: black !important;" id="lblErrorMsg">...</span>

                    <div class="modal-footer modalFooter modal-footer">
                        <input type="button" value="OK" class="error-modal-btn" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            setControls();
        });

        function setControls() {
            $('[id$=txtFromDate]').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtToDate]').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=lbStatus]').multiselect({
                includeSelectAllOption: true
            });

            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            $('[id$=txtStartDateNew]').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $("[id$=txtStartDateNew]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
        }

        function insertValidation(ctrl) {
            var txtComp = $(ctrl).closest('tr').find('#txtComponentNew').val();
            var txtPriority = $(ctrl).closest('tr').find('#txtPriorityNew').val();
            var ddlplant = $(ctrl).closest('tr').find('#ddlPlantNew').val();
            var ddlcell = $(ctrl).closest('tr').find('#ddlCellNew').val();
            var ddloperation = $(ctrl).closest('tr').find('#ddlOperationNew').val();
            var txtstartdate = $(ctrl).closest('tr').find('#txtStartDateNew').val();
            var txtqty = $(ctrl).closest('tr').find('#txtQtyNew').val();
            if (txtComp == "") {
                openWarningModal("Please select Component.");
                return false;
            }
            if (txtPriority.trim() == "") {
                openWarningModal("Please Enter Priority for Schedule.");
                return false;
            }
            if (ddlplant == "") {
                openWarningModal("Please select plant for Schedule.");
                return false;
            }
            if (ddlcell == "") {
                openWarningModal("Please select cell for Schedule.");
                return false;
            }
            if (ddloperation == "") {
                openWarningModal("Please select operation for Schedule.");
                return false;
            }
            if (txtstartdate == "") {
                openWarningModal("Please enter startdate for Schedule.");
                return false;
            }
            if (txtqty == "") {
                openWarningModal("Please enter qty. for Schedule.");
                return false;
            }
            //var datalistOption = $(ctrl).closest('tr').find('#dlComponentNew option');
            //var datalistHidden = $(ctrl).closest('tr').find('#dlComponentNew .compHdn');
            //var flag = 0;
            //debugger;
            //for (var i = 0; i < datalistOption.length; i++) {
            //    if (txtComp == $(datalistOption[i]).val()) {
            //        flag = 1;
            //        $(ctrl).closest('tr').find('#hdnCompNew').val($(datalistHidden[i]).val());
            //        break;
            //    }
            //}
            //if (flag == 1) {
            //    return true;
            //}
            //else {
            //    openWarningModal("Please select correct Component.");
            //    return false;
            //}
        }


        function openConfirmModal(id) {
            $("#" + id).modal('show');
        }
        function openWarningModal(msg) {
            $("#lblWarningMsg").text(msg);
            $("#warningModal").modal('show');
        }
        function openErrorModal(msg) {
            $('#errorModal').modal('show');
            $('#lblErrorMsg').text(msg);
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




        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            setControls();
            $.unblockUI({});


        });
    </script>

</asp:Content>



