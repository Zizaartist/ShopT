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
    public partial class Find : ContentPage
    {
        bool ProsSelChan;
        BasketViewModel basketVM;
        FindProductsViewModel findProductVM;
        public Find()
        {
            InitializeComponent();

            basketVM = BasketViewModel.getInstance();
            Task.Run(() => basketVM.GetData());

            findProductVM = new FindProductsViewModel(basketVM.AddToBasket);
            BindingContext = findProductVM;
            Task.Run(() => findProductVM.GetRemoteData(""));

            ProsSelChan = false;

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
            DisplayAlert("Внимание", "Товар добавлен в корзину", "Понятно");
        }

        private async void Search_Clicked(object sender, EventArgs e)
        {
            string SearchText = FindEntry.Text;
            await findProductVM.GetRemoteData(SearchText);

            ProsSelChan = false;
            FilterPicker.SelectedIndex = -1;
            Sorting.SelectedIndex = -1;

            FilterPicker.Items.Clear();
            Sorting.Items.Clear();

            ProsSelChan = true;
            foreach (var item in findProductVM.Filters)
            {
                FilterPicker.Items.Add(item);
            }

            Sorting.Items.Add("По возрастанию цены");
            Sorting.Items.Add("По убыванию цены");
           
        }

        private void Sorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProsSelChan)
            {
                var qwed = Sorting.Items[Sorting.SelectedIndex];
                findProductVM.SortFilters(qwed);
                BindingContext = findProductVM;

                ProsSelChan = false;
                FilterPicker.SelectedIndex = -1;
                FilterPicker.Items.Clear();
                ProsSelChan = true;
                foreach (var item in findProductVM.Filters)
                {
                    FilterPicker.Items.Add(item);
                }
            }

        }

        private void FilterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProsSelChan)
            {
                var qwed = FilterPicker.Items[FilterPicker.SelectedIndex];
                findProductVM.FilterPickerFilters(qwed);
                
            }
        }
    }
}