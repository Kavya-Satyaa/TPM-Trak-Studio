<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MethodInstrumentImagesUploadPage.aspx.cs" Inherits="Web_TPMTrakDashboard.Vulkan.MethodInstrumentImagesUploadPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .legendStyleSettings {
            width: 30%;
            border: 0px;
            margin: 0px 20px;
            padding: 0px 10px;
            color: white;
            font-size: 18px;
        }

        .tblMetIns {
            width: 100%;
            display: flex;
            justify-content: center;
        }

            .tblMetIns > tbody > tr > td {
                padding: 10px;
                text-align: center;
                color: white;
            }

        .gvMethod {
            width: 100%;
        }

            .gvMethod > tbody > tr > td {
                color: black;
                padding: 10px;
                text-align: center;
                font-weight: bold;
            }

            .gvMethod > tbody > tr > td:first-child {
                width: 20%;
            }

            .gvMethod > tbody > tr:nth-child(even) > td {
                background-color: white;
            }
            .gvMethod > tbody > tr:nth-child(odd) > td {
                background-color: #ddd;
            }

                .gvMethod > tbody > tr > td:last-child {
                    text-align: center;
                }

            .gvMethod > tbody > tr > th {
                height: 38px;
                text-align: center;
            }
    </style>
    <div class="container-fluid">
        <div class="col-lg-5">
            <fieldset style="border: 1px solid white;">
                <legend class="legendStyleSettings">Checkpoint Method</legend>
                <table class="tblMetIns">
                    <tr>
                        <td style="font-size: 17px">Image</td>
                        <td>
                            <asp:FileUpload runat="server" ID="fuMethodImage" CssClass="form-control" AllowMultiple="true" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnMethodSave" CssClass="bajaj-btn-style" Text="Save Methods" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>

                <div style="margin: 20px;">
                    <asp:GridView runat="server" ID="gvMethod" CssClass="gvMethod headerFixer" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl No.">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnRefID" Value='<%# Eval("RefID") %>' />
                                    <asp:Label runat="server" Text='<%# Eval("SlNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Image">
                                <ItemTemplate>
                                     <asp:Image runat="server" ID="imgMethod" ImageUrl='<%# Eval("imgUrl") %>' Height="50" Width="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField HeaderText="Delete?">
                                <ItemTemplate>
                                    <asp:Button runat="server" Text="Delete" ID="btnDelete" CssClass="btn btn-primary" OnClick="btnDelete_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="col-lg-5">
            <fieldset style="border: 1px solid white;">
                <legend class="legendStyleSettings" style="width: 35% !important;">Checkpoint Instrument</legend>
                <table class="tblMetIns">
                    <tr>
                        <td style="font-size: 17px">Image</td>
                        <td>
                            <asp:FileUpload runat="server" ID="fuInstrumentImage" CssClass="form-control" AllowMultiple="true" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSaveInstrument" CssClass="bajaj-btn-style" Text="Save Instruments" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>

                 <div style="margin: 20px;">
                    <asp:GridView runat="server" ID="gvInstrument" CssClass="gvMethod headerFixer" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl No.">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnRefID" Value='<%# Eval("RefID") %>' />
                                    <asp:Label runat="server" Text='<%# Eval("SlNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Image">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgInstrument" ImageUrl='<%# Eval("imgUrl") %>' Height="50" Width="50" />
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField HeaderText="Delete?">
                                <ItemTemplate>
                                    <asp:Button runat="server" Text="Delete" ID="btnDelete" CssClass="btn btn-primary" OnClick="btnDelete_Click"/>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
