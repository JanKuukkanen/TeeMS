<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Home</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-twothird">
        <div class="w3-container">
            <h1>Your Projects</h1>
            <div id="divYourProjects" runat="server">

            </div>
            <a class="w3-btn" href="CreateProject.aspx" style="margin-top:20px;">Create new Project</a>
        </div>

        <div class="w3-container" style="margin-top:80px;">
            <h1>Your Groups</h1>
            <div id="divYourGroups" runat="server">

            </div>
            <asp:Button ID="btnCreateGroup" runat="server" Text="Create new Group" CssClass="w3-btn" Style="margin-top:20px;" OnClick="btnCreateGroup_Click" />
        </div>

        <div class="w3-container">
            <asp:Label ID="lbMessages" runat="server" Text="" />

            <div>
                <asp:Label ID="lbErrors" runat="server" Text="" />
            </div>

            <div>
                <asp:Label ID="lbGroupErrors" runat="server" Text="" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>