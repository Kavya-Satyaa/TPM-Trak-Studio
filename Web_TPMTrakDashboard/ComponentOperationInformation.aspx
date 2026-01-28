<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComponentOperationInformation.aspx.cs" Inherits="Web_TPMTrakDashboard.ComponentOperationInformation" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <link href="MyCssAndJS/SearcableDropDown/select2.min.css" rel="stylesheet" />
    <script src="MyCssAndJS/SearcableDropDown/select2.min.js"></script>
    <style>
        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
                padding: 5px;
                text-align: center;
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
            /*width: 65px;*/
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
            /*padding: 0.1em 0.5em 1.1em !important;*/
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 0.9em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            /*height: 159px;*/
            padding: 0px 10px 10px 10px;
            width: max-content;
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
            margin-bottom: 5px;
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

        .timecontrols {
            /* width: 100px;*/
        }

        #startedtime {
            width: max-content;
        }

            #startedtime tr td {
                border: none;
                text-align: left;
                padding: 2px;
            }

        #operationinfo tr td {
            border: none;
            padding: 2px;
        }

        .td-header {
            width: 135px;
            text-align: left !important;
            padding-left: 10px !important;
        }

        .td-content {
            width: 200px;
            text-align: left !important;
        }

        .tdComp .select2.select2-container {
            width:250px !important;
        }

        .select2-container .select2-selection--single {
            height: 35px !important;
        }

        .select2-container--default .select2-selection--single .select2-selection__clear {
            height: 35px !important;
            position: relative;
            top: -3px;
        }

        .select2-container .select2-selection--single .select2-selection__rendered {
            padding-top: 2px;
        }

        #componentgrd tr th {
            white-space: unset !important;
        }

        #componentgrd tr td {
            width: 100px;
        }

         #updateStdTimeModal table tr td{
             text-align: left !important;
         }
         #MainContent_lstboxCell .multiselect{
             width:200px;
         }
    </style>

    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnScrollPosTop" ClientIDMode="Static" />
            <table style="width: 65%;">
                <tr>
                    <td>
                        <a class="glyphicon glyphicon-arrow-left" style="font-size: 25px; font-weight: bold" onclick="goToCompPage();"></a>&nbsp;
                    </td>
                    <td class="commontd" style="white-space: nowrap; padding-left: 20px;">Component ID 
                    </td>
                    <td class="tdComp">
                        <asp:DropDownList runat="server" ID="ddlComponentID" ClientIDMode="Static" CssClass="form-control searchable-dropdown-list" Style="width: auto;"></asp:DropDownList>

                        <asp:Button runat="server" ID="btnView" ClientIDMode="Static" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                    </td>
                    <td style="white-space: nowrap; padding-left: 40px;" runat="server" id="tdOperations">
                        <asp:Button runat="server" Text="<%$Resources:CommanResource,New %>" Style="display: inline-block;background-color: #2c8fad !important" class="btn btn-info" ID="btnnew" OnClick="btnnew_Click"></asp:Button>

                        <asp:Button runat="server" Text="<%$Resources:CommanResource,Save %>" Style="display:  inline-block;background-color: #2c8fad !important" class="btn btn-info" ID="btnsave" OnClick="btnsave_Click"></asp:Button>

                        <asp:Button runat="server" Text="<%$Resources:CommanResource,Delete %>" Style="display: inline-block;background-color: #eb3d3d !important" class="btn btn-info" ID="btndelete" OnClick="btndelete_Click" OnClientClick="return confirm('Are you sure you want delete');"></asp:Button>
                    </td>
                    <td style="white-space: nowrap; padding-left: 40px;" runat="server" id="tdAssignCO">
                        <asp:Button runat="server" Text="<%$Resources:AssignCo %>" class="btn btn-info" ID="btnassign" OnClick="btnassign_Click"></asp:Button>

                        <input type="button" id="btnUpdateStdTime" runat="server" value='Update Cycle Time' class="btn btn-info" onclick=" return getUpdateStdTimes();" />

                        <asp:Button runat="server" Text="<%$Resources:CommanResource,CreateFolder %>" class="btn btn-info" ID="btnCreateFolder" OnClick="btnCreateFolder_Click"></asp:Button>
                    </td>
                </tr>
            </table>
            <div id="COPContainer" class="row" style="overflow: auto">
                <div style="text-align: center">
                    <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri; color: green; font-size: x-large;"></asp:Label>
                </div>
                <%--  <div class="col-md-2">
                    <div>--%>
                <%--  <asp:TextBox ID="txtsearch" class="form-control " Style="margin-top: 9px;" title="<%$Resources:CommanResource,SearchHere %>" placeholder="<%$Resources:SearchComp %>" CssClass="searchdata form-control" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                <%--<input type="text" id="search" data-toggle="tooltip" title="search here !" placeholder="search Component here..." class="searchdata form-control " />--%>
            </div>

            <div style="margin-top: 10px;">
                <%--<table id="tblcomponentinfo" class="table table-bordered table-hover">
                    <thead class="blue">
                        <tr>
                            <th>Component ID</th>
                        </tr>
                    </thead>
                </table>--%>
                <%--  <div style="height: 752px; overflow: auto">
                            <asp:GridView ID="ComponentIdgrd" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                HeaderStyle-Font-Size="13" EmptyDataText="<%$Resources:CommanResource,Nodataavailable %>" ShowHeaderWhenEmpty="true" HeaderStyle-CssClass="blue" AlternatingRowStyle-BackColor="#FFFFFF" BackColor="#F2F2F2">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$Resources:CommanResource,ComponentID %>">

                                        <ItemTemplate>
                                            <u>
                                                <asp:LinkButton ID="btn_componentid" runat="server" ForeColor="#002BC0" Text='<%# Eval("componentid") %>' OnClick="btncomponent_Click" OnClientClick="ShowLoader();"></asp:LinkButton>
                                            </u>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>--%>
                <%--  </div>
                </div>--%>
                <%--   <div class="col-md-10" style="height: 760px;">--%>
                <%--<a class="glyphicon glyphicon-arrow-left" style="font-size:25px;font-weight:bold" href="ComponentInformation.aspx"></a>--%>
                <%--  <fieldset class="scheduler-border" id="opinfo">
                        <legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource", "OprInfo") %> - <%=ddlComponentID.SelectedValue %> </legend>--%>
                <fieldset class="scheduler-border1" runat="server" style="display:  <%: admin %>">
                    <legend class="scheduler-border commontd">Operation Information</legend>

                    <table class="table" id="operationinfo" style="margin: 0px; width: max-content;">
                        <%--  class="table table-bordered"--%>
                        <thead>

                            <tr>
                                <%--  <th colspan="7"></th>--%>
                                <%--<th></th>
                             <th></th>
                             <th></th>
                             <th></th>
                             <th></th>--%>
                                <%--<th style="min-width: 175px;"></th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "OperationNo") %></div>
                                </td>
                                <td class="td-content">

                                    <asp:TextBox ID="txtopn_no" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                    <%--  <input type="text" id="opn_no" class="form-control " />--%>
                                 
                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "PlantID") %></div>
                                </td>
                                <td class="td-content">
                                    <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" title="<%$Resources:CommanResource,PlantID %>" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "Drawing") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtdrawing" class="form-control " runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                        <%-- <input type="text" id="txtdrawing" class="form-control " />--%>
                                    </div>
                                </td>
                                <td class="td-header">
                                    <div runat="server" id="lblFinishOpr" class="commontd"><%=GetGlobalResourceObject("CommanResource", "FinishOperation") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:CheckBox ID="chkFinishOperation" runat="server"></asp:CheckBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "InterfaceID") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtinterfaceid" class="form-control" runat="server" onkeypress="return event.charCode >= 48 && event.charCode <= 57" AutoCompleteType="Disabled"></asp:TextBox>
                                        <%-- <input type="text" id="txtinterfaceid" class="form-control " />--%>
                                    </div>
                                </td>
                                <td id="lblCell" runat="server" class="td-header">
                                    <div class="commontd">Cell ID</div>
                                </td>
                                <td id="lstboxCell" runat="server" class="td-content">
                                    <div>
                                        <asp:ListBox ID="ddlMultiCellID" runat="server" SelectionMode="Multiple" CssClass="form-control" OnSelectedIndexChanged="ddlMultiCellID_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                    </div>

                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "MachineId") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:ListBox ID="ddlMultiDownID" runat="server" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                    </div>
                                </td>

                                <%-- <td style="min-width: 140px;">

                                        <div class="row">
                                            <div>
                                                
                                            </div>
                                            <%--  class="col-md-9" <div class="col-md-3">
                                                <label class="checkbox" style="float: right">
                                                    <asp:CheckBox ID="chkall" runat="server" />All</label>
                                            </div>--%>

                                <%-- <div style="float:right">--%>


                                <%-- </div>--%>
                                <%-- </div>
                                    </td>--%>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "Target") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txttarget" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                        <%--   <input type="text" id="txttarget" class="form-control " />--%>
                                    </div>
                                </td>
                                <%--  <td rowspan="3" colspan="2">
                                <div style="margin-top: 20px; margin-left: 2px; min-width: 145px">
                                    <span style="width: 100%">
                                </div>
                            </td>--%>
                            </tr>
                            <tr>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "Description") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtdescription" class="form-control " runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                        <%--  <input type="text" id="txtdescription" class="form-control " />--%>
                                    </div>
                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "Price") %></div>
                                </td>
                                <td class="td-content">
                                    <asp:TextBox ID="txtprice" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "SubOpr") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtsubop" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                        <%--  <input type="text" id="txtsubop" class="form-control " />--%>
                                    </div>
                                </td>

                            </tr>
                            <tr>
                                <td id="tdProcessName" runat="server" class="td-header">
                                    <div class="commontd">Process</div>
                                </td>
                                <td id="tdProcessControl" runat="server" class="td-content">
                                    <asp:DropDownList ID="ddlProcess" runat="server" CssClass="form-control" data-toggle="tooltip">
                                    </asp:DropDownList>
                                </td>
                                <td id="tdInputCodeName" runat="server" class="td-header">
                                    <div class="commontd">Input Code</div>
                                </td>
                                <td id="tdInputCodeControl" runat="server" class="td-content">
                                    <asp:TextBox ID="txtInputCode" class="form-control " runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td id="tdOutputCodeName" runat="server" class="td-header">
                                    <div class="commontd">Output Code</div>
                                </td>
                                <td id="tdOutputCodeControl" runat="server" class="td-content">
                                    <asp:TextBox ID="txtOutputCode" class="form-control " runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td id="tdIsManualOpnName" runat="server" class="td-header">
                                    <div class="commontd">Is Manual Operation?</div>
                                </td>
                                <td id="tdIsManualOpnControl" runat="server" class="td-content">
                                    <asp:CheckBox runat="server" ID="chkIsManualOperation" ClientIDMode="Static" />
                                </td>
                            </tr>

                        </tbody>
                    </table>
                </fieldset>

                <fieldset class="scheduler-border1" id="stdtime">
                    <legend runat="server" id="lgStandartTime" class="scheduler-border commontd" title="<%$Resources:StdTimeSec %>"></legend>

                    <table class="table" id="startedtime" style="margin: 0px;">
                        <thead>
                            <tr>
                                <%-- <th colspan="6"></th>
                                //Standard Time(in Seconds)--%>
                            </tr>
                            <tr>
                                <td class="td-header">
                                    <div class="commontd"><%=GetLocalResourceObject("MachiningTime") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtmachinetime" class="form-control timecontrols" runat="server" OnTextChanged="txtmachinetime_TextChanged" AutoPostBack="true" onkeypress="return allowDecimal(this,event);"></asp:TextBox>
                                        <%--onkeypress="return ((event.charCode >= 48 && event.charCode <= 57)|| event.charCode==46)"--%>
                                        <%-- <input type="text" id="txtmachinetime" class="form-control " />--%>
                                    </div>
                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetGlobalResourceObject("CommanResource", "CycleTime") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtCycleTime" class="form-control timecontrols" runat="server" onkeypress="return allowDecimal(this,event);"></asp:TextBox>
                                        <%--onkeypress="return ((event.charCode >= 48 && event.charCode <= 57)|| event.charCode==46)"--%>
                                        <%--   <input type="text" id="txtCycleTime" class="form-control " />--%>
                                    </div>
                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetLocalResourceObject("CuttingTimeThr") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtcuttingtime" class="form-control timecontrols" runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                        <%--  <input type="text" id="txtcuttingtime" class="form-control " />--%>
                                    </div>
                                </td>
                                 <td class="td-header">
                                    <div style="margin-top: 5px;" class="commontd">
                                       <asp:Label runat="server" Text="<%$Resources:SCIThreshold %>"></asp:Label>
                                    </div>
                                </td>
                                <td class="td-content">
                                    <asp:TextBox runat="server" ID="txtScIThreshold" CssClass="form-control" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-header">
                                    <div class="commontd"><%=GetLocalResourceObject("LoadUnload") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtloadunload" class="form-control timecontrols" runat="server" OnTextChanged="txtloadunload_TextChanged" AutoPostBack="True" onkeypress="return allowDecimal(this,event);"></asp:TextBox>
                                        <%--onkeypress="return ((event.charCode >= 48 && event.charCode <= 57)|| event.charCode==46)"--%>
                                        <%--    <input type="text" id="txtload" class="form-control " />--%>
                                    </div>
                                </td>
                                <td class="td-header">
                                    <div class="commontd"><%=GetLocalResourceObject("LoadUnloadThr") %></div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtloadunloadthreshold" class="form-control timecontrols" runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                        <%--|| event.charCode==46)--%>
                                        <%-- <input type="text" id="txtloadunload" class="form-control " />--%>
                                    </div>
                                </td>
                                <td class="td-header">
                                    <div class="commontd">
                                        <asp:Label ID="lblstdTime" runat="server" Text="<%$Resources:StdSetupTime %>"></asp:Label>
                                        <%--Std. Setup Time(in Seconds)--%>
                                    </div>
                                </td>
                                <td class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtsetuptime" class="form-control timecontrols" runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                        <%--  <input type="text" id="txtsetuptime" class="form-control " />--%>
                                    </div>
                                </td>
                                <td id="tdMinLULThreHeader" runat="server" class="td-header">
                                    <div class="commontd">Min. LUL Threshold</div>
                                </td>
                                <td id="tdMinLULThreContent" runat="server" class="td-content">
                                    <div>
                                        <asp:TextBox ID="txtminLoadUnloadThreshold" class="form-control timecontrols" runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                    </div>
                                </td>
                                 <td class="td-header">
                                    <div style="margin-top: 5px;" class="commontd">
                                       <asp:Label runat="server" Text="<%$Resources:DCLThreshold %>"></asp:Label>
                                    </div>
                                </td>
                                <td class="td-content">
                                    <asp:TextBox runat="server" ID="txtDCLThreshold" CssClass="form-control" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                 <td id="tdIncTime" runat="server" class="td-header">
                                    <div class="commontd">Incentive Time (mins)</div>
                                </td>
                                <td class="td-content">
                                    <asp:TextBox runat="server" ID="txtIncentiveTime" CssClass="form-control" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                </td>
                            </tr>
                        </thead>
                    </table>
                </fieldset>



                <div id="opnContainer" style="height: 50vh; overflow: auto;">
                    <asp:GridView ID="componentgrd" runat="server" AutoGenerateColumns="false" Width="100%"
                         EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true"
                        HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnSelectedIndexChanged="componentgrd_SelectedIndexChanged" OnRowDataBound="OnRowDataBound" AllowSorting="true" OnSorting="componentgrd_Sorting" ClientIDMode="Static" CssClass="headerFixer">
                        <%-- <HeaderStyle HorizontalAlign="Center" OnRowEditing="componentgrd_RowEditing" OnSelectedIndexChanged="componentgrd_SelectedIndexChanged" />--%>

                        <Columns>
                            <%--<asp:HiddenField runat="server" value='<%# Eval("componentgrd.sortdirection") %>'></asp:HiddenField>--%>

                            <asp:TemplateField HeaderText="<%$Resources:CommanResource,OperationNo %>" SortExpression="operationno" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl" runat="server" Text='<%# Eval("operationno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField HeaderText="Operation No" DataField="operationno" HeaderStyle-HorizontalAlign="Center" SortExpression="operationno" />--%>
                            <asp:BoundField HeaderText="<%$Resources:CommanResource,Description %>" HeaderStyle-Wrap="false" DataField="description" HeaderStyle-HorizontalAlign="Center" SortExpression="description" />
                            <asp:BoundField HeaderText="<%$Resources:CommanResource,MachineId %>" HeaderStyle-Wrap="false" DataField="machineid" HeaderStyle-HorizontalAlign="Center" SortExpression="machineid" ItemStyle-Wrap="false" />
                            <asp:BoundField HeaderText="<%$Resources:MachiningTime %>" HeaderStyle-Wrap="false" DataField="MachiningTime" HeaderStyle-HorizontalAlign="Center" SortExpression="MachiningTime" />
                            <asp:BoundField HeaderText="<%$Resources:LoadUnload %>" HeaderStyle-Wrap="false" DataField="LoadUnload" HeaderStyle-HorizontalAlign="Center" SortExpression="LoadUnloadThreshold" />
                            <asp:BoundField HeaderText="<%$Resources:CommanResource,CycleTime %>" HeaderStyle-Wrap="false" DataField="cycletime" HeaderStyle-HorizontalAlign="Center" SortExpression="cycletime" />
                            <asp:BoundField HeaderText="<%$Resources:StdSetupTime %>" HeaderStyle-Wrap="false" ControlStyle-Width="200" DataField="StdSetupTime" HeaderStyle-HorizontalAlign="Center" SortExpression="StdSetupTime" />
                            <asp:BoundField HeaderText="<%$Resources:CuttingTime %>" HeaderStyle-Wrap="false" DataField="MachiningTimeThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="CuttingTime" />
                            <asp:BoundField HeaderText="<%$Resources:LoadunloadThr %>" HeaderStyle-Wrap="false" DataField="LoadUnloadThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="LoadUnloadThreshold" />
                            <asp:BoundField HeaderText="<%$Resources:SCIThreshold %>" HeaderStyle-Wrap="false" DataField="SCIThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="SCIThreshold" />
                             <asp:BoundField HeaderText="<%$Resources:DCLThreshold %>" HeaderStyle-Wrap="false" DataField="DCLThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="DCLThreshold" />
                           <%-- <asp:BoundField HeaderText="Min. LUL Threshold" HeaderStyle-Wrap="false" DataField="MinLoadUnloadThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="MinLoadUnloadThreshold" />--%>
                            <asp:BoundField HeaderText="<%$Resources:TargetPrc %>" HeaderStyle-Wrap="false" DataField="TargetPercent" HeaderStyle-HorizontalAlign="Center" SortExpression="TargetPercent" />
                            <asp:BoundField HeaderText="<%$Resources:CommanResource,Drawing %>" HeaderStyle-Wrap="false" DataField="drawingno" HeaderStyle-HorizontalAlign="Center" SortExpression="drawingno" />
                            <asp:BoundField HeaderText="<%$Resources:CommanResource,FinishOperation %>" HeaderStyle-Wrap="false" DataField="FinishedOperation" HeaderStyle-HorizontalAlign="Center" SortExpression="FinishedOperation" />
                             <asp:BoundField HeaderText="<%$Resources:CommanResource,IncentiveTime %>" HeaderStyle-Wrap="false" DataField="IncentiveTime" HeaderStyle-HorizontalAlign="Center" SortExpression="IncentiveTime" />
                            <asp:BoundField HeaderText="<%$Resources:UpdatedBy %>" HeaderStyle-Wrap="false" DataField="UpdatedBy" HeaderStyle-HorizontalAlign="Center" SortExpression="UpdatedBy" />
                            <asp:BoundField HeaderText="<%$Resources:UpdatedTS %>" HeaderStyle-Wrap="false" ControlStyle-Width="200" DataField="UpdatedTS" HeaderStyle-HorizontalAlign="Center" SortExpression="UpdatedTS" />
                        </Columns>
                        <HeaderStyle Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
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
                                        <asp:ListBox ID="lbUpdateTimeMachine" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Component ID</label></td>
                                    <td>
                                        <asp:Label ID="lblComponentID" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Operation No.</label></td>
                                    <td>
                                        <asp:Label ID="lblOperationNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Std. Machining Time (in seconds)</label></td>
                                    <td>
                                        <asp:TextBox ID="txtStdCycleTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std. Machining Time" MaxLength="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Std. Load Unload Time (<span id="stdLoadTime"></span>)</label></td>
                                    <td>
                                        <asp:TextBox ID="txtStdLoadTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std. Load Unload Time" MaxLength="7"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                            <div class="modal-footer text-center">
                                <asp:Button runat="server" ID="btnStdTimeUpdateConfirm" Text="Update Time" CssClass="btn btn-primary" OnClientClick="return updateStandardTime();" OnClick="btnStdTimeUpdateConfirm_Click" />

                                <button type="button" class="btn btn-primary" data-dismiss="modal"><%=GetGlobalResourceObject("CommanResource","btnCancel") %></button>
                            </div>
                        </div>

                    </div>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="componentgrd" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <script>
        var bigDiv = document.getElementById('opnContainer');
        $(document).ready(function () {
            $('#sidebar').hide();
            $('#main-content').css('margin-left', '0px');
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '200px',
            });

            $('[id$=ddlMultiCellID]').multiselect({
                includeSelectAllOption: true
            });


            $('[id$=lbUpdateTimeMachine]').multiselect({
                includeSelectAllOption: true
            });
            $(".searchable-dropdown-list").select2({
                placeholder: "Search",
                allowClear: false
            });
            //$('[id$=txtsearch]').keyup(function (event) {

            //    if (event.keyCode == 13) {
            //        event.preventDefault();
            //        return false;
            //    }
            //    else {
            //        searchTable($(this).val());
            //    }
            //});
            setTextBoxNumericClass();
        });
        function goToCompPage() {
            window.location.href = "ComponentInformation.aspx";
            window.close();
        }
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
            $('[id*=hdnScrollPosTop]').val(bigDiv.scrollTop);
        }
        window.onload = function () {
            bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
            bigDiv.scrollTop = $('[id*=hdnScrollPosTop]').val();
        }
        function getUpdateStdTimes() {
            if ($("[id$=ddlComponentID]").val() == null || $("[id$=ddlComponentID]").val() == "") {
                alert("Please select Component ID.");
                return false;
            }
            $.ajax({
                type: "POST",
                url: "ComponentOperationInformation.aspx/GetUpdateStdTimes",
                contentType: "application/json; charset=utf-8",
                data: '{machineId:"' + $("[id$=ddlMultiDownID]").val() + '", component:"' + $("[id$=ddlComponentID]").val() + '", operation:"' + $("[id$=txtopn_no]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itemdata = response.d;
                    $("[id$=lblComponentID]").text($("[id$=ddlComponentID]").val());
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
            if ($("[id$=ddlComponentID]").val() == null || $("[id$=ddlComponentID]").val() == "") {
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
        $(".loadData tr").dblclick(function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        })

        function ShowLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }

        function HideLoader() {
            $.unblockUI({});
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            var bigDiv = document.getElementById('opnContainer');
            bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
            bigDiv.scrollTop = $('[id*=hdnScrollPosTop]').val();
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollLeft);
                $('[id*=hdnScrollPosTop]').val(bigDiv.scrollTop);
            }
            window.onload = function () {
                bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
                bigDiv.scrollTop = $('[id*=hdnScrollPosTop]').val();
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

            //$('[id$=txtsearch]').keyup(function (event) {
            //    if (event.keyCode == 13) {
            //        event.preventDefault();
            //        return false;
            //    }
            //    else {
            //        searchTable($(this).val());
            //    }
            //});

            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '200px',
            });

            $('[id$=ddlMultiCellID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbUpdateTimeMachine]').multiselect({
                includeSelectAllOption: true
            });
            $(".searchable-dropdown-list").select2({
                placeholder: "Search",
                allowClear: false
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
