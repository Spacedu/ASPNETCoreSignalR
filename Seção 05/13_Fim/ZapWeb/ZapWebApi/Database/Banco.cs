using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapWebApi.Models;

namespace ZapWebApi.Database
{
    public class Banco
    {
        public static List<Usuario> Usuarios = new List<Usuario>() { 
            new Usuario(){ Id = Guid.NewGuid(), Nome = "Elias Ribeiro", Email = "elias.ribeiro.s@gmail.com", Senha = "123456", IsOnline = false, ConnectionsId = new List<string>() }
        };
        public static List<Grupo> Grupos = new List<Grupo>();
        public static List<Mensagem> Mensagens = new List<Mensagem>();
    }
}
