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
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 1!", Usuario = new Usuario { Id = 1, Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 2!", Usuario = new Usuario { Id = 2, Nome = "Aline" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 3!", Usuario = new Usuario { Id = 1, Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 4!", Usuario = new Usuario { Id = 2, Nome = "Aline" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 5!", Usuario = new Usuario { Id = 1, Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 5!", Usuario = new Usuario { Id = 1, Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 5!", Usuario = new Usuario { Id = 1, Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 5!", Usuario = new Usuario { Id = 1, Nome = "Elias" } },
                new Mensagem { NomeGrupo = "1", Texto = "Olá mundo 5!", Usuario = new Usuario { Id = 1, Nome = "Elias" } },
            };
        }
    }


    public class MensagemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EsquerdaTemplate { get; set; }
        public DataTemplate DireitaTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //TODO - Obter Usuário Logado depois.
            Usuario usuarioLogado = new Usuario() { Id = 1, Nome = "Elias" };

            return ((Mensagem)item).Usuario.Id == usuarioLogado.Id ? DireitaTemplate : EsquerdaTemplate;
        }
    }
}