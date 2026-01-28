<%@ Page Title="JH Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.PMMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .headerFixerTable tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        .headerFixerTable > tbody > tr:last-child > td {
            position: sticky;
            bottom: 0px;
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
            width: 70%;
            overflow: auto;
            margin-top: 15px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
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
            padding-top: 15px;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>
            <table id="tblfilter" class="table table-bordered" style="width: auto;">
                <tr>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Group</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlGroup" CssClass="form-control" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" Width="240" Style="height: 40px" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Machine</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlMachineid" CssClass="form-control" Width="240" Style="height: 40px" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="width: auto;">
                        <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>


                    </td>
                    <td class="commanTd" style="width: auto;">
                        <asp:FileUpload runat="server" ID="filePMDetails" CssClass="form-control" />

                    </td>

                    <td class="commanTd" style="width: auto;">

                        <asp:Button runat="server" Text="Import" CssClass="btn btn-info btn-sm displayCss" ID="btnImport" OnClick="btnImport_Click"></asp:Button>
                    </td>
                    <tr>
            </table>
            <div id="gridContainer" class="divGrid">
                <asp:GridView runat="server" ID="gvPMMaster" AutoGenerateColumns="False" ClientIDMode="Static"
                    CssClass="table table-bordered cockpit headerFixerTable " ShowHeaderWhenEmpty="true" OnRowEditing="gvPMMaster_RowEditing" OnRowDeleting="gvPMMaster_RowDeleting" OnRowUpdating="gvPMMaster_RowUpdating" ShowFooter="true" OnRowCancelingEdit="gvPMMaster_RowCancelingEdit">
                    <Columns>
                        <%--  <asp:TemplateField HeaderText="Sl. No.">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSlno" Text='<%#Eval("SlNo")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="lblSlno" Text='<%#Eval("SlNo")%>' ForeColor="White" />
                            </EditItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="PM Activity">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPMActivity" Text='<%#Eval("PMActivity")%>' ForeColor="White" />
                                <asp:HiddenField runat="server" ID="hfPMId" Value='<%#Eval("PMID")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="edttxtPMActivity" Text='<%#Eval("PMActivity")%>' CssClass="form-control"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="edthfPMId" Value='<%#Eval("PMID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="newtxtPMActivity" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No. of Cycle">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblNoofCycle" Text='<%#Eval("NoOfCycle")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtNoofCycle" onkeypress="return allowNumberic(event);" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled" Text='<%#Eval("NoOfCycle")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="newtxtNoofcycle" onkeypress="return allowNumberic(event);" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnEdit" CommandName="Edit" ToolTip="Edit" CssClass="glyphicon glyphicon-edit" Style="font-size: 20px; color: #46b8da"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnDelete" CommandName="Delete" ToolTip="Delete" CssClass="glyphicon glyphicon-trash " Style="font-size: 20px; color: #46b8da"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" ID="btnUpdate" CommandName="Update" ToolTip="Update" CssClass="glyphicon glyphicon-plus-sign" Style="font-size: 20px; color: #46b8da"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnCancel" CommandName="Cancel" ToolTip="Cancel" CssClass="glyphicon glyphicon-remove" Style="font-size: 20px; color: #46b8da"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" ID="btnInsert" CssClass="glyphicon glyphicon-plus-sign" ToolTip="New" Style="font-size: 20px; color: #46b8da" OnClick="btnInsert_Click"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="HeaderCss" HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                        <div style="height: 100%; background-color: white; text-align: center; color: red">No Data Found</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImport" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="modal fade" id="ConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Are you sure you want to delete this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <asp:Button runat="server" Text="Yes" ID="yesGridDeleteBtn" Style="width: 80px;" OnClick="yesGridDeleteBtn_Click" />
                    <asp:Button runat="server" Text="No" ID="noImgBtn" Style="width: 80px;" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            var Height = $(window).height() - (190);
            $('#gridContainer').css('height', Height);
        });
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("lblMessages").style.display = "none";
            }, 2000);

        };
        function openConfirmModal() {
            $('[id*=ConfirmModal]').modal('show');
        };
        function allowNumberic(evt) {

            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        $(window).resize(function () {
            var Height = $(window).height() - (190);
            $('#gridContainer').css('height', Height);
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                var Height = $(window).height() - (190);
                $('#gridContainer').css('height', Height);
            });
            $(window).resize(function () {
                var Height = $(window).height() - (190);
                $('#gridContainer').css('height', Height);
            });
            function allowNumberic(evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
