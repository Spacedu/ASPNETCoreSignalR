/* Conexão e Reconexão com o SignalR - Hub */
var connection = new signalR.HubConnectionBuilder().withUrl("/ZapWebHub").build();

function ConnectionStart() {
    connection.start().then(function () {
        console.info("Connected!");
    }).catch(function (err) {
        console.error(err.toString());
        setTimeout(ConnectionStart(), 5000);
    });
}

connection.onclose(async () => { await ConnectionStart(); });