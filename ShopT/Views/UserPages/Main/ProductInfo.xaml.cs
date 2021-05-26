using ShopT.Models.LocalModels;
using ShopT.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfo : ContentPage
    {
        ProductLocal _item;
        BasketViewModel basketVM;
        public ProductInfo(ProductLocal _productLocal)
        {
            InitializeComponent();

            Points.BindingContext = UsersViewModel.Instance;

            basketVM = BasketViewModel.getInstance();
            Task.Run(() => basketVM.GetData());

            HeaderLabel.Text = _productLocal.Product.ProductName;
            MainStack.BindingContext = _productLocal;
            _item = _productLocal;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Bonus());
        }

        private async void AddBasket_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert(null, "Товар добавлен в корзину", "Понятно");
        }
    }
}