using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZapWebApi.Database;
using ZapWebApi.Models;

namespace ZapWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        public ActionResult Conversacao()
        {
            Banco.Setup();
            return View();
        }
    }
}
