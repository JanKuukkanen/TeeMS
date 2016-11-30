<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Login</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <asp:Login ID="loginUser"  runat="server" OnAuthenticate="loginUser_Authenticate"></asp:Login> <hr style="border-top: 1px solid black" />

    <div class="w3-container">
    <asp:Label ID="lbRegister" runat="server" Text="Don't have an account?" />
    <a href="Register.aspx">Register a new account</a>
    </div>

    <div class="w3-container">
        <asp:Label ID="lbMessages" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>