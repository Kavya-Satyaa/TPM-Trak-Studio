<%@ Page Title="JH Production Head" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JHProductionHeadObservation.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.JHProductionHeadObservation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>

    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    
     <script src="../Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="../Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <style type="text/css">
        .headerFixerTable tr th {
            position: sticky;
            top: -1px;
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
            width: 100%;
            overflow: auto;
            margin-top: 15px;
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
            height: 45px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }


        .table .lbl {
            padding-top: 15px;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

       #gridContainer input[type="checkbox"] {
            width: 30px;
            height: 30px;
        }

        #tblProdHeadData tr td {
            color: white;
        }
        .multiselect.dropdown-toggle.btn{
            width:250px;
            overflow:hidden;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnEmpRole" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>


            <table id="tblfilter" class="table table-bordered" style="width: auto;">
                <tr>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Plant</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" Width="160" Style="height: 40px" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"  ></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Cell</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control"  Width="160" Style="height: 40px" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged" AutoPostBack="true"   ></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Machine</td>
                    <td style="min-width: 160px;">
                    <asp:ListBox ID="ddlMachineId" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource, MachineTooltip %> " Width="150" ClientIDMode="Static"  ></asp:ListBox>
                     
                    </td>
                    <td class="commanTd" style="width: 70px;">Shift</td>
                    <td style="min-width: 160px;">
                      <asp:ListBox ID="ddlShift" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource,ShiftTooltip %>" Width="150" ClientIDMode="Static"  ></asp:ListBox>
                    </td>
                    <td class="commanTd" style="width: auto;">JH Type</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList ID="ddlJHType" runat="server" CssClass="form-control" Style="height: 40px">
                            <asp:ListItem Value="" Text="All" />
                            <asp:ListItem Value="Auto" Text="JH Auto" />
                            <asp:ListItem Value="Manual" Text="JH Manual" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="commanTd" style="width: auto;">Month / Year</td>
                    <td class="" style="min-width: 150px; border: 0">

                      <%--  <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>--%>

                          <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                        <span class="commanTd" style="font-size:22px">/</span>
                         <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>

                    </td>
                  
                    <td style="text-align: center; width: auto; border-right: none"></td>
                    <td style="text-align: center; width: auto; border-left: none; border-right: none">
                        <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>
                        <asp:Button runat="server" Text="Save" CssClass="btn btn-info btn-sm displayCss" ID="btnSave" OnClick="btnSave_Click"></asp:Button>
                         &nbsp; &nbsp; &nbsp; &nbsp;
                          <asp:Button runat="server" Text="Email Report" CssClass="btn btn-info btn-sm displayCss" ID="btnEmail"  OnClientClick="return openEmailConfirmModal();" ></asp:Button>
                    </td>
                    <td style="border-left: none"></td>
                </tr>
            </table>
            <div style="display: flex; justify-content: center; align-content: center;">
                <div id="gridContainer" class="divGrid" style="">
                    <asp:ListView runat="server" ID="lvProdHeadData">
                        <LayoutTemplate>
                            <table id="tblProdHeadData"  class="table table-bordered cockpit headerFixerTable" runat="server" clientidmode="static">
                                <tr>
                                    <th>Date</th>
                                    <th>Machine ID</th>
                                    <th>Shiftwise</th>
                                    <th>JH Type</th>
                                    <th>Operator Status</th>
                                    <th>Supervisor</th>
                                    <th>Production Head</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td  style="display:<%# Eval("CellVisibility1") %>"  rowspan='<%# Eval("RowSpan") %>'><asp:Label runat="server" ID="lblDate" Text='<%# Eval("Date") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnAuditDate" Value='<%# Eval("AuditDate") %>' />
                                    <asp:HiddenField runat="server" ID="hdnNoOoSelectedShift" Value='<%# Eval("RowSpan") %>' />
                                </td>
                                <td  style="display:<%# Eval("CellVisibility1") %>"   rowspan='<%# Eval("RowSpan") %>' ><asp:Label runat="server" ID="lblMachineID"  Text='<%# Eval("Machine") %>'></asp:Label></td>
                                <td><asp:Label runat="server" ID="lblShift"  Text='<%# Eval("Shift") %>'></asp:Label></td>
                                <td><asp:Label runat="server" ID="lblJHType"  Text='<%# Eval("JHType") %>'></asp:Label></td>
                                <td><asp:Label runat="server" ID="lblOperatorStatus"  Text='<%# Eval("OperatorStatus") %>'></asp:Label></td>
                                <td><asp:Label runat="server" ID="lblSupStatus"  Text='<%# Eval("SupervisorStatus") %>'></asp:Label></td>
                                <td style="display:<%# Eval("ChkBoxVisibility") %>;text-align:center"  rowspan='<%# Eval("ChkRowSpan") %>' >
                                     <asp:HiddenField runat="server" ID="hdnProdHeadUpdate" ClientIDMode="Static" />
                                    <asp:CheckBox runat="server" ID="chkProdHeadObs" Checked='<%# Eval("ProdHeadObservation") %>' onclick="productionHeadChkChange(this)"/>
                                </td>
                               
                            </tr>
                        </ItemTemplate>
                          <EmptyDataTemplate>
                            <p style="font-size: 14px;color: black;background-color: white;padding: 3px;">No Data Found</p>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <div class="modal fade" id="emailConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922;background-color:white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Do you want to send Email?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                  <asp:Button runat="server" ID="sendEmail"  Style="width: 80px;" Text="Yes"  OnClick="sendEmail_Click"/>
                     <button  type="button" id="approveNo" Style="width: 80px;" data-dismiss="modal" >No</button>
                </div>
            </div>
        </div>
    </div>
   <script type="text/javascript">

        $(document).ready(function () {
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlShift]').multiselect({
                includeSelectAllOption: true
            });

            //$('[id$=txtFromDate]').datetimepicker({
            //    format: 'DD-MM-YYYY',
            //    locale: 'en-US'
            //});
            //$('[id$=txtToDate]').datetimepicker({
            //    format: 'DD-MM-YYYY',
            //    useCurrent: false,
            //    locale: 'en-US'
            //});
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            var Height = $(window).height() - (220);
            $('#gridContainer').css('height', Height);
        });
       var bigDiv = document.getElementById('gridContainer');
       bigDiv.onscroll = function () {
           $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
           console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
       }
       window.onload = function () {

           bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
           console.log("id load =" + bigDiv.scrollTop);
       }
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
        function remarksChange(txt) {
            $(txt).closest('td').find('#hdnRemarksUpdate').val("update");
        }
        function supervisorChkChange(chk) {
            debugger;
            $(chk).closest('td').find('#hdnSupUpdate').val("update");
        }
        function productionHeadChkChange(chk) {
            debugger;
            $(chk).closest('td').find('#hdnProdHeadUpdate').val("update");
        }
        $(window).resize(function () {
            var Height = $(window).height() - (220);
            $('#gridContainer').css('height', Height);
        });

        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("lblMessages").style.display = "none";
            }, 2000);

       };
       function openEmailConfirmModal() {
           $('[id*=emailConfirmModal]').modal('show');
           return false;
       }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(function () {
           var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                $('[id$=ddlMachineId]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=ddlShift]').multiselect({
                    includeSelectAllOption: true
                });

                //$('[id$=txtFromDate]').datetimepicker({
                //    format: 'DD-MM-YYYY',
                //    locale: 'en-US'
                //});
                //$('[id$=txtToDate]').datetimepicker({
                //    format: 'DD-MM-YYYY',
                //    useCurrent: false,
                //    locale: 'en-US'
                //});
                $("[id$=txtFromDate]").on("dp.change", function (e) {
                    $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
                });
                $("[id$=txtToDate]").on("dp.change", function (e) {
                    $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
                });

                var Height = $(window).height() - (220);
                $('#gridContainer').css('height', Height);
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
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
           $(window).resize(function () {
               var Height = $(window).height() - (220);
               $('#gridContainer').css('height', Height);
           });
       });
       bigDiv.onscroll = function () {
           $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
           console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
       }
       window.onload = function () {

           bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
           console.log("id load =" + bigDiv.scrollTop);
       }

   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
