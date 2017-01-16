<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Register</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

    <div class="w3-rest">

        <div style="margin-right:40%">
            <h3 class="w3-gray" style="padding-left:20%; margin-right:auto;">Sign up for your new account</h3>

            <table>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbUserName" runat="server" AssociatedControlID="txtUserName" Text="User Name:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName" ErrorMessage="User Name is required." ForeColor="Red" ToolTip="User Name is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbPassword" runat="server" AssociatedControlID="txtPassword" Text="Password:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." ForeColor="Red" ToolTip="Password is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" Text="Confirm Password:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm Password is required." ForeColor="Red" ToolTip="Confirm Password is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbEmail" runat="server" AssociatedControlID="txtEmail" Text="E-mail:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtEmail" ErrorMessage="E-mail is required." ForeColor="Red" ToolTip="E-mail is required.">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexEmail" runat="server" ErrorMessage="Enter a valid email-adress!" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
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
                    <td align="center" colspan="2">
                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="color:Red;">
                        <asp:Literal ID="ltErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>

        <div class="w3-container">
            <asp:Button ID="btnRegisterUser" runat="server" Text="Register" OnClick="btnRegisterUser_Click" /> <br />
            <asp:Label ID="lbAlreadyRegistered" runat="server" Text="Already Registered?" />
            <a href="Login.aspx">Sign in</a>
        </div> <br />
        <div class="w3-container">
        <asp:Label ID="lbMessages" runat="server" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

