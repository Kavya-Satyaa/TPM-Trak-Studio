<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JHChecklistMasterAdvik184.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.JHChecklistMasterAdvik184" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style type="text/css">
        .headerFixerTable tr th {
            position: sticky;
            top: -10px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 96%;
            overflow: auto;
            min-height: 70px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
            }

        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        .table tbody > tr > th {
            vertical-align: middle;
            white-space: nowrap;
        }

        .table > tr > td {
            vertical-align: middle;
        }
        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 15px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 60px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        a {
            color: black;
        }

        .table .lbl {
            padding-top: 15px;
        }

        #MainContent_updateTableViewData {
            margin-top: -15px;
        }

        .machineClick {
            text-decoration: underline;
            cursor: pointer;
        }

        th[data-content='OEE'] td {
            text-decoration: underline;
            cursor: pointer;
        }

        .hypercol {
            text-decoration: underline;
            cursor: pointer;
        }

        .GridHeader {
            text-align: center !important;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .footerRow {
            vertical-align: central;
            text-align: center;
        }

        .FixedFooter > tbody > tr:last-child > td {
            position: sticky;
            bottom: 0px;
            background-color: #2E6886;
        }

        .footerFixed {
            font-weight: bold;
            position: -webkit-sticky;
            position: sticky;
            bottom: 190px;
            left: 0;
            z-index: 2;
        }

        .GVFixedFooter {
            font-weight: bold;
            position: relative;
            bottom: expression(getScrollBottom(this.parentNode.parentNode.parentNode.parentNode));
        }
    </style>

    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="display: flex; justify-content: center; align-content: center;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
                </div>
                <div style="display: flex; justify-content: center; align-content: center; margin-bottom: 20px;">

                    <table id="tblfilter" class="table table-bordered" style="width: 96%;">
                        <tr>
                            <td class="commanTd" style="min-width: 50px; height: 50px">Plant</td>
                            <td style="min-width: 160px;">
                                <asp:DropDownList ID="ddlPlants" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlants_SelectedIndexChanged" />
                            </td>
                            <td class="commanTd" style="min-width: 50px; height: 50px">Group</td>
                            <td style="min-width: 160px;">
                                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" />
                            </td>
                            <td class="commanTd" style="width: 70px;">Machine</td>
                            <td style="min-width: 160px;">
                                <asp:DropDownList ID="ddlMachines" runat="server" CssClass="form-control" />
                            </td>
                            <td class="commanTd" style="width: auto;">JH Type</td>
                            <td style="min-width: 160px;">
                                <asp:DropDownList ID="ddlJHType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="All" />
                                    <asp:ListItem Value="Auto" Text="Auto" />
                                    <asp:ListItem Value="Manual" Text="Manual" />
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center; width: auto;">
                                <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>
                            </td>
                            <td>
                                <asp:FileUpload ID="fuToImport" runat="server" CssClass="form-control" Width="275" />
                            </td>
                            <td style="text-align: center; width: auto;">
                                <asp:Button runat="server" Text="Import" CssClass="btn btn-info btn-sm displayCss" ID="btnImport" OnClick="btnImport_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divMaster" style="display: flex; justify-content: center; align-content: center;">
                    <div id="divChecklistMaster" class="divGrid" style="">
                        <asp:GridView runat="server" ID="gvChecklistMaster" AutoGenerateColumns="False" ClientIDMode="Static"
                            CssClass="table table-bordered cockpit headerFixerTable FixedFooter" ShowHeaderWhenEmpty="true" ShowFooter="true" Style="">
                            <Columns>
                                <%--<asp:TemplateField HeaderText="Machine">
								<ItemTemplate>
									<span style="white-space: nowrap;color:white;"><%#Eval("MachineID")%></span>
								</ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlGvMachines" runat="server" CssClass="form-control" Width="120"/>
                                </FooterTemplate>
							</asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="RegisterAddress/ChecklistID">
                                    <ItemTemplate>
                                        <%--<span style="white-space: nowrap;color:white;"><%#Eval("ChecklistID")%></span>--%>
                                        <asp:HiddenField ID="hdfUpdate" runat="server" />
                                        <asp:Label runat="server" ID="lblIChecklistId" Text='<%#Eval("ChecklistID")%>' ForeColor="White" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtChecklistID" runat="server" CssClass="form-control" Style="text-align: center;" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Checklist Item">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblIChecklistItem" Text='<%#Eval("ChecklistItem")%>' ForeColor="White" />
                                        <asp:TextBox ID="txtIChecklistItem" runat="server" MaxLength="20" CssClass="form-control" Width="100%" Text='<%#Eval("ChecklistItem")%>' Visible="false" Style="text-align: center;" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtChecklistItem" runat="server" MaxLength="20" CssClass="form-control" Width="100%" Style="text-align: center;" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="McArea">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMcArea" Text='<%#Eval("McArea")%>' ForeColor="White" />
                                        <asp:TextBox ID="txtMcAreaEdit" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Text='<%#Eval("McArea")%>' Visible="false" Style="text-align: center;" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtMcAreaNew" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Style="text-align: center;" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblLocation" Text='<%#Eval("Location")%>' ForeColor="White" />
                                        <asp:TextBox ID="txtLocationEdit" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Text='<%#Eval("Location")%>' Visible="false" Style="text-align: center;" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtLocationNew" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Style="text-align: center;" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Std Condition">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCondition" Text='<%#Eval("StdCondition")%>' ForeColor="White" />
                                        <asp:TextBox ID="txtConditionEdit" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Text='<%#Eval("StdCondition")%>' Visible="false" Style="text-align: center;" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtConditionNew" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Style="text-align: center;" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Checking Method">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCheckingMethod" Text='<%#Eval("CheckingMethod")%>' ForeColor="White" />
                                        <asp:TextBox ID="txtCheckingMethodEdit" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Text='<%#Eval("CheckingMethod")%>' Visible="false" Style="text-align: center;" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCheckingMethodNew" runat="server" MaxLength="14" CssClass="form-control" Width="100%" Style="text-align: center;" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="No of Cycles">
								<ItemTemplate>
                                    <asp:Label runat="server" ID="lblINoOfCycles" Text='<%#Eval("NoOfCycles")%>' ForeColor="White" />
                                    <asp:TextBox ID="txtINoOfCycles" runat="server" CssClass="form-control" Text='<%#Eval("NoOfCycles")%>' Visible="false" style="text-align:center;"/>
								</ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNoOfCycles" runat="server" CssClass="form-control" style="text-align:center;"/>
                                </FooterTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"/>
							</asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Is Enabled">
                                    <ItemTemplate>
                                        <%--<span style="white-space: nowrap;color:white;"><%#Eval("IsEnabled")%></span>--%>
                                        <asp:CheckBox runat="server" ID="chkEnbl" CssClass="chkIsEnabled" Checked='<%#Eval("IsEnabled")%>' Enabled="false" Style="text-align: center;" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkIsEnabled" runat="server" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="JH Type">
                                    <ItemTemplate>
                                        <%--<span style="white-space: nowrap;color:white;"><%#Eval("NoOfCycles")%></span>--%>
                                        <asp:Label runat="server" ID="lblJHType" Text='<%#Eval("JHType")%>' ForeColor="White" />
                                        <%--<asp:DropDownList runat="server" ID="ddlItemJHType" SelectedValue='<%#Eval("JHType")%>' CssClass="form-control" Visible="false">
                                        <asp:ListItem Value="JH Auto" Text="JH Auto" />
                                        <asp:ListItem Value="JH Manual" Text="JH Manual" />
                                    </asp:DropDownList>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList runat="server" ID="ddlFooterJHType" CssClass="form-control" Style="width: auto">
                                            <asp:ListItem Value="Auto" Text="Auto" />
                                            <asp:ListItem Value="Manual" Text="Manual" />
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sort Order">
                                    <ItemTemplate>
                                        <%--<span style="white-space: nowrap;color:white;"><%#Eval("SortOrder")%></span>--%>
                                        <asp:Label runat="server" ID="lblSortOrder" Text='<%#Eval("SortOrder")%>' ForeColor="White" />
                                        <asp:TextBox ID="txtISortOrder" runat="server" CssClass="form-control" Text='<%#Eval("SortOrder")%>' Visible="false" Style="text-align: center;" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtSortOrder" runat="server" CssClass="form-control" Style="text-align: center;" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;" />
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="HeaderCss" HorizontalAlign="Center" />

                            <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" Height="35px" BackColor="#2E6886" />
                            <EmptyDataTemplate>
                                <div style="height: 100%; background-color: white; text-align: center; color: red">No Checklist Information Available</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
                <div style="display: flex; justify-content: center; width: 75%; margin: auto; margin-top: 10px;">
                    <asp:Button runat="server" Text="New" CssClass="btn btn-info btn-sm displayCss" ID="btnAdd" OnClick="btnAdd_Click" ClientIDMode="Static" Style="margin-right: 20px;"></asp:Button>
                    <asp:Button runat="server" Text="Edit" CssClass="btn btn-info btn-sm displayCss" ID="btnSave" OnClick="btnSave_Click" Style="margin-right: 20px;"></asp:Button>
                    <asp:Button runat="server" Text="Cancel" CssClass="btn btn-info btn-sm displayCss" ID="btnCancel" OnClick="btnCancel_Click"></asp:Button>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnImport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
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
                    <span class="error-modal-msg" id="lblErrorMsg">...</span>

                    <div class="modal-footer modalFooter modal-footer">
                        <input type="button" value="OK" class="error-modal-btn" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
    <script>
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMessages.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
        function resize() {

            var heights = window.innerHeight - 220;
            document.getElementById("divChecklistMaster").style.height = heights + "px";
        }

        function getScrollBottom() {
            debugger;
            var footer = $('#<%=gvChecklistMaster.ClientID %>').find('.footerRow');
            $(footer[0]).addClass("footerFixed");
        }
        function IsRowPresent() {
            debugger;
            var isPresent = false;
            if ($('[id$=btnAdd]').val() == "Add") {
                var machineID = $('[id$=ddlMachines]').val();
                var jhType = $('[id$=ddlJHType]').val();
                var checklistItem = "";
                //var noOfCycles = "";
                var sortOrder = "";
                var checklistID = "";

                var txtIChecklistID = $('#<%=gvChecklistMaster.ClientID %> tr:last').each(function () {
                    var textBoxes = $(this).find('input[type=text]');
                    checklistItem = $(textBoxes[1]).val();
                    noOfCycles = $(textBoxes[2]).val();
                    //sortOrder = $(textBoxes[3]).val();
                    checklistID = $(textBoxes[0]).val();
                });

                if (checklistID != "" && checklistItem != "" && sortOrder != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "JHChecklistMasterAdvik184.aspx/IsRowPresent",
                        data: '{machineID:"' + machineID + '", checklistID:"' + checklistID + '", jhType:"' + jhType + '"}',
                        dataType: "json",
                        success: function (result) {
                            debugger;
                            isPresent = result.d;
                            if (!isPresent) {
                                //alert("The entered Checklist is already Present, Please try another Checklist ID!");
                                $('[id$=lblMessages]').css("color", "red");
                                $('[id$=lblMessages]').text("The entered Checklist is already Present, Please try another Checklist ID!");
                                $('[id$=lblMessages]').css("display", "block");
                                return false;
                            }
                            else return true;
                        },
                        error: function (Result) {
                            alert("Error");
                        }
                    });
                }
                else {
                    $('[id$=lblMessages]').css("color", "red");
                    $('[id$=lblMessages]').css("display", "block");
                    $('[id$=lblMessages]').text("Please fill all the Informations!");
                    //alert("Please fill all the information!");
                    return false;
                }
            }
            else return true;
        }
        $(document).ready(function () {
            $.unblockUI({});
            resize();
            window.onresize = function () {
                resize();
            };

            debugger;
            $('#<%=gvChecklistMaster.ClientID %> tr').not(':first').not(':last').each(function () {
                var textboxes = $(this).find('input[type=text]');
                //$(textboxes[1]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //})
                //$(textboxes[2]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //})
            })
            $('#<%=gvChecklistMaster.ClientID %> tr:last').each(function () {
                var txtFooter = $(this).find('input[type=text]');
                //$(txtFooter[2]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //});
                //$(txtFooter[3]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //});
            });

            $('#<%=gvChecklistMaster.ClientID %> tr td').change(function () {
                $(this).closest('tr').find('input[type=hidden]').val("Update");
            })
            document.querySelector("#btnAdd").addEventListener('click', function (event) {
                debugger;
                var isPresent = false;
                var IsReqComp = false;
                if ($('[id$=btnAdd]').val() == "Add") {
                    var machineID = $('[id$=ddlMachines]').val();
                    if (machineID == "" || machineID == null) {
                        openWarningModal("Please select MachineId.");
                        return false;
                    }
                    var jhType = $('[id$=ddlJHType]').val();
                    var newChklistContents = $("#gvChecklistMaster tr").last().find('input[type=text]');
                    if (newChklistContents != null) {
                        var checklistID = $(newChklistContents[0]).val();
                        var checklistItem = $(newChklistContents[1]).val();
                        var location = $(newChklistContents[2]).val();
                        var stdcondition = $(newChklistContents[3]).val();
                        var checkingmethod = $(newChklistContents[4]).val();
                        //var noOfCycles = $(newChklistContents[2]).val();
                        var sortOrder = $(newChklistContents[5]).val();

                        if (checklistID != "" && checklistItem != "" && sortOrder != "" && location != "" && stdcondition != "" && checkingmethod != "") {
                            var httpRequest = new XMLHttpRequest();
                            if (!httpRequest) {
                                alert('Giving up :( Cannot create an XMLHTTP instance');
                                event.preventDefault();
                            }
                            httpRequest.onload = function () {
                                if (httpRequest.readyState === XMLHttpRequest.DONE) {
                                    if (httpRequest.status === 200) {
                                        IsReqComp = true;
                                        isPresent = JSON.parse(httpRequest.responseText).d;
                                        if (!isPresent) {
                                            $('[id$=lblMessages]').css("color", "red");
                                            $('[id$=lblMessages]').text("The entered Checklist ID is already Present, Please try another Checklist ID!");
                                            $('[id$=lblMessages]').css("display", "block");
                                            event.preventDefault();
                                        }
                                        else {
                                            debugger;
                                            event.returnValue = true;
                                        }
                                    }
                                }
                            };
                            httpRequest.onabort = function () {
                                alert('Request Aborted. Please try again.');
                            };
                            httpRequest.ontimeout = function () {
                                alert('Request Timeout. Please try again.');
                            };
                            httpRequest.onerror = function () {
                                alert('Error!!');
                            };
                            httpRequest.open('POST', 'JHChecklistMasterAdvik184.aspx/IsRowPresent', false);
                            //httpRequest.timeout = 60000;
                            httpRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                            httpRequest.send('{machineID:"' + machineID + '", checklistID:"' + checklistID + '", jhType:"' + jhType + '"}');
                        }
                        else {
                            $('[id$=lblMessages]').css("color", "red");
                            $('[id$=lblMessages]').css("display", "block");
                            $('[id$=lblMessages]').text("Please fill all the Informations!");
                            event.preventDefault();
                        }
                    }
                    else {
                        event.preventDefault();
                    }
                    if (!IsReqComp) {
                        event.preventDefault();
                    }
                }
                else {
                    event.returnValue = true;
                }
            });
        });
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
            resize();
            window.onresize = function () {
                resize();
            };

            debugger;
            $('#<%=gvChecklistMaster.ClientID %> tr').not(':first').not(':last').each(function () {
                var textboxes = $(this).find('input[type=text]');
                //$(textboxes[1]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //})
                //$(textboxes[2]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //})
            })

            $('#<%=gvChecklistMaster.ClientID %> tr:last').each(function () {
                var txtFooter = $(this).find('input[type=text]');
                //$(txtFooter[2]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //});
                //$(txtFooter[3]).keypress(function (e) {
                //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                //        return false;
                //    }
                //});
            });
            $('#<%=gvChecklistMaster.ClientID %> tr td').change(function () {
                $(this).closest('tr').find('input[type=hidden]').val("Update");
            })
            document.querySelector("#btnAdd").addEventListener('click', function (event) {
                var isPresent = false;
                var IsReqComp = false;
                if ($('[id$=btnAdd]').val() == "Add") {
                    var machineID = $('[id$=ddlMachines]').val();
                    if (machineID == "" || machineID == null) {
                        openWarningModal("Please select MachineId.");
                        return false;
                    }
                    var jhType = $('[id$=ddlJHType]').val();
                    var newChklistContents = $("#gvChecklistMaster tr").last().find('input[type=text]');
                    if (newChklistContents != null) {
                        var checklistID = $(newChklistContents[0]).val();
                        var checklistItem = $(newChklistContents[1]).val();
                        var location = $(newChklistContents[2]).val();
                        var stdcondition = $(newChklistContents[3]).val();
                        var checkingmethod = $(newChklistContents[4]).val();
                        //var noOfCycles = $(newChklistContents[2]).val();
                        var sortOrder = $(newChklistContents[5]).val();

                        if (checklistID != "" && checklistItem != "" && sortOrder != "" && location != "" && stdcondition != "" && checkingmethod != "") {
                            var httpRequest = new XMLHttpRequest();
                            if (!httpRequest) {
                                alert('Giving up :( Cannot create an XMLHTTP instance');
                                event.preventDefault();
                            }
                            httpRequest.onload = function () {
                                if (httpRequest.readyState === XMLHttpRequest.DONE) {
                                    if (httpRequest.status === 200) {
                                        IsReqComp = true;
                                        isPresent = JSON.parse(httpRequest.responseText).d;
                                        if (!isPresent) {
                                            $('[id$=lblMessages]').css("color", "red");
                                            $('[id$=lblMessages]').text("The entered Checklist ID is already Present, Please try another Checklist ID!");
                                            $('[id$=lblMessages]').css("display", "block");
                                            event.preventDefault();
                                        }
                                        else {
                                            debugger;
                                            event.returnValue = true;
                                        }
                                    }
                                }
                            };
                            httpRequest.onabort = function () {
                                alert('Request Aborted. Please try again.');
                            };
                            httpRequest.ontimeout = function () {
                                alert('Request Timeout. Please try again.');
                            };
                            httpRequest.onerror = function () {
                                alert('Error!!');
                            };
                            httpRequest.open('POST', 'JHChecklistMasterAdvik184.aspx/IsRowPresent', false);
                            //httpRequest.timeout = 60000;
                            httpRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                            httpRequest.send('{machineID:"' + machineID + '", checklistID:"' + checklistID + '", jhType:"' + jhType + '"}');
                        }
                        else {
                            $('[id$=lblMessages]').css("color", "red");
                            $('[id$=lblMessages]').css("display", "block");
                            $('[id$=lblMessages]').text("Please fill all the Informations!");
                            event.preventDefault();
                        }
                    }
                    else {
                        event.preventDefault();
                    }
                    if (!IsReqComp) {
                        event.preventDefault();
                    }
                }
                else {
                    event.returnValue = true;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
