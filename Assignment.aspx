<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Assignment.aspx.cs" Inherits="Assignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Change title to show the assignment currently being viewed -->
    <title id="titleAssignmentTitle" runat="server">Undefined Assignment</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-twothird" style="width:auto;">
        <div id="divTitle" class="w3-container" style="float:left; width:50%;">
            <div>
                <h1 id="h1AssignmentName" runat="server">Undefined Assignment</h1>
            </div>

            <div>
                <h2>Description</h2>
                <asp:TextBox ID="txtAssignmentDescription" runat="server" TextMode="MultiLine" Height="100px" Width="450px" ReadOnly="false" ></asp:TextBox>
                <br />
                <asp:Button ID="btnEditAssignmentDescription" runat="server" Text="Edit description" CssClass="w3-btn" />
            </div>
        </div>

        <div id="divAssignmentMembers" class="w3-container" style="float:left; width:50%;">
            <div>
                <h2>Assignment members</h2>
            </div>

            <div id="divMemberList">

            </div>
        </div>

        <div id="divWorkflow" class="w3-container" style="float:left; width:100%; margin-top:40px;">
            <div class="w3-container">
                <h2>Workflow</h2>
            </div>

            <div id="divAssignmentComponents" class="w3-container">

            </div>

            <div>
                <asp:Label ID="lbMessages" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

