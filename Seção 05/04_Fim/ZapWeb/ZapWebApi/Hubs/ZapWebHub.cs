using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapWebApi.Database;
using ZapWebApi.Models;

namespace ZapWebApi.Hubs
{
    public class ZapWebHub : Hub
    {

        public async Task Cadastrar(Usuario usuario)
        {
            var resultado = Banco.Usuarios.FirstOrDefault(a => a.Email == usuario.Email && a.Senha == usuario.Senha);
            if(resultado == null)
            {
                usuario.Id = Guid.NewGuid();
                usuario.IsOnline = false;
                usuario.ConnectionsId = new List<string>();
                Banco.Usuarios.Add(usuario);
                await Clients.Caller.SendAsync("CadastrarUsuario", true, usuario, "Usuário cadastrado com sucesso!");
            }
            else
            {
                await Clients.Caller.SendAsync("CadastrarUsuario", false, null, "Usuário já existe!");
            }
        }
        public async Task Login(Usuario usuario)
        {
            var resultado = Banco.Usuarios.FirstOrDefault(a => a.Email == usuario.Email && a.Senha == usuario.Senha);
            if (resultado != null)
            {
                resultado.IsOnline = true;
                resultado.ConnectionsId.Add(Context.ConnectionId);
                await Clients.Caller.SendAsync("Login", true, resultado, null);
            }
            else
            {
                await Clients.Caller.SendAsync("Login", false, null, "E-mail e senha incorretos!");
            }
        }

        public async Task Logout(Usuario usuario)
        {
            var resultado = Banco.Usuarios.FirstOrDefault(a => a.Id == usuario.Id);
            if (resultado != null)
            {
                resultado.IsOnline = false;
                resultado.ConnectionsId.Remove(Context.ConnectionId);
                await Clients.Caller.SendAsync("Logout", true);
            }
            else
            {
                await Clients.Caller.SendAsync("Logout", false);
            }
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
