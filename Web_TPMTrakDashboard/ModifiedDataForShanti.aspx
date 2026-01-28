<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifiedDataForShanti.aspx.cs" Inherits="Web_TPMTrakDashboard.ModifiedDataForShanti"  EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #downdatagrid tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #downdatagrid tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
            height: 38px;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
                position:sticky;
                top:-1px;
                background-color: #2E6886 !important;
                z-index:20;
            }

        .textboxcss {
            border: none;
            background-color: transparent;
            font-style: italic;
            color: black;
        }

        .addtextcss {
            border: initial;
            background-color: none;
        }

        .select {
            /*border-radius: 5px;*/
            -webkit-appearance: none;
        }

        #tblheader tbody > tr > td {
            padding-top: 3px;
            height: 10px;
            padding-bottom: 0px;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            padding: 1px;
        }

        td {
            text-align: center;
            /*width: 65px;*/
            height: 10px;
            /*padding: 2px;*/
        }

        .txtcolor {
            color: white;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            /*margin: -16px 0 1.5em 0 !important;*/
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 136px;
        }



        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: -14px 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            /*height: 80px;*/
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
            color: white;
        }

        /*/--------------Boostrap css--------------/*/
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }

        /*td{
            color:white;
        }*/
         .nav-pills > li {
            display: inline-block;
            float: none;
        }
    </style>
    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
              <asp:HiddenField runat="server" ID="hdnActiveTabID" ClientIDMode="Static" />
             <asp:HiddenField runat="server" ID="hdnUpdateSectionToggle" ClientIDMode="Static" />
             <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
             <asp:HiddenField runat="server" ID="hdnViewValue" ClientIDMode="Static" />
            <div class="row">
                <div style="width: auto; column-fill: auto">
                    <fieldset class="scheduler-border1" id="viewbox" style="padding-bottom:10px !important">
                        <legend class="scheduler-border">View Data</legend>
                        <table class="table table-bordered" id="tblheader" style="margin-top: -24px; column-width: auto; width: auto; color: white;margin-bottom:0px">
                            <tbody>
                                <tr>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">Plant ID</div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlplant" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </td>

                                    <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">Machine </div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlmachine" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 70px;" class="txtcolor">Data Type</div>
                                    </td>

                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddldatatype" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldatatype_SelectedIndexChanged">
                                                <asp:ListItem Value="Production Data">Production Data</asp:ListItem>
                                                <asp:ListItem Value="Down Data"> Down Data </asp:ListItem>
                                                <asp:ListItem Value="Rejection Data"> Rejection Data </asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </td>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 70px;" class="txtcolor">From Time</div>
                                    </td>
                                    <td class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" data-toggle="tooltip"
                                            title="From Date !" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">To Time</div>
                                    </td>
                                    <td class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" data-toggle="tooltip"
                                            title="To Date !" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>

                                    <td colspan="2">
                                        <div style="min-width: 115px;">
                                            <asp:Button runat="server" Text="View" Style="float: none" class="btn btn-info" ID="btnview" OnClick="btnview_Click" OnClientClick="ShowLoader()"></asp:Button>&nbsp;
									
                                            <asp:Button runat="server" Text="Save" Style="float: none" class="btn btn-info" ID="Button2" OnClick="btnsave_Click" OnClientClick="ShowLoader()"></asp:Button>&nbsp;
                                       
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </fieldset>
                </div>

                <div style="margin-top: -23px;">
                    <div style="margin-top: 27px; margin-left: 10px;">
                      <%--  <input type="button" value="Show UpdateData" id="showgrpoption" class="btn btn-primary" style="height: 30px;">--%>
                        <%--                <span class="glyphicon glyphicon-chevron-down" style="color: white" id="showgrpoption">show UpdateData</span>--%>
                    </div>
                </div>
                <div>
                     <fieldset class="scheduler-border1" style="padding-bottom:5px !important">
                        <legend  class="scheduler-border" style="margin-bottom:5px !important">
                             <label style="height: 30px;background-color:#08666f;border:1px solid #1ac9d2;padding:6px 12px;font-size:14px;border-radius:4px"  class="" id="showgrpoption" > <span id="filterSpn">Hide Filter</span>&nbsp;<i id="updateSectionIcon" class="glyphicon glyphicon-chevron-down"></i></label>
                        </legend>
                    <div id="grpoption">
                        <fieldset class="scheduler-border" id="updatabox" style="width: 71%; height: 135px; display: inline-block;min-width:71%">
                            <legend class="scheduler-border">Update Data</legend>
                            <ul class="nav nav-pills" style="margin-top: -20px;;white-space:nowrap;overflow:auto" id="tabsContainer">
                                <li runat="server" id="liWorkOrder" class="tabsName active" clientidmode="static"><a class="menuData" data-toggle="tab" href="#workorder">WorkOrderNO</a></li>
                                <li  runat="server" id="liRejectionCode" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#rejcode">Rejection Code</a></li>
                                <li  runat="server" id="liCompOperation" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#componentoperation">Component Operation</a></li>
                                <li  runat="server" id="liOperator" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#operators">Operator</a></li>
                                <li  runat="server" id="liDownID" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#downid">Down ID</a></li>
                                <li  runat="server" id="liPartCount" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#partscount">Parts Count</a></li>
                                 <li runat="server" id="liSlno" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#divSlno">Serial Number</a></li>
                                 <li runat="server" id="liSupervisorCode" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#divSupervisor">Supervisor</a></li>
                                 <li runat="server" id="liSupplierCode" class="tabsName"  clientidmode="static"><a class="menuData" data-toggle="tab" href="#divSupplier">Supplier</a></li>
                            </ul>
                            <div style="margin-top: 10px;">

                                <div class="tab-content">
                                    <div id="workorder" class="tab-pane fade in active"  runat="server" clientidmode="static" >
                                        <div>
                                            <table class="table table-bordered" style="width: 75%;">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlfrm" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To</div>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="toWorkOrder" class="form-control " runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="workupdate" OnClick="workupdate_Click" OnClientClick="return GetWorkorderUpdateConfirmation()"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="rejcode" class="tab-pane fade"  runat="server" clientidmode="static" >
                                        <div>
                                            <table class="table table-bordered" style="width: 90%">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlrejfrom" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlrejto" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="rejupdate" OnClick="rejupdate_Click"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="componentoperation" class="tab-pane fade"  runat="server" clientidmode="static" >
                                        <div>
                                            <table class="table table-bordered" style="margin-top: -6px;">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From Component</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlfrmcomponent" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlfrmcomponent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To Component</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddltocomponent" Style="width: 115px;" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddltocomponent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From Operation</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlfrmoperation" CssClass="form-control" runat="server" Style="min-width: 70px"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To Operation</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlopreration_to" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="compupdate" OnClick="compupdate_Click"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="operators" class="tab-pane fade"  runat="server" clientidmode="static" >
                                        <div>
                                            <table class="table table-bordered" style="width: 90%;">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlfrmorp" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlopr_to" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="oprupdate" OnClick="oprupdate_Click" OnClientClick="return GetOperatorUpdateConfirmation()"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="downid" class="tab-pane fade"  runat="server" clientidmode="static" >
                                        <div>
                                            <table class="table table-bordered" style="width: 90%;">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlfrmdown" Style="min-width: 100px;" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldown_to" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="downupdate" OnClick="downupdate_Click"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="partscount" class="tab-pane fade"  runat="server" clientidmode="static" >
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <table class="table table-bordered">
                                                    <tr>
                                                        <td>
                                                            <div style="margin-top: 5px;" class="txtcolor">Component</div>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlpartcomp" AutoPostBack="true" OnSelectedIndexChanged="ddlpartcomp_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <div style="margin-top: 5px;" class="txtcolor">From Count</div>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlfrmcount" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <div style="margin-top: 5px;" class="txtcolor">Operation</div>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlpatsopr" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlpatsopr_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <div style="margin-top: 5px;" class="txtcolor">To Count</div>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ddlpartstocount" CssClass="form-control" runat="server" onkeypress="return event.charCode >= 48 && event.charCode <= 57" ></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button runat="server" Text="Update" class="btn btn-info" ID="partsupdate" OnClick="Partsupdate_Click"></asp:Button>&nbsp;
                                                </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                     <div id="divSlno" runat="server" clientidmode="static"  class="tab-pane fade in active"> 
                                        <div>
                                            <table class="table table-bordered" style="width: 75%;">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFromSlno" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To</div>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToSlno" class="form-control " runat="server" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="slnoUpdateBtn" OnClick="slnoUpdateBtn_Click" OnClientClick="return GetSlnoUpdateConfirmation()"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                     <div id="divSupervisor" runat="server" clientidmode="static"  class="tab-pane fade in active"> 
                                        <div>
                                            <table class="table table-bordered" style="width: 75%;">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFromSupervisor" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To</div>
                                                    </td>
                                                    <td>
                                                         <asp:DropDownList ID="ddlToSupervisor" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="supervisorUpdateBtn" OnClick="supervisorUpdateBtn_Click" OnClientClick="return GetSupervisorUpdateConfirmation()"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                     <div id="divSupplier" runat="server" clientidmode="static"  class="tab-pane fade in active">
                                        <div>
                                            <table class="table table-bordered" style="width: 75%;">
                                                <tr>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">From</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFromSupplier" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div style="margin-top: 5px;" class="txtcolor">To</div>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToSupplier" class="form-control " runat="server" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button runat="server" Text="Update" class="btn btn-info" ID="supplierUpdateBtn" OnClick="supplierUpdateBtn_Click" OnClientClick="return GetSupplierUpdateConfirmation()"></asp:Button>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <%--  <div id="inconsistency" class="tab-pane fade">
                        <div>
                            <table class="table table-bordered" style="width: 60%;">
                                <tr>
                                    <td>
                                        <div style="margin-top: 5px;">Validate</div>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlvalidate" CssClass="form-control" runat="server">
                                            <asp:ListItem>
                                                Component-Operation
                                            </asp:ListItem>
                                            <asp:ListItem>
                                             Operator
                                            </asp:ListItem>
                                            <asp:ListItem>
                                              DownID
                                            </asp:ListItem>

                                        </asp:DropDownList>
                                    </td>

                                    <td>
                                        <asp:Button runat="server" Text="View" class="btn btn-info" ID="validateview"></asp:Button>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>--%>
                                </div>
                            </div>
                        </fieldset>

                        <fieldset class="scheduler-border" id="Vlid" style="width: 25%; height: 135px; display: inline-block; padding-right: 0px;;min-width:25%;float:right">
                            <legend class="scheduler-border">View Data Inconsistency</legend>

                            <div>
                                <table class="table table-bordered">
                                    <tr>
                                        <td>
                                            <div style="margin-top: 5px;" class="txtcolor">Validate</div>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlvalidatedata" CssClass="form-control" runat="server">
                                                <asp:ListItem Value="Component-Operation">Component-Operation </asp:ListItem>
                                                <asp:ListItem Value="Operator">Operator</asp:ListItem>
                                                <asp:ListItem Value="DownID">DownID</asp:ListItem>

                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                            <asp:Button runat="server" Text="View" class="btn btn-info" ID="btn_Inconsistency" OnClick="btn_Inconsistency_Click" OnClientClick="ShowLoader()"></asp:Button>&nbsp;
                                
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </fieldset>
                    </div>
                         </fieldset>
                </div>
                <div id="divgrd" style="height: 570px; overflow: auto; margin-top: 5px;">
                    <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; color: white; text-align: center; font-family: Calibri;"></asp:Label>

                    <%-- Down grid--%>
                    <asp:GridView ID="downdatagrid" runat="server" AutoGenerateColumns="false" Width="100%" AllowPaging="true" PageSize="100" HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available." ShowHeaderWhenEmpty="true" ShowHeader="true" HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnRowDataBound="downdatagrid_RowDataBound" OnPageIndexChanging="downdatagrid_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="ComponentName">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfdownsavecheck" runat="server" />
                                    <asp:HiddenField ID="hdfcomponentname" runat="server" Value='<%# Eval("ComponentInterfaceid") %>' />
                                    <asp:DropDownList ID="ComponentName" runat="server" CssClass="textboxcss select"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="OperationID" Style="width: 80px; text-align: left" CssClass="textboxcss" Text='<%# Eval("Operation") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfoperator" runat="server" Value='<%# Eval("OperatorInterfaceid") %>' />
                                    <asp:DropDownList ID="OperatorName" runat="server" CssClass="textboxcss select"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Down Code">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfdowncode" runat="server" Value='<%# Eval("DownId") %>' />
                                    <asp:DropDownList ID="DownCode" runat="server" CssClass="textboxcss select"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TimeFrom">
                                <ItemTemplate>
                                    <%-- <asp:Label ID="downFromTime" CssClass="splitfun" runat="server"  Text='<%# Eval("TimeFrom") %>'></asp:Label>--%>
                                   <asp:Label ID="downFromTime" CssClass="splitfun" runat="server" ids='<%# Eval("ID") %>' ComponentID='<%# Eval("Component") %>' OperationNo='<%# Eval("Operation") %>' DownID='<%# Eval("DownId") %>' OperatorID='<%# Eval("Operator") %>' ndtime='<%# Eval("TimeTo") %>' sttime='<%# Eval("TimeFrom") %>' Text='<%# Eval("TimeFrom") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TimeTo">
                                <ItemTemplate>
                                  <asp:Label ID="downToTime" CssClass="splitfun" runat="server" ids='<%# Eval("ID") %>' ComponentID='<%# Eval("Component") %>' OperationNo='<%# Eval("Operation") %>' DownID='<%# Eval("DownId") %>' OperatorID='<%# Eval("Operator") %>' ndtime='<%# Eval("TimeTo") %>' sttime='<%# Eval("TimeFrom") %>' Text='<%# Eval("TimeTo") %>'></asp:Label>
                                    <%-- <asp:Label ID="downToTime" CssClass="splitfun" runat="server"  Text='<%# Eval("TimeTo") %>'></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WorkOrderNo">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="workorderno" Style="width: 70px; float: left" CssClass="textboxcss" Text='<%# Eval("WorkOrderNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Serial Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSlno" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supervisor Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSupervisorCode" Text='<%# Eval("Supervisorcode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supplier Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSupplierCode" Text='<%# Eval("SupplierCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Bold="True" ForeColor="White" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>

                    <%-- Rejection grid--%>
                    <asp:GridView ID="rejectiongrd" runat="server" AutoGenerateColumns="false" Width="100%" AllowPaging="true" PageSize="100" HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available." ShowHeaderWhenEmpty="true" ShowHeader="true" HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnRowDataBound="rejectiongrd_RowDataBound" OnPageIndexChanging="rejectiongrd_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Component">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfdownsavecheck" runat="server" />
                                    <asp:HiddenField ID="hdfcomponentrej" runat="server" Value='<%# Eval("ComponentInterfaceid") %>' />
                                    <asp:DropDownList ID="comprejection" runat="server" Style="min-width: 145px;" CssClass="textboxcss select"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="operation" Style="width: 70px; text-align: left" CssClass="textboxcss" Text='<%# Eval("Operation") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfoperatorrej" runat="server" Value='<%# Eval("OperatorInterfaceid") %>' />
                                    <asp:DropDownList ID="Operator" runat="server" Style="min-width: 145px;" CssClass="textboxcss select"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rejection Code">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="RejectionCodeID" CssClass="textboxcss" Text='<%# Eval("RejectionCode") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rejection Qty">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="RejectionQty" CssClass="textboxcss" Text='<%# Eval("RejectionQty") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CreatedTimeStamp">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="CreatedTs" CssClass="textboxcss" Text='<%# Eval("createdTs") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RejectionDate">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="RejectionDate" CssClass="textboxcss" Text='<%# Eval("RejectionDate") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RejectionShift">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="RejectionShift" CssClass="addtextcss" Text='<%# Eval("RejectionShift") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Serial Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSlno" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supervisor Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSupervisorCode" Text='<%# Eval("Supervisorcode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supplier Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSupplierCode" Text='<%# Eval("SupplierCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="mc" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="mc" runat="server" Text='<%# Eval("mc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Bold="True" ForeColor="White" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>

                    <%-- production grid--%>
                    <asp:GridView ID="datagridproductiondata" runat="server" AutoGenerateColumns="false" Visible="false" Width="100%" PageSize="100" AllowPaging="true" OnPageIndexChanging="datagridproductiondata_PageIndexChanging"
                        HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true"
                        HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnRowDataBound="datagridproductiondata_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Component">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfprodsavecheck" runat="server" />
                                    <asp:HiddenField ID="hdfcomponent" runat="server" Value='<%# Eval("ComponentInterfaceid") %>' />
                                    <asp:DropDownList ID="Component" CssClass="textboxcss select" runat="server"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Down ID">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="Downid" CssClass="textboxcss" Text='<%# Eval("DownId") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="Operation" Style="width: 70px; text-align: left" CssClass="textboxcss" Text='<%# Eval("Operation") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfoperator" runat="server" Value='<%# Eval("OperatorInterfaceid") %>' />
                                    <asp:DropDownList ID="Operator" CssClass="textboxcss select" runat="server"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PartCount">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="PartsCount" CssClass="textboxcss" Text='<%# Eval("PartsCount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TimeFrom">
                                <ItemTemplate>
                                    <asp:Label ID="TimeFrom" runat="server" Text='<%# Eval("TimeFrom") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TimeTo">
                                <ItemTemplate>
                                    <asp:Label ID="TimeTo" runat="server" Text='<%# Eval("TimeTo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WorkOrderNo">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="WorkOrderNo" Text='<%# Eval("WorkOrderNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Serial Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSlno" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supervisor Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSupervisorCode" Text='<%# Eval("Supervisorcode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Supplier Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" CssClass="textboxcss" Style="width: 70px; float: left" ID="lblSupplierCode" Text='<%# Eval("SupplierCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Bold="True" ForeColor="White" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                    <asp:Button runat="server" ID="btnReloadBtn" Visible="false" OnClick="btnReloadBtn_Click"/>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
         var splitChild;
        var splitTimer
        function checkSplitChild() {
            if (splitChild.closed) {
                clearInterval(splitTimer);
                __doPostBack('<%= btnReloadBtn.UniqueID%>', '');
            }
        }
         function GetSlnoUpdateConfirmation() {
            return window.confirm("Do you really want to update serial number from " + $("[id$=ddlFromSlno]").val() + " to " + $("[id$=txtToSlno]").val() + " ?");
        }
        function GetSupervisorUpdateConfirmation() {
            return window.confirm("Do you really want to update Supervisor from " + $("[id$=ddlFromSupervisor]").val() + " to " + $("[id$=ddlToSupervisor]").val() + " ?");
        }
        function GetSupplierUpdateConfirmation() {
            return window.confirm("Do you really want to update Supplier from " + $("[id$=ddlFromSupplier]").val() + " to " + $("[id$=txtToSupplier]").val() + " ?");
        }
         $('.tabsName').click(function () {
            debugger;
            $('#hdnActiveTabID').val($(this).attr('id'));
        });
        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MMM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtToDate]').datetimepicker({
            format: 'DD-MMM-YYYY HH:mm',
            useCurrent: false,
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $("[id$=txtFromDate]").on("dp.change", function (e) {
            $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
        });
        $("[id$=txtToDate]").on("dp.change", function (e) {
            $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
        });

        $("#showgrpoption").on("click", function () {
            if ($('#updateSectionIcon').hasClass('glyphicon-chevron-down')) {
                $('#updateSectionIcon').removeClass('glyphicon-chevron-down');
                $('#updateSectionIcon').addClass('glyphicon-chevron-up');
                $('#hdnUpdateSectionToggle').val("minimize");
                $('#grpoption').css('display', 'none'); 
                $('#filterSpn').text("Show Filter");
            }
            else {
                $('#updateSectionIcon').removeClass('glyphicon-chevron-up');
                $('#updateSectionIcon').addClass('glyphicon-chevron-down');
                $('#hdnUpdateSectionToggle').val("maximize");
                $('#grpoption').css('display', 'block');
                $('#filterSpn').text("Hide Filter");
            }
            //$("#grpoption").toggle()
        });
         var bigDiv = document.getElementById('tabsContainer');
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollLeft);
        }
        window.onload = function () {
            bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
        }
        $(document).on("click", "tr .splitfun", function (e) {

            var sttime = $(this).attr('sttime');
            var ndtime = $(this).attr('ndtime');
            var Component = $(this).attr('ComponentID');
            var Operation = $(this).attr('OperationNo');
            var OperatorID = $(this).attr('OperatorID');
            var ID = $(this).attr('ids');
            var DownID = $(this).attr('DownID');
            var MachineID = $("[id$=ddlmachineid]").val();
            var Type = "ModifiedData";
            PopupCenter("SplitFun.aspx?MachineID=" + MachineID + "&ComponentID=" + Component + "&Operation=" + Operation + "&Operator=" + OperatorID + "&DownID=" + DownID + "&sttime=" + sttime + "&ndtime=" + ndtime + "&Type=" + Type + "&ID=" + ID, "", 2000, 600);
             splitTimer = setInterval(checkSplitChild, 1000); 
            //PopupCenter("SplitFun.aspx?MachineID=" + MachineID + "&ID=" + ID + "&ComponentID=" + Component + "&Operation=" + Operation + "&Operator=" + OperatorID + "&DownID=" + DownID + "&sttime=" + sttime + "&ndtime=" + ndtime + "&Type=" + Type, 1000, 400);
        });

        function PopupCenter(url, title, w, h) {

            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=yes,toolbar=no,resizable=yes,width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            //location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=no
            splitChild = newWindow;
            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        //............add remove csss..............
        $("[id$=downdatagrid]").on("click", "td", function () {

            $(this).closest('tr').find('input[type=hidden]').val("updated");
            $("[id$=downdatagrid] tr td").find('input').removeClass("form-control");
            $("[id$=downdatagrid] tr td").find('input').addClass("textboxcss");
            $("[id$=downdatagrid] tr td").find('select').addClass("select");
            $("[id$=downdatagrid] tr td").find('select').addClass("textboxcss");
            $("[id$=downdatagrid] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");
        });

        $("[id$=datagridproductiondata]").on("click", "td", function () {

            $(this).closest('tr').find('input[type=hidden]').val("updated");
            $("[id$=datagridproductiondata] tr td").find('input').removeClass("form-control");
            $("[id$=datagridproductiondata] tr td").find('input').addClass("textboxcss");
            $("[id$=datagridproductiondata] tr td").find('select').addClass("select");
            $("[id$=datagridproductiondata] tr td").find('select').addClass("textboxcss");
            $("[id$=datagridproductiondata] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");
        });

        $("[id$=rejectiongrd]").on("click", "td", function () {

            $(this).closest('tr').find('input[type=hidden]').val("updated");
            $("[id$=rejectiongrd] tr td").find('input').removeClass("form-control");
            $("[id$=rejectiongrd] tr td").find('input').addClass("textboxcss");
            $("[id$=rejectiongrd] tr td").find('select').addClass("select");
            $("[id$=rejectiongrd] tr td").find('select').addClass("textboxcss");
            $("[id$=rejectiongrd] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");
        });

        function ShowLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }

        function HideLoader() {
            $.unblockUI({});
        }

        function GetWorkorderUpdateConfirmation() {
            return window.confirm("Do you really want to update work order from " + $("[id$=ddlfrm]").val() + " to " + $("[id$=toWorkOrder]").val() + " ?");
        }

        function GetOperatorUpdateConfirmation() {
            return window.confirm("Do you really want to update operator from " + $("[id$=ddlfrmorp]").val() + " to " + $("[id$=ddlopr_to]").val() + " ?");
        }

        //.......end add remove css.................


        //because of updatepannel
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
             var bigDiv = document.getElementById('tabsContainer');
            $('.tabsName').click(function () {
                debugger;
                $('#hdnActiveTabID').val($(this).attr('id'));
            });
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MMM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MMM-YYYY HH:mm',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            $("#showgrpoption").on("click", function () {
                  if ($('#updateSectionIcon').hasClass('glyphicon-chevron-down')) {
                    $('#updateSectionIcon').removeClass('glyphicon-chevron-down');
                    $('#updateSectionIcon').addClass('glyphicon-chevron-up');
                    $('#hdnUpdateSectionToggle').val("minimize");
                      $('#grpoption').css('display', 'none');
                       $('#filterSpn').text("Show Filter");
                }
                else {
                    $('#updateSectionIcon').removeClass('glyphicon-chevron-up');
                    $('#updateSectionIcon').addClass('glyphicon-chevron-down');
                    $('#hdnUpdateSectionToggle').val("maximize");
                      $('#grpoption').css('display', 'block'); 
                      $('#filterSpn').text("Hide Filter");
                }
                //$("#grpoption").toggle()
            });
             $(document).ready(function () {
                setUpdateTooggle();
                bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
            });
            function setUpdateTooggle() {
                debugger;
                if ($('#hdnUpdateSectionToggle').val() == "minimize") {
                    $('#updateSectionIcon').removeClass('glyphicon-chevron-down');
                    $('#updateSectionIcon').addClass('glyphicon-chevron-up');
                    $('#grpoption').css('display', 'none')
                }
                else {
                    $('#updateSectionIcon').removeClass('glyphicon-chevron-up');
                    $('#updateSectionIcon').addClass('glyphicon-chevron-down');
                    $('#grpoption').css('display', 'block')
                }
            }
             bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollLeft);
            }
            window.onload = function () {
                bigDiv.scrollLeft = $('[id*=hdnScrollPos]').val();
            }
            //add remove css
            $("[id$=downdatagrid]").on("click", "td", function () {

                $(this).closest('tr').find('input[type=hidden]').val("updated");
                $("[id$=downdatagrid] tr td").find('input').removeClass("form-control");
                $("[id$=downdatagrid] tr td").find('input').addClass("textboxcss");
                $("[id$=downdatagrid] tr td").find('select').addClass("select");
                $("[id$=downdatagrid] tr td").find('select').addClass("textboxcss");
                $("[id$=downdatagrid] tr td").find('select').removeClass("form-control");

                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
                $(this).closest('td').find('select').addClass("form-control");
                $(this).closest('td').find('select').removeClass("textboxcss");
                $(this).closest('td').find('select').removeClass("select");
            });

            $("[id$=datagridproductiondata]").on("click", "td", function () {

                $(this).closest('tr').find('input[type=hidden]').val("updated");
                $("[id$=datagridproductiondata] tr td").find('input').removeClass("form-control");
                $("[id$=datagridproductiondata] tr td").find('input').addClass("textboxcss");
                $("[id$=datagridproductiondata] tr td").find('select').addClass("select");
                $("[id$=datagridproductiondata] tr td").find('select').addClass("textboxcss");
                $("[id$=datagridproductiondata] tr td").find('select').removeClass("form-control");

                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
                $(this).closest('td').find('select').addClass("form-control");
                $(this).closest('td').find('select').removeClass("textboxcss");
                $(this).closest('td').find('select').removeClass("select");
            });

            $("[id$=rejectiongrd]").on("click", "td", function () {

                $(this).closest('tr').find('input[type=hidden]').val("updated");
                $("[id$=rejectiongrd] tr td").find('input').removeClass("form-control");
                $("[id$=rejectiongrd] tr td").find('input').addClass("textboxcss");
                $("[id$=rejectiongrd] tr td").find('select').addClass("select");
                $("[id$=rejectiongrd] tr td").find('select').addClass("textboxcss");
                $("[id$=rejectiongrd] tr td").find('select').removeClass("form-control");

                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
                $(this).closest('td').find('select').addClass("form-control");
                $(this).closest('td').find('select').removeClass("textboxcss");
                $(this).closest('td').find('select').removeClass("select");
            });
            //end add remve css

            function ShowLoader() {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            }

            function HideLoader() {
                $.unblockUI({});
            }

            function GetWorkorderUpdateConfirmation() {
                return window.confirm("Do you really want to update work order from " + $("[id$=ddlfrm]").val() + " to " + $("[id$=toWorkOrder]").val() + " ?");
            }

            function GetOperatorUpdateConfirmation() {
                return window.confirm("Do you really want to update operator from " + $("[id$=ddlfrmorp]").val() + " to " + $("[id$=ddlopr_to]").val() + " ?");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
