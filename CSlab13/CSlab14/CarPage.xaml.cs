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
    public partial class CarPage : ContentPage
    {
        public CarPage()
        {
            InitializeComponent();
        }

        private void SaveCar(object sender, EventArgs e)
        {
            var car = (Car)BindingContext;
            if (!String.IsNullOrEmpty(car.Name))
            {
                if (car.Id == 0)
                    AddCar(car);
                else

                    ChangeCar(car);
            }

            this.Navigation.PopAsync();
        }

        private void DeleteCar(object sender, EventArgs e)
        {
            var car = (Car)BindingContext;
            HttpRequestData.DeleteCar(car);

            this.Navigation.PopAsync();
        }
    }
}