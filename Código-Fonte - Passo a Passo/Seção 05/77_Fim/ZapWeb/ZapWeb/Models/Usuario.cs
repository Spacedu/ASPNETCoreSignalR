using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapWeb.Models
{
    public class Usuario
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Nome")]
        public string Nome { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Senha")]
        public string Senha { get; set; }
        [JsonProperty("IsOnline")]
        public bool IsOnline { get; set; }
        [JsonProperty("ConnectionId")]
        public string ConnectionId { get; set; } //JSON - n
    }
}
