using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZapApp.Models;

namespace ZapApp.Services
{
    public class ZapWebService
    {
        private static HubConnection _connection;
        private static ZapWebService _instance;

        public static ZapWebService GetInstance()
        {
            if(_connection == null)
            {
                _connection = new HubConnectionBuilder().WithUrl("https://zapwebapiweb.azurewebsites.net/ZapWebHub").Build();
            }
            if(_connection.State == HubConnectionState.Disconnected)
            {
                _connection.StartAsync();
            }
            _connection.Closed += async (error) => {
                await Task.Delay(5000);
                await _connection.StartAsync();
            };

            if(_instance == null)
            {
                _instance = new ZapWebService();
            }
            return _instance;
        }


        private ZapWebService()
        {
            _connection.On<bool, Usuario, string>("ReceberLogin", (sucesso, usuario, msg)=> {
                if (sucesso)
                {
                    UsuarioManager.SetUsuarioLogado(usuario);

                    Task.Run(async()=> { await Entrar(usuario); });

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(()=> {
                        App.Current.MainPage = new NavigationPage( new ListagemUsuarios() );
                    });
                }
                else
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                        var inicioPage = ((Inicio)App.Current.MainPage);
                        var loginPage = ((Login)inicioPage.Children[0]);
                        loginPage.SetMensagem(msg);
                    });
                }
            });

            _connection.On<bool, Usuario, string>("ReceberCadastro", (sucesso, usuario, msg) => {
                if (sucesso)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                        var inicioPage = ((Inicio)App.Current.MainPage);
                        var loginPage = ((Cadastro)inicioPage.Children[1]);
                        loginPage.SetMensagem(msg, false);
                    });
                }
                else
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                        var inicioPage = ((Inicio)App.Current.MainPage);
                        var loginPage = ((Cadastro)inicioPage.Children[1]);
                        loginPage.SetMensagem(msg, true);
                    });
                }
            });

            _connection.On<List<Usuario>>("ReceberListaUsuarios", (usuarios) => {
                if (App.Current.MainPage.GetType() == typeof(NavigationPage) && ((NavigationPage)App.Current.MainPage).CurrentPage.GetType() == typeof(ListagemUsuarios))
                {
                    var usuarioLogado = usuarios.Find(a => a.Id == UsuarioManager.GetUsuarioLogado().Id);
                    usuarios.Remove(usuarioLogado);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                        var navigationPage = ((NavigationPage) App.Current.MainPage);
                        var listagemUsuarios = (ListagemUsuarios) navigationPage.CurrentPage;
                        ((ListagemUsuariosViewModel)listagemUsuarios.BindingContext).Usuarios = usuarios;
                    });                    
                }
            });

            _connection.On<string, List<Mensagem>>("AbrirGrupo", (nomeGrupo, mensagens) => {
                if (App.Current.MainPage.GetType() == typeof(NavigationPage) && ((NavigationPage)App.Current.MainPage).CurrentPage.GetType() == typeof(ListagemMensagens))
                {
                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    var listagemUsuarios = (ListagemMensagens)navigationPage.CurrentPage;
                    listagemUsuarios.SetNomeGrupo(nomeGrupo);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        var listagemViewModel = (ListagemMensagensViewModel)listagemUsuarios.BindingContext;
                        listagemViewModel.Mensagens = mensagens;
                    });
                }
            });
        }

        public async Task Login(Usuario usuario)
        {
            await _connection.InvokeAsync("Login", usuario);
        }


        public async Task Cadastrar(Usuario usuario)
        {
            await _connection.InvokeAsync("Cadastrar", usuario);
        }

        public async Task Sair(Usuario usuario)
        {
            await _connection.InvokeAsync("DelConnectionIdDoUsuario", usuario);
        }
        public async Task Entrar(Usuario usuario)
        {
            await _connection.InvokeAsync("AddConnectionIdDoUsuario", usuario);
        }

        public async Task ObterListaUsuarios()
        {
            await _connection.InvokeAsync("ObterListaUsuarios");
        }

        public async Task CriarOuAbrirGrupo(string emailUm, string emailDois)
        {
            await _connection.InvokeAsync("CriarOuAbrirGrupo", emailUm, emailDois);
        }
    }
}
