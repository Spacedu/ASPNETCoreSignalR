using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapWeb.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool IsOnline { get; set; }
        public string ConnectionId { get; set; }
    }
}
