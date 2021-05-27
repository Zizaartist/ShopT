using Akavache;
using ShopT.Models.LocalModels;
using ShopT.Models.Statistics;
using ShopT.StaticValues;
using ShopT.ViewModels;
using ShopT.Views.Registration;
using ShopT.Views.UserPages.ShopsPage;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShopT
{
    public partial class App : Application
    {
        public Style payment, paymentSelected;
        public bool subcategory = true, Sale = true;

        private Task<int> GetLastLocationId;

        public App()
        {
            InitializeComponent();
            InitializationPublicStyles();

            BlobCache.ApplicationName = "ShopT";
            BlobCache.EnsureInitialized();

            //Стартуем заранее
            GetLastLocationId = new CacheFunctions().tryToGet<int>($"{Caches.LOCATION_SELECTED.key}", CacheFunctions.BlobCaches.UserAccount);

            MainPage = new ShopsShell();
        }

        void InitializationPublicStyles()
        {
            payment = Payment;
            paymentSelected = PaymentSelected;
        }

        protected override async void OnStart()
        {
            var lastLocationId = await GetLastLocationId;
            if (lastLocationId == default)
            {
                await Shell.Current.GoToAsync("///shops/locations");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
