using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapWebApi.Models;

namespace ZapWebApi.Hubs
{
    public class ZapWebHub : Hub
    {
        public async Task Login(Usuario usuario)
        {
            //TODO - Setar o Usuario Online(IsOnline = true);
            //Clients.Caller.SendAsync("");
        }

        public async Task Cadastrar(Usuario usuario)
        {
            //Clients.Caller.SendAsync("")
        }

        public async Task Logout()
        {
            //TODO - Setar o Usuario Offline(IsOnline = false);
            //Clients.Caller.SendAsync("");
        }

        public async Task CriarGrupo(string usuarioEmailOne, string usuarioEmailTwo)
        {
            //TODO - Verificação se grupo já existe
        }

        public async Task EnviarMensagem(string autorEmailMensagem, string grupoName, string mensagemTexto)
        {
            //TODO - Verificar se o autor está no grupoName.
            //TODO - Data de criação da mensagem deve ser atribuida automaticamente.
        }
    }
}
