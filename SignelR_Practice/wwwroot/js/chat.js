
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (sender, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = sender + " : " + msg;
    var div = document.createElement("div");
    div.textContent = encodedMsg;
    document.getElementById("recieverMessage").appendChild(div);
});
connection.on("SendMessage", function (sender, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = sender + " : " + msg;
    var div = document.createElement("div");
    div.textContent = encodedMsg;
    document.getElementById("senderMessage").appendChild(div);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var receiverId = document.getElementById("recieverInput").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessageToReceiver", receiverId, message).catch(function (err) {
       
        return console.error(err.toString());
    });
    event.preventDefault();
});
