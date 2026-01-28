<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AndonSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.AndonSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div>
            <span>Andon General Setting</span>
        </div>
        <div>
            <div class="panel panel-primary">
                <div class="panel-heading tabHeader">Application Setting </div>
                <table class="table table-bordered">
                    <tr id="tdAndonHide" runat="server">
                        <td>
                            <label class="control-label">Andon Title</label></td>
                        <td>
                            <asp:TextBox ID="txtAndonTitle" data-toggle="tooltip" runat="server" CssClass="form-control input-sm toolTip"> </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="control-label">Plant To Display</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPlantToDisplay" runat="server" CssClass="form-control input-sm">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="control-label">View Type</label></td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Text="By Shift" Value="Shift"></asp:ListItem>
                                <asp:ListItem Text="By Day" Value="Day"></asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <label class="control-label">Data Refresh Interval (sec.)</label></td>
                        <td>
                            <asp:DropDownList ID="ddlRefreshInterval" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                <asp:ListItem Text="70" Value="70"></asp:ListItem>
                                <asp:ListItem Text="80" Value="80"></asp:ListItem>
                                <asp:ListItem Text="90" Value="90"></asp:ListItem>
                                <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                <asp:ListItem Text="110" Value="110"></asp:ListItem>
                                <asp:ListItem Text="120" Value="120"></asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnSaveGeneralPageSetting" runat="server" Text="Save" CssClass="btn btn-primary" /></td>
                    </tr>
                </table>
            </div>
            <div>
                <input type="button" value="SAVE" />
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            BindPlant();
            BindData();
        });

        function BindPlant() {
            $.ajax({
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                url: "AndonSettings.aspx/GetPlantData",
                data: '{}',
                datatype: "json",
                success: function (response) {
                    
                },
                error: function (response) {
                    console.log(response)
                }
            });
        }

        function BindData() {
            $.ajax({
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                url: "AndonSettings.aspx/GetData",
                data: '{}',
                datatype: "json",
                success: function (response) {
                    BindTable(response.d);
                },
                error: function (response) {
                    console.log(response)
                }
            });
        }

        function BindTable(Data) {
            console.log(Data);
            $("[id$=ddlPlantToDisplay] option:selected").val(Data.PlantID);
            $("[id$=ddlRefreshInterval] option:selected").val(Data.Type);
            $("[id$=ddlRefreshInterval] option:selected").text(Data.Type);
            $("[id$=txtAndonTitle]").text(Data.AndonTitle);
            $("[id$=ddlType] option:selected").text(Data.Type);
            $("[id$=ddlType] option:selected").val(Data.Type);

        }
        $("btnSave").click(function () {
            if ($("[id$=ddlPlantToDisplay] option:selected").val == "") {
                alert(" Please Select Plant to Save")
                return false;
            }
            else if ($("[id$=ddlRefreshInterval] option:selected").val == "") {
                alert(" Please Select Refresh Interval to Save")
                return false;
            }
            else if ($("[id$=txtAndonTitle]").text == "") {
                alert(" Please add Andon Title to Save")
                return false;
            }
            else if ($("[id$=ddlType] option:selected").val == "") {
                alert(" Please Select Type to Save")
                return false;
            }
            else {
                var plantid = $("[id$=ddlPlantToDisplay] option:selected").val();
                var RefreshInterval = $("[id$=ddlRefreshInterval] option:selected").val();
                var Title = $("[id$=txtAndonTitle]").text();
                var Type = $("[id$=ddlType] option:selected").val();
                $.ajax({
                    contentType: "application/json; chartset=utf-8",
                    url: "AndonSettings.aspx/SaveDate",
                    datatype: "json",
                    data: '{PlantID:"' + plantid + '",RefreshInterval:"' + RefreshInterval + '",AndonTitle:"' + Title + '",Type:"' + Type + '"}',
                    success: function(response)
                    {
                        BindData();
                        alert(response.d);
                    },
                    error: function(response)
                    {
                        alert(response.d);
                    }
                });

            }

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
