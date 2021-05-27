using ShopT.ViewModels.Temp;
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
    public partial class FindCity : ContentPage
    {
        public FindCity()
        {
            InitializeComponent();
            LocationCollection.BindingContext = new LocationViewModel();
        }

        private void LocationCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search_Clicked(object sender, EventArgs e)
        {

        }

        private void Next_Clicked(object sender, EventArgs e)
        {
            if(LocationCollection.SelectedItem == null)
            {
                DisplayAlert("Внимание", "Пожалуйста, выберите локацию", "Хорошо");
            }
            else
            {
               App.Current.MainPage = new ShopsShell();
            }
        }
    }
}