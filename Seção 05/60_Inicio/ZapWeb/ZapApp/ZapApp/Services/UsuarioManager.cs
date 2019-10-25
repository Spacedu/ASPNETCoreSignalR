using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ZapApp.Models;

namespace ZapApp.Services
{
    class UsuarioManager
    {
        public static void SetUsuarioLogado(Usuario usuario)
        {
            App.Current.Properties["Logado"] = JsonConvert.SerializeObject(usuario);
            App.Current.SavePropertiesAsync();
        }
        public static Usuario GetUsuarioLogado()
        {
            if(App.Current.Properties.ContainsKey("Logado"))
            {
                var logado = (string)App.Current.Properties["Logado"];
                return JsonConvert.DeserializeObject<Usuario>(logado);
            }

            return null;
        }

        public static void DelUsuarioLogado()
        {
            App.Current.Properties.Remove("Logado");
            App.Current.SavePropertiesAsync();
        }
    }
}
