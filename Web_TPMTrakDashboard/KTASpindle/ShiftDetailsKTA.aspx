<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShiftDetailsKTA.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.ShiftDetailsKTA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
        }

        #MainContent_gridShiftDetails tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        #MainContent_gridShiftDetails tbody tr:nth-child(even) {
            background-color: #FFFFFF;
        }
    	.auto-style7 {
			width: 120px;
		}
		.table-bordered {}
		.auto-style15 {
			color: white;
			width: 133px;
            vertical-align: middle;
		}
		.auto-style17 {
			color: white;
			width: 133px;
			height: 36px;
            vertical-align: middle;
		}
		.auto-style18 {
			width: 120px;
			height: 36px;
            vertical-align: middle !important;
		}
		.auto-style20 {
			height: 36px;
            vertical-align: middle !important;
		}
		.auto-style21 {
			width: 219px;
			height: 36px;
            vertical-align: middle !important;
		}
		.auto-style24 {
			width: 219px;
            vertical-align: middle;
		}
		.auto-style27 {
			color: white;
			width: 136px;
			height: 36px;
            vertical-align: middle;
		}
		.auto-style28 {
			color: white;
            vertical-align: middle;
			width: 136px;
		}
      .table tbody>tr>td
        {
          padding:5px !important;
        }
    </style>
      <asp:UpdatePanel ID="updatePnal" runat="server">
        <ContentTemplate>

            <div class="row text-center">
                <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
            </div>

            <div class="">
                <table class="table table-bordered" style="width: 99%;">
                    <tr>
                        <td class="auto-style17" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ShiftType") %></td>
                        <td>
                            <asp:DropDownList ID="ddlShiftType" runat="server" CssClass="form-control" Height="30px" Width="150px" OnSelectedIndexChanged="ddlShiftType_SelectedIndexChanged" AutoPostBack="true" >
                            </asp:DropDownList>
                        </td>
                        <td colspan="5">
                            <asp:Button ID="btnCreate" runat="server" Text="<%$ Resources: CommanResource, Create %>" CssClass="btn btn-info" OnClick="btnCreate_Click" Style="display: inline" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style17" style="vertical-align: middle" ><%=GetGlobalResourceObject("CommanResource","ShiftID") %></td>
                        <td class="auto-style18">
                            <asp:DropDownList ID="ddlShiftID" runat="server" CssClass="form-control"
                                OnSelectedIndexChanged="ddlShiftID_SelectedIndexChanged" AutoPostBack="true" Height="30px" Width="150px">
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                            </asp:DropDownList></td>

                        <td class="auto-style27" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","FromDay") %></td>
                        <td class="auto-style20">
                            <asp:DropDownList ID="ddlFromDay" runat="server" CssClass="form-control" >
                                <asp:ListItem Value="Today">Today</asp:ListItem>
                                <asp:ListItem Value="Tomorrow">Tomorrow</asp:ListItem>
                                <asp:ListItem Value="Yesterday">Yesterday</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style27" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ToDay") %></td>
                        <td class="auto-style20">
                            <asp:DropDownList ID="ddlToDay" runat="server" CssClass="form-control">
                                <asp:ListItem Value="Today">Today</asp:ListItem>
                                <asp:ListItem Value="Tomorrow">Tomorrow</asp:ListItem>
                                <asp:ListItem Value="Yesterday">Yesterday</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style21">
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources: CommanResource, Save %>" CssClass="btn btn-info" OnClick="btnSave_Click" Style="display: inline" />&nbsp;<asp:Button ID="btnHour" runat="server" Text="<%$ Resources: CommanResource, HourDefinition %>" CssClass="btn btn-info" Style="display: inline" />
                        	&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="auto-style15" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ShiftName") %></td>
                        <td class="auto-style7">
                            <asp:TextBox ID="txtShiftName" runat="server" CssClass="form-control" Width="150px" AutoCompleteType="Disabled"></asp:TextBox></td>
                        <td class="auto-style28" style="vertical-align: middle "><%=GetGlobalResourceObject("CommanResource","FromTime") %></td>
                        <td class="input-group">
                            <div class="input-group-addon" style="height:34px">
                                <i class="glyphicon glyphicon-time"></i>
                            </div>
                            <asp:TextBox ID="txtFromTime" runat="server" CssClass="form-control date" Height="34px" AutoCompleteType="Disabled"></asp:TextBox></td>
                        <td class="auto-style28" style="vertical-align: middle"><%=GetGlobalResourceObject("CommanResource","ToTime") %></td>
                        <td class="input-group">
                            <div class="input-group-addon" style="height:34px">
                                <i class="glyphicon glyphicon-time"></i>
                            </div>
                            <asp:TextBox ID="txtToTime" runat="server" CssClass="form-control date"  Height="34px" AutoCompleteType="Disabled"></asp:TextBox></td>
                        <td class="auto-style24">
                            <asp:Button ID="btnClear" runat="server" Text="<%$ Resources: CommanResource, ClearAll %>" CssClass="btn btn-info" OnClick="btnClear_Click" />&nbsp;&nbsp;</td>
                    </tr>

                </table>

                <asp:GridView ID="gridShiftDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" Width="54%">
                    <Columns>
                        <asp:BoundField DataField="ShiftType" HeaderText="<%$ Resources: CommanResource, ShiftType %>" />
                        <asp:BoundField DataField="shiftId" HeaderText="<%$ Resources: CommanResource, ShiftID %>" />
                        <asp:BoundField DataField="ShiftName" HeaderText="<%$ Resources: CommanResource, ShiftName %>" />
                        <asp:BoundField DataField="FromDay" HeaderText="<%$ Resources: CommanResource, FromDay %>" />
                        <asp:BoundField DataField="FromTime" HeaderText="<%$ Resources: CommanResource, FromTime %>" />
                        <asp:BoundField DataField="ToDay" HeaderText="<%$ Resources: CommanResource, ToDay %>" />
                        <asp:BoundField DataField="ToTime" HeaderText="<%$ Resources: CommanResource, ToTime %>" />
                    </Columns>
                    <HeaderStyle CssClass="HeaderCss" />
                </asp:GridView>
            </div>

            <div class="modal infoModal bajaj-info-modal" id="newShiftTypeModal" role="dialog" style="min-width: 850px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 850px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header" style="padding:8px 15px !important;">
                                <h4 class="modal-title" id="modalTitle" runat="server">Add Shift Type</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('newShiftTypeModal','compare');"></i>
                                <asp:HiddenField runat="server" ID="hfShiftTypeNewEdit" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 50%;" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Shift Type</td>
                                            <td>
                                                 <asp:TextBox runat="server" ID="txtShiftType" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>
                                                 <asp:Button runat="server" ID="btnShiftTypeSave" ClientIDMode="Static" Text="Save" CssClass="bajaj-btn-style   bajaj-add-edit-btn-style" OnClientClick="return ShiftTypeValidation();" OnClick="btnShiftTypeSave_Click"
                                                     />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gvShiftType" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No.">
                                                <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSlNo" ClientIDMode="Static" Text='<%# Eval("Slno") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="<%$ Resources: CommanResource, ShiftType %>">
                                                <ItemTemplate>
                                                     <asp:Label runat="server" ID="lblShiftType" ClientIDMode="Static" Text='<%# Eval("ShiftType") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel runat="server">
                                                      <ContentTemplate>
                                                           <asp:LinkButton runat="server" ID="lbShiftTypeDelete" CssClass="bajaj-action-icons bajaj-delete-icons" ToolTip="Delete" OnClick="lbShiftTypeDelete_Click">
                                                               <i class="glyphicon glyphicon-trash "></i>
                                                               <span>DELETE</span> 
                                                           </asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lbShiftTypeDelete" EventName="Click" />
                                                    </Triggers>
                                                   </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="HeaderCss" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('newShiftTypeModal','compare');" class="bajaj-btn-style cancel-btn">CANCEL</button>
                            </div>
                        </div>
                    </div>
                </div>
               <div class="modal fade" id="deleteConfirmModal" role="dialog">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content modalContent confirm-modal-content">
                            <div class="modal-header modalHeader confirm-modal-header">
                                <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                                <br />
                                <h4 class="confirm-modal-title">Confirmation!</h4>
                                <br />
                                <span id="DeleteMsg" class="confirm-modal-msg">Are you sure you want to delete Record?</span>
                            </div>
                            <div class="modal-footer modalFooter modal-footer">
                                <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                                 <asp:Button runat="server" Text="No" ID="btnNo" CssClass="confirm-modal-btn" OnClick="btnNo_Click" ClientIDMode="Static" />
                            </div>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

        <script type="text/javascript">
        $(document).ready(function () {
            $.unblockUI({});
            $(".date").datetimepicker({
                format: 'HH:mm:ss',
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $(".date").datetimepicker({
                format: 'HH:mm:ss',
            });
        });

        $(document).on("click", "[id$=btnClear]", function (e) {
            if (confirm("Do you want to clear all data?")) {
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            }
            else {
                return false;
            }
        });

        $(document).on("change", "[id$=ddlShiftID]", function () {
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "[id$=btnSave]", function () {
            
            var fromTime = $("[id$=txtFromTime]").val();
            var toTime = $('[id$=txtToTime]').val();
            
            var stt = new Date("November 13, 2000 " + fromTime);
            stt = stt.getTime();
            if (($("[id$=ddlToDay]").val() == "Tomorrow")) {
            	var endt = new Date("November 14, 2000 " + toTime);
            	endt = endt.getTime();
            }
            else
            {
            	var endt = new Date("November 13, 2000 " + toTime);
            	endt = endt.getTime();
            }
          


            if ($("[id$=txtShiftName]").val() == "") {
                alert("Please enter the Shift Name !!");
                $("[id$=txtShiftName]").focus();
                return false;
            }
            if ($("[id$=txtFromTime]").val() == "") {
                alert("Please enter the From Time !!");
                $("[id$=txtFromTime]").focus();
                return false;
            }
            if ($("[id$=txtToTime]").val() == "") {
                alert("Please enter the To Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }
            if ((($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Today")) ||
                (($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Yesterday")) ||
                (($("[id$=ddlFromDay]").val() == "Yesterday") && ($("[id$=ddlToDay]").val() == "Tomorrow")) ||
                (($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Yesterday")) ||
                (($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Today"))) {
                alert("Please enter Valid Days. !!");
                $("[id$=ddlFromDay]").focus();
                return false;
            }
            if ((($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Today")) && (stt > endt)) {
                alert("Please enter Valid Timings. From Time Cannot Be Greater than To Time !!");
                $("[id$=txtFromTime]").focus();
                return false;
            }
            if (stt == endt) {
                alert("Please enter Valid Timings. To Time Cannot Be  Equal to  the From Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }
            if (endt < stt) {
                alert("Please enter Valid Timings. From Time Cannot Be  Less Than the To Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }
            if (($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Tomorrow")) {
            	
            	var difference = endt - stt;
            	if (difference > 86400000) {
                    alert("Please enter Valid Timings. Time Is Greater than 24 hrs. Please Check the End time !!");
                    $("[id$=txtToTime]").focus();
                    return false;
                }
            }
            if ((($("[id$=ddlFromDay]").val() == "Today") && ($("[id$=ddlToDay]").val() == "Today")) && (stt == endt) ||
               (($("[id$=ddlFromDay]").val() == "Tomorrow") && ($("[id$=ddlToDay]").val() == "Tomorrow")) && (stt == endt) ||
                 (($("[id$=ddlFromDay]").val() == "Yesterday") && ($("[id$=ddlToDay]").val() == "Yesterday")) && (stt == endt)) {
                alert("Please enter Valid Timings. From Time Cannot Be Equal To the To Time !!");
                $("[id$=txtToTime]").focus();
                return false;
            }

              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        })

         $(document).on("click", "[id$=btnHour]", function () {
            let ShiftType = $("[id$=ddlShiftType]").val();
            let shiftID = $("[id$=ddlShiftID]").val();
             let ShiftName = $("[id$=txtShiftName]").val();
             PopupCenter("HourDefinitionKTA.aspx?ShiftID=" + shiftID + "&ShiftName=" + ShiftName + "&ShiftType=" + ShiftType, "Shift Working Hour", 1300, 500);
            return false;
        })

        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('deleteConfirmModal');
            }

         function CloseDeleteConfirmModal() {
            $('#deleteConfirmModal').modal('hide');
         }

        function ShiftTypeValidation() {
                if ($('#txtShiftType').val() == "" || $('#txtShiftType').val() == null) {
                    toasterWarningMsg("Shift Type is required.", "");
                return false;
            }
         }

        function PopupCenter(url, title, w, h) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=yes,toolbar=no,resizable=yes,width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
