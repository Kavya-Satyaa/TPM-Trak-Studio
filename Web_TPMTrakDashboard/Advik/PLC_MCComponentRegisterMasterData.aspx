<%@ Page Title="PLC MC Component Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PLC_MCComponentRegisterMasterData.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.PLC_MCComponentRegisterMasterData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.js"></script>
    <link href="Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.min.js"></script>
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
        <div>
            <div  class="table table-bordered col-lg-4" style="width: auto;margin:5px">
                <table >
                    <tr>
                        <td  class="commanTd" style="min-width: 50px;">
                            Machine ID
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control"/>
                        </td>
                        <td >
                            <asp:Button Text="View" runat="server" OnClick="btnView_Click" ID="btnView"  CssClass="btn btn-info btn-sm displayCss"/>
                        </td>
                    </tr>
                </table>
            </div>
            <div   class="table table-bordered col-lg-4" style="width: auto;margin:5px;">
                <table>
                    <tr>
                        <th>
                            <asp:FileUpload runat ="server" ID="fileupload" CssClass="form-control" Width="275"/>
                        </th>
                        <th>
                             <asp:Button runat="server" id="btnupload"   OnClick="btnupload_Click" Text="Import"  CssClass="btn btn-info btn-sm displayCss"/>
                        </th>
                    </tr>
                </table>
            </div>
        </div>
        <div style="margin-top:20px">
            <asp:UpdatePanel runat="server" style="margin:5px">
                <ContentTemplate>

                    <asp:GridView runat="server" ID="gridcompregidter" OnRowDataBound="gridcompregidter_RowDataBound" AutoGenerateColumns="False" OnRowCommand="gridcompregidter_RowCommand" CssClass="table table-bordered headerFixerTable"  ShowHeaderWhenEmpty="true" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="SLNO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSLNO" Text='<%# Bind("SLNO") %>' Width="80" ForeColor="White" style="text-align:center;vertical-align:central;"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine-ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%# Bind("MachineID") %>' ForeColor="White" style="text-align:center;vertical-align:central;" Width="180"/>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="cmbFooterMachineID" CssClass="form-control" Width="180"/>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperation" Text='<%# Bind("Operation") %>' Width="80" ForeColor="White" style="text-align:center;vertical-align:central;"/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtOperation" Text='<%# Bind("Operation") %>' Width="80" CssClass="form-control"/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtFooterOperation"  Width="80" CssClass="form-control"/>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Event Type">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEventType" Text='<%# Bind("EventType") %>' ForeColor="White" style="text-align:center;vertical-align:central;" Width="100"/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="cmbEventType" CssClass="form-control" Width="150">
                                        <asp:ListItem Text="Cycle Data" Value="CycleData" />
                                        <asp:ListItem Text="Poke-Yoke" Value="Poke-Yoke" />
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="cmbFooterEventType" CssClass="form-control" Width="150">
                                        <asp:ListItem Text="Cycle Data" Value="CycleData" />
                                        <asp:ListItem Text="Poke-Yoke" Value="Poke-Yoke" />
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Event ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEventID" Text='<%# Bind("EventID") %>' ForeColor="White" style="text-align:center;vertical-align:central;" Width="150" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="cmbFooterEventID" CssClass="form-control">
                                        <asp:ListItem Value="CycleStart" Text="Cycle Start" />
                                        <asp:ListItem Value="PartOK" Text="Part-OK" />
                                        <asp:ListItem Value="PartNotOK" Text="Part-NotOK" />
                                        <asp:ListItem Value="CycleEnd" Text="Cycle End" />
                                        <asp:ListItem Value="Poka-Yoke1" Text="Poka-Yoke1" />
                                        <asp:ListItem Value="Poka-Yoke2" Text="Poka-Yoke2" />
                                        <asp:ListItem Value="Poka-Yoke3" Text="Poka-Yoke3" />
                                        <asp:ListItem Value="Poka-Yoke5" Text="Poka-Yoke5" />
                                        <asp:ListItem Value="RejectionBin" Text="Rejection Bin" />
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Event Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEventName" Text='<%# Bind("EventName") %>' ForeColor="White" style="text-align:center;vertical-align:central;" Width="100"/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Width="100" ID="txtEventName" Text='<%# Bind("EventName") %>'  CssClass="form-control"/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" Width="100" ID="txtFooterEventName" CssClass="form-control"/>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Register">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRegister" Text='<%# Bind("Register") %>' ForeColor="White" style="text-align:center;vertical-align:central;" Width="120"/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Width="120" ID="txtRegister" Text='<%# Bind("Register") %>' CssClass="form-control"/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtFooterRegester" Width="120" CssClass="form-control"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="linkEdit" CommandName="EditRow" CommandArgument='<%# Bind("SLNO") %>' Text="Edit" />
                                    <asp:LinkButton runat="server" ID="linkDelete" CommandName="DeleteRow" CommandArgument='<%# Bind("SLNO") %>' Text="Delete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" ID="linkUpdate" CommandName="UpdateRow" CommandArgument='<%# Bind("SLNO") %>' Text="Update" />
                                    <asp:LinkButton runat="server" ID="linkCancel" CommandName="CancelRow" CommandArgument='<%# Bind("SLNO") %>' Text="Cancel" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" ID="linkNew" CommandName="NewRow" CommandArgument='<%# Bind("SLNO") %>' Text="Add" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
