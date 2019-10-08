using Microsoft.AspNetCore.SignalR.Client;
using RealPromo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RealPromo.Services
{
    public class RealPromoSignalR
    {
        public RealPromoSignalR(ObservableCollection<Promocao> lista)
        {
            Task.Run(async()=> { await ConfigurarSignalR(lista); });
        }

        private async Task ConfigurarSignalR(ObservableCollection<Promocao> lista)
        {
            var connection = new HubConnectionBuilder().WithUrl("https://realpromoapiweb.azurewebsites.net/PromoHub").Build();

            connection.On<Promocao>("ReceberPromocao", (promocao) => {
                Xamarin.Forms.Device.InvokeOnMainThreadAsync(() => {
                    lista.Add(promocao);
                });
            });

            await connection.StartAsync();
        }
    }
}
