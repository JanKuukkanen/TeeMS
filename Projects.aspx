<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Projects.aspx.cs" Inherits="Projects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Your Projects</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest">
        <div class="w3-container">
            <h1>Your Projects</h1>
            <div id="divYourProjects" runat="server">

            </div>
            <a class="w3-btn" href="CreateProject.aspx" style="margin-top:20px;">Create new Project</a>
        </div>

        <div class="w3-container">
            <asp:Label ID="lbMessages" runat="server" Text="" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

