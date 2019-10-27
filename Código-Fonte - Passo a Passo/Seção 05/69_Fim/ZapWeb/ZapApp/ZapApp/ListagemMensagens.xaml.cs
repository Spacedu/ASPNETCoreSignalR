using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            Enviar.Clicked += async (sender, args) => {
                var mensagem = Mensagem.Text.Trim();

                if(mensagem.Length > 0)
                {
                    //TODO - Enviar Mensagem com SignalR.
                }
                else
                {
                    await DisplayAlert("Erro no preenchimento!", "Preencha o campo mensagem!", "OK");
                }

            };
        }

        public void SetUsuario(Usuario usuario)
        {
            _usuario = usuario;
            Title = usuario.Nome.FirstCharWordsToUpper();

            var emailUm = UsuarioManager.GetUsuarioLogado().Email;
            var emailDois = usuario.Email;

            Task.Run(async()=> { await ZapWebService.GetInstance().CriarOuAbrirGrupo(emailUm, emailDois); });
        }
        public void SetNomeGrupo(string nomeGrupo)
        {
            _nomeGrupo = nomeGrupo;
        }

        public string GetNomeGrupo()
        {
            return _nomeGrupo;
        }
    }

    public class ListagemMensagensViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Mensagem> _mensagens;
        public ObservableCollection<Mensagem> Mensagens
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