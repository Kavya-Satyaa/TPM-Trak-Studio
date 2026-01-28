<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineShiftTypeAssociation.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.MachineShiftTypeAssociation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
     <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .labelStyle
        {
            color:white;
        }
        .main-table
        {
           background-color: #fff;
           width: 100%;
        }
        .main-table tr td{
            border-right:1px solid #ddd !important;
        }
        .inner-tbl thead>tr>th, 
        .inner-tbl tbody>tr>th, 
        .inner-tbl tfoot>tr>th, 
        .inner-tbl thead>tr>td, 
        .inner-tbl tbody>tr>td, 
        .inner-tbl tfoot>tr>td 
        {
            border:unset !important;
        }

        .table-headerborder
        {
            background-color: #70afcf;
            box-shadow: 1px 3px 5px 1px #000000;
            border-radius: 3px;
            height: 66px;
            
        }
        .table-headerborder tbody>tr>td
        {
            vertical-align:middle !important;
            border-top: none !important;
            font-size:15px;
            font-weight:bold;
        }
        .btn-style,.btn-style:hover{
            background-color:#215b6c;
        }

        .multiselect-container
        {
            height: 45vh;
            overflow: auto;
        }

    </style>
      <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table class="table table-headerborder" style="width:90%">
                    <tr>
                        <td class="labelStyle">Plant</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="labelStyle">Cell</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCell" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="labelStyle">Machine</td>
                        <td>
                           <%-- <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control">
                            </asp:DropDownList>--%>
                            <asp:ListBox runat="server" ID="lstMachine" ClientIDMode="Static" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td class="labelStyle">Shift Type</td>
                        <td>
                             <asp:DropDownList ID="ddlShiftType" runat="server" CssClass="form-control" ClientIDMode="Static" >
                             </asp:DropDownList>
                        </td>
                        <td>
                             <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-info btn-style" OnClick="btnView_Click" Style="display: inline" />
                        </td>
                         <td>
                             <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-info btn-style" OnClick="btnSave_Click" Style="display: inline" />
                        </td>
                    </tr>
                </table>
                <div style="height: 80vh; overflow: auto;" id="gridContainer" class="col-lg-10">
                       <asp:ListView runat="server" ID="lvMachineShiftTypeAssoc" ClientIDMode="Static">
                        <LayoutTemplate>
                            <table class="table table-bordered headerFixer" id="tblMachineShiftTypeAssoc">
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="background-color:<%# Eval("HeaderColor") %>;color:<%# Eval("ForeHeaderColor") %>;min-width:300px;">
                                    <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("Machine") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hfDate" Value=' <%# Eval("EffDate") %>' />
                                </td>
                                <td style="padding: 0; border-top: unset; border-right: unset; border-left: unset" class="tdclass">
                                    <asp:ListView runat="server" ID="lvShiftTypeDetails" DataSource='<%# Eval("machineShiftslst") %>'>
                                        <LayoutTemplate>
                                            <table class="main-table">
                                                <tr>
                                                    <td id="itemplaceholder" runat="server"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td style="padding: 0; border: unset; vertical-align: inherit; border-right: 1px solid white; width: 200px;background-color:<%# Eval("HeaderColor") %>" class="ShiftTypeInfoTd">
                                                <table class="inner-tbl  value-header-tbl">
                                                    <tr style="visibility:<%# Eval("HeaderVisible") %>;background-color:#2e6886">
                                                        <th>
                                                            <%# Eval("ShiftType") %>  &nbsp;&nbsp;
                                                            <asp:CheckBox runat="server" ID="chkAllShiftType" ClientIDMode="Static" ForeColor="White" style="vertical-align:top" Checked='<%# Eval("HeaderChecked") %>' onchange="return CheckAllShiftType(this);" CssClass="chkBtn"/>
                                                        </th>
                                                    </tr>
                                                     <tr style="visibility:<%# Eval("ContentVisible") %>">
                                                        <td>
                                                              <asp:HiddenField runat="server" ID="hfShiftType" Value=' <%# Eval("ShiftType") %>' />
                                                              <asp:RadioButton ID="rbShiftType" GroupName="ShiftType" runat="server" Checked='<%# Eval("ISShiftTypeChecked") %>' Style="border-bottom: none" CssClass="rmnRadioBtns" onchange="return ShiftTypeRadioBtnClick(this);" />
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

              </ContentTemplate>
          </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            setLeftFreeze();

            $('[id$=lstMachine]').multiselect({
                includeSelectAllOption: true
            });
        });

        function CheckAllShiftType(evt) {
            $(".chkBtn").find('input').prop("checked", false);
            $(evt).find('input').prop('checked', true);

            let checkedIndex = $('.chkBtn').index(evt);
            let tblOuter = $('.main-table');
            for (var i = 1; i < tblOuter.length; i++) {
                let tbl = tblOuter[i];
                let tds = $(tbl).find('tr td.ShiftTypeInfoTd');
                let selectedtd = tds[checkedIndex];
                let radiosmaple = $(selectedtd).find('.rmnRadioBtns input');
                if (radiosmaple.length > 0) {
                    for (var j = 0; j < tds.length; j++) {
                        let innertd = tds[j];
                        let radio = $(innertd).find('.rmnRadioBtns input');
                        $(radio).prop("checked", false);
                        if (checkedIndex == j) {
                            if ($(evt).find('input').prop("checked")) {
                                $(radio).prop("checked", true);
                            }
                            else
                                $(radio).prop("checked", false);
                        }
                    }
                }
            }
        }

        function ShiftTypeRadioBtnClick(radio) {
            $(radio).closest('.main-table').find('.rmnRadioBtns input').prop('checked', false);
            $(radio).find('input').prop('checked', true);

            let count = 0;
            let index = $(radio).closest('td.ShiftTypeInfoTd').index();
            let tblOuter = $('.main-table');
            for (var i = 1; i < tblOuter.length; i++) {
                let tbl = tblOuter[i];
                let tds = $(tbl).find('tr td.ShiftTypeInfoTd');
                for (var j = 0; j < tds.length; j++) {
                    let innertd = tds[j];
                    let radio = $(innertd).find('.rmnRadioBtns input');
                    if (index == j) {
                        if ($(radio).prop("checked")) {
                            count++;
                        }
                    }
                }
            }

            let tbl = tblOuter[0];
            let tds = $(tbl).find('tr td.ShiftTypeInfoTd');
            let innertd = tds[index];
            $('.chkBtn input').prop("checked", false);
            if (tblOuter.length - 1 == count) {
                $(innertd).find('input').prop('checked', true);
            }
            else {
                $(innertd).find('input').prop('checked', false);
            }

            //let tblOuter = $('.main-table');
            //let tbl = tblOuter[0];
            //let tds = $(tbl).find('tr td.ShiftTypeInfoTd');
            //for (var j = 0; j < tds.length; j++) {
            //    let innertd = tds[j];
            //    if (tblOuter.length - 1 == $(innertd).find('.rmnRadioBtns input:checked').length) {
            //        $(innertd).find('.chkBtn input').prop("checked", true);
            //    }
            //    else {
            //        $(innertd).find('.chkBtn input').prop("checked", false);
            //    }
            //}

        }

        function setLeftFreeze() {
            let tblRow = $('[id$=tblMachineShiftTypeAssoc] tr');
            if (tblRow.length > 0) {
                let rowth = tblRow[0];
                $(rowth).css({ "position": "sticky", "top": "0px", "font-weight": "bold" });

                //let tblOuter = $('.main-table');
                //let tr = $(tblOuter[0]).find('tr td');
                //$(tr).css({ "padding-left": "80px" });

            }
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setLeftFreeze();
            $('[id$=lstMachine]').multiselect({
                includeSelectAllOption: true
            });

            function ShiftTypeRadioBtnClick(radio) {
                $(radio).closest('.main-table').find('.rmnRadioBtns input').prop('checked', false);
                $(radio).find('input').prop('checked', true);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
