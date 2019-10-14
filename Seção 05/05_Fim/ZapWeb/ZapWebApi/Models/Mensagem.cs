using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapWebApi.Models
{
    public class Mensagem
    {
        public string GrupoNome { get; set; }
        public DateTime Criado { get; set; }
        public string Texto { get; set; }
        public Usuario Usuario { get; set; }
    }
}
