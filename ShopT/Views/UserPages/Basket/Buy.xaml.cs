using ShopT.Models.EnumModels;
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
        private Dictionary<PaymentMethod, Button> PaymentMethodButtons;
        public Buy(BasketViewModel _basketVM)
        {
            InitializeComponent();

            PaymentMethodButtons = new Dictionary<PaymentMethod, Button>()
            {
                { PaymentMethod.cash, Cash },
                { PaymentMethod.card, Card },
                { PaymentMethod.online, CardOnline }
            };

            basketVM = _basketVM;

            orderVM = new OrderViewModel(_basketVM.OrderDetails, true);
            BindingContext = orderVM;
            Task.Run(() => orderVM.Autofill());
        }


        private void ChangePaymentMethod_Clicked(object sender, EventArgs e)
        {
            App app = new App();

            var selectedPM = PaymentMethodButtons.First(x => x.Value.Equals(sender));

            var initialPM = PaymentMethodButtons[orderVM.PaymentMethod];

            selectedPM.Value.Style = app.paymentSelected;
            initialPM.Style = app.payment;

            orderVM.PaymentMethod = selectedPM.Key;
        }



        //private void CardOnline_Clicked(object sender, EventArgs e)
        //{
        //    App app = new App();
        //    ClearMethodPayment();
        //    CardOnline.Style = app.paymentSelected;
        //}

        //private void Card_Clicked(object sender, EventArgs e)
        //{
        //    App app = new App();
        //    ClearMethodPayment();
        //    Card.Style = app.paymentSelected;
        //}

        //private void Cash_Clicked(object sender, EventArgs e)
        //{
        //    App app = new App();
        //    ClearMethodPayment();
        //    DeliveryOfMoney.IsVisible = true;
        //    Cash.Style = app.paymentSelected;
        //}
        //void ClearMethodPayment()
        //{
        //    App app = new App();
        //    Cash.Style = app.payment;
        //    CardOnline.Style = app.payment;
        //    Card.Style = app.payment;
        //    DeliveryOfMoney.IsVisible = false;
        //}

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