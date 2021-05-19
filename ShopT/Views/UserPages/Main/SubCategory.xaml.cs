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
    public partial class SubCategory : ContentPage
    {
        public SubCategory(string CatName, int? parentCategoryId = null)
        {
            InitializeComponent();

            Points.BindingContext = UsersViewModel.Instance;

            var categoryVM = new CategoryViewModel(parentCategoryId);

            Refreshable.BindingContext = categoryVM;
            Task.Run(() => categoryVM.GetCachedData());

            HeaderLabel.Text = CatName;
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

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Bonus());
        }
    }
}