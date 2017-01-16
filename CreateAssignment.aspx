<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateAssignment.aspx.cs" Inherits="CreateAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Create Assignment</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest">

        <div class="w3-container">
            <h1>Create a new assignment</h1>
            <hr />
        </div>

        <div class="w3-container" style="float:left;">
            <div class="w3-container">
                <asp:Label ID="lbAssignmentName" runat="server" Text="Assignment Name: " />
                <asp:TextBox ID="txtAssignmentName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNameRequired" runat="server" ErrorMessage="Assignment name required" ControlToValidate="txtAssignmentName"></asp:RequiredFieldValidator>
                <br />
            </div>

            <div class="w3-container" style="margin-top: 40px;">
                <asp:Label ID="lbAssignmentDescription" runat="server" Text="Assignment Description:" />
                <br />
                <asp:TextBox ID="txtAssignmentDescription" runat="server" TextMode="MultiLine" Height="80px"></asp:TextBox>
            </div>

            <div class="w3-container" style="margin-top: 40px;">
                <asp:Label ID="lbUserList" runat="server" Text="Add a user to the assignment" />
                <br />
                <asp:DropDownList ID="ddlUserList" runat="server" />
            </div>

            <div class="w3-container" style="margin-top: 40px;">
                <asp:Label ID="lbDueDate" runat="server" Text="Due date" />
                <asp:Calendar ID="calendarDueDate" runat="server"></asp:Calendar>
                <br />
                <asp:Button ID="btnCreateAssignment" runat="server" Text="Create Assignment" OnClick="btnCreateAssignment_Click" Style="margin-top: 5px;" />
            </div>

            <!-- Still to add: privacy rules -->

            <div class="w3-container">
                <asp:Label ID="lbmessages" runat="server" />
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

