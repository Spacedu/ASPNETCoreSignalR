using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZapApp.Models;
using ZapApp.Services;

namespace ZapApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListagemMensagens : ContentPage
    {
        private string _nomeGrupo { get; set; }

        private Usuario _usuario { get; set; }
        public ListagemMensagens()
        {
            InitializeComponent();
        }

        public void SetUsuario(Usuario usuario)
        {
            _usuario = usuario;

            var emailUm = UsuarioManager.GetUsuarioLogado().Email;
            var emailDois = usuario.Email;

            Task.Run(async()=> { await ZapWebService.GetInstance().CriarOuAbrirGrupo(emailUm, emailDois); });
        }
        public void SetNomeGrupo(string nomeGrupo)
        {
            _nomeGrupo = nomeGrupo;
        }
    }

    public class ListagemMensagensViewModel : INotifyPropertyChanged
    {
        private List<Mensagem> _mensagens;
        public List<Mensagem> Mensagens
        {
            get
            {
                return _mensagens;
            }
            set
            {
                _mensagens = value;
                NotifyPropertyChanged(nameof(Mensagens));
            }
        }

        public ListagemMensagensViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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