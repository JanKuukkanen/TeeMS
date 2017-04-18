<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Group.aspx.cs" Inherits="Group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Change title to show the group currently being viewed -->
    <title id="groupTitle" runat="server">Group</title>
    <script src="JS/Group.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest" style="width:auto;">

        <!-- SignalR title and image change -->

        <div id="divTitleAndImageDefault" class="w3-container" visible="true" style="float:left; width:50%">
            <div class="w3-container" style="float:left; width:50%;">
                
                <h1 id="h1GroupTitle"><input type="button" id="btnChangeGroupTitle" value="Undefined Group" style="background: none; border: none; padding-bottom:4px;" /></h1>
                
                <h3 id="h3GroupTagNro">Group tag: Undefined</h3>
            </div>

            <div class="w3-container" style="float:left; width:50%;">
                <div class="w3-container">
                    <img id="imgGroupImage" src="/Images/no_image.png" alt="Group picture" width="150" height="150" />
                    <br />
                    <button type="button" id="btnGroupImageChange" class="w3-btn">Change picture</button>
                </div>
            </div>
        </div>

        <div id="divChangeTitleAndImage" class="w3-container w3-blue" style="padding-top:5%; padding-bottom:5%; width:50%; float:left; display:none;">
            <div id="divChangeTitle" style="display:none;">
                <input type="text" id="txtChangeTitle" style="color:black;" />
                <!-- Insert validator to check that textbox is not empty here -->

                <button type="button" id="btnChangeTitle" class="w3-btn">Change Name</button>
                <button type="button" id="btnChangeTitleCancel" class="w3-btn">Cancel</button>
            </div>

            <div id="divChangeImage" style="display:none;">
                <input type="text" id="txtChangeImage" style="color:black;" />
                <!-- Insert validator to check that textbox is not empty here -->

                <button type="button" id="btnChangeImage" class="w3-btn">Change Image</button>
                <button type="button" id="btnChangeImageCancel" class="w3-btn">Cancel</button>
            </div>
        </div>

        <!-- SignalR title and image change -->

        <div style="float:left; width:50%;">
            <div class="w3-container" style="float:left;">
                <h2>Group Members</h2>

                <div id="divGroupMembers" runat="server" visible="true">
                    <asp:RadioButtonList ID="rblGroupMembers" runat="server"></asp:RadioButtonList>
                    <br />
                    <asp:Button ID="btnAddMembers" runat="server" Text="Add members" OnClick="btnAddMembers_Click" CssClass="w3-btn" />
                    <asp:Button ID="btnAssignRole" runat="server" Text="Assign new role" CssClass="w3-btn" />
                    <asp:Button ID="btnDeleteMember" runat="server" Text="Remove Member" OnClick="btnDeleteMember_Click" CssClass="w3-btn" />
                    <asp:Button ID="btnShowInfo" runat="server" Text="Show info" OnClick="btnShowInfo_Click" CssClass="w3-btn" />
                </div>

                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>

                <ajaxToolkit:ConfirmButtonExtender ID="cbtnAssignRole" runat="server" TargetControlID="btnAssignRole" ConfirmText=""
                     DisplayModalPopupID="mpeConfirmAssignRole" />

                    <!-- ModalPopupExtender -->
                    <ajaxToolkit:ModalPopupExtender ID="mpeConfirmAssignRole" runat="server" PopupControlID="panelConfirmAssignRole" TargetControlID="btnAssignRole"
                        CancelControlID="btnCancelAssignRole" BackgroundCssClass="w3-blue-grey w3-opacity">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="panelConfirmAssignRole" runat="server" CssClass="w3-purple">
                        <div style="width:500px; margin:10px 10px 10px 10px;">

                            <div id="div2" style="margin:auto; width:400px;">
                                <!-- Display confirmnation text -->
                                <asp:Label ID="lbConfirmArchiveProject" runat="server" Text="Select a new role for the member." />
                            </div>

                            <div>
                                <asp:RadioButtonList ID="rblGroupRoleChange" runat="server" />
                            </div>

                            <div style=" margin-left:30%; margin-bottom:10px; margin-top:10px; width:400px;">
                                <asp:Button ID="btnConfirmAssignRole" runat="server" Text="Confirm" OnClick="btnConfirmAssignRole_Click" CssClass="w3-btn" />
                                <asp:Button ID="btnCancelAssignRole" runat="server" Text="Cancel" CssClass="w3-btn" />
                            </div>
                        </div>
                    </asp:Panel>
                    <!-- ModalPopupExtender -->

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


    <!--Script references. -->
        <!--Reference the jQuery library. -->
        <script src="Scripts/jquery-1.9.1.min.js"></script>
        <!--Reference the SignalR library. -->
        <script src="Scripts/jquery.signalR-2.2.1.min.js"></script>
        <!--Reference the autogenerated SignalR hub script. -->

        <script src="signalr/hubs" type="text/javascript"></script>

        <!--Add script to update the page and send messages.-->
        <script type="text/javascript">

            $(function () {

                var urlParams = new URLSearchParams(window.location.search);

                // Declare a proxy to reference the hub.
                var group = $.connection.groupHub;

                group.client.updateGroupImage = function (group_imageurl) {
                    // Change the group image.
                    $('#imgGroupImage').attr("src", group_imageurl);
                };

                // For some reason this function only activates if groupname variable value is the same as the current groupname value in the database
                group.client.updateGroupName = function (groupname) {

                    // Change the group title.
                    $('#btnChangeGroupTitle').val(groupname);

                };

                // Create a function the hub can call to insert the group picture upon connecting to the hub
                group.client.insertGroupName = function (groupname, grouptag) {

                    // Change the group title
                    $('#btnChangeGroupTitle').val(groupname);

                    // Insert group tag in to textarea
                    $('#h3GroupTagNro').text('Group tag: #PRO' + grouptag);

                }

                // Create a function the hub can call to insert the group picture upon connecting to the hub
                group.client.insertGroupImage = function (grouppic_url) {

                    // change the image in thr group page
                    $('#imgGroupImage').attr("src", grouppic_url);

                }

                // Set connection parameters
                $.connection.hub.qs = { 'Group': urlParams.get('Group') };

                // Start the connection.
                $.connection.hub.start().done(function () {

                    $('#btnChangeTitle').click(function () {

                        var groupname = $('#txtChangeTitle').val();
                        $('#txtChangeTitle').val('');

                        $('#btnChangeGroupTitle').val(groupname);

                        $('#divChangeTitleAndImage').css({ "display": 'none' });
                        $('#divTitleAndImageDefault').css({ "display": 'inline-block' });
                        $('#divChangeTitle').css({ "display": 'none' });

                        group.server.saveGroupName(groupname);

                    });

                    $('#btnChangeImage').click(function () {

                        var group_imageurl = $('#txtChangeImage').val();
                        $('#txtChangeImage').val('');

                        $('#imgGroupImage').attr("src", group_imageurl);

                        $('#divChangeTitleAndImage').css({ "display": 'none' });
                        $('#divTitleAndImageDefault').css({ "display": 'inline-block' });
                        $('#divChangeImage').css({ "display": 'none' });

                        group.server.saveGroupImage(group_imageurl);

                    });
                });

                // Buttons with functionality that alter the css of elements
                // Button that will hide the regular title and image and bring up the change title textbox
                $('#btnChangeGroupTitle').click(function () {
                    $('#divTitleAndImageDefault').css({ "display": 'none' });
                    $('#divChangeTitleAndImage').css({ "display": 'inline-block' });
                    $('#divChangeTitle').css({ "display": 'inline-block' });
                });

                // Button that will hide the regular title and image and bring up the change image textbox
                $('#btnGroupImageChange').click(function () {
                    $('#divTitleAndImageDefault').css({ "display": 'none' });
                    $('#divChangeTitleAndImage').css({ "display": 'inline-block' });
                    $('#divChangeImage').css({ "display": 'inline-block' });
                });
                
                // Button that will return the regular title and image to be visible
                $('#btnChangeTitleCancel').click(function () {
                    $('#divChangeTitleAndImage').css({ "display": 'none' });
                    $('#divTitleAndImageDefault').css({ "display": 'inline-block' });
                    $('#divChangeTitle').css({ "display": 'none' });
                });

                // Button that will return the regular title and image to be visible
                $('#btnChangeImageCancel').click(function () {
                    $('#divChangeTitleAndImage').css({ "display": 'none' });
                    $('#divTitleAndImageDefault').css({ "display": 'inline-block' });
                    $('#divChangeImage').css({ "display": 'none' });
                });

            });

        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

