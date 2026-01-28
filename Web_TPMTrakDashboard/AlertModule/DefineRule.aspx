<%@ Page Title="Define Rule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefineRule.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.DefineRule" %>

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
            width: 100%;
            overflow: auto;
            margin-top: 10px;
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

        input[type="checkbox"] {
            height: 20px;
            width: 20px;
            vertical-align: sub;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>
            <div id="gridContainer" class="divGrid" style="overflow: auto;">
                <asp:GridView runat="server" ID="gvDefineRule" AutoGenerateColumns="False" ClientIDMode="Static"
                    CssClass="table table-bordered cockpit headerFixerTable " ShowHeaderWhenEmpty="true" OnRowEditing="gvDefineRule_RowEditing" OnRowDeleting="gvDefineRule_RowDeleting" OnRowUpdating="gvDefineRule_RowUpdating" ShowFooter="true" OnRowCancelingEdit="gvDefineRule_RowCancelingEdit" OnRowDataBound="gvDefineRule_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Rule ID" HeaderStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblruleid" Text='<%#Eval("RuleId")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="lblruleid" Text='<%#Eval("RuleId")%>' ForeColor="White" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtnewRuleID" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="200">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDesc" Text='<%#Eval("Description")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtedtDesc" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled" Text='<%#Eval("Description")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtnewDesc" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Parameter" HeaderStyle-Width="300">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblParameter" Text='<%#Eval("Parameter")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="lbledtParameter" Text='<%#Eval("Parameter")%>' ForeColor="White" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%-- <asp:TextBox runat="server" ID="txtnewParameter" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>--%>
                                <%--onchange="return ParameterChange(this)"--%>
                                <asp:DropDownList runat="server" ID="ddlnewParameter" CssClass="form-control" OnSelectedIndexChanged="ddlnewParameter_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static">
                                    <asp:ListItem Text="Parts Count" Value="Parts Count"></asp:ListItem>
                                    <asp:ListItem Text="Ping Status" Value="Ping Status"></asp:ListItem>
                                    <asp:ListItem Text="Downtime Notification" Value="Downtime Notification"></asp:ListItem>
                                    <asp:ListItem Text="Production Detail" Value="Production Detail"></asp:ListItem>
                                    <asp:ListItem Text="Parts Count By CO" Value="Parts Count By CO"></asp:ListItem>
                                    <%-- <asp:ListItem>Ping Status</asp:ListItem>--%>
                                    <asp:ListItem Text="Cycle Start" Value="Cycle Start"></asp:ListItem>
                                    <asp:ListItem Text="Cycle Complete" Value="Cycle Complete"></asp:ListItem>
                                    <asp:ListItem Text="Scheduled Components" Value="Scheduled Components"></asp:ListItem>
                                    <asp:ListItem Text="Top Downtimes" Value="Top Downtimes"></asp:ListItem>
                                    <asp:ListItem Text="High Performance Machines" Value="High Performance Machines"></asp:ListItem>
                                    <asp:ListItem Text="Low Performance Machines" Value="Low Performance Machines"></asp:ListItem>
                                    <asp:ListItem Text="Breakdown Notification" Value="Breakdown Notification"></asp:ListItem>
                                    <asp:ListItem Text="Tafe_Quality Status" Value="Tafe_Quality Status"></asp:ListItem>
                                    <asp:ListItem Text="Cycle Start After Down" Value="Cycle Start After Down"></asp:ListItem>
                                    <asp:ListItem Text="JH Checklist Status" Value="JH Checklist Status"></asp:ListItem>
                                    <asp:ListItem Text="PM Notification" Value="PM Notification"></asp:ListItem>
                                    <asp:ListItem Text="Audit Reminder To Supervisor" Value="Audit Reminder To Supervisor"></asp:ListItem>
                                    <asp:ListItem Text="Audit Reminder To ProdHead" Value="Audit Reminder To ProdHead"></asp:ListItem>
                                    <asp:ListItem Text="Bypass End Notification" Value="Bypass End Notification"></asp:ListItem>
                                    <asp:ListItem Text="PM (Shanti)" Value="PM (Shanti)"></asp:ListItem>
                                    <asp:ListItem Text="PM (AAAPL)" Value="PM (AAAPL)"></asp:ListItem>
                                    <asp:ListItem Text="Process Parameter Alert" Value="Process Parameter Alert"></asp:ListItem>
                                    <asp:ListItem Text="Down Start" Value="Down Start"></asp:ListItem>
                                    <asp:ListItem Text="Down End" Value="Down End"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Compare">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompare" Text='<%#Eval("Compare")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddledtCompare" CssClass="form-control" ClientIDMode="Static">
                                    <asp:ListItem>>=</asp:ListItem>
                                    <asp:ListItem><=</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ID="ddlnewCompre" CssClass="form-control" ClientIDMode="Static">
                                    <asp:ListItem>>=</asp:ListItem>
                                    <asp:ListItem><=</asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField runat="server" ID="hfcontrolnull" ClientIDMode="Static" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Threshold" ItemStyle-Width="400">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblThreshold" Text='<%#Eval("ThresholdValue")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtedtThresold" CssClass="form-control" Text='<%#Eval("Threshold")%>' onkeypress="return allowNumberic(event);" ClientIDMode="Static" Style="width: 40px; display: inline" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:DropDownList runat="server" ID="ddledtThresholdUnit" CssClass="form-control" Style="width: 100px; display: inline" ClientIDMode="Static">
                                    <%--   <asp:ListItem>min</asp:ListItem>
                                      <asp:ListItem>Hour</asp:ListItem>
                                      <asp:ListItem>Days</asp:ListItem>
                                      <asp:ListItem>Shift</asp:ListItem>
                                      <asp:ListItem>%</asp:ListItem>--%>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtnewThresold" CssClass="form-control" onkeypress="return allowNumberic(event);" ClientIDMode="Static" Style="width: 100px; display: inline" AutoCompleteType="Disabled"></asp:TextBox>

                                <asp:DropDownList runat="server" ID="ddlnewThresholdUnit" CssClass="form-control" Style="width: 100px; display: inline" ClientIDMode="Static">
                                    <%--  <asp:ListItem>min</asp:ListItem>
                                      <asp:ListItem>Hour</asp:ListItem>
                                      <asp:ListItem>Days</asp:ListItem>
                                      <asp:ListItem>Shift</asp:ListItem>
                                      <asp:ListItem>%</asp:ListItem>--%>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Evaluate Every" HeaderStyle-Width="400">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEvaluateEvery" Text='<%#Eval("EvaluateEveryValue")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtedtEvaluateEvery" CssClass="form-control" Text='<%#Eval("EvaluateEvery") %>' ClientIDMode="Static" Style="width: 50px; display: inline" onkeypress="return allowNumberic(event);" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:DropDownList runat="server" ID="ddledtEvaluateEvery" AutoPostBack="true" OnSelectedIndexChanged="ddledtEvaluateEvery_SelectedIndexChanged" CssClass="form-control" Style="width: 100px; display: inline" ClientIDMode="Static">
                                    <%--   <asp:ListItem>min</asp:ListItem>
                                      <asp:ListItem>Hours</asp:ListItem>
                                      <asp:ListItem>EveryDay</asp:ListItem>
                                      <asp:ListItem>EveryShift</asp:ListItem>
                                      <asp:ListItem>HourlyShift</asp:ListItem>--%>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtnewEvaluateEvery" CssClass="form-control" ClientIDMode="Static" onkeypress="return allowNumberic(event);" Style="width: 50px; display: inline" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:DropDownList runat="server" ID="ddlnewEvaluateEvery" AutoPostBack="true" OnSelectedIndexChanged="ddlnewEvaluateEvery_SelectedIndexChanged" CssClass="form-control" Style="width: 100px; display: inline" ClientIDMode="Static">
                                    <%--  <asp:ListItem>min</asp:ListItem>
                                      <asp:ListItem>Hours</asp:ListItem>
                                      <asp:ListItem>EveryDay</asp:ListItem>
                                      <asp:ListItem>EveryShift</asp:ListItem>
                                      <asp:ListItem>HourlyShift</asp:ListItem>--%>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enabled">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbEnable" Checked='<%#Eval("Enable")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox runat="server" ID="cbedtEnable" Checked='<%#Eval("Enable")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox runat="server" ID="cbnewEnable" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Applies To" ItemStyle-Width="120">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblApplyto" Text='<%#Eval("AppliesTo")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddledtApplyto" CssClass="form-control" ClientIDMode="Static">
                                    <asp:ListItem>Machine</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ID="ddlnewApplyto" CssClass="form-control" ClientIDMode="Static">
                                    <asp:ListItem>Machine</asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SMS Enabled">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbSMSEnable" Checked='<%#Eval("SMSEnable")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox runat="server" ID="cbedtSMSEnable" Checked='<%#Eval("SMSEnable")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox runat="server" ID="cbnewSMSEnable" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Enabled">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbEmailEnable" Checked='<%#Eval("EmailEnable")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox runat="server" ID="cbedtEmailEnable" Checked='<%#Eval("EmailEnable")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox runat="server" ID="cbnewEmailEnable" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Telegram Enabled">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbTelegramEnable" Checked='<%#Eval("TelegramEnabled")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox runat="server" ID="cbedtTelegramEnable" Checked='<%#Eval("TelegramEnabled")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox runat="server" ID="cbnewTelegramEnable" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobile Enabled">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbMobileEnable" Checked='<%#Eval("MobileEnabled")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox runat="server" ID="cbedtMobileEnable" Checked='<%#Eval("MobileEnabled")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox runat="server" ID="cbnewMobileEnable" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Message">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMsg" Text='<%#Eval("Message")%>' ForeColor="White" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txtedtMsg" CssClass="form-control" Text='<%#Eval("Message")%>' ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:ListBox ID="lbProductionDetails" runat="server" SelectionMode="Multiple" CssClass="form-control prodDetails" ClientIDMode="Static"></asp:ListBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtnewMSg" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:ListBox ID="lbProductionDetails" runat="server" SelectionMode="Multiple" CssClass="form-control prodDetails" ClientIDMode="Static"></asp:ListBox>
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
                                <asp:LinkButton runat="server" ID="btnInsert" OnClick="btnInsert_Click" CssClass="glyphicon glyphicon-plus-sign" ToolTip="New" Style="font-size: 20px; color: #46b8da"></asp:LinkButton>
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
            var Height = $(window).height() - (110);            $('#gridContainer').css('height', Height);
            //ParameterChange();
            $('.prodDetails').multiselect({
                includeSelectAllOption: true
            });
        });
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("lblMessages").style.display = "none";
            }, 2000);

        };
        function ParameterChange() {
            debugger;
            alert("Enter");
            let parametername = $("#ddlnewParameter").val();
            if (parametername == "Production Detail" || parametername == "Cycle Start" || parametername == "Cycle Complete" || parametername == "Scheduled Components" || parametername == "Parts Count" || parametername == "Parts Count By CO" || parametername == "Top And Bottom OEE" || parametername == "Top Downtimes" || parametername == "High Performance Machines" || parametername == "Low Performance Machines" || parametername == "Tafe_Quality Status" || parametername == "Cycle Start After Down") {
                //$(val).closest('tr').find("#ddlnewComparediv").css('display', 'none');
                //$(val).closest('tr').find("#ddlnewThresholdUnitdiv").css('display', 'none');
                //$(val).closest('tr').find("#hfcontrolnull").val("1");
                $("#ddlnewComparediv").css('display', 'none');
                $("#ddlnewThresholdUnitdiv").css('display', 'none');
                $("#hfcontrolnull").val("1");
            } else {
                //$(val).closest('tr').find("#ddlnewComparediv").css('display', 'block');
                //$(val).closest('tr').find("#ddlnewThresholdUnitdiv").css('display', 'block');
                // $(val).closest('tr').find("#hfcontrolnull").val("0");
                $("#ddlnewComparediv").css('display', 'block');
                $("#ddlnewThresholdUnitdiv").css('display', 'inline-block');
                $("#hfcontrolnull").val("0");
            }
        }
        function openConfirmModal() {
            $('[id*=ConfirmModal]').modal('show');
        };
        function allowNumberic(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode == 45 && pos != 0) {
                return false;
            } else if (charCode == 43 && pos != 0) {
                return false;
            } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                return false;
            } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                return false;
            }
            return true;
        }
        $(window).resize(function () {            var Height = $(window).height() - (110);            $('#gridContainer').css('height', Height);        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                var Height = $(window).height() - (110);                $('#gridContainer').css('height', Height);
                $('.prodDetails').multiselect({
                    includeSelectAllOption: true
                });
            });
            $(window).resize(function () {                var Height = $(window).height() - (110);                $('#gridContainer').css('height', Height);            });
            function allowNumberic(evt) {
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 45 && pos != 0) {
                    return false;
                } else if (charCode == 43 && pos != 0) {
                    return false;
                } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                    return false;
                } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                    return false;
                }
                return true;
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
