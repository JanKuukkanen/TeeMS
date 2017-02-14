﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Chat.aspx.cs" Inherits="Chat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Chat</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-rest w3-container">
        <div style="width:60%;">
            <div id="divChatScreen" style="border:2px solid black; overflow:auto; height:500px; float:left; width:70%;">
                <ul id="discussion">
                </ul>
            </div>

            <div id="divChatMemberScreen" style="border:2px solid black; overflow:auto; height:500px; float:left; width:30%;">
                <ul id="chatMembers">
                </ul>
            </div>

            <div class="container" style="float:left; width:100%;">
                <input type="text" id="message" />
                <input type="button" id="sendmessage" value="Send" class="w3-btn" />
                <input type="hidden" id="displayname" />
            </div>

            <div>
                <asp:Label ID="lbMessages" runat="server" />
            </div>
        </div>
    </div>

        <!--Script references. -->
        <!--Reference the jQuery library. -->
        <script src="Scripts/jquery-1.6.4.min.js"></script>
        <!--Reference the SignalR library. -->
        <script src="Scripts/jquery.signalR-2.2.1.min.js"></script>
        <!--Reference the autogenerated SignalR hub script. -->

        <script src="signalr/hubs" type="text/javascript"></script>

        <!--Add script to update the page and send messages.-->
        <script type="text/javascript">

            $(function () {
                // Declare a proxy to reference the hub.
                var chat = $.connection.teeMsHub;
                // Create a function that the hub can call to broadcast messages.
                chat.client.broadcastMessage = function (name, message) {
                    // Html encode display name and message. 
                    var encodedName = $('<div />').text(name).html();
                    var encodedMsg = $('<div />').text(message).html();
                    // Add the message to the page. 
                    $('#discussion').append('<li><strong>' + encodedName
                        + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
                };
                // Get the user name and store it to prepend to messages.
                //$('#displayname').val(prompt('Enter your name:', ''));
                // Set initial focus to message input box.  
                $('#message').focus();
                // Start the connection.
                $.connection.hub.start().done(function () {

                    // Call the connect method on the hub
                    var chatmembers = chat.server.connect();

                    $('#sendmessage').click(function () {
                        // Call the Send method on the hub. 
                        chat.server.send($('#displayname').val(), $('#message').val());
                        // Clear text box and reset focus for next comment. 
                        $('#message').val('').focus();

                        // Scroll to the bottom of the divChatScreen
                        var chatscreen = document.getElementById('divChatScreen');
                        setTimeout(function () { chatscreen.scrollTop = chatscreen.scrollHeight; }, 200);
                    });
                });
            });
        </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>