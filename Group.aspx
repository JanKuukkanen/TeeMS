<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Group.aspx.cs" Inherits="Group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Change title to show the group currently being viewed -->
    <title id="groupTitle" runat="server">Group</title>
    <script src="JS/Group.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-twothird" style="width:auto;">

        <div id="divDefault" runat="server" class="w3-container" visible="true" style="float:left; width:50%;">
            <div class="w3-container" style="float:left; width:50%">
                <asp:LinkButton ID="lbtnTriggerTitleChange" runat="server" OnClick="lbtnTriggerTitleChange_Click">
                    <h1 id="h1GroupName" runat="server">Undefined Group</h1>
                </asp:LinkButton>
            </div>

            <div class="w3-container" style="float:right; width:50%;">
                <img id="imgGroupPicture" runat="server" alt="Group picture" width="150" height="150" /> <br />
                <asp:Button ID="btnChangePicture" runat="server" Text="Change picture" CssClass="w3-btn" OnClick="btnChangePicture_Click" />
            </div>
        </div>

        <div id="divDuringChange" runat="server" class="w3-container w3-blue" visible="false" style="padding-top:5%; padding-bottom:5%; width:50%; float:left;">
            <div id="divTitleChanger" runat="server" visible="false">
                <asp:TextBox ID="txtTitleChanger" runat="server" />
                <asp:RequiredFieldValidator ID="GroupNameRequired" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtTitleChanger" ValidationGroup="TitleChange"></asp:RequiredFieldValidator>

                <asp:Button ID="btnTitleChanger" runat="server" Text="Change Name" CssClass="w3-btn" OnClick="btnTitleChanger_Click" ValidationGroup="TitleChange" />
                <asp:Button ID="btnTitleCancel" runat="server" Text="Cancel" CssClass="w3-btn" OnClick="btnTitleCancel_Click" />
            </div>

            <div id="divImageChanger" runat="server" visible="false">
                <asp:TextBox ID="txtImageChanger" runat="server" />
                <asp:RequiredFieldValidator ID="ImageRequired" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtImageChanger" ValidationGroup="ImageChange"></asp:RequiredFieldValidator>

                <asp:Button ID="btnImageChanger" runat="server" Text="Change Image" CssClass="w3-btn" OnClick="btnImageChanger_Click" ValidationGroup="ImageChange" />
                <asp:Button ID="btnImageCancel" runat="server" Text="Cancel" CssClass="w3-btn" OnClick="btnImageCancel_Click" />
            </div>
        </div>

        <div style="float:left; width:50%;">
            <div class="w3-container" style="float:left;">
                <h2>Group Members</h2>

                <div id="divGroupMembers" runat="server" visible="true">
                    <asp:RadioButtonList ID="rblGroupMembers" runat="server"></asp:RadioButtonList>
                    <br />
                    <asp:Button ID="btnAddMembers" runat="server" Text="Add members" OnClick="btnAddMembers_Click" CssClass="w3-btn" />
                    <asp:Button ID="btnDeleteMember" runat="server" Text="Remove Member" OnClick="btnDeleteMember_Click" CssClass="w3-btn" />
                    <asp:Button ID="btnShowInfo" runat="server" Text="Show info" OnClick="btnShowInfo_Click" CssClass="w3-btn" />
                </div>

                <div id="divSearch" runat="server" visible="false" style="padding-left:5%; padding-right:10%;" >
                    <h4>Search</h4>
                    <asp:TextBox ID="txtSearchGroupMembers" runat="server" /> <br />
                    <asp:Button ID="btnSearchGroupMembers" runat="server" Text="Search" OnClick="btnSearchGroupMembers_Click" CssClass="w3-btn" Style="margin-top:5px;" />
                    <hr style="color:#000;background-color:#000; height:5px;" />
                    <asp:GridView ID="gvGroupMembers" OnSelectedIndexChanged="gvGroupMembers_SelectedIndexChanged" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:ButtonField DataTextField="username" HeaderText="Username" CommandName="Select" />                            
                            <asp:BoundField DataField="first_name" HeaderText="First name" />
                            <asp:BoundField DataField="last_name" HeaderText="Last name" />
                            <asp:BoundField DataField="email" HeaderText="Email" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <div style="float:left; width:60%;">
            <div class="w3-container" style="float: left; margin-top:40px;">

                <!-- This will become a list of elements instead of a dropdownlist -->
                <h2>Group projects</h2>
                <asp:DropDownList ID="ddlProjectList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProjectList_SelectedIndexChanged" />
                <br />

                <h2 style="margin-top: 60px;">Project info</h2>
                <table>
                    <tr>
                        <td style="border: 1px solid black; width:350px; height:250px; vertical-align:top;">
                            <asp:Label ID="lbProjectInfo" runat="server" />
                        </td>
                    </tr>
                </table>

                <div>
                    <asp:Label ID="lbMessages" runat="server" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

