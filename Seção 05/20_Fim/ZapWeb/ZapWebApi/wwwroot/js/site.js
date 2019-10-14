/* Conexão e Reconexão */
var connection = new signalR.HubConnectionBuilder().withUrl("/ZapWebHub").build();
start();

function start() {
    connection.start()
        .then(function () {
            AtualizarConnectionId();
            ObterUsuarios();
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

        window.location.href = "/Home/Conversacao";
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

        window.location.href = "/Home/Conversacao";
    } else {
        mensagem.innerText = "ERRO: " + msg;
    }
});

function ObterUsuarios() {
    var conversacao = document.getElementById("conversacao");
    if (conversacao != null) {
        connection.invoke("ObterUsuarios");
    }
}

connection.on("ReceberUsuarios", function (usuarios) {
    var logado = JSON.parse(sessionStorage.getItem("Logado"));
    var container = document.getElementById("lista-usuario");

    var html = "";

    for (i = 0; i < usuarios.length; i++) {
        if (logado.email != usuarios[i].email) {
            html += "<span>" + usuarios[i].nome + " - " + usuarios[i].email + " (" + (usuarios[i].isOnline ? "online" : "offline") + ") </span>";
        }
    }
    container.innerHTML = html;

    var spans = container.getElementsByTagName("span");

    for (i = 0; i < spans.length; i++) {
        spans[i].addEventListener("click", function (event) {
            var span = event.target || event.srcElement;
            
            var usuarioEmailOne = logado.email;
            var usuarioEmailTwo = span.innerText.substring(span.innerText.indexOf("-") + 2, span.innerText.indexOf("(") - 1);

            connection.invoke("CriarOuAbrirGrupo", usuarioEmailOne, usuarioEmailTwo);

            
        });
    }
});

connection.on("AbrirGrupo", function (nomeGrupo, mensagens) {
    var logado = JSON.parse(sessionStorage.getItem("Logado"));
    var container = document.getElementById("container-messages");
    container.innerHTML = "";
    var mensagensHtml = "";

    for (i = 0; i < mensagens.length; i++) {
        mensagensHtml += '<div class="message message-' + ((logado.id == mensagens[i].usuario.id) ? "right" : "left" ) + '"><div class="message-head" ><img src="/imagem/chat.png" /> ' + mensagens[i].usuario.nome + '</div><div class="message-message">' + mensagens[i].texto + '</div></div>';
    }

    container.innerHTML = mensagensHtml;
});