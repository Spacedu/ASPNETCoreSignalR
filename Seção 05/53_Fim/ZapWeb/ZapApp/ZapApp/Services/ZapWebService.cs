using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(()=> {
                        App.Current.MainPage = new ListagemUsuarios();
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
        }

        public async Task Login(Usuario usuario)
        {
            await _connection.InvokeAsync("Login", usuario);
        }
    }
}
