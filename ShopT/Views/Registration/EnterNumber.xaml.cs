using ShopT.StaticValues;
using ShopT.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterNumber : ContentPage
    {
        AuthViewModel registrationVM = new AuthViewModel();
        public EnterNumber()
        {
            InitializeComponent();
            BindingContext = registrationVM;
        }

        private async void Confirm_Clicked(object sender, EventArgs e)
        {
            if ((await registrationVM.SmsCheck()).IsSuccessStatusCode)
            {
                await Navigation.PushAsync(new EnterSMS(registrationVM));
                var thisPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2];
                Navigation.RemovePage(thisPage);
            }
            else
            {
                await DisplayAlert("Click", AlertMessages.UNEXPECTED_ERROR, "Понятно");
            }
        }
    }
}