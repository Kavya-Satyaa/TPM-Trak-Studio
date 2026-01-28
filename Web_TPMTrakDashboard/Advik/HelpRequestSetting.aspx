<%@ Page Title="Help Request Setting" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HelpRequestSetting.aspx.cs" EnableEventValidation="false" Inherits="Web_TPMTrakDashboard.Advik.HelpRequestSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
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

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }


        fieldset {
            border: 1px solid #4f4e63;
            padding: 0px;
            border-radius: 4px;
            width: auto;
            /*box-shadow: 2px 2px 8px 2px #efe7e7;*/
        }

        .masterFS {
            margin: 0 8px 0 8px;
            padding: 0 10px 5px 10px;
        }

        legend {
            text-align: left;
            color: white;
            display: block;
            width: auto;
            padding: 0;
            margin-bottom: 5px;
            font-size: 15px;
            line-height: inherit;
            border-bottom: transparent;
        }

        .smstbl {
            width: 100%;
        }

            .smstbl tr td {
                text-align: center;
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
        .multiselect{
            height: 32px;
            /*width: 150px;*/
        }
        .multiselect-native-select .btn-group button{
            width:240px;
            overflow:hidden;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            				 <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>
            <div class="container-fluid" style="margin: 0px 10px">
                <div class="row" id="selectContainer">

                    <fieldset class="masterFS" style="margin: 0px">
                        <legend>Select</legend>

                        <div  style=" display: table; width: 100%">

                       
                        <div style="display: table-cell; width: 43%; padding-right: 20px">
                            <table id="tblfilter" class="table table-bordered" style="width: 100%; margin-top: 10px;">
                                <tr>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Plant</td>
                                    <td style="min-width: 160px;">
                                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" Width="240" Style="height: 32px" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Machine</td>
                                    <td style="min-width: 160px;">
                                        <asp:ListBox ID="ddlMachineId" runat="server" SelectionMode="Multiple" Width="150" Style="height: 32px"  ClientIDMode="Static"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Help Request</td>
                                    <td style="min-width: 160px;">
                                        <asp:DropDownList runat="server" ID="ddlHelpRequest" CssClass="form-control" Width="240" Style="height: 32px"></asp:DropDownList>
                                    </td>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Action</td>
                                    <td style="min-width: 160px;">
                                        <asp:DropDownList runat="server" ID="ddlAction" CssClass="form-control" Width="240" Style="height: 32px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Message</td>
                                    <td colspan="3">
                                        <%--<span style="color: white; font-weight: bold; display: inline">Message</span>--%>
                                        <asp:TextBox runat="server" ID="txtMsg" TextMode="MultiLine" Width="500" Style="display: inline" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center; width: auto;">
                                        <asp:Button runat="server" Text="New Rule" CssClass="btn btn-info btn-sm displayCss" ID="btnNewRule" OnClick="btnNewRule_Click"></asp:Button>
                                        <asp:Button runat="server" Text="Save" CssClass="btn btn-info btn-sm displayCss" ID="btnSave" OnClick="btnSave_Click"></asp:Button>
                                        <asp:Button runat="server" Text="Delete" CssClass="btn btn-info btn-sm displayCss" ID="btnDelete" OnClick="btnDelete_Click"></asp:Button>
                                    </td>

                                </tr>
                            </table>
                        </div>
                        <div  style="display: table-cell; vertical-align: middle; width: 50%">
                            <fieldset class="masterFS" style="margin: 0px; width: 100%">
                                <legend>Mobile No.</legend>
                                <table class="table table-bordered" style="width: 100%;">
                                    <tr>
                                        <td class="commanTd" style="min-width: 50px; height: 50px">Employee ID</td>
                                        <td style="min-width: 160px;">
                                            <asp:DropDownList runat="server" ID="ddlEmployeeID" CssClass="form-control" OnSelectedIndexChanged="ddlEmployeeID_SelectedIndexChanged" AutoPostBack="true" Width="240" Style="height: 32px"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmpMblNo" ReadOnly="true" CssClass="form-control" Width="240"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div class="row" style="margin-top: 10px">
                                    <div class="col-lg-4">
                                        <div style="text-align: center">
                                              <span style="color: white; font-weight: bold; margin: auto">Threshold (in Minutes)</span>
                                        </div>

                                        <fieldset class="masterFS" style="margin-top: 18px">
                                            <legend>SMS To</legend>
                                            <table class="smstbl">
                                                <tr>
                                                    <td>
                                                        <asp:Button runat="server" Text="Add" CssClass="btn btn-info btn-sm displayCss" ID="btn1stlvlAdd" OnClick="btn1stlvlAdd_Click"></asp:Button>
                                                        <asp:Button runat="server" Text="Remove" CssClass="btn btn-info btn-sm displayCss" ID="btn1stlvlRemove" OnClick="btn1stlvlRemove_Click"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox runat="server" TextMode="MultiLine" ReadOnly="true" ID="txt1stlvlSMS" CssClass="form-control"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:DropDownList runat="server" ID="ddl2ndlvlThreshold" CssClass="form-control" Width="120" style="margin: auto; height: 32px"></asp:DropDownList>
                                        <fieldset class="masterFS" style="margin-top: 5px">
                                            <legend>2nd Level SMS To</legend>
                                            <table class="smstbl">
                                                <tr>
                                                    <td>
                                                        <asp:Button runat="server" Text="Add" CssClass="btn btn-info btn-sm displayCss" ID="btn2ndlvlAdd" OnClick="btn2ndlvlAdd_Click"></asp:Button>
                                                        <asp:Button runat="server" Text="Remove" CssClass="btn btn-info btn-sm displayCss" ID="btn2ndlvlRemove" OnClick="btn2ndlvlRemove_Click"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox runat="server" TextMode="MultiLine" ReadOnly="true" ID="txt2ndlvlSMS" CssClass="form-control"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:DropDownList runat="server" ID="ddl3rdlvlThreshold" CssClass="form-control" Width="120"  style="margin: auto; height: 32px"></asp:DropDownList>
                                        <fieldset class="masterFS" style="margin-top: 5px">
                                            <legend>3rd Level SMS To</legend>
                                            <table class="smstbl">
                                                <tr>
                                                    <td>
                                                        <asp:Button runat="server" Text="Add" CssClass="btn btn-info btn-sm displayCss" ID="btn3rdlvlAdd" OnClick="btn3rdlvlAdd_Click"></asp:Button>
                                                        <asp:Button runat="server" Text="Remove" CssClass="btn btn-info btn-sm displayCss" ID="btn3rdlvlRemove" OnClick="btn3rdlvlRemove_Click"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox runat="server" TextMode="MultiLine" ReadOnly="true" ID="txt3rdlvlSMS" CssClass="form-control"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </div>
                            </fieldset>
                        </div>

                             </div>
                    </fieldset>
                </div>

                <div class="row" style="margin-top: 10px">
                    <fieldset class="masterFS" style="margin: 0px">
                        <legend>Help Request Rule</legend>
                           <table class="table table-bordered" style="width: auto;">
                                <tr>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Plant</td>
                                    <td style="min-width: 160px;">
                                        <asp:DropDownList runat="server" ID="ddlFilterPlantID" CssClass="form-control" Width="150" Style="height: 32px"></asp:DropDownList>
                                    </td>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Machine</td>
                                    <td style="min-width: 150px;">
                                        <asp:ListBox ID="ddlFilterMachineID" runat="server" SelectionMode="Multiple" Style="height: 32px"  ClientIDMode="Static"></asp:ListBox>
                                    </td>
                               
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Help Request</td>
                                    <td style="min-width: 160px;">
                                        <asp:DropDownList runat="server" ID="ddlFilterHelprequest" CssClass="form-control" Width="150" Style="height: 32px"></asp:DropDownList>
                                    </td>
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Action</td>
                                    <td style="min-width: 160px;">
                                        <asp:DropDownList runat="server" ID="ddlFilterAction" CssClass="form-control" Width="150" Style="height: 32px"></asp:DropDownList>
                                    </td>
                               
                                    <td class="commanTd" style="min-width: 50px; height: 50px">Employee ID</td>
                                    <td style="min-width: 160px;">
                                          <asp:DropDownList runat="server" ID="ddlFilterEmployeeID" CssClass="form-control" Width="150" Style="height: 32px"></asp:DropDownList>
                                    </td>
                                
                                    <td style="min-width: 160px;">
                                        <asp:Button runat="server" Text="Search" CssClass="btn btn-info btn-sm displayCss" ID="btnSearch" OnClick="btnSearch_Click"></asp:Button>
                                    </td>

                                </tr>
                            </table>

                            <div id="gridContainer" class="divGrid" style="">
                    <asp:GridView runat="server" ID="gvHelpRequestDetails" AutoGenerateColumns="False" ClientIDMode="Static"
                        CssClass="table table-bordered cockpit headerFixerTable" ShowHeaderWhenEmpty="true" OnRowDataBound="gvHelpRequestDetails_RowDataBound"  OnSelectedIndexChanged="gvHelpRequestDetails_SelectedIndexChanged">
                        <Columns>
                              <asp:TemplateField HeaderText="Plant ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblplantid" Text='<%#Eval("PlantID")%>' ForeColor="White" />
                                    <asp:HiddenField runat="server" ID="hfSlNo" Value='<%#Eval("SlNo")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("MachineID")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Help Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblhelpcode" Text='<%#Eval("RequestType")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblaction" Text='<%#Eval("Action")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Level1 Employee ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbllvl1emp" Text='<%#Eval("Level1Empid")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Level2 Employee ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbllvl2emp" Text='<%#Eval("Level2Empid")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Level2 Threshold">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbllvl2t" Text='<%#Eval("Level2Threshold")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Level3 Employee ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbllvl3emp" Text='<%#Eval("Level3Empid")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Level3 Threshold">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbllvl3t" Text='<%#Eval("Level3Threshold")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Message">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblmsg" Text='<%#Eval("Message")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <div style="height: 100%; background-color: white; text-align: center; color: red">No Data Found</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                    </fieldset>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var oprDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlFilterMachineID]').multiselect({
                includeSelectAllOption: true
            });

            var Height = $(window).height() - (190 + $('#selectContainer').height());
            $('#gridContainer').css('height', Height);
        });
        $(window).resize(function () {
            var Height = $(window).height() - (190 + $('#selectContainer').height());
            $('#gridContainer').css('height', Height);
        });
        oprDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(oprDiv.scrollTop);;
        }
        window.onload = function () {
            oprDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            var oprDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                $('[id$=ddlMachineId]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=ddlFilterMachineID]').multiselect({
                    includeSelectAllOption: true
                });
                var Height = $(window).height() - (190 + $('#selectContainer').height());
                $('#gridContainer').css('height', Height);
            });
            $(window).resize(function () {
                var Height = $(window).height() - (190 + $('#selectContainer').height());
                $('#gridContainer').css('height', Height);
            });
            oprDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(oprDiv.scrollTop);;
            }
            window.onload = function () {
                oprDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
