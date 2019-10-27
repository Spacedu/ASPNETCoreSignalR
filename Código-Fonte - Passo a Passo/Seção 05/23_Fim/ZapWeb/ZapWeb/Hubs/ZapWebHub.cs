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

                
                await Clients.All.SendAsync("ReceberListaUsuarios", _banco.Usuarios.ToList());
            }
        }

        public async Task Logout(Usuario usuario)
        {
            var usuarioDB = _banco.Usuarios.Find(usuario.Id);
            usuarioDB.IsOnline = false;
            _banco.Usuarios.Update(usuarioDB);
            _banco.SaveChanges();
            
            await DelConnectionIdDoUsuario(usuarioDB);

            await Clients.All.SendAsync("ReceberListaUsuarios", _banco.Usuarios.ToList());

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

            usuarioDB.ConnectionId = JsonConvert.SerializeObject(connectionsId);
            _banco.Usuarios.Update(usuarioDB);
            _banco.SaveChanges();

            //TODO - Adicionado aos Grupos de conversa desses usuários.
        }

        public async Task DelConnectionIdDoUsuario(Usuario usuario)
        {
            Usuario usuarioDB = _banco.Usuarios.Find(usuario.Id);
            if (usuarioDB.ConnectionId.Length > 0)
            {
                var ConnectionIdCurrent = Context.ConnectionId;

                List<string> connectionsId = JsonConvert.DeserializeObject<List<string>>(usuarioDB.ConnectionId);
                if (connectionsId.Contains(ConnectionIdCurrent))
                {
                    connectionsId.Remove(ConnectionIdCurrent);
                }
                usuarioDB.ConnectionId = JsonConvert.SerializeObject(connectionsId);


                _banco.Usuarios.Update(usuarioDB);
                _banco.SaveChanges();
            }

            //TODO - Remover ConnectionId dos Grupos de conversa desse usuário.
        }

        public async Task ObterListaUsuarios()
        {
            var usuarios = _banco.Usuarios.ToList();
            await Clients.Caller.SendAsync("ReceberListaUsuarios", usuarios);
        }

        /*
         SignalR - 
         elias@gmail.com - aline@gmail.com = elias@gmail.com-aline@gmail.com
         aline@gmail.com - elias@gmail.com = aline@gmail.com-elias@gmail.com 

        */
        public async Task CriarOuAbrirGrupo(string emailUserUm, string emailUserDois)
        {
            string nomeGrupo = CriarNomeGrupo(emailUserUm, emailUserDois);

            //TODO - Verificar se o Grupo já existe no Banco, se não existir, criar grupo.
            Grupo grupo = _banco.Grupos.FirstOrDefault(a => a.Nome == nomeGrupo);
            if(grupo == null)
            {
                Usuario usuarioUm = _banco.Usuarios.First(a => a.Email == emailUserUm);
                Usuario usuarioDois= _banco.Usuarios.First(a => a.Email == emailUserDois);

                grupo = new Grupo();
                grupo.Nome = nomeGrupo;
                grupo.Usuarios = JsonConvert.SerializeObject(new List<Usuario>()
                {
                    usuarioUm,
                    usuarioDois
                });

                _banco.Grupos.Add(grupo);
                _banco.SaveChanges();
            }

            //TODO - SignalR - ConnectionsID

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
