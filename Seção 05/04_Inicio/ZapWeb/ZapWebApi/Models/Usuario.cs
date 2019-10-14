using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapWebApi.Models
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool IsOnline { get; set; }
        public List<string> ConnectionsId { get; set; }
    }
}
