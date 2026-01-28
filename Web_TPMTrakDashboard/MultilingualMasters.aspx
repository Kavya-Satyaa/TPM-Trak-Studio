<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MultilingualMasters.aspx.cs" Inherits="Web_TPMTrakDashboard.MultilingualMasters" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/jquery-editable-select.css" rel="stylesheet" />
    <script src="../Scripts/jquery-editable-select.js"></script>

    <style>
        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }

        .dropdown {
            -webkit-appearance: none;
        }


        #tblDownCodesInfo tbody tr:nth-child(odd) td {
            background-color: #FFFFFF;
            color: black;
        }

        #tblDownCodesInfo tbody tr:nth-child(even) td {
            background-color: #DCDCDC;
            color: black;
        }

        .form-control {
            color: black;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        .btnExport {
            background-color: #1c701c;
        }

        .headerbtns {
            width: 250px;
            background: radial-gradient(#025973, #1c7691);
        }

        .tableHeaders {
            width: 50%;
            margin-left: 10px;
            margin-bottom: 10px
        }

        .textboxcss {
            border: none;
            background-color: transparent;
            color: black;
        }

        .select {
            -webkit-appearance: none;
        }

        .createhyperlink {
            text-decoration: underline;
            color: blue;
            cursor: pointer;
        }


        .addtextcss {
            border: initial;
            background-color: none;
            color: black;
        }


        #tblcomponentinfo tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #tblcomponentinfo tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        fieldset {
            border: 1px solid white !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            margin: 0px;
            vertical-align: top;
        }

        legend {
            font-size: 1.1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            color: white;
            border-bottom: none;
            margin-top: -4px;
            margin: 0px;
        }

        #tblRejection > tbody > tr:nth-child(odd) > td {
            background-color: #FFFFFF;
        }

        #tblRejection > tbody > tr:nth-child(even) > td {
            background-color: #DCDCDC;
        }

        #tblRework > tbody > tr:nth-child(odd) > td {
            background-color: #FFFFFF;
        }

        #tblRework > tbody > tr:nth-child(even) > td {
            background-color: #DCDCDC;
        }

        .navigationbar .nav-pills li a {
            background-color: #394a59;
            color: white;
        }

        .navigationbar .nav-pills li.active a {
            background-color: #4b83b5;
        }
        /* .navigationbar .nav-pills li:hover a{
            color: black
        }*/
    </style>

    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <div class="row navigationbar" style="display: flex; justify-content: left;">
        <ul class="nav nav-pills">
            <li class="active"><a class="menuData" data-toggle="tab" href="#menu1">Down Time Codes </a></li>
            <li><a class="menuData" data-toggle="tab" href="#menu2">Component Information        </a></li>
            <li><a class="menuData" data-toggle="tab" href="#menu3">Rejection Codes         </a></li>
        </ul>
    </div>

    <div class="row text-center">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; color: white" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>

    <div class="tab-content" style="margin-top: 10px;">


        <div id="menu1" class="tab-pane fade in active">
            <div class="col-lg-6" style="text-align: left; display: block; margin-bottom: 10px;">
                <table>
                    <tr>
                        <td>
                            <input type="text" autocomplete="off" id="search" data-toggle="tooltip" title="search !" placeholder="search..." class="form-control" style="width: 250px; display: inline;">
                        </td>
                        <td style="padding-left: 30px;">
                            <asp:Button runat="server" ID="btnUpdateDownCodeInfo" Text='<%$Resources:CommanResource,Save %>' CssClass="btn btn-info btnSave" OnClick="btnUpdateDownCodeInfo_Click" />
                        </td>
                    </tr>
                </table>
            </div>

            <%-- <div id="griddata" style="overflow-x: hidden; overflow-y: auto; min-height: 550px; width: 100%">
                <table id="tblDownCodesInfo" class="table table-bordered table-hover headerFixer" style="width: 100%">
                    <thead class="blue">
                        <tr>
                            <th><%=GetGlobalResourceObject("CommanResource","DownTime") %> 
                            </th>
                            <th><%=GetGlobalResourceObject("CommanResource","InterfaceID") %> 
                            </th>
                            <th><%=GetGlobalResourceObject("CommanResource","Catagory") %>
                            </th>
                            <th><%=GetGlobalResourceObject("CommanResource","Description") %>
                            </th>
                            <th><%= GetGlobalResourceObject("CommanResource", "DescriptionInHindi") %>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>--%>
            <div id="griddata" style="overflow-y: auto; height: 90vh; width: 100%">
                <asp:ListView runat="server" ID="lvDownCodesInfo" ClientIDMode="Static">
                    <LayoutTemplate>
                        <table id="tblDownCodesInfo" class="table table-bordered table-hover headerFixer" style="width: 100%">
                            <thead class="blue">
                                <tr>
                                    <th>Down ID 
                                    </th>
                                    <th style="max-width: 150px !important;">Interface ID 
                                    </th>
                                    <th>Category
                                    </th>
                                    <th>Description
                                    </th>
                                    <th>Description In Hindi</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblDownTime" CssClass="lblDownId" ClientIDMode="Static" Text='<%# Eval("downid") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdfInterface" ClientIDMode="Static" />
                                <asp:Label runat="server" ID="lblInterfaceid" Style="text-align: left;" Text='<%# Eval("interfaceid") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCategory" Text='<%# Eval("Catagory") %>'></asp:Label>
                            </td>
                            <td class="update">
                                <asp:TextBox runat="server" ID="txtDescription" CssClass="txtdes txtupdate form-control" Text='<%# Eval("downdescription") %>' Style="text-align: left"></asp:TextBox>
                            </td>
                            <td class="update">
                                <asp:TextBox runat="server" ID="DownDescriptionInHindi" CssClass="txtdesInHindi txtupdate form-control" Style="text-align: left;" Text='<%# Eval("DownDescriptionInHindi") %>'></asp:TextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>

        <div id="menu2" style="margin-left: 10px;" class="tab-pane fade">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <asp:HiddenField runat="server" ID="hdnCompList" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfInterfaceDataType" ClientIDMode="Static" />
                    <table class="" id="tblsearch" style="display: inline-block; width: auto">
                        <tr>
                            <td>
                                <div class="col-md-12">
                                    <input type="text" runat="server" clientidmode="static" id="searchComponent" data-toggle="tooltip" autocomplete="off" class="searchdata form-control " title="search here !" placeholder="search component here..." />
                                </div>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtComponentSearch" AutoCompleteType="Disabled" CssClass="form-control" OnTextChanged="txtComponentSearch_TextChanged" Placeholder="Enter Component ID"></asp:TextBox>
                            </td>
                            <td>
                                <div style="float: right">
                                    <asp:Button runat="server" ID="BtnView" CssClass="btn btn-info" ClientIDMode="Static" OnClick="BtnView_Click" Text="View" />
                                    &nbsp;&nbsp;
                                </div>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-info" ClientIDMode="Static" OnClick="btnSave_Click" OnClientClick="return ValidateInterfaceID();" Text='<%$Resources:CommanResource, Save  %>' />
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%">
                        <div id="ComponentGrid" style="overflow-x: hidden; overflow-y: auto; height: 90vh; width: 100%">
                            <asp:ListView runat="server" ID="lvComponentInfo" OnPagePropertiesChanging="lvComponentInfo_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <table id="tblcomponentinfo" class="table table-bordered table-hover headerFixer">
                                        <thead class="blue" runat="server">
                                            <tr>
                                                <th runat="server" id="thComponentID"><%=settings.Where(x=>x.ValueInText=="ComponentID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                                <th runat="server" id="thInterfaceID"><%=settings.Where(x=>x.ValueInText=="InterfaceID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                                <th runat="server" id="thCustomer"><%=settings.Where(x=>x.ValueInText=="Customer").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                                <th runat="server" id="thDescription"><%=settings.Where(x=>x.ValueInText=="Description").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                                <th>Description In Hindi</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr runat="server" id="itemplaceholder"></tr>
                                            <tr runat="server" style="position: sticky; bottom: -1px;">
                                                <td runat="server" colspan="6" style="border: 0px; text-align: center">
                                                    <asp:DataPager runat="server" PageSize="500" ID="DataPager1">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn btn-primary" ShowFirstPageButton="true" ShowNextPageButton="false" ShowPreviousPageButton="true" PreviousPageText="&laquo;" FirstPageText="&laquo;&laquo;" />
                                                            <asp:NumericPagerField NumericButtonCssClass="btn btn-primary" RenderNonBreakingSpacesBetweenControls="true" CurrentPageLabelCssClass="btn btn-success"></asp:NumericPagerField>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn btn-primary" ShowFirstPageButton="false" ShowNextPageButton="true" ShowPreviousPageButton="false" ShowLastPageButton="true" NextPageText="&raquo;" LastPageText="&raquo;&raquo;" />
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <EmptyDataTemplate>
                                    <table id="tblcomponentinfo" class="table table-bordered table-hover headerFixer">
                                        <tr class="blue">
                                            <th runat="server" id="thComponentID"><%=settings.Where(x=>x.ValueInText=="ComponentID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                            <th runat="server" id="thInterfaceID"><%=settings.Where(x=>x.ValueInText=="InterfaceID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                            <th runat="server" id="thCustomer"><%=settings.Where(x=>x.ValueInText=="Customer").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                            <th runat="server" id="thDescription"><%=settings.Where(x=>x.ValueInText=="Description").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                            <th>Description In Hindi</th>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HiddenField runat="server" ID="hdfInterface" ClientIDMode="Static" />
                                            <asp:Label runat="server" ID="lblComp" CssClass="txtComp searchField" Text='<%# Eval("componentid") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblInterfaceID" Style="text-align: left;" Text='<%# Eval("InterfaceID") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblCustomer" Text='<%# Eval("customerid") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDescription" CssClass="txtdes txtupdate form-control" Style="text-align: left;" Text='<%# Eval("description") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDescriptionInHindi" CssClass="txtdescInHindi txtupdate form-control" Style="text-align: left;" Text='<%# Eval("CompDescriptionHindi") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnView" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div id="menu3" class="tab-pane fade ">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <div class="row" style="width: 70%; margin-left: 10px;">

                        <table class="" id="tblsearchRejection" style="margin-bottom: 10px;">
                            <tr>
                                <%--<td>
                                    <div style="margin-top: 5px;" class="commontd">
                                        <b>
                                            <asp:Label ID="lblreason" runat="server" Text="<%$Resources:CommanResource,RejectionReason %>"></asp:Label></b>
                                    </div>
                                </td>--%>
                                 <td style="width:320px;">
                                     <input type="text" runat="server" clientidmode="static" id="txtRejectionRworkSearch" data-toggle="tooltip" autocomplete="off" class="searchdata form-control " title="search here !" placeholder="search Rejection/Rework Reason here..." />
                                </td>
                                <td style="color: white; padding: 5px; font-weight: bold;">Select Rejection/Rework</td>
                                <td>
                                    <asp:DropDownList ID="ddloption" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddloption_SelectedIndexChanged" ClientIDMode="Static">
                                        <asp:ListItem Value="Rejection">Rejection Reason</asp:ListItem>
                                        <asp:ListItem Value="Rework">Rework Reason</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="text-center">
                                    <asp:Button runat="server" Text='<%$Resources:CommanResource, Save %>' class="btn btn-info" ID="btnsaveRejection" OnClick="btnsaveRejection_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="grddiv" style="max-height: 90vh; overflow: auto; width: 100%">
                        <asp:HiddenField ID="hdfsavecheck" runat="server" />
                        <asp:ListView runat="server" ID="lvRejection">
                            <LayoutTemplate>
                                <table id="tblRejection" class="table table-bordered headerFixer">
                                    <thead class="blue">
                                        <tr>
                                            <th>Rejection Reason</th>
                                            <th>Interface ID</th>
                                            <th>Rejection Category </th>
                                            <th>Rejection Description</th>
                                            <th>Description In Hindi</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr runat="server" id="itemplaceholder"></tr>
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblRejectionReason" Text='<%# Eval("rejectionid") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hdfInterfaceID" ClientIDMode="Static" />
                                        <asp:Label runat="server" ID="lblRejectionID" Text='<%# Eval("interfaceid") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblRejectioncategory" Text='<%# Eval("Catagory") %>'></asp:Label>
                                    </td>
                                    <td class="update">
                                        <asp:TextBox runat="server" ID="txtDescription" ClientIDMode="Static" CssClass="txtboxcomman form-control txtupdate" Text='<%# Eval("rejectiondescription") %>'></asp:TextBox>
                                    </td>
                                    <td class="update">
                                        <asp:TextBox runat="server" ID="txtDescriptionInHindi" ClientIDMode="Static" CssClass="txtboxcomman form-control txtupdate" Text='<%# Eval("RejDescriptionHindi") %>'></asp:TextBox>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>

                    </div>

                    <div id="divrework" style="max-height: 90vh; overflow: auto; width: 100%">
                        <asp:HiddenField ID="hdfrework" runat="server" />
                        <asp:ListView runat="server" ID="lvRework">
                            <LayoutTemplate>
                                <table id="tblRework" class="table table-bordered headerFixer">
                                    <thead class="blue">
                                        <tr>
                                            <th>Rework Reason</th>
                                            <th>Interface ID</th>
                                            <th>Rework Category</th>
                                            <th>Rework Description</th>
                                            <th>Description In Hindi</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr runat="server" id="itemplaceholder"></tr>
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblReworkReson" Text='<%# Eval("Reworkid") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hdfInterfaceID" ClientIDMode="Static" />
                                        <asp:Label runat="server" ID="lblReworkInterfaceID" Text='<%# Eval("Reworkinterfaceid") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblReworkCategory" Text='<%# Eval("ReworkCatagory") %>'></asp:Label>
                                    </td>
                                    <td class="update">
                                        <asp:TextBox runat="server" ID="txtDescription" ClientIDMode="Static" CssClass="txtboxcomman form-control txtupdate" Text='<%# Eval("Reworkdescription") %>'></asp:TextBox>
                                    </td>
                                    <td class="update">
                                        <asp:TextBox runat="server" ID="txtDescriptionInHindi" ClientIDMode="Static" CssClass="txtboxcomman form-control txtupdate" Text='<%# Eval("ReworkDescriptionInHindi") %>'></asp:TextBox>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddloption" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
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

    <script type="text/javascript">
        $(document).ready(function () {
            $('#searchComponent').keyup(function () {
                searchTableComponent($(this).val());
            });

         $('#txtRejectionRworkSearch').keyup(function () {
            debugger;
            if ($('#ddloption :selected').text() == "Rejection Reason")
                searchRejectionTable($(this).val());
            else
                searchReworkTable($(this).val());
            });
        });

        //-------Search Start-------------------------
        $('#tblDownCodesInfo th').each(function (col) {
            debugger;
            $(this).hover(
                function () { $(this).addClass('focus'); },
                function () { $(this).removeClass('focus'); }

            );
            $(this).click(function () {
                var headerTxt = $(this).html().trim();
                if ($(this).is('.asc')) {
                    $(this).removeClass('asc');
                    $(this).addClass('desc');
                    sortOrder = -1;
                }
                else {
                    $(this).addClass('asc');
                    $(this).removeClass('desc');
                    sortOrder = 1;
                }

                $(this).siblings().removeClass('asc');
                $(this).siblings().removeClass('desc');
                var arrData = $('#tblDownCodesInfo').find('tbody >tr:has(td)').get();
                arrData.sort(function (a, b) {
                    var val1 = $(a).children('td').eq(col).text().toUpperCase();
                    var val2 = $(b).children('td').eq(col).text().toUpperCase();

                    if ($.isNumeric(val1) && $.isNumeric(val2))
                        return sortOrder == 1 ? val1 - val2 : val2 - val1;
                    else
                        return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                });
                $.each(arrData, function (index, row) {
                    $('#tblDownCodesInfo tbody').append(row);
                });
            });
        });

        $('#tblcomponentinfo th').each(function (col) {
            debugger;
            $(this).hover(
                function () { $(this).addClass('focus'); },
                function () { $(this).removeClass('focus'); }

            );
            $(this).click(function () {
                var headerTxt = $(this).html().trim();
                if ($(this).is('.asc')) {
                    $(this).removeClass('asc');
                    $(this).addClass('desc');
                    sortOrder = -1;
                }
                else {
                    $(this).addClass('asc');
                    $(this).removeClass('desc');
                    sortOrder = 1;
                }

                $(this).siblings().removeClass('asc');
                $(this).siblings().removeClass('desc');
                var arrData = $('#tblcomponentinfo').find('tbody >tr:has(td)').get();
                arrData.sort(function (a, b) {
                    var val1 = $(a).children('td').eq(col).text().toUpperCase();
                    var val2 = $(b).children('td').eq(col).text().toUpperCase();

                    if ($.isNumeric(val1) && $.isNumeric(val2))
                        return sortOrder == 1 ? val1 - val2 : val2 - val1;
                    else
                        return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                });
                $.each(arrData, function (index, row) {
                    $('#tblcomponentinfo tbody').append(row);
                });
            });
        });

        $('#tblRejection th').each(function (col) {
            debugger;
            $(this).hover(
                function () { $(this).addClass('focus'); },
                function () { $(this).removeClass('focus'); }

            );
            $(this).click(function () {
                var headerTxt = $(this).html().trim();
                if ($(this).is('.asc')) {
                    $(this).removeClass('asc');
                    $(this).addClass('desc');
                    sortOrder = -1;
                }
                else {
                    $(this).addClass('asc');
                    $(this).removeClass('desc');
                    sortOrder = 1;
                }

                $(this).siblings().removeClass('asc');
                $(this).siblings().removeClass('desc');
                var arrData = $('#tblRejection').find('tbody >tr:has(td)').get();
                arrData.sort(function (a, b) {
                    var val1 = $(a).children('td').eq(col).text().toUpperCase();
                    var val2 = $(b).children('td').eq(col).text().toUpperCase();

                    if ($.isNumeric(val1) && $.isNumeric(val2))
                        return sortOrder == 1 ? val1 - val2 : val2 - val1;
                    else
                        return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                });
                $.each(arrData, function (index, row) {
                    $('#tblRejection tbody').append(row);
                });
            });
        });

        $('#tblRework th').each(function (col) {
            debugger;
            $(this).hover(
                function () { $(this).addClass('focus'); },
                function () { $(this).removeClass('focus'); }

            );
            $(this).click(function () {
                var headerTxt = $(this).html().trim();
                if ($(this).is('.asc')) {
                    $(this).removeClass('asc');
                    $(this).addClass('desc');
                    sortOrder = -1;
                }
                else {
                    $(this).addClass('asc');
                    $(this).removeClass('desc');
                    sortOrder = 1;
                }

                $(this).siblings().removeClass('asc');
                $(this).siblings().removeClass('desc');
                var arrData = $('#tblRework').find('tbody >tr:has(td)').get();
                arrData.sort(function (a, b) {
                    var val1 = $(a).children('td').eq(col).text().toUpperCase();
                    var val2 = $(b).children('td').eq(col).text().toUpperCase();

                    if ($.isNumeric(val1) && $.isNumeric(val2))
                        return sortOrder == 1 ? val1 - val2 : val2 - val1;
                    else
                        return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                });
                $.each(arrData, function (index, row) {
                    $('#tblRework tbody').append(row);
                });
            });
        });

        //---------End Search-----------------------------------


        $('#search').keyup(function () {
            debugger;
            searchTable($(this).val());
        });

        function searchTable(inputVal) {
            debugger;
            var table = $('#tblDownCodesInfo');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td input');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).val())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }

        function searchRejectionTable(inputVal) {
            debugger;
            var table = $('#tblRejection');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td input');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).val())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }

        function searchReworkTable(inputVal) {
            debugger;
            var table = $('#tblRework');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td input');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).val())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }
        $("#tblDownCodesInfo").on("click", ".update", function () {
            $(this).closest('tr').find('input[type=hidden]').val("update");
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
        });

        function messageNotOk(msg) {
            Command: toastr["success"](msg, "<%=GetGlobalResourceObject("CommanResource","Save")%>")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-center",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }



        //------------------------------------ start Searching Code------------------------------//

        function searchTableComponent(inputVal) {
            var table = $('#tblcomponentinfo');
            inputVal = regExpReplace(inputVal);
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        let testValue = "";
                        testValue = $(td).find('.searchField').val();
                        if (testValue == undefined || testValue == "") {
                            testValue = $(td).find('.searchField').text();
                        }
                        testValue = regExpReplace(testValue);
                        if (regExp.test(testValue)) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }

        function regExpReplace(value) {
            const regex = /[.*+?^${}()|[\]\\]/i;
            value = value.replace(regex, '');
            return value;
        }
        //------------------------------------ End Searching Code------------------------------//

        function successMsg(msg, title) {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            toastr['success'](msg, title);
            clearModalScreen();
            return false;
        }

        $("#tblcomponentinfo").on("click", "td", function () {
            $(this).closest('tr').find('input[type=hidden]').val("update");
            $("#tblcomponentinfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblcomponentinfo tr td").find('.txtupdate').addClass("textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
        });

        function searchTableRework(inputVal) {
            var table = $('#tblRework');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).html())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }

        function searchRejectionTable(inputVal) {
            var table = $('#tblRejection');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).html())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }

        $("#tblRejection").on("click", ".update", function () {
            debugger;
            $(this).closest('tr').find('#hdfInterfaceID').val("update");
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
        });

        $("#tblRework").on("click", ".update", function () {
            debugger;
            $(this).closest('tr').find('#hdfInterfaceID').val("update");
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
        });

        function hieghtManage() {
            var winHeight = $(window).height();
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (630);
                console.log('min');
            } else {
                winHeight = (830);
                console.log('max');
            }
            if ($('[id$=ddloption]').val() == "Rejection Reason") {
                $("#grddiv").height(winHeight);
                $("#grddiv").attr('style', 'overflow-x: hidden; overflow-y: auto; min-height: 300px;max-height: ' + winHeight - 100 + 'px; width: 100%');
            }
            else if ($('[id$=ddloption]').val() == "Rework Reason") {
                $("#divrework").height(winHeight);
                $("#divrework").attr('style', 'overflow:auto; overflow-x: hidden; min-height: 300px;max-height: ' + winHeight - 100 + 'px; width: 100%');
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            debugger;
            $("#tblcomponentinfo").on("click", "td", function () {
                $(this).closest('tr').find('input[type=hidden]').val("update");
                $("#tblcomponentinfo tr td").find('.txtupdate').removeClass("form-control");
                $("#tblcomponentinfo tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });

            $("#tblDownCodesInfo").on("click", ".update", function () {
                $(this).closest('tr').find('input[type=hidden]').val("update");
                $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
                $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });

            $("#tblRejection").on("click", ".update", function () {
                debugger;
                $(this).closest('tr').find('#hdfInterfaceID').val("update");
                $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
                $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });

            $("#tblRework").on("click", ".update", function () {
                debugger;
                $(this).closest('tr').find('#hdfInterfaceID').val("update");
                $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
                $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });

            $('#searchComponent').keyup(function () {
                searchTableComponent($(this).val());
            });
            $('#txtRejectionRworkSearch').keyup(function () {
                debugger;
                if ($('#ddloption :selected').text() == "Rejection Reason")
                    searchRejectionTable($(this).val());
                else
                    searchReworkTable($(this).val());
                  
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
