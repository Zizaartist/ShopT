using MvvmHelpers;
using ShopT.Models.HubModels;
using ShopT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.ShopsPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindCity : ContentPage
    {
        public FindCity()
        {
            InitializeComponent();

            BindingContext = LocationViewModel.Instance;
        }

        private void Search_Clicked(object sender, EventArgs e)
        {

        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            if(LocationCollection.SelectedItem == null)
            {
                await DisplayAlert("Внимание", "Пожалуйста, выберите локацию", "Хорошо");
            }
            else
            {
                await LocationViewModel.Instance.LocationSelected();
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}