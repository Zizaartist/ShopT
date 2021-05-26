
using ShopT.Models.LocalModels;
using ShopT.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersDetail : ContentPage
    {
        public OrdersDetail(OrderLocal order)
        {
            InitializeComponent();

            var detailsVM = new OrderDetailsViewModel();
            BindingContext = detailsVM;
            Task.Run(() => detailsVM.GetRemoteData(order.Order.OrderId));

            SpanSum.Text = order.Order.Sum.ToString();
        }

    }
}