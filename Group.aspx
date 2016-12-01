<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Group.aspx.cs" Inherits="Group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Change title to show the group currently being viewed -->
    <title id="groupTitle" runat="server">Group</title>
    <script src="JS/Group.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-twothird">

        <div id="divDefault" runat="server" class="w3-container" visible="true">
            <asp:LinkButton ID="lbtnTriggerTitleChange" runat="server" OnClick="lbtnTriggerTitleChange_Click">
            <h1 id="h1GroupName" runat="server">Undefined Group</h1>
            </asp:LinkButton>
        </div>

        <div id="divDuringChange" runat="server" class="w3-container w3-blue" visible="false" style="padding-top:5%; padding-bottom:5%;">
            <asp:TextBox ID="txtTitleChanger" runat="server" />
            <asp:RequiredFieldValidator ID="GroupNameRequired" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtTitleChanger" ValidationGroup="TitleChange"></asp:RequiredFieldValidator>

            <asp:Button ID="btnTitleChanger" runat="server" Text="Change Name" CssClass="w3-btn" OnClick="btnTitleChanger_Click" ValidationGroup="TitleChange" />
            <asp:Button ID="btnTitleCancel" runat="server" Text="Cancel" CssClass="w3-btn" OnClick="btnTitleCancel_Click" />
        </div>

        <div class="w3-container" style="float:left;">
            <h2>Group projects</h2>
            <asp:DropDownList ID="ddlProjectList" runat="server" /> <br />

            <h2 Style="margin-top:60px;">Project info</h2>
            <asp:ListBox ID="listbProjectInfo" runat="server"></asp:ListBox>

            <div>
                <asp:Label ID="lbMessages" runat="server" />
            </div>
        </div>

        <div class="w3-container" style="margin-left:35%;">
            <h2>Group Members</h2>
            <asp:RadioButtonList ID="rblGroupMembers" runat="server"></asp:RadioButtonList> <br />
            <asp:Button ID="btnAddMembers" runat="server" Text="Add members" CssClass="w3-btn" />
            <asp:Button ID="btnShowInfo" runat="server" Text="Show info" CssClass="w3-btn" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

