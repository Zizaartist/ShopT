using ShopT.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserData : ContentPage
    {
        DataUserCacheViewModels dataUserVM;
        public UserData()
        {
            InitializeComponent();

            dataUserVM = new DataUserCacheViewModels();
            BindingContext = dataUserVM;
            Task.Run(() => dataUserVM.Autofill());
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            Task.Run(() => dataUserVM.SaveAutofill());
            DisplayAlert("Внимание", "Изменения сохранены", "Понятно");
        }
    }
}