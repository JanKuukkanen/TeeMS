<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewUser.aspx.cs" Inherits="ViewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Change title to show the name of the user being viewed -->
    <title id="userTitle" runat="server">User</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest">
        <div>
            <div class="w3-container">
                <h1 id="h1UserName" runat="server">Undefined Group</h1>
            </div>

            <div class="w3-container">
                <h2>User info</h2>
                <asp:Label ID="lbName" runat="server" Text="Nimi:" /> <br />
                <asp:Label ID="lbFirstName" runat="server" />
                <asp:Label ID="lbLastName" runat="server" />
                <br />
                <asp:Label ID="lbMail" runat="server" Text="Email:" /> <br />
                <asp:Label ID="lbEmail" runat="server" />
            </div>

            <div class="w3-container">
                <h2>Groups</h2>

                <div id="divUserGroups" runat="server" class="w3-container">

                </div>
            </div>

            <div class="w3-container">
                <h2>Projects</h2>

                <div id="divUserProjects" runat="server" class="w3-container">

                </div>
            </div>

            <div class="w3-container">
                <asp:Label ID="lbMessages" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

