$(function () {
    // Declare a proxy to reference the hub.
    var project = $.connection.projectHub;
    // Create a function that the hub can call to broadcast messages.
    project.client.updateComments = function () {
        // Trigger the onclick event of a hidden button in upComments updatepanel. 
        document.getElementById('<%=btnUpdateComments.ClientID %>').click();
    };

    // Start the connection.
    $.connection.hub.start().done(function () {

        $('<%=btnSaveComment %>').click(function () {

            // Call the BroadcastUpdateComments method on the hub. 
            project.server.broadcastUpdateComments();
            
        });

    });
});