using MvvmHelpers;
using Plugin.Geolocator;
using ShopT.Models.LocalModels;
using ShopT.Models.Maps;
using ShopT.StaticValues;
using ShopT.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            CustomMap customMap = new CustomMap
            {
                MapType = MapType.Street,
                IsShowingUser = true,
            };

            Content = customMap;

            List<ShopLocal> shopLocals = StaticShopLocals.shopLocals;
            List<CustomPin> CustomPinList = new List<CustomPin>();

            foreach (var item in shopLocals)
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

                CustomPinList.Add(pin);

                customMap.Pins.Add(pin);
            }

            customMap.CustomPins = CustomPinList;

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10000), null, true);

            customMap.MapClicked += OnMapClicked;
            //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(200)));
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(62.0332725, 129.7309803), Distance.FromKilometers(1)));

        }

        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            DisplayAlert("", $"MapClick: {e.Position.Latitude}, {e.Position.Longitude}", "ОК");
        }
    }
}