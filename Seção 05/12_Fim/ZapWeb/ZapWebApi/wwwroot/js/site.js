/* Conexão e Reconexão */
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

/* Login */
var btnAcessar = document.getElementById("btnAcessar");
btnAcessar.addEventListener("click", function () {
    var email = document.getElementById("email").value;
    var senha = document.getElementById("senha").value;

    
    var usuario = { Email: email, Senha: senha };

    console.info(usuario);
    connection.invoke("Login", usuario).then(function () {
        console.info("Solicitação realizada com sucesso!");
    }).catch(function (err) {
        console.error(err.toString());
    });
});

connection.on("ReceberLogin", function (loginSucesso, usuario, msg) {
    var mensagem = document.getElementById("mensagem");

    if (loginSucesso == true) {
        mensagem.innerText = "Login realizado com sucesso!";
        console.info(usuario);

        sessionStorage.setItem("Logado", JSON.stringify(usuario));
    } else {
        mensagem.innerText = "ERRO: " + msg;
    }
});