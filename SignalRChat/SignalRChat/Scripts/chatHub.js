var chat = $.connection.chatHub;
chat.client.broadcastMessage = function (message) {
    $('#chat').prepend('<tr><td>' + message.Time + '</td><td>' + message.Author + '</td><td>' + message.Text + '</td></tr>');
}

$.connection.hub.start().done(
    function () {
        $("#send-btn").click(
            function () {
                if ($("form").valid()) {
                    var message = { author: $('#Author').val(), text: $('#Text').val() };
                    chat.server.broadcastMessage(message);
                    $('#Text').val('');
                }
            });
    });