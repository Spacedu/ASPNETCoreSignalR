using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapWebApi.Models
{
    public class Grupo
    {
        public string Nome { get; set; }

        public List<Usuario> Usuarios { get; set; }
    }
}
