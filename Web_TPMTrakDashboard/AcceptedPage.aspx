<%@ Page Title="Acception Information" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="AcceptedPage.aspx.cs" Inherits="Web_TPMTrakDashboard.AcceptedPage" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        th {
            color: white !important;
            background-color: #2E6886 !important;
        }

        td[colspan="4"] {
            text-align: center;
        }

        #MainContent_gridviewAcceptedData tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_gridviewAcceptedData tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }

        .HeaderCss {
            background-color: #DCDCDC;
            /*//font-weight: bold;*/
            min-width: 100px;
        }

            .HeaderCss th {
                color: white;
            }
    </style>

    <div class="container">
        <div class="row text-center" style="font-size: 18px; font-weight: 900; color: white">
            <%=GetLocalResourceObject("AcceptedPartsInformation") %> - <%= Session["MachineId"]%>
            <br />
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False" meta:resourcekey="lblMessageResource1"></asp:Label>
        </div>
        <div class="row">
            <asp:GridView ID="gridviewAcceptedData" runat="server" AutoGenerateColumns="False"
                CssClass="table table-bordered" ShowHeaderWhenEmpty="True"
                EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>" HorizontalAlign="Center">
                <Columns>
                    <%--<asp:BoundField HeaderText="Component" DataField="ComponentId" meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField HeaderText="Operation" DataField="Operationno" meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField HeaderText="Accepted Parts" DataField="AcceptedParts" DataFormatString="{0:N0}" HtmlEncode="false" meta:resourcekey="BoundFieldResource3" />
                    <asp:BoundField HeaderText="Avg. Cycle Time (hh:mm:ss)" DataField="AvgCycletime" meta:resourcekey="BoundFieldResource4" />--%>
                </Columns>
                <HeaderStyle CssClass="HeaderCss" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
