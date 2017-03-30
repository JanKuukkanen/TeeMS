<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Assignment.aspx.cs" Inherits="Assignment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Change title to show the assignment currently being viewed -->
    <title id="titleAssignmentTitle" runat="server">Undefined Assignment</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-rest" style="width:auto;">
        <div id="divTitle" class="w3-container" style="float:left; width:50%;">
            <div>
                <asp:Button ID="btnBackToProject" runat="server" Text="Back to project page" OnClick="btnBackToProject_Click" CssClass="w3-btn w3-blue-gray" />
                <h1 id="h1AssignmentName" runat="server">Undefined Assignment</h1>
            </div>

            <div>

                <div id="divAssignmentDescription">
                    <h2>Assignment description</h2>

                    <textarea id="txtAssignmentsDescription" readonly="readonly" style="height:100px; width:450px; vertical-align:top;"></textarea>
                    <br />
                    <button type="button" id="btnEditAssignmentDescription" class="w3-btn" >Edit description</button>

                    <button type="button" id="btnSaveAssignmentDescription" class="w3-btn" style="display:none;">Save description</button>

                </div>
            </div>
        </div>

        <div id="divAssignmentMembers" class="w3-container" style="float:left; width:50%;">
            <div>
                <h2>Assignment members</h2>
            </div>

            <!-- Memberlist on the Assignment.aspx frontpage -->
            <div id="divMemberList" runat="server">
                <asp:CheckBoxList ID="cblAssignmentMemberList" runat="server"></asp:CheckBoxList>
            </div>

            <div style="margin-top:10px;">
                <asp:Button ID="btnAddAssignmentMember" runat="server" Text="Add Member" OnClick="btnAddAssignmentMember_Click" CssClass="w3-btn" />
                <asp:Button ID="btnRemoveAssignmentMember" runat="server" Text="Remove Member" OnClick="btnRemoveAssignmentMember_Click" CssClass="w3-btn" />
            </div>

            <div id="divSearch" runat="server" visible="false" style="padding-left:5%; padding-right:10%;" >
                    <h4>Search</h4>
                    <asp:TextBox ID="txtSearchPersons" runat="server" /> <br />
                    <asp:Button ID="btnSearchPersons" runat="server" Text="Search" OnClick="btnSearchPersons_Click" CssClass="w3-btn" Style="margin-top:5px;" />
                    <hr style="color:#000;background-color:#000; height:5px;" />
                    <asp:GridView ID="gvPersons" runat="server" OnSelectedIndexChanged="gvPersons_SelectedIndexChanged" AutoGenerateColumns="False">
                        <Columns>
                            <asp:ButtonField DataTextField="username" HeaderText="Username" CommandName="Select" />                            
                            <asp:BoundField DataField="creation_date" HeaderText="Creation day" />
                        </Columns>
                    </asp:GridView>
            </div>

            <div style="margin-top:50px;">
                <asp:Button ID="btnDeleteAssignment" runat="server" Text="Delete Assignment" CssClass="w3-btn w3-red" />
                <ajaxToolkit:ConfirmButtonExtender ID="cbtnDeleteAssignment" runat="server" TargetControlID="btnDeleteAssignment" ConfirmText="Are you sure you wish to delete this assignment?"
                 DisplayModalPopupID="mpeConfirmDeleteAssignment" />

                <!-- Start ModalPopupExtender -->
                <ajaxToolkit:ModalPopupExtender ID="mpeConfirmDeleteAssignment" runat="server" PopupControlID="panelConfirmDeleteAssignment" TargetControlID="btnDeleteAssignment"
                    CancelControlID="btnCancelDeleteAssignment" BackgroundCssClass="w3-blue-grey w3-opacity">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="panelConfirmDeleteAssignment" runat="server" CssClass="w3-purple">
                    <div style="width:500px; margin:10px 10px 10px 10px;">

                        <div id="div2" style="margin:auto; width:400px;">
                            <!-- Display confirmnation text -->
                            <asp:Label ID="lbConfirmDeleteAssignment" runat="server" Text="Are you sure you wish to delete this assignment?" />
                        </div>

                        <div style=" margin-left:30%; margin-bottom:10px; margin-top:10px; width:400px;">
                            <asp:Button ID="btnConfirmDeleteAssignment" runat="server" Text="Confirm" OnClick="btnConfirmDeleteAssignment_Click" CssClass="w3-btn" />
                            <asp:Button ID="btnCancelDeleteAssignment" runat="server" Text="Cancel" CssClass="w3-btn" />
                        </div>
                    </div>
                </asp:Panel>
                <!-- End ModalPopupExtender -->
            </div>
        </div>

        <div id="divWorkflow" class="w3-container" style="float:left; width:100%; margin-top:40px;">
            <div>
                <h2>Workflow</h2>
                <h4>Assignment Components</h4>
            </div>

            <div id="divAssignmentComponents" runat="server" class="w3-container" >

            </div>

            <div id="divAddComponent">

                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Button ID="btnShowAddComponentModal" runat="server" Text="Create new component" CssClass="w3-btn" />
 
                <!-- Start ModalPopupExtender -->
                <ajaxToolkit:ModalPopupExtender ID="mpeAddComponent" runat="server" PopupControlID="panelAddComponent" TargetControlID="btnShowAddComponentModal"
                    CancelControlID="btnCloseAddComponentModal" BackgroundCssClass="w3-blue-grey w3-opacity">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="panelAddComponent" runat="server" CssClass="w3-purple">
                    <div style="width:700px; margin:10px 10px 10px 10px;">
                        <div style="float: left; width:50%;">
                            <asp:Label ID="lbAddComponentName" runat="server" Text="Name: "></asp:Label>
                            <asp:TextBox ID="txtAddComponent" runat="server" ValidationGroup="ModalValidation"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvComponentNameRequired" runat="server" ControlToValidate="txtAddComponent" ErrorMessage="Name is required." ForeColor="Red" ValidationGroup="ModalValidation" ToolTip="Password is required.">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Button ID="btnAddComponent" runat="server" Text="Add Component" CssClass="w3-btn" OnClick="btnAddComponent_Click" Style="margin-top:10px;" />
                        </div>

                        <div id="divPanelAssignmentMembers" style="float:left; width:50%;">
                            <!-- List all assignment members on the modal popup -->
                            <asp:CheckBoxList ID="cblPanelAssignmentMembers" runat="server" Style="float:right;"></asp:CheckBoxList>
                        </div>

                        <div style="margin-left: 45%; margin-bottom:10px; float: left; width:100%;">
                            <asp:Button ID="btnCloseAddComponentModal" runat="server" Text="Close" CssClass="w3-btn" />
                        </div>
                    </div>
                </asp:Panel>
                <!-- End ModalPopupExtender -->

            </div>

            <div id="divDisplayComponent">

                <asp:Button ID="btnHiddenModalTarget" runat="server" Style="display:none;" />

                <!-- Start ModalPopupExtender -->
                <ajaxToolkit:ModalPopupExtender ID="mpeShowComponentModal" runat="server" PopupControlID="panelShowComponent" TargetControlID="btnHiddenModalTarget"
                    CancelControlID="btnCloseShowComponent" BackgroundCssClass="w3-blue-grey w3-opacity">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="panelShowComponent" runat="server" CssClass="w3-purple">
                    <div style="width:700px; margin:10px 10px 10px 10px;">
                        <div style="float: left; width:60%;">
                            <asp:Label ID="lbShowComponentName" runat="server" Text="Name: "></asp:Label>
                            <asp:TextBox ID="txtShowComponentName" runat="server" ValidationGroup="ShowModalValidation"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvShowComponentNameRequired" runat="server" ControlToValidate="txtAddComponent" ErrorMessage="Name is required." ForeColor="Red" ValidationGroup="ShowModalValidation" ToolTip="Password is required.">*</asp:RequiredFieldValidator>
                            <asp:CheckBox ID="cbComponentFinished" runat="server" Text="Completed" />
                            <br />
                            <asp:Button ID="btnSaveChanges" runat="server" Text="Save changes" CssClass="w3-btn" OnClick="btnSaveChanges_Click" Style="margin-top:5px;" />
                            <asp:Button ID="btnRemoveAssignmentComponent" runat="server" Text="Delete Component" CssClass="w3-btn" OnClick="btnRemoveAssignmentComponent_Click" Style="margin-left:5px; margin-top:5px;" />
                        </div>

                        <div id="div1" style="float:left; width:40%;">
                            <!-- List all assignment members on the modal popup -->
                            <asp:CheckBoxList ID="cblShowComponentMembers" runat="server" Style="float:right;"></asp:CheckBoxList>
                        </div>

                        <div style="margin-left: 45%; margin-top:50px; margin-bottom:10px; float: left; width:100%;">
                            <asp:Button ID="btnCloseShowComponent" runat="server" Text="Close" CssClass="w3-btn" />
                        </div>
                    </div>
                </asp:Panel>
                <!-- End ModalPopupExtender -->

            </div>

            <!-- Comments section -->
            <div id="divAssignmentComments" style="margin-top:40px";>
                <h3>Comments</h3>

                <asp:UpdatePanel ID="upComments" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div id="divAssignmentCommentMessages" runat="server">

                        </div>

                        <asp:Button ID="btnUpdateComments" runat="server" OnClick="UpdateComments" style="display:none;" />
                    </ContentTemplate> 
                </asp:UpdatePanel>
            </div>
            <!-- Comments section -->

            <!-- SignalR comments section -->
                <div id="divAssignmentCommentWrite" style="margin-top:40px";>
                              
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
                project.client.updateAssignmentComments = function () {
                    // Trigger the onclick event of a hidden button in upComments updatepanel. 
                    document.getElementById('<%=btnUpdateComments.ClientID%>').click();
                };

                project.client.updateAssignmentDescription = function (assignmentdesc) {
                    // Trigger the onclick event of a hidden button in upProjectDescription updatepanel.
                    $('#txtAssignmentsDescription').val(assignmentdesc);
                };

                // Create a function the hub can call to insert the project description upon connecting to the hub
                project.client.insertAssignmentDescription = function (assignmentdesc, archived) {

                    if (archived != "true")
                    {
                        // Insert the projectdescription in to the textarea
                        $('#txtAssignmentsDescription').val(assignmentdesc);
                    }
                    else
                    {
                        $('#btnEditAssignmentDescription').prop("disabled", true);
                    }

                }

                // Set connection parameters
                $.connection.hub.qs = { 'Project': urlParams.get('Project'), 'Assignment': urlParams.get('Assignment') };

                // Start the connection.
                $.connection.hub.start().done(function () {

                    $('#btnSaveComments').click(function () {

                        var comment = $('textarea#txtWriteComments').val();
                        $('textarea#txtWriteComment').val('');

                        project.server.saveAssignmentComment(comment);
                    });

                    $('#btnEditAssignmentDescription').click(function () {

                        $('#txtAssignmentsDescription').removeAttr('readonly');
                        $('#btnEditAssignmentDescription').css({ "display": 'none' });
                        $('#btnSaveAssignmentDescription').css({ "display": 'inline-block' });

                    });

                    $('#btnSaveAssignmentDescription').click(function () {

                        $('#txtAssignmentsDescription').attr("readonly", "readonly");
                        $('#btnSaveAssignmentDescription').css({ "display": 'none' });
                        $('#btnEditAssignmentDescription').css({ "display": 'inline-block' });

                        var assignmentdesc = $('#txtAssignmentsDescription').val();

                        project.server.saveAssignmentDescription(assignmentdesc);

                    });
                });
            });

        </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

