using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapWeb.Database;
using ZapWeb.Models;

namespace ZapWeb.Hubs
{
    public class ZapWebHub : Hub
    {
        private BancoContext _banco;
        public ZapWebHub(BancoContext banco)
        {
            _banco = banco;
        }
        public async Task Cadastrar(Usuario usuario)
        {
            bool IsExistUser = _banco.Usuarios.Where(a => a.Email == usuario.Email).Count() > 0;

            if (IsExistUser)
            {
                await Clients.Caller.SendAsync("ReceberCadastro", false, null, "E-mail já cadastrado!");
            }
            else
            {
                _banco.Usuarios.Add(usuario);
                _banco.SaveChanges();

                await Clients.Caller.SendAsync("ReceberCadastro", true, usuario, "Usuário cadastrado com sucesso!");
            }
        }

        public async Task Login(Usuario usuario)
        {
            var usuarioDB = _banco.Usuarios.FirstOrDefault(a => a.Email == usuario.Email && a.Senha == usuario.Senha);

            if(usuarioDB == null)
            {
                await Clients.Caller.SendAsync("ReceberLogin", false, null, "E-mail ou senha errado!");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceberLogin", true, usuarioDB, null);
                
                usuarioDB.IsOnline = true;
                _banco.Usuarios.Update(usuarioDB);
                _banco.SaveChanges();


                await NotificarMudancaNaListaUsuarios();
            }
        }

        public async Task Logout(Usuario usuario)
        {
            var usuarioDB = _banco.Usuarios.Find(usuario.Id);
            usuarioDB.IsOnline = false;
            _banco.Usuarios.Update(usuarioDB);
            _banco.SaveChanges();
            
            await DelConnectionIdDoUsuario(usuarioDB);

            await NotificarMudancaNaListaUsuarios();

        }

        public async Task AddConnectionIdDoUsuario(Usuario usuario)
        {
            var ConnectionIdCurrent = Context.ConnectionId;
            List<string> connectionsId = null;

            Usuario usuarioDB = _banco.Usuarios.Find(usuario.Id);
            if(usuarioDB.ConnectionId == null)
            {
                connectionsId = new List<string>();
                connectionsId.Add(ConnectionIdCurrent);
            }
            else
            {
                connectionsId = JsonConvert.DeserializeObject<List<string>>(usuarioDB.ConnectionId);
                if(!connectionsId.Contains(ConnectionIdCurrent))
                {
                    connectionsId.Add(ConnectionIdCurrent);
                }
            }

            usuarioDB.IsOnline = true;
            usuarioDB.ConnectionId = JsonConvert.SerializeObject(connectionsId);
            _banco.Usuarios.Update(usuarioDB);
            _banco.SaveChanges();
            await NotificarMudancaNaListaUsuarios();

            //Adicionar ConnectionsId aos grupos do SignalR
            var grupos = _banco.Grupos.Where(a => a.Usuarios.Contains(usuarioDB.Email));
            foreach (var connectionId in connectionsId)
            {
                foreach (var grupo in grupos)
                {
                    await Groups.AddToGroupAsync(connectionId, grupo.Nome);
                }
            }
        }

        public async Task DelConnectionIdDoUsuario(Usuario usuario)
        {
            Usuario usuarioDB = _banco.Usuarios.Find(usuario.Id);
            List<string> connectionsId = null;
            if (usuarioDB.ConnectionId.Length > 0)
            {
                var ConnectionIdCurrent = Context.ConnectionId;

                connectionsId = JsonConvert.DeserializeObject<List<string>>(usuarioDB.ConnectionId);
                if (connectionsId.Contains(ConnectionIdCurrent))
                {
                    connectionsId.Remove(ConnectionIdCurrent);
                }
                usuarioDB.ConnectionId = JsonConvert.SerializeObject(connectionsId);

                if (connectionsId.Count <= 0)
                {
                    usuarioDB.IsOnline = false;
                    
                }

                _banco.Usuarios.Update(usuarioDB);
                _banco.SaveChanges();
                await NotificarMudancaNaListaUsuarios();


                //Remoção da ConnectionId dos Grupos de conversa desse usuário no SignalR.
                var grupos = _banco.Grupos.Where(a => a.Usuarios.Contains(usuarioDB.Email));
                foreach (var connectionId in connectionsId)
                {
                    foreach (var grupo in grupos)
                    {
                        await Groups.RemoveFromGroupAsync(connectionId, grupo.Nome);
                    }
                }
            }

            
        }

        public async Task ObterListaUsuarios()
        {
            var usuarios = _banco.Usuarios.ToList();
            await Clients.Caller.SendAsync("ReceberListaUsuarios", usuarios);
        }
        public async Task NotificarMudancaNaListaUsuarios()
        {
            var usuarios = _banco.Usuarios.ToList();
            await Clients.All.SendAsync("ReceberListaUsuarios", usuarios);
        }

        /*
         SignalR - 
         elias@gmail.com - aline@gmail.com = elias@gmail.com-aline@gmail.com
         aline@gmail.com - elias@gmail.com = aline@gmail.com-elias@gmail.com 

        */
        public async Task CriarOuAbrirGrupo(string emailUserUm, string emailUserDois)
        {
            string nomeGrupo = CriarNomeGrupo(emailUserUm, emailUserDois);

            Grupo grupo = _banco.Grupos.FirstOrDefault(a => a.Nome == nomeGrupo);
            if(grupo == null)
            {
                grupo = new Grupo();
                grupo.Nome = nomeGrupo;
                grupo.Usuarios = JsonConvert.SerializeObject(new List<string>()
                {
                    emailUserUm,
                    emailUserDois
                });

                _banco.Grupos.Add(grupo);
                _banco.SaveChanges();
            }

            //Adicionou as Connections Ids para o Grupo no SignalR
            List<string> emails = JsonConvert.DeserializeObject<List<string>>( grupo.Usuarios );
            List<Usuario> usuarios = new List<Usuario>() {
                _banco.Usuarios.First(a => a.Email == emails[0]),
                _banco.Usuarios.First(a => a.Email == emails[1])
            };

            foreach(var usuario in usuarios)
            {
                var connectionsId = JsonConvert.DeserializeObject<List<string>>(usuario.ConnectionId);
                foreach (var connectionId in connectionsId)
                {
                    await Groups.AddToGroupAsync(connectionId, nomeGrupo);
                }
            }

            var mensagens = _banco.Mensagens.Where(a => a.NomeGrupo == nomeGrupo).OrderBy(a=>a.DataCriacao).ToList();
            for(int i=0; i < mensagens.Count; i++)
            {
                mensagens[i].Usuario = JsonConvert.DeserializeObject<Usuario>(mensagens[i].UsuarioJson);
            }
            await Clients.Caller.SendAsync("AbrirGrupo", nomeGrupo, mensagens);
        }
        public async Task EnviarMensagem(Usuario usuario, string msg, string nomeGrupo)
        {
            Grupo grupo = _banco.Grupos.FirstOrDefault(a=>a.Nome == nomeGrupo);
            
            if (!grupo.Usuarios.Contains(usuario.Email))
            {
                throw new Exception("Usuário não pertence ao grupo!");
            }

            Mensagem mensagem = new Mensagem();
            mensagem.NomeGrupo = nomeGrupo;
            mensagem.Texto = msg;
            mensagem.UsuarioId = usuario.Id;
            mensagem.UsuarioJson = JsonConvert.SerializeObject(usuario);
            mensagem.Usuario = usuario;
            mensagem.DataCriacao = DateTime.Now;

            _banco.Mensagens.Add(mensagem);
            _banco.SaveChanges();

            await Clients.Group(nomeGrupo).SendAsync("ReceberMensagem", mensagem, nomeGrupo);
        }


        private string CriarNomeGrupo(string emailUserUm, string emailUserDois)
        {
            List<string> lista = new List<string>() { emailUserUm, emailUserDois };
            var listaOrdernada = lista.OrderBy(a=>a).ToList();

            StringBuilder sb = new StringBuilder();
            foreach(var item in listaOrdernada)
            {
                sb.Append(item);
            }

            return sb.ToString();
        }
    }
}
