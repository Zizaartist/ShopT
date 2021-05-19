using ShopT.ViewModels;
using ShopT.Views.UserPages.Main;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();

            Points.BindingContext = UsersViewModel.Instance;
        }

        private void UserData_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserData());
        }

        private void OrganisationData_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrganisationData());
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Bonus());
        }
    }
}