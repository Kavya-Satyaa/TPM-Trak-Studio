<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComponentInformation.aspx.cs" Inherits="Web_TPMTrakDashboard.Component_Information" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .textboxcss {
            border: none;
            background-color: transparent;
            color: black;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        .select {
            /*border-radius: 5px;*/
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

        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        /*.form-control {
            color: #000;
        }*/

        .form-control {
            color: black;
        }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }


        .select {
            /*border-radius: 5px;*/
            -webkit-appearance: none;
        }

        /*.thead-inverse th {
            color: #fff;
            background-color: #2E6886;
        }*/

        /*#tblcomponentinfo tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #tblcomponentinfo tr:nth-child(even) {
            background-color: #DCDCDC;
        }*/


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

        #btnexport {
            background-color: #1c701c;
        }
    </style>

    <div class="row">
        <asp:HiddenField runat="server" ID="hdnCompList" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfInterfaceDataType" ClientIDMode="Static" />
        <table class="table table-bordered" id="tblsearch" style="display: inline-block; width: auto; margin-left: 10px">
            <tr>
                <%--<td>
                    <div class="col-md-12">
                        <input type="text" runat="server" clientidmode="static" id="search" data-toggle="tooltip" autocomplete="off" class="searchdata form-control " title="search here !" placeholder="search component here..." visible="false" />
                        <%--title="<%=GetGlobalResourceObject("CommanResource","SearchHere") %>" placeholder="<%=GetLocalResourceObject("SearchComp") %>"--%>
                   <%-- </div>
                </td>--%>
                 <td>
                    <div class="commontd" style="margin-top: 5px;">Search</div>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSearchDropdown" runat="server" CssClass="form-control" ClientIDMode="Static" Style="display: inline-block; width: auto">
                        <asp:ListItem Value="Description">Description</asp:ListItem>
                        <asp:ListItem Value="componentid">Component ID</asp:ListItem>
                        <asp:ListItem Value="InterfaceID">Interface ID</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtComponentSearch" AutoCompleteType="Disabled" CssClass="form-control" OnTextChanged="txtComponentSearch_TextChanged" Placeholder="Search"></asp:TextBox>
                </td>
                <%-- <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="btn btn-primary btncolor" OnClick="btnSearch_Click" />
                </td>--%>
                <td>
                    <div class="commontd" style="margin-top: 5px;">Sort Order</div>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSortOrderName" runat="server" CssClass="form-control" ClientIDMode="Static" Style="display: inline-block; width: auto">
                        <asp:ListItem Value="componentid">Component ID</asp:ListItem>
                        <asp:ListItem Value="InterfaceID">Interface ID</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlSortOrderType" runat="server" CssClass="form-control" ClientIDMode="Static" Style="display: inline-block; width: auto">
                        <asp:ListItem Value="asc">Asc</asp:ListItem>
                        <asp:ListItem Value="desc">Desc</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <div style="float: right">
                        <asp:HiddenField runat="server" ID="hdnKTAFlag" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="BtnView" CssClass="btn btn-info" ClientIDMode="Static" OnClick="BtnView_Click" Text="View" />
                        &nbsp;&nbsp;
                    </div>
                </td>
                <td runat="server" id="tdOperations">
                <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-info" OnClientClick="return openNewEntryModal();" />
                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-info" ClientIDMode="Static" OnClick="BtnSave_Click" OnClientClick="return ValidateInterfaceID();" Text='<%$Resources:CommanResource, Save  %>' />
                    <input type="button" value="<%=GetGlobalResourceObject("CommanResource", "Delete")  %>" class="btn btn-info" id="btndelete" style="visibility: collapse; display: none" />
                    <asp:Button runat="server" Text="<%$ Resources:CommanResource, Export %>" class="btn btn-info" ID="btnexport" ClientIDMode="Static" meta:resourcekey="btnexportResource1" OnClick="btnexport_Click" OnClientClick="return storeComponentID();"></asp:Button>

                </td>
            </tr>
        </table>
        <fieldset class="masterFS" style="margin-left: 5px !important; margin-right: 4px !important; display: inline-block" runat="server" id="masterFS">
            <legend>Select Excel file to import Master</legend>
            <div style="display: inline-block">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:CheckBox runat="server" ID="chkUpdateComponent" Checked="true" ClientIDMode="Static" Text="Update Comp." ForeColor="White" />
                        &nbsp;&nbsp;
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
        <div style="width: 95%">
            <div class="row text-center">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; color: red; font-family: Calibri; font-size: x-large;" meta:resourcekey="lblMessagesResource1"></asp:Label>
            </div>
            <div id="griddata" style="overflow-x: auto; overflow-y: auto; min-height: 700px; width: 100%; margin-left: 10px;">
                <asp:ListView runat="server" ID="lvComponentInfo" OnPagePropertiesChanging="lvComponentInfo_PagePropertiesChanging" OnItemDataBound="lvComponentInfo_ItemDataBound">
                    <LayoutTemplate>
                        <table id="tblcomponentinfo" class="table table-bordered table-hover headerFixer">
                            <thead class="blue" runat="server">
                                <tr>
                                    <th runat="server" id="thComponentID"><%=settings.Where(x=>x.ValueInText=="ComponentID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                    <th runat="server" id="thInterfaceID"><%=settings.Where(x=>x.ValueInText=="InterfaceID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                    <th runat="server" id="thCustomer"><%=settings.Where(x=>x.ValueInText=="Customer").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                    <th runat="server" id="thDescription"><%=settings.Where(x=>x.ValueInText=="Description").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                                    <th runat="server" id="thWeightColumn">Weight</th>
                                    <th runat="server" id="thPartFamilyColumn">Part Family</th>
                                    <th runat="server" id="thPartTypeColumn">Part Type</th>
                                    <th runat="server" id="Delete"><%=settings.Where(x=>x.ValueInText=="Delete").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
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
                                <th runat="server" id="thWeightColumn">Weight</th>
                                <th runat="server" id="thPartFamilyColumn">Part Family</th>
                                <th runat="server" id="thPartTypeColumn">Part Type</th>
                                <th runat="server" id="Delete"><%=settings.Where(x=>x.ValueInText=="Delete").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HiddenField runat="server" ID="hdfInterface" />
                                <asp:Label runat="server" ID="lblComp" CssClass="txtComp createhyperlink searchField" Text='<%# Eval("componentid") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtIntefaceID" ClientIDMode="Static" CssClass="txtinter txtupdate textboxcommon searchField" Style="text-align: left;" Text='<%# Eval("InterfaceID") %>'></asp:TextBox>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdfCustomer" ClientIDMode="Static" Value='<%# Eval("customerid") %>' />
                                <asp:DropDownList runat="server" ID="ddlCustomer" CssClass="catValue txtupdate textboxcommon select login-Admin-header-gradient"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDescription" CssClass="txtdes txtupdate textboxcommon" Style="text-align: left; width: 100%;" Text='<%# Eval("description") %>'></asp:TextBox>
                            </td>
                            <td runat="server" visible='<%# Eval("WeightColumnVisible") %>'>
                                <asp:TextBox runat="server" ID="txtWeight" CssClass=" txtupdate textboxcommon allowNumber txtWeight" Style="text-align: left;" Text='<%# Eval("InputWeight") %>'></asp:TextBox>
                            </td>
                            <td runat="server" visible='<%# Eval("PartFamilyIsVisible") %>'>
                                <asp:HiddenField runat="server" ID="hdfPartFamily" ClientIDMode="Static" Value='<%# Eval("PartFamily") %>' />
                                <asp:DropDownList runat="server" ID="ddlPartFamily" CssClass="catValue txtupdate textboxcommon select login-Admin-header-gradient"></asp:DropDownList>
                            </td>
                            <td runat="server" visible='<%# Eval("PartTypeIsVisible") %>'>
                                <asp:HiddenField runat="server" ID="hdnPartType" ClientIDMode="Static" Value='<%# Eval("PartType") %>' />
                                <asp:DropDownList runat="server" ID="ddlPartType" CssClass="catValue txtupdate textboxcommon select login-Admin-header-gradient">
                                    <asp:ListItem Value="Main" Text="Main"></asp:ListItem>
                                    <asp:ListItem Value="Child" Text="Child"></asp:ListItem>
                                    <asp:ListItem Value="Assembly" Text="Assembly"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="BtnDelete" Text="Delete" CssClass="btn btn-info" OnClick="BtnDelete_Click" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>

                <%--<table id="tblcomponentinfo" class="table table-bordered table-hover headerFixer">
                    <thead class="blue">
                        <tr>
                           <th><%=GetGlobalResourceObject("CommanResource", "ComponentID") %></th>
                            <th><%=GetGlobalResourceObject("CommanResource", "InterfaceID") %> </th>
                            <th><%=GetGlobalResourceObject("CommanResource", "Customer")%></th>
                            <th><%=GetGlobalResourceObject("CommanResource", "Description") %> </th>
                            <th><%=GetGlobalResourceObject("CommanResource", "Delete")  %></th>
                            <th><%=settings.Where(x=>x.ValueInText=="ComponentID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                            <th><%=settings.Where(x=>x.ValueInText=="InterfaceID").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                            <th><%=settings.Where(x=>x.ValueInText=="Customer").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                            <th><%=settings.Where(x=>x.ValueInText=="Description").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                            <th class="weightColumn">Weight</th>
                            <th class="Partfamily">Part Family</th>
                            <th><%=settings.Where(x=>x.ValueInText=="Delete").Select(x=>x.ValueInText2).FirstOrDefault() %></th>
                        </tr>
                    </thead>
                </table>--%>
            </div>
        </div>
    </div>
    <div class="modal infoModal" id="NewEntryScreen" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 950px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="newEditModalTitle" runat="server">Add New Component</h4>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                        <table style="width: 100%; margin: auto" class="modal-tbl addcomponent" id="addcomponent" clientidmode="static">
                            <tr>
                                <td style="padding-left: 20px">Component ID</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtComponentID" ClientIDMode="Static" CssClass="txtComp form-control"></asp:TextBox>
                                </td>
                                <td style="padding-left: 20px">Interface ID</td>
                                <td>
                                   <%-- <asp:TextBox runat="server" ID="txtInterfaceID" ClientIDMode="Static" CssClass="txtinter form-control"></asp:TextBox>--%>
                                     <asp:TextBox runat="server" ID="txtInterfaceID" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 20px">Customer</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCustomer" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td style="padding-left: 20px">Description</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trPartFamily" visible="false">
                                <td style="padding-left: 20px">Part Family</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPartFamily" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trPartType" runat="server">
                                <td style="padding-left: 20px">Part Type</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPartType" ClientIDMode="Static" CssClass="form-control">
                                        <asp:ListItem Value="Main" Text="Main"></asp:ListItem>
                                        <asp:ListItem Value="Child" Text="Child"></asp:ListItem>
                                        <asp:ListItem Value="Assembly" Text="Assembly"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trWeight" runat="server">
                                <td style="padding-left:20px">Weight</td>
                                <td>
                                   <asp:TextBox runat="server" ID="txtNewWeight" CssClass="form-control allowNumber"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnNewSave" ClientIDMode="Static" Text="Save" CssClass="btn btn-primary btn-style " OnClick="btnNewSave_Click" />
                    <button type="button" class="btn btn-style cancel-btn" onclick="CloseNewEditModal();">Cancel</button>

                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="deleteConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <span style="color: black">Are you sure you want to Delete this Record?</span>
                    <input type="hidden" id="hdnDeleteCompID" />
                    <input type="hidden" id="hdnDeleteInterfaceID" />
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" style="width: 80px;" onclick="deleteComponentIDConfirm();">Yes</button>
                    <button type="button" style="width: 80px;" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;" clientidmode="static">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Warning!</h4>
                </div>
                <div class="modal-body">
                    <span id="lblWarningMsg" style="color: black"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" id="warningOk" style="width: 80px;" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>



    <script>
        $(document).ready(function () {
            // $.unblockUI({});
            var winHeight = $(window).height();
            console.log(winHeight);
            $("#griddata").height(winHeight - 200);
            debugger;
            //if ($("#hfInterfaceDataType").val() == "Numeric") {
            //    const txtin = document.querySelectorAll(".txtinter");
            //    txtin.forEach(function (txtinterface) {
            //        txtinterface.classList.add("allowNumber");
            //        //txtinterface.classList.add("NumberOnly");
            //    });
            //}
            //else {
            //    const txtin = document.querySelectorAll(".txtinter");
            //    txtin.forEach(function (txtinterface) {
            //        txtinterface.classList.remove("allowNumber");
            //        //txtinterface.classList.remove("NumberOnly");
            //    });
            //}
        });
        $("#txtIntefaceID").keypress(function (evt) {
            debugger;
            if ($("#hfInterfaceDataType").val() == "Numeric") {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 48 && pos == 0) {
                    return false
                } else if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
        });
        $("#txtInterfaceID").keypress(function (evt) {
            debugger;
            if ($("#hfInterfaceDataType").val() == "Numeric") {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 48 && pos == 0) {
                    return false
                } else if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
        });
        function storeComponentID() {
            return true;
        }

        function ValidateInterfaceID() {
            debugger;
            var count = 0;
            $("#tblcomponentinfo tr:gt(0)").each(function (src, i) {
                if ($(this).closest("tr").find(".hdfInterface").val() == "update") {
                    if (($(this, i).closest("tr").find('.txtinter').val() == "")) {
                        count++;
                    }
                    if (count != 0) {
                        alert('<%=GetLocalResourceObject("PlzEnterInterface")%>');
                        return false;
                    }
                }
            });
            return true;
        }


        function onSave(saveResponce) {
            var result = saveResponce.d;
            $("[id$=lblMessages]").text(result);
            $("[id$=lblMessages]").css("color", "white");
            $("#btnnew").val("<%=GetGlobalResourceObject("CommanResource","New") %>");
        }

        function checkIsNumeric(element) {
            var intRegex = /^\d+$/;
            var str = $(element).val();
            if (!intRegex.test(str)) {
                $(element).val("");
                alert('Value should be numeric.');
            }
        }

        function deleteComponentID(btn) {
            var compid = $(btn).closest('tr').find('#lblCompID').text();
            var interfaceid = $(btn).closest('tr').find('#txtInterfaceID').val();
            $('#hdnDeleteCompID').val(compid);
            $('#hdnDeleteInterfaceID').val(interfaceid);
            $('#deleteConfirmModal').modal('show');
        }


        //------------------------------------ start Searching Code------------------------------//
        $(document).ready(function () {
            $('#search').keyup(function () {
                searchTable($(this).val());
            });
        });

        function searchTable(inputVal) {
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

        function isParticularCustomerPageEnabled(customerPageName) {
            var isAMPageEnabled = false;
            $.ajax({
                async: false,
                type: "POST",
                url: "ComponentInformation.aspx/isCustomerPageEnabled",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{customerPageKeyName:"' + customerPageName + '"}',
                success: function (response) {
                    isAMPageEnabled = response.d;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "Login.aspx";
                }
            });
            return isAMPageEnabled;
        }

        function openNewEntryModal() {
            debugger;
            $("#txtComponentID").text("");
            $("#txtInterfaceID").text("");
            $("#txtDescription").text("");
            if ($('#hdnKTAFlag').val() == "1") {
                $("#trPartFamily").show();
            }
            else {
                $("#trPartFamily").hide();
            }
            $("#NewEntryScreen").modal('show');
            return false;
        }

        function OpenNewEntryScreen() {
            $("#NewEntryScreen").modal('show');
        }

        function CloseNewEditModal() {
            $("#txtComponentID").val("");
            $("#txtInterfaceID").val("");
            $("#txtDescription").val("");
            if ($('#hdnKTAFlag').val() == "1") {
                $("#trPartFamily").show();
            }
            else {
                $("#trPartFamily").hide();
            }
            $("#NewEntryScreen").modal('hide');
        }


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

        $("#addcomponent").on("focusout", ".txtinter", function () {
            var friValue = $(this).closest('td').find('.txtinter').val();
            var list = $("#tblcomponentinfo .txtinter");
            for (var i = 0; i < list.length; i++) {
                if (list[i].value == friValue) {
                    alert("<%=GetLocalResourceObject("DuplicateInfIdChk")%>");
                    break;
                }
            }
        });

        $("#tblcomponentinfo").on("click", '.createhyperlink', function () {
            //debugger;
            var value = $(this).closest('tr').children('td:first').text().trim();
            var interID = $(this).closest('tr').find('.txtinter').val().trim();
            value = encodeURIComponent(value);
            var url = "ComponentOperationInformation.aspx?name=" + value + "&intefaceID=" + interID;
            if (isParticularCustomerPageEnabled("AmararagaMangalPages") == true) {
                url = "AmararajaMangal/COPInformation.aspx?name=" + value + "&intefaceID=" + interID;
            }
            else if (isParticularCustomerPageEnabled("AlliedPages") == true) {
                url = "Allied/COPAllied.aspx?name=" + value + "&intefaceID=" + interID;
            }

            window.open(url, "Component Operation Information");
        });

        $("#tblcomponentinfo").on("click", "td", function () {
            $(this).closest('tr').find('input[type=hidden]').val("update");
            $("#tblcomponentinfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblcomponentinfo tr td").find('.txtupdate').addClass("select");
            $("#tblcomponentinfo tr td").find('.txtupdate').addClass("textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcommon");
            $(this).closest('td').find('select').removeClass("select");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('input[type="button"]').removeClass("form-control");
        });



        $('.allowNumber').keypress(function (evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $('.allowNumber').keypress(function (evt) {
                debugger;
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode < 48 || charCode > 57) {
                    return false;
                }
                return true;
            });
        })

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
