using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapWebApi.Database;
using ZapWebApi.Models;

namespace ZapWebApi.Hubs
{
    public class ZapWebHub : Hub
    {
        public async Task AtualizarConnectionId(Usuario usuario)
        {
            var usuarioDB = Banco.Usuarios.FirstOrDefault(a => a.Id == usuario.Id);
            if(usuarioDB != null)
            {
                usuarioDB.ConnectionsId.Add(Context.ConnectionId);
            }

            //TODO - Adicionar ConnectionId aos grupos existentes.
        }
        public async Task Cadastrar(Usuario usuario)
        {
            var resultado = Banco.Usuarios.FirstOrDefault(a => a.Email == usuario.Email);
            if(resultado == null)
            {
                usuario.Id = Guid.NewGuid();
                usuario.IsOnline = false;
                usuario.ConnectionsId = new List<string>();
                Banco.Usuarios.Add(usuario);
                await Clients.Caller.SendAsync("ReceberCadastrarUsuario", true, usuario, "Usuário cadastrado com sucesso!");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceberCadastrarUsuario", false, null, "Usuário já existe!");
            }
        }
        public async Task Login(Usuario usuario)
        {
            var resultado = Banco.Usuarios.FirstOrDefault(a => a.Email == usuario.Email && a.Senha == usuario.Senha);
            if (resultado != null)
            {
                resultado.IsOnline = true;
                await Clients.Caller.SendAsync("ReceberLogin", true, resultado, null);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceberLogin", false, null, "E-mail e senha incorretos!");
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
            /* Criar o grupo e armazenar no banco de dados */
            string nome = CriarNomeGrupo(usuarioEmailOne, usuarioEmailTwo);
            Grupo grupo = Banco.Grupos.FirstOrDefault(a => a.Nome == nome);

            if (grupo == null) {
                grupo = new Grupo() { Nome = nome, Usuarios = new List<Usuario>() };

                var usuarioBancoOne = Banco.Usuarios.First(a => a.Email == usuarioEmailOne);
                var usuarioBancoTwo = Banco.Usuarios.First(a => a.Email == usuarioEmailTwo);

                grupo.Usuarios.Add(usuarioBancoOne);
                grupo.Usuarios.Add(usuarioBancoTwo);

                Banco.Grupos.Add(grupo);
            }

            /* SignalR - Criar o Grupo e Adicionar os usuarios ao Grupo */
            foreach(var usuario in grupo.Usuarios)
            {
                foreach(string connectionId in usuario.ConnectionsId)
                {
                    await Groups.AddToGroupAsync(connectionId, grupo.Nome);
                }
            }
        }

        private static string CriarNomeGrupo(string usuarioEmailOne, string usuarioEmailTwo)
        {
            List<string> emails = new List<string> { usuarioEmailOne, usuarioEmailTwo };
            StringBuilder sb = new StringBuilder();
            foreach (var email in emails.OrderBy(a => a))
            {
                sb.Append(email);
            }

            return sb.ToString();
        }

        public async Task EnviarMensagem(string autorEmailMensagem, string grupoName, string mensagemTexto)
        {
            var autor = Banco.Usuarios.First(a => a.Email == autorEmailMensagem);

            /* Verificar se usuario pertence ao grupo */
            var grupo = Banco.Grupos.First(a => a.Nome == grupoName);
            var usuarioExist = grupo.Usuarios.First(a => a.Email == autorEmailMensagem);
            if(usuarioExist == null)
            {
                throw new Exception("Usuário não pertence ao grupo!");
            }

            /* Armazenar mensagem no banco */
            Mensagem mensagem = new Mensagem();
            mensagem.GrupoNome = grupoName;
            mensagem.Texto = mensagemTexto;
            mensagem.Usuario = Banco.Usuarios.First(a => a.Email == autorEmailMensagem);
            mensagem.Criado = DateTime.Now;

            Banco.Mensagens.Add(mensagem);

            /* Enviar Mensagem via SignalR */
            await Clients.Group(grupoName).SendAsync("ReceberMensagem", mensagem);
        }
    }
}
