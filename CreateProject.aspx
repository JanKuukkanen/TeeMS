<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateProject.aspx.cs" Inherits="CreateProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Create Project</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-twothird">

        <div class="w3-container">
            <h1>Create a new project</h1>
            <hr />
        </div>

        <div class="w3-container" style="float:left;">
            <div class="w3-container">
                <asp:Label ID="lbProjectName" runat="server" Text="Project Name: " />
                <asp:TextBox ID="txtProjectName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNameRequired" runat="server" ErrorMessage="Project name required" ControlToValidate="txtProjectName"></asp:RequiredFieldValidator>
                <br />
            </div>

            <div class="w3-container" style="margin-top: 40px;">
                <asp:Label ID="lbProjectDescription" runat="server" Text="Project Description:" />
                <br />
                <asp:TextBox ID="txtProjectDescription" runat="server" TextMode="MultiLine" Height="80px"></asp:TextBox>
            </div>

            <div class="w3-container" style="margin-top: 40px;">
                <asp:Label ID="lbGroupList" runat="server" Text="Add a group to the project" />
                <br />
                <asp:DropDownList ID="ddlGroupList" runat="server" />
            </div>

            <div class="w3-container" style="margin-top: 40px;">
                <asp:Label ID="lbDueDate" runat="server" Text="Due date" />
                <asp:Calendar ID="calendarDueDate" runat="server"></asp:Calendar>
                <br />
                <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" OnClick="btnCreateProject_Click" Style="margin-top: 5px;" />
            </div>

            <!-- Still to add: Picture to a project, privacy rules -->

            <div class="w3-container">
                <asp:Label ID="lbmessages" runat="server" />
            </div>
        </div>

        <div class="w3-container">
            <asp:Label ID="lbPictureURI" runat="server" Text="Picture URL:" />
            <asp:TextBox ID="txtPictureURI" runat="server" />
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

