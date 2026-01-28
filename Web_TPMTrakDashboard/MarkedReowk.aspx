<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="MarkedReowk.aspx.cs" Inherits="Web_TPMTrakDashboard.MarkedReowk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            font-weight: bold;
            height: 260px;
        }


        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: -15px 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            font-weight: bold;
            height: 340px;
        }

        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
        }

        #MainContent_grdmarkrework tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_grdmarkrework tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }

         #MainContent_grdreason tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_grdreason tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }

     
              .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
            height: 38px;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }
           
    </style>
    <div class="row">
        <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; color:white;text-align:center; text-align-last:center; font-family: Calibri;"></asp:Label>
        <div id="rework" runat="server">
                  <asp:HiddenField runat="server" ID="hdnMarkedForRework" ClientIDMode="Static" />
              <asp:HiddenField runat="server" ID="hdnAcceptedParts" ClientIDMode="Static" /> 
             <asp:HiddenField runat="server" ID="hdnrejectionTotalValue" ClientIDMode="Static" /> 
        <div  class="col-md-6" runat="server">
            <fieldset class="scheduler-border" id="serch">
                <legend class="scheduler-border commontd">Rework </legend>
                <table class="table-responsive table table-bordered" id="markrework" style="border:none">
                    <%--  class="table table-bordered" style="margin-top: -23px;"--%>
                    <thead>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <div class="commontd" style="margin-top: 5px;">Catagory</div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreworkcat" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" AutoPostBack="true" OnSelectedIndexChanged="ddlreworkcat_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="commontd" style="margin-top: 5px;">ID</div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreworkreason" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="commontd" style="margin-top: 5px;">Qty</div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtqty" CssClass="form-control"  runat="server" Text='<%# Eval("Dummy_Cycles") %>'  ClientIDMode="Static" onkeypress="return allowNumberic(event);" autocomplete="off"></asp:TextBox>
                            </td>
                        </tr>
                       
                        <tr>
                        <td colspan="2" style="align-items:center">
                            <div style="text-align-last:center">
                                  <asp:Button runat="server" Text="ADD" Style="display: inline-block" class="btn btn-info" ID="btnaddrework" OnClick="btnaddrework_Click"></asp:Button>
                            <asp:Button runat="server" Text="Update" Style="display: inline-block" class="btn btn-info" ID="btnupdaterework" OnClick="btnupdaterework_Click"></asp:Button>
                            <asp:Button runat="server" Text="Delete" Style="display: inline-block" class="btn btn-info" ID="btndeleterework" OnClick="btndeleterework_Click"></asp:Button>
                            </div>
                          
                        </td>
                        </tr>
                    </tbody>
                </table>

            </fieldset>

        </div>
        <div class="col-md-6" style="margin-top:10px;">
            <asp:GridView ID="grdmarkrework" runat="server" AutoGenerateColumns="False"
                CssClass="table table-bordered" ShowHeaderWhenEmpty="true" ShowHeader="true"
                EmptyDataText="No data available." OnSelectedIndexChanged="changedindex" HorizontalAlign="Center" HeaderStyle-CssClass="blue" BackColor="#FFFFFF">
                <Columns>
                   <%--  <asp:BoundField HeaderText="ID" DataField="ID" />--%>
                     <asp:TemplateField HeaderText="S.No.">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSlno" Text='<%# Eval("Slno") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Rework Qty"> 
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReworkQty" Text='<%# Eval("Rework_Qty") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Rework ID"><%-- kkkk--%>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReworkReason" Text='<%# Eval("Rework_Reason") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                 <%--   <asp:BoundField HeaderText="S.No." DataField="Slno" />
                    <asp:BoundField HeaderText="Rework Qty" DataField="Rework_Qty" />
                    <asp:BoundField HeaderText="Rework Reason" DataField="Rework_Reason" />--%>
					<asp:ButtonField ButtonType="Button"
							CommandName="Select"
							Text="Select"  ControlStyle-CssClass="btn btn-info"  />
                </Columns>
             
            </asp:GridView>
            <%-- DataField="ComponentI--%>
        </div>
        </div>
        <div id="reasondiv"  runat="server" >
               <div  class="col-md-6" runat="server">
            <fieldset class="scheduler-border" >
                <legend class="scheduler-border commontd">Reason </legend>
                <table class="table-responsive table table-bordered"  style="border:none">
                    <%--  class="table table-bordered" style="margin-top: -23px;"--%>
                    <thead>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <div class="commontd" style="margin-top: 5px;">Catagory</div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreasoncat" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" OnSelectedIndexChanged="ddlreasoncat_SelectedIndexChanged"  AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="commontd" style="margin-top: 5px;">Reason</div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreason" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="commontd" style="margin-top: 5px;">Qty</div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtreasonqty" CssClass="form-control"  runat="server" Text='<%# Eval("Dummy_Cycles") %>'  onkeypress="return allowNumberic(event);" autocomplete="off"></asp:TextBox>
                            </td>
                        </tr>
                       
                        <tr>
                        <td colspan="2" style="align-items:center">
                            <div style="text-align-last:center">
                                  <asp:Button runat="server" Text="ADD" Style="display: inline-block" class="btn btn-info" ID="reasonAdd" OnClick="reasonRejAdd_Click"></asp:Button>
                            <asp:Button runat="server" Text="Update" Style="display: inline-block" class="btn btn-info" ID="reasonUpdate" OnClick="reasonRejUpdate_Click"></asp:Button>
                            <asp:Button runat="server" Text="Delete" Style="display: inline-block" class="btn btn-info" ID="reasonDelete" OnClick="reasonRejDelete_Click"></asp:Button>
                            </div>
                          
                        </td>
                        </tr>
                    </tbody>
                </table>

            </fieldset>

        </div>
        <div class="col-md-6" style="margin-top:10px;">
            <asp:GridView ID="grdreason" runat="server" AutoGenerateColumns="False"
                CssClass="table table-bordered" ShowHeaderWhenEmpty="true" ShowHeader="true"
                EmptyDataText="No data available." HorizontalAlign="Center" OnSelectedIndexChanged="changedindex" HeaderStyle-CssClass="blue" BackColor="#FFFFFF">
                <Columns>
                    <%--<asp:BoundField HeaderText="ID" DataField="ID" />--%>
                        <asp:TemplateField HeaderText="S.No.">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSlno" Text='<%# Eval("Slno") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rejection Qty">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblRejectionQty" Text='<%# Eval("Rejection_Qty") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rejection Reason">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblRejectionReason" Text='<%# Eval("Rejection_Reason") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                  <%--  <asp:BoundField HeaderText="S.No." DataField="Slno"  />
                    <asp:BoundField HeaderText="Rejection Qty" DataField="Rejection_Qty" />
                    <asp:BoundField HeaderText="Rejection Reason" DataField="Rejection_Reason"/>--%>
					<asp:ButtonField ButtonType="Button"
							CommandName="Select"
							Text="Select"  ControlStyle-CssClass="btn btn-info"  />
                </Columns>
               
            </asp:GridView>
            <%-- DataField="ComponentI--%>
        </div>
        </div>

    </div>
    <script>
        function allowNumberic(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if ((charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
