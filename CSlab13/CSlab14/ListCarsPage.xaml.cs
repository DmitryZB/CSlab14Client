using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CSlab13.HttpRequestData;

namespace CSlab13
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCarsPage : ContentPage
    {
        public ListCarsPage()
        {
            InitializeComponent();
        }
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            CarList.ItemsSource = GetAllCars();
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreateCar(object sender, EventArgs e)
        {
            Car car = new Car();
            CarPage carPage = new CarPage();
            carPage.BindingContext = car;
            await Navigation.PushAsync(carPage);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Car selectedCar = (Car)e.SelectedItem;
            var car = GetCarFromId(selectedCar.Id);
            CarPage carPage = new CarPage();
            carPage.BindingContext = car;
            await Navigation.PushAsync(carPage);
        }
    }
}