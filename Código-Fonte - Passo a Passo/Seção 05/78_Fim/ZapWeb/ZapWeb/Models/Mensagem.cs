using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZapWeb.Models
{
    public class Mensagem
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("NomeGrupo")]
        public string NomeGrupo { get; set; }
        [JsonProperty("UsuarioId")]
        public int UsuarioId { get; set; }
        [JsonProperty("UsuarioJson")]
        public string UsuarioJson { get; set; }
        [NotMapped]
        public Usuario Usuario { get; set; }
        [JsonProperty("Texto")]
        public string Texto { get; set; }
        [JsonProperty("DataCriacao")]
        public DateTime? DataCriacao { get; set; }
    }
}
