<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Project.aspx.cs" Inherits="Project" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link href="Content/bootstrap.css" rel="stylesheet" type="text/css" />

    <!-- Change title so that it shows the name of the project currently being worked on -->
    <title id="projectTitle" runat="server">Project</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest" style="width:auto">

        <asp:ScriptManager ID="smComments" runat="server" />

        <!-- SignalR title and image change -->

        <div id="divTitleAndImageDefault" class="w3-container" visible="true" style="float:left; width:50%">
            <div class="w3-container" style="float:left; width:50%;">
                
                <h1 id="h1ProjectTitle"><input type="button" id="btnChangeProjectTitle" value="Undefined Project" style="background: none; border: none; padding-bottom:4px;" /></h1>
                
                <h3 id="h3ProjectTagNro">Project tag: Undefined</h3>
            </div>

            <div class="w3-container" style="float:left; width:50%;">
                <div class="w3-container">
                    <img id="imgProjectImage" src="/Images/no_image.png" alt="Project picture" width="150" height="150" />
                    <br />
                    <button type="button" id="btnProjectImageChange" class="w3-btn">Change picture</button>
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

                <div id="divProjectDescription">
                    <h2>Project description</h2>

                    <textarea id="txtProjectsDescription" readonly="readonly" style="height:100px; width:450px; vertical-align:top;"></textarea>
                    <br />
                    <button type="button" id="btnEditProjectDescription" class="w3-btn" >Edit description</button>

                    <button type="button" id="btnSaveProjectDescription" class="w3-btn" style="display:none;">Save description</button>

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

                            <asp:Button ID="btnUpdateComments" runat="server" OnClick="UpdateComments" style="display:none;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!-- Comments section -->

                <!-- SignalR comments section -->
                <div id="divProjectCommentWrite" style="margin-top:40px";>
                              
                        <div style="margin-top:40px;">
                            <textarea id="txtWriteComments" style="width:400px; height:100px;"></textarea>
                            
                            <!-- Find out how to do a requiredfieldvalidator in javascript -->
                            <br />

                            <button type="button" id="btnSaveComments" class="w3-btn">Send Comment</button>
                        </div>
                </div>
                <!-- SignalR comments section -->

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
                var project = $.connection.projectHub;
                // Create a function that the hub can call to broadcast messages.
                project.client.updateComments = function () {
                    // Trigger the onclick event of a hidden button in upComments updatepanel. 
                    document.getElementById('<%=btnUpdateComments.ClientID%>').click();
                };

                project.client.updateAssignmentProgress = function () {
                    // Trigger the onclick event of a hidden button in upAssignmentProgress updatepanel
                    document.getElementById('<%=btnUpdateAssignmentProgress.ClientID%>').click();
                };

                project.client.updateProjectImage = function (project_imageurl) {
                    // Change the project image.
                    $('#imgProjectImage').attr("src", project_imageurl);
                };

                // For some reason this function only activates if projectname variable value is the same as the current projectname value in the database
                project.client.updateProjectName = function (projectname) {

                    // Change the project title.
                    $('#btnChangeProjectTitle').val(projectname);

                };

                project.client.updateProjectDescription = function (projectdesc) {
                    // Update the projects description.
                    $('#txtProjectsDescription').val(projectdesc);
                };

                // Create a function the hub can call to insert the project picture upon connecting to the hub
                project.client.insertProjectName = function (projectname, projecttag, archived) {

                    if (archived != "true") {
                        // Insert the projectdescription in to the textarea
                        // $('#btnChangeProjectTitle').text(projectname);
                        $('#btnChangeProjectTitle').val(projectname);

                        // Insert project tag in to textarea
                        $('#h3ProjectTagNro').text('Project tag: #PRO' + projecttag);
                    }
                    else {

                        //$('#btnChangeProjectTitle').text(projectname);
                        $('#btnChangeProjectTitle').val(projectname);
                        $('#h3ProjectTagNro').text('This project has been archived and cannot be edited further!');
                        $('#btnChangeProjectTitle').prop("disabled", true);
                    }

                }

                // Create a function the hub can call to insert the project picture upon connecting to the hub
                project.client.insertProjectImage = function (projectpic_url, archived) {

                    if (archived != "true") {
                        // Insert the projectdescription in to the textarea
                        $('#imgProjectImage').attr("src", projectpic_url);
                    }
                    else {
                        $('#imgProjectImage>').attr("src", projectpic_url);
                        $('#btnProjectImageChange').prop("disabled", true);
                    }

                }

                // Create a function the hub can call to insert the project description upon connecting to the hub
                project.client.insertProjectDescription = function (projectdesc, archived) {

                    if (archived != "true")
                    {
                        // Insert the projectdescription in to the textarea
                        $('#txtProjectsDescription').val(projectdesc);
                    }
                    else
                    {
                        $('#txtProjectsDescription').val(projectdesc);
                        $('#btnEditProjectDescription').prop("disabled", true);
                    }

                }

                // Set connection parameters
                $.connection.hub.qs = { 'Project': urlParams.get('Project') };

                // Start the connection.
                $.connection.hub.start().done(function () {

                    $('#btnSaveComments').click(function () {

                        var comment = $('textarea#txtWriteComments').val();
                        $('textarea#txtWriteComment').val('');

                        project.server.saveComment(comment);
                    });

                    $('#btnChangeTitle').click(function () {

                        var projectname = $('#txtChangeTitle').val();
                        $('#txtChangeTitle').val('');

                        $('#btnChangeProjectTitle').val(projectname);

                        $('#divChangeTitleAndImage').css({ "display": 'none' });
                        $('#divTitleAndImageDefault').css({ "display": 'inline-block' });
                        $('#divChangeTitle').css({ "display": 'none' });

                        project.server.saveProjectName(projectname);

                    });

                    $('#btnChangeImage').click(function () {

                        var project_imageurl = $('#txtChangeImage').val();
                        $('#txtChangeImage').val('');

                        $('#imgProjectImage').attr("src", project_imageurl);

                        $('#divChangeTitleAndImage').css({ "display": 'none' });
                        $('#divTitleAndImageDefault').css({ "display": 'inline-block' });
                        $('#divChangeImage').css({ "display": 'none' });

                        project.server.saveProjectImage(project_imageurl);

                    });

                    $('#btnSaveProjectDescription').click(function () {

                        $('#txtProjectsDescription').attr("readonly", "readonly");
                        $('#btnSaveProjectDescription').css({ "display": 'none' });
                        $('#btnEditProjectDescription').css({ "display": 'inline-block' });

                        var projectdesc = $('#txtProjectsDescription').val();

                        project.server.saveProjectDescription(projectdesc);

                    });
                });

                // Buttons with functionality that alter the css of elements
                // Button that will hide the regular title and image and bring up the change title textbox
                $('#btnChangeProjectTitle').click(function () {
                    $('#divTitleAndImageDefault').css({ "display": 'none' });
                    $('#divChangeTitleAndImage').css({ "display": 'inline-block' });
                    $('#divChangeTitle').css({ "display": 'inline-block' });
                });

                // Button that will hide the regular title and image and bring up the change image textbox
                $('#btnProjectImageChange').click(function () {
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

                // Button that will make the project description textbox able to be modified
                $('#btnEditProjectDescription').click(function () {

                    $('#txtProjectsDescription').removeAttr('readonly');
                    $('#btnEditProjectDescription').css({ "display": 'none' });
                    $('#btnSaveProjectDescription').css({ "display": 'inline-block' });

                });

            });

        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

