<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GEASubOperations.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.GEASubOperations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .document {
            width: 400px;
            margin-top: 150px;
            background-color: beige;
        }

        .headerstyle {
            font-size: 20px;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bolder;
        }

        .style {
            color: white;
            background-color: #2E6886;
        }

        .Rowstyle {
            color: black;
            background-color: white;
        }

        .headerClass {
            background-color: #2E6886;
            color: white;
        }
    </style>
    <div>
       
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                 <div style="margin: 5px;" >
            <div style="float: left; margin-left: 40px;display:inline-flex" class="col-lg-4">
                <asp:DropDownList runat="server" ID="ddlStations" CssClass="form-control" Width="200" style="margin-right:10px;margin-bottom:10px;"/>
                 <asp:DropDownList runat="server" ID="ddlComponent" CssClass="form-control" Width="200" style="margin-right:10px;margin-bottom:10px;"/>
                <asp:Button runat="server" ID="btnView" Text="View" ClientIDMode="Static" CssClass="btn btn-primary" Height="35" OnClick="btnView_Click"/>
            </div>
            <div style="float: right; margin-right: 100px;" class="col-lg-3">
               <%--  <asp:Button runat="server" ID="btnSaveModel" Text="Add Model" ClientIDMode="Static" CssClass="btn btn-info" />--%>
                <asp:Button runat="server" ID="btnNew" Text="New" ClientIDMode="Static" CssClass="btn btn-success" />
                <asp:Button runat="server" ID="btnSave" Text="Save" ClientIDMode="Static" CssClass="btn btn-success" OnClick="btnSave_Click" />
                <asp:Button runat="server" ID="btnDelete" Text="Delete" ClientIDMode="Static" CssClass="btn btn-danger" OnClick="btnDelete_Click" />
            </div>
            <div style="float:right">
                <span><b>* Is Default is wrt Station Keep same for all operations</b></span>
            </div>
        </div>
                <div style="margin: 5px;">
                    <asp:GridView runat="server" ID="gridviewsuboperation" AutoGenerateColumns="false" CssClass="table table-bordered cockpit headerFixerTable" HeaderStyle-CssClass="headerClass" RowStyle-CssClass="Rowstyle">
                        <Columns>
                            <%--<asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton Text="Edit" runat="server" ID="edit" CssClass="linkbutton" ClientIDMode="Static" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Station">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblstation" Text='<%# Bind("Station") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Material ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMaterialID" Text='<%# Bind("MaterialID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperation" Text='<%# Bind("Operation") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSubOperation" Text='<%# Bind("SubOperation") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cycle Time">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="lblCycleTime" Text='<%# Bind("CycleTime") %>' CssClass="form-control"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Is Default">
                                <ItemTemplate> 
                                    <asp:CheckBox runat="server" ID="chkChecked" Checked='<%# Bind("Checked") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate> 
                                   <asp:CheckBox runat="server" ID="chkDelete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="modal" role="dialog" id="addSubOperation">
            <div class="modal-dialog document" role="document">
                <div class="modal-content ">
                    <div class="modal-header headerstyle">
                        <button type="button" class="close closes" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><b>Add Sub-Activity</b></h4>
                    </div>
                    <div class="modal-body">
                        <div>
                            <span><b>Station</b></span>
                            <%--<asp:TextBox runat="server" ID="txtStation" placeholder="Station" CssClass="form-control" ClientIDMode="Static" /><br />--%>
                            <asp:DropDownList runat="server" ID="ddlStation" placeholder="Station" CssClass="form-control" ClientIDMode="Static" /><br />
                            <br />
                            <asp:DropDownList runat="server" ID="ddlComponents" placeholder="Station" CssClass="form-control" ClientIDMode="Static" /><br />
                            <br />
                            <span><b>Operation</b></span>
                            <asp:TextBox runat="server" ID="txtOperation" placeholder="Operation" CssClass="form-control" ClientIDMode="Static" /><br />
                            <br />
                            <span><b>Sub-Activity</b></span>
                            <asp:TextBox runat="server" ID="txtSubOperation" placeholder="Sub-Operation" CssClass="form-control" ClientIDMode="Static" /><br />
                            <br />
                            <span><b>Cycle Time in Sec</b></span>
                            <asp:TextBox runat="server" ID="txtCycleTime" placeholder="Cycle Time in Sec" CssClass="form-control" ClientIDMode="Static" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" />
                            <br />
                            <span><b>Is Default</b></span>
                            <asp:CheckBox runat="server" ID="chkIsEnabled" placeholder="Is Default" CssClass="form-control" ClientIDMode="Static" Width="40"/>
                            <br />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" runat="server" class="btn btn-primary btnInsertData" id="btnInsertData" onserverclick="InsertData">Save changes</button>
                        <button type="button" class="btn btn-secondary closes" id="btnAddsuboperationclose">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal" role="dialog" id="UpdateSubOperation">
            <div class="modal-dialog document" role="document">
                <div class="modal-content ">
                    <div class="modal-header headerstyle">
                        <button type="button" class="close closes" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><b>Update SubActivity</b></h4>
                    </div>
                    <div class="modal-body">
                        <div>
                            <span><b>Station</b></span>
                            <asp:TextBox runat="server" ID="txtUpdateStation" placeholder="Station" CssClass="form-control" ClientIDMode="Static" Enabled="false" /><br />
                            <br />
                            <span><b>Operation</b></span>
                            <asp:TextBox runat="server" ID="txtUpdateOperation" placeholder="Operation" CssClass="form-control" ClientIDMode="Static" Enabled="false" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" />
                            <br />
                            <span><b>Sub Activity</b></span>
                            <asp:TextBox runat="server" ID="txtUpdateSubOperation" placeholder="Sub-Operation" CssClass="form-control" ClientIDMode="Static" Enabled="false" /><br />
                            <br />
                            <span><b>Cycle Time in Sec</b></span>
                            <asp:TextBox runat="server" ID="txtUpdateCycleTime" placeholder="Cycle Time in Sec" CssClass="form-control" ClientIDMode="Static" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57))" />
                            <br />
                            <span><b>Is Default</b></span>
                            <asp:CheckBox runat="server" ID="chkIsUpdateEnabled" placeholder="Is Default" CssClass="form-control" ClientIDMode="Static" Width="40" />
                            <br />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" runat="server" class="btn btn-primary btnInsertData" id="btnUpdateData" onserverclick="UpdateData">Save changes</button>
                        <button type="button" class="btn btn-secondary closes" id="btnUpdatesuboperationclose">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $("#btnNew").click(function () {
                debugger;
                $("#addSubOperation").show();
            });
            $(".linkbutton").click(function () {
                debugger;
                $("#UpdateSubOperation").show();
                $("#txtUpdateStation").val($(this).closest("tr").find('td:eq(1)').text().trim());
                $("#txtUpdateOperation").val($(this).closest("tr").find('td:eq(2)').text().trim());
                $("#txtUpdateSubOperation").val($(this).closest("tr").find('td:eq(3)').text().trim());
                $("#txtUpdateCycleTime").val($(this).closest("tr").find('td:eq(4)').text().trim());

                
                $("#chkIsUpdateEnabled")[0].checked = $(this).closest("tr").find('td:eq(5)').children()[0].checked;
            });
            $(".closes").click(function () {
                $("#addSubOperation").hide();
                $("#UpdateSubOperation").hide();
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#btnNew").click(function () {
                $("#addSubOperation").show();
            });
            $(".linkbutton").click(function () {
                debugger;
                $("#UpdateSubOperation").show();
                $("#txtUpdateStation").val($(this).closest("tr").find('td:eq(1)').text().trim());
                $("#txtUpdateOperation").val($(this).closest("tr").find('td:eq(2)').text().trim());
                $("#txtUpdateSubOperation").val($(this).closest("tr").find('td:eq(3)').text().trim());
                $("#txtUpdateCycleTime").val($(this).closest("tr").find('td:eq(4)').text().trim());
                $("#chkIsUpdateEnabled")[0].checked = $(this).closest("tr").find('td:eq(5)').children()[0].checked;
            });
            $(".closes").click(function () {
                $("#addSubOperation").hide();
                $("#UpdateSubOperation").hide();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
