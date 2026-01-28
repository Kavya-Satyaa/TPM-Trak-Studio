<%@ Page Title="Employee Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeInformation.aspx.cs" Inherits="Web_TPMTrakDashboard.EmployeeInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        #MainContent_lvEmployeeInfo_itemPlaceholderContainer th, #MainContent_lvEmployeeInfo_Td2 {
            color: white;
            background-color: #2E6886 !important;
        }

        #MainContent_lvEmployeeInfo_itemPlaceholderContainer tbody tr:nth-child(odd) td {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_lvEmployeeInfo_itemPlaceholderContainer tbody tr:nth-child(even) td {
            background-color: #FFFFFF;
            color: black;
        }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }

        .dropdown {
            -webkit-appearance: none;
        }
    </style>

    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <div style="text-align: center">
        <asp:HiddenField ID="hdfCondition" runat="server" />
        <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri; color: white;"></asp:Label>
    </div>
    <div class="col-lg-6" style="text-align: left; display: block; margin-bottom: 10px;">
        <%--   <input runat="server" clientidmode="static" type="text" id="searchEmployee" data-toggle="tooltip" title="search !" placeholder="search .." class="form-control" style="width: 250px; display: inline;">--%>
        <table>
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="ddlField" ClientIDMode="Static" CssClass="form-control">
                        <asp:ListItem Text="Employee ID" Value="EmployeeID"></asp:ListItem>
                        <asp:ListItem Text="Name" Value="Name"></asp:ListItem>
                        <asp:ListItem Text="Interface ID" Value="InterfaceID"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFieldSearch" ClientIDMode="Static" CssClass="form-control" placeholder="search .."></asp:TextBox>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnView" CssClass="btn btn-info" Text="View" OnClick="btnView_Click" />
                </td>
                <td style="padding-left: 30px;">
                    <asp:Button ID="btnAdd" runat="server" Text="<%$Resources:CommanResource,New %>" CssClass="btn btn-info" Style="display: inline; margin-left: 0px" />

                    <asp:Button ID="btnCancel" runat="server" Text="<%$Resources:CommanResource,Cancel %>" CssClass="btn btn-info" Style="display: inline; display: none; margin-left: 0px" />

                    <asp:Button ID="btnSave" runat="server" Text="<%$Resources:CommanResource,Save %>" CssClass="btn btn-info" Style="display: inline;" OnClick="btnSave_Click" />
                    </td>
                <td style="padding-left: 30px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-info" Style="display: inline; background-color: #1c701c;" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="griddata" style="overflow: auto; min-height: 800px; width: 100%;">
        <asp:ListView ID="lvEmployeeInfo" runat="server" DataKeyNames="employeeid" InsertItemPosition="FirstItem" class="table table-bordered table-hover"
            OnItemDataBound="OnItemDataBound" OnItemCreated="lvEmployeeInfo_ItemCreated" OnItemDeleting="lvEmployeeInfo_ItemDeleting" OnItemInserting="lvEmployeeInfo_ItemInserting">

            <%--<EditItemTemplate>
            <tr style="background-color: #008A8C; color: #FFFFFF;">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                </td>
                <td>
                    <asp:TextBox ID="txtemployeeid" runat="server" Text='<%# Bind("employeeid") %>' />
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' />
                </td>
                <td>
                    <asp:TextBox ID="txtinterfaceid" runat="server" Text='<%# Bind("interfaceid") %>' />
                </td>
            </tr>
        </EditItemTemplate>--%>
            <EmptyDataTemplate>
                <table id="Table1" runat="server">
                    <tr>
                        <td>No data was returned.
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>


            <InsertItemTemplate>
                <tr class="addtablerow" style="display: none">
                    <td>
                        <asp:HiddenField ID="hdfUpade" runat="server" />
                        <asp:TextBox ID="txtemployeeid" CssClass="form-control" runat="server" Text='<%# Bind("employeeid") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server" Text='<%# Bind("Name") %>' />
                    </td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txtinterfaceid" MaxLength="40" CssClass="form-control txtinter" Style="width: 90px" runat="server" Text='<%# Bind("interfaceid") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtdesignation" CssClass="form-control" runat="server" Text='<%# Bind("designation") %>' />
                    </td>
                    <td style="width: 90px">
                        <asp:TextBox ID="txtqualification" CssClass="form-control" Style="width: 80px" runat="server" Text='<%# Bind("qualification") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtaddress" CssClass="form-control" runat="server" Text='<%# Bind("address1") %>' />
                    </td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txtphone" MaxLength="12" CssClass="form-control numberValue" Style="width: 100px" runat="server" Text='<%# Bind("phone") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" Text='<%# Bind("Email") %>' />
                    </td>
                    <td style="text-align: center; width: 80px">
                        <asp:CheckBox ID="chkEdit" runat="server" CssClass="chkCompany" Checked='<%# Bind("company_default") %>' />
                    </td>
                    <td>
                        <asp:ListBox ID="ddlMultiPlantId" CssClass="multiDropdown" runat="server" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEmployeeRole" CssClass="form-control" runat="server" Style="width: 200px">
                            <asp:ListItem Text="Operator" Value="Operator" />
                            <asp:ListItem Text="Production Engineer" Value="Production Engineer" />
                            <asp:ListItem Text="Supervisor" Value="Supervisor" />
                            <asp:ListItem Text="Inspector" Value="Inspector" />
                            <asp:ListItem Text="QA Engineer" Value="QA Engineer" />
                            <asp:ListItem Text="QA Admin" Value="QA Admin" />
                            <asp:ListItem Text="Machine Shop Admin" Value="Machine Shop Admin" />
                            <asp:ListItem Text="Planning Admin" Value="Planning Admin" />
                            <asp:ListItem Text="Sr. Engineer" Value="Sr Engineer" />
                            <asp:ListItem Text="Manager" Value="Manager" />
                            <asp:ListItem Text="HOD" Value="HOD" />
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: center; width: 80px">
                        <asp:CheckBox ID="chkIsActiveEdit" runat="server" CssClass="chkIsActive" Checked='<%# Bind("IsActiveStatus") %>' />
                    </td>
                    <td style="text-align: center">
                        <asp:Button ID="InsertButton" CssClass="btn btn-info" ValidationGroup="HolidayRegistration" CommandName="Insert" Style="display: none"
                            runat="server" Text="Save" />
                        <asp:Button ID="btnUpadte" CssClass="btn btn-info" ValidationGroup="HolidayRegistration" CommandName="Update" OnClick="lvEmployeeInfo_ItemUpdating"
                            runat="server" Text="Update" Style="display: none" />
                        <%--   <asp:Button ID="btnClear" CssClass="btn btn-info" runat="server" OnClientClick="javascript:resetValues();" Text="Clear" />--%>
                        <%--   <asp:Button ID="btnDelete" OnClientClick="javascript:return confirm('Are you sure to delete this record?');"
                        runat="server" CommandName="Delete" Text="Delete" />--%>
                    </td>
                </tr>
            </InsertItemTemplate>

            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdfUpade" runat="server" ClientIDMode="AutoID" />
                        <asp:Label ID="lblemployeeid" CssClass="textboxcommon" runat="server" Text='<%# Bind("employeeid") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" CssClass="txtupdate textboxcommon" runat="server" Text='<%# Bind("Name") %>' />
                    </td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txtinterfaceid" CssClass="txtupdate textboxcommon txtinter" MaxLength="40" Style="width: 90px" runat="server" Text='<%# Bind("interfaceid") %>' />
                        <asp:HiddenField runat="server" ID="hfPassword" Value='<%# Bind("Password") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtdesignation" CssClass="txtupdate textboxcommon" runat="server" Text='<%# Bind("designation") %>' /></td>
                    <td style="width: 90px">
                        <asp:TextBox ID="txtqualification" CssClass="txtupdate textboxcommon" Style="width: 80px" runat="server" Text='<%# Bind("qualification") %>' /></td>
                    <td>
                        <asp:TextBox ID="txtaddress" CssClass="txtupdate textboxcommon" runat="server" Text='<%# Bind("address1") %>' /></td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txtphone" MaxLength="12" CssClass="txtupdate textboxcommon numberValue" Style="width: 100px" runat="server" Text='<%# Bind("phone") %>' /></td>
                    <td>
                        <asp:TextBox ID="txtEmail" CssClass="txtupdate textboxcommon" runat="server" Text='<%# Bind("Email") %>' /></td>
                    <td style="text-align: center; width: 80px">
                        <asp:CheckBox ID="chkEdit" runat="server" CssClass="chkCompany" Checked='<%# Bind("company_default") %>' /></td>
                    <td>
                        <asp:Label ID="hdfPlantID" runat="server" Text='<%# Eval("SMSTextTemplate") %>' Visible="false"></asp:Label>
                        <asp:ListBox ID="ddlMultiPlantId" CssClass="txtupdate textboxcommon dropdown multiDropdown" runat="server" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td>
                        <asp:Label ID="hdfEmployeeRole" runat="server" Text='<%# Eval("EmployeeRole") %>' Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlEmployeeRole" CssClass="form-control ddlupdate" runat="server" Style="width: 200px">
                            <asp:ListItem Text="Operator" Value="Operator" />
                            <asp:ListItem Text="Production Engineer" Value="Production Engineer" />
                            <asp:ListItem Text="Supervisor" Value="Supervisor" />
                            <asp:ListItem Text="Inspector" Value="Inspector" />
                            <asp:ListItem Text="QA Engineer" Value="QA Engineer" />
                            <asp:ListItem Text="QA Admin" Value="QA Admin" />
                            <asp:ListItem Text="Machine Shop Admin" Value="Machine Shop Admin" />
                            <asp:ListItem Text="Planning Admin" Value="Planning Admin" />
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: center; width: 80px">
                        <asp:CheckBox ID="chkIsActiveEdit" runat="server" CssClass="chkIsActive" Checked='<%# Bind("IsActiveStatus") %>' />
                    </td>
                    <td style="text-align: center">
                        <%--  <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" />--%>
                        <%-- <asp:Button ID="btnDelete" CssClass="btn btn-info btn-sm" OnClientClick="javascript:return confirm('Are you sure to delete this record?');"
                        runat="server" CommandName="Delete" Text="Delete" />--%>

                        <asp:LinkButton ID="lnkRemove" runat="server"
                            CommandArgument='<%# Eval("employeeid")%>' CommandName='<%# Eval("interfaceid")%>'
                            Text="Delete" OnClick="DeleteDownCat"></asp:LinkButton>
                        <%--  OnClientClick="return confirm('Do you want to delete?')"--%>
                    </td>
                </tr>
            </ItemTemplate>

            <LayoutTemplate>
                <table id="Table2" runat="server" class="headerFixer">
                    <tr id="Tr1" runat="server">
                        <td id="Td1" runat="server">
                            <table id="itemPlaceholderContainer" runat="server" class="table table-bordered">
                                <thead class="blue">
                                    <tr id="Tr2" runat="server">
                                        <th id="Th2" runat="server"><%=GetGlobalResourceObject("CommanResource","EmployeeID") %>
                                        </th>
                                        <th id="Th3" runat="server"><%=GetGlobalResourceObject("CommanResource","Name") %>
                                        </th>
                                        <th style="width: 100px" id="Th4" runat="server"><%=GetGlobalResourceObject("CommanResource","InterfaceID") %>
                                        </th>
                                        <th id="Th5" runat="server"><%=GetGlobalResourceObject("CommanResource","Designation") %>
                                        </th>
                                        <th style="width: 100px" id="Th6" runat="server"><%=GetGlobalResourceObject("CommanResource","Qualification") %>
                                        </th>
                                        <th id="Th7" runat="server"><%=GetGlobalResourceObject("CommanResource","Address") %>
                                        </th>
                                        <th style="width: 100px" id="Th8" runat="server"><%=GetGlobalResourceObject("CommanResource","Phone") %>
                                        </th>
                                        <th id="Th9" runat="server"><%=GetGlobalResourceObject("CommanResource","Email") %>
                                        </th>
                                        <th id="Th10" style="width: 80px" runat="server"><%=GetGlobalResourceObject("CommanResource","CompanyDefault") %>
                                        </th>
                                        <th id="Th11" runat="server"><%=GetGlobalResourceObject("CommanResource","PlantID") %>
                                        </th>
                                        <th id="Th12" runat="server" style="width: 150px;"><%=GetGlobalResourceObject("CommanResource","Role") %>
                                        </th>
                                        <th id="Th13" runat="server" style="text-align: center"><%=GetGlobalResourceObject("CommanResource","IsActive") %></th>
                                        <th id="Th1" runat="server" style="text-align: center"><%=GetGlobalResourceObject("CommanResource","Action") %></th>
                                    </tr>
                                </thead>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr id="Tr3" runat="server" style="position: sticky; bottom: 0;">
                        <td id="Td2" runat="server" style="text-align: center; padding: 5px;">
                            <asp:DataPager ID="dpgHoliday" OnPreRender="lvEmployeeInfo_PreRender" PageSize="100" PagedControlID="lvEmployeeInfo"
                                runat="server">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowFirstPageButton="True" ShowNextPageButton="False"
                                        ShowPreviousPageButton="False" />
                                    <asp:NumericPagerField />
                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowLastPageButton="True" ShowNextPageButton="False"
                                        ShowPreviousPageButton="False" />
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <%--<SelectedItemTemplate>
            <tr style="background-color: #008A8C; font-weight: bold; color: #FFFFFF;">
                <td></td>
                <td>
                    <asp:Label ID="lblemployeeid" runat="server" Text='<%# Eval("employeeid") %>' />
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' />
                </td>
                <td>
                    <asp:Label ID="lblinterfaceid" runat="server" Text='<%# Eval("interfaceid") %>' />
                </td>
                <td>
                    <asp:Label ID="lbldesignation" runat="server" Text='<%# Eval("designation") %>' />
                </td>
                <td>
                    <asp:Label ID="lblqualification" runat="server" Text='<%# Eval("qualification") %>' />
                </td>
                <td>
                    <asp:Label ID="lbladdress" runat="server" Text='<%# Eval("address1") %>' />
                </td>
                <td>
                    <asp:Label ID="lblphone" runat="server" Text='<%# Eval("phone") %>' />
                </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="chkShow" runat="server" Checked='<%# Eval("company_default") %>' />
                </td>
                <td>
                    <asp:Label ID="lblPlantID" runat="server" Text='<%# Eval("SMSTextTemplate") %>'></asp:Label>
                </td>
            </tr>
        </SelectedItemTemplate>--%>
        </asp:ListView>
    </div>
    <script type="text/javascript">

        ///------------------------------------Searching Code------------------------------
        $(document).ready(function () {
            $.unblockUI({});
            $('#searchEmployee').keyup(function () {
                searchTable($(this).val());
            });
            freezeColumnFromLeft('MainContent_lvEmployeeInfo_itemPlaceholderContainer', 3);
        });

        var winHeight = $(window).height();
        var maxheight = winHeight
        console.log(winHeight);
        if (winHeight < 650) {
            winHeight = (650);
            console.log('min');
        } else {
            winHeight = (850);
            console.log('max');
        }
        $("#griddata").height(maxheight - 125);

        function searchTable(inputVal) {
            var table = $('#MainContent_lvEmployeeInfo_itemPlaceholderContainer');
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

        $("[id$=btnAdd]").click(function () {
            $(".addtablerow").css("display", "");
            $("[id$=btnCancel]").css("display", "");
            $("[id$=hdfCondition]").val("Save");
            $("[id$=btnAdd]").css("display", "none");
            $("[id$=txtemployeeid]").focus();
            return false;
        });

        $("[id$=btnSave]").click(function () {
            debugger;
            if ($("[id$=hdfCondition]").val() == "Save") {
                if ($("[id$=txtemployeeid]").val() == "") {
                    alert("Please enter the Employee ID !!");
                    $("[id$=txtemployeeid]").focus();
                    return false;
                }
                if ($("[id$=txtName]").val() == "") {
                    alert("Please enter the Employee Name !!");
                    $("[id$=txtName]").focus();
                    return false;
                }
                if ($("[id$=txtinterfaceid]").val() == "") {
                    alert("Please enter the Interface ID !!");
                    $("[id$=txtinterfaceid]").focus();
                    return false;
                }
                var inter = parseInt($("[id$=txtinterfaceid]").val());
                if (inter == 0) {
                    alert(" Interface ID Cannot be Zero!!");
                    $("[id$=txtinterfaceid]").focus();
                    return false;
                }
                //if ($("[id$=ddlMultiPlantId]").val() == null) {
                //    alert("Please select the Plnat ID !!");
                //    $("[id$=ddlMultiPlantId]").focus();
                //    return false;
                //}
            }
            if ($("[id$=hdfCondition]").val() == "Update") {
                var count = 0;

                $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr:gt(0)").each(function (src, i) {

                    if (src != ($("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr").length - 2)) {
                        if ($(this, i).closest("tr").find("input[type=hidden]").val() == "update") {
                            if (($(this, i).closest("tr").find('td:eq(1) input').val() == "")) {
                                count++;
                                alert('Please enter the Name !!');
                                $(this, i).closest("tr").find('td:eq(1) input').removeClass("textboxcommon");
                                $(this, i).closest("tr").find('td:eq(1) input').addClass("form-control");
                                $(this, i).closest("tr").find('td:eq(1) input').focus();
                                return false;
                            }
                            if (($(this, i).closest("tr").find('td:eq(2) input').val() == "")) {
                                count++;
                                alert('Please enter the Interface ID !!');
                                $(this, i).closest("tr").find('td:eq(2) input').removeClass("textboxcommon");
                                $(this, i).closest("tr").find('td:eq(2) input').addClass("form-control");
                                $(this, i).closest("tr").find('td:eq(2) input').focus();
                                return false;
                            }
                            var inter = parseInt($(this, i).closest("tr").find('td:eq(2) input').val());
                            if (inter == 0) {
                                count++;
                                alert("Interface ID Cannot be Zero!!!!");
                                $("[id$=txtinterfaceid]").focus();
                                return false;
                            }
                            //if (($(this, i).closest("tr").find('.dropdown').val() == null)) {
                            //    count++;
                            //    alert('Please select the Plant ID !!');
                            //    $(this, i).closest("tr").find('.dropdown').focus();
                            //    return false;
                            //}
                        }
                    }
                });
                if (count != 0) {
                    return false;
                }
                count = 0;
                $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr:gt(0)").each(function () {
                    if ($(this).closest("tr").find("input[type=hidden]").val() == "update") {
                        count++;
                    }
                });
                if (count == 0) {
                    alert('Please atleast one record update !!');
                    return false;
                }
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $("[id$=btnCancel]").click(function () {
            $(".addtablerow").css("display", "none");
            $("[id$=btnCancel]").css("display", "none");
            $("[id$=hdfCondition]").val("Update");
            $("[id$=btnAdd]").css("display", "");
            return false;
        })

        $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr").on("click", "", function () {
            debugger;
            //$(this).closest('tr').find('input[type=hidden]').val("select");
            //$(this).find('input[type=hidden]').val("update");
            $(this).find('[id$=hdfUpade]').val("update");
            if (this.value != undefined)
                this.value = this.value.replace(/[^0-9\.]/g, '');
        });

        $("[id*=MainContent_lvEmployeeInfo_lnkRemove_]").click(function () {
            if (confirm('Do you want to delete?')) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            }
            else return false;
        });

        $("[id$=btnUpadte]").click(function () {

            var count = 0;
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr:gt(0)").each(function (src, i) {
                if (src != ($("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr").length - 2)) {
                    if ($(this, i).closest("tr").find("input[type=hidden]").val() == "update") {
                        if (($(this, i).closest("tr").find('td:eq(1) input').val() == "")) {
                            count++;
                            alert('Please enter the Name !!');
                            $(this, i).closest("tr").find('td:eq(1) input').removeClass("textboxcommon");
                            $(this, i).closest("tr").find('td:eq(1) input').addClass("form-control");
                            $(this, i).closest("tr").find('td:eq(1) input').focus();
                            return false;
                        }
                        if (($(this, i).closest("tr").find('td:eq(2) input').val() == "")) {
                            count++;
                            alert('Please enter the Interface ID !!');
                            $(this, i).closest("tr").find('td:eq(2) input').removeClass("textboxcommon");
                            $(this, i).closest("tr").find('td:eq(2) input').addClass("form-control");
                            $(this, i).closest("tr").find('td:eq(2) input').focus();
                            return false;
                        }
                        if (($(this, i).closest("tr").find('.dropdown').val() == null)) {
                            count++;
                            alert('Please select the Plant ID !!');
                            $(this, i).closest("tr").find('.dropdown').focus();
                            return false;
                        }
                    }
                }

            });
            if (count != 0) {
                return false;
            }
            count = 0;
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr:gt(0)").each(function () {
                if ($(this).closest("tr").find("input[type=hidden]").val() == "update") {
                    count++;
                }
            });
            if (count == 0) {
                alert('Please update atleast one record !!');
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $("[id$=InsertButton]").click(function () {
            if ($("[id$=txtemployeeid]").val() == "") {
                alert("Please enter the Employee ID !!");
                $("[id$=txtemployeeid]").focus();
                return false;
            }
            if ($("[id$=txtName]").val() == "") {
                alert("Please enter the Employee Name !!");
                $("[id$=txtName]").focus();
                return false;
            }
            if ($("[id$=txtinterfaceid]").val() == "") {
                alert("Please enter the Interface ID !!");
                $("[id$=txtinterfaceid]").focus();
                return false;
            }

            //if ($("[id$=ddlMultiPlantId]").val() == null) {
            //    alert("Please select the Plnat ID !!");
            //    $("[id$=ddlMultiPlantId]").focus();
            //    return false;
            //}
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });
        var countValue = 0; var i = 0;
        $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer").on("change", ".txtinter", function () {

            var friValue = $(this).closest('td').find('.txtinter').val();
            var index = $(this).parent().parent().index();
            var i = 1;
            if ($("#MainContent_btnAdd")[0].style.display == "none") {
                i = 1;
            }
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer .txtinter").each(function () {

                var value = $(this).val();
                if (index != i) {
                    if (friValue == value) {
                        countValue++;
                    }
                }
                i++;
            });
            if (countValue >= 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","DuplicateInfIdChk")%>");
                $(this).closest('td').find('input').val('');
                countValue = 0;
                return false;
            }
        });

        //$("#MainContent_lvEmployeeInfo_itemPlaceholderContainer").on("click", ".multiselect", function () {
        //    $(this).closest('tr').find('input[type=hidden]').val("update");
        //    $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.multiselect').removeClass("form-control");
        //    $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.multiselect').addClass("multiselect");
        //    $(this).closest('td').find('button').removeClass("multiselect");
        //    $(this).closest('td').find('button').addClass("form-control");
        //});


        $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer").on("click", ".txtupdate", function () {

            $("[id$=hdfCondition]").val("Update");
            //$(this).closest('tr').find('input[type=hidden]').val("update");
            $(this).closest('tr').find('[id$=hdfUpade]').val("update");
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').removeClass("form-control");
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').addClass("textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
        });

        $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer").on("click", ".ddlupdate", function () {
            $("[id$=hdfCondition]").val("Update");
            //$(this).closest('tr').find('input[type=hidden]').val("update");
            $(this).closest('tr').find('[id$=hdfUpade]').val("update");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
        });

        $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer").on("change", ".dropdown", function () {
            /*$(this).closest('tr').find('input[type=hidden]').val("update");*/
            $(this).closest('tr').find('[id$=hdfUpade]').val("update");
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').removeClass("form-control");
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').addClass("textboxcommon");
        });

        $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer").on("click", ".chkCompany", function () {

            $("[id$=hdfCondition]").val("Update");
            //$(this).closest('tr').find('input[type=hidden]').val("update");
            $(this).closest('tr').find('[id$=hdfUpade]').val("update");
            var id = ($(this).closest('tr').index());
            var i = 1;
            $('#MainContent_lvEmployeeInfo_itemPlaceholderContainer .chkCompany').each(function () {

                if (id != i) {
                    $(this).find('input[type=checkbox]')[0].checked = false;
                    /* $(this).parent().parent().parent().closest('tr').find('input[type=hidden]').val("update");*/
                    $(this).parent().parent().parent().closest('tr').find('[id$=hdfUpade]').val("update");
                }
                i++;
            });

            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').removeClass("form-control");
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').addClass("textboxcommon");
        });

        $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer").on("click", ".chkIsActive", function () {

            $("[id$=hdfCondition]").val("Update");
            //$(this).closest('tr').find('input[type=hidden]').val("update");
            $(this).closest('tr').find('[id$=hdfUpade]').val("update");
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').removeClass("form-control");
            $("#MainContent_lvEmployeeInfo_itemPlaceholderContainer tr td").find('.txtupdate').addClass("textboxcommon");
        });

        $('.multiDropdown').multiselect({
            includeSelectAllOption: true
        });

        function resetValues(index) {
            var lvholiday = document.getElementById('<%= lvEmployeeInfo.ClientID %>' + '_itemPlaceholderContainer');

            if (lvholiday != null) {
                var elementArray = lvholiday.getElementsByTagName('input');
                for (var i = 0; i < elementArray.length; i++) {
                    var elementRef = elementArray[i];
                    if (elementRef.type == 'text') {
                        ele.value = "";
                    }
                }
                return false;
            }
            return false;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            freezeColumnFromLeft('MainContent_lvEmployeeInfo_itemPlaceholderContainer', 3);
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
