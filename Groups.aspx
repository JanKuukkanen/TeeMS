<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Groups.aspx.cs" Inherits="Groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Your Groups</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest">
        <div class="w3-container">
            <h1>Your Groups</h1>
            <div id="divYourGroups" runat="server">

            </div>
            <asp:Button ID="btnCreateGroup" runat="server" Text="Create new Group" CssClass="w3-btn" Style="margin-top:20px;" OnClick="btnCreateGroup_Click" />
        </div>

        <div class="w3-container">
            <asp:Label ID="lbMessages" runat="server" Text="" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

