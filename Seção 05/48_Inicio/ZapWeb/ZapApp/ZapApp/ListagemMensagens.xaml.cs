using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZapApp.Models;

namespace ZapApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListagemMensagens : ContentPage
    {
        public ListagemMensagens()
        {
            InitializeComponent();
        }
    }

    public class ListagemMensagensViewModel
    {
        public List<Mensagem> Mensagens { get; set; }

        public ListagemMensagensViewModel()
        {
            Mensagens = MockMensagens();
        }

        private List<Mensagem> MockMensagens()
        {
            return new List<Mensagem>()
            {
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 1!", Usuario = new Usuario { Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 2!", Usuario = new Usuario { Nome = "Aline" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 3!", Usuario = new Usuario { Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 4!", Usuario = new Usuario { Nome = "Aline" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 5!", Usuario = new Usuario { Nome = "Elias" } },
            };
        }
    }
}