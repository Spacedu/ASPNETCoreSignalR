var connection = new signalR.HubConnectionBuilder().withUrl("/ZapWebHub").buid();
start();

function start() {
    connection.start()
        .then(function () { console.info("Connected!"); })
        .catch(function (err) {
            console.error(err.toString());
            setTimeout(() => start(), 5000);
        });
}

connection.onclose(async () => {
    await start();
})