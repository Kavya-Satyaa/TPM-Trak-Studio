<%@ Page Title="Coolant Top-Up Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CoolantTopUpMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.CoolantTopUpMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
       /* #tblFilter > tbody> tr > td{
            color:white;
        }*/
          #lvCoolantTopUpTbl th {
            color: white;
            background-color: #2E6886 !important;
        }
           #lvCoolantTopUpTbl tr td{

           }

        #lvCoolantTopUpTbl tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #lvCoolantTopUpTbl tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }
        .filterTxt{
            color:white;
            vertical-align:middle !important;
        }
    </style>
    <div class="row">
        <div class="col-lg-12">
            <div>
                 <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered" >
                            <tr>
                                <td class="filterTxt">Plant ID
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlantId" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="filterTxt">Cell ID
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCellID" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="filterTxt">Machine ID
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" style="width:auto"></asp:DropDownList>
                                </td>
                                <td class="filterTxt">From Date
                                </td>
                                <td class="input-group" style="min-width: 150px;">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="min-width: 110px; width:110px;  min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td class="filterTxt">To Date
                                </td>
                                <td class="input-group" style="min-width: 150px;">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" Style="min-width: 110px;width:110px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                               
                            </tr>
                            <tr>
                                 <td colspan="4">
                                    <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                                    <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-info" OnClick="btnNew_Click" />
                                     <asp:Button runat="server" ID="btnInsert" Text="Add" CssClass="btn btn-info" OnClick="btnInsert_Click" />
                                     <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-info" OnClick="btnCancel_Click" />
                                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" />
                                    <asp:Button runat="server" ID="btnReport" Text="Export" CssClass="btn btn-info" OnClick="btnReport_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnReport" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div id="gridContainer" style="height:78vh;overflow:scroll">
                <asp:ListView runat="server" ID="lvCoolantTopUp"  class="table table-bordered table-hover" OnItemCreated="lvCoolantTopUp_ItemCreated" OnItemInserting="lvCoolantTopUp_ItemInserting" InsertItemPosition="LastItem">
                    <LayoutTemplate>
                        <table class="table table-bordered headerFixer" id="lvCoolantTopUpTbl">
                            <tr>
                                <%--<th>Plant</th>--%>
                                <th>Cell ID</th>
                                <th>Machine ID</th>
                                <th>Operator</th>
                                <th>Top-Up (Liter)</th>
                                <th>Top-Up Datetime</th>
                                <th>Remarks</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                          <%--  <td>
                                <asp:Label runat="server" ID="lblPlantId" Text='<%# Eval("PlantID") %>'></asp:Label>
                            </td>--%>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnPlant"  Value='<%# Eval("PlantID") %>'/>
                                <asp:Label runat="server" ID="lblCellID" Text='<%# Eval("CellID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOperator" Text='<%# Eval("Operator") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblTopUpValue" Text='<%# Eval("TopUpValue") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblTopUpDatetime" Text='<%# Eval("TopUpDatetime") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRemarks" Text='<%# Eval("Remarks") %>' CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        <tr>
                           <%-- <td>
                                <asp:DropDownList runat="server" ID="ddlPlantIdNew" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantIdNew_SelectedIndexChanged"></asp:DropDownList>
                            </td>--%>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCellNew" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCellNew_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlMachineNew" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlOperatorNew" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTopUpNew" AutoCompleteType="Disabled" CssClass="form-control allowDecimal"></asp:TextBox>
                            </td>
                            <td class="input-group" style="min-width: 220px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtTopUpDateTimeNew" runat="server" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtremarksNew" CssClass="form-control" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                    </InsertItemTemplate>
                </asp:ListView>
                             </div>
                         </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
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
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {

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
            return true;
        });
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
        }
        window.onload = function () {
            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MM-YYYY',
            locale: 'en-US'
        });
        $('[id$=txtToDate]').datetimepicker({
            format: 'DD-MM-YYYY',
            locale: 'en-US'
        });
        $('[id$=txtTopUpDateTimeNew]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm:ss',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
                console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
            }
            window.onload = function () {
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
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
                return true;
            });
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtTopUpDateTimeNew]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
