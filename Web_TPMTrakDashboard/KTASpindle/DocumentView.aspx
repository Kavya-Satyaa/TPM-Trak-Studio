<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentView.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.DocumentView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
     <style>
        .innergrid tbody tr:nth-child(odd) {
            background-color: #00b8ff2b;
        }
        a {
            color: #027ce5;
            text-decoration: none;
        }
        .innergrid
        {
            box-shadow: 4px 6px 5px 0px #88888840;
        }
        .multiselect-container {
            height:50vh;
            overflow:auto;
            
        }
      .btn-group>.btn:first-child {
            margin-left: 0;
            width: 250px;
            overflow: hidden;
        }
      .input-group .form-control {
            height:40px !important;
        }
    </style>

      <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td>Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>Cell</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCell" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>Component</td>
                            <td>
                                 <asp:ListBox ID="ddlMultiComponent" ClientIDMode="Static" CssClass="dropdown multiDropdown" runat="server" SelectionMode="Multiple" Style="min-width: 260px;" AutoPostBack="true" OnSelectedIndexChanged="ddlMultiComponent_SelectedIndexChanged"></asp:ListBox>
                              <%--  <asp:DropDownList runat="server" ID="ddlComponent" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" AutoPostBack="true" Width="260px"></asp:DropDownList>--%>
                                 <asp:LinkButton  runat="server" ID="lnkClear" ClientIDMode="Static" ToolTip="Clear" OnClick="lnkClear_Click">
                                      <i class="glyphicon glyphicon-remove-circle" style="font-size:19px;vertical-align:middle"></i>
                                 </asp:LinkButton>
                            </td>
                            <td>Operation</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlOperation" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="View"  CssClass="btn btn-info" OnClientClick="return showLoader();" OnClick="btnView_Click"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
         </ContentTemplate>  
          <Triggers>
              <asp:PostBackTrigger ControlID="lvDocumentUpload" />
              <asp:AsyncPostBackTrigger ControlID="ddlMultiComponent" EventName="SelectedIndexChanged" />
              <asp:AsyncPostBackTrigger ControlID="ddlPlant" EventName="SelectedIndexChanged" />
              <asp:AsyncPostBackTrigger ControlID="ddlCell" EventName="SelectedIndexChanged" />
              <asp:PostBackTrigger ControlID="lnkClear" />
              <asp:PostBackTrigger ControlID="btnView" />
          </Triggers>
       </asp:UpdatePanel>
            <div style="height:80vh;overflow:auto;margin-top:5px;width:100%;">
                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                <asp:ListView ItemPlaceholderID="itemplaceholder" ID="lvDocumentUpload" runat="server" ClientIDMode="Static">
                    <EmptyDataTemplate>
                         <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblDocumentUpload">
                            <tr>
                                <th>Component</th>
                                <th>Operation</th>
                                <th>Drawing</th>
                                <th style="display:none;">Program</th>
                                <th>Tools</th>
                                <th>Fixture</th>
                                <th>Gauge</th>
                                <th>Inspection</th>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblDocumentUpload">
                            <tr>
                                <th>Component</th>
                                <th>Operation</th>
                                <th>Drawing</th>
                                <th style="display:none;">Program</th>
                                <th>Tools</th>
                                <th>Fixture</th>
                                <th>Gauge</th>
                                <th>Inspection</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblComponent" ClientIDMode="Static" Text='<%# Eval("ComponentID") %>'></asp:Label>
                            </td>
                            <td>
                               <asp:Label runat="server" ID="lblOpnID" ClientIDMode="Static" Text='<%# Eval("operationno") %>'></asp:Label>
                            </td>
                            <td>
                               <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvDrawingDocumentDisplay" runat="server" DataSource='<%#Eval("DrawingData")%>'>
                                    <LayoutTemplate>
                                        <table runat="server" class="table table-bordered innergrid" id="tblDrawinglst">
                                            <tr id="inneritemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>                                              
                                                <asp:LinkButton CssClass="DrawingControl" runat="server" ID="lnDrawingFileName" ClientIDMode="Static" Text='<%# Eval("DrawingDocFileName") %>' OnClick="lnDocumentView_Click" ></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                             <td style="display:none;">
                                <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvProgramDocumentDisplay" runat="server" DataSource='<%#Eval("ProgramData")%>'>
                                    <LayoutTemplate>
                                        <table  runat="server" class="table table-bordered innergrid">
                                            <tr id="inneritemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:HiddenField runat="server" ID="hdnProgramUpdate" ClientIDMode="Static" />
                                                <asp:LinkButton runat="server" ID="lnkProgram" ClientIDMode="Static" CssClass="ProgramControl" Text='<%# Eval("ProgramDocFileName") %>' OnClick="lnDocumentView_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                             <td>
                                <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvTools" runat="server" DataSource='<%#Eval("ToolsData")%>'>
                                    <LayoutTemplate>
                                        <table  runat="server" class="table table-bordered innergrid">
                                            <tr id="inneritemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                 <asp:HiddenField runat="server" ID="hdnToolsUpdate" ClientIDMode="Static"  />
                                                 <asp:LinkButton runat="server" ID="lnkTools" ClientIDMode="Static" CssClass="ToolsControl" Text='<%# Eval("ToolsDocFileName") %>' OnClick="lnDocumentView_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                             <td>
                                <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvFixture" runat="server" DataSource='<%#Eval("FixtureData")%>'>
                                <LayoutTemplate>
                                    <table  runat="server" class="table table-bordered innergrid">
                                        <tr id="inneritemplaceholder" runat="server"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HiddenField runat="server" ID="hdnFixtureUpdate" ClientIDMode="Static" />
                                            <asp:LinkButton runat="server" ID="lnkFixture" ClientIDMode="Static" CssClass="FixtureControl" Text='<%# Eval("FixtureDocFileName") %>' OnClick="lnDocumentView_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            </td>
                            <td>
                                <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvGauge" runat="server" DataSource='<%#Eval("GaugeData")%>'>
                                <LayoutTemplate>
                                    <table  runat="server" class="table table-bordered innergrid">
                                        <tr id="inneritemplaceholder" runat="server"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                             <asp:HiddenField runat="server" ID="hdnGaugeUpdate" ClientIDMode="Static" />
                                             <asp:LinkButton runat="server" ID="lnkGauge" ClientIDMode="Static" CssClass="GaugeControl" Text='<%# Eval("GaugeDocFileName") %>' OnClick="lnDocumentView_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            </td>
                             <td>
                                <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvInspection" runat="server" DataSource='<%#Eval("InspectionData")%>'>
                                <LayoutTemplate>
                                    <table  runat="server" class="table table-bordered innergrid">
                                        <tr id="inneritemplaceholder" runat="server"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HiddenField runat="server" ID="hdnInspectionUpdate" ClientIDMode="Static"/>
                                            <asp:LinkButton runat="server" ID="lnkInspection" CssClass="InspectionControl" ClientIDMode="Static" Text='<%# Eval("InspectionDocFileName") %>' OnClick="lnDocumentView_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
    <script>
        var multiselctListExpand = false;
        var selectedValue = ""; 
        $(document).ready(function () {
           
            $(".innergrid").on("click", "td .DrawingControl", function () {
                $("#hdnUpdate").val("drawing");
            });
            $(".innergrid").on("click", "td .ProgramControl", function () {
                $("#hdnUpdate").val("program");
            });
            $(".innergrid").on("click", "td .ToolsControl", function () {
                $("#hdnUpdate").val("tools");
            });
            $(".innergrid").on("click", "td .FixtureControl", function () {
                $("#hdnUpdate").val("fixture");
            });
            $(".innergrid").on("click", "td .GaugeControl", function () {
                $("#hdnUpdate").val("gauge");
            });
            $(".innergrid").on("click", "td .InspectionControl", function () {
                $("#hdnUpdate").val("inspection");
            });
            $('.multiDropdown').multiselect({
                includeSelectAllOption: false,
                enableFiltering: true,
                maxHeight: 600,
                buttonWidth: '250px',
                enableCaseInsensitiveFiltering: true
            });
        });

        function StayMultiselectedList() {
            multiselctListExpand = true;
        }

        function showWarningMsg(msg, title) {
            debugger;
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "4000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "toastClass": "toaster-position"
            }

            toastr['warning'](msg, title);
            return false;
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                
                $(".innergrid").on("click", "td .DrawingControl", function () {
                    $("#hdnUpdate").val("drawing");
                });
                $(".innergrid").on("click", "td .ProgramControl", function () {
                    $("#hdnUpdate").val("program");
                });
                $(".innergrid").on("click", "td .ToolsControl", function () {
                    $("#hdnUpdate").val("tools");
                });
                $(".innergrid").on("click", "td .FixtureControl", function () {
                    $("#hdnUpdate").val("fixture");
                });
                $(".innergrid").on("click", "td .GaugeControl", function () {
                    $("#hdnUpdate").val("gauge");
                });
                $(".innergrid").on("click", "td .InspectionControl", function () {
                    $("#hdnUpdate").val("inspection");
                });
                $('.multiDropdown').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    maxHeight: 600,
                    buttonWidth: '250px',
                    enableCaseInsensitiveFiltering: true
                });

                if (multiselctListExpand) {
                    $(".btn-group").addClass('open');
                } else {
                    $(".btn-group").removeClass('open');
                }
                multiselctListExpand = false;

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
