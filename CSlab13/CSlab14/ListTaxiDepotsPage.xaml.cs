using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static CSlab13.HttpRequestData;

namespace CSlab13
{
    public partial class ListTaxiDepotsPage : ContentPage
    {
        public ListTaxiDepotsPage()
        {
            InitializeComponent();
        }

        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            TaxiDepotList.ItemsSource = GetAllTaxiDepots();
            base.OnAppearing();
        }

        // Обработка кнопки добавления сборки
        private async void CreateTaxiDepot(object sender, EventArgs e)
        {
            TaxiDepot taxiDepot = new TaxiDepot();
            TaxiDepotPage taxidepotPage = new TaxiDepotPage();
            taxidepotPage.BindingContext = taxiDepot;
            await Navigation.PushAsync(taxidepotPage);
        }

        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            TaxiDepot selectedtaxidepot = (TaxiDepot)e.SelectedItem;
            TaxiDepotPage taxiDepotPage = new TaxiDepotPage();
            taxiDepotPage.BindingContext = selectedtaxidepot;
            await Navigation.PushAsync(taxiDepotPage);
        }
    }
}