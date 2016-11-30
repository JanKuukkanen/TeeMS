<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Home</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-twothird">
        <div class="w3-container">
            <h1>Your Projects</h1>
            <div id="projectDiv" runat="server">

            </div>
            <a class="w3-btn" href="CreateProject.aspx">Create new Project</a>
        </div>

        <div class="w3-container">
            <h1>Your Groups</h1>
            <div id="groupDiv" runat="server">

            </div>
            <asp:Button ID="btnCreateGroup" runat="server" Text="Create new Group" CssClass="w3-btn" OnClick="btnCreateGroup_Click" />
        </div>

        <div class="w3-container">
            <asp:Label ID="lbMessages" runat="server" Text="" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>