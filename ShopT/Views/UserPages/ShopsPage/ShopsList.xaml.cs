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

        private void LocationPickerButton_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("locations");
        }
    }
}