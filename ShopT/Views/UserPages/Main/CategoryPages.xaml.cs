using ShopT.Models.LocalModels;
using ShopT.Models.Statistics;
using ShopT.ViewModels;
using ShopT.Views.UserPages.ShopsPage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPages : ContentPage
    {
        public CategoryPages()
        {
            InitializeComponent();

            NameShop.Text = ShopInfoStatis.shopInfo.OrganizationName;

            int? parentCategoryId = null;
            var categoryVM = new CategoryViewModel(parentCategoryId);

            Refreshable.BindingContext = categoryVM;
            Task.Run(() => categoryVM.GetCachedData());
        }

        private void CategoriesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Any())
            {
                var category = CategoriesCollection.SelectedItem as CategoryLocal;
                CategoriesCollection.SelectedItem = null;
                if (category.Category.IsEndpoint)
                {
                    Navigation.PushAsync(new ProductsList(category));
                }
                else
                {
                    Navigation.PushAsync(new SubCategory(category.Category.CategoryName, category.Category.CategoryId), false);
                }
            }
        }

        private void Find_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Find());
        }

        private void ShopChoice_Clicked(object sender, EventArgs e)
        {
          
            App.Current.MainPage = new ShopsShell();
        }
    }
}