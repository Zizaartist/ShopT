using MvvmHelpers;
using Plugin.Geolocator;
using ShopT.Models.LocalModels;
using ShopT.Models.Maps;
using ShopT.StaticValues;
using ShopT.ViewModels;
using ShopT.Views.Registration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.ShopsPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopsMaps : ContentPage
    {
        public ShopsMaps()
        {
            InitializeComponent();

            MapsInitAsync();
        }

        async void MapsInitAsync()
        {
            List<CustomPin> CustomPinList = new List<CustomPin>();

            foreach (var item in LocationViewModel.Instance.shopVM.Shops)
            {
                double x = double.Parse(item.Shop.ShopInfo.CoordinatesX, CultureInfo.InvariantCulture);
                double y = double.Parse(item.Shop.ShopInfo.CoordinatesY, CultureInfo.InvariantCulture);

                CustomPin pin = new CustomPin();
                pin.Type = PinType.Place;
                pin.Position = new Position(x, y);
                pin.Address = item.Shop.ShopInfo.LegalAddress;
                pin.Name = item.Shop.ShopInfo.OrganizationName;
                pin.Label = item.Shop.ShopInfo.OrganizationName;
                pin.Images = item.Shop.ShopInfo.Avatar;
                pin.ShopId = item.Shop.ShopId;

                CustomPinList.Add(pin);

                customMap.Pins.Add(pin);
            }

            customMap.CustomPins = CustomPinList;

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10000), null, true);

            customMap.MapClicked += OnMapClicked;
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(62.0332725, 129.7309803), Distance.FromKilometers(1)));

            customMap.ShopSelected += OnShopSelected;
        }

        void OnShopSelected(object sender, ShopSelectedEventArgs e) 
        {
            var selectedShop = LocationViewModel.Instance.shopVM.Shops.First(shop => shop.Shop.ShopId == e.shopId);

            ShopInfoStatic.shopInfo = selectedShop.Shop.ShopInfo;
            ShopInfoStatic.shopConfiguration = selectedShop.Shop.ShopConfiguration;
            ShopInfoStatic.currentShopId = selectedShop.Shop.ShopId;

            ApiStrings.HOST = selectedShop.Shop.ClientApiAddress + "/";
            ApiStrings.HOST_ADMIN = selectedShop.Shop.AdminApiAddress + "/";

            App.Current.MainPage = new StartPage();
        }

        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            DisplayAlert("", $"MapClick: {e.Position.Latitude}, {e.Position.Longitude}", "ОК");
        }
    }
}