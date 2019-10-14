using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapWebApi.Models;

namespace ZapWebApi.Database
{
    public class Banco
    {
        public static void Setup()
        {
            //Grupos
            Banco.Grupos.Add(
                new Grupo() { Nome = Usuarios[1].Email + Usuarios[0].Email, Usuarios = new List<Usuario>() { Usuarios[0], Usuarios[1] } }
            );
            Banco.Mensagens.Add(new Mensagem { GrupoNome = Usuarios[1].Email + Usuarios[0].Email, Criado = DateTime.Now, Texto = "Olá Aline, tudo bem?", Usuario = Usuarios[0] });
            
        }
        public static List<Usuario> Usuarios = new List<Usuario>() { 
            new Usuario(){ Id = Guid.NewGuid(), Nome = "Elias Ribeiro", Email = "elias.ribeiro.s@gmail.com", Senha = "123456", IsOnline = false, ConnectionsId = new List<string>() },
            new Usuario(){ Id = Guid.NewGuid(), Nome = "Aline", Email = "aline@gmail.com", Senha = "123456", IsOnline = false, ConnectionsId = new List<string>() },
            new Usuario(){ Id = Guid.NewGuid(), Nome = "José", Email = "jose@gmail.com", Senha = "123456", IsOnline = false, ConnectionsId = new List<string>() }
        };
        public static List<Grupo> Grupos = new List<Grupo>();
        public static List<Mensagem> Mensagens = new List<Mensagem>();
    }
}
