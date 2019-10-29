using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapWeb.Models
{
    public class Grupo
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Nome")]
        public string Nome { get; set; }
        [JsonProperty("Usuarios")]
        public string Usuarios { get; set; }
    }
}
