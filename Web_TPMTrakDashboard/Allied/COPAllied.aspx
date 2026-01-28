<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="COPAllied.aspx.cs" Inherits="Web_TPMTrakDashboard.Allied.COPAllied" culture="auto" EnableEventValidation="false" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <style>
        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        .createhyperlink {
            text-decoration: underline;
            color: blue;
            cursor: pointer;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            padding: 4px;
        }

        td {
            text-align: center;
            width: 65px;
            height: 20px;
            padding: 2px;
        }

        th {
            text-align: left;
            /*height:20px;*/
        }

        #MainContent_chkFinishOperation {
            vertical-align: -webkit-baseline-middle;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 795px;
        }


        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 159px;
        }


        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
        }

        .setminwidth {
            min-width: 60px;
        }

        .hover_row {
            background-color: snow;
        }
        /* #componentgrd tr:hover {
            background-color: red !important;
        }*/

        /*  #componentgrd tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #componentgrd tr:nth-child(even) {
            background-color: #DCDCDC;
        }
*/
        .ascendingorder {
            background: url("../Image/AscO.jpg") no-repeat scroll right center;
        }

        .descendingorder {
            background: url("../Image/DescO.jpg") no-repeat scroll right center;
        }

        #updateStdTimeModal table tr td:nth-child(2) {
            max-width: 100px;
        }

        #lbUpdateTimeMachine button {
            width: 100px;
            overflow: auto;
        }
    </style>

    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />

            <div id="COPContainer" class="row" style="overflow: auto">
                <div style="text-align: center">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; color: green; font-size: x-large;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>
                <div class="col-md-2">
                    <div>
                        <asp:TextBox ID="txtsearch" class="form-control " Style="margin-top: 9px;" CssClass="searchdata form-control" runat="server" onkeydown="return (event.keyCode!=13);" meta:resourcekey="txtsearchResource1"></asp:TextBox>
                    </div>

                    <div>
                        <div style="height: 752px; overflow: auto">
                            <asp:GridView ID="ComponentIdgrd" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" EmptyDataText="<%$ Resources:CommanResource,Nodataavailable %>" ShowHeaderWhenEmpty="True" BackColor="#F2F2F2" meta:resourcekey="ComponentIdgrdResource1">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource,ComponentID %>" meta:resourcekey="TemplateFieldResource1">

                                        <ItemTemplate>
                                            <u>
                                                <asp:LinkButton ID="btn_componentid" runat="server" ForeColor="#002BC0" Text='<%# Eval("componentid") %>' OnClick="btncomponent_Click" meta:resourcekey="btn_componentidResource1"></asp:LinkButton>
                                            </u>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <HeaderStyle CssClass="blue" Font-Size="13pt" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-md-10" style="height: 760px;">
                    <a class="glyphicon glyphicon-arrow-left" style="font-size: 25px; font-weight: bold" href="../ComponentInformation.aspx"></a>
                    <fieldset class="scheduler-border" id="opinfo">
                        <legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource", "OprInfo") %> &nbsp;</legend>
                        <table class="table table-bordered" id="operationinfo" style="margin-top: -18px;">
                            <thead>

                                <tr>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "OperationNo") %></div>
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtopn_no" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txtopn_noResource1"></asp:TextBox>
                                 
                                    </td>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "PlantID") %></div>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" AutoPostBack="True" meta:resourcekey="ddlPlantIdResource1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Drawing") %></div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:TextBox ID="txtdrawing" class="form-control " runat="server" AutoCompleteType="Disabled" meta:resourcekey="txtdrawingResource1"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div runat="server" id="lblFinishOpr" class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "FinishOperation") %></div>
                                    </td>
                                    <td>
                                        <div style="height: 35px; width: 40px;">
                                            <asp:CheckBox ID="chkFinishOperation" runat="server" meta:resourcekey="chkFinishOperationResource1"></asp:CheckBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "InterfaceID") %></div></div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:TextBox ID="txtinterfaceid" class="form-control" runat="server" onkeypress="return event.charCode >= 48 && event.charCode <= 57" AutoCompleteType="Disabled" meta:resourcekey="txtinterfaceidResource1"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "MachineId") %></div></div>
                                    </td>
                                    <td style="min-width: 140px;">

                                        <div class="row">
                                            <div>
                                                <asp:ListBox ID="ddlMultiDownID" runat="server" SelectionMode="Multiple" CssClass="form-control" meta:resourcekey="ddlMultiDownIDResource1"></asp:ListBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Target") %></div></div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:TextBox ID="txttarget" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txttargetResource1"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td rowspan="2" colspan="2">
                                        <div style="margin-top: 20px; margin-left: 2px; min-width: 145px">
                                            <span style="width: 100%">
                                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,New %>" Style="display: inline-block" class="btn btn-info" ID="btnnew" OnClick="btnnew_Click" meta:resourcekey="btnnewResource1"></asp:Button>&nbsp;
                                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,Save %>" Style="display: inline-block" class="btn btn-info" ID="btnsave" OnClick="btnsave_Click" meta:resourcekey="btnsaveResource1"></asp:Button>
                                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,Delete %>" Style="display: inline-block" class="btn btn-info" ID="btndelete" OnClick="btndelete_Click" OnClientClick="return confirm('Are you sure you want delete');" meta:resourcekey="btndeleteResource1"></asp:Button>
                                            </span>
                                            <br />
                                            <asp:Button runat="server" Text="<%$ Resources:AssignCo %>" Style="margin-top: 10px;" class="btn btn-info" ID="btnassign" OnClick="btnassign_Click" meta:resourcekey="btnassignResource1"></asp:Button>
                                            <input type="button" id="btnUpdateStdTime" runat="server" value='Update Cycle Time' class="btn btn-info btn-sm" onclick=" return getUpdateStdTimes();" style="margin-top: 10px" />
                                        &nbsp;</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Description") %></div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:TextBox ID="txtdescription" class="form-control " runat="server" AutoCompleteType="Disabled" meta:resourcekey="txtdescriptionResource1"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Price") %></div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtprice" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txtpriceResource1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <div class="commontd" style="margin-top: 5px; text-align-last: left;"><%=GetGlobalResourceObject("CommanResource", "SubOpr") %></div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:TextBox ID="txtsubop" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txtsubopResource1"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>


                            </tbody>
                        </table>


                        <fieldset class="scheduler-border1" id="stdtime">
                            <legend runat="server" id="lgStandartTime" class="scheduler-border commontd"></legend>

                            <table class="table table-bordered" id="startedtime" style="margin-top: -24px;">
                                <thead>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;">><%=GetLocalResourceObject("MachiningTime") %></div></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtmachinetime" class="form-control" runat="server" OnTextChanged="txtmachinetime_TextChanged" AutoPostBack="True" onkeypress="return allowDecimal(this,event);" meta:resourcekey="txtmachinetimeResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "CycleTime") %></div></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtCycleTime" class="form-control" runat="server" onkeypress="return allowDecimal(this,event);" meta:resourcekey="txtCycleTimeResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetLocalResourceObject("CuttingTimeThr") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtcuttingtime" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txtcuttingtimeResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetLocalResourceObject("LoadUnload") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtloadunload" class="form-control" runat="server" OnTextChanged="txtloadunload_TextChanged" AutoPostBack="True" onkeypress="return allowDecimal(this,event);" meta:resourcekey="txtloadunloadResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetLocalResourceObject("LoadUnloadThr") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtloadunloadthreshold" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txtloadunloadthresholdResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="margin-top: 5px;" class="commontd">
                                                <asp:Label ID="lblstdTime" runat="server" Text="<%$ Resources:StdSetupSec %>" meta:resourcekey="lblstdTimeResource1"></asp:Label>
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtsetuptime" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txtsetuptimeResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdMinLULThreHeader" runat="server">
                                            <div class="commontd" style="margin-top: 5px;">Min. LUL Threshold</div>
                                        </td>
                                        <td id="tdMinLULThreContent" runat="server">
                                            <div>
                                                <asp:TextBox ID="txtminLoadUnloadThreshold" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" meta:resourcekey="txtminLoadUnloadThresholdResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;">Std. Test Pressure</div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtStdTestPressure" class="form-control " runat="server"  onkeypress="return allowDecimal(this,event);" meta:resourcekey="txtStdTestPressureResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                         <td>
                                            <div class="commontd" style="margin-top: 5px;">Std. Holding Time</div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtStdHoldingTime" class="form-control " runat="server"  onkeypress="return allowDecimal(this,event);" meta:resourcekey="txtStdHoldingTimeResource1"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </thead>
                            </table>
                        </fieldset>



                        <div style="height: 450px; overflow: auto;">


                            <asp:GridView ID="componentgrd" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="True" BackColor="White" OnSelectedIndexChanged="componentgrd_SelectedIndexChanged" OnRowDataBound="OnRowDataBound" AllowSorting="True" OnSorting="componentgrd_Sorting" ClientIDMode="Static" meta:resourcekey="componentgrdResource1">
                                <AlternatingRowStyle BackColor="#F2F2F2" />

                                <Columns>

                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource,OperationNo %>" SortExpression="operationno" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl" runat="server" Text='<%# Eval("operationno") %>' meta:resourcekey="lblResource1"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="<%$ Resources:CommanResource,Description %>" DataField="description" SortExpression="description" meta:resourcekey="BoundFieldResource1" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:CommanResource,MachineId %>" DataField="machineid" SortExpression="machineid" meta:resourcekey="BoundFieldResource2" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:MachiningTime %>" DataField="MachiningTime" SortExpression="MachiningTime" meta:resourcekey="BoundFieldResource3" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:CommanResource,CycleTime %>" DataField="cycletime" SortExpression="cycletime" meta:resourcekey="BoundFieldResource4" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:LoadUnload %>" DataField="LoadUnloadThreshold" SortExpression="LoadUnloadThreshold" meta:resourcekey="BoundFieldResource5" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:StdSetupTime %>" DataField="StdSetupTime" SortExpression="StdSetupTime" meta:resourcekey="BoundFieldResource6" >
                                    <ControlStyle Width="200px" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:CuttingTime %>" DataField="MachiningTimeThreshold" SortExpression="CuttingTime" meta:resourcekey="BoundFieldResource7" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Min. LUL Threshold" DataField="MinLoadUnloadThreshold" SortExpression="MinLoadUnloadThreshold" meta:resourcekey="BoundFieldResource8" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:TargetPrc %>" DataField="TargetPercent" SortExpression="TargetPercent" meta:resourcekey="BoundFieldResource9" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:CommanResource,Drawing %>" DataField="drawingno" SortExpression="drawingno" meta:resourcekey="BoundFieldResource10" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:CommanResource,FinishOperation %>" DataField="FinishedOperation" SortExpression="FinishedOperation" meta:resourcekey="BoundFieldResource11" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:UpdatedBy %>" DataField="UpdatedBy" SortExpression="UpdatedBy" meta:resourcekey="BoundFieldResource12" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:UpdatedTS %>" DataField="UpdatedTS" SortExpression="UpdatedTS" meta:resourcekey="BoundFieldResource13" >
                                    <ControlStyle Width="200px" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Std. Test Pressure" DataField="StdTestPressure" SortExpression="StdTestPressure" meta:resourcekey="BoundFieldResource14" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                     <asp:BoundField HeaderText="Std. Holding Time" DataField="StdHoldingTime" SortExpression="StdHoldingTime" meta:resourcekey="BoundFieldResource15" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle Font-Bold="True" ForeColor="White" CssClass="blue" Font-Italic="True" />
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="modal fade" id="updateStdTimeModal" role="dialog">
                <div class="modal-dialog modal-md">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header bg-primary text-center">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Update Standard Times</h4>
                        </div>

                        <table class="table table-bordered">
                            <tr>
                                <td>
                                    <label class="control-label"><%=GetGlobalResourceObject("CommanResource","MachineId") %></label></td>
                                <td>
                                    <asp:ListBox ID="lbUpdateTimeMachine" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static" meta:resourcekey="lbUpdateTimeMachineResource1"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Component ID</label></td>
                                <td>
                                    <asp:Label ID="lblComponentID" runat="server" meta:resourcekey="lblComponentIDResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Operation No.</label></td>
                                <td>
                                    <asp:Label ID="lblOperationNo" runat="server" meta:resourcekey="lblOperationNoResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Std. Cycle Time (<span id="stdCycleTime"></span>)</label></td>
                                <td>
                                    <asp:TextBox ID="txtStdCycleTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std Cycle Time" MaxLength="7" meta:resourcekey="txtStdCycleTimeResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Std. Load Time (<span id="stdLoadTime"></span>)</label></td>
                                <td>
                                    <asp:TextBox ID="txtStdLoadTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std Load Time" MaxLength="7" meta:resourcekey="txtStdLoadTimeResource1"></asp:TextBox>
                                </td>
                            </tr>
                        </table>

                        <div class="modal-footer text-center">
                            <asp:Button runat="server" ID="btnStdTimeUpdateConfirm" Text="Update Time" CssClass="btn btn-primary" OnClientClick="return updateStandardTime();" OnClick="btnStdTimeUpdateConfirm_Click" meta:resourcekey="btnStdTimeUpdateConfirmResource1" />

                            <button type="button" class="btn btn-primary" data-dismiss="modal"><%=GetGlobalResourceObject("CommanResource","btnCancel") %></button>
                        </div>
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        var bigDiv = document.getElementById('COPContainer');
        $(document).ready(function () {
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbUpdateTimeMachine]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=txtsearch]').keyup(function (event) {

                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
                else {
                    searchTable($(this).val());
                }
            });
            setTextBoxNumericClass();
        });
        function setTextBoxNumericClass() {
            //$.ajax({
            //    type: "POST",
            //    url: "ComponentOperationInformation.aspx/getTimeFormat",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (response) {
            //        var timeFormat = response.d;
            //        var className = "allowDecimal";
            //        if (timeFormat == "ss") {
            //            className = "allowNumeric";
            //        }
            //        $('#txtmachinetime').addClass(className);
            //        $('#txtCycleTime').addClass(className);
            //        $('#txtcuttingtime').addClass(className);
            //        $('#txtloadunload').addClass(className);
            //    },
            //    error: function (jqXHR, textStatus, err) {
            //        alert('Error: ' + err);
            //    }
            //});
        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollLeft);
            console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
        }
        window.onload = function () {
            bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
        }
        function getUpdateStdTimes() {
            if ($("[id$=txtsearch]").val() == null || $("[id$=txtsearch]").val() == "") {
                alert("Please select Component ID.");
                return false;
            }
            $.ajax({
                type: "POST",
                url: "ComponentOperationInformation.aspx/GetUpdateStdTimes",
                contentType: "application/json; charset=utf-8",
                data: '{machineId:"' + $("[id$=ddlMultiDownID]").val() + '", component:"' + $("[id$=txtsearch]").val() + '", operation:"' + $("[id$=txtopn_no]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemdata = response.d;
                    $("[id$=lblComponentID]").text($("[id$=txtsearch]").val());
                    $("[id$=lblOperationNo]").text($("[id$=txtopn_no]").val());
                    if (itemdata != null) {
                        $("[id$=txtStdLoadTime]").removeAttr("onkeypress");
                        $("[id$=txtStdLoadTime]").removeAttr("onkeypress");
                        if (itemdata.TimeFormat == "Standard Time (in seconds)") {
                            $("#stdCycleTime").html('in seconds');
                            $("#stdLoadTime").html('in seconds');
                            $("[id$=txtStdLoadTime]").attr("onkeypress", "return allowNumber(event);");
                            $("[id$=txtStdCycleTime]").attr("onkeypress", "return allowNumber(event);");


                        } else {
                            $("#stdCycleTime").html('in minutes');
                            $("#stdLoadTime").html('in minutes');
                            $("[id$=txtStdLoadTime]").attr("onkeypress", "return allowDecimal(this,event);");
                            $("[id$=txtStdCycleTime]").attr("onkeypress", "return allowDecimal(this,event);");
                        }
                        $("[id$=txtStdLoadTime]").val(itemdata.LoadUnLoad);
                        $("[id$=txtStdCycleTime]").val(itemdata.CycleTime);
                        //var machineul = $('#lbUpdateTimeMachine').closest('.multiselect-native-select').find('ul.dropdown-menu')[0];
                        //$(machineul).empty();
                        //var machinelist = itemdata.MachineId;
                        //if (machinelist != null) {
                        //    var appendStr = "";
                        //    for (var i = 0; i < machinelist.length; i++) {
                        //        appendStr += '<li><a tabindex="0"><label class="checkbox"><input type="checkbox" value="' + machinelist[i] + '">' + machinelist[i] + '</label></a></li>';
                        //    }
                        //    $(machineul).append(appendStr);
                        //}
                    }
                    $('#updateStdTimeModal').modal('show');
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
            return false;
        }
        function updateStandardTime() {
            if ($("[id$=txtsearch]").val() == null || $("[id$=txtsearch]").val() == "") {
                alert("Please select Component ID.");
                return false;
            }
            if ($("[id$=txtStdCycleTime]").val() == "" || $("[id$=txtStdCycleTime]").val() == "0") {
                alert("Please Enter Std. Cycle Time Value greater than zero.");
                $("[id$=txtStdCycleTime]").focus();
                return false;
            }
            if ($('#lbUpdateTimeMachine').val() == null || $('#lbUpdateTimeMachine').val() == "") {
                alert("Please select Machine ID.");
                $("[id$=lbUpdateTimeMachine]").focus();
                return false;
            }
             <%--if ($("[id$=txtStdLoadTime]").val() == "" || $("[id$=txtStdLoadTime]").val() == "0") {
                alert("<%=GetLocalResourceObject("PleaseEnterStdLoadTimeValuegreaterthanzero")%>");
                $("[id$=txtStdLoadTime]").focus();
                return false;
            }--%>
            var result = "";
            $.ajax({
                async: false,
                type: "POST",
                url: "ComponentOperationInformation.aspx/updateStandardTimeData",
                contentType: "application/json; charset=utf-8",
                data: '{machineId:"' + $('#lbUpdateTimeMachine').val() + '", component:"' + $("[id$=lblComponentID]").text() + '", operation:"' + $("[id$=lblOperationNo]").text() + '", stdCycleTime:"' + $("[id$=txtStdCycleTime]").val() + '", stdLoadTime:"' + $("[id$=txtStdLoadTime]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    result = response.d;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
            if (result == "Successfull") {
                alert("Data Updated Successfully");
                $('#updateStdTimeModal').modal('hide');
                return true;
            }
            else {
                alert("Data Updation Failed");
                return false;
            }
        }

        function searchTable(inputVal) {

            var table = $('[id$=ComponentIdgrd]');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).text())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }

        function ShowFinishOprWarningConfirmation() {
            return confirm("This operation is already finished for this part. Do you want to refinish it ?");
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            var bigDiv = document.getElementById('COPContainer');
            bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollLeft);
                console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
            }
            window.onload = function () {
                bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
            }
            setTextBoxNumericClass();
            $("[id*=componentgrd] td").hover(function () {
                let row = $(this).closest("tr")[0];
                if ($(row).attr("rowselected") != "true") {

                    $("td", $(this).closest("tr")).addClass("hover_row");
                }
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });

            $('[id$=txtsearch]').keyup(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
                else {
                    searchTable($(this).val());
                }
            });

            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbUpdateTimeMachine]').multiselect({
                includeSelectAllOption: true
            });
            function ShowFinishOprWarningConfirmation() {
                return confirm("This operation is already finished for this part. Do you want to refinish it ?");
            }
            function allowDecimal(txt, evt) {
                //evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                var ptpos = $(txt).val().indexOf(".");
                debugger;
                if (pos >= ptpos) {
                    var afterdecimalpt = $(txt).val().split('.')[1];
                    if (afterdecimalpt != undefined) {
                        if (afterdecimalpt.length > 2) {
                            return false;
                        }
                    }
                }
                if (charCode == 45 && pos != 0) {
                    return false;
                } else if (charCode == 43 && pos != 0) {
                    return false;
                } else if (charCode == 46 && $(txt).val().indexOf('.') != -1) {
                    return false;
                } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            };
            function allowNumber(evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            $('.numbersOnly').keypress(function (evt) {
                debugger;

            });
        });
        $("[id*=componentgrd] td").hover(function () {
            let row = $(this).closest("tr")[0];
            if ($(row).attr("rowselected") != "true") {

                $("td", $(this).closest("tr")).addClass("hover_row");
            }
        }, function () {
            $("td", $(this).closest("tr")).removeClass("hover_row");
        });
        function allowDecimal(txt, evt) {
            // evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            var ptpos = $(txt).val().indexOf(".");
            debugger;
            if (pos >= ptpos) {
                var afterdecimalpt = $(txt).val().split('.')[1];
                if (afterdecimalpt != undefined) {
                    if (afterdecimalpt.length > 2) {
                        return false;
                    }
                }
            }
            if (charCode == 45 && pos != 0) {
                return false;
            } else if (charCode == 43 && pos != 0) {
                return false;
            } else if (charCode == 46 && $(txt).val().indexOf('.') != -1) {
                return false;
            } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        };
        function allowNumber(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if ((charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        $("[id$=txtmachinetime]").focusout(function () {

            var machinetime = $("[id$=txtmachinetime]").val();
            if (isNaN(machinetime)) {
                $("[id$=txtmachinetime]").val("");
                alert("<%=GetLocalResourceObject("MachineTimemustbeanumber")%>")
            }

        });

        $('[id$=txtsubop]').keyup(function () {
            debugger;
            var value = $(this).val();
            //value = $('id$=ctl00$MainContent$txtsubop]')[0].value();
            console.log(value);
            if (value > 1) {

                if (confirm("Sub Operation is more than 1. is it right ?\n 'Press Ok to accept or press cancel to correct it'")) {
                    once = false;
                    $(this).val(value)
                }
                else {
                    ($(this).val("1"));
                }

            }
        });
        //$("[id$=txtloadunload]").focusout(function () {
        //    
        //    var loadUnload = $("[id$=txtloadunload]").val();
        //    if (isNaN(loadUnload)) {
        //        $("[id$=txtloadunload]").val() = "";
        //        alert("LoadUnload Time must be a number.")
        //    }
        //    else {
        //        $("[id$=txtCycleTime]").val(parseInt($("[id$=txtmachinetime]").val()) + parseInt($("[id$=txtloadunload]").val()));
        //    }
        //});
    </script>
    <script>
        function ChkAddClass() {
            $("#Scomponentgrd tr th").addClass("bothorder");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
