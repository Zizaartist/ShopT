using ShopT.StaticValues;
using ShopT.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterSMS : ContentPage
    {
        AuthViewModel registrationVM;
        public EnterSMS(AuthViewModel _registrationVM)
        {
            InitializeComponent();

            registrationVM = _registrationVM;
            BindingContext = registrationVM;
        }

        private async void Confirm_Clicked(object sender, EventArgs e)
        {
            if (registrationVM.AllFieldsValid)
            {
                if ((await registrationVM.CodeCheck()).IsSuccessStatusCode)
                {
                    if ((await registrationVM.GetToken()).IsSuccessStatusCode)
                    {
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Click", AlertMessages.UNEXPECTED_ERROR, "Понятно");
                    }
                }
                else
                {
                    await DisplayAlert("Click", AlertMessages.ERROR_CODE_IS_INVALID, "Понятно");
                }
            }
            else
            {
                await DisplayAlert("Click", AlertMessages.EMPTY_FIELDS, "Понятно");
            }
        }
    }
}