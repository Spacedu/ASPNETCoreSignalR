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


    public class ListagemUsuariosViewModel
    {
        public List<Usuario> Usuarios { get; set; }

        public ListagemUsuariosViewModel()
        {
            Usuarios = MockUsuarios();
        }

        private List<Usuario> MockUsuarios()
        {
            return new List<Usuario>()
            {
                new Usuario { Nome = "Elias Ribeiro", Email = "elias.ribeiro.s@gmail.com", Senha = "123456", IsOnline = false },
                new Usuario { Nome = "Aline Souza", Email = "aline123@gmail.com", Senha = "123456", IsOnline = false },
                new Usuario { Nome = "João Vitor", Email = "vitor123@gmail.com", Senha = "123456", IsOnline = false },
                new Usuario { Nome = "Maria Gomes", Email = "maria123@gmail.com", Senha = "123456", IsOnline = false },
            };
        }
    }
}