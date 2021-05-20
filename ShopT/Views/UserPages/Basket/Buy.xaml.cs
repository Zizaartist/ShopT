using ShopT.Models.EnumModels;
using ShopT.Models.LocalModels;
using ShopT.StaticValues;
using ShopT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Basket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Buy : ContentPage
    {
        private OrderViewModel orderVM;
        private BasketViewModel basketVM;

        public Buy(BasketViewModel _basketVM)
        {
            InitializeComponent();

            basketVM = _basketVM;

            orderVM = new OrderViewModel(_basketVM.OrderDetails, true);

            if (!orderVM.PaymentMethods.ContainsKey(PaymentMethod.cash)) Cash.BindingContext = null;
            else Cash.BindingContext = orderVM.PaymentMethods[PaymentMethod.cash];

            if (!orderVM.PaymentMethods.ContainsKey(PaymentMethod.card)) Card.BindingContext = null;
            else Card.BindingContext = orderVM.PaymentMethods[PaymentMethod.card];

            if (!orderVM.PaymentMethods.ContainsKey(PaymentMethod.online)) CardOnline.BindingContext = null;
            else CardOnline.BindingContext = orderVM.PaymentMethods[PaymentMethod.online];

            var firstAvailablePM = orderVM.PaymentMethods.First();
            firstAvailablePM.Value.Toggle.Execute(null); //Выбираем первый доступный метод по умолчанию

            BindingContext = orderVM;

            Task.Run(() => orderVM.Autofill());
        }

        private async void Confirm_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Внимание", "Вы действительно хотите осуществить заказ?", "Да", "Нет");
            if (result)
            {
                var response = await orderVM.PostOrder();
                if (response.IsSuccessStatusCode)
                {
                    await basketVM.Clear();
                    await DisplayAlert("Внимание", "Заказ осуществлен, ожидайте", "Понятно");
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await DisplayAlert("Ошибка", AlertMessages.UNEXPECTED_ERROR, "Понятно");
                }
            }
        }
    }
}