var connection = new signalR.HubConnectionBuilder().withUrl("/ZapWebHub").build();
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
});

var btnAcessar = document.getElementById("btnAcessar");
btnAcessar.addEventListener("click", function () {
    var email = document.getElementById("email").value;
    var senha = document.getElementById("senha").value;

    var mensagem = document.getElementById("mensagem");
    var usuario = { Email: email, Senha: senha };

    console.info(usuario);
    connection.invoke("Login", usuario).then(function () {
        console.info("Solicitação realizada com sucesso!");
    }).catch(function (err) {
        console.error(err.toString());
    });
});


