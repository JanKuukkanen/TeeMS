﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Chat.aspx.cs" Inherits="Chat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="CSS/Chat.css" rel="stylesheet" type="text/css" />
    <title>Chat</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="w3-rest w3-container">
        <div style="width:60%;">

            <div id="divGroupChatList" style="border:2px solid black; overflow:auto; height:500px; float:left; width:20%;">
                <div id="divChatGroups" class="w3-container">

                </div>

                <ul id="chatGroups">
                </ul>
            </div>

            <div id="divChatScreen" style="border:2px solid black; overflow:auto; height:500px; float:left; width:60%;">
                <ul id="discussion">
                </ul>
            </div>

            <div id="divChatMemberScreen" style="border:2px solid black; overflow:auto; height:500px; float:left; width:20%;">
                <ul id="chatMembers">
                </ul>
            </div>

            <div class="container" style="float:left; width:100%;">
                <input type="text" id="message" style="width:50%;" />
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

                // Create a function that the hub can call to fill the chat memberlist
                chat.client.fillMemberList = function (jsonmemberlist) {

                    $('#chatMembers').empty();

                    var memberlist = JSON.parse(jsonmemberlist);

                    // Iterate through the memberlist object using key-value pairs
                    $.each(memberlist, function (key, value) {
                        $('#chatMembers').append('<li><strong>' + value + '</strong>' + '</li>');
                    });
                };

                // Create a function that the hub can call to fill the chat message screen when the user joins a new group
                chat.client.fillDiscussion = function (jsonmessages) {

                    $('#discussion').empty();

                    var messagelist = JSON.parse(jsonmessages);

                    $.each(messagelist, function (key, value) {
                        $('#discussion').append('<li><strong>' + value[0]
                        + '</strong>:&nbsp;&nbsp;' + value[2] + '</li>');
                    });

                    // Scroll to the bottom of the divChatScreen
                    var chatscreen = document.getElementById('divChatScreen');
                    setTimeout(function () { chatscreen.scrollTop = chatscreen.scrollHeight; }, 200);
                };

                // Create a function that the hub can call to fill the chat grouplist
                chat.client.fillGroupList = function (jsongrouplist) {
                    $('#chatGroups').empty();

                    var grouplist = JSON.parse(jsongrouplist);

                    // Iterate through the grouplist object using key-value pairs
                    $.each(grouplist, function (key, value) {
                        //Create an input type dynamically.   
                        var element = document.createElement("input");
                        //Assign different attributes to the element. 
                        element.type = "button";
                        element.value = value;
                        element.id = key;
                        element.className = "ChatGrouppButtons";
                        element.onclick = function () {
                            // Call server side methods without the generated proxy
                            chat.invoke('leaveGroup');
                            chat.invoke('joinGroup', value);

                            var elements = document.getElementsByClassName('ChatGrouppButtons');
                            for (var i = 0; i < elements.length; i++) {
                                elements[i].style.backgroundColor = "purple";
                            }

                            document.getElementById(key).style.backgroundColor = "black";
                        };

                        divChatGroups.appendChild(element);
                    })
                };

                // Get the user name and store it to prepend to messages.
                //$('#displayname').val(prompt('Enter your name:', ''));
                // Set initial focus to message input box.  
                $('#message').focus();
                // Start the connection.
                $.connection.hub.start().done(function () {

                    $('#sendmessage').click(function () {
                        // Call the Send method on the hub. 
                        chat.server.send($('#message').val());
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