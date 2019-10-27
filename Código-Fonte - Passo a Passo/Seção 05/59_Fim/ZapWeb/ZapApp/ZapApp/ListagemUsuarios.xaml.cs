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
    public partial class ListagemUsuarios : ContentPage
    {
        public ListagemUsuarios()
        {
            InitializeComponent();

            Sair.Clicked += async (sender, args) => {
                //SignalR
                await ZapWebService.GetInstance().Sair(UsuarioManager.GetUsuarioLogado());
                
                //App
                UsuarioManager.DelUsuarioLogado();

                App.Current.MainPage = new Inicio();                
            };
        }
    }


    public class ListagemUsuariosViewModel : INotifyPropertyChanged
    {
        private List<Usuario> _usuarios;
        public List<Usuario> Usuarios { 
            get {
                return _usuarios;
            } 
            set {
                _usuarios = value;
                NotifyPropertyChanged(nameof(Usuarios));
            }
        }

        public ListagemUsuariosViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}