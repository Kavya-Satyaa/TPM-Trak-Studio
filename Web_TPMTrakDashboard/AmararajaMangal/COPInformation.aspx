<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="COPInformation.aspx.cs" Inherits="Web_TPMTrakDashboard.AmararajaMangal.COPInformation" EnableEventValidation="false" %>

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
            /* padding: 0.1em 0.5em 1.1em !important;*/
            padding: 0.1em 0.5em 0 !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            /* margin: 0 0 1.5em 0 !important;*/
            margin: 0 0 0 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            /*height: 795px;*/
        }


        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 0 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 250px;
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
                    <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri; color: green; font-size: x-large;"></asp:Label>
                </div>
                <div class="col-md-2">
                    <div>
                        <asp:TextBox ID="txtsearch" class="form-control " Style="margin-top: 9px;" title="<%$Resources:CommanResource,SearchHere %>" placeholder="<%$Resources:SearchComp %>" CssClass="searchdata form-control" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <%--<input type="text" id="search" data-toggle="tooltip" title="search here !" placeholder="search Component here..." class="searchdata form-control " />--%>
                    </div>

                    <div>
                        <%--<table id="tblcomponentinfo" class="table table-bordered table-hover">
                    <thead class="blue">
                        <tr>
                            <th>Component ID</th>
                        </tr>
                    </thead>
                </table>--%>
                        <div style="height: 752px; overflow: auto">
                            <asp:GridView ID="ComponentIdgrd" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                HeaderStyle-Font-Size="13" EmptyDataText="<%$Resources:CommanResource,Nodataavailable %>" ShowHeaderWhenEmpty="true" HeaderStyle-CssClass="blue" AlternatingRowStyle-BackColor="#FFFFFF" BackColor="#F2F2F2">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$Resources:CommanResource,ComponentID %>">

                                        <ItemTemplate>
                                            <u>
                                                <asp:LinkButton ID="btn_componentid" runat="server" ForeColor="#002BC0" Text='<%# Eval("componentid") %>' OnClick="btncomponent_Click"></asp:LinkButton>
                                            </u>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:BoundField HeaderText="Component ID" DataField="componentid" ItemStyle-CssClass="createhyperlink" HeaderStyle-HorizontalAlign="Center" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-md-10" style="height: 760px;">
                    <div>

                        <a class="glyphicon glyphicon-arrow-left" style="font-size: 25px; font-weight: bold" href="../ComponentInformation.aspx"></a>

                        <fieldset class="scheduler-border" id="opinfo">
                            <legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource", "OprInfo") %> - <%=txtsearch.Text %> </legend>
                            <table class="table table-bordered" id="operationinfo" style="margin-top: -18px;">
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
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "OperationNo") %></div>
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtopn_no" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                            <%--  <input type="text" id="opn_no" class="form-control " />--%>
                                 
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "PlantID") %></div>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" title="<%$Resources:CommanResource,PlantID %>" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Drawing") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtdrawing" class="form-control " runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                                <%-- <input type="text" id="txtdrawing" class="form-control " />--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="lblFinishOpr" class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "FinishOperation") %></div>
                                        </td>
                                        <td>
                                            <div style="height: 35px; width: 40px;">
                                                <asp:CheckBox ID="chkFinishOperation" runat="server"></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "InterfaceID") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtinterfaceid" class="form-control" runat="server" onkeypress="return event.charCode >= 48 && event.charCode <= 57" AutoCompleteType="Disabled"></asp:TextBox>
                                                <%-- <input type="text" id="txtinterfaceid" class="form-control " />--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "MachineId") %></div>
                                        </td>
                                        <td style="min-width: 140px;">

                                            <div class="row">
                                                <div>
                                                    <asp:ListBox ID="ddlMultiDownID" runat="server" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                                </div>
                                                <%--  class="col-md-9" <div class="col-md-3">
                                                <label class="checkbox" style="float: right">
                                                    <asp:CheckBox ID="chkall" runat="server" />All</label>
                                            </div>--%>

                                                <%-- <div style="float:right">--%>


                                                <%-- </div>--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Target") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txttarget" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                                <%--   <input type="text" id="txttarget" class="form-control " />--%>
                                            </div>
                                        </td>
                                        <td rowspan="2" colspan="2">
                                            <div style="margin-top: 20px; margin-left: 2px; min-width: 145px">
                                                <span style="width: 100%">
                                                    <asp:Button runat="server" Text="<%$Resources:CommanResource,New %>" Style="display: inline-block" class="btn btn-info" ID="btnnew" OnClick="btnnew_Click"></asp:Button>&nbsp;
                                 <%--   <input type="button" value="New" class="btn btn-info btn-sm" id="btnnew" />&nbsp;--%>
                                                    <asp:Button runat="server" Text="<%$Resources:CommanResource,Save %>" Style="display: inline-block" class="btn btn-info" ID="btnsave" OnClick="btnsave_Click"></asp:Button>
                                                    <asp:Button runat="server" Text="<%$Resources:CommanResource,Delete %>" Style="display: inline-block" class="btn btn-info" ID="btndelete" OnClick="btndelete_Click" OnClientClick="return confirm('Are you sure you want delete');"></asp:Button>
                                                </span>
                                                <br />
                                                <asp:Button runat="server" Text="<%$Resources:AssignCo %>" Style="margin-top: 10px;" class="btn btn-info" ID="btnassign" OnClick="btnassign_Click"></asp:Button>
                                                <input type="button" id="btnUpdateStdTime" runat="server" value='Update Cycle Time' class="btn btn-info btn-sm" onclick=" return getUpdateStdTimes();" style="margin-top: 10px" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Description") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtdescription" class="form-control " runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                                <%--  <input type="text" id="txtdescription" class="form-control " />--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "Price") %></div>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtprice" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                        </td>
                                        <td>
                                            <div class="commontd" style="margin-top: 5px; text-align-last: left;"><%=GetGlobalResourceObject("CommanResource", "SubOpr") %></div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtsubop" class="form-control " runat="server" AutoCompleteType="Disabled" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                                <%--  <input type="text" id="txtsubop" class="form-control " />--%>
                                            </div>
                                        </td>
                                    </tr>


                                </tbody>
                            </table>


                            <fieldset class="scheduler-border1" id="stdtime">
                                <legend runat="server" id="lgStandartTime" class="scheduler-border commontd" title="<%$Resources:StdTimeSec %>"></legend>

                                <table class="table table-bordered" id="startedtime" style="margin-top: -24px;">
                                    <thead>
                                        <tr>
                                            <%-- <th colspan="6"></th>
                                //Standard Time(in Seconds)--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;"><%=GetLocalResourceObject("MachiningTime") %></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtmachinetime" class="form-control " runat="server" OnTextChanged="txtmachinetime_TextChanged" AutoPostBack="true" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57)|| event.charCode==46)"></asp:TextBox>
                                                    <%-- <input type="text" id="txtmachinetime" class="form-control " />--%>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;"><%=GetGlobalResourceObject("CommanResource", "CycleTime") %></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtCycleTime" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57)|| event.charCode==46)"></asp:TextBox>
                                                    <%--   <input type="text" id="txtCycleTime" class="form-control " />--%>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;"><%=GetLocalResourceObject("CuttingTimeThr") %></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtcuttingtime" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                                    <%--  <input type="text" id="txtcuttingtime" class="form-control " />--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;"><%=GetLocalResourceObject("LoadUnload") %></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtloadunload" class="form-control " runat="server" OnTextChanged="txtloadunload_TextChanged" AutoPostBack="True" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57)|| event.charCode==46)"></asp:TextBox>
                                                    <%--    <input type="text" id="txtload" class="form-control " />--%>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;"><%=GetLocalResourceObject("LoadUnloadThr") %></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtloadunloadthreshold" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57)|| event.charCode==46)"></asp:TextBox>
                                                    <%-- <input type="text" id="txtloadunload" class="form-control " />--%>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="margin-top: 5px;" class="commontd">
                                                    <asp:Label ID="lblstdTime" runat="server" Text="<%$Resources:StdSetupSec %>"></asp:Label>
                                                    <%--Std. Setup Time(in Seconds)--%>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtsetuptime" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                                    <%--  <input type="text" id="txtsetuptime" class="form-control " />--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Min. LUL Threshold</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtminLoadUnloadThreshold" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Parts Per Cycle</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtPartPerCycle" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Strokes Per Part</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtStrokesPerPart" class="form-control " runat="server" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Weight</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtWeight" class="form-control " runat="server" onkeypress="return allowDecimal(event);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Thickness</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtThickness" class="form-control " runat="server" onkeypress="return allowDecimal(event);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Grade</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:DropDownList ID="ddlGrade" runat="server" CssClass="form-control" data-toggle="tooltip" title="Grade">
                                                        <asp:ListItem Value="None" Text="None"></asp:ListItem>
                                                        <asp:ListItem Value="AL" Text="AL"></asp:ListItem>
                                                        <asp:ListItem Value="GI" Text="GI"></asp:ListItem>
                                                        <asp:ListItem Value="MS" Text="MS"></asp:ListItem>
                                                        <asp:ListItem Value="SS" Text="SS"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Length</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtLength" class="form-control " runat="server" onkeypress="return allowDecimal(event);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="commontd" style="margin-top: 5px;">Width</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:TextBox ID="txtWidth" class="form-control " runat="server" onkeypress="return allowDecimal(event);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </thead>
                                </table>
                            </fieldset>
                        </fieldset>
                        <div style="height: 360px; overflow: scroll;">


                            <asp:GridView ID="componentgrd" runat="server" AutoGenerateColumns="false" Width="100%"
                                HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true"
                                HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnSelectedIndexChanged="componentgrd_SelectedIndexChanged" OnRowDataBound="OnRowDataBound" AllowSorting="true" OnSorting="componentgrd_Sorting" ClientIDMode="Static">
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
                                    <asp:BoundField HeaderText="<%$Resources:CommanResource,CycleTime %>" HeaderStyle-Wrap="false" DataField="cycletime" HeaderStyle-HorizontalAlign="Center" SortExpression="cycletime" />
                                    <asp:BoundField HeaderText="<%$Resources:LoadUnload %>" HeaderStyle-Wrap="false" DataField="LoadUnloadThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="LoadUnloadThreshold" />
                                    <asp:BoundField HeaderText="<%$Resources:StdSetupTime %>" HeaderStyle-Wrap="false" ControlStyle-Width="200" DataField="StdSetupTime" HeaderStyle-HorizontalAlign="Center" SortExpression="StdSetupTime" />
                                    <asp:BoundField HeaderText="<%$Resources:CuttingTime %>" HeaderStyle-Wrap="false" DataField="MachiningTimeThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="CuttingTime" />
                                    <asp:BoundField HeaderText="Min. LUL Threshold" HeaderStyle-Wrap="false" DataField="MinLoadUnloadThreshold" HeaderStyle-HorizontalAlign="Center" SortExpression="MinLoadUnloadThreshold" />
                                    <asp:BoundField HeaderText="Parts Per Cycle" HeaderStyle-Wrap="false" DataField="PartsPerCycle" HeaderStyle-HorizontalAlign="Center" SortExpression="PartsPerCycle" />
                                    <asp:BoundField HeaderText="Strokes Per Part" HeaderStyle-Wrap="false" DataField="StrokesPerPart" HeaderStyle-HorizontalAlign="Center" SortExpression="StrokesPerPart" />
                                    <asp:BoundField HeaderText="Weight" HeaderStyle-Wrap="false" DataField="Weight" HeaderStyle-HorizontalAlign="Center" SortExpression="Weight" />
                                    <asp:BoundField HeaderText="Thickness" HeaderStyle-Wrap="false" DataField="Thickness" HeaderStyle-HorizontalAlign="Center" SortExpression="Thickness" />
                                    <asp:BoundField HeaderText="Grade" HeaderStyle-Wrap="false" DataField="Grade" HeaderStyle-HorizontalAlign="Center" SortExpression="Grade" />
                                    <asp:BoundField HeaderText="Length" HeaderStyle-Wrap="false" DataField="Length" HeaderStyle-HorizontalAlign="Center" SortExpression="Length" />
                                    <asp:BoundField HeaderText="Width" HeaderStyle-Wrap="false" DataField="Width" HeaderStyle-HorizontalAlign="Center" SortExpression="Width" />
                                    <asp:BoundField HeaderText="<%$Resources:TargetPrc %>" HeaderStyle-Wrap="false" DataField="TargetPercent" HeaderStyle-HorizontalAlign="Center" SortExpression="TargetPercent" />
                                    <asp:BoundField HeaderText="<%$Resources:CommanResource,Drawing %>" HeaderStyle-Wrap="false" DataField="drawingno" HeaderStyle-HorizontalAlign="Center" SortExpression="drawingno" />
                                    <asp:BoundField HeaderText="<%$Resources:CommanResource,FinishOperation %>" HeaderStyle-Wrap="false" DataField="FinishedOperation" HeaderStyle-HorizontalAlign="Center" SortExpression="FinishedOperation" />
                                    <asp:BoundField HeaderText="<%$Resources:UpdatedBy %>" HeaderStyle-Wrap="false" DataField="UpdatedBy" HeaderStyle-HorizontalAlign="Center" SortExpression="UpdatedBy" />
                                    <asp:BoundField HeaderText="<%$Resources:UpdatedTS %>" HeaderStyle-Wrap="false" ControlStyle-Width="200" DataField="UpdatedTS" HeaderStyle-HorizontalAlign="Center" SortExpression="UpdatedTS" />
                                </Columns>
                                <HeaderStyle Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                        </div>
                    </div>
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
                                    <label class="control-label">Std. Cycle Time (<span id="stdCycleTime"></span>)</label></td>
                                <td>
                                    <asp:TextBox ID="txtStdCycleTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std Cycle Time" MaxLength="7"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Std. Load Time (<span id="stdLoadTime"></span>)</label></td>
                                <td>
                                    <asp:TextBox ID="txtStdLoadTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std Load Time" MaxLength="7"></asp:TextBox>
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
        });
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
                url: "COPInformation.aspx/GetUpdateStdTimes",
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
                            $("[id$=txtStdLoadTime]").attr("onkeypress", "return allowDecimal(event);");
                            $("[id$=txtStdCycleTime]").attr("onkeypress", "return allowDecimal(event);");
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
                url: "COPInformation.aspx/updateStandardTimeData",
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
            function allowDecimal(evt) {
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
        function allowDecimal(evt) {
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
