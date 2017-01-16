<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Project.aspx.cs" Inherits="Project" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Change title so that it shows the name of the project currently being worked on -->
    <title id="projectTitle" runat="server">Project</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest" style="width:auto">

        <div id="divDefault" runat="server" class="w3-container" visible="true" style="float:left; width:50%">
            <div class="w3-container" style="float:left; width:50%;">
                <asp:LinkButton ID="lbtnTriggerTitleChange" OnClick="lbtnTriggerTitleChange_Click" runat="server">
                    <h1 id="h1ProjectName" runat="server">Undefined Project</h1>
                </asp:LinkButton>
            </div>

            <div class="w3-container" style="float:left; width:50%;">
                <div class="w3-container">
                    <img id="imgProjectPicture" runat="server" alt="Project picture" width="150" height="150" />
                    <br />
                    <asp:Button ID="btnChangePicture" runat="server" Text="Change picture" OnClick="btnChangePicture_Click" CssClass="w3-btn" />
                </div>
            </div>
        </div>

        <div id="divDuringChange" runat="server" class="w3-container w3-blue" visible="false" style="padding-top:5%; padding-bottom:5%; width:50%; float:left;">
            <div id="divTitleChanger" runat="server" visible="false">
                <asp:TextBox ID="txtTitleChanger" runat="server" />
                <asp:RequiredFieldValidator ID="ProjectNameRequired" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtTitleChanger" ValidationGroup="TitleChange"></asp:RequiredFieldValidator>

                <asp:Button ID="btnTitleChanger" runat="server" Text="Change Name" OnClick="btnTitleChanger_Click" CssClass="w3-btn" ValidationGroup="TitleChange" />
                <asp:Button ID="btnTitleCancel" runat="server" Text="Cancel" OnClick="btnTitleCancel_Click" CssClass="w3-btn" />
            </div>

            <div id="divImageChanger" runat="server" visible="false">
                <asp:TextBox ID="txtImageChanger" runat="server" />
                <asp:RequiredFieldValidator ID="ImageRequired" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtImageChanger" ValidationGroup="ImageChange"></asp:RequiredFieldValidator>

                <asp:Button ID="btnImageChanger" runat="server" Text="Change Image" OnClick="btnImageChanger_Click" CssClass="w3-btn" ValidationGroup="ImageChange" />
                <asp:Button ID="btnImageCancel" runat="server" Text="Cancel" OnClick="btnImageCancel_Click" CssClass="w3-btn" />
            </div>
        </div>

        <div style="float:left; width:50%;">
            <div class="w3-container" style="float:left;">
                <div class="w3-container">
                    <h2>Groups working on the project</h2>
                    <asp:DropDownList ID="ddlMemberGroupList" runat="server" />
                    <br />
                    <asp:Button ID="btnAddGroup" runat="server" Text="Add a Group" OnClick="btnAddGroup_Click" CssClass="w3-btn" style="margin-top:10px;" />
                    <asp:Button ID="btnRemoveGroup" runat="server" Text="Remove Group" OnClick="btnRemoveGroup_Click" CssClass="w3-btn" style="margin-top:10px;" />
                    <asp:Button ID="btnShowGroupInfo" runat="server" Text="Show info" OnClick="btnShowGroupInfo_Click" CssClass="w3-btn" style="margin-top:10px;" />
                </div>

                <div id="divSearch" runat="server" visible="false" style="padding-left:5%; padding-right:10%;" >
                    <h4>Search</h4>
                    <asp:TextBox ID="txtSearchGroups" runat="server" /> <br />
                    <asp:Button ID="btnSearchGroups" runat="server" Text="Search" OnClick="btnSearchGroups_Click" CssClass="w3-btn" Style="margin-top:5px;" />
                    <hr style="color:#000;background-color:#000; height:5px;" />
                    <asp:GridView ID="gvGroups" runat="server" OnSelectedIndexChanged="gvGroups_SelectedIndexChanged" AutoGenerateColumns="False">
                        <Columns>
                            <asp:ButtonField DataTextField="groupname" HeaderText="Group name" CommandName="Select" />                            
                            <asp:BoundField DataField="creation_date" HeaderText="Creation day" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="w3-container" style="margin-top:40px;">
                    <h2>Assignments</h2>
                    <asp:DropDownList ID="ddlAssignmentList" runat="server" />
                    <br />
                    <asp:Button ID="btnCreateNewAssignment" runat="server" Text="Create new" OnClick="btnCreateNewAssignment_Click" CssClass="w3-btn" style="margin-top:10px;" />
                    <asp:Button ID="btnShowAssignmentInfo" runat="server" Text="Show info" OnClick="btnShowAssignmentInfo_Click" CssClass="w3-btn" style="margin-top:10px;" />
                </div>
            </div>
        </div>

        <div style="float:left;">
            <div class="w3-container" style="float: left;">

                <div id="divProjectDesc">
                    <h2>Project description</h2>
                    <asp:TextBox ID="txtProjectDescription" runat="server" TextMode="MultiLine" Height="100px" Width="450px" Style="vertical-align:top;" ReadOnly="true" />
                    <br />
                    <asp:Button ID="btnEditDescription" runat="server" Text="Edit description" OnClick="btnEditDescription_Click" CssClass="w3-btn" />
                </div>

                <!-- This will contain all the charts of the workflow -->
                <div id="divWorkFlow" style="margin-top:40px;">
                    <h2>Workflow</h2>

                    <h3>Due date</h3>
                    <asp:Calendar ID="calendarDueDate" runat="server"></asp:Calendar>

                    <!-- Number of charts will be displayed here -->

                </div>

                <!-- This will be the comments section -->
                <div id="divComments">

                </div>

                <div>
                    <asp:Label ID="lbMessages" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

