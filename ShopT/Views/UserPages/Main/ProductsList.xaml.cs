using ShopT.Models.LocalModels;
using ShopT.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsList : ContentPage
    {
        BasketViewModel basketVM;
        public ProductsList(CategoryLocal _categoryLocal)
        {
            InitializeComponent();

            Points.BindingContext = UsersViewModel.Instance;

            basketVM = BasketViewModel.getInstance();
            Task.Run(() => basketVM.GetData());

            var productVM = new ProductsViewModel(_categoryLocal.Category, basketVM.AddToBasket);
            BindingContext = productVM;
            Task.Run(() => productVM.GetCachedData());
            
            HeaderLabel.Text = _categoryLocal.Category.CategoryName;
        }

        private void ItemsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Any())
            {
                ItemsCollection.SelectedItem = null;
                Navigation.PushAsync(new ProductInfo(e.CurrentSelection.LastOrDefault() as ProductLocal));
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Bonus());
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            DisplayAlert("Внимание", "Товар добавлен в корзину", "Понятно");
        }
    }
}