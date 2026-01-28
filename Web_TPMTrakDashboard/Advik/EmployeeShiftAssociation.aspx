<%@ Page Title="Employee Shift Association" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeShiftAssociation.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.EmployeeShiftAssociation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
         .headerFixerTable tr th {
            position: sticky;
            top: -1px;
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

        .headerFixerTable th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }
         .headerFixerTable td {
             color: white;
         }

    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>

            <table id="tblfilter" class="table table-bordered" style="width: auto; margin-top: 10px">
                <tr>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Plant</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" Width="240" Style="height: 32px" ></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Help Request</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlHelpRequest" CssClass="form-control" Width="240" Style="height: 32px"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>
                        <asp:Button runat="server" Text="Save" CssClass="btn btn-info btn-sm displayCss" ID="btnSave" OnClick="btnSave_Click"></asp:Button>

                    </td>
                </tr>
            </table>

               <div id="gridContainer" class="divGrid" style="">
                    <asp:GridView runat="server" ID="gvEmpShiftDetails" AutoGenerateColumns="False" ClientIDMode="Static"
                        CssClass="table table-bordered cockpit headerFixerTable" OnRowDataBound="gvEmpShiftDetails_RowDataBound" ShowHeaderWhenEmpty="true">
                        </asp:GridView>
                   </div>
        </ContentTemplate>
    </asp:UpdatePanel>

     <script type="text/javascript">

        $(document).ready(function () {
            var Height = $(window).height() - (170);
            $('#gridContainer').css('height', Height);
        });
        $(window).resize(function () {
            var Height = $(window).height() - (170);
            $('#gridContainer').css('height', Height);
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                  var Height = $(window).height() - (170);
                $('#gridContainer').css('height', Height);
            });
            $(window).resize(function () {
                var Height = $(window).height() - (170);
                $('#gridContainer').css('height', Height);
            });
         });
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
