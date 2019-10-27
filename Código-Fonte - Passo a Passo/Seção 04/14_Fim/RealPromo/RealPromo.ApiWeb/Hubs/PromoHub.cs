using Microsoft.AspNetCore.SignalR;
using RealPromo.ApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealPromo.ApiWeb.Hubs
{
    //RPC
    public class PromoHub : Hub
    {
        /*
         * Cliente - JS/C#/Java
         * RPC
         * - Cliente(JS) -> Hub(C#) (C# - CadastrarPromoca)
         * - Hub(C#) -> Cliente(JS) (JS - ReceberPromocao)
         */
        public async Task CadastrarPromocao(Promocao promocao)
        {
            /*
             * Banco
             * Queue/Scheduler........
             * Notificar o usuário (SignalR).
             */

            await Clients.Caller.SendAsync("CadastradoSucesso");
            await Clients.Others.SendAsync("ReceberPromocao", promocao);
        }
    }
}
