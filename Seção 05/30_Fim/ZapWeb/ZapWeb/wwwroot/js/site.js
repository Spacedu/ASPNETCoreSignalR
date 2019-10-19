/* Conexão e Reconexão com o SignalR - Hub */
var connection = new signalR.HubConnectionBuilder().withUrl("/ZapWebHub").build();
var nomeGrupo = "";
function ConnectionStart() {
    connection.start().then(function () {
        HabilitarLogin();
        HabilitarCadastro();
        HabilitarConversacao();
        console.info("Connected!");
    }).catch(function (err) {
        if (connection.state == 0) {
            console.error(err.toString());
            setTimeout(ConnectionStart, 5000);
        }        
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
}

function HabilitarConversacao() {
    var telaConversacao = document.getElementById("tela-conversacao");
    if (telaConversacao != null) {
        MonitorarConnectionID();
        MonitorarListaUsuarios();
        EnviarReceberMensagem();
        AbrirGrupo();
    }
}
function AbrirGrupo() {
    connection.on("AbrirGrupo", function (nomeDoGrupo, mensagens) {
        nomeGrupo = nomeDoGrupo;
        console.info(nomeGrupo);

        var container = document.querySelector(".container-messages");
        container.innerHTML = "";


        var mensagemHTML = "";
        for (i = 0; i < mensagens.length; i++) {
            console.info(mensagens[i].usuario.id);
            mensagemHTML += '<div class="message message-' + (mensagens[i].usuario.id == GetUsuarioLogado().id ? "right" : "left") + '"><div class="message-head"><img src="/imagem/chat.png" /> ' + mensagens[i].usuario.nome + '</div><div class="message-message">' + mensagens[i].texto + '</div></div>'
        }
        container.innerHTML += mensagemHTML;

        document.querySelector(".container-button").style.display = "flex";
    });
}
function EnviarReceberMensagem() {
    var btnEnviar = document.getElementById("btnEnviar");
    btnEnviar.addEventListener("click", function () {
        var mensagem = document.getElementById("mensagem").value;
        var usuario = GetUsuarioLogado();


        connection.invoke("EnviarMensagem", usuario, mensagem, nomeGrupo);
    });


    connection.on("ReceberMensagem", function (mensagem, nomeDoGrupo) {

        if (nomeGrupo == nomeDoGrupo) {
            var container = document.querySelector(".container-messages");
            var mensagemHTML = '<div class="message message-' + (mensagem.usuario.id == GetUsuarioLogado().id ? "right" : "left") + '"><div class="message-head"><img src="/imagem/chat.png" />' + mensagem.usuario.nome + '</div><div class="message-message">' + mensagem.texto + '</div></div>'

            container.innerHTML += mensagemHTML;
        }
    });
}

function MonitorarListaUsuarios() {
    connection.invoke("ObterListaUsuarios");

    connection.on("ReceberListaUsuarios", function (usuarios) {
        var html = "";
        for (i = 0; i < usuarios.length; i++) {
            if (usuarios[i].id != GetUsuarioLogado().id) {
                html += '<div class="container-user-item"><img src = "/imagem/logo.png" style = "width: 20%;" /><div><span>' + usuarios[i].nome.split(" ")[0] + ' (' + (usuarios[i].isOnline ? "online" : "offline") + ')</span><br /><span class="email">' + usuarios[i].email + '</span></div></div>';
            }
        }
        document.getElementById("users").innerHTML = html;

        var container = document.getElementById("users").querySelectorAll(".container-user-item");
        for (i = 0; i < container.length; i++) {
            container[i].addEventListener("click", function (event) {
                var componente = event.target || event.srcElement;

                var emailUserUm = GetUsuarioLogado().email;
                var emailUserDois = componente.parentElement.querySelector(".email").innerText;

                connection.invoke("CriarOuAbrirGrupo", emailUserUm, emailUserDois);
            });
        }
    });
}
function MonitorarConnectionID() {
    var telaConversacao = document.getElementById("tela-conversacao");
    if (telaConversacao != null) {
        connection.invoke("AddConnectionIdDoUsuario", GetUsuarioLogado());

        var btnSair = document.getElementById("btnSair");
        btnSair.addEventListener("click", function () {
            connection.invoke("Logout", GetUsuarioLogado()).then(function () {
                DelUsuarioLogado();
                window.location.href = "/Home/Login";
            });
        });
    }
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