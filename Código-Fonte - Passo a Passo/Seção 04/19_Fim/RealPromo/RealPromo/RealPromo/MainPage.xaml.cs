using RealPromo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealPromo
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ListViewPromocao.ItemsSource = GetPromocoes();
        }

        private List<Promocao> GetPromocoes()
        {
            List<Promocao> lista = new List<Promocao>();

            lista.Add(new Promocao { Empresa = "Carrefour", Chamada = "TVs em promoções", Regras = "10 unidades", EnderecoURL = "https://www.carrefour.com.br" });
            lista.Add(new Promocao { Empresa = "Carrefour", Chamada = "Notebooks em promoções", Regras = "20 unidades", EnderecoURL = "https://www.carrefour.com.br" });

            return lista;
        }
    }
}
