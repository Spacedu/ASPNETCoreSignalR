/* Conexão e Reconexão */
var connection = new signalR.HubConnectionBuilder().withUrl("/ZapWebHub").build();
start();

function start() {
    connection.start()
        .then(function () {
            AtualizarConnectionId();
            console.info("Connected!");
        })
        .catch(function (err) {
            console.error(err.toString());
            setTimeout(() => start(), 5000);
        });
}

connection.onclose(async () => {
    await start();
});

/* Atualizar Connection Id */
function AtualizarConnectionId() {
    if (sessionStorage.getItem("Logado") != null) {
        connection.invoke("AtualizarConnectionId", JSON.parse(sessionStorage.getItem("Logado")));
    }
}

/* Login */
var btnAcessar = document.getElementById("btnAcessar");
if (btnAcessar != null) {
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
}

connection.on("ReceberLogin", function (loginSucesso, usuario, msg) {
    var mensagem = document.getElementById("mensagem");

    if (loginSucesso == true) {
        mensagem.innerText = "Login realizado com sucesso!";
        console.info(usuario);

        sessionStorage.setItem("Logado", JSON.stringify(usuario));

        //TODO - Redirecionar para tela de conversação
    } else {
        mensagem.innerText = "ERRO: " + msg;
    }
});


/* Cadastrar */
var btnCadastrar = document.getElementById("btnCadastrar");
if (btnCadastrar != null) {
    btnCadastrar.addEventListener("click", function () {
        var nome = document.getElementById("nome").value;
        var email = document.getElementById("email").value;
        var senha = document.getElementById("senha").value;

        var usuario = { Nome: nome, Email: email, Senha: senha };

        connection.invoke("Cadastrar", usuario).catch(function (err) {
            console.error("ERRO:" + err.toString());
        });
    });
}

connection.on("ReceberCadastrarUsuario", function (cadastradoSucesso, usuario, msg) {
    var mensagem = document.getElementById("mensagem");

    if (cadastradoSucesso == true) {
        mensagem.innerText = msg;
        console.info(usuario);

        sessionStorage.setItem("Logado", JSON.stringify(usuario));
        //TODO - Redirecionar para tela de conversação
    } else {
        mensagem.innerText = "ERRO: " + msg;
    }

});