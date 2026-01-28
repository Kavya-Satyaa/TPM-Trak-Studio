<%@ Page Title="Add Consumer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddConsumer.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.AddConsumer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>
    <style type="text/css">
        .headerFixerTable tr th {
            position: sticky;
            top: 0px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 100%;
            overflow: auto;
            margin-top: 15px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
            }

               .divGrid td {
               
                color: white;
                height:20px;
            }
        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        .table tbody > tr > th {
            vertical-align: middle;
            width:auto;
        }

        .table > tr > td {
            vertical-align: middle;
        }
        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 15px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }


        .table .lbl {
            padding-top: 10px;
        }

        #tblfilter tr td {
            vertical-align: middle;
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
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="gridAddCustomer" OnRowDataBound="gridAddCustomer_RowDataBound" OnRowCommand="gridAddCustomer_RowCommand" ShowFooter="true" AutoGenerateColumns="false" CssClass="table table-bordered headerFixerTable">
                    <Columns>
                        <asp:TemplateField HeaderText=" Plant ID">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPlantID" Text='<%# Bind("PlantID") %>' ForeColor="White" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ID="cmbFooterPlantID" CssClass="form-control" OnSelectedIndexChanged="cmbFooterPlantID_SelectedIndexChanged" AutoPostBack="true" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" User ID">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblUserID" Text='<%# Bind("UserID") %>' ForeColor="White"/>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ID="cmbFooterUserID" AutoPostBack="true" OnSelectedIndexChanged="cmbFooterUserID_SelectedIndexChanged" CssClass="form-control" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Email-ID">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEmail" Text='<%# Bind("Email") %>' ForeColor="White"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtEmail" Text='<%# Bind("Email") %>' onblur="return checkEmailValidation(this,'')" TextMode="Email" CssClass="form-control" />
                                <asp:HiddenField runat="server" ID="hfEmailValidation"  ClientIDMode="Static" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtFooterEmail" TextMode="Email" onblur="return checkEmailValidation(this,'Footer')" CssClass="form-control" />
                                       <asp:HiddenField runat="server" ID="hfFooterEmailValidation" ClientIDMode="Static" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Phone No">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPhoneNo" Text='<%# Bind("Phone") %>' ForeColor="White"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtPhoneNo" onkeypress="return allowNumberic(this,event);" Text='<%# Bind("Phone") %>' TextMode="Phone" CssClass="form-control" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtFooterPhoneNo" onkeypress="return allowNumberic(this,event);" TextMode="Phone" CssClass="form-control" />
                            </FooterTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="Telegram Group ID">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblChatID" Text='<%# Bind("ChatID") %>' ForeColor="White"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtChatID"  Text='<%# Bind("ChatID") %>' CssClass="form-control" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtFooterChatID"  CssClass="form-control" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="linkeditRow" runat="server" CommandName="EditRow" CommandArgument='<%# Bind("SLNO") %>' CssClass="glyphicon glyphicon-edit" ToolTip="Edit" Style="font-size: 20px;"/>
                                <asp:LinkButton ID="linkDeleteRow" runat="server" CommandName="DeleteRow" CommandArgument='<%#Bind("SLNO") %>' CssClass="glyphicon glyphicon-trash "
                                    ToolTip="Delete" Style="font-size: 20px;"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="linkUpdateRow" runat="server" CommandName="UpdateRow" CommandArgument='<%# Bind("SLNO") %>' CssClass="glyphicon glyphicon-save"  ToolTip="Update" Style="font-size: 20px;"/>
                                <asp:LinkButton ID="linkCancelRow" runat="server" CommandName="CancelRow" CommandArgument='<%#Bind("SLNO") %>' CssClass="glyphicon glyphicon-remove" ToolTip="Cancel" Style="font-size: 20px;"/>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="linkAddRow" runat="server" CommandName="AddRow" CommandArgument='<%# Bind("SLNO") %>' CssClass="glyphicon glyphicon-plus-sign"  ToolTip="New" Style="font-size: 20px;"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        function allowNumberic(element,evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ($(element).val().length >= 10) {
                return false;
            }
            if ((charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function checkEmailValidation(element, param) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if ($(element).val() != "") {
                if (!regex.test($(element).val())) {
                    if (param == "Footer") {
                        $(element).closest('tr').find('#hfFooterEmailValidation').val("1");
                    } else {
                        $(element).closest('tr').find('#hfEmailValidation').val("1");
                    }
                    alert("Please eneter valid EmailId.");
                } else {
                    if (param == "Footer") {
                        $(element).closest('tr').find('#hfFooterEmailValidation').val("0");
                    } else {
                        $(element).closest('tr').find('#hfEmailValidation').val("0");
                    }
                }
            } else {
                if (param == "Footer") {
                    $(element).closest('tr').find('#hfFooterEmailValidation').val("0");
                } else {
                    $(element).closest('tr').find('#hfEmailValidation').val("0");
                }
            }
          
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            function allowNumberic(element, evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if ($(element).val().length >= 10) {
                    return false;
                }
                if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            function checkEmailValidation(element, param) {
                var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if ($(element).val() != "") {
                    if (!regex.test($(element).val())) {
                        if (param == "Footer") {
                            $(element).closest('tr').find('#hfFooterEmailValidation').val("1");
                        } else {
                            $(element).closest('tr').find('#hfEmailValidation').val("1");
                        }
                        alert("Please eneter valid EmailId.");
                    } else {
                        if (param == "Footer") {
                            $(element).closest('tr').find('#hfFooterEmailValidation').val("0");
                        } else {
                            $(element).closest('tr').find('#hfEmailValidation').val("0");
                        }
                    }
                } else {
                    if (param == "Footer") {
                        $(element).closest('tr').find('#hfFooterEmailValidation').val("0");
                    } else {
                        $(element).closest('tr').find('#hfEmailValidation').val("0");
                    }
                }

            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
