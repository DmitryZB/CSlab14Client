#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CSlab13.HttpRequestData;

namespace CSlab13
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaxiDepotPage : ContentPage
    {
        public TaxiDepotPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            TaxiDepot v = (TaxiDepot)this.BindingContext;
            if (v.Id != 0)
            {
                ListTaxiGroupsInTaxiDepot.ItemsSource = GetTaxiDepotFromId(v.Id).TaxiGroups;
            }

            base.OnAppearing();
        }

        private void SaveTaxiDepot(object sender, EventArgs e)
        {
            var taxidepot = (TaxiDepot)BindingContext;
            if (!String.IsNullOrEmpty(taxidepot.Address))
            {
                if (taxidepot.Id == 0)
                    AddTaxiDepot(taxidepot);
                else
                {
                    var taxidepotUpd = GetTaxiDepotFromId(taxidepot.Id);
                    taxidepotUpd.Address = taxidepot.Address;
                    ChangeTaxiDepot(taxidepotUpd);
                }
            }

            this.Navigation.PopAsync();
        }

        private void DeleteTaxiDepot(object sender, EventArgs e)
        {
            var taxidepot = (TaxiDepot)BindingContext;
            HttpRequestData.DeleteTaxiDepot(taxidepot);
            this.Navigation.PopAsync();
        }

        private async void AddTaxiGroup(object sender, EventArgs e)
        {
            var taxidepot = (TaxiDepot)BindingContext;
            if (taxidepot.Id == 0)
            {
                AddTaxiDepot(taxidepot);
                taxidepot = GetAllTaxiDepots().Find(x => x.Address == taxidepot.Address);
            }
            
            var carName = await DisplayPromptAsync("Добавление машины в таксопарк",
                "Введите название машины",
                keyboard: Keyboard.Text);
            Console.WriteLine(carName);
            // Сделать выход после нажания отмены 
            if (carName == "" || carName == null)
                return;
            if (taxidepot.Id != 0)
            {
                var tempTaxiDepot = GetTaxiDepotFromId(taxidepot.Id);
                var partL = tempTaxiDepot.TaxiGroups.Find(x => x.Car.Name == carName);
                if (partL != null)
                {
                    await DisplayAlert("Внимание", "Машина уже есть в таксопарке", "Хорошо");
                    return;
                }
            }
            
            
            var allCars = GetAllCars();
            var car = allCars.Find(x => x.Name == carName);
            if (car == null)
            {
                bool flag = await DisplayAlert(
                    "Ошибка",
                    "Похоже, нет такой машины(\nХотите создать?",
                    "Создать",
                    "Отмена");
                // Если хотим то можно сразу создать ее
                if (flag)
                {
                    CarPage carPage = new CarPage();
                    Car carNew = new Car { Name = carName };
                    carPage.BindingContext = carNew;
                    await Navigation.PushAsync(carPage);
                    return;
                }
                else
                {
                    return;
                }
            }

            string quantity = await DisplayPromptAsync("Добавление машины в таксопарк",
                $"Введите небходимое в таксопарке количество \"{carName}\"",
                keyboard: Keyboard.Numeric);
            if (quantity == "0" || quantity == "" || !int.TryParse(quantity, out var numericValue))
                return;
            TaxiGroup temp = new TaxiGroup
            {
                Quantity = Int32.Parse(quantity),
                Car = car,
                CarId = car.Id,
                TaxiDepot = taxidepot,
                TaxiDepotId = taxidepot.Id
            };
            taxidepot.TaxiGroups.Add(temp);
            ListTaxiGroupsInTaxiDepot.ItemsSource = taxidepot.TaxiGroups;
            ChangeTaxiDepot(taxidepot);
            OnAppearing();
            await DisplayAlert("Внимание", "Машина добавлена", "Хорошо");
        }

        private async void EditTaxiGroup(object sender, EventArgs e)
        {
            var taxiDepot = (TaxiDepot)BindingContext;
            var carName = ((MenuItem)sender).CommandParameter.ToString();
            string quantityNew = await DisplayPromptAsync("Редактирование таксопарка",
                $"Введите новое количество \"{carName}\"",
                keyboard: Keyboard.Numeric);
            if (quantityNew == "0" || quantityNew == "" || !int.TryParse(quantityNew, out var numericValue))
                return;
            var taxidepotUpd = GetTaxiDepotFromId(taxiDepot.Id);
            taxidepotUpd.TaxiGroups.Find(x => x.Car.Name == carName).Quantity = int.Parse(quantityNew);
            ChangeTaxiDepot(taxidepotUpd);
            OnAppearing();
        }

        private void DeleteTaxiGroup(object sender, EventArgs e)
        {
            var taxiDepot = (TaxiDepot)BindingContext;
            var taxidepotUpd = GetTaxiDepotFromId(taxiDepot.Id);
            var carName = ((MenuItem)sender).CommandParameter.ToString();
            taxidepotUpd.TaxiGroups.RemoveAll(x => x.Car.Name == carName);
            ChangeTaxiDepot(taxidepotUpd);
            OnAppearing();
        }
    }
}