using ShopT.Models.LocalModels;
using ShopT.Models.Statistics;
using ShopT.StaticValues;
using ShopT.ViewModels;
using ShopT.Views.Registration;
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
    public partial class ShopsList : ContentPage
    {
        public ShopsList()
        {
            InitializeComponent();

            BindingContext = LocationViewModel.Instance;
            Task.Run(() => LocationViewModel.Instance.GetCachedData());
        }

        private void ShopList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Any())
            {
                var _shopLocal = ShopList.SelectedItem as ShopLocal;
                ShopList.SelectedItem = null;


                ShopInfoStatic.shopInfo = _shopLocal.Shop.ShopInfo;
                ShopInfoStatic.shopConfiguration = _shopLocal.Shop.ShopConfiguration;
                ShopInfoStatic.currentShopId = _shopLocal.Shop.ShopId;

                ApiStrings.HOST = _shopLocal.Shop.ClientApiAddress +"/";
                ApiStrings.HOST_ADMIN = _shopLocal.Shop.AdminApiAddress +"/";

                App.Current.MainPage = new StartPage();
            }
        }

        private async void LocationPickerButton_Clicked(object sender, EventArgs e)
        {
            var selection = await DisplayActionSheet("Выберите новую локацию", null, null, LocationViewModel.Instance.Locations.Select(loc => loc.LocationName).ToArray());
            if (selection == null) return;

            LocationViewModel.Instance.SelectedLocation = LocationViewModel.Instance.Locations.FirstOrDefault(loc => loc.LocationName == selection);
        }
    }
}