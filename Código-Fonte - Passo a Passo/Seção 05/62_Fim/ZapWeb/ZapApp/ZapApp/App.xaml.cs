using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZapApp.Services;

namespace ZapApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Inicio();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Task.Run(async()=> { await ZapWebService.GetInstance().Sair(UsuarioManager.GetUsuarioLogado()); });
        }

        
        protected override void OnResume()
        {
            Task.Run(async () => { await ZapWebService.GetInstance().Entrar(UsuarioManager.GetUsuarioLogado()); });
        }
    }
}
