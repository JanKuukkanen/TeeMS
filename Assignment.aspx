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
                <h1 id="h1AssignmentName" runat="server">Undefined Assignment</h1>
            </div>

            <div>
                <h2>Description</h2>
                <asp:TextBox ID="txtAssignmentDescription" runat="server" TextMode="MultiLine" Height="100px" Width="450px" ReadOnly="true" ></asp:TextBox>
                <br />
                <asp:Button ID="btnEditAssignmentDescription" runat="server" Text="Edit description" CssClass="w3-btn" OnClick="btnEditAssignmentDescription_Click" />
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
 
                <!-- ModalPopupExtender -->
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
                            <asp:Button ID="btnAddComponent" runat="server" Text="Add Component" OnClick="btnAddComponent_Click" Style="margin-top:10px;" />
                        </div>

                        <div id="divPanelAssignmentMembers" style="float:left; width:50%;">
                            <!-- List all assignment members on the modal popup -->
                            <asp:CheckBoxList ID="cblPanelAssignmentMembers" runat="server" Style="float:right;"></asp:CheckBoxList>
                        </div>

                        <div style="margin-left: 45%; margin-bottom:10px; float: left; width:100%;">
                            <asp:Button ID="btnCloseAddComponentModal" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
                <!-- ModalPopupExtender -->

            </div>

            <div id="divDisplayComponent">

                <asp:Button ID="btnHiddenModalTarget" runat="server" Style="display:none;" />

                <!-- ModalPopupExtender -->
                <ajaxToolkit:ModalPopupExtender ID="mpeShowComponentModal" runat="server" PopupControlID="panelShowComponent" TargetControlID="btnHiddenModalTarget"
                    CancelControlID="btnCloseShowComponent" BackgroundCssClass="w3-blue-grey w3-opacity">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="panelShowComponent" runat="server" CssClass="w3-purple">
                    <div style="width:700px; margin:10px 10px 10px 10px;">
                        <div style="float: left; width:50%;">
                            <asp:Label ID="lbShowComponentName" runat="server" Text="Name: "></asp:Label>
                            <asp:TextBox ID="txtShowComponentName" runat="server" ValidationGroup="ShowModalValidation"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvShowComponentNameRequired" runat="server" ControlToValidate="txtAddComponent" ErrorMessage="Name is required." ForeColor="Red" ValidationGroup="ShowModalValidation" ToolTip="Password is required.">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Button ID="btnSaveChanges" runat="server" Text="Save changes" OnClick="btnSaveChanges_Click" Style="margin-top:10px;" />
                        </div>

                        <div id="div1" style="float:left; width:50%;">
                            <!-- List all assignment members on the modal popup -->
                            <asp:CheckBoxList ID="cblShowComponentMembers" runat="server" Style="float:right;"></asp:CheckBoxList>
                        </div>

                        <div style="margin-left: 45%; margin-bottom:10px; float: left; width:100%;">
                            <asp:Button ID="btnCloseShowComponent" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
                <!-- ModalPopupExtender -->

            </div>

            <div>
                <asp:Label ID="lbMessages" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

