using Microsoft.AspNetCore.SignalR;
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
         * Cliente -> Hub
         * Hub -> Cliente
         */
        public async Task CadastrarPromocao(Promocao promocao)
        {
            /*
             * Banco
             * Queue/Scheduler........
             * Notificar o usuário (SignalR).
             */
        }
    }
}
