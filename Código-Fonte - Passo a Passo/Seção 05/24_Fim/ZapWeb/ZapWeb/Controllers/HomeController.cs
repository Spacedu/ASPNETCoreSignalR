using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }
        public ActionResult Login() {
            return View();
        }
        public ActionResult Cadastro() {
            return View();
        }
        public ActionResult Conversacao()
        {
            return View();
        }
    }
}
