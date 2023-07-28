
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveNotification", function (message, isleft) {
    document.getElementById("messageInput").value = "";
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;

    var table = document.querySelector("table.table");
    var tbody = table.querySelector("tbody");

    // Create a new row for the message
    var newRow = document.createElement("tr");

    // Create the cells for the row
    var messageCell = document.createElement("td");
    var dateCell = document.createElement("td");

    if (isleft === 1) {
        dateCell.textContent = encodedMsg;
        messageCell.textContent = "";
    } else {
        dateCell.textContent = "";
        messageCell.textContent = encodedMsg;
    }

    newRow.appendChild(dateCell);   
    newRow.appendChild(messageCell);

    // Insert the new row at the end of the table
    tbody.appendChild(newRow);

    window.scrollTo(0, document.body.scrollHeight);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var receiverId = document.getElementById("receiverId").innerHTML;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendNotificationToReceiver", receiverId, true, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
