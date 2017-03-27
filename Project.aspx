<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Project.aspx.cs" Inherits="Project" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link href="Content/bootstrap.css" rel="stylesheet" type="text/css" />

    <!-- Change title so that it shows the name of the project currently being worked on -->
    <title id="projectTitle" runat="server">Project</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest" style="width:auto">

        <asp:ScriptManager ID="smComments" runat="server" />

        <div id="divDefault" runat="server" class="w3-container" visible="true" style="float:left; width:50%">
            <div class="w3-container" style="float:left; width:50%;">
                <asp:LinkButton ID="lbtnTriggerTitleChange" OnClick="lbtnTriggerTitleChange_Click" runat="server">
                    <h1 id="h1ProjectName" runat="server">Undefined Project</h1>
                </asp:LinkButton>
                <h3 id="h3ProjectTag" runat="server">Project tag: Undefined</h3>
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

                <div class="w3-container" style="margin-top:50px;">
                    <asp:Button ID="btnArchiveProject" runat="server" Text="Archive Project" CssClass="w3-btn w3-red" />
                    <ajaxToolkit:ConfirmButtonExtender ID="cbtnArchiveProject" runat="server" TargetControlID="btnArchiveProject" ConfirmText="Are you sure you wish to archive this Project?"
                     DisplayModalPopupID="mpeConfirmArchiveProject" />

                    <!-- ModalPopupExtender -->
                    <ajaxToolkit:ModalPopupExtender ID="mpeConfirmArchiveProject" runat="server" PopupControlID="panelConfirmArchiveProject" TargetControlID="btnArchiveProject"
                        CancelControlID="btnCancelArchiveProject" BackgroundCssClass="w3-blue-grey w3-opacity">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="panelConfirmArchiveProject" runat="server" CssClass="w3-purple">
                        <div style="width:500px; margin:10px 10px 10px 10px;">

                            <div id="div2" style="margin:auto; width:400px;">
                                <!-- Display confirmnation text -->
                                <asp:Label ID="lbConfirmArchiveProject" runat="server" Text="Are you sure you wish to archive this project?" />
                            </div>

                            <div style=" margin-left:30%; margin-bottom:10px; margin-top:10px; width:400px;">
                                <asp:Button ID="btnConfirmArchiveProject" runat="server" Text="Confirm" OnClick="btnConfirmArchiveProject_Click" CssClass="w3-btn" />
                                <asp:Button ID="btnCancelArchiveProject" runat="server" Text="Cancel" CssClass="w3-btn" />
                            </div>
                        </div>
                    </asp:Panel>
                    <!-- ModalPopupExtender -->
                </div>
            </div>
        </div>

        <div style="float:left;">
            <div class="w3-container" style="float: left;">

                <div id="divProjectDesc">
                    <h2>Project description</h2>

                    <asp:UpdatePanel ID="upProjectDescription" runat="server" UpdateMode="Conditional">
                        
                        <ContentTemplate>

                            <asp:TextBox ID="txtProjectDescription" runat="server" TextMode="MultiLine" Height="100px" Width="450px" Style="vertical-align:top;" ReadOnly="true" />
                            <br />
                            <asp:Button ID="btnEditDescription" runat="server" Text="Edit description" OnClick="btnEditDescription_Click" CssClass="w3-btn" />

                            <asp:Button ID="btnUpdateProjectDescription" runat="server" OnClick="UpdateProjectDescription" style="display:none;" />
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>

                <!-- This will contain all the charts of the workflow -->
                <div id="divWorkFlow" style="margin-top:40px;">
                    <h2>Workflow</h2>

                    <h3>Due date</h3>
                    <asp:Calendar ID="calendarDueDate" runat="server"></asp:Calendar>
                    <br />
                    <!-- Information on the projects progress will be displayed here -->
                    <h3>Project progress</h3>

                    <asp:UpdatePanel ID="upAssignmentProgress" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <div id="divAssignmentProgress" runat="server">
                                <h4 runat="server">No current assignments!</h4>
                            </div>

                            <asp:Button ID="btnUpdateAssignmentProgress" runat="server" OnClick="UpdateAssignmentProgress" style="display:none;" />
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>

                <!-- Comments section -->
                <div id="divProjectComments" style="margin-top:40px";>
                    <h3>Comments</h3>

                    <asp:UpdatePanel ID="upComments" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <div id="divProjectCommentMessages" runat="server">

                            </div>
                              
                            <div style="margin-top:40px;">
                                <asp:TextBox ID="txtWriteComment" runat="server" TextMode="MultiLine" Width="400" Height="100" ValidationGroup="CommentValidation" />
                                <asp:RequiredFieldValidator ID="rfvCommentContentRequired" runat="server" ControlToValidate="txtWriteComment" ErrorMessage="Write a comment before sending!" 
                                                            ToolTip="Write a comment before sending." ForeColor="Red" ValidationGroup="CommentValidation" />
                                <br />

                                <asp:Button ID="btnSaveComment" runat="server" Text="Send Comment" OnClick="btnSaveComment_Click" CssClass="w3-btn" />
                            </div>

                            <asp:Button ID="btnUpdateComments" runat="server" OnClick="UpdateComments" style="display:none;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!-- Comments section -->

                <div>
                    <asp:Label ID="lbMessages" runat="server" />
                </div>

                <!-- SignalR comments section -->
                <div id="divProjectCommentSection" style="margin-top:40px";>
                    <h3>Comments</h3>

                        <div id="divCommentMessages">

                        </div>
                              
                        <div style="margin-top:40px;">
                            <textarea id="txtWriteComments" style="width:400px; height:100px;"></textarea>
                            
                            <!-- Find out how to do a requiredfieldvalidator in javascript -->
                            <br />

                            <button type="button" id="btnSaveComments" class="w3-btn">Send Comment</button>
                        </div>
                        <button type="button" id="btnHiddenUpdateComments" style="display:none;"></button>
                </div>
                <!-- SignalR comments section -->

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
                var project = $.connection.projectHub;
                // Create a function that the hub can call to broadcast messages.
                project.client.updateComments = function () {
                    // Trigger the onclick event of a hidden button in upComments updatepanel. 
                    document.getElementById('<%=btnUpdateComments.ClientID%>').click();
                };

                project.client.updateProjectDescription = function () {
                    // Trigger the onclick event of a hidden button in upProjectDescription updatepanel.
                    document.getElementById('<%=btnUpdateProjectDescription.ClientID%>').click();
                };

                // Set connection parameters
                $.connection.hub.qs = { 'Project': urlParams.get('Project') };

                // Start the connection.
                $.connection.hub.start().done(function () {

                    $('#<%=btnSaveComment.ClientID%>').click(function () {

                        // Call the BroadcastUpdateComments method on the hub. 
                        project.server.broadcastUpdateComments();
            
                    });

                    $('#<%=btnEditDescription.ClientID%>').click(function () {

                        console.log("Let's Start");

                        if ( $('#<%=btnEditDescription.ClientID%>').is('[readonly]')) {
                            // Call the BroadcastUpdateProjectDescription method on the hub.
                            console.log("Hello");
                            project.server.broadcastUpdateProjectDescription();
                        }

                    });
                });
            });

        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

