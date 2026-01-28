<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteTextBox.aspx.cs" Inherits="Web_TPMTrakDashboard.AutoCompleteTextBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Auto Complete Textbox</title>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.22/jquery-ui.js"></script>
    <link rel="stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css" />
    <script>
        $(document).ready(function () {
            $("#txtEmployee").autocomplete({
                source: function (request, response) {
                    var param = { EmpName: $('#txtEmployee').val() };
                    $.ajax({
                        url: "AutoCompleteTextBox.aspx/getEmployees",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            console.log(JSON.stringify(data));
                            response($.map(data.d, function (item) {
                                // console.log({  Name: item.EmpName });
                                return {
                                    value: item.EmpName + " (" + item.Address + ")"
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message)
                            // console.log("Ajax Error!");  
                        }
                    });
                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblEmployee" Text="Employee Search" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmployee" runat="server" Width="200" placeholder="Employee Name"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
