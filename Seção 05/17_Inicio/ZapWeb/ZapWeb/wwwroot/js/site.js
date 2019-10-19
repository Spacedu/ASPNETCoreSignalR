/* Conexão e Reconexão com o SignalR - Hub */
var connection = new signalR.HubConnectionBuilder().withUrl("/ZapWebHub").build();

function ConnectionStart() {
    connection.start().then(function () {
        HabilitarLogin();
        HabilitarCadastro();
        console.info("Connected!");
    }).catch(function (err) {
        console.error(err.toString());
        setTimeout(ConnectionStart(), 5000);
    });
}

connection.onclose(async () => { await ConnectionStart(); });

/*  */
function HabilitarCadastro() {
    var formCadastro = document.getElementById("form-cadastro");
    if (formCadastro != null) {
        var btnCadastrar = document.getElementById("btnCadastrar");

        btnCadastrar.addEventListener("click", function () {
            var nome = document.getElementById("nome").value;
            var email = document.getElementById("email").value;
            var senha = document.getElementById("senha").value;

            var usuario = { Nome: nome, Email: email, Senha: senha };

            connection.invoke("Cadastrar", usuario);
        });
    }


    connection.on("ReceberCadastro", function (sucesso, usuario, msg) {
        var mensagem = document.getElementById("mensagem");
        if (sucesso) {
            console.info(usuario);


            document.getElementById("nome").value = "";
            document.getElementById("email").value = "";
            document.getElementById("senha").value = "";
        }

        mensagem.innerText = msg;
    });
}

function HabilitarLogin() {
    var formLogin = document.getElementById("form-login");
    if (formLogin != null) {
        if (GetUsuarioLogado() != null) {
            window.location.href = "/Home/Conversacao";
        }

        var btnAcessar = document.getElementById("btnAcessar");
        btnAcessar.addEventListener("click", function () {
            var email = document.getElementById("email").value;
            var senha = document.getElementById("senha").value;

            var usuario = { Email: email, Senha: senha };

            connection.invoke("Login", usuario);
        });
    }

    connection.on("ReceberLogin", function (sucesso, usuario, msg) {
        if (sucesso) {
            SetUsuarioLogado(usuario);
            window.location.href = "/Home/Conversacao";
        } else {
            var mensagem = document.getElementById("mensagem");
            mensagem.innerText = msg;
        }
    });
}




var telaConversacao = document.getElementById("tela-conversacao");
if (telaConversacao != null) {
    if (GetUsuarioLogado() == null) {
        window.location.href = "/Home/Login";
    }

    connection.invoke("AtualizarConnectionIdDoUsuario", GetUsuarioLogado());

    var btnSair = document.getElementById("btnSair");
    btnSair.addEventListener("click", function () {
        connection.invoke("DelConnectionIdDoUsuario", GetUsuarioLogado()).then(function () {
            DelUsuarioLogado();
            window.location.href = "/Home/Login";
        });
        
    });
}



function GetUsuarioLogado() {
    return JSON.parse(sessionStorage.getItem("Logado"));
}
function SetUsuarioLogado(usuario) {
    sessionStorage.setItem("Logado", JSON.stringify(usuario));
}
function DelUsuarioLogado() {
    sessionStorage.removeItem("Logado");
}

ConnectionStart();