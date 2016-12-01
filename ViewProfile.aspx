<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewProfile.aspx.cs" Inherits="ViewProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Profile</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-container w3-twothird">
        <h1>Profile</h1>
        <hr />

        <div class="w3-container">
            <table>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbUserNameTitle" runat="server" AssociatedControlID="txtUserName" Text="User Name:" />
                    </td>
                    <td>
                        <asp:Label ID="lbUsernameInsert" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbEmailTitle" runat="server" AssociatedControlID="txtEmail" Text="E-mail:" />
                    </td>
                    <td>
                        <asp:Label ID="lbEmailInsert" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbFirstNameTitle" runat="server" AssociatedControlID="txtFirstName" Text="First Name:" />
                    </td>
                    <td>
                        <asp:Label ID="lbFirstNameInsert" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbLastNameTitle" runat="server" AssociatedControlID="txtLastName" Text="Last Name:" />
                    </td>
                    <td>
                        <asp:Label ID="lbLastNameInsert" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <div class="w3-container" style="margin-top:40px;">
            <table>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbUserName" runat="server" AssociatedControlID="txtUserName" Text="User Name:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbEmail" runat="server" AssociatedControlID="txtEmail" Text="E-mail:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexEmail" runat="server" ErrorMessage="Enter a valid email-adress!" ControlToValidate="txtEmail" ValidationGroup="UserProfile" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbFirstName" runat="server" AssociatedControlID="txtFirstName" Text="First Name:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexFirstName" runat="server" ErrorMessage="Your name contains invalid characters!" ForeColor="Red" ControlToValidate="txtFirstName" ValidationExpression="^[A-Za-z]+(((\'|\-|\.)?([A-Za-z])+))?$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbLastName" runat="server" AssociatedControlID="txtLastName" Text="Last Name:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexLastName" runat="server" ErrorMessage="Your name contains invalid characters!" ForeColor="Red" ControlToValidate="txtLastName" ValidationExpression="^[A-Za-z]+(((\'|\-|\.)?([A-Za-z])+))?$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile" CssClass="w3-btn" ValidationGroup="UserProfile" OnClick="btnEditProfile_Click" />
                    </td>
                </tr>
            </table>

            <table style="margin-top:40px;">
                <tr>
                    <td align="right">
                        <asp:Label ID="lbPassword" runat="server" AssociatedControlID="txtPassword" Text="Password:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." ForeColor="Red" ToolTip="Password is required." ValidationGroup="UserPassword">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" Text="Confirm Password:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm Password is required." ForeColor="Red" ToolTip="Confirm Password is required." ValidationGroup="UserPassword">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="color:Red;">
                        <asp:Literal ID="ltErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnEditPassword" runat="server" Text="Edit Password" CssClass="w3-btn" ValidationGroup="UserPassword" OnClick="btnEditPassword_Click" />
                    </td>
                </tr>
            </table>
        </div>

        <div>
            <asp:Label ID="lbErrorMessages" runat="server" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

