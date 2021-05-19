
using ShopT.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bonus : ContentPage
    {
        public Bonus()
        {
            InitializeComponent();

            Points.BindingContext = UsersViewModel.Instance;

            var pointRegisterVM = new PointRegisterViewModel();
            Refreshable.BindingContext = pointRegisterVM;
            Task.Run(async () => await pointRegisterVM.GetInitialData.ExecuteAsync());
        }
    }
}