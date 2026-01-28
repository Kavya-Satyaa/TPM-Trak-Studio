<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadtoHMI.aspx.cs" Inherits="Web_TPMTrakDashboard.AmararajaMangal.UploadtoHMI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <style>
        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            /*min-width:115px;*/
        }

        #MainContent_gridavailableinfo tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #MainContent_gridavailableinfo tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_gridassignedinfo tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #MainContent_gridassignedinfo tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
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

        .iconcss {
            color: red;
            text-align: center;
        }

        #MainContent_catlsthide .multiselect {
            width: 240px;
            overflow: hidden;
        }

        #MainContent_catlsthide .multiselect {
            width: 240px;
            overflow: hidden;
        }

        #machddl .multiselect {
            width: 240px;
            overflow: hidden;
        }
    </style>

    <div class="container-fluid" style="">
        <asp:UpdatePanel ID="UpdatePanelHMI" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnCharLengthCount" ClientIDMode="Static" />
                <div class="row" style="height: 60px;">
                    <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                        <tr>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Type</td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static">
                                    <asp:ListItem Text="Down Code" Value="DownCode"></asp:ListItem>
                                    <asp:ListItem Text="Component" Value="Component"></asp:ListItem>
                                    <asp:ListItem Text="Down Category" Value="Category"></asp:ListItem>
                                    <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                                    <asp:ListItem Text="Rejection Code" Value="RejectionCode"></asp:ListItem>
                                    <asp:ListItem Text="Rejection Category" Value="RejectionCategory"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Plant</td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" />
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine</td>
                            <td style="width: 220px;" id="machddl">
                                <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control" ToolTip="<%$Resources:CommanResource, MachineTooltip %> " Width="150" ClientIDMode="Static"></asp:DropDownList>
                                <%--<asp:ListBox ID="lstMachine" CssClass="form-control" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource, MachineTooltip %> " Width="150" ClientIDMode="Static"></asp:ListBox>--%>
                             
                            </td>
                            <td class="commanTd" id="cathide" style="width: 122px; vertical-align: middle;" runat="server">Category</td>
                            <td style="width: 220px;" id="catlsthide" runat="server">
                                <asp:ListBox ID="lstcategory" CssClass="form-control" runat="server" SelectionMode="Multiple" ToolTip=" Category" ClientIDMode="Static"></asp:ListBox>
                            </td>
                            <td class="commanTd" id="rejCatHide" style="width: 122px; vertical-align: middle;" runat="server">Category</td>
                            <td style="width: 220px;" id="rejCatLstHide" runat="server">
                                <asp:ListBox ID="lbRejCategory" CssClass="form-control" runat="server" SelectionMode="Multiple" ToolTip=" Category" ClientIDMode="Static"></asp:ListBox>
                            </td>
                            <td style="width: 130px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" Style="min-width: 120px;" OnClick="btnView_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row text-center" style="margin-top: 20px;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <h3 style="margin-left: 10px;"><span id="msgheader" runat="server" style="color: white;" clientidmode="Static"></span></h3>
                        <div id="tblGrid1" class="row manageData" style="margin: 10px; height: 650px; overflow: auto;">
                            <asp:GridView ID="gridavailableinfo" CssClass="table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSlno" runat="server" Text='<%# Eval("Slno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="60">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chklst" runat="server" Style="float: left; padding-right: 5px" OnCheckedChanged="chklst_CheckedChanged" AutoPostBack="true" />
                                            <span>All </span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Checkcomp" runat="server" CssClass="checkss" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Machine ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmachineID" runat="server" Text='<%# Eval("MachineID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interface ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInterfaceID" runat="server" Text='<%# Eval("InterfaceID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComponentID" ClientIDMode="Static" runat="server" Text='<%# Eval("ComponentID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Down ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDownID" runat="server" Text='<%# Eval("DownID") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rejection ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRejectionID" runat="server" Text='<%# Eval("RejectionID") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Down Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDownCategory" runat="server" Text='<%# Eval("DownCategory") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rejection Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRejCategory" runat="server" Text='<%# Eval("RejectionCategory") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerID" runat="server" Text='<%# Eval("CustomerId") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="HeaderCss" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <h3 style="margin-left: 10px;"><span id="msgheader1" runat="server" style="color: white;" clientidmode="Static"></span></h3>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="BtnDelete" CssClass="btn btn-primary" Text="Delete" ClientIDMode="Static" OnClick="BtnDelete_Click" OnClientClick="return deleteValidation();" />
                                </td>
                            </tr>
                        </table>


                        <div id="tblGrid2" class="row manageData" style="margin: 10px; height: 650px; overflow: auto; margin-top: 0px;">
                            <asp:GridView ID="gridassignedinfo" CssClass="table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssSlno" runat="server" Text='<%# Eval("Slno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Machine ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssmachineID" runat="server" Text='<%# Eval("MachineID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Down Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssDownCategory" runat="server" Text='<%# Eval("DownCategory") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rejection Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssRejCategory" runat="server" Text='<%# Eval("RejectionCategory") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Down ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssDownID" runat="server" Text='<%# Eval("DownID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rejection ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssRejectionID" runat="server" Text='<%# Eval("RejectionID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interface ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssInterfaceID" runat="server" Text='<%# Eval("InterfaceID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssComponentID" runat="server" Text='<%# Eval("ComponentID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssCustomerId" runat="server" Text='<%# Eval("CustomerId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="60">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chklst" runat="server" Style="float: left; padding-right: 5px" OnCheckedChanged="chklst1_CheckedChanged" AutoPostBack="true" />
                                            <span>All </span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="iconcss">
                                                <%--  <asp:LinkButton ID="btndelte" runat="server" Width="30px" CssClass="iconcss" title="Delete" OnClick="btndelte_Click" OnClientClick="return confirm('Are you sure you want to delete this Assigned Machine ?')" meta:resourcekey="btndeltecustResource1"><i class="glyphicon glyphicon-trash"></i></asp:LinkButton>--%>
                                                <asp:CheckBox runat="server" ID="chkDelete" ClientIDMode="Static" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <HeaderStyle CssClass="HeaderCss" />
                            </asp:GridView>
                        </div>
                    </div>

                </div>
                <div style="margin: 10px; float: right" class="col-lg-7">
                    <asp:Button ID="btnAssign" runat="server" Text="Assign" CssClass="btn btn-info assign" OnClick="btnAssign_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        var rv = "";
        $(document).ready(function () {
            $('[id$=lstcategory]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbRejCategory]').multiselect({
                includeSelectAllOption: true
            });
            //$('[id$=lstMachine]').multiselect({
            //   includeSelectAllOption: true
            //});
            $(".assign").click(function () {
                //$("[id$=MainContent_gridavailableinfo] tr").each(function () {
                //    //$(this).find('.checkss input').children().is(':checked')
                //    if ($("#ddlType").val() == "Component") {
                //        if ($(this).find('.checkss').children().is(':checked')) {
                //            let compval = $(this).closest("tr").find("#lblComponentID").text();
                //            if (compval.length > 20) {
                //                rv = false;
                //                break;
                //            }
                //            else {
                //                rv = true;
                //            }
                //        }
                //    }
                //    else {
                //        rv = true;
                //    }
                //});
                var maxChar = parseInt($('#hdnCharLengthCount').val());
                var rows = $("[id$=MainContent_gridavailableinfo] tr");
                var rv = true;
                for (var i = 0; i < rows.length; i++) {
                    var row = $(rows)[i];
                    if ($(row).find('.checkss').children().is(':checked')) {
                        var character = "";
                        if ($("#ddlType").val() == "DownCode") {
                            character = $(row).find("#lblDownID").text();
                        }
                        else if ($("#ddlType").val() == "Component") {
                            character = $(row).find("#lblComponentID").text();
                        }
                        else if ($("#ddlType").val() == "Category") {
                            character = $(row).find("#lblDownCategory").text();
                        }
                        else if ($("#ddlType").val() == "Customer") {
                            character = $(row).find("#lblCustomerID").text();
                        }
                        else if ($("#ddlType").val() == "RejectionCode") {
                            character = $(row).find("#lblRejectionID").text();
                        }
                        else if ($("#ddlType").val() == "RejectionCategory") {
                            character = $(row).find("#lblRejCategory").text();
                        }
                        if (character.length > maxChar) {
                            rv = false;
                            break;
                        }
                    }
                }



                if (rv == false) {
                    alert($("#ddlType").val() + " cannot have characters greater than " + maxChar);
                    return rv;
                }
                else {
                    return rv;
                }

            });
        });
        function deleteValidation() {
            var chks = $('#MainContent_gridassignedinfo #chkDelete');
            var flag = 0;
            for (var i = 0; i < chks.length; i++) {
                if ($(chks[i]).prop("checked")) {
                    flag = 1;
                    break;
                }
            }
            if (flag == 0) {
                alert("Please select Record.");
                return false;
            }
            return confirm('Are you sure you want to delete this Assigned Machine ?');
        }


        //function check(element) {
        //    if ($("#ddlType").val() == "Component") {
        //        debugger;
        //        if ($(element).find("input").prop("checked")) {
        //            let compval = $(element).closest("tr").find("#lblComponentID").text();
        //            if (compval.length > 4) {
        //                alert("ComponentId cannot have characters greater than 20");
        //            }
        //        }
        //    }
        //}

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('[id$=lstcategory]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbRejCategory]').multiselect({
                includeSelectAllOption: true
            });
            //$('[id$=lstMachine]').multiselect({
            //  includeSelectAllOption: true
            //});

            $(".assign").click(function () {
                //$("[id$=MainContent_gridavailableinfo] tr").each(function () {
                //    //$(this).find('.checkss input').children().is(':checked')
                //    if ($("#ddlType").val() == "Component") {
                //        if ($(this).find('.checkss').children().is(':checked')) {
                //            let compval = $(this).closest("tr").find("#lblComponentID").text();
                //            if (compval.length > 20) {
                //                rv = false;
                //              
                //            }
                //            else {
                //                rv = true;
                //            }
                //        }
                //    }
                //    else {
                //        rv = true;
                //    }
                //});
                var maxChar = parseInt($('#hdnCharLengthCount').val());
                var rows = $("[id$=MainContent_gridavailableinfo] tr");
                var rv = true;
                for (var i = 0; i < rows.length; i++) {
                    var row = $(rows)[i];
                    if ($(row).find('.checkss').children().is(':checked')) {
                        var character = "";
                        if ($("#ddlType").val() == "DownCode") {
                            character = $(row).find("#lblDownID").text();
                        }
                        else if ($("#ddlType").val() == "Component") {
                            character = $(row).find("#lblComponentID").text();
                        }
                        else if ($("#ddlType").val() == "Category") {
                            character = $(row).find("#lblDownCategory").text();
                        }
                        else if ($("#ddlType").val() == "Customer") {
                            character = $(row).find("#lblCustomerID").text();
                        }
                        else if ($("#ddlType").val() == "RejectionCode") {
                            character = $(row).find("#lblRejectionID").text();
                        }
                        else if ($("#ddlType").val() == "RejectionCategory") {
                            character = $(row).find("#lblRejCategory").text();
                        }
                        if (character.length > maxChar) {
                            rv = false;
                            break;
                        }
                    }
                }


                if (rv == false) {
                    alert($("#ddlType").val() + " cannot have characters greater than " + maxChar);
                    return rv;
                }
                else {
                    return rv;
                }

            });

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
