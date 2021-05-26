using ShopT.Models.Statistics;
using ShopT.StaticValues;
using ShopT.ViewModels;
using ShopT.Views.Registration;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Basket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketPage : ContentPage
    {
        BasketViewModel basketVM;
        public BasketPage()
        {
            InitializeComponent();

            basketVM = BasketViewModel.getInstance();
            BindingContext = basketVM;
            Task.Run(() => basketVM.GetData());
        }

        private async void Basket_Clicked(object sender, EventArgs e)
        {
            CacheFunctions cache = new CacheFunctions();

            string qew = await cache.tryToGet<string>($"{ShopInfoStatic.currentShopId}_{Caches.TOKENTYPE_CACHE.key}", CacheFunctions.BlobCaches.Secure);
            if (qew == "Default" || qew == default)
            {
                await Navigation.PushAsync(new EnterNumber());
            }
            else
            {
                await new ShopConfigViewModel().RefreshConfiguration(); //ожидаем обновления конфигурации
                await Navigation.PushAsync(new Buy(basketVM));
            }
        }
    }
}