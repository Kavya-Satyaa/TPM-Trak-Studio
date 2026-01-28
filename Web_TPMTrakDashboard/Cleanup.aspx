<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cleanup.aspx.cs" Inherits="Web_TPMTrakDashboard.Cleanup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/editDropDownJs") %>
    <%: Styles.Render("~/bundles/editDropDownCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>

    <style>
        rbn {
            font-size: medium;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            font-size: 15px;
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            font-weight: normal;
            height: 235px;
            color: white;
            width: 60%;
        }

        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            font-size: 15px;
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            font-weight: bold;
            height: 250px;
            color: white;
            width: 60%;
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

        .td-content {
            color: black;
            text-align: center;
        }

        .tblCleanUpdata {
            margin-left: 15px;
        }

            .tblCleanUpdata tbody tr td {
                padding: 0px 10px;
            }
    </style>


    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 1px;">
                <div class="row" style="margin-left: 1px; max-width: auto;">
                    <fieldset class="scheduler-border1" id="stdtime" style="height: auto;">
                        <legend class="scheduler-border commontd" style="height: auto; text-align: left;">Select the option</legend>
                        <asp:RadioButton CssClass="rbn" Text="Database Backup" ID="rbnDBBackup" GroupName="rbnOption" runat="server" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="rbnDBBackup_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton CssClass="rbn" Text="Table records cleanup" ID="rbnTblRecordsCleanup" GroupName="rbnOption" runat="server" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="rbnTblRecordsCleanup_CheckedChanged" />
                        <%--                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton CssClass="rbn" Text="Files cleanup" ID="rbnFilesCleanup" GroupName="rbnOption" runat="server" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="rbnFilesCleanup_CheckedChanged" />--%>
                    </fieldset>
                </div>

                <div class="row" style="margin-left: 1px; width: auto;">
                    <fieldset class="scheduler-border1" id="selectedOperation" style="width: 98%; height: auto; margin-right: 1px;">
                        <legend runat="server" id="Legend1" text="Select option" style="text-align: center" class="scheduler-border commontd"></legend>

                        <div id="DBBackup" runat="server">
                            <div style="box-shadow: 0px 0px 2px #ddd; background: #8aacb8;">
                                <div style="padding: 5px;">
                                    <table class="tblCleanUpdata">
                                        <tr>
                                            <td class="td-content">
                                                <asp:Label runat="server" Text="Database Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDBName" runat="server" ReadOnly="true" CssClass="form-control" Style="width: 250px;" ClientIDMode="Static" Text=""></asp:TextBox>
                                            </td>
                                            <td class="td-content">
                                                <asp:Label ID="lblFreq" runat="server" ClientIDMode="Static" Text="DB backup and maintainance frequency"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFreq" runat="server" ClientIDMode="Static" CssClass="form-control" Style="width: 100px; border-radius: 4px;" Text=""></asp:TextBox>
                                            </td>
                                            <td class="td-content">
                                                <asp:Label ID="lblDestination" runat="server" Style="display: inline;" Text="Destination folder:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtFolderPath" Style="width: 300px;" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAdd" Style="display: inline; border-radius: 4px; font-weight: bold;" runat="server" ClientIDMode="Static" Text="Add/Update" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="DBBackup">
                                <asp:GridView ID="gvDBBackup" OnRowDataBound="gvDBBackup_RowDataBound" runat="server" HeaderStyle-BackColor="#2e6886" BackColor="White" AutoGenerateColumns="false" CssClass="table table-bordered">
                                    <AlternatingRowStyle BackColor="#F2F2F2" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSlNo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Parameter" HeaderText="Operation Name" />
                                        <asp:BoundField DataField="ValueInText" HeaderText="Database Name" />
                                        <asp:BoundField DataField="ValueInText2" HeaderText="File Path" />
                                        <asp:BoundField DataField="ValueInInt" HeaderText="No of days" />
                                    </Columns>
                                    <HeaderStyle CssClass="HeaderCss " />
                                </asp:GridView>
                            </div>
                        </div>

                        <div id="tblRecordsUpdate" runat="server">
                            <div style="box-shadow: 0px 0px 2px #ddd; background: #8aacb8;">
                                <div style="padding: 5px;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <table class="tblCleanUpdata">
                                                <tr>
                                                    <td class="td-content">
                                                        <asp:Label runat="server" Text="Table to be Cleaned"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlRecords"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRecords_SelectedIndexChanged" CssClass="form-control" data-toggle="tooltip" Style="width: 250px;" ClientIDMode="Static">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="td-content">
                                                        <asp:Label runat="server" Text="No. Of Days"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDays" ClientIDMode="Static" CssClass="form-control allowNumber" Style="width: 100px;" runat="server"> </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnUpdate" Text="Update" runat="server" CssClass="btn btn-primary" Style="border-radius: 4px; font-weight: bold;" ClientIDMode="Static" OnClick="btnUpdate_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlRecords" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <br />
                            <br />
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>

                                    <asp:GridView ID="gvRecords" BackColor="White" HeaderStyle-BackColor="#2e6886" runat="server" ClientIDMode="Static" AutoGenerateColumns="false" CssClass="table table-bordered">
                                        <AlternatingRowStyle BackColor="#F2F2F2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("SlNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Table Name">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTableName" Text='<%# Eval("TableName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="No of days">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblNoOfDays" Text='<%# Eval("NoOfDaysToRetain") %>'></asp:Label> <%--ondblclick="labelDoubleClick(this)"--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lbDelete" CssClass="glyphicon glyphicon-trash DeletelinkBtn" ToolTip="Delete" OnClick="lbDelete_Click" CausesValidation="false"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle CssClass="HeaderCss " />
                                    </asp:GridView>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>

                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="rbnDBBackup" EventName="CheckedChanged" />--%>
        </Triggers>
    </asp:UpdatePanel>

     <div class="modal fade" id="myConfirmationModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                <div class="modal-content" style="border: 2px solid #1c1a47">
                    <%--<div class="modal-header" style="background-color: white; padding: 8px">
                        <h4 class="modal-title" style="color:#1c1a47;">Confirmation?</h4>
                    </div>--%>
                    <div class="modal-body">
                        <span id="confirmationmessageText" style="font-size: 17px;">Are you sure, you want to delete this group?</span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #1c1a47">
                        <input type="button" value="Yes" class="btn btn-info" id="saveConfirmYes" onserverclick="saveConfirmYes_ServerClick" runat="server" style="background-color:#1ea5d6; color: white" data-dismiss="modal" />
                        <input type="button" value="No" class="btn btn-info" id="saveConfirmNo" onclick="saveConfirmNo()" style="background-color: #1ea5d6; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>


    <script>

        //const rbnDBBackup = document.getElementById('rbnDBBackup');
        //const DBBackup = document.getElementById('DBBackup');


        //rbnDBBackup.addEventListener('click', function () {
        //    if (rbnDBBackup.click) {
        //        DBBackup.style.display = 'block';
        //    }
        //    else {
        //        DBBackup.style.display = 'none';
        //    }
        //});

       
        $($('#gvRecords tr')).on("dblclick", function () {
            debugger;
            $('#ddlRecords').val(($(this).find('#lblTableName').text()));
            $('#txtDays').val(($(this).find('#lblNoOfDays').text()));
        })

       <%-- function labelDoubleClick(label) {
            debugger;
            $(this).closest("tr");
            var textBox = document.getElementById('<%= txtDays.ClientID %>');
            debugger;
            if (label !== null) {
                textBox.value = label.innerText;
            }
        }--%>

        $("#btnAdd").on('click', function () {
            if ($("#txtFreq").val() == "") {
                alert('Please enter maintainance frequency');
                return false;
            }
            if ($("#txtFolderPath").val() == "") {
                alert('Please select destination folder');
                return false;
            }
            return true;
        });

        $("#btnUpdate").on('click', function () {
            if ($("#txtDays").val() == "") {
                alert('Please enter No. of days records to be retained.');
                return false;
            }
            return true;
        });
        $('.allowNumber').keypress(function (evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#btnAdd").on('click', function () {
                if ($("#txtFreq").val() == "") {
                    alert('Please enter maintainance frequency');
                    return false;
                }
                if ($("#txtFolderPath").val() == "") {
                    alert('Please select destination folder');
                    return false;
                }
                return true;
            });

            $("#btnUpdate").on('click', function () {
                if ($("#txtDays").val() == "") {
                    alert('Please enter No. of days records to be kept.');
                    return false;
                }
                return true;
            });
            $('.allowNumber').keypress(function (evt) {
                debugger;
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode < 48 || charCode > 57) {
                    return false;
                }
                return true;
            });


            $($('#gvRecords tr')).on("dblclick", function () {
                debugger;
                $('#ddlRecords').val(($(this).find('#lblTableName').text()));
                $('#txtDays').val(($(this).find('#lblNoOfDays').text()));
            })

        });

        


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
