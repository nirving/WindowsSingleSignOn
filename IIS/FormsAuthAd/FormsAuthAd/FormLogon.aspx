<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormLogon.aspx.cs" Inherits="FormsAuthAd.Logon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Logon</title>
</head>
<body>
    <form id="Login" method="post" runat="server">
    <asp:Label ID="Label2" runat="server">Username:</asp:Label>
    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox><br />
    <asp:Label ID="Label3" runat="server">Password:</asp:Label>
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
    <asp:Label ID="Label1" runat="server">Domain:</asp:Label>
    <asp:DropDownList ID="txtDomain" runat="server" DataTextField="Name" DataValueField="Path"
        DataSourceID="ObjectDataSource1">
    </asp:DropDownList>
    <br />
    <asp:Label ID="errorLabel" runat="server"></asp:Label>
    <br />
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"></asp:Button>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetDomains"
        TypeName="FormsAuthAd.Logon"></asp:ObjectDataSource>
    </form>
</body>
</html>
