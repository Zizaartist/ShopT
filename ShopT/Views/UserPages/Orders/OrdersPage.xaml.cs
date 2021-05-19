using ShopT.Models.LocalModels;
using ShopT.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {

        private readonly OrderHistoryViewModel ordersVM;
        public OrdersPage()
        {
            InitializeComponent();
            ordersVM = new OrderHistoryViewModel();
            BindingContext = ordersVM;
            Task.Run(() => ordersVM.GetCachedData());
        }

        private void OrderCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Any())
            {
                OrderCollection.SelectedItem = null;
                Navigation.PushAsync(new OrdersDetail(e.CurrentSelection.LastOrDefault() as OrderLocal));
            }
        }
    }
}